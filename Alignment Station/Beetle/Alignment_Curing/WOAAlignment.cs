using System;

namespace Beetle
{
    class WOAAlignment : BeetleSearch
    {
        public WOAAlignment(Parameters prmts, BeetleControl bc, PiezoControl pc) : base(prmts, bc, pc)
        { }

        public void WOAPiezoSearch()
        {
            parameters.piezoRunning = true;
            parameters.errorFlag = false;
            parameters.errors = "";
            PiezoSteppingSearch(0, forWOA: true);
            parameters.errorFlag = false;
            PiezoSteppingSearch(1, forWOA: true);
            parameters.errorFlag = false;
            PiezoSteppingSearch(2, forWOA: true);
            parameters.piezoRunning = false;
        }

        public void Run()
        {
            ProductSelect();

            lossCriteria = parameters.lossCriteria;
            parameters.lossCurrentMax = -50;

            ParameterReset();

            ScanSearches();
            
            while (!parameters.errorFlag)
            {
                WOAPiezoSearch();
            }

            Console.WriteLine($"Alignment Finished. Best Loss {parameters.lossCurrentMax}");
            Parameters.Log($"Alignment Finished. Best Loss {parameters.lossCurrentMax}");

            beetleControl.DisengageMotors();
        }

        private void ParameterReset()
        {
            lossFailToImprove = 0;
            secondTry = false;
            doubleCheckFlag = parameters.doublecheckFlag;
            stopInBetweenFlag = parameters.stopInBetweenFlag;

            parameters.errorFlag = false;

            beetleControl.globalErrorCount = 0;
        }

        private bool ScanSearches()
        {
            // The lens 'Z' is Beetle's X 
            // Assume the starting position is at contact, need to go back for some distance first based on focal length
            //limitZ = parameters.position[0] + 0.02;
            //beetleControl.XMoveTo(limitZ - parameters.productGap[parameters.productName]);

            // XY search first for three times until loss is smaller than -30
            for (byte i = 0; i < 1; i++)
            {
                if (!AxisScanSearch(axis: 1, limitationAxial: 0))
                {
                    Console.WriteLine("Y Scan Search Failed");
                    Parameters.Log("Y Scan Search Failed");
                    parameters.errorFlag = true;
                    return false;
                }
                // Lens' X is Beetle's Z
                if (!AxisScanSearch(axis: 2, limitationAxial: 0))
                {
                    Console.WriteLine("X Scan Search Failed");
                    Parameters.Log("X Scan Search Failed");
                    parameters.errorFlag = true;
                    return false;
                }
                if (PowerMeter.loss > -30)
                    break;
                else if (i >= 2)
                {
                    Console.WriteLine("Failed to Find First Light");   
                    return false;
                }
            }

            //Console.WriteLine("Push Close");
            //Parameters.Log("Push Close");
            //// Forward to very close
            //beetleControl.XMoveTo(limitZ - 0.1, mode: 't', speed: 5000);

            //if (!AxisScanSearch(axis: 1, limitationAxial: 0))
            //{
            //    Console.WriteLine("Y Scan Search Failed");
            //    Parameters.Log("Y Scan Search Failed");
            //    parameters.errorFlag = true;
            //    return false;
            //}
            //// Lens' X is Beetle's Z
            //if (!AxisScanSearch(axis: 2, limitationAxial: 0))
            //{
            //    Console.WriteLine("X Scan Search Failed");
            //    Parameters.Log("X Scan Search Failed");
            //    parameters.errorFlag = true;
            //    return false;
            //}

            //// Lens' Z is Beetle's X 
            //if (!AxisScanSearch(axis: 0, scanLossTarget: -1.5f, limitationAxial: 0))
            //{
            //    Console.WriteLine("Z Scan Search Failed");
            //    Parameters.Log("Z Scan Search Failed");
            //    parameters.errorFlag = true;
            //    return false;
            //}

            return true;
        }

        protected override void StatusCheck(double loss0)
        {
            if (loss0 > (parameters.lossCurrentMax + 0.01))
            {
                parameters.lossCurrentMax = loss0;
                lossFailToImprove = 0;
            }
            else
            {
                if ((loss0 < (4 * parameters.lossCurrentMax) && loss0 < -10) || loss0 < -45)
                {
                    parameters.errors = "Unexpected High Loss";
                    Console.WriteLine("Unexpected High Loss");
                    Parameters.Log("Unexpected High Loss");
                    parameters.errorFlag = true;
                }

                lossFailToImprove += 1;
                if (lossFailToImprove >= 9)
                {
                    parameters.errors = "Fail to find better Loss, End the Search";
                    Console.WriteLine("Fail to find better Loss, End the Search");
                    Parameters.Log("Fail to find better Loss, End the Search");
                    parameters.errorFlag = true;
                }
            }
        }

        protected override double LossBound(double lossRef)
        {
            lossRef = Math.Abs(lossRef);
            return 0.1 * lossRef;
        }

        protected override double PiezoLossBound(double lossRef)
        {
            lossRef = Math.Abs(lossRef);
            return 0.1 * lossRef;
        }

    }
}
