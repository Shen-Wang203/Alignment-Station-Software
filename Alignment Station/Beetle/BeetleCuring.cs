﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Beetle
{
    class BeetleCuring : BeetleSearch
    {
        private static BeetleCuring instance;

        public static BeetleCuring GetInstance()
        {
            if (instance == null)
                instance = new BeetleCuring();
            return instance;
        }

        private static readonly int totalMinutes = 16;
        private static bool xEpoxySolid = false;
        private static bool yEpoxySolid = false;
        private static sbyte xySearchCount = 0;
        private static sbyte zSearchCount = 0;
        private static sbyte zSearchCountLoop = 0;
        private static bool laterTimeFlag = false;
        private static bool epoxyWillSolidFlag = false;
        private static bool xSearchFirst = true;
        private static double zStepSize = 0.001; // in mm

        private static double buffer = 0.03;
        private static double bufferBig = 0.03;
        private static double bufferSmall = 0.015;
        private static double lowerCriteriaStep = 0.015;
        private static double toleranceForNewCriteria = 0.003;

        private static void ParameterReset()
        {
            GlobalVar.errorFlag = false;
            // Father class parameter reset
            lossFailToImprove = 0;
            xyStepCountsLimit = false;
            xyStepGoBackToLast = false;
            xyStepSizeAmp = 1.0f;
            if (lossCurrentMax >= -10)
            {
                Console.WriteLine("Run Alignment first");
                GlobalVar.errorFlag = true;
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

            buffer = 0.03;
            bufferBig = 0.03;
            bufferSmall = 0.015;
            lowerCriteriaStep = 0.015;
            toleranceForNewCriteria = 0.003;

            switch (productCondition)
            {
                case 1: // SM + larget gap
                    lossCriteria = lossCurrentMax - 0.01;
                    zStepSize = 0.001;
                    xyStepSizeAmp = 1.0f;
                    break;
                case 2: // SM + small gap
                    lossCriteria = lossCurrentMax - 0.01;
                    zStepSize = 0.0005;
                    xyStepSizeAmp = 1.0f;
                    break;
                case 3: // MM + larget gap
                    lossCriteria = lossCurrentMax - 0.005;
                    zStepSize = 0.0005;
                    xyStepSizeAmp = 2.0f;
                    bufferBig = 0.007;
                    bufferSmall = 0.007;
                    lowerCriteriaStep = 0.01;
                    break;
                case 4: // MM + small gap
                    lossCriteria = lossCurrentMax - 0.005;
                    zStepSizeAmp = 2.0f;
                    zMode = "normal";
                    break;
            }
        }

        public void Run()
        {
            ParameterReset();
            var startTime = DateTime.Now;
            TimeSpan timeElapsed;
            bool curingActive = true;

            loss.Clear();
            while (!GlobalVar.errorFlag)
            {
                // Curing phase control by time
                timeElapsed = DateTime.Now - startTime;
                if (timeElapsed.Seconds > totalMinutes * 60)
                {
                    Console.WriteLine("Time is up");
                    break;
                }
                else if (!laterTimeFlag && timeElapsed.Seconds > 150)
                {
                    Console.WriteLine("Later Time Flag is on");
                    laterTimeFlag = true;
                    zStepSize = 0.0005;
                    buffer = bufferSmall;
                    xyStepCountsLimit = true;
                    loss.Clear();
                    toleranceForNewCriteria = 0.002;
                }
                else if (!xyStepGoBackToLast && timeElapsed.Seconds > 60)
                {
                    xyStepGoBackToLast = true;
                    Console.WriteLine("XY Step Go Back To Last is on");
                }

                // Curing phase control by loss
                Thread.Sleep(500);
                loss.Add(PowerMeter.Read());
                StatusCheck(loss[loss.Count - 1]);
                if (curingActive && laterTimeFlag && loss.Count == 160)
                {
                    // if loss is within the buffer range for 80s, then we assume the epoxy is solid already
                    Console.WriteLine("Loss is stable, pause the program");
                    curingActive = false;
                }
                else if (curingActive && loss.Count > 24)
                {
                    if (productCondition >= 3)
                        buffer = 0.003;
                    else
                        buffer = 0.007;
                }
                else if (curingActive && loss.Count == 24)
                    Console.WriteLine("Smaller the buffer");

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
                        Console.WriteLine("Epoxy will solid, lower criteri 0.005 to minimize movements");
                    }
                    // Z adjust
                    if (xySearchCount == 2)
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
                        if (xEpoxySolid && yEpoxySolid)
                        {
                            Console.WriteLine("Pause Program because X and Y are solid");
                            curingActive = false;
                        }
                        GlobalVar.errorFlag = false;
                    }
                    loss.Clear();

                    // if fail to meet criteria for 2 rounds, then we loose the criteria
                    if (zSearchCount >= 1 && !laterTimeFlag && xySearchCount >= 2 && timeElapsed.Seconds > 50)
                    {
                        lossCriteria -= lowerCriteriaStep;
                        Console.WriteLine($"Lower Criteria for {lowerCriteriaStep}");
                        zSearchCount = 0;
                        // allow one more xy after lower criteria
                        xySearchCount = 1;
                    }
                    // loose criteria earlier after latertimeflag
                    else if (zSearchCount >= 1 && laterTimeFlag && xySearchCount >= 1)
                    {
                        lossCriteria -= lowerCriteriaStep;
                        Console.WriteLine($"Lower Criteria for {lowerCriteriaStep}");
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
                    }
                }
            }
        }

        private bool XYSearch()
        {
            double[] P0 = new double[6], P1 = new double[6];
            GlobalVar.position.CopyTo(P0, 0);
            for (int i = 0; i < 2; i ++)
            {
                if (xSearchFirst)
                {
                    if (!xEpoxySolid && !AxisSteppingSearch(axis: 0))
                    {
                        Console.WriteLine("X step Unchange");
                        if (laterTimeFlag)
                        {
                            GlobalVar.errorFlag = true;
                            xEpoxySolid = true;
                            return false;
                        }
                    }
                    if (!xEpoxySolid && (LossMeetCriteria() || GlobalVar.errorFlag))
                    {
                        // if meet target on x, then x first
                        if (!xSearchFirst)
                            xSearchFirst = true;
                        return true;
                    }
                    xSearchFirst = false;
                    continue;
                }

                if (!yEpoxySolid && !AxisSteppingSearch(axis: 1))
                {
                    Console.WriteLine("Y step Unchange");
                    if (laterTimeFlag)
                    {
                        GlobalVar.errorFlag = true;
                        yEpoxySolid = true;
                        return false;
                    }
                }
                if (!yEpoxySolid && (LossMeetCriteria() || GlobalVar.errorFlag))
                {
                    // Change x or y search priority based on which one has larger movements
                    if (xSearchFirst)
                        xSearchFirst = false;
                    return true;
                }
                xSearchFirst = true;
            }

            GlobalVar.position.CopyTo(P1, 0);
            // Change x or y search priority based on which one has larger movements
            if (Math.Abs(P1[0] - P0[0]) > (Math.Abs(P1[1] - P0[1]) + 0.0001) && !xSearchFirst)
                xSearchFirst = true;
            else if ((Math.Abs(P1[0] - P0[0]) + 0.0001) < Math.Abs(P1[1] - P0[1]) && xSearchFirst)
                xSearchFirst = false;

            return true;
        }

        private static void ZStepBack() => BeetleControl.ZMoveTo(GlobalVar.position[2] - zStepSize, ignoreError: true, doubleCheck: true);

        private static void ZStepBidirection()
        {
            double z = GlobalVar.position[2], loss0, bound, diff;
            sbyte trend = 1, sameCount = 0;
            int direc = -1;
            loss.Clear();
            pos.Clear();
            loss.Add(PowerMeter.Read());
            pos.Add(z);
            loss0 = loss[loss.Count - 1];

            while (!GlobalVar.errorFlag)
            {
                z += zStepSize * direc;
                BeetleControl.ZMoveTo(z, ignoreError: true, applyBacklash: true);
                Thread.Sleep(150); // delay 150ms
                loss.Add(PowerMeter.Read());
                pos.Add(GlobalVar.position[2]);
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
                        break;
                    }
                    direc = -direc;
                    loss0 = loss[loss.Count - 1];
                    Console.WriteLine("Change Direction");
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
                        z = z - zStepSize * direc * 5 - BeetleControl.zBacklashMM * direc;
                        Console.WriteLine("Loss doesn't Change in Z");
                        break;
                    }
                }
            }

            BeetleControl.ZMoveTo(z, ignoreError: true, applyBacklash: true);
            Thread.Sleep(150);
            StatusCheck(loss.Max());
        }


        private static new void StatusCheck(double loss0)
        {
            if (loss0 > lossCurrentMax)
            {
                lossCurrentMax = loss0;
                lossCriteria = lossCurrentMax - toleranceForNewCriteria;
            }
            else if (loss0 < -20)
            {
                Console.WriteLine("Unexpected High Loss");
                BeetleControl.NormalTrajSpeed();
                GlobalVar.errorFlag = true;
            }
        }

        private static new bool LossMeetCriteria()
        {
            if (GlobalVar.loss >= lossCriteria)
            {
                Console.WriteLine($"Meet Criteria {Math.Round(lossCriteria, 4)}");
                xySearchCount = 0;
                zSearchCount = 0;
                zSearchCountLoop = 0;
                if (laterTimeFlag)
                    buffer = bufferSmall;
                else
                    buffer = bufferBig;
                if (GlobalVar.loss > (lossCriteria + toleranceForNewCriteria))
                    lossCriteria = GlobalVar.loss - toleranceForNewCriteria;
                if (GlobalVar.loss > lossCurrentMax)
                {
                    lossCurrentMax = GlobalVar.loss;
                    lossCriteria = lossCurrentMax - toleranceForNewCriteria;
                }

                return true;
            }
            return false;
        }

        private static new double LossBound(double lossRef)
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