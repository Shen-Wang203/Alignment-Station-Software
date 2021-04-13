using System;
using System.Linq;
using System.Threading;

namespace Beetle
{
    class BeetleCuring : BeetleSearch
    {

        public BeetleCuring(Parameters prmts, BeetleControl bc, PiezoControl pc) : base(prmts, bc, pc)
        { }

        private readonly int totalMinutes = 16;
        private bool xEpoxySolid = false;
        private bool yEpoxySolid = false;
        private sbyte xySearchCount = 0;
        private sbyte zSearchCount = 0;
        private sbyte zSearchCountLoop = 0;
        private bool laterTimeFlag = false;
        private bool epoxyWillSolidFlag = false;
        private bool xSearchFirst = true;
        private double zStepSize = 0.001; // in mm
        private bool zStepOff = false;
        private bool beginLowerCriteria = false;
        private bool piezoStart = false;

        private double buffer = 0.03;
        private double bufferBig = 0.03;
        private double bufferSmall = 0.015;
        private double lowerCriteriaStep = 0.015;
        private double toleranceForNewCriteria = 0.003;

        private void ParameterReset()
        {
            parameters.errorFlag = false;
            beetleControl.globalErrorCount = 0;
            beetleControl.tolerance = 2;
            // Father class parameter reset
            lossFailToImprove = 0;
            xyStepCountsLimit = false;
            xyStepGoBackToLast = false;
            xyStepSizeAmp = 2.0f;
            doubleCheckFlag = parameters.doublecheckFlag;
            stopInBetweenFlag = parameters.stopInBetweenFlag;
            if (parameters.lossCurrentMax <= -10)
            {
                Console.WriteLine("Run Alignment first");
                // TODO: Display on the GUI
                parameters.errorFlag = true;
            }

            // this class parameter reset
            xEpoxySolid = false;
            yEpoxySolid = false;
            xySearchCount = 0;
            zSearchCount = 0;
            zSearchCountLoop = 0;
            laterTimeFlag = false;
            epoxyWillSolidFlag = false;
            xSearchFirst = true;
            zStepSize = 0.001; // in mm
            zStepOff = false;
            beginLowerCriteria = false;
            piezoStart = false;

            buffer = 0.03;
            bufferBig = 0.03;
            bufferSmall = 0.015;
            lowerCriteriaStep = 0.015;
            toleranceForNewCriteria = 0.003;

            ProductSelect();
            switch (productCondition)
            {
                case 1: // SM + larget gap
                    lossCriteria = parameters.lossCurrentMax - 0.01;
                    zStepSize = 0.001;
                    xyStepSizeAmp = 3.0f; // 6 encoder counts
                    break;
                case 2: // SM + small gap
                    lossCriteria = parameters.lossCurrentMax - 0.01;
                    zStepSize = 0.0005;
                    xyStepSizeAmp = 3.0f; // 6 encoder counts
                    break;
                case 3: // MM + larget gap
                    lossCriteria = parameters.lossCurrentMax - 0.005;
                    zStepSize = 0.0005;
                    xyStepSizeAmp = 8.0f; // 16 encoder counts
                    bufferBig = 0.007;
                    bufferSmall = 0.007;
                    lowerCriteriaStep = 0.01;
                    break;
                case 4: // MM + small gap
                    lossCriteria = parameters.lossCurrentMax - 0.005;
                    zStepSize = 0.0005;
                    xyStepSizeAmp = 6.0f; // 12 encoder counts
                    break;
            }
        }

