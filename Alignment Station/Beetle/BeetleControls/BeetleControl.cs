using System;
using System.Linq;
using System.Threading;
using System.IO.Ports;
using System.Windows.Forms;

namespace Beetle
{
    class BeetleControl
    {
        private Parameters parameters;
        private BeetleMathModel mathModel;

        private SerialPort T1Port;
        private SerialPort T2Port;
        private SerialPort T3Port;
        private int[] limit; //this limit is the max counts for each axis for the version where 0 count is at middle; or min for the version where 0 counts is at end  
        private double[] countsOffset;
        private readonly float rangePerAxis = 17.7f; //range in mm, default full range 17.7mm
        private int countsPerMM = 20000; // counts per mm
        private sbyte xDirectionOld = 0;
        private sbyte yDirectionOld = 0;
        private sbyte zDirectionOld = 0;
        private bool normalSpeedFlag = true;
        
        public sbyte[] onTargetFlag = new sbyte[6] { 0, 0, 0, 0, 0, 0 }; // it will control which motor to move, 1 means on target, 0 means not yet
        public byte globalErrorCount = 0;
        public sbyte[] motorEngageFlag = new sbyte[6] { 0, 0, 0, 0, 0, 0 };
        public double xBacklashMM = 0; // in mm
        public double yBacklashMM = 0; // in mm
        public double zBacklashMM = 0.0002; // in mm
        public sbyte tolerance = 2; // in encoder counts
        public double encoderResolution = 50e-6; // in mm/counts
        public int[] countsReal = new int[6] { 0, 0, 0, 0, 0, 0 }; // {T1x, T1y, T2x, T2y, T3x, T3y}, updates only at RealCountsFetch() or OnTarget()
        public int[] countsTarget = new int[6] { 0, 0, 0, 0, 0, 0 };
        public double[] tempP = new double[6] { 0, 0, 140, 0, 0, 0};
        public int zTrajT1xCountRange = 0;

        public BeetleControl(Parameters prmts, BeetleMathModel mm)
        {
            parameters = prmts;
            mathModel = mm;
            FixtureInit();
        }

        public void FixtureInit()
        { 
            int x1 = 185000, x2 = 185000, x3 = 185000, y1 = 185000, y2 = 185000, y3 = 185000;
            double A1x, A1y, A2x, A2y, A3x, A3y;
            //switch(parameters.beetleFixtureNumber)
            //{
            //    case 1:
            //        x1 = 193050;
            //        y1 = 187450;
            //        x2 = 192010;
            //        y2 = 189780;
            //        x3 = 183700;
            //        y3 = 187400;
            //        break;
            //    case 2:
            //        x1 = 192120;
            //        y1 = 187570;
            //        x2 = 186840;
            //        y2 = 187150;
            //        x3 = 183500;
            //        y3 = 183820;
            //        break;
            //    case 3:
            //        x1 = 188040;
            //        y1 = 183300;
            //        x2 = 180030;
            //        y2 = 183880;
            //        x3 = 185180;
            //        y3 = 184270;
            //        break;
            //    case 4:
            //        x1 = 188920;
            //        y1 = 188310;
            //        x2 = 184950;
            //        y2 = 181650;
            //        x3 = 183790;
            //        y3 = 183540;
            //        break;
            //    case 5:
            //        x1 = 188710;
            //        y1 = 180590;
            //        x2 = 180400;
            //        y2 = 187880;
            //        x3 = 182640;
            //        y3 = 180670;
            //        break;
            //    case 0:
            //        x1 = 188030;
            //        y1 = 180000;
            //        x2 = 183400;
            //        y2 = 190250;
            //        x3 = 194300;
            //        y3 = 179350;
            //        break;
            //}
            A1x = x1 - (-85.796144) / encoderResolution;
            A1y = y1 - 9.55 / encoderResolution;
            A2x = x2 + 38.123072 / encoderResolution;
            A2y = y2 - (-73.022182) / encoderResolution;
            A3x = x3 + 38.123072 / encoderResolution;
            A3y = y3 - 92.122182 / encoderResolution;

            limit = new int[] { x1, y1, x2, y2, x3, y3 };
            countsOffset = new double[] { A1x, A1y, A2x, A2y, A3x, A3y };
        }

        public bool Connection()
        {
            try
            {
                T1Port = new SerialPort(parameters.beetleT1ComPortName, 115200, Parity.None, 8, StopBits.One);
                T2Port = new SerialPort(parameters.beetleT2ComPortName, 115200, Parity.None, 8, StopBits.One);
                T3Port = new SerialPort(parameters.beetleT3ComPortName, 115200, Parity.None, 8, StopBits.One);
                T1Port.ReadTimeout = 200;
                T1Port.WriteTimeout = 200;
                T2Port.ReadTimeout = 200;
                T2Port.WriteTimeout = 200;
                T3Port.ReadTimeout = 200;
                T3Port.WriteTimeout = 200;
                T1Port.Open();
                T2Port.Open();
                T3Port.Open();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\nFailed to Connect Beetle Control Box");
                return false;
            }
        }

        public void ClosePorts()
        {
            if (T1Port != null)
                T1Port.Close();
            if (T2Port != null)
                T2Port.Close();
            if (T3Port != null)
                T3Port.Close();
        }

        public void OpenPorts()
        {
            if (T1Port != null)
                T1Port.Open();
            if (T2Port != null)
                T2Port.Open();
            if (T3Port != null)
                T3Port.Open();
        }

        public bool PortsIsOpen() => T1Port.IsOpen && T2Port.IsOpen && T3Port.IsOpen;

