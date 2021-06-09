using System;

namespace Beetle
{
    class BeetleAlignment : BeetleSearch
    {
        public BeetleAlignment(Parameters prmts, BeetleControl bc, PiezoControl pc) : base(prmts, bc, pc)
        {}

        //parameters.errorflag will be used as program stop flag as well.Intance like meet criteria, unexpected high loss, failed
        //      to find better loss and motor errors will errect this flag.


        /* Based on Loss, we divide it into three phases
         * 1. -60 ~ lossStage1: use ScanSearch method
         * 2. lossStage1 ~ lossStage2: use InterpolationSerach method, change ZSteppingSearch parameters 
         * 3. lossStage2 ~ lossCriteria: use InterpolationSearch method, change ZSteppingSearch parameters 
        */
        protected float lossStage1 = -4.0f;
        protected float lossStage2 = -2.0f;

        private string searchMode = "scan"; // defaul scan mode

        public double SetLossCriteria
        {
            get { return lossCriteria; }
            set { parameters.lossCriteria = value; }
        }

        public void AlignmentRun() => Run(criteriaSelect: "global", backDistanceAfterSearching: 0);

        public void PreCuringRun() => Run(criteriaSelect: "currentMax", backDistanceAfterSearching: 0, runFromContact: false, useScanMode: false);

        public void Test() => XYSquareSearch();

        // Start search from the current position, and stopped at the best position
        // criteria select: 
        //      "global": use parameters.lossCriteria as criteria
        //      "currentMax": use lossCurentMax as criteria
        // runFromContact: start searching from the current position where ferrule and lens cap are very close to each other
        // backDistanceAfterSearching: means the distance to go back after searching, this is for another search after applying epoxy
        // useScanMode: in XYSearch, whether to use ScanSearch. This can be achieved by changing the lossStage1 value to a larger one
        // gapNattowDirection: 1 means when z becomes larger, gap is smaller; -1 means when z becomes smaller, gap is smaller
        protected void Run(string criteriaSelect = "global", double backDistanceAfterSearching = 0.01, bool runFromContact = true, bool useScanMode = true, sbyte gapNarrowDiretion = 1, bool forWOA = false)
        {
            ProductSelect();

            if (criteriaSelect == "currentMax" && parameters.lossCurrentMax != -50)
            {
                if (productCondition >= 3)
                    lossCriteria = parameters.lossCurrentMax - 0.006;
                else
                    lossCriteria = parameters.lossCurrentMax - 0.02;
            }
            else if (criteriaSelect == "global")
            {
                lossCriteria = parameters.lossCriteria;
                parameters.lossCurrentMax = -50;
            }

            ParameterReset();

            if (!useScanMode)
                lossStage1 = -10;

            if (runFromContact)
            {
                if (forWOA)
                    limitZ = parameters.position[2];
                else
                    // Assume the starting position is at contact, need to go back for some distance first based on focal length
                    limitZ = parameters.position[2] + 0.1 * gapNarrowDiretion;
                beetleControl.ZMoveTo(parameters.position[2] - parameters.productGap[parameters.productName] * gapNarrowDiretion, mode:'t', speed: 2000);
            }

            loss.Add(PowerMeter.Read());
            parameters.position.CopyTo(posCurrentMax, 0);
            while (!parameters.errorFlag)
            {
                if (ParameterUpdate(loss[loss.Count - 1]))
                    break;
                if (XYSearch())
                    break;
                if (ParameterUpdate(loss[loss.Count - 1]))
                    break;
                ZSteppingSearch(gapNarrowDirection: gapNarrowDiretion, forWOA: forWOA);
                //ZSearch(); //this method is not stable yet
            }

            beetleControl.NormalTrajSpeed();
            Console.WriteLine($"Alignment Finished. Best Loss {parameters.lossCurrentMax}");
            Parameters.Log($"Alignment Finished. Best Loss {parameters.lossCurrentMax}");

            if (backDistanceAfterSearching != 0)
                // after searching, go back for some distance in order for another run after applying epoxy.
                beetleControl.ZMoveTo(parameters.position[2] - backDistanceAfterSearching * gapNarrowDiretion);
            beetleControl.DisengageMotors();
        }

        protected virtual void ParameterReset()
        {
            lossFailToImprove = 0;
            secondTry = false;
            doubleCheckFlag = parameters.doublecheckFlag;
            stopInBetweenFlag = parameters.stopInBetweenFlag;

            lossStage1 = -4.0f;
            lossStage2 = -2.0f;
            //searchMode = "scan";

            parameters.errorFlag = false;

            beetleControl.globalErrorCount = 0;

            spd = parameters.productName == "WOA" ? 100 : 400;
            
            // for multimode step size is larger
            if (productCondition >= 3)
            {
                xyStepSizeAmp = 3.0f;
                lossStage1 = -3.0f;
                lossStage2 = -1.5f;
            }
            else
                xyStepSizeAmp = 1.0f;
        }