        public void Run()
        {
            ParameterReset();
            var startTime = DateTime.Now;
            bool curingActive = true;

            loss.Clear();
            while (!parameters.errorFlag)
            {
                // Curing phase control by time
                if (!TimeBasedUpdates(startTime))
                    break;

                // Curing phase control by loss
                Thread.Sleep(500);
                loss.Add(PowerMeter.Read());
                StatusCheck(loss[loss.Count - 1]);
                if (curingActive && laterTimeFlag && loss.Count == 160)
                {
                    // if loss is within the buffer range for 80s, then we assume the epoxy is solid already
                    Console.WriteLine("Loss is stable, pause the program");
                    Parameters.Log("Loss is stable, pause the program");
                    curingActive = false;
                }
                else if (curingActive && loss.Count > 24)
                {
                    if (productCondition >= 3)
                        buffer = 0.003;
                    else
                        buffer = 0.01;
                }
                else if (curingActive && loss.Count == 24)
                {
                    Console.WriteLine("Smaller the buffer");
                    Parameters.Log("Smaller the buffer");
                }

                // if loss is too high, cancel xy search step limit
                if (curingActive && laterTimeFlag && loss[loss.Count - 1] < -3)
                    xyStepCountsLimit = false;
                else if (curingActive && laterTimeFlag && loss[loss.Count - 1] > (lossCriteria - 0.1))
                    xyStepCountsLimit = true;

                // Start Moving if loss smaller than criteria - buffer
                if (curingActive && loss[loss.Count - 1] < (lossCriteria - buffer))
                {
                    buffer = 0;
                    if (!epoxyWillSolidFlag && loss.Count > 80 && laterTimeFlag && productCondition < 3)
                    {
                        epoxyWillSolidFlag = true;
                        lossCriteria -= 0.005;
                        Console.WriteLine("Epoxy will solid, lower criteria 0.005 to minimize movements");
                        Parameters.Log("Epoxy will solid, lower criteria 0.005 to minimize movements");
                    }
                    // Z adjust
                    if (xySearchCount == 2 && !zStepOff) 
                    {
                        if (zSearchCountLoop >= 2)
                        {
                            ZStepBidirection();
                            zSearchCountLoop = 0;
                        }
                        else
                        {
                            ZStepBack();
                            zSearchCountLoop += 1;
                        }
                        xySearchCount = 0;
                        zSearchCount += 1;
                        loss.Add(PowerMeter.Read());
                        if (LossMeetCriteria())
                        {
                            loss.Clear();
                            continue;
                        }
                    }
                    // XY adjust
                    xySearchCount += 1;
                    if (!XYSearch())
                    {
                        Console.WriteLine("X or Y doesn't change");
                        Parameters.Log("X or Y doesn't change");
                        if (xEpoxySolid && yEpoxySolid)
                        {
                            Console.WriteLine("Pause Program because X and Y are solid");
                            Parameters.Log("Pause Program because X and Y are solid");
                            curingActive = false;
                        }
                        parameters.errorFlag = false;
                    }
                    loss.Clear();

                    // TimeBasedUpdates(startTime);

                    // if fail to meet criteria for 2 rounds, then we loose the criteria; don't lower the criteria for the first minute
                    if (zSearchCount >= 1 && !laterTimeFlag && xySearchCount >= 2 && beginLowerCriteria)
                    {
                        lossCriteria -= lowerCriteriaStep;
                        Console.WriteLine($"Lower Criteria for {lowerCriteriaStep}");
                        Parameters.Log($"Lower Criteria for {lowerCriteriaStep}");
                        zSearchCount = 0;
                        // allow one more xy after lower criteria
                        xySearchCount = 1;
                    }
                    // loose criteria earlier after latertimeflag
                    else if (zSearchCount >= 1 && laterTimeFlag && xySearchCount >= 1)
                    {
                        lossCriteria -= lowerCriteriaStep;
                        Console.WriteLine($"Lower Criteria for {lowerCriteriaStep}");
                        Parameters.Log($"Lower Criteria for {lowerCriteriaStep}");
                        zSearchCount = 0;
                        // allow one more xy after lower criteria
                        xySearchCount = 1;
                    }
                    else if (zStepOff && xySearchCount >= 3)
                    {
                        lossCriteria -= lowerCriteriaStep;
                        Console.WriteLine($"Lower Criteria for {lowerCriteriaStep}");
                        Parameters.Log($"Lower Criteria for {lowerCriteriaStep}");
                        zSearchCount = 0;
                        // allow one more xy after lower criteria
                        xySearchCount = 1;
                    }
                }
                else if (curingActive && loss[loss.Count - 1] >= lossCriteria)
                {
                    xySearchCount = 0;
                    zSearchCount = 0;
                    zSearchCountLoop = 0;
                    if (laterTimeFlag)
                        buffer = bufferSmall;
                    else
                        buffer = bufferBig;
                    if (loss[loss.Count-1] > (lossCriteria + toleranceForNewCriteria))
                    {
                        lossCriteria = loss[loss.Count - 1] - toleranceForNewCriteria;
                        Console.WriteLine($"New Criteria {Math.Round(lossCriteria, 4)}");
                        Parameters.Log($"New Criteria {Math.Round(lossCriteria, 4)}");
                    }
                }
            }
            Console.WriteLine("Program Stopped");
            Parameters.Log("Program Stopped");
            beetleControl.DisengageMotors();
            parameters.piezoRunning = false;
        }