        public void ClearErrors()
        {
            parameters.errors = "";
            parameters.errorFlag = false;
            string xstr = "w axis0.error 0";
            string ystr = "w axis1.error 0";
            T123SendOnly(xstr, ystr);
            xstr = "w axis0.controller.error 0";
            ystr = "w axis1.controller.error 0";
            T123SendOnly(xstr, ystr);
            xstr = "w axis0.motor.error 0";
            ystr = "w axis1.motor.error 0";
            T123SendOnly(xstr, ystr);
            xstr = "w axis0.encoder.error 0";
            ystr = "w axis1.encoder.error 0";
            T123SendOnly(xstr, ystr);
        }

        public void Calibration()
        {
            ClearErrors();
            string xstr = "w axis0.requested_state 3";
            string ystr = "w axis1.requested_state 3";
            T123SendOnly(xstr, ystr);
            motorEngageFlag = new sbyte[6] { 1, 1, 1, 1, 1, 1 }; // indicating that the motor is running
            Thread.Sleep(15000);

            xstr = "r axis0.encoder.is_ready";
            ystr = "r axis1.encoder.is_ready";
            byte sum = 0;
            sum += byte.Parse(T1Talk(xstr));
            sum += byte.Parse(T2Talk(xstr));
            sum += byte.Parse(T3Talk(xstr));
            sum += byte.Parse(T1Talk(ystr));
            sum += byte.Parse(T2Talk(ystr));
            sum += byte.Parse(T3Talk(ystr));
            motorEngageFlag = new sbyte[6] { 0, 0, 0, 0, 0, 0 }; // indicating the motor is in idle
            if (sum != 6)
            {
                CheckErrors();
                MessageBox.Show("Calibratio Failed, Reset Motor Position and Try Again");
            }
            else
            {
                parameters.errorFlag = false;
                parameters.errors = "";
            }
        }

        // axial can be 0(all axis) or 0-5 axis
        public void EngageMotors(sbyte axial = 6)
        {
            //string xstr = "w axis0.requested_state 8";
            //string ystr = "w axis1.requested_state 8";
            string xstr = "j";
            string ystr = "k";
            if (axial == 0 && motorEngageFlag[0] == 0)
            {
                T1SendOnly(xstr);
                motorEngageFlag[0] = 1;
            }
            else if (axial == 1 && motorEngageFlag[1] == 0)
            {
                T1SendOnly(ystr);
                motorEngageFlag[1] = 1;
            }
            else if (axial == 2 && motorEngageFlag[2] == 0)
            {
                T2SendOnly(xstr);
                motorEngageFlag[2] = 1;
            }
            else if (axial == 3 && motorEngageFlag[3] == 0)
            {
                T2SendOnly(ystr);
                motorEngageFlag[3] = 1;
            }
            else if (axial == 4 && motorEngageFlag[4] == 0)
            {
                T3SendOnly(xstr);
                motorEngageFlag[4] = 1;
            }
            else if (axial == 5 && motorEngageFlag[5] == 0)
            {
                T3SendOnly(ystr);
                motorEngageFlag[5] = 1;
            }
            else if (axial == 6)
            { 
                T123SendOnly(xstr, ystr);
                motorEngageFlag = new sbyte[6] { 1, 1, 1, 1, 1, 1 };
            }
        }

        public void DisengageMotors()
        {
            //string xstr = "w axis0.requested_state 1";
            //string ystr = "w axis1.requested_state 1";
            string xstr = "m";
            string ystr = "n";
            if (motorEngageFlag[0] == 1)
            {
                T1SendOnly(xstr);
                motorEngageFlag[0] = 0;
            }
            if (motorEngageFlag[1] == 1)
            {
                T1SendOnly(ystr);
                motorEngageFlag[1] = 0;
            }
            if (motorEngageFlag[2] == 1)
            {
                T2SendOnly(xstr);
                motorEngageFlag[2] = 0;
            }
            if (motorEngageFlag[3] == 1)
            {
                T2SendOnly(ystr);
                motorEngageFlag[3] = 0;
            }
            if (motorEngageFlag[4] == 1)
            {
                T3SendOnly(xstr);
                motorEngageFlag[4] = 0;
            }
            if (motorEngageFlag[5] == 1)
            {
                T3SendOnly(ystr);
                motorEngageFlag[5] = 0;
            }
        }

        public void NormalTrajSpeed()
        {
            if (!normalSpeedFlag)
            {
                //string xstr1 = "w axis0.trap_traj.config.accel_limit 100000";
                //string xstr2 = "w axis0.trap_traj.config.decel_limit 100000";
                string xstr3 = "w axis0.trap_traj.config.vel_limit 100000";
                //string ystr1 = "w axis1.trap_traj.config.accel_limit 100000";
                //string ystr2 = "w axis1.trap_traj.config.decel_limit 100000";
                string ystr3 = "w axis1.trap_traj.config.vel_limit 100000";
                //T123SendOnly(xstr1, ystr1);
                //T123SendOnly(xstr2, ystr2);
                T123SendOnly(xstr3, ystr3);
                normalSpeedFlag = true;
            }
        }

