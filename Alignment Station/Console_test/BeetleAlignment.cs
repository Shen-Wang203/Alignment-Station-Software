using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_test
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

        //GlobalVar.errorflag will be used as program stop flag as well.Intance like meet criteria, unexpected high loss, failed
        //      to find better loss and motor errors will errect this flag.


        /* Based on Loss, we divide it into three phases
         * 1. -60 ~ lossStage1: use ScanSearch method
         * 2. lossStage1 ~ lossStage2: use InterpolationSerach method, change ZSteppingSearch parameters 
         * 3. lossStage2 ~ lossCriteria: use InterpolationSearch method, change ZSteppingSearch parameters
        */
        private static float lossStage1 = -4.0f;
        private static float lossStage2 = -2.0f;

        private static string searchMode = "scan"; // defaul scan mode

        public static double SetLossCriteria
        {
            get { return lossCriteria; }
            set { GlobalVar.lossCriteria = value; }
        }

        public static double ReadLossCurrentMax => lossCurrentMax;

        // Start search from the current position, and stopped at the best position
        // criteria select: 
        //      "global": use GlobalVar.lossCriteria as criteria
        //      "currentMax": use lossCurentMax as criteria
        // runFromContact: start searching from the current position where ferrule and lens cap are very close to each other
        // backDistanceAfterSearching: means the distance to go back after searching, this is for another search after applying epoxy
        // useScanMode: in XYSearch, whether to use ScanSearch. This can be achieved by changing the lossStage1 value to a larger one
        public void Run(string criteriaSelect, double backDistanceAfterSearching, bool runFromContact, bool useScanMode = true)
        {
            ProductSelect();

            if (criteriaSelect == "currentMax" && lossCurrentMax != -50)
            {
                if (productCondition >= 3)
                    lossCriteria = lossCurrentMax - 0.006;
                else
                    lossCriteria = lossCurrentMax - 0.02;
            }
            else if (criteriaSelect == "global")
                lossCriteria = GlobalVar.lossCriteria;

            ParameterReset();

            if (!useScanMode)
                lossStage1 = -10;

            if (runFromContact)
            {
                // Assume the starting position is at contact, need to go back for some distance first based on focal length
                limitZ = GlobalVar.position[2];
                BC.ZMoveTo(limitZ - GlobalVar.product[GlobalVar.productName]);
            }

            BeetleControl.SlowTrajSpeed();

            loss.Add(PowerMeter.Read());
            lossCurrentMax = loss[loss.Count - 1];
            while (!GlobalVar.errorFlag)
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
            Console.WriteLine($"Best Loss {lossCurrentMax}");

            if (backDistanceAfterSearching != 0)
                // after searching, go back for some distance in order for another run after applying epoxy.
                BC.ZMoveTo(GlobalVar.position[2] - backDistanceAfterSearching);
        }

        private static void ParameterReset()
        {
            lossFailToImprove = 0;
            secondTry = false;
            lossCurrentMax = -50;

            lossStage1 = -4.0f;
            lossStage2 = -2.0f;
            //searchMode = "scan";

            GlobalVar.errorFlag = false;

            // for multimmode step size is larger
            if (productCondition >= 3)
            {
                xyStepSizeAmp = 3.0f;
                lossStage1 = -3.0f;
                lossStage2 = -1.5f;
            }
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
                GlobalVar.errorFlag = true;
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
                    // for scan mode don't set errorFlag until y is complited
                    //GlobalVar.errorFlag = true;
                }
                if (LossMeetCriteria())
                    return true;
                if (!AxisScanSearch(axis: 1))
                {
                    Console.WriteLine("Y Scan Search Failed");
                    GlobalVar.errorFlag = true;
                    return true;
                }
            }
            else if (searchMode == "interpolation")
            {
                if (!AxisInterpolationSearch(axis: 0))
                {
                    Console.WriteLine("X Interpolation Search Failed");
                    GlobalVar.errorFlag = true;
                }
                if (GlobalVar.errorFlag || LossMeetCriteria())
                    return true;
                if (!AxisInterpolationSearch(axis: 1))
                {
                    Console.WriteLine("Y Interpolation Search Failed");
                    GlobalVar.errorFlag = true;
                    return true;
                }
            }
            return false;
        }


    }
}