        private bool TimeBasedUpdates(DateTime sT)
        {
            TimeSpan timeElapsed = DateTime.Now - sT;

            if (!beginLowerCriteria && timeElapsed.TotalSeconds > 60)
            {
                beginLowerCriteria = true;
                Console.WriteLine("Allow Lower Criteria");
                Parameters.Log("Allow Lower Criteria");
            }
            else if (!xyStepGoBackToLast && timeElapsed.TotalSeconds > 100)
            {
                xyStepGoBackToLast = true;
                Console.WriteLine("XY Step Go Back To Last is On");
                Parameters.Log("XY Step Go Back To Last is On");
                // Change step size smaller at this moment
                xyStepSizeAmp -= 2;
                if (xyStepSizeAmp < 2 && !parameters.smallestResolution)
                {
                    xyStepSizeAmp = 2;
                    beetleControl.tolerance = 1;
                }
                if (parameters.highestAccuracy)
                    beetleControl.tolerance = 1;

                piezoStart = parameters.usePiezo;
                parameters.piezoRunning = piezoStart;
                if (piezoStart)
                {
                    Console.WriteLine("Start Piezo");
                    Parameters.Log("Start Piezo");
                }
            }
            else if (!laterTimeFlag && timeElapsed.TotalSeconds > 150)
            {
                Console.WriteLine("Later Time Flag is On");
                Parameters.Log("Later Time Flag is On");
                laterTimeFlag = true;
                zStepSize = 0.0005;
                buffer = bufferSmall;
                xyStepCountsLimit = true;
                loss.Clear();
                toleranceForNewCriteria = 0.002;
            }
            else if (!zStepOff && timeElapsed.TotalSeconds > 300)
                zStepOff = true;
            else if (timeElapsed.TotalSeconds > totalMinutes * 60)
            {
                Console.WriteLine("Time is Up");
                Parameters.Log("Time is Up");
                return false;
            }

            return true;
        }

        private bool XYSearch()
        {
            double[] P0 = new double[6], P1 = new double[6];
            if (!piezoStart)
                parameters.position.CopyTo(P0, 0);
            else
                parameters.piezoPosition.CopyTo(P0, 0);
            for (int i = 0; i < 2; i ++)
            {
                if (xSearchFirst)
                {
                    if (!xEpoxySolid && ((piezoStart && !PiezoSteppingSearch(axis: 0)) || (!piezoStart && !AxisSteppingSearch(axis: 0))))
                    {
                        Console.WriteLine("X step Unchange");
                        Parameters.Log("X step Unchange");
                        if (laterTimeFlag)
                        {
                            parameters.errorFlag = true;
                            xEpoxySolid = true;
                            return false;
                        }
                    }
                    if (!xEpoxySolid && (LossMeetCriteria() || parameters.errorFlag))
                    {
                        // if meet target on x, then x first
                        if (!xSearchFirst)
                            xSearchFirst = true;
                        return true;
                    }
                    xSearchFirst = false;
                    continue;
                }

                if (!yEpoxySolid && ((piezoStart && !PiezoSteppingSearch(axis: 1)) || (!piezoStart && !AxisSteppingSearch(axis: 1))))
                {
                    Console.WriteLine("Y step Unchange");
                    Parameters.Log("Y step Unchange");
                    if (laterTimeFlag)
                    {
                        parameters.errorFlag = true;
                        yEpoxySolid = true;
                        return false;
                    }
                }
                if (!yEpoxySolid && (LossMeetCriteria() || parameters.errorFlag))
                {
                    // Change x or y search priority based on which one has larger movements
                    if (xSearchFirst)
                        xSearchFirst = false;
                    return true;
                }
                xSearchFirst = true;
            }

            if (!piezoStart)
                parameters.position.CopyTo(P1, 0);
            else
                parameters.piezoPosition.CopyTo(P1, 0);
            // Change x or y search priority based on which one has larger movements
            if (Math.Abs(P1[0] - P0[0]) > (Math.Abs(P1[1] - P0[1]) + 0.0001) && !xSearchFirst)
                xSearchFirst = true;
            else if ((Math.Abs(P1[0] - P0[0]) + 0.0001) < Math.Abs(P1[1] - P0[1]) && xSearchFirst)
                xSearchFirst = false;

            return true;
        }