        // axial: 0-5
        public void SetTrajSpeed(byte axial, int vel)
        {
            string cmd;
            if (axial == 0)
            {
                cmd = "w axis0.trap_traj.config.vel_limit " + vel.ToString();
                T1SendOnly(cmd);
            }
            else if (axial == 1)
            {
                cmd = "w axis1.trap_traj.config.vel_limit " + vel.ToString();
                T1SendOnly(cmd);
            }
            else if (axial == 2)
            {
                cmd = "w axis0.trap_traj.config.vel_limit " + vel.ToString();
                T2SendOnly(cmd);
            }
            else if (axial == 3)
            {
                cmd = "w axis1.trap_traj.config.vel_limit " + vel.ToString();
                T2SendOnly(cmd);
            }
            else if (axial == 4)
            {
                cmd = "w axis0.trap_traj.config.vel_limit " + vel.ToString();
                T3SendOnly(cmd);
            }
            else if (axial == 5)
            {
                cmd = "w axis1.trap_traj.config.vel_limit " + vel.ToString();
                T3SendOnly(cmd);
            }
        }

        // position is the platform position
        // will update Parameters.position
        // speed only works for mode 't'
        // mode has three type: 'p' means stepping; 't' means synchronize move, all motor stop at the same time; 'j' means trajectory move, motors don't need to stop at the same time
        public void GotoPosition(double[] position, bool stopInBetween = true, bool ignoreError = false, bool doubleCheck = false, char mode = 'p', bool checkOnTarget = true, int speed = 100000)
        {
            double[] Tmm = mathModel.FindAxialPosition(position[0], position[1], position[2], position[3], position[4], position[5]);
            int[] targetCounts = TranslateToCounts(Tmm);
            if (checkOnTarget)
                position.CopyTo(parameters.position, 0);
            GotoTargetCounts(targetCounts, freedom: 'a', mode: mode, doubleCheck: doubleCheck, stopInBetween: stopInBetween, ignoreError: ignoreError, checkOnTarget: checkOnTarget, speed: speed);
        }

        // XAbs is the platform x absolute position in mm
        // will update Parameters.position if checkOnTarget is true
        // Caution: Before running this function, countsReal and Parameters.Position need to be updated at current position for all 6 axial.  So if checkOnTarget
        // is false on a certain move, you have to update countsReal and Parameters.Position after this move
        public void XMoveTo(double XAbs, bool stopInBetween = true, bool ignoreError = false, bool applyBacklash = false, bool doubleCheck = false, char mode = 'p', bool checkOnTarget = true, int speed = 400)
        {
            sbyte xDirec;
            double counter = 0;
            if (XAbs > parameters.position[0])
                xDirec = 1;
            else if (XAbs < parameters.position[0])
                xDirec = -1;
            else
                xDirec = xDirectionOld;
            if (applyBacklash && xDirec != xDirectionOld)
                counter = xBacklashMM * xDirec;
            xDirectionOld = xDirec;

            int[] targetCounts = new int[6];
            countsReal.CopyTo(targetCounts, 0);
            int deltacounts = (int)Math.Round((XAbs + counter - parameters.position[0]) / encoderResolution);
            targetCounts[0] = countsReal[0] + deltacounts;
            targetCounts[2] = countsReal[2] - deltacounts;
            targetCounts[4] = countsReal[4] - deltacounts;

            if (checkOnTarget)
                parameters.position[0] = XAbs;

            GotoTargetCounts(targetCounts, freedom: 'x', mode: mode, doubleCheck: doubleCheck, stopInBetween: stopInBetween, ignoreError: ignoreError, checkOnTarget: checkOnTarget, speed: speed);
        }

        // YAbs is the platform y absolute position in mm
        // will update Parameters.position if checkOnTarget is true
        // Caution: Before running this function, countsReal and Parameters.Position need to be updated at current position for all 6 axial, so if checkOnTarget
        // is false on a certain move, you have to update countsReal and Parameters.Position after this move
        public void YMoveTo(double YAbs, bool stopInBetween = true, bool ignoreError = false, bool applyBacklash = false, bool doubleCheck = false, char mode = 'p', bool checkOnTarget = true, int speed = 400)
        {
            sbyte yDirec;
            double counter = 0;
            if (YAbs > parameters.position[1])
                yDirec = 1;
            else if (YAbs < parameters.position[1])
                yDirec = -1;
            else
                yDirec = yDirectionOld;
            if (applyBacklash && yDirec != yDirectionOld)
                counter = yBacklashMM * yDirec;
            yDirectionOld = yDirec;

            int[] targetCounts = new int[6];
            countsReal.CopyTo(targetCounts, 0);
            int deltacounts = (int)Math.Round((YAbs + counter - parameters.position[1]) / encoderResolution);
            targetCounts[1] = countsReal[1] + deltacounts;
            targetCounts[3] = countsReal[3] + deltacounts;
            targetCounts[5] = countsReal[5] + deltacounts;

            if (checkOnTarget)
                parameters.position[1] = YAbs;

            GotoTargetCounts(targetCounts, freedom: 'y', mode: mode, doubleCheck: doubleCheck, stopInBetween: stopInBetween, ignoreError: ignoreError, checkOnTarget: checkOnTarget, speed: speed);
        }

