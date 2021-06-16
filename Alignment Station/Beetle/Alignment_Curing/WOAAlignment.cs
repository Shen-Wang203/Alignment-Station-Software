using System;

namespace Beetle
{
    class WOAAlignment : BeetleSearch
    {
        public WOAAlignment(Parameters prmts, BeetleControl bc, PiezoControl pc) : base(prmts, bc, pc)
        { }

        private void WOAPiezoSearch()
        {
            parameters.errorFlag = false;
            parameters.errors = "";
            PiezoSteppingSearch(0, forWOA: true);
            PiezoSteppingSearch(1, forWOA: true);
            PiezoSteppingSearch(2, forWOA: true);
        }

        public void SingleRun()
        {
            Console.WriteLine("WOA Search");
            Parameters.Log("WOA Search");

            if (!WOABeetleSearch())
                return;

            Console.WriteLine("Starts Piezo Search");
            Parameters.Log("Starts Piezo Search");
            parameters.piezoRunning = true;
            while (!parameters.errorFlag)
            {
                WOAPiezoSearch();
                if (PowerMeter.loss > -18.0)
                    parameters.piezoStepSize = 6;
            }
            parameters.piezoRunning = false;

            Console.WriteLine($"Alignment Finished. Best Loss {parameters.lossCurrentMax}");
            Parameters.Log($"Alignment Finished. Best Loss {parameters.lossCurrentMax}");

            beetleControl.DisengageMotors();
        }

        private bool WOABeetleSearch()
        {
            // Start position needs the ferrule to contact the chip
            limitZ = parameters.position[2] - 0.02;
            beetleControl.ZMoveTo(parameters.position[2] + parameters.productGap[parameters.productName], mode: 't', speed: 2000);
            parameters.position.CopyTo(beetleControl.tempP, 0);

            // XY scan and square scan if nended
            scanSearchRadius = 0.1;
            spd = 100;
            if (!AxisScanSearch(axis: 0))
            {
                Console.WriteLine("X Scan Search Failed");
                Parameters.Log("X Scan Search Failed");
            }
            if (!AxisScanSearch(axis: 1))
            {
                Console.WriteLine("Y Scan Search Failed");
                Parameters.Log("Y Scan Search Failed");
                // if x and y scan search all failed, use square search one time
                if (!XYSquareSearch(singleRange: 0.01))
                {
                    Console.WriteLine("Square Search Failed");
                    Parameters.Log("Square Search Failed");
                    beetleControl.GotoTempTraj(); // return to original position
                    return false;
                }
            }
            return true;
        }

        protected override void StatusCheck(double loss0)
        {
            if (loss0 > (parameters.lossCurrentMax + 0.01))
            {
                parameters.lossCurrentMax = loss0;
                parameters.position.CopyTo(posCurrentMax, 0);
                lossFailToImprove = 0;
            }
            else
            {
                //if ((loss0 < (4 * parameters.lossCurrentMax) && loss0 < -10) || loss0 < -60)
                //{
                //    parameters.errors = "\nUnexpected High Loss";
                //    Console.WriteLine("Unexpected High Loss");
                //    Parameters.Log("Unexpected High Loss");
                //    parameters.errorFlag = true;
                //}

                lossFailToImprove += 1;
                if (lossFailToImprove >= 12 && parameters.piezoRunning)
                {
                    lossFailToImprove = 0;
                    parameters.errors = "Piezo Search Failed to find better Loss";
                    Console.WriteLine("Piezo Search Failed to find better Loss");
                    Parameters.Log("Piezo Search Failed to find better Loss");
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
            if (0.05 * lossRef > 0.4)
                return 0.4;
            else if (lossRef < 3)
                return 0.02;
            else
                return 0.05 * lossRef;
        }

    }
}
