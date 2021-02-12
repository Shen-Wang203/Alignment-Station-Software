using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beetle
{
    class BeetleAlignment : BeetleSearch
    {
        private static BeetleAlignment instance;
        
        public static BeetleAlignment GetInstance()
        {
            if (instance == null)
                instance = new BeetleAlignment();
            return instance;
        }

        //Parameters.errorflag will be used as program stop flag as well.Intance like meet criteria, unexpected high loss, failed
        //      to find better loss and motor errors will errect this flag.


        /* Based on Loss, we divide it into three phases
         * 1. -60 ~ lossStage1: use ScanSearch method
         * 2. lossStage1 ~ lossStage2: use InterpolationSerach method, change ZSteppingSearch parameters 
         * 3. lossStage2 ~ lossCriteria: use InterpolationSearch method, change ZSteppingSearch parameters
        */
        private static float lossStage1 = -4.0f;
        private static float lossStage2 = -2.0f;

        private static string searchMode = "scan"; // defaul scan mode

        public double SetLossCriteria
        {
            get { return lossCriteria; }
            set { Parameters.lossCriteria = value; }
        }

        public void AlignmentRun() => Run(criteriaSelect: "global", backDistanceAfterSearching: 0);

        public void PreAlignRun() => Run(criteriaSelect: "currentMax", backDistanceAfterSearching: 0, runFromContact: false, useScanMode: false);

        // Start search from the current position, and stopped at the best position
        // criteria select: 
        //      "global": use Parameters.lossCriteria as criteria
        //      "currentMax": use lossCurentMax as criteria
        // runFromContact: start searching from the current position where ferrule and lens cap are very close to each other
        // backDistanceAfterSearching: means the distance to go back after searching, this is for another search after applying epoxy
        // useScanMode: in XYSearch, whether to use ScanSearch. This can be achieved by changing the lossStage1 value to a larger one
        private void Run(string criteriaSelect = "global", double backDistanceAfterSearching = 0.01, bool runFromContact = true, bool useScanMode = true)
        {
            ProductSelect();

            if (criteriaSelect == "currentMax" && Parameters.lossCurrentMax != -50)
            {
                if (productCondition >= 3)
                    lossCriteria = Parameters.lossCurrentMax - 0.006;
                else
                    lossCriteria = Parameters.lossCurrentMax - 0.02;
            }
            else if (criteriaSelect == "global")
                lossCriteria = Parameters.lossCriteria;

            ParameterReset();

            if (!useScanMode)
                lossStage1 = -10;

            if (runFromContact)
            {
                // Assume the starting position is at contact, need to go back for some distance first based on focal length
                limitZ = Parameters.position[2] + 0.03;
                BeetleControl.ZMoveTo(limitZ - Parameters.productGap[Parameters.productName]);
            }

            BeetleControl.SlowTrajSpeed();

            loss.Add(PowerMeter.Read());
            Parameters.lossCurrentMax = loss[loss.Count - 1];
            Parameters.position.CopyTo(posCurrentMax, 0);
            while (!Parameters.errorFlag)
            {
                if (ParameterUpdate(loss[loss.Count - 1]))
                    break;
                if (XYSearch())
                    break;
                if (ParameterUpdate(loss[loss.Count - 1]))
                    break;
                ZSteppingSearch();
            }

            BeetleControl.NormalTrajSpeed();
            Console.WriteLine($"Alignment Finished. Best Loss {Parameters.lossCurrentMax}");
            Parameters.Log($"Alignment Finished. Best Loss {Parameters.lossCurrentMax}");

            if (backDistanceAfterSearching != 0)
                // after searching, go back for some distance in order for another run after applying epoxy.
                BeetleControl.ZMoveTo(Parameters.position[2] - backDistanceAfterSearching);
            BeetleControl.DisengageMotors();
        }

        private static void ParameterReset()
        {
            lossFailToImprove = 0;
            secondTry = false;
            Parameters.lossCurrentMax = -50;
            doubleCheckFlag = Parameters.doublecheckFlag;
            stopInBetweenFlag = Parameters.stopInBetweenFlag;

            lossStage1 = -4.0f;
            lossStage2 = -2.0f;
            //searchMode = "scan";

            Parameters.errorFlag = false;

            // for multimmode step size is larger
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
        private static bool ParameterUpdate(double lossRef)
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
                BeetleControl.tolerance = 1;
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
                Parameters.errorFlag = true;
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
                    //Parameters.errorFlag = true;
                }
                if (LossMeetCriteria())
                    return true;
                if (!AxisScanSearch(axis: 1))
                {
                    Console.WriteLine("Y Scan Search Failed");
                    Parameters.Log("Y Scan Search Failed");
                    Parameters.errorFlag = true;
                    return true;
                }
            }
            else if (searchMode == "interpolation")
            {
                if (!AxisInterpolationSearch(axis: 0))
                {
                    Console.WriteLine("X Interpolation Search Failed");
                    Parameters.Log("X Interpolation Search Failed");
                    Parameters.errorFlag = true;
                }
                if (Parameters.errorFlag || LossMeetCriteria())
                    return true;
                if (!AxisInterpolationSearch(axis: 1))
                {
                    Console.WriteLine("Y Interpolation Search Failed");
                    Parameters.Log("Y Interpolation Search Failed");
                    Parameters.errorFlag = true;
                    return true;
                }
            }
            return false;
        }


    }
}