        // ZAbs is the platform z absolute position in mm
        // will update Parameters.position
        // if speed is smaller than 0 means to keep the speed as it is.
        public void ZMoveTo(double ZAbs, bool stopInBetween = true, bool ignoreError = false, bool applyBacklash = false, bool doubleCheck = false, char mode = 'p', bool checkOnTarget = true, bool normToFace = true, int speed = 400)
        {
            sbyte zDirec;
            double counter = 0;
            if (ZAbs > parameters.position[2])
                zDirec = 1;
            else if (ZAbs < parameters.position[2])
                zDirec = -1;
            else
                zDirec = zDirectionOld;
            if (applyBacklash && zDirec != zDirectionOld)
                counter = zBacklashMM * zDirec;
            zDirectionOld = zDirec;

            double[] targetPosition = new double[6];
            parameters.position.CopyTo(targetPosition, 0);
            targetPosition[2] = ZAbs + counter;

            // Move along Z direction that's norm to current platform face, instead of along world coordicate Z direciton
            // This will take angles into consideration
            if (normToFace)
            {
                double deltaZ = targetPosition[2] - parameters.position[2];
                double[] normVector = BeetleMathModel.NormalVector(parameters.position[3], parameters.position[4], parameters.position[5]);
                targetPosition[0] = deltaZ * normVector[0] + parameters.position[0];
                targetPosition[1] = deltaZ * normVector[1] + parameters.position[1];
                targetPosition[2] = deltaZ * normVector[2] + parameters.position[2];
            }

            GotoPosition(targetPosition, stopInBetween: stopInBetween, ignoreError: ignoreError, doubleCheck: doubleCheck, mode: mode, checkOnTarget: checkOnTarget, speed: speed);
        }

        // RxAbs is the absolute Rx value in degree
        // will update Parameters.position
        public void RxMoveTo(double RxAbs, bool stopInBetween = true, bool ignoreError = false, bool doubleCheck = false, char mode = 'p', bool checkOnTarget = true, int speed = 4000)
        {
            double[] targetPosition = new double[6];
            parameters.position.CopyTo(targetPosition, 0);
            targetPosition[3] = RxAbs;

            GotoPosition(targetPosition, stopInBetween: stopInBetween, ignoreError: ignoreError, doubleCheck: doubleCheck, mode: mode, checkOnTarget: checkOnTarget, speed: speed);
        }

        // RyAbs is the absolute Ry value in degree
        // will update Parameters.position
        public void RyMoveTo(double RyAbs, bool stopInBetween = true, bool ignoreError = false, bool doubleCheck = false, char mode = 'p', bool checkOnTarget = true, int speed = 4000)
        {
            double[] targetPosition = new double[6];
            parameters.position.CopyTo(targetPosition, 0);
            targetPosition[4] = RyAbs;

            GotoPosition(targetPosition, stopInBetween: stopInBetween, ignoreError: ignoreError, doubleCheck: doubleCheck, mode: mode, checkOnTarget: checkOnTarget, speed: speed);
        }

        // RzAbs is the absolute Rz value in degree
        // will update Parameters.position
        public void RzMoveTo(double RzAbs, bool stopInBetween = true, bool ignoreError = false, bool doubleCheck = false, char mode = 'p', bool checkOnTarget = true, int speed = 4000)
        {
            double[] targetPosition = new double[6];
            parameters.position.CopyTo(targetPosition, 0);
            targetPosition[5] = RzAbs;

            GotoPosition(targetPosition, stopInBetween: stopInBetween, ignoreError: ignoreError, doubleCheck: doubleCheck, mode: mode, checkOnTarget: checkOnTarget, speed: speed);
        }

        public void GotoReset() => GotoPosition(parameters.initialPosition, stopInBetween:true);

        public void GotoClose() => GotoTargetCounts(new int[6] { -1000, -1000, -1000, -1000, -1000, -1000 }, stopInBetween:true, speed: 10000);

        public void GotoTemp() => GotoPosition(tempP, stopInBetween: true);

        public void GotoTempSyn() => GotoPosition(tempP, stopInBetween: true, mode: 't', speed: 8000);

        public void GotoTempTraj() => GotoPosition(tempP, stopInBetween: true, mode: 'j', speed: 8000); // trajectory movement for all motors, doesn't need to stop at the same time

        // return false when timeout or driver board errors or out of range
        // freedom should be 'x' or 'y' or 'a', meaning sending counts in x or y or all freedom
        private bool GotoTargetCounts(int[] targetCounts, int speed, char freedom = 'a', bool stopInBetween = true, bool ignoreError = false, bool doubleCheck = false, char mode = 'p', bool checkOnTarget = true)
        {
            targetCounts.CopyTo(countsTarget, 0);
            if (!SafetyCheck(targetCounts))
            { 
                DisengageMotors();
                parameters.errorFlag = true;
                Console.WriteLine(parameters.errors);
                MessageBox.Show("Out of Range");
                return false;
            }
            int timeoutloop, timeout = 50;
            if (mode != 'p')
                timeout = 60; // traj mode time out is about 30s
            SetOnTargetFlag(freedom);
            // try three times on doublecheck
            for (int i = 0; i < 3; i++)
            {
                SendCounts(targetCounts, mode: mode, speed: speed);
                // timeout for about 5s
                timeoutloop = 0;
                while (checkOnTarget && timeoutloop < timeout && !parameters.errorFlag)
                {
                    if (mode == 't')
                    {
                        // come to here only when: 1. in Syn-Mode; 2. in XYZ scan search. 
                        Thread.Sleep(500);
                        // Purpose for the following code: 
                        // At least 3 motors will move and should stop at same time. Check onTargetFlag, if over 4 motors are in position the others are not, then
                        // errors may exist on those motors. So check the error here to break the loop earlier. 
                        sbyte sum = 0;
                        for (sbyte ii = 0; ii < 6; ii++)
                            sum += onTargetFlag[ii];
                        if (sum > 4)
                            CheckErrors();
                    }
                    else
                        Thread.Sleep(100);
                    if (OnTarget(targetCounts))
                        break;
                    timeoutloop++;
                }

                if (timeoutloop >= timeout - 1)
                {
                    Console.WriteLine("Time Out Error");
                    Parameters.Log("Time Out Error");
                    globalErrorCount += 1;
                    if (!ignoreError || globalErrorCount > 4)
                    {
                        Console.WriteLine("Exit Due to Time Out ");
                        Parameters.Log("Exit Due to Time Out");
                        DisengageMotors();
                        if (!CheckErrors())
                        {
                            for (int j = 0; j < 6; j++)
                            {
                                if (Math.Abs(targetCounts[i] - countsReal[i]) > tolerance)
                                    parameters.errors = string.Concat(parameters.errors, "Axis ", j + 1, " Timeout Error\n");
                            }
                        }
                        Console.WriteLine(parameters.errors);
                        Parameters.Log(parameters.errors);
                        parameters.errorFlag = true;
                        return false;
                    }
                }
                if (checkOnTarget && (doubleCheck || stopInBetween))
                {
                    DisengageMotors();
                    if (doubleCheck)
                    {
                        Thread.Sleep(100);
                        SetOnTargetFlag(freedom);
                        if (OnTarget(targetCounts))
                            return true;
                        else
                            continue;
                    }
                }
                return true;
            }
            return true;
        }

