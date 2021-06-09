using System;

namespace Beetle
{
    class WOAAlignment : BeetleAlignment
    {
        public WOAAlignment(Parameters prmts, BeetleControl bc, PiezoControl pc) : base(prmts, bc, pc)
        { }

        public void WOAPiezoSearch()
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
            Run(criteriaSelect: "global", backDistanceAfterSearching: 0.01, runFromContact: true, useScanMode: true, gapNarrowDiretion: -1, forWOA: true);

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

        protected override void ParameterReset()
        {
            lossFailToImprove = 0;
            secondTry = false;
            doubleCheckFlag = parameters.doublecheckFlag;
            stopInBetweenFlag = parameters.stopInBetweenFlag;

            parameters.errorFlag = false;

            beetleControl.globalErrorCount = 0;

            // TODO: need to be tunned
            xyStepSizeAmp = 1.0f;
            lossStage1 = -20.0f;
            lossStage2 = -15.0f;

            scanSearchRadius = 0.1;
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
                if ((loss0 < (4 * parameters.lossCurrentMax) && loss0 < -10) || loss0 < -45)
                {
                    parameters.errors = "\nUnexpected High Loss";
                    Console.WriteLine("Unexpected High Loss");
                    Parameters.Log("Unexpected High Loss");
                    parameters.errorFlag = true;
                }

                lossFailToImprove += 1;
                if (lossFailToImprove == 6 && !parameters.piezoRunning)
                {
                    lossFailToImprove = 0;
                    parameters.errors = "Beetle Search Failed to find better Loss, Go to Best Position";
                    Console.WriteLine("Beetle Search Failed to find better Loss, Go to Best Position");
                    Parameters.Log("Beetle Search Failed to find better Loss, Go to Best Position");
                    parameters.errorFlag = true;
                    beetleControl.GotoPosition(posCurrentMax);
                }
                else if (lossFailToImprove >= 12 && parameters.piezoRunning)
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
            if (0.05 * lossRef > 0.7)
                return 0.7;
            else if (lossRef < 3)
                return 0.02;
            else
                return 0.05 * lossRef;
        }

    }
}
