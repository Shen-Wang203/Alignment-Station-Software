using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Console_test
{
    class BeetleCuring : BeetleSearch
    {
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
                    zStepSize = 0.001;
                    xyStepSizeAmp = 1.0f;
                    break;
                case 2: // SM + small gap
                    zStepSize = 0.0005;
                    xyStepSizeAmp = 1.0f;
                    break;
                case 3: // MM + larget gap
                    zStepSize = 0.0005;
                    xyStepSizeAmp = 2.0f;
                    bufferBig = 0.007;
                    bufferSmall = 0.007;
                    lowerCriteriaStep = 0.01;
                    break;
                case 4: // MM + small gap
                    zStepSizeAmp = 2.0f;
                    zMode = "normal";
                    break;
            }
        }

        public void CuringRun()
        {

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

        private static void ZStepBack() => BC.ZMoveTo(GlobalVar.position[2] - zStepSize, ignoreError: true, doubleCheck: true);

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
                BC.ZMoveTo(z, ignoreError: true, applyBacklash: true);
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

            BC.ZMoveTo(z, ignoreError: true, applyBacklash: true);
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