        private void SetOnTargetFlag(char freedom)
        {
            if (freedom == 'x')
            {
                onTargetFlag[0] = 0;
                onTargetFlag[2] = 0;
                onTargetFlag[4] = 0;
                onTargetFlag[1] = 1;
                onTargetFlag[3] = 1;
                onTargetFlag[5] = 1;
            }
            else if (freedom == 'y')
            {
                onTargetFlag[1] = 0;
                onTargetFlag[3] = 0;
                onTargetFlag[5] = 0;
                onTargetFlag[0] = 1;
                onTargetFlag[2] = 1;
                onTargetFlag[4] = 1;
            }
            else if (freedom == 'a')
            {
                onTargetFlag[1] = 0;
                onTargetFlag[3] = 0;
                onTargetFlag[5] = 0;
                onTargetFlag[0] = 0;
                onTargetFlag[2] = 0;
                onTargetFlag[4] = 0;
            }
        }

        private int[] TranslateToCounts(double[] Tmm)
        {
            int[] counts = { 0, 0, 0, 0, 0, 0};
            counts[0] = (int)Math.Round( Tmm[0] / encoderResolution + countsOffset[0]);
            counts[1] = (int)Math.Round( Tmm[1] / encoderResolution + countsOffset[1]);
            counts[2] = (int)Math.Round(-Tmm[2] / encoderResolution + countsOffset[2]);
            counts[3] = (int)Math.Round( Tmm[3] / encoderResolution + countsOffset[3]);
            counts[4] = (int)Math.Round(-Tmm[4] / encoderResolution + countsOffset[4]);
            counts[5] = (int)Math.Round( Tmm[5] / encoderResolution + countsOffset[5]);
            return counts;
        }

        // If the return is a number, it can be convert to int or float directly , no line feed sign
        // If the return is a description, print it out using WriteLine(), don't use Write()
        private string T1Talk(string str)
        {
            if (parameters.errorFlag)
                return "";
            string message = "";
            try
            { 
                T1Port.WriteLine(str);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message + "\nBeetle COM Port Failed");
                parameters.errorFlag = true;
                parameters.errors = "\nBeetle COM Port Failed";
                return "";
            }
            try
            {
                message = T1Port.ReadLine();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\nBeetle COM Port Failed");
                parameters.errorFlag = true;
                parameters.errors = "\nBeetle COM Port Failed";
            }
            return message;
        }

        // If the return is a number, it can be convert to int or float directly , no line feed sign
        // If the return is a description, print it out using WriteLine(), don't use Write()
        private string T2Talk(string str)
        {
            if (parameters.errorFlag)
                return "";
            string message = "";
            try
            {
                T2Port.WriteLine(str);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\nBeetle COM Port Failed");
                parameters.errorFlag = true;
                parameters.errors = "\nBeetle COM Port Failed";
                return "";
            }
            try
            {
                message = T2Port.ReadLine();
            }
            catch (Exception e)
            {
                message = "";
                MessageBox.Show(e.Message + "\nBeetle COM Port Failed");
                parameters.errorFlag = true;
                parameters.errors = "\nBeetle COM Port Failed";
            }
            return message;
        }

        // If the return is a number, it can be convert to int or float directly , no line feed sign
        // If the return is a description, print it out using WriteLine(), don't use Write()
        private string T3Talk(string str)
        {
            if (parameters.errorFlag)
                return "";
            string message = "";
            try
            {
                T3Port.WriteLine(str);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\nBeetle COM Port Failed");
                parameters.errorFlag = true;
                parameters.errors = "\nBeetle COM Port Failed";
                return "";
            }
            try
            {
                message = T3Port.ReadLine();
            }
            catch (Exception e)
            {
                message = "";
                MessageBox.Show(e.Message + "\nBeetle COM Port Failed");
                parameters.errorFlag = true;
                parameters.errors = "\nBeetle COM Port Failed";
            }
            return message;
        }

        private void T1SendOnly(string str)
        {
            if (parameters.errorFlag)
                return;
            try
            {
                T1Port.WriteLine(str);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\nBeetle COM Port Failed");
                parameters.errorFlag = true;
                parameters.errors = "\nBeetle COM Port Failed";
            }
        } 


        private void T2SendOnly(string str)
        {
            if (parameters.errorFlag)
                return;
            try
            {
                T2Port.WriteLine(str);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\nBeetle COM Port Failed");
                parameters.errorFlag = true;
                parameters.errors = "\nBeetle COM Port Failed";
            }
        }

