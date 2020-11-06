using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace Console_test
{
    class BeetleAlignment
    {
        /*
         * All the motor errors will erecte GlobalVar.errorFlag automaticly in BeetleControl, so monitor this flag constantly
         * All the error messages will be in GlobalVar.error
         * Real-time position (Globalvar.position) will be updated whenever XMoveTo (Y, Z as well) or GotoPosition method is called; one exception
         *     is that when checkOnTarget is false, then the GlobalVar.position will be updated early even thought target position hasn't been reached
         * Loss will be updated in GlobalVar.loss whenever PowerMeter.read() is called
         * Real-time motor counts can be get from BeetleControl.countsOld
         */


        private static int xDirectionTrend = 1;
        private static int yDirectionTrend = 1;
        private static double lossCurrentMax;
        private static double[] posCurrentMax;
        private static sbyte lossFailToImprove = 0;
        private static sbyte productCondition = 0;

        protected static BeetleControl BC = new BeetleControl();
        protected static double lossCriteria = -0.2;
        protected static bool xyStepCountsLimit = false;
        protected static double scanSearchRadius = 0.15; // in mm, default is 150um
        protected static double stepSearchStepSize = 0.0005; // in mm, default is 0.5um 

        public BeetleAlignment()
        {
            /* Add more if product extended
            *  productCondition has four types:
            *       = 1: SM + Large Ferrule Focal Length (> 0.1mm)
            *       = 2: SM + Small Ferrule Focal Length (< 0.1mm)
            *       = 3: MM + Large Ferrule Focal Length (> 0.1mm)
            *       = 4: MM + Small Ferrule Focal Length (< 0.1mm)
            *  all different products should be classified as one of those four types
            */
            if (GlobalVar.productName == "VOA")
                productCondition = 2;
            else if (GlobalVar.productName == "SM1xN")
                productCondition = 1;
            else if (GlobalVar.productName == "MM1xN")
                productCondition = 3;
            else if (GlobalVar.productName == "UWDM")
                productCondition = 2;
        }

        // for axis: x is 0, y is 1
        // radius in mm
        protected bool AxisScanSearch(sbyte axis)
        {
            double p1, p2, p0;
            int count0;
            List<double> loss = new List<double>();
            List<double> pos = new List<double>();
            double loss0;
            sbyte trend;

            p1 = GlobalVar.position[axis] - scanSearchRadius * xDirectionTrend;
            p2 = GlobalVar.position[axis] + scanSearchRadius * xDirectionTrend;
            double[] p = new double[2] { p1, p2 };
            p0 = GlobalVar.position[axis];
            count0 = BeetleControl.countsOld[axis];

            for (int i = 0; i < 2; i ++)
            {
                if (axis == 0)
                    BC.XMoveTo(p[i], mode: 't', checkOnTarget: false);
                else
                    BC.YMoveTo(p[i], mode: 't', checkOnTarget: false);
                
                // Active Monitor Loss and Position
                trend = 0;
                loss.Clear();
                pos.Clear();
                loss0 = PowerMeter.Read();
                while (Math.Abs(GlobalVar.position[axis] - p[i]) > BeetleControl.tolerance * BeetleControl.encoderResolution)
                {
                    BeetleControl.RealCountsFetch(axis);
                    GlobalVar.position[axis] = p0 + (BeetleControl.countsReal[axis] - count0) * BeetleControl.encoderResolution;
                    pos.Add(GlobalVar.position[axis]);
                    loss.Add(PowerMeter.Read());

                    if ((loss[loss.Count-1] - loss0) <= -LossBound(loss0))
                    {
                        trend -= 1;
                        loss0 = loss[loss.Count - 1];
                    }
                    else if ((loss[loss.Count - 1] - loss0) >= LossBound(loss0))
                    {
                        trend = 3;
                        loss0 = loss[loss.Count - 1];
                        continue;
                    }
                    // when trend <= -2 means wrong direction, trend = 1 means has max
                    if (trend <= -2 || trend == 1)
                        break;
                }

                // if has max, go to the max position
                if (trend == 1)
                {
                    if (axis == 0)
                    {
                        BC.XMoveTo(pos[loss.IndexOf(loss.Max())]);
                        xDirectionTrend = xDirectionTrend * (-2 * i + 1);
                    }
                    else
                    {
                        BC.YMoveTo(pos[loss.IndexOf(loss.Max())]);
                        yDirectionTrend = yDirectionTrend * (-2 * i + 1);
                    }
                    StatusCheck(PowerMeter.Read());
                    return true;
                }
                // else go the other direction
                else
                {
                    // return to original position first
                    if (axis == 0)
                        BC.XMoveTo(p0);
                    else
                        BC.YMoveTo(p0);
                }
            }
            // if both direction failed to find max, return false
            return false;
        }

        protected bool AxisStepping(sbyte axis, double stepSize)
        {
            List<double> loss = new List<double>();
            List<double> pos = new List<double>();
            double loss0, p;
            sbyte trend = 1, sameCount = 0, totalStep = 0;
            loss.Add(PowerMeter.Read());
            pos.Add(GlobalVar.position[axis]);
            if (LossMeetCriteria())
                return true;
            while (!GlobalVar.errorFlag)
            {
                totalStep += 1;
                if (xyStepCountsLimit && totalStep >= 4)
                {
                    Console.WriteLine("Reach step limit");
                    return true;
                }
                p = GlobalVar.position[axis] - stepSearchStepSize * xDirectionTrend;
                if (axis == 0)
                    BC.XMoveTo(p, ignoreError: true, applyBacklash: true);
                else
                    BC.YMoveTo(p, ignoreError: true, applyBacklash: true);
                // It's important to delay some time after disengaging motor to let the motor fully stopped, then fetch the loss.
                Thread.Sleep(150); // delay 150ms
                loss.Add(PowerMeter.Read());
                pos.Add(GlobalVar.position[axis]);
                if (LossMeetCriteria())
                    return true;



            }
            return true;
        }

        protected virtual void StatusCheck(double loss0)
        {
            if (loss0 > (lossCurrentMax + 0.007))
            {
                lossCurrentMax = loss0;
                GlobalVar.position.CopyTo(posCurrentMax, 0);
                lossFailToImprove = 0;
            }
            else
            {
                if ((loss0 < (4 * lossCurrentMax) && loss0 < -10) || loss0 < -45)
                {
                    GlobalVar.errors = "Unexpected High Loss";
                    BeetleControl.NormalTrajSpeed();
                    BC.GotoReset();
                    GlobalVar.errorFlag = true;
                }

                lossFailToImprove += 1;
                if (lossFailToImprove == 6)
                {
                    lossFailToImprove = 0;
                    GlobalVar.errors = "Fail to find better Loss, Go to Best Position";
                    GlobalVar.errorFlag = true;
                    BC.GotoPosition(posCurrentMax);
                }
            }
        }

        protected virtual bool LossMeetCriteria()
        {
            if (GlobalVar.loss > lossCriteria)
                return true;
            return false;
        }

        protected static double LossBound(double lossRef)
        {
            lossRef = Math.Abs(lossRef);
            if (lossRef < 0.7)
                return 0.002;
            else if (lossRef < 1.5)
                return 0.005;
            else if (lossRef > 50)
                return 4;
            else
                return (0.00003 * lossRef * lossRef * lossRef - 0.0011 * lossRef * lossRef + 0.0245 * lossRef - 0.018) * 0.8;
        }

        private static double LossBoundSmall(double lossRef)
        {
            lossRef = Math.Abs(lossRef);
            if (lossRef < 0.7)
                return 0.0007;
            else if (lossRef < 1.5)
                return 0.003;
            else if (lossRef > 50)
                return 4;
            else
                return (0.00003 * lossRef * lossRef * lossRef - 0.0011 * lossRef * lossRef + 0.0245 * lossRef - 0.018) * 0.5;
        }

        private static double ZLossBound(double lossRef)
        {
            if (productCondition == 1)
            {
                lossRef = Math.Abs(lossRef);
                if (lossRef < 0.5)
                    return 0.05;
                else if (lossRef < 0.7)
                    return 0.1;
                else if (lossRef < 1.2)
                    return 0.3;
                else if (lossRef < 4)
                    return 0.3 * lossRef;
                else if (lossRef > 50)
                    return 3;
                else
                    // x = 40,   30,   20,   10,   8,    6,    4
                    // y = 2.46, 1.84, 1.23, 0.61, 0.49, 0.37, 0.24
                    return 0.06141469 * lossRef - 0.001513462;
            }
            else if (productCondition == 2)
                return LossBound(lossRef);
            else
                return LossBoundSmall(lossRef);
        }

        private static double XYLossBound(double lossRef)
        {
            if (productCondition <= 2)
                return LossBound(lossRef);
            else
                return LossBoundSmall(lossRef);
        }


    }
}