        // Return true when errorFlag is errected, which is meet criteria here.
        private bool ParameterUpdate(double lossRef)
        {
            if (lossRef < lossStage1)
            {
                searchMode = "scan";
                zStepSizeAmp = 3.5f;
                zMode = "aggressive";
            }
            else if (lossRef < lossStage2)
            {
                searchMode = "interpolation";
                if (parameters.highestAccuracy)
                    beetleControl.tolerance = 1;
                switch(productCondition)
                {
                    case 1: // SM + larget gap
                        zStepSizeAmp = 3.0f;
                        zMode = "aggressive";
                        break;
                    case 2: // SM + small gap
                        zStepSizeAmp = 1.5f;
                        zMode = "normal";
                        break;
                    case 3: // MM + larget gap
                        zStepSizeAmp = 3.5f;
                        zMode = "normal";
                        break;
                    case 4: // MM + small gap
                        zStepSizeAmp = 2.0f;
                        zMode = "normal";
                        break;
                }
            }
            else if (lossRef < lossCriteria)
            {
                searchMode = "interpolation";
                zMode = "normal";
                if (parameters.highestAccuracy)
                    beetleControl.tolerance = 1;
                switch (productCondition)
                {
                    case 1:
                        zStepSizeAmp = 2.5f;
                        break;
                    case 2:
                        zStepSizeAmp = 1.2f;
                        break;
                    case 3:
                        zStepSizeAmp = 2.5f;
                        break;
                    case 4:
                        zStepSizeAmp = 1.5f;
                        break;
                }
            }
            else
            {
                Console.WriteLine($"Meet Criteria {lossCriteria}");
                Parameters.Log($"Meet Criteria {lossCriteria}");
                parameters.errorFlag = true;
                return true;
            }
            return false;
        }

        /*
         * Return true when errorFlag is errected
           Mode can be 's' (step) or 'c' (continusly) or 'i' (interpolation)
           errorFlag is errected when 
                1. scan mode, decrease in both direction;
                2. step mode, loss doesn't change for several steps
                3. interp mode, loss deosn't change for all the sampling points
         */
        private bool XYSearch()
        {
            if (searchMode == "scan")
            {
                if (!AxisScanSearch(axis: 0))
                {
                    Console.WriteLine("X Scan Search Failed");
                    Parameters.Log("X Scan Search Failed");
                    // for scan mode don't set errorFlag until y is complited
                    //parameters.errorFlag = true;
                }
                if (LossMeetCriteria())
                    return true;
                if (!AxisScanSearch(axis: 1))
                {
                    Console.WriteLine("Y Scan Search Failed");
                    Parameters.Log("Y Scan Search Failed");
                    // if x and y scan search all failed, use square search one time
                    if (parameters.lossCurrentMax < -37.0 && !XYSquareSearch())
                    {
                        Console.WriteLine("Square Search Failed");
                        Parameters.Log("Square Search Failed");
                        parameters.errorFlag = true;
                        return true;
                    }
                }
            }
            else if (searchMode == "interpolation")
            {
                if (!AxisInterpolationSearch(axis: 0))
                {
                    Console.WriteLine("X Interpolation Search Failed");
                    Parameters.Log("X Interpolation Search Failed");
                    parameters.errorFlag = true;
                }
                if (parameters.errorFlag || LossMeetCriteria())
                    return true;
                if (!AxisInterpolationSearch(axis: 1))
                {
                    Console.WriteLine("Y Interpolation Search Failed");
                    Parameters.Log("Y Interpolation Search Failed");
                    parameters.errorFlag = true;
                    return true;
                }
            }
            return false;
        }

        private void ZSearch()
        { 
            if (searchMode == "scan")
            {
                if (!AxisScanSearch(axis: 2, scanLossTarget: -0.8f))  // if z scan loss is smaller than -0.8dB, exit directly
                {
                    Console.WriteLine("Z Scan Search Failed");
                    Parameters.Log("Z Scan Search Failed");
                    parameters.errorFlag = true;
                }
            }
            else
                ZSteppingSearch();
        }

        public void PiezoSearchXYZRun()
        {
            parameters.piezoRunning = true;
            parameters.errorFlag = false;
            parameters.errors = "";
            PiezoSteppingSearch(0, targetLess: true);
            PiezoSteppingSearch(1, targetLess: true);
            //PiezoSteppingSearch(0, targetLess: true);
            //PiezoSteppingSearch(1, targetLess: true);
            PiezoSteppingSearch(2, targetLess: true);
            parameters.piezoRunning = false;
        }

    }
}