        private void T3SendOnly(string str)
        {
            if (parameters.errorFlag)
                return;
            try
            {
                T3Port.WriteLine(str);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\nBeetle COM Port Failed");
                parameters.errorFlag = true;
                parameters.errors = "\nBeetle COM Port Failed";
            }
        }

        private void T123SendOnly(string xstr, string ystr)
        {
            T1SendOnly(xstr);
            T2SendOnly(xstr);
            T3SendOnly(xstr);
            T1SendOnly(ystr);
            T2SendOnly(ystr);
            T3SendOnly(ystr);
        }

        // freedom should be 'x' or 'y' or 'a', meaning sending counts in x or y or all freedom
        // mode can be 't' or 'p' meaning trajectory or stepping method
        // speed only works when mode is 't', if speed is smaller than 0 means to keep the speed as it is.
        private void SendCounts(int[] counts, int speed, char mode = 'p')
        {
            int trajectoryThreshold = 10000;
            string xstrp = "p 0 ";
            string xstrt = "t 0 ";
            string ystrp = "p 1 ";
            string ystrt = "t 1 ";
            string strpp = " 0 0";
            string cmd;

            int[] delta = new int[6] { Math.Abs(counts[0] - countsReal[0]), Math.Abs(counts[1] - countsReal[1]), Math.Abs(counts[2] - countsReal[2]),
                                        Math.Abs(counts[3] - countsReal[3]), Math.Abs(counts[4] - countsReal[4]), Math.Abs(counts[5] - countsReal[5])};
            // Change each axial's speed so that all axial can stop at the same time
            if (mode == 't' && speed > 0)
            {
                SpeedCalForTraj(delta, speed);
                zTrajT1xCountRange = delta[0]; // used to track position at Z traj scan mode
            }
            else if (!normalSpeedFlag && speed > 0)
                NormalTrajSpeed();

            // Engage motors as needed, only engage motors that need to move
            // Engage motor and send counts to this motor has to have some time delay in between
            // otherwise there will have strange bugs to come from Odrive
            // Because of this, we need to engage motors all together first then send counts
            for (sbyte i = 0; i < 6; i++)
            {
                if (motorEngageFlag[i] == 0 && onTargetFlag[i] == 0)
                    EngageMotors(i);
            }
            Thread.Sleep(100);

            if (onTargetFlag[0] == 0)
            {
                if ((delta[0] < trajectoryThreshold) && mode == 'p')
                    cmd = string.Concat(xstrp, counts[0], strpp);
                else
                    cmd = string.Concat(xstrt, counts[0]);
                T1SendOnly(cmd);
            }

            if (onTargetFlag[2] == 0)
            {
                if ((delta[2] < trajectoryThreshold) && mode == 'p')
                    cmd = string.Concat(xstrp, counts[2], strpp);
                else
                    cmd = string.Concat(xstrt, counts[2]);
                T2SendOnly(cmd);
            }

            if (onTargetFlag[4] == 0)
            {
                if ((delta[4] < trajectoryThreshold) && mode == 'p')
                    cmd = string.Concat(xstrp, counts[4], strpp);
                else
                    cmd = string.Concat(xstrt, counts[4]);
                T3SendOnly(cmd);
            }

            if (onTargetFlag[1] == 0)
            {
                if ((delta[1] < trajectoryThreshold) && mode == 'p')
                    cmd = string.Concat(ystrp, counts[1], strpp);
                else
                    cmd = string.Concat(ystrt, counts[1]);
                T1SendOnly(cmd);
            }

            if (onTargetFlag[3] == 0)
            {
                if ((delta[3] < trajectoryThreshold) && mode == 'p')
                    cmd = string.Concat(ystrp, counts[3], strpp);
                else
                    cmd = string.Concat(ystrt, counts[3]);
                T2SendOnly(cmd);
            }

            if (onTargetFlag[5] == 0)
            {
                if ((delta[5] < trajectoryThreshold) && mode == 'p')
                    cmd = string.Concat(ystrp, counts[5], strpp);
                else
                    cmd = string.Concat(ystrt, counts[5]);
                T3SendOnly(cmd);
            }
        }

        // Calculate the speed for each axial so that each axial can arrive target position at the same time
        // input is the delta counts for each axial from 0-5, speed is the max speed of six motors
        private void SpeedCalForTraj(int[] deltaCount, int speed)
        {
            int m = deltaCount.Max();
            m = m == 0 ? 1 : m;
            // Axial that needs to travel longest has 300 speed
            // Other axial will scale based on their relative deltacounts
            for (byte i = 0; i < 6; i++)
            {
                // if some axial don't need to move then don't change its speed
                if (deltaCount[i] > tolerance)
                    SetTrajSpeed(i, speed * deltaCount[i] / m);
            }
            normalSpeedFlag = false;
        }

