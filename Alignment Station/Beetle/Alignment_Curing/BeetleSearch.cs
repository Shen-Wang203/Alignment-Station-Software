﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Beetle
{
    class BeetleSearch
    {
        /*
         * All the motor errors will errecte Parameters.errorFlag automaticly in beetleControl, so monitor this flag constantly
         * Parameters.errorflag will be used as program stop flag as well. Intance like meet criteria, unexpected high loss, failed
         *     to find better loss and motor errors will errect this flag.
         *     
         * All the error messages will be in Parameters.error
         * 
         * Real-time position (Parameters.position) will be updated whenever XMoveTo (Y, Z as well) or GotoPosition method is called; one exception
         *     is that when checkOnTarget is false, then the Parameters.position will be updated early even thought target position hasn't been reached
         *     
         * Loss will be updated in Parameters.loss whenever PowerMeter.read() is called
         * 
         * Real-time motor counts can be get from beetleControl.countsReal
         * 
         * Motors don't need to engage before moving, beetleControl will engage motors that will need to move. But remember to disengage all motors after alignment and curing process.
         * 
         * Searching Step Size Adjust:
         *     AxisSteppingSearch step size is adjusted through amplification of min step size (xyStepSizeAmp * stepSearchMinStepSize), so change xyStepSizeAmp can change the step size
         *     AxisInterpolationSearch step size is automatically adjusted based on loss, can change xyStepSizeAmp to change step size as needed (xyStepSizeAmp * theStepBasedOnLoss)
         *     ZSteppingSearch step size is automatically adjusted based on loss, can change zStepSizeAmp to change step size as needed () (zStepSizeAmp * theStepBasedOnLoss)
         */ 
        private int xDirectionTrend = 1;
        private int yDirectionTrend = 1;
        private int zDirectionTrend = 1;
        private readonly double stepSearchMinStepSize = 0.0001; // in mm, default is 0.1um 

        protected sbyte lossFailToImprove = 0;
        protected bool secondTry = false;
        protected static sbyte productCondition = 0;
        protected List<double> loss = new List<double>();
        protected List<double> pos = new List<double>();
        protected double lossCriteria;
        protected bool xyStepCountsLimit = false;
        protected bool xyStepGoBackToLast = true;
        protected double scanSearchRadius = 0.15; // in mm, default is 150um
        protected float zStepSizeAmp = 1.0f;
        protected float xyStepSizeAmp = 1.0f; // need to larger than 1
        protected double limitZ = 145;
        protected string zMode = "normal";
        protected double[] posCurrentMax = new double[6] { 0, 0, 138, 0, 0, 0 };
        protected bool doubleCheckFlag = false;
        protected bool stopInBetweenFlag = false;
        protected int spd = 400;

        protected Parameters parameters;
        protected BeetleControl beetleControl;
        protected PiezoControl piezoControl;

        public BeetleSearch(Parameters prmts, BeetleControl bc, PiezoControl pc)
        {
            parameters = prmts;
            beetleControl = bc;
            lossCriteria = parameters.lossCriteria;
            piezoControl = pc;
        }
        
        public void ProductSelect()
        {
            /* Add more if product extended
            *  productCondition has four types:
            *       = 1: SM + Large Ferrule Focal Length (> 0.1mm)
            *       = 2: SM + Small Ferrule Focal Length (< 0.1mm)
            *       = 3: MM + Large Ferrule Focal Length (> 0.1mm)
            *       = 4: MM + Small Ferrule Focal Length (< 0.1mm)
            *  all different products should be classified as one of those four types
            */
            if (parameters.productName == "VOA")
                productCondition = 2;
            else if (parameters.productName == "SM 1xN")
                productCondition = 1;
            else if (parameters.productName == "MM 1xN")
                productCondition = 3;
            else if (parameters.productName == "UWDM")
                productCondition = 2;
            else if (parameters.productName == "WOA")
                productCondition = 2;
            else
            {
                Console.WriteLine("Unsupported product");
                Parameters.Log("Unsupported product");
                parameters.errorFlag = true;
            }
        }

        // Square shape search in XY plane where the center is current position.
        // will return true is loss imporved for 5dB
        // this is good if xyscan search is failed and needs a larger area search
        protected bool XYSquareSearch(double singleRange = 0.05)
        {
            Console.WriteLine("Square Search Started");
            Parameters.Log("Square Search Started");
            double pX0 = parameters.position[0], pY0 = parameters.position[1];
            int countX0 = beetleControl.countsReal[0], countY0 = beetleControl.countsReal[1];
            double loss0 = PowerMeter.Read(); ;
            double[] px, py;
            double squareRadius;
            parameters.errorFlag = false;
            
            for (byte ii = 1; ii < 5; ii++)
            {
                squareRadius = ii * singleRange; 
                px = new double[5] { pX0 + squareRadius, pX0, pX0 - squareRadius, pX0, pX0 + squareRadius };
                py = new double[5] { pY0, pY0 + squareRadius, pY0, pY0 - squareRadius, pY0 };

                beetleControl.XMoveTo(px[0], stopInBetween: false, mode: 't', speed: 2000);

                Console.WriteLine($"****Square {ii} starts****");
                for (int i = 1; i < 5; i ++)
                {
                    Console.WriteLine($"Edge {i} starts");
                    beetleControl.XMoveTo(px[i], mode: 't', checkOnTarget: false, doubleCheck: false, stopInBetween: false, speed: spd);
                    beetleControl.YMoveTo(py[i], mode: 't', checkOnTarget: false, doubleCheck: false, stopInBetween: false, speed: spd);
                    while (Math.Abs(parameters.position[0] - px[i]) > 4 * beetleControl.encoderResolution && !parameters.errorFlag)
                    {
                        beetleControl.RealCountsFetch(0); // 0 is T1x, 1 is T1y
                        parameters.position[0] = pX0 + (beetleControl.countsReal[0] - countX0) * beetleControl.encoderResolution;
                        if (PowerMeter.Read() > loss0 + 5.0)
                        {
                            beetleControl.DisengageMotors();
                            // Update counts and position before exiting
                            beetleControl.RealCountsFetch(6);
                            parameters.position[0] = pX0 + (beetleControl.countsReal[0] - countX0) * beetleControl.encoderResolution;
                            parameters.position[1] = pY0 + (beetleControl.countsReal[1] - countY0) * beetleControl.encoderResolution;
                            return true;
                        }
                    }

                    if (parameters.errorFlag)
                        beetleControl.DisengageMotors();
                        
                    Thread.Sleep(100);
                    // Update counts and position for next movement
                    beetleControl.RealCountsFetch(6);
                    parameters.position[0] = pX0 + (beetleControl.countsReal[0] - countX0) * beetleControl.encoderResolution;
                    parameters.position[1] = pY0 + (beetleControl.countsReal[1] - countY0) * beetleControl.encoderResolution;

                    if (parameters.errorFlag)
                        return false;
                }
                Console.WriteLine($"****Square {ii} is done****");
            }

            return false;
        }

        // for axis: x is 0, y is 1, z is 2
        // radius in mm
        // starting from the current position and exit at the best position (if return true)
        // loss is updated at Parameters.loss and match current position (if return true)
        // scanLossTarget means when the loss reach this target, the scan will exit
        // limitationAxial means which axial will be bounded with limitation, if value is not 0~2 (xyz) then none axial will be bounded
        // TODO: Use threading to do scan and power fetch
        protected bool AxisScanSearch(sbyte axis, float scanLossTarget = -0.3f, sbyte limitationAxial = 2)
        {
            parameters.errorFlag = false;
            if (axis == 0)
            {
                Console.WriteLine("Scan Search X Started");
                Parameters.Log("Scan Search X Started");
            }
            else if (axis == 1)
            {
                Console.WriteLine("Scan Search Y Started");
                Parameters.Log("Scan Search Y Started");
            }
            else if (axis == 2)
            {
                Console.WriteLine("Scan Search Z Started");
                Parameters.Log("Scan Search Z Started");
            }
            double p1, p2, p0;
            int count0;
            loss.Clear();
            pos.Clear();
            double loss0;
            sbyte trend, unchange;
            double zScanSearchRadius = scanSearchRadius * 3;

            if (axis == 0)
            {
                p1 = parameters.position[axis] + scanSearchRadius * xDirectionTrend;
                p2 = parameters.position[axis] - scanSearchRadius * xDirectionTrend;
            }
            else if (axis == 1)
            {
                p1 = parameters.position[axis] + scanSearchRadius * yDirectionTrend;
                p2 = parameters.position[axis] - scanSearchRadius * yDirectionTrend;
            }
            else
            {
                p1 = parameters.position[axis] + zScanSearchRadius; // z scan always go plus direction first
                p2 = parameters.position[axis] - zScanSearchRadius;
            }
            if (axis == limitationAxial)
            { 
                p1 = p1 > limitZ ? limitZ - 0.002 : p1;
                p2 = p2 > limitZ ? limitZ - 0.002 : p2;
            }

            double[] p = new double[2] { p1, p2 };
            p0 = parameters.position[axis];
            // Z position is estimated through how much T1x has moved relative to its total range, and scale to Z total movement
            count0 = axis == 2 ? beetleControl.countsReal[0] : beetleControl.countsReal[axis];

            for (int i = 0; i < 2; i ++)
            {
                if (axis == 0)
                    beetleControl.XMoveTo(p[i], mode: 't', checkOnTarget: false, speed: spd);
                else if (axis == 1)
                    beetleControl.YMoveTo(p[i], mode: 't', checkOnTarget: false, speed: spd);
                else
                    beetleControl.ZMoveTo(p[i], mode: 't', checkOnTarget: false, speed: spd*3);

                // Active Monitor Loss and Position
                trend = 0;
                unchange = 0;
                loss.Clear();
                pos.Clear();
                loss0 = PowerMeter.Read();
                while (true)
                {
                    if (!(Math.Abs(parameters.position[axis] - p[i]) > beetleControl.tolerance * beetleControl.encoderResolution && !parameters.errorFlag))
                    {
                        trend = 1; // set as 1 so that it will not go to origin
                        Console.WriteLine("Exit Due to Error or Reach Search End");
                        Parameters.Log("Exit Due to Error or Reach Search End");
                        Console.WriteLine(parameters.errorFlag);
                        Console.WriteLine(parameters.position[axis]);
                        Console.WriteLine(p[i]);
                        break;
                    }

                    if (axis < 2)
                    {
                        beetleControl.RealCountsFetch(axis); // 0 is T1x, 1 is T1y
                        parameters.position[axis] = p0 + (beetleControl.countsReal[axis] - count0) * beetleControl.encoderResolution;
                    }
                    else
                    {
                        beetleControl.RealCountsFetch(0); // z uses T1x axial to guess position
                        // Z position is estimated through how much T1x has moved relative to its total range, and scale to Z total movement 
                        parameters.position[axis] = p0 + Math.Abs(p[i] - p0) * Math.Abs(beetleControl.countsReal[0] - count0) / beetleControl.zTrajT1xCountRange;
                    }
                    Console.WriteLine($"Pos: {Math.Round(parameters.position[axis], 4)}");
                    Parameters.Log($"Pos: {Math.Round(parameters.position[axis], 4)}");
                    pos.Add(parameters.position[axis]);
                    loss.Add(PowerMeter.Read());

                    // if scan loss is smaller than target, exit directly
                    if (loss[loss.Count - 1] > scanLossTarget)
                    {
                        trend = 1;
                        break;
                    }

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
                    if (unchange > 100)
                        break;

                    if (parameters.errorFlag)
                        return false;
                }
                beetleControl.RealCountsFetch(6); // update the countsReal for all axial, this is important for XMoveTo and YMoveTo with checkOnTarget to be false

                if (parameters.errorFlag)
                    return false;

                // if has max, go to the max position
                if (trend == 1)
                {
                    beetleControl.DisengageMotors();
                    Console.WriteLine("Has Max");
                    Parameters.Log("Has Max");
                    Thread.Sleep(500);
                    double Pr;
                    if (axis == 0)
                    {
                        Pr = pos[loss.IndexOf(loss.Max())];
                        beetleControl.XMoveTo(Pr, mode: 't', checkOnTarget: true, speed: 4000);
                        xDirectionTrend *= (-2 * i + 1);
                    }
                    else if (axis == 1)
                    {
                        Pr = pos[loss.IndexOf(loss.Max())];
                        beetleControl.YMoveTo(Pr, mode: 't', checkOnTarget: true, speed: 4000);
                        yDirectionTrend *= (-2 * i + 1);
                    }
                    else
                    {
                        Pr = pos[loss.IndexOf(loss.Max())];
                        beetleControl.ZMoveTo(Pr, mode: 't', checkOnTarget: false, speed: -1); // make spped as -1 to keep the speed so that they can move back as the way it moves forward
                    }

                    Console.WriteLine($"Goes to Pr: {Pr}");
                    Parameters.Log($"Goes to Pr: {Pr}");
                    // TODO: if running within this loop for sometime or loss keeps the same, exit directly 
                    while (Math.Abs(parameters.position[axis] - Pr) > 4 * beetleControl.tolerance * beetleControl.encoderResolution && !parameters.errorFlag)
                    {
                        if (axis < 2)
                        {
                            beetleControl.RealCountsFetch(axis); // 0 is T1x, 1 is T1y
                            parameters.position[axis] = p0 + (beetleControl.countsReal[axis] - count0) * beetleControl.encoderResolution;
                        }
                        else
                        {
                            beetleControl.RealCountsFetch(0); // z uses T1x axial to guess position
                            // Z position is estimated through how much T1x has moved relative to its total range, and scale to Z total movement 
                            parameters.position[axis] = p0 + Math.Abs(p[i] - p0) * Math.Abs(beetleControl.countsReal[0] - count0) / beetleControl.zTrajT1xCountRange;
                        }
                        Console.WriteLine($"Pos: {Math.Round(parameters.position[axis], 4)}");
                        Parameters.Log($"Pos: {Math.Round(parameters.position[axis], 4)}");
                        if (PowerMeter.Read() > loss.Max() - 0.02)
                        {
                            beetleControl.DisengageMotors();
                            Thread.Sleep(500);
                            break;
                        }
                    }
                    beetleControl.RealCountsFetch(6); // update the countsReal for all axial, this is important for XMoveTo and YMoveTo with checkOnTarget to be false
                    parameters.position[axis] = axis < 2 ? p0 + (beetleControl.countsReal[axis] - count0) * beetleControl.encoderResolution : 
                                                           p0 + Math.Abs(p[i] - p0) * Math.Abs(beetleControl.countsReal[0] - count0) / beetleControl.zTrajT1xCountRange;
                    StatusCheck(PowerMeter.Read());
                    return true;
                }
                // else return to origin and go the other direction
                else
                {
                    beetleControl.DisengageMotors(); // Disengage first to avoid unexpected issues
                    Console.WriteLine("Return to Original");
                    Parameters.Log("Return to Original");
                    // return to original position first
                    if (axis == 0)
                        beetleControl.XMoveTo(p0, doubleCheck: false, stopInBetween: stopInBetweenFlag, mode: 't', speed: 2000);
                    else if (axis == 1)
                        beetleControl.YMoveTo(p0, doubleCheck: false, stopInBetween: stopInBetweenFlag, mode: 't', speed: 2000);
                    else
                        beetleControl.ZMoveTo(p0, doubleCheck: false, stopInBetween: stopInBetweenFlag, mode: 't', speed: 2000);
                    Thread.Sleep(500);
                }
            }
            // if both direction failed to find max, return false and return to original pos
            StatusCheck(loss.Max());
            return false;
        }

        // for axis: x is 0, y is 1
        // step size is not based on loss, can be adjusted through and xyStepSizeAmp. 
        // starting from the current position and exit at the best position
        // loss is updated at Parameters.loss and match current position
        // return false only when loss unchanged
        protected bool AxisSteppingSearch(sbyte axis)
        {
            if (axis == 0)
            {
                Console.WriteLine("Stepping Search X Started");
                Parameters.Log("Stepping Search X Started");
            }
            else
            {
                Console.WriteLine("Stepping Search Y Started");
                Parameters.Log("Stepping Search Y Started");
            }
            loss.Clear();
            pos.Clear();
            double loss0, p = parameters.position[axis], bound, diff;
            sbyte trend = 1, sameCount = 0, totalStep = 0;
            loss.Add(PowerMeter.Read());
            pos.Add(parameters.position[axis]);
            loss0 = loss[loss.Count - 1];
            if (LossMeetCriteria())
                return true;
            while (!parameters.errorFlag)
            {
                totalStep += 1;
                if (xyStepCountsLimit && totalStep >= 4)
                {
                    Console.WriteLine("Reach step limit");
                    Parameters.Log("Reach step limit");
                    return true;
                }

                if (axis == 0)
                {
                    p = parameters.position[axis] + stepSearchMinStepSize * xyStepSizeAmp * xDirectionTrend;
                    beetleControl.XMoveTo(p, ignoreError: true, applyBacklash: true, doubleCheck: doubleCheckFlag, stopInBetween: stopInBetweenFlag);
                }
                else
                {
                    p = parameters.position[axis] + stepSearchMinStepSize * xyStepSizeAmp * yDirectionTrend;
                    beetleControl.YMoveTo(p, ignoreError: true, applyBacklash: true, doubleCheck: doubleCheckFlag, stopInBetween: stopInBetweenFlag);
                }
                // It's important to delay some time after disengaging motor to let the motor fully stopped, then fetch the loss.
                Thread.Sleep(150); // delay 150ms
                loss.Add(PowerMeter.Read());
                pos.Add(parameters.position[axis]);
                if (LossMeetCriteria())
                    return true;

                bound = XYLossBound(loss0);
                diff = loss[loss.Count - 1] - loss0;
                if (diff <= -bound)
                {
                    // Sometimes during curing, going back will make the loss worse because the best loss position 
                    // is drifting along the forwarding direction
                    if (!xyStepGoBackToLast && totalStep >= 2)
                        return true;
                    if (axis == 0)
                        p = parameters.position[axis] - stepSearchMinStepSize * xyStepSizeAmp * xDirectionTrend;
                    else
                        p = parameters.position[axis] - stepSearchMinStepSize * xyStepSizeAmp * yDirectionTrend;
                    trend -= 1;
                    if (trend != 0)
                    {
                        Console.WriteLine("Over");
                        Parameters.Log("Over");
                        break;
                    }
                    if (axis == 0)
                        xDirectionTrend = -xDirectionTrend;
                    else
                        yDirectionTrend = -yDirectionTrend;
                    loss0 = loss[loss.Count - 1];
                    Console.WriteLine("Change Direction");
                    Parameters.Log("Change Direction");
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
                            p = parameters.position[axis] - stepSearchMinStepSize * xyStepSizeAmp * xDirectionTrend * 2;
                        else
                            p = parameters.position[axis] - stepSearchMinStepSize * xyStepSizeAmp * yDirectionTrend * 2;
                        Console.WriteLine("Exit due to same loss");
                        Parameters.Log("Exit due to same loss");
                        break;
                    }
                }
            }

            if (parameters.errorFlag)
                return false;

            if (axis == 0)
                beetleControl.XMoveTo(p, ignoreError: true, applyBacklash: true, doubleCheck: doubleCheckFlag, stopInBetween: stopInBetweenFlag);
            else
                beetleControl.YMoveTo(p, ignoreError: true, applyBacklash: true, doubleCheck: doubleCheckFlag, stopInBetween: stopInBetweenFlag);
            // It's important to delay some time after disengaging motor to let the motor fully stopped, then fetch the loss.
            Thread.Sleep(150); // delay 150ms
            loss.Add(PowerMeter.Read());
            pos.Add(parameters.position[axis]);
            StatusCheck(loss.Max());
            // if unchanged, return false
            if (loss.Max() - loss.Min() < 0.002)
                return false;
            if (loss[loss.Count - 1] < (loss.Max() - 0.04))
            {
                Console.WriteLine("Failed to go to best");
                Parameters.Log("Failed to go to best");
            }
            return true;
        }

        // step size is based on loss value, can be adjusted through xyStepSizeAmp. 
        // starting from the current position and exit at the best position (if return true)
        // loss is updated at Parameters.loss and match current position (if return true)
        protected bool AxisInterpolationSearch(sbyte axis)
        {
            if (axis == 0)
            {
                Console.WriteLine("Interpolation Search X Started");
                Parameters.Log("Interpolation Search X Started");
            }
            else
            {
                Console.WriteLine("Interpolation Search Y Started");
                Parameters.Log("Interpolation Search Y Started");
            }
            loss.Clear();
            pos.Clear();
            List<double> pList;
            List<double> grid = new List<double>();
            double step, p0 = parameters.position[axis], pFinal;
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
            
            while (i < 6 && !parameters.errorFlag)
            {
                if (axis == 0)
                    beetleControl.XMoveTo(pList[i], ignoreError: true, applyBacklash: true, doubleCheck: doubleCheckFlag, stopInBetween: stopInBetweenFlag);
                else
                    beetleControl.YMoveTo(pList[i], ignoreError: true, applyBacklash: true, doubleCheck: doubleCheckFlag, stopInBetween: stopInBetweenFlag);
                Console.WriteLine($"X: {Math.Round(parameters.position[0], 4)}, Y: {Math.Round(parameters.position[1], 4)}");
                Parameters.Log($"X: {Math.Round(parameters.position[0], 4)}, Y: {Math.Round(parameters.position[1], 4)}");
                Thread.Sleep(150); // delay 150ms
                loss.Add(PowerMeter.Read());
                pos.Add(parameters.position[axis]);
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

            if (parameters.errorFlag)
                return false;

            if ((loss.Max() - loss.Min()) < 0.002)
            {
                Console.WriteLine("Unchange, go back to original");
                Parameters.Log("Unchange, go back to original");
                if (axis == 0)
                    beetleControl.XMoveTo(p0, ignoreError: true, applyBacklash: true, doubleCheck: doubleCheckFlag, stopInBetween: stopInBetweenFlag);
                else
                    beetleControl.YMoveTo(p0, ignoreError: true, applyBacklash: true, doubleCheck: doubleCheckFlag, stopInBetween: stopInBetweenFlag);
                Console.WriteLine($"X: {Math.Round(parameters.position[0], 4)}, Y: {Math.Round(parameters.position[1], 4)}");
                Parameters.Log($"X: {Math.Round(parameters.position[0], 4)}, Y: {Math.Round(parameters.position[1], 4)}");
                return false;
            }

            for (double g = pos.Min(); g <= pos.Max(); g += beetleControl.encoderResolution)
                grid.Add(g);
            double[] s = BarycentericInterpolation(pos, loss, grid);
            pFinal = grid[Array.IndexOf(s, s.Max())];
            if (Math.Abs(pFinal - pos[pos.Count - 1]) > beetleControl.encoderResolution)
            {
                if (axis == 0)
                    beetleControl.XMoveTo(pFinal, ignoreError: true, applyBacklash: true, doubleCheck: doubleCheckFlag, stopInBetween: stopInBetweenFlag);
                else
                    beetleControl.YMoveTo(pFinal, ignoreError: true, applyBacklash: true, doubleCheck: doubleCheckFlag, stopInBetween: stopInBetweenFlag);
                Console.WriteLine($"End X: {Math.Round(parameters.position[0], 4)}, End Y: {Math.Round(parameters.position[1],4)}");
                Parameters.Log($"End X: {Math.Round(parameters.position[0], 4)}, End Y: {Math.Round(parameters.position[1], 4)}");
                Thread.Sleep(150); // delay 150ms
                loss.Add(PowerMeter.Read());
                pos.Add(parameters.position[axis]);
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
            {
                Console.WriteLine("Failed to go to best");
                Parameters.Log("Failed to go to best");
            }
            return true;
        }

        // step size is based on loss value, can be adjusted through zStepSizeAmp. 
        // starting from the current position
        // loss is updated at Parameters.loss and match current position (if return true)
        // return false when reach Z limit the second time
        protected bool ZSteppingSearch(sbyte gapNarrowDirection = 1)
        {
            Console.WriteLine("Z Stepping Started");
            Parameters.Log("Z Stepping Started");
            double z = parameters.position[2], loss0, step, bound, diff;
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

            while (!parameters.errorFlag)
            {
                z += step * gapNarrowDirection;
                if ((z > limitZ && gapNarrowDirection == 1) || (z < limitZ && gapNarrowDirection == -1))
                {
                    if (secondTry)
                    {
                        parameters.errorFlag = true;
                        Console.WriteLine("Reach Limit Second Try Failed");
                        Parameters.Log("Reach Limit Second Try Failed");
                        parameters.errors = "Reach Z Limit, Move Closer\n";
                        return false;
                    }
                    secondTry = true;
                    lossFailToImprove = 0;

                    z -= (step + 0.07) * gapNarrowDirection;
                    Console.WriteLine("Reach Limit, Go Back 0.07 for second try");
                    Parameters.Log("Reach Limit, Go Back 0.07 for second try");
                    beetleControl.ZMoveTo(z, ignoreError: true, doubleCheck: doubleCheckFlag, stopInBetween: stopInBetweenFlag);
                    Console.WriteLine($"z: {Math.Round(z, 5)}");
                    Parameters.Log($"z: {Math.Round(z, 5)}");
                    break;
                }

                beetleControl.ZMoveTo(z, ignoreError: true, applyBacklash: true, doubleCheck: doubleCheckFlag, stopInBetween: stopInBetweenFlag);
                Thread.Sleep(150); // delay 150ms
                loss.Add(PowerMeter.Read());
                pos.Add(parameters.position[2]);
                Console.WriteLine($"z: {Math.Round(z, 5)}");
                Parameters.Log($"z: {Math.Round(z, 5)}");
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
                    z -= step * gapNarrowDirection;
                    // radio here should be smaller than 0.5 not 0,5, otherwise two steps will go back to the previous position
                    step *= 0.4;
                    // for large focal length products, if step size is smaller than 1 um, exit
                    if (step < 0.001 && (productCondition == 1 || productCondition == 3))
                    {
                        // don't go back, we want at least 1um forwarding
                        z += gapNarrowDirection * step / 0.4;
                        break;
                    }
                    if (successNum != 0)
                    {
                        // go back to the previous points
                        beetleControl.ZMoveTo(z, ignoreError: true, applyBacklash: true, doubleCheck: doubleCheckFlag, stopInBetween: stopInBetweenFlag);
                        Thread.Sleep(150); // delay 150ms
                        loss.Add(PowerMeter.Read());
                        pos.Add(parameters.position[2]);
                        Console.WriteLine($"Z position: {Math.Round(z, 5)}");
                        Parameters.Log($"Z position: {Math.Round(z, 5)}");
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

            Console.WriteLine($"Z ends at {Math.Round(z, 5)}");
            Parameters.Log($"Z ends at {Math.Round(z, 5)}");
            // set current pos as max loss position temporarily in order to update the max loss position in check_abnormal_loss function
            parameters.position[2] = pos[loss.IndexOf(loss.Max())];
            StatusCheck(loss.Max());
            // change current_pos to the correct one
            parameters.position[2] = z;

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
        protected virtual void StatusCheck(double loss0)
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
                    parameters.errors = "Unexpected High Loss";
                    Console.WriteLine("Unexpected High Loss");
                    Parameters.Log("Unexpected High Loss");
                    beetleControl.NormalTrajSpeed();
                    beetleControl.GotoReset();
                    parameters.errorFlag = true;
                }

                lossFailToImprove += 1;
                if (lossFailToImprove == 6)
                {
                    lossFailToImprove = 0;
                    parameters.errors = "Fail to find better Loss, Go to Best Position";
                    Console.WriteLine("Fail to find better Loss, Go to Best Position");
                    Parameters.Log("Fail to find better Loss, Go to Best Position");
                    parameters.errorFlag = true;
                    beetleControl.GotoPosition(posCurrentMax);
                }
            }
        }

        // In Python code is method is called loss_target_check
        protected virtual bool LossMeetCriteria()
        {
            if (PowerMeter.loss > lossCriteria)
            {
                parameters.errorFlag = true;
                return true;
            }
            return false;
        }

        protected virtual double LossBound(double lossRef)
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

        protected virtual double ZLossBound(double lossRef)
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

        protected virtual double XYLossBound(double lossRef)
        {
            if (productCondition <= 2)
                return LossBound(lossRef);
            else
                return LossBoundSmall(lossRef);
        }

        // *************************************************************
        // Piezo Search
        // *************************************************************
        // Need to reset the piezo to 0x800 for every run
        // for axis: x is 0, y is 1, z is 2
        protected bool PiezoSteppingSearch(byte axis, bool targetLess = false, bool forWOA = false)
        {
            if (axis == 0)
            {
                Console.WriteLine("Piezo Stepping Search X Started");
                Parameters.Log("Piezo Stepping Search X Started");
            }
            else if (axis == 1)
            {
                Console.WriteLine("Piezo Stepping Search Y Started");
                Parameters.Log("Piezo Stepping Search Y Started");
            }
            else
            {
                Console.WriteLine("Piezo Stepping Search Z Started");
                Parameters.Log("Piezo Stepping Search Z Started");
            }
            float zAmp = 2; // Z needs larger step size
            if (targetLess)
                zAmp = 1; // Z is the same step size not for curing
            loss.Clear();
            pos.Clear();
            double loss0, bound, diff;
            ushort p = parameters.piezoPosition[axis];
            sbyte trend = 1, sameCount = 0;
            bool directionChanged = false;
            loss.Add(PowerMeter.Read());
            pos.Add(parameters.piezoPosition[axis]);
            loss0 = loss[loss.Count - 1];
            if (!targetLess && LossMeetCriteria())
                return true;
            while (!parameters.errorFlag)
            {
                if (axis == 0)
                    p = (ushort)(parameters.piezoPosition[axis] + parameters.piezoStepSize * xDirectionTrend);
                else if (axis == 1)
                    p = (ushort)(parameters.piezoPosition[axis] + parameters.piezoStepSize * yDirectionTrend);
                else
                    p = (ushort)(parameters.piezoPosition[axis] + parameters.piezoStepSize * zAmp * zDirectionTrend);

                if (p > 0xff0 || p < 0x00f)
                {
                    Console.WriteLine("Reach Piezo Range Limit");
                    Parameters.Log("Reach Piezo Range Limit");
                    if (parameters.productName == "WOA")
                    {
                        if (axis == 0)
                            Compensate(axis, beetleAxis: 0, sameDirection: true);
                        else if (axis == 1)
                            Compensate(axis, beetleAxis: 1, sameDirection: false);
                        else if (axis == 2)
                            Compensate(axis, beetleAxis: 2, sameDirection: true);
                    }
                    else
                    {
                        if (axis == 0)
                            Compensate(axis, beetleAxis: 0, sameDirection: false);
                        else if (axis == 1)
                            Compensate(axis, beetleAxis: 1, sameDirection: true);
                        else if (axis == 2)
                            Compensate(axis, beetleAxis: 2, sameDirection: true);
                    }
                    // *************
                    continue;
                }

                piezoControl.Send(axis, p);

                Thread.Sleep(30); // delay 30ms
                loss.Add(PowerMeter.Read());
                pos.Add(parameters.piezoPosition[axis]);
                if (!targetLess && LossMeetCriteria())
                    return true;

                if (targetLess || forWOA)
                    bound = PiezoLossBound(loss0);
                else
                    bound = XYLossBound(loss0);
                diff = loss[loss.Count - 1] - loss0;
                if (diff <= -bound)
                {
                    if (axis == 0)
                        p = (ushort)(parameters.piezoPosition[axis] - parameters.piezoStepSize * xDirectionTrend);
                    else if (axis == 1)
                        p = (ushort)(parameters.piezoPosition[axis] - parameters.piezoStepSize * yDirectionTrend);
                    else
                        p = (ushort)(parameters.piezoPosition[axis] - parameters.piezoStepSize * zAmp * zDirectionTrend);
                    trend -= 1;
                    if (trend != 0 || directionChanged)
                    {
                        Console.WriteLine("Over");
                        Parameters.Log("Over");
                        break;
                    }
                    if (axis == 0)
                        xDirectionTrend = -xDirectionTrend;
                    else if (axis == 1)
                        yDirectionTrend = -yDirectionTrend;
                    else
                        zDirectionTrend = -zDirectionTrend;
                    loss0 = loss[loss.Count - 1];
                    Console.WriteLine("Change Direction");
                    Parameters.Log("Change Direction");
                    directionChanged = true;
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
                    trend = 1;
                    sameCount += 1;
                    // same losss for about 0.4um which is about 48 DAC value
                    if (!targetLess && sameCount >= 12)
                    {
                        if (axis == 0)
                            p = (ushort)(parameters.piezoPosition[axis] - parameters.piezoStepSize * xDirectionTrend * 6);
                        else if (axis == 1)
                            p = (ushort)(parameters.piezoPosition[axis] - parameters.piezoStepSize * yDirectionTrend * 6);
                        else
                            p = (ushort)(parameters.piezoPosition[axis] - parameters.piezoStepSize * zAmp * zDirectionTrend * 6);
                        Console.WriteLine("Exit Due to Same Loss");
                        Parameters.Log("Exit Due to Same Loss");
                        break;
                    }
                }
            }

            if (parameters.errorFlag)
                return false;

            piezoControl.Send(axis, p);
            // It's important to delay some time after disengaging motor to let the motor fully stopped, then fetch the loss.
            Thread.Sleep(40); // delay 40ms
            loss.Add(PowerMeter.Read());
            pos.Add(parameters.piezoPosition[axis]);
            if (!targetLess)
                StatusCheck(loss.Max());
            // if unchanged, return false
            if (!targetLess && (loss.Max() - loss.Min() < 0.002))
                return false;
            if (loss[loss.Count - 1] < (loss.Max() - 0.04))
            {
                Console.WriteLine("Failed to go to best");
                Parameters.Log("Failed to go to best");
            }
            return true;
        }

        protected virtual double PiezoLossBound(double lossRef)
        {
            if (productCondition <= 2)
            {
                lossRef = Math.Abs(lossRef);
                if (0.05 * lossRef > 0.4)
                    return 0.4;
                else if (lossRef < 3)
                    return 0.02;
                else
                    return 0.05 * lossRef;
            }
            else
                return 0.007;
        }

        // sameDirection means ADC value increase(the direction of arrow on the piezo), Beetle axial value increase too.
        // need to shift 1/3 of the full piezo range, which is about 50/3 = 16um
        private void Compensate(byte piezoAxis, byte beetleAxis, bool sameDirection)
        {
            piezoControl.Reset(piezoAxis);
            int directionTrend;
            float compRange = 0.014f;
            directionTrend = piezoAxis == 0 ? xDirectionTrend : piezoAxis == 1 ? yDirectionTrend : zDirectionTrend;
            if (beetleAxis == 0)
            {
                // is xDirectionTrend is 1, then piezoPosition is larger then 0xfff; if is -1, then piezoPosition is smaller than 0
                if (sameDirection)
                    beetleControl.XMoveTo(parameters.position[0] + compRange * directionTrend, mode: 't', speed: 2000);
                else
                    beetleControl.XMoveTo(parameters.position[0] - compRange * directionTrend, mode: 't', speed: 2000);
            }
            else if (beetleAxis == 1)
            {
                if (sameDirection)
                    beetleControl.YMoveTo(parameters.position[1] + compRange * directionTrend, mode: 't', speed: 2000);
                else
                    beetleControl.YMoveTo(parameters.position[1] - compRange * directionTrend, mode: 't', speed: 2000);
            }
            else if (beetleAxis == 2)
            {
                if (sameDirection)
                    beetleControl.ZMoveTo(parameters.position[2] + compRange * directionTrend, mode: 't', speed: 10000);
                else
                    beetleControl.ZMoveTo(parameters.position[2] - compRange * directionTrend, mode: 't', speed: 10000);
            }
        }

    }
}
