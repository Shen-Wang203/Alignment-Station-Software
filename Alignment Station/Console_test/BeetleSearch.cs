using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Console_test
{
    class BeetleSearch
    {
        /*
         * All the motor errors will errecte GlobalVar.errorFlag automaticly in BeetleControl, so monitor this flag constantly
         * GlobalVar.errorflag will be used as program stop flag as well. Intance like meet criteria, unexpected high loss, failed
         *     to find better loss and motor errors will errect this flag.
         *     
         * All the error messages will be in GlobalVar.error
         * 
         * Real-time position (Globalvar.position) will be updated whenever XMoveTo (Y, Z as well) or GotoPosition method is called; one exception
         *     is that when checkOnTarget is false, then the GlobalVar.position will be updated early even thought target position hasn't been reached
         *     
         * Loss will be updated in GlobalVar.loss whenever PowerMeter.read() is called
         * 
         * Real-time motor counts can be get from BeetleControl.countsOld
         * 
         * Searching Step Size Adjust:
         *     AxisSteppingSearch step size is adjusted through amplification of min step size (xyStepSizeAmp * minStepSize), so change xyStepSizeAmp can change the step size
         *     AxisInterpolationSearch step size is automatically adjusted based on loss, can change xyStepSizeAmp to change step size as needed (xyStepSizeAmp * theStepBasedOnLoss)
         *     ZSteppingSearch step size is automatically adjusted based on loss, can change zStepSizeAmp to change step size as needed () (zStepSizeAmp * theStepBasedOnLoss)
         */ 
        private static int xDirectionTrend = 1;
        private static int yDirectionTrend = 1;
        private static double[] posCurrentMax = new double[6] { 0, 0, 138, 0, 0, 0};
        private static readonly double stepSearchMinStepSize = 0.0002; // in mm, default is 0.2um 

        protected static sbyte lossFailToImprove = 0;
        protected static bool secondTry = false;
        protected static BeetleControl BC = new BeetleControl();
        protected static sbyte productCondition = 0;
        protected static List<double> loss = new List<double>();
        protected static List<double> pos = new List<double>();
        protected static double lossCriteria = GlobalVar.lossCriteria;
        protected static bool xyStepCountsLimit = false;
        protected static bool xyStepGoBackToLast = true;
        protected static double scanSearchRadius = 0.15; // in mm, default is 150um
        protected static float zStepSizeAmp = 1.0f;
        protected static float xyStepSizeAmp = 1.0f;
        protected static double limitZ = 145;
        protected static string zMode = "normal";
        protected static double lossCurrentMax = -50;

        protected static void ProductSelect()
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
            else
            {
                Console.WriteLine("Unsupported product");
                GlobalVar.errorFlag = true;
            }
        }

        // for axis: x is 0, y is 1
        // radius in mm
        // starting from the current position and exit at the best position (if return true)
        // loss is updated at GlobalVar.loss and match current position (if return true)
        protected bool AxisScanSearch(sbyte axis)
        {
            Console.WriteLine($"Scan Search {axis} Started");
            double p1, p2, p0;
            int count0;
            loss.Clear();
            pos.Clear();
            double loss0;
            sbyte trend, unchange;
            
            if (axis == 0)
            {
                p1 = GlobalVar.position[axis] + scanSearchRadius * xDirectionTrend;
                p2 = GlobalVar.position[axis] - scanSearchRadius * xDirectionTrend;
            }
            else
            {
                p1 = GlobalVar.position[axis] + scanSearchRadius * yDirectionTrend;
                p2 = GlobalVar.position[axis] - scanSearchRadius * yDirectionTrend;
            }

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
                unchange = 0;
                loss.Clear();
                pos.Clear();
                loss0 = PowerMeter.Read();
                while (Math.Abs(GlobalVar.position[axis] - p[i]) > BeetleControl.tolerance * BeetleControl.encoderResolution)
                {
                    BeetleControl.RealCountsFetch(axis);
                    GlobalVar.position[axis] = p0 + (BeetleControl.countsReal[axis] - count0) * BeetleControl.encoderResolution;
                    pos.Add(GlobalVar.position[axis]);
                    loss.Add(PowerMeter.Read());

                    if ((loss[loss.Count - 1] - loss0) <= -LossBound(loss0))
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
                    else
                        unchange += 1;
                    // when trend <= -2 means wrong direction, trend = 1 means has max
                    if (trend <= -2 || trend == 1)
                        break;
                    if (unchange > 50)
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
                    Thread.Sleep(150); // delay 150ms
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
            // if both direction failed to find max, return false and return to original pos
            return false;
        }

        // for axis: x is 0, y is 1
        // step size is not based on loss, can be adjusted through and xyStepSizeAmp. 
        // starting from the current position and exit at the best position
        // loss is updated at GlobalVar.loss and match current position
        // return false only when loss unchanged
        protected bool AxisSteppingSearch(sbyte axis)
        {
            Console.WriteLine($"Stepping Search {axis} Started");
            loss.Clear();
            pos.Clear();
            double loss0, p = GlobalVar.position[axis], bound, diff;
            sbyte trend = 1, sameCount = 0, totalStep = 0;
            loss.Add(PowerMeter.Read());
            pos.Add(GlobalVar.position[axis]);
            loss0 = loss[loss.Count - 1];
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

                if (axis == 0)
                {
                    p = GlobalVar.position[axis] + stepSearchMinStepSize * xyStepSizeAmp * xDirectionTrend;
                    BC.XMoveTo(p, ignoreError: true, applyBacklash: true);
                }
                else
                {
                    p = GlobalVar.position[axis] + stepSearchMinStepSize * xyStepSizeAmp * yDirectionTrend;
                    BC.YMoveTo(p, ignoreError: true, applyBacklash: true);
                }
                // It's important to delay some time after disengaging motor to let the motor fully stopped, then fetch the loss.
                Thread.Sleep(150); // delay 150ms
                loss.Add(PowerMeter.Read());
                pos.Add(GlobalVar.position[axis]);
                if (LossMeetCriteria())
                    return true;

                bound = XYLossBound(loss0);
                diff = loss[loss.Count - 1] - loss0;
                if (diff <= -bound)
                {
                    // Sometimes during curing, going back will make the loss worse because the mim loss position 
                    // is drifting along the forwarding direction
                    if (!xyStepGoBackToLast && totalStep >= 2)
                        return true;
                    if (axis == 0)
                        p = GlobalVar.position[axis] - stepSearchMinStepSize * xyStepSizeAmp * xDirectionTrend;
                    else
                        p = GlobalVar.position[axis] - stepSearchMinStepSize * xyStepSizeAmp * yDirectionTrend;
                    trend -= 1;
                    if (trend != 0)
                    {
                        Console.WriteLine("Over");
                        break;
                    }
                    if (axis == 0)
                        xDirectionTrend = -xDirectionTrend;
                    else
                        yDirectionTrend = -yDirectionTrend;
                    loss0 = loss[loss.Count - 1];
                    Console.WriteLine("Change Direction");
                    sameCount = 0;
                    totalStep = 0;
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
                    if (sameCount >= 2)
                    {
                        if (!xyStepGoBackToLast)
                            return true;
                        if (axis == 0)
                            p = GlobalVar.position[axis] - stepSearchMinStepSize * xyStepSizeAmp * xDirectionTrend * 2;
                        else
                            p = GlobalVar.position[axis] - stepSearchMinStepSize * xyStepSizeAmp * yDirectionTrend * 2;
                        Console.WriteLine("Exit due to same loss");
                        break;
                    }
                }
            }

            if (axis == 0)
                BC.XMoveTo(p, ignoreError: true, applyBacklash: true);
            else
                BC.YMoveTo(p, ignoreError: true, applyBacklash: true);
            // It's important to delay some time after disengaging motor to let the motor fully stopped, then fetch the loss.
            Thread.Sleep(150); // delay 150ms
            loss.Add(PowerMeter.Read());
            pos.Add(GlobalVar.position[axis]);
            StatusCheck(loss.Max());
            // if unchanged, return false
            if (loss.Max() - loss.Min() < 0.002)
                return false;
            if (loss[loss.Count - 1] < (loss.Max() - 0.04))
                Console.WriteLine("Failed to go to best");
            return true;
        }

        // step size is based on loss value, can be adjusted through xyStepSizeAmp. 
        // starting from the current position and exit at the best position (if return true)
        // loss is updated at GlobalVar.loss and match current position (if return true)
        protected bool AxisInterpolationSearch(sbyte axis)
        {
            Console.WriteLine($"Interpolation Search {axis} Started");
            loss.Clear();
            pos.Clear();
            List<double> pList;
            List<double> grid = new List<double>();
            double step, p0 = GlobalVar.position[axis], pFinal;
            sbyte i = 1;
            loss.Add(PowerMeter.Read());
            pos.Add(p0);
            if (LossMeetCriteria())
                return true;
            step = XYInterpStepSize(loss[0]);
            // for multimode fiber, step size times 3
            step *= xyStepSizeAmp;
            if ((axis == 0 && xDirectionTrend == -1) || (axis == 1 && yDirectionTrend == -1))
                pList = new List<double> { p0, p0 - 2 * step, p0 + step, p0 - step, p0 + 2 * step };
            else
                pList = new List<double> { p0, p0 + 2 * step, p0 - step, p0 + step, p0 - 2 * step };
            
            while (i < 6)
            {
                if (axis == 0)
                    BC.XMoveTo(pList[i], ignoreError: true, applyBacklash: true);
                else
                    BC.YMoveTo(pList[i], ignoreError: true, applyBacklash: true);
                Thread.Sleep(150); // delay 150ms
                loss.Add(PowerMeter.Read());
                pos.Add(GlobalVar.position[axis]);
                if (LossMeetCriteria())
                {
                    // larger than start position, then plus steps first, dir_trend is 1
                    if (pList[i] > p0)
                        if (axis == 0)
                            xDirectionTrend = 1;
                        else
                            yDirectionTrend = 1;
                    else if (pList[i] < p0)
                        if (axis == 0)
                            xDirectionTrend = -1;
                        else
                            yDirectionTrend = -1;
                    return true;
                }
                // if current loss is larger than start loss, meaning the direction is right, we dont'
                // need to go to the opposite direction, which is the next iteration
                i += 1;
                if (loss[loss.Count - 1] > loss[0])
                    i += 1;
                if (i == 5 || i == 6)
                {
                    // max loss is at left edge, need to extend on the left for more steps
                    if (loss.Max() == loss[1] && pos[pos.Count - 1] == pList[3])
                    {
                        if (axis == 0)
                            pList.Add(pList[1] + step * xDirectionTrend);
                        else
                            pList.Add(pList[1] + step * yDirectionTrend);
                    }
                    //# max loss is at right edge, need to extend on the right for 2 more steps
                    // make sure the loss and position are matched
                    else if (loss.Max() == loss[loss.Count - 1] && pos[pos.Count - 1] == pList[4])
                    {
                        if (axis == 0)
                            pList.Add(pList[pList.Count - 1] - step * xDirectionTrend);
                        else
                            pList.Add(pList[pList.Count - 1] - step * yDirectionTrend);
                    }
                    else
                        break;
                    i = 5;
                }
            }
            
            if ((loss.Max() - loss.Min()) < 0.002)
            {
                Console.WriteLine("Unchange, go back to original");
                if (axis == 0)
                    BC.XMoveTo(p0, ignoreError: true, applyBacklash: true);
                else
                    BC.YMoveTo(p0, ignoreError: true, applyBacklash: true);
                return false;
            }

            for (double g = pos.Min(); g <= pos.Max(); g += BeetleControl.encoderResolution)
                grid.Add(g);
            double[] s = BarycentericInterpolation(pos, loss, grid);
            pFinal = grid[Array.IndexOf(s, s.Max())];
            if (Math.Abs(pFinal - pos[pos.Count - 1]) > BeetleControl.encoderResolution)
            {
                if (axis == 0)
                    BC.XMoveTo(pFinal, ignoreError: true, applyBacklash: true);
                else
                    BC.YMoveTo(pFinal, ignoreError: true, applyBacklash: true);
                Thread.Sleep(150); // delay 150ms
                loss.Add(PowerMeter.Read());
                pos.Add(GlobalVar.position[axis]);
            }   

            if (pFinal > p0)
                if (axis == 0)
                    xDirectionTrend = 1;
                else
                    yDirectionTrend = 1;
            else if (pFinal < p0)
                if (axis == 0)
                    xDirectionTrend = -1;
                else
                    yDirectionTrend = -1;
            StatusCheck(loss.Max());
            if (loss[loss.Count - 1] < (loss.Max() - 0.04))
                Console.WriteLine("Failed to go to best");

            return true;
        }

        // step size is based on loss value, can be adjusted through zStepSizeAmp. 
        // starting from the current position
        // loss is updated at GlobalVar.loss and match current position (if return true)
        // return false when reach Z limit the second time
        protected bool ZSteppingSearch()
        {
            Console.WriteLine("Z Stepping Started");
            double z = GlobalVar.position[2], loss0, step, bound, diff;
            loss.Clear();
            pos.Clear();
            sbyte successNum = 0;
            loss.Add(PowerMeter.Read());
            pos.Add(z);
            loss0 = loss[loss.Count - 1];

            // Step size is related to loss, for example in -15.72 dB, step size is 15.7 um, then times the amplifier
            step = Math.Round(Math.Abs(loss[loss.Count - 1]), 1) * 0.001 * zStepSizeAmp;
            // large focal length products
            if (step < 0.002 && (productCondition == 1 || productCondition == 3))
                step = 0.002;
            // small focal length products and others
            else if (step < 0.0015)
                step = 0.0015;

            while (!GlobalVar.errorFlag)
            {
                z += step;
                if (z > limitZ)
                {
                    if (secondTry)
                    {
                        GlobalVar.errorFlag = true;
                        return false;
                    }
                    secondTry = true;
                    lossFailToImprove = 0;
                    z -= (step + 0.07);
                    BC.ZMoveTo(z, ignoreError: true);
                    break;
                }

                BC.ZMoveTo(z, ignoreError: true, applyBacklash: true);
                Thread.Sleep(150); // delay 150ms
                loss.Add(PowerMeter.Read());
                pos.Add(GlobalVar.position[2]);
                if (LossMeetCriteria())
                    return true;

                bound = ZLossBound(loss0);
                diff = loss[loss.Count - 1] - loss0;
                /*
                   aggressive mode: Z goes forward until loss is 1.5 times of the initial value
                   for instance, loss_o = -15, then until -22.5 dB we stop forwarding Z
                   The purpose is to forward Z more aggressively to faster the process
                   make sure the loss is smaller than -5 and larger than -40, so that larger Z stepping won't be problem
                   the max num of stepping is 6
                */
                if (zMode == "aggressive" && loss.Min() > -40)
                {
                    if (diff > bound)
                        loss0 = loss[loss.Count - 1];
                    if (loss[loss.Count - 1] > 1.5 * loss0 && successNum < 5)
                    {
                        successNum += 1;
                        continue;
                    }
                }

                if (diff < -bound)
                {
                    z -= step;
                    // radio here should be smaller than 0.5 not 0,5, otherwise two steps will go back to the previous position
                    step *= 0.4;
                    // for large focal length products, if step size is smaller than 1 um, exit
                    if (step < 0.001 && (productCondition == 1 || productCondition == 3))
                    {
                        // don't go back, we want at least 1um forwarding
                        z += step / 0.4;
                        break;
                    }
                    if (successNum != 0)
                    {
                        // go back to the previous points
                        BC.ZMoveTo(z, ignoreError: true, applyBacklash: true);
                        Thread.Sleep(150); // delay 150ms
                        loss.Add(PowerMeter.Read());
                        pos.Add(GlobalVar.position[2]);
                        break;
                    }
                }
                else if (diff > bound)
                {
                    loss0 = loss[loss.Count - 1];
                    successNum += 1;
                }
                else
                {
                    successNum += 1;
                    // if loss is about the same for 5 times, exit to avoid overrun
                    if (successNum == 5)
                        break;
                    // for multimode if loss is the same, exit directly
                    if (productCondition >= 3 && diff > 0)
                        break;
                }
            }

            Console.WriteLine($"Z ends at {z}");
            // set current pos as max loss position temporarily in order to update the max loss position in check_abnormal_loss function
            GlobalVar.position[2] = pos[loss.IndexOf(loss.Max())];
            StatusCheck(loss.Max());
            // change current_pos to the correct one
            GlobalVar.position[2] = z;

            return true;
        }


        // Interpolation method stepsize, use 5 points
        protected double XYInterpStepSize(double lossRef)
        {
            if (lossRef <= -12)
                return 1.25e-3;
            else if (lossRef <= -3)
                return 1e-3;
            else if (lossRef <= -2)
                return 0.65e-3;
            else if (lossRef <= -1)
                return 0.5e-3;
            else
                return 0.3e-3;
        }

        private static double[] BarycentericInterpolation(List<double> x, List<double> f, List<double> grid)
        {
            int n = x.Count;
            int m = grid.Count;
            double[] P = new double[m];
            double[] w = new double[n];
            double a, num, den;
            bool flag;

            // Weight function
            for (int i = 0; i < n; i++)
            {
                a = 1;
                for (int j = 0; j < n; j++)
                {
                    if (j != i)
                        a *= (x[i] - x[j]);
                }
                w[i] = a;
            }
            for (int i = 0; i < n; i++)
                w[i] = 1 / w[i];

            // Configure numeritor and denomitor
            for (int j = 0; j < m; j++)
            {
                num = 0;
                den = 0;
                flag = false;
                for (int i = 0; i < n; i++)
                {
                    if (grid[j] == x[i])
                    {
                        P[j] = f[i];
                        flag = true;
                        break;
                    }
                    else
                    {
                        num += w[i] * f[i] / (grid[j] - x[i]);
                        den += w[i] / (grid[j] - x[i]);
                    }
                }
                // Polynomial expression
                if (!flag)
                    P[j] = num / den;
            }

            return P;
        }

        // In Python code its called check_abnormal_loss
        protected static void StatusCheck(double loss0)
        {
            if (loss0 > (lossCurrentMax + 0.01))
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

        // In Python code is method is called loss_target_check
        protected static bool LossMeetCriteria()
        {
            if (GlobalVar.loss > lossCriteria)
            {
                GlobalVar.errorFlag = true;
                return true;
            }
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