        // Fetch each axis's real counts. axis = 6 fetch all 6 axis
        // 0 - 5 will fetch motor 1 - 6 individually
        public void RealCountsFetch(sbyte axis)
        {
            //string strx = "r axis0.encoder.shadow_count";
            //string stry = "r axis1.encoder.shadow_count";
            string strx = "d";
            string stry = "g";
            if (axis == 6)
            {
                try
                {
                    countsReal[0] = int.Parse(T1Talk(strx));
                    countsReal[1] = int.Parse(T1Talk(stry));
                    countsReal[2] = int.Parse(T2Talk(strx));
                    countsReal[3] = int.Parse(T2Talk(stry));
                    countsReal[4] = int.Parse(T3Talk(strx));
                    countsReal[5] = int.Parse(T3Talk(stry));
                }
                catch(Exception)
                {
                    countsReal[0] = 1000000;
                    countsReal[1] = 1000000;
                    countsReal[2] = 1000000;
                    countsReal[3] = 1000000;
                    countsReal[4] = 1000000;
                    countsReal[5] = 1000000;
                }
            }
            else if (axis == 0)
            {
                try
                { countsReal[0] = int.Parse(T1Talk(strx)); }
                catch (Exception)
                { countsReal[0] = 1000000; }
            }
            else if (axis == 1)
            {
                try
                { countsReal[1] = int.Parse(T1Talk(stry)); }
                catch(Exception)
                { countsReal[1] = 1000000; }
            }
            else if (axis == 2)
            {
                try
                { countsReal[2] = int.Parse(T2Talk(strx)); }
                catch (Exception)
                { countsReal[2] = 1000000; }
            }
            else if (axis == 3)
            {
                try
                { countsReal[3] = int.Parse(T2Talk(stry)); }
                catch (Exception)
                { countsReal[3] = 1000000; }
            }
            else if (axis == 4)
            {
                try
                { countsReal[4] = int.Parse(T3Talk(strx)); }
                catch (Exception)
                { countsReal[4] = 1000000; }
            }
            else if (axis == 5)
            {
                try
                { countsReal[5] = int.Parse(T3Talk(stry)); }
                catch (Exception)
                { countsReal[5] = 1000000; }
            }
        }

        // Check if x or y or all (freedom = 'x' or 'y' or 'a') motors are at target counts 
        private bool OnTarget(int[] counts)
        {
            for (sbyte i = 0; i < 6; i++)
            { 
                if (onTargetFlag[i] == 0)
                { 
                    RealCountsFetch(i);
                    if (countsReal[i] >= (counts[i] - tolerance) && countsReal[i] <= (counts[i] + tolerance))
                        onTargetFlag[i] = 1;
                }
            }
            sbyte sum = 0;
            for (sbyte i = 0; i < 6; i++)
                sum += onTargetFlag[i];
            if (sum == 6)
                return true;
            else
                return false;
        }

        // if error exists return true, else return false
        private bool CheckErrors()
        {
            string xstr = "r axis0.error";
            string ystr = "r axis1.error";
            int[] errorCodes = new int[6];
            errorCodes[0] = int.Parse(T1Talk(xstr));
            errorCodes[1] = int.Parse(T1Talk(ystr));
            errorCodes[2] = int.Parse(T2Talk(xstr));
            errorCodes[3] = int.Parse(T2Talk(ystr));
            errorCodes[4] = int.Parse(T3Talk(xstr));
            errorCodes[5] = int.Parse(T3Talk(ystr));
            if (errorCodes.Sum() != 0)
            {
                ErrorCodeExplain(errorCodes);
                //DisengageMotors();
                parameters.errorFlag = true;
                return true;
            }
            else
            {
                parameters.errors = "";
                parameters.errorFlag = false;
                return false;
            }
        }

        private void ErrorCodeExplain(int[] codes)
        {
            string xstr = "r axis0.";
            string ystr = "r axis1.";
            string str0;
            parameters.errors = "";
            for (int i = 0; i < 6; i++)
            {
                if (codes[i] == 0x_00)
                    continue;
                else if (codes[i] == 0x_40)
                {
                    if (i == 0 || i == 2 || i == 4)
                        str0 = string.Concat(xstr, "motor.error"); 
                    else
                        str0 = string.Concat(ystr, "motor.error"); 
                }
                else if (codes[i] == 0x_100)
                {
                    if (i == 0 || i == 2 || i == 4)
                        str0 = string.Concat(xstr, "encoder.error");
                    else
                        str0 = string.Concat(ystr, "encoder.error");
                }
                else if (codes[i] == 0x_200)
                {
                    if (i == 0 || i == 2 || i == 4)
                        str0 = string.Concat(xstr, "controller.error");
                    else
                        str0 = string.Concat(ystr, "controller.error");
                }
                else
                {
                    parameters.errors = string.Concat(parameters.errors, "Axis ", i + 1, " error: ", AxisErrorCode(codes[i]), "\n");
                    continue;
                }

                if (i <= 1)
                    codes[i] = int.Parse(T1Talk(str0));
                else if (i <= 3)
                    codes[i] = int.Parse(T2Talk(str0));
                else
                    codes[i] = int.Parse(T3Talk(str0));

                if (str0[8] == 'm')
                    parameters.errors = string.Concat(parameters.errors, "Motor ", i + 1, " error: ", MotorErrorCode(codes[i]), "\n");
                else if (str0[8] == 'e')
                    parameters.errors = string.Concat(parameters.errors, "Encoder ", i + 1, " error: ", EncoderErrorCode(codes[i]), "\n");
                else if (str0[8] == 'c')
                    parameters.errors = string.Concat(parameters.errors, "Controller ", i + 1, " error: ", ControllerErrorCode(codes[i]), "\n");
            }
        }