        private void ZStepBack()
        {
            Console.WriteLine("Z Steps Back");
            Parameters.Log("Z Steps Back");
            if (!piezoStart)
                beetleControl.ZMoveTo(parameters.position[2] - zStepSize, ignoreError: true, doubleCheck: false, stopInBetween: stopInBetweenFlag);
            //PiezoControl.GetInstance().Send(2, (ushort)(parameters.piezoPosition[2] + 70 * parameters.piezoZvsGap)); // gap larger for about 0.5 um
        }

        private void ZStepBidirection()
        {
            if (piezoStart)
            {
                PiezoSteppingSearch(axis: 2);
                return;
            }
            Console.WriteLine("Z Bidirection Stepping Start");
            Parameters.Log("Z Bidirection Stepping Start");
            double z, loss0, bound, diff;
            z = parameters.position[2];
            sbyte trend = 1, sameCount = 0;
            int direc = 1; // go up or smaller the gap first
            loss.Clear();
            pos.Clear();
            loss.Add(PowerMeter.Read());
            pos.Add(z);
            loss0 = loss[loss.Count - 1];

            while (!parameters.errorFlag)
            {
                z += zStepSize * direc;
                beetleControl.ZMoveTo(z, ignoreError: true, applyBacklash: true, doubleCheck: false, stopInBetween: stopInBetweenFlag);
                Thread.Sleep(150); // delay 150ms
                pos.Add(parameters.position[2]);
                loss.Add(PowerMeter.Read());
                if (LossMeetCriteria())
                    return;

                bound = LossBound(loss0);
                diff = loss[loss.Count - 1] - loss0;
                if (diff <= -bound)
                {
                    z -= zStepSize * direc;
                    trend -= 1;
                    if (trend != 0)
                    {
                        Console.WriteLine("Over");
                        Parameters.Log("Over");
                        break;
                    }
                    direc = -direc;
                    loss0 = loss[loss.Count - 1];
                    Console.WriteLine("Change Direction");
                    Parameters.Log("Change Direction");
                    sameCount = 0;
                }
                else if (diff >= bound)
                {
                    trend = 2;
                    loss0 = loss[loss.Count - 1];
                    sameCount = 0;
                }
                else
                {
                    trend = 2;
                    sameCount += 1;
                    if (sameCount >= 5)
                    {
                        z = z - zStepSize * direc * 5 - beetleControl.zBacklashMM * direc;
                        Console.WriteLine("Loss doesn't Change in Z");
                        Parameters.Log("Loss doesn't Change in Z");
                        break;
                    }
                }
            }

            beetleControl.ZMoveTo(z, ignoreError: true, applyBacklash: true, doubleCheck: false, stopInBetween: stopInBetweenFlag);
            Thread.Sleep(150);
            StatusCheck(loss.Max());
        }


        protected override void StatusCheck(double loss0)
        {
            if (loss0 > parameters.lossCurrentMax)
            {
                parameters.lossCurrentMax = loss0;
                lossCriteria = parameters.lossCurrentMax - toleranceForNewCriteria;
            }
            else if (loss0 < -20)
            {
                Console.WriteLine("Unexpected High Loss");
                Parameters.Log("Unexpected High Loss");
                beetleControl.NormalTrajSpeed();
                parameters.errorFlag = true;
            }
        }

        protected override bool LossMeetCriteria()
        {
            if (PowerMeter.loss >= lossCriteria)
            {
                Console.WriteLine($"Meet Criteria {Math.Round(lossCriteria, 4)}");
                Parameters.Log($"Meet Criteria {Math.Round(lossCriteria, 4)}");
                xySearchCount = 0;
                zSearchCount = 0;
                zSearchCountLoop = 0;
                if (laterTimeFlag)
                    buffer = bufferSmall;
                else
                    buffer = bufferBig;
                if (PowerMeter.loss > (lossCriteria + toleranceForNewCriteria))
                    lossCriteria = PowerMeter.loss - toleranceForNewCriteria;
                if (PowerMeter.loss > parameters.lossCurrentMax)
                {
                    parameters.lossCurrentMax = PowerMeter.loss;
                    lossCriteria = parameters.lossCurrentMax - toleranceForNewCriteria;
                }

                return true;
            }
            return false;
        }

        private new double LossBound(double lossRef)
        {
            lossRef = Math.Abs(lossRef);
            if (lossRef < 0.7)
                return 0.004;
            else if (lossRef < 1.5)
                return 0.005;
            else if (lossRef > 50)
                return 4;
            else
                return (0.00003 * lossRef * lossRef * lossRef - 0.0011 * lossRef * lossRef + 0.0245 * lossRef - 0.018) * 0.8;
        }

    }

}