        private bool SafetyCheck(int[] counts)
        {
            parameters.errors = "";
            // for version where 0 counts is at middle
            if (limit[0] > 150000)
            {
                if (counts[0] > (limit[0] - 3000) || counts[0] < (limit[0] - rangePerAxis * countsPerMM))
                    parameters.errors = "T1x Out of Range\n";
                else if (counts[1] > (limit[1] - 3000) || counts[1] < (limit[1] - rangePerAxis * countsPerMM))
                    parameters.errors = "T1y Out of Range\n";
                else if (counts[2] > (limit[2] - 3000) || counts[2] < (limit[2] - rangePerAxis * countsPerMM))
                    parameters.errors = "T2x Out of Range\n";
                else if (counts[3] > (limit[3] - 3000) || counts[3] < (limit[3] - rangePerAxis * countsPerMM))
                    parameters.errors = "T2y Out of Range\n";
                else if (counts[4] > (limit[4] - 3000) || counts[4] < (limit[4] - rangePerAxis * countsPerMM))
                    parameters.errors = "T3x Out of Range\n";
                else if (counts[5] > (limit[5] - 3000) || counts[5] < (limit[5] - rangePerAxis * countsPerMM))
                    parameters.errors = "T3y Out of Range\n";
                else
                    return true;
                return false;
            }
            else // for version where 0 counts is at the end. Update: this version has been cancelled
            {
                if (counts[0] < (limit[0] + 3000) || counts[0] > (limit[0] + rangePerAxis * countsPerMM))
                    parameters.errors = "T1x Out of Range\n";
                else if (counts[1] < (limit[1] + 3000) || counts[1] > (limit[1] + rangePerAxis * countsPerMM))
                    parameters.errors = "T1y Out of Range\n";
                else if (counts[2] < (limit[2] + 3000) || counts[2] > (limit[2] + rangePerAxis * countsPerMM))
                    parameters.errors = "T2x Out of Range\n";
                else if (counts[3] < (limit[3] + 3000) || counts[3] > (limit[3] + rangePerAxis * countsPerMM))
                    parameters.errors = "T2y Out of Range\n";
                else if (counts[4] < (limit[4] + 3000) || counts[4] > (limit[4] + rangePerAxis * countsPerMM))
                    parameters.errors = "T3x Out of Range\n";
                else if (counts[5] < (limit[5] + 3000) || counts[5] > (limit[5] + rangePerAxis * countsPerMM))
                    parameters.errors = "T3y Out of Range\n";
                else
                    return true;
                return false;
            }
            
        }

        private static string AxisErrorCode(int code)
        {
            switch (code)
            {
                case 0x_00:
                    return "ERROR_NONE";
                case 0x_01:
                    return "ERROR_INVALID_STATE";
                case 0x_02:
                    return "ERROR_DC_BUS_UNDER_VOLTAGE";
                case 0x_04:
                    return "ERROR_DC_BUS_OVER_VOLTAGE";
                case 0x_08:
                    return "ERROR_CURRENT_MEASUREMENT_TIMEOUT";
                case 0x_10:
                    return "ERROR_BRAKE_RESISTOR_DISARMED";
                case 0x_20:
                    return "ERROR_MOTOR_DISARMED";
                case 0x_40:
                    return "ERROR_MOTOR_FAILED";
                case 0x_80:
                    return "ERROR_SENSORLESS_ESTIMATOR_FAILED";
                case 0x_100:
                    return "ERROR_ENCODER_FAILED";
                case 0x_200:
                    return "ERROR_CONTROLLER_FAILED";
                case 0x_400:
                    return "ERROR_POS_CTRL_DURING_SENSORLESS";
                case 0x_800:
                    return "ERROR_WATCHDOG_TIMER_EXPIRED";
                default:
                    return "INVALID CODE";
            }
        }

        private static string MotorErrorCode(int code)
        {
            switch (code)
            {
                case 0x_00:
                    return "ERROR_NONE";
                case 0x_01:
                    return "ERROR_PHASE_RESISTANCE_OUT_OF_RANGE";
                case 0x_02:
                    return "ERROR_PHASE_INDUCTANCE_OUT_OF_RANGE";
                case 0x_04:
                    return "ERROR_ADC_FAILED";
                case 0x_08:
                    return "ERROR_DRV_FAULT";
                case 0x_10:
                    return "ERROR_CONTROL_DEADLINE_MISSED";
                case 0x_20:
                    return "ERROR_NOT_IMPLEMENTED_MOTOR_TYPE";
                case 0x_40:
                    return "ERROR_BRAKE_CURRENT_OUT_OF_RANGE";
                case 0x_80:
                    return "ERROR_MODULATION_MAGNITUDE";
                case 0x_100:
                    return "ERROR_BRAKE_DEADTIME_VIOLATION";
                case 0x_200:
                    return "ERROR_UNEXPECTED_TIMER_CALLBACK";
                case 0x_400:
                    return "ERROR_CURRENT_SENSE_SATURATION";
                case 0x_800:
                    return "ERROR_INVERTER_OVER_TEMP";
                case 0x_1000:
                    return "ERROR_CURRENT_UNSTABLE";
                default:
                    return "INVALID CODE";
            }
        }

        private static string EncoderErrorCode(int code)
        {
            switch (code)
            {
                case 0x_00:
                    return "ERROR_NONE";
                case 0x_01:
                    return "ERROR_UNSTABLE_GAIN";
                case 0x_02:
                    return "ERROR_CPR_OUT_OF_RANGE";
                case 0x_04:
                    return "ERROR_NO_RESPONSE";
                case 0x_08:
                    return "ERROR_UNSUPPORTED_ENCODER_MODE";
                case 0x_10:
                    return "ERROR_ILLEGAL_HALL_STATE";
                case 0x_20:
                    return "ERROR_INDEX_NOT_FOUND_YET";
                default:
                    return "INVALID CODE";
            }
        }

        private static string ControllerErrorCode(int code)
        {
            switch (code)
            {
                case 0x_00:
                    return "ERROR_NONE";
                case 0x_01:
                    return "ERROR_OVERSPEED";
                default:
                    return "INVALID CODE";
            }
        }

    }
}
