using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO.Ports;
using System.Windows.Forms;

namespace Beetle
{
    static class BeetleControl
    {
        private static SerialPort T1Port;
        private static SerialPort T2Port;
        private static SerialPort T3Port;
        private static int[] limit; //this limit is the max counts for each axis for the version where 0 count is at middle; or min for the version where 0 counts is at end  
        private static double[] countsOffset;
        private static readonly float rangePerAxis = 17.7f; //range in mm, default full range 17.7mm
        private static int countsPerMM = 20000; // counts per mm
        private static sbyte xDirectionOld = 0;
        private static sbyte yDirectionOld = 0;
        private static sbyte zDirectionOld = 0;
        private static sbyte[] onTargetFlag = new sbyte[6] { 0, 0, 0, 0, 0, 0 }; // it will control which motor to move
        
        public static byte globalErrorCount = 0;
        public static sbyte[] motorEngageFlag = new sbyte[6] { 0, 0, 0, 0, 0, 0 };
        public static double xBacklashMM = 0; // in mm
        public static double yBacklashMM = 0; // in mm
        public static double zBacklashMM = 0.0002; // in mm
        public static sbyte tolerance = 2; // in encoder counts
        public static double encoderResolution = 50e-6; // in mm/counts
        public static int[] countsReal = new int[6] { 0, 0, 0, 0, 0, 0}; // {T1x, T1y, T2x, T2y, T3x, T3y}, updates only at RealCountsFetch() or OnTarget()
        public static double[] tempP;

        static BeetleControl()
        {
            int x1 = 183000, x2 = 183000, x3 = 183000, y1 = 183000, y2 = 183000, y3 = 183000;
            double A1x, A1y, A2x, A2y, A3x, A3y;
            switch(Parameters.beetleFixtureNumber)
            {
                case 1:
                    x1 = 193050;
                    y1 = 187450;
                    x2 = 192010;
                    y2 = 189780;
                    x3 = 183700;
                    y3 = 187400;
                    break;
                case 2:
                    x1 = 192120;
                    y1 = 187570;
                    x2 = 186840;
                    y2 = 187150;
                    x3 = 183500;
                    y3 = 183820;
                    break;
                case 3:
                    x1 = 188040;
                    y1 = 183300;
                    x2 = 180030;
                    y2 = 183880;
                    x3 = 185180;
                    y3 = 184270;
                    break;
                case 4:
                    x1 = 188920;
                    y1 = 188310;
                    x2 = 184950;
                    y2 = 181650;
                    x3 = 183790;
                    y3 = 183540;
                    break;
                case 5:
                    x1 = 188710;
                    y1 = 180590;
                    x2 = 180400;
                    y2 = 187880;
                    x3 = 182640;
                    y3 = 180670;
                    break;
                case 0:
                    x1 = 188030;
                    y1 = 180000;
                    x2 = 183400;
                    y2 = 190250;
                    x3 = 194300;
                    y3 = 179350;
                    break;
            }
            A1x = x1 - (-85.796144) / encoderResolution;
            A1y = y1 - 9.55 / encoderResolution;
            A2x = x2 + 38.123072 / encoderResolution;
            A2y = y2 - (-73.022182) / encoderResolution;
            A3x = x3 + 38.123072 / encoderResolution;
            A3y = y3 - 92.122182 / encoderResolution;

            limit = new int[] { x1, y1, x2, y2, x3, y3 };
            countsOffset = new double[] { A1x, A1y, A2x, A2y, A3x, A3y };
        }

        public static bool Connection()
        {
            try
            {
                T1Port = new SerialPort(Parameters.beetleT1ComPortName, 115200, Parity.None, 8, StopBits.One);
                T2Port = new SerialPort(Parameters.beetleT2ComPortName, 115200, Parity.None, 8, StopBits.One);
                T3Port = new SerialPort(Parameters.beetleT3ComPortName, 115200, Parity.None, 8, StopBits.One);
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

        public static void ClearErrors()
        {
            Parameters.errors = "";
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

        public static void Calibration()
        {
            string xstr = "w axis0.requested_state 3";
            string ystr = "w axis1.requested_state 3";
            T123SendOnly(xstr, ystr);
            motorEngageFlag = new sbyte[6] { 1, 1, 1, 1, 1, 1 }; // indicating that the motor is running
            Thread.Sleep(15000);
            CheckErrors();
            motorEngageFlag = new sbyte[6] { 0, 0, 0, 0, 0, 0 }; // indicating the motor is in idle
        }

        // axial can be 0(all axis) or 0-5 axis
        public static void EngageMotors(sbyte axial = 6)
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

        public static void DisengageMotors()
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

        public static void SlowTrajSpeed()
        {
            string xstr1 = "w axis0.trap_traj.config.accel_limit 1000";
            string xstr2 = "w axis0.trap_traj.config.decel_limit 1000";
            string xstr3 = "w axis0.trap_traj.config.vel_limit 600";
            string ystr1 = "w axis1.trap_traj.config.accel_limit 1000";
            string ystr2 = "w axis1.trap_traj.config.decel_limit 1000";
            string ystr3 = "w axis1.trap_traj.config.vel_limit 600";
            T123SendOnly(xstr1, ystr1);
            T123SendOnly(xstr2, ystr2);
            T123SendOnly(xstr3, ystr3);
        }

        public static void SlowTrajSpeed2()
        {
            string xstr1 = "w axis0.trap_traj.config.accel_limit 80";
            string xstr2 = "w axis0.trap_traj.config.decel_limit 80";
            string xstr3 = "w axis0.trap_traj.config.vel_limit 80";
            string ystr1 = "w axis1.trap_traj.config.accel_limit 80";
            string ystr2 = "w axis1.trap_traj.config.decel_limit 80";
            string ystr3 = "w axis1.trap_traj.config.vel_limit 80";
            T123SendOnly(xstr1, ystr1);
            T123SendOnly(xstr2, ystr2);
            T123SendOnly(xstr3, ystr3);
        }

        public static void NormalTrajSpeed()
        {
            string xstr1 = "w axis0.trap_traj.config.accel_limit 70000";
            string xstr2 = "w axis0.trap_traj.config.decel_limit 70000";
            string xstr3 = "w axis0.trap_traj.config.vel_limit 100000";
            string ystr1 = "w axis1.trap_traj.config.accel_limit 70000";
            string ystr2 = "w axis1.trap_traj.config.decel_limit 70000";
            string ystr3 = "w axis1.trap_traj.config.vel_limit 100000";
            T123SendOnly(xstr1, ystr1);
            T123SendOnly(xstr2, ystr2);
            T123SendOnly(xstr3, ystr3);
        }

        // position is the platform position
        // will update Parameters.position
        public static void GotoPosition(double[] position, bool stopInBetween = true, bool ignoreError = false, bool doubleCheck = false, char mode = 'p', bool checkOnTarget = true)
        {
            double[] Tmm = BeetleMathModel.FindAxialPosition(position[0], position[1], position[2], position[3], position[4], position[5]);
            int[] targetCounts = TranslateToCounts(Tmm);
            position.CopyTo(Parameters.position, 0);
            GotoTargetCounts(targetCounts, freedom: 'a', mode: mode, doubleCheck: doubleCheck, stopInBetween: stopInBetween, ignoreError: ignoreError, checkOnTarget: checkOnTarget);
        }

        // XAbs is the platform x absolute position in mm
        // will update Parameters.position if checkOnTarget is true
        // Caution: Before running this function, countsReal and Parameters.Position need to be updated at current position for all 6 axial. 
        // So if checkOnTarget is false on a certain move, you have to update countsReal and Parameters.Position after this move
        public static void XMoveTo(double XAbs, bool stopInBetween = true, bool ignoreError = false, bool applyBacklash = false, bool doubleCheck = false, char mode = 'p', bool checkOnTarget = true)
        {
            sbyte xDirec;
            double counter = 0;
            if (XAbs > Parameters.position[0])
                xDirec = 1;
            else if (XAbs < Parameters.position[0])
                xDirec = -1;
            else
                xDirec = xDirectionOld;
            if (applyBacklash && xDirec != xDirectionOld)
                counter = xBacklashMM * xDirec;
            xDirectionOld = xDirec;

            int[] targetCounts = new int[6];
            countsReal.CopyTo(targetCounts, 0);
            int deltacounts = (int)Math.Round((XAbs + counter - Parameters.position[0]) / encoderResolution);
            targetCounts[0] = countsReal[0] + deltacounts;
            targetCounts[2] = countsReal[2] - deltacounts;
            targetCounts[4] = countsReal[4] - deltacounts;

            if (checkOnTarget)
                Parameters.position[0] = XAbs;

            GotoTargetCounts(targetCounts, freedom: 'x', mode: mode, doubleCheck: doubleCheck, stopInBetween: stopInBetween, ignoreError: ignoreError, checkOnTarget: checkOnTarget);
        }

        // YAbs is the platform y absolute position in mm
        // will update Parameters.position if checkOnTarget is true
        // Caution: Before running this function, countsReal and Parameters.Position need to be updated at current position for all 6 axial
        // So if checkOnTarget is false on a certain move, you have to update countsReal and Parameters.Position after this move
        public static void YMoveTo(double YAbs, bool stopInBetween = true, bool ignoreError = false, bool applyBacklash = false, bool doubleCheck = false, char mode = 'p', bool checkOnTarget = true)
        {
            sbyte yDirec;
            double counter = 0;
            if (YAbs > Parameters.position[1])
                yDirec = 1;
            else if (YAbs < Parameters.position[1])
                yDirec = -1;
            else
                yDirec = yDirectionOld;
            if (applyBacklash && yDirec != yDirectionOld)
                counter = yBacklashMM * yDirec;
            yDirectionOld = yDirec;

            int[] targetCounts = new int[6];
            countsReal.CopyTo(targetCounts, 0);
            int deltacounts = (int)Math.Round((YAbs + counter - Parameters.position[1]) / encoderResolution);
            targetCounts[1] = countsReal[1] + deltacounts;
            targetCounts[3] = countsReal[3] + deltacounts;
            targetCounts[5] = countsReal[5] + deltacounts;

            if (checkOnTarget)
                Parameters.position[1] = YAbs;

            GotoTargetCounts(targetCounts, freedom: 'y', mode: mode, doubleCheck: doubleCheck, stopInBetween: stopInBetween, ignoreError: ignoreError, checkOnTarget: checkOnTarget);
        }

        // ZAbs is the platform z absolute position in mm
        // will update Parameters.position
        public static void ZMoveTo(double ZAbs, bool stopInBetween = true, bool ignoreError = false, bool applyBacklash = false, bool doubleCheck = false, char mode = 'p', bool checkOnTarget = true, bool normToFace = true)
        {
            sbyte zDirec;
            double counter = 0;
            if (ZAbs > Parameters.position[2])
                zDirec = 1;
            else if (ZAbs < Parameters.position[2])
                zDirec = -1;
            else
                zDirec = zDirectionOld;
            if (applyBacklash && zDirec != zDirectionOld)
                counter = zBacklashMM * zDirec;
            zDirectionOld = zDirec;

            double[] targetPosition = new double[6];
            Parameters.position.CopyTo(targetPosition, 0);
            targetPosition[2] = ZAbs + counter;

            // Move along Z direction that's norm to current platform face, instead of along world coordicate Z direciton
            // This will take angles into consideration
            if (normToFace)
            {
                double deltaZ = targetPosition[2] - Parameters.position[2];
                double[] normVector = BeetleMathModel.NormalVector(Parameters.position[3], Parameters.position[4], Parameters.position[5]);
                targetPosition[0] = deltaZ * normVector[0] + Parameters.position[0];
                targetPosition[1] = deltaZ * normVector[1] + Parameters.position[1];
                targetPosition[2] = deltaZ * normVector[2] + Parameters.position[2];
            }

            GotoPosition(targetPosition, stopInBetween: stopInBetween, ignoreError: ignoreError, doubleCheck: doubleCheck, mode: mode, checkOnTarget: checkOnTarget);
        }

        // RxAbs is the absolute Rx value in degree
        // will update Parameters.position
        public static void RxMoveTo(double RxAbs, bool stopInBetween = true, bool ignoreError = false, bool doubleCheck = false, char mode = 'p', bool checkOnTarget = true)
        {
            double[] targetPosition = new double[6];
            Parameters.position.CopyTo(targetPosition, 0);
            targetPosition[3] = RxAbs;

            GotoPosition(targetPosition, stopInBetween: stopInBetween, ignoreError: ignoreError, doubleCheck: doubleCheck, mode: mode, checkOnTarget: checkOnTarget);
        }

        // RyAbs is the absolute Ry value in degree
        // will update Parameters.position
        public static void RyMoveTo(double RyAbs, bool stopInBetween = true, bool ignoreError = false, bool doubleCheck = false, char mode = 'p', bool checkOnTarget = true)
        {
            double[] targetPosition = new double[6];
            Parameters.position.CopyTo(targetPosition, 0);
            targetPosition[4] = RyAbs;

            GotoPosition(targetPosition, stopInBetween: stopInBetween, ignoreError: ignoreError, doubleCheck: doubleCheck, mode: mode, checkOnTarget: checkOnTarget);
        }

        // RzAbs is the absolute Rz value in degree
        // will update Parameters.position
        public static void RzMoveTo(double RzAbs, bool stopInBetween = true, bool ignoreError = false, bool doubleCheck = false, char mode = 'p', bool checkOnTarget = true)
        {
            double[] targetPosition = new double[6];
            Parameters.position.CopyTo(targetPosition, 0);
            targetPosition[5] = RzAbs;

            GotoPosition(targetPosition, stopInBetween: stopInBetween, ignoreError: ignoreError, doubleCheck: doubleCheck, mode: mode, checkOnTarget: checkOnTarget);
        }

        public static void GotoReset() => GotoPosition(Parameters.initialPosition, stopInBetween:true);

        public static void GotoClose() => GotoTargetCounts(new int[6] { -1000, -1000, -1000, -1000, -1000, -1000 }, stopInBetween:true);

        public static void GotoTemp() => GotoPosition(tempP, stopInBetween: true);

        // return false when timeout or driver board errors or out of range
        // freedom should be 'x' or 'y' or 'a', meaning sending counts in x or y or all freedom
        private static bool GotoTargetCounts(int[] targetCounts, char freedom = 'a', bool stopInBetween = true, bool ignoreError = false, bool doubleCheck = false, char mode = 'p', bool checkOnTarget = true)
        {
            if (!SafetyCheck(targetCounts))
            { 
                DisengageMotors();
                Parameters.errors = "Out of Range\n";
                Parameters.errorFlag = true;
                Console.WriteLine(Parameters.errors);
                return false;
            }
            int timeoutloop, timeout = 50;
            if (mode == 't')
                timeout = 100;
            SetOnTargetFlag(freedom);
            // try three times on doublecheck
            for (int i = 0; i < 3; i++)
            {
                SendCounts(targetCounts, mode: mode);
                // timeout for about 5s
                timeoutloop = 0;
                while (checkOnTarget && timeoutloop < timeout)
                {
                    Thread.Sleep(100);
                    if (OnTarget(targetCounts))
                        break;
                    timeoutloop++;
                }

                if (timeoutloop >= 49)
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
                                    Parameters.errors = string.Concat(Parameters.errors, "Axis ", j + 1, " Timeout Error\n");
                            }
                        }
                        Console.WriteLine(Parameters.errors);
                        Parameters.Log(Parameters.errors);
                        Parameters.errorFlag = true;
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

        private static void SetOnTargetFlag(char freedom)
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

        private static int[] TranslateToCounts(double[] Tmm)
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
        private static string T1Talk(string str)
        {
            string message;
            try
            { 
                T1Port.WriteLine(str);
            }
            catch(TimeoutException)
            {
                Console.WriteLine("T1 Serial Write Timeout");
            }
            try
            {
                message = T1Port.ReadLine();
            }
            catch (TimeoutException)
            {
                message = "";
                Console.WriteLine("T1 Serial Read Timeout");
            }
            return message;
        }

        // If the return is a number, it can be convert to int or float directly , no line feed sign
        // If the return is a description, print it out using WriteLine(), don't use Write()
        private static string T2Talk(string str)
        {
            string message;
            try
            {
                T2Port.WriteLine(str);
            }
            catch (TimeoutException)
            {
                Console.WriteLine("T2 Serial Write Timeout");
            }
            try
            {
                message = T2Port.ReadLine();
            }
            catch (TimeoutException)
            {
                message = "";
                Console.WriteLine("T2 Serial Read Timeout");
            }
            return message;
        }

        // If the return is a number, it can be convert to int or float directly , no line feed sign
        // If the return is a description, print it out using WriteLine(), don't use Write()
        private static string T3Talk(string str)
        {
            string message;
            try
            {
                T3Port.WriteLine(str);
            }
            catch (TimeoutException)
            {
                Console.WriteLine("T3 Serial Write Timeout");
            }
            try
            {
                message = T3Port.ReadLine();
            }
            catch (TimeoutException)
            {
                message = "";
                Console.WriteLine("T3 Serial Read Timeout");
            }
            return message;
        }

        private static void T1SendOnly(string str)
        {
            try
            {
                T1Port.WriteLine(str);
            }
            catch (TimeoutException)
            {
                Console.WriteLine("T1 Serial Write Timeout");
            }
        } 


        private static void T2SendOnly(string str)
        {
            try
            {
                T2Port.WriteLine(str);
            }
            catch (TimeoutException)
            {
                Console.WriteLine("T2 Serial Write Timeout");
            }
        }

        private static void T3SendOnly(string str)
        {
            try
            {
                T3Port.WriteLine(str);
            }
            catch (TimeoutException)
            {
                Console.WriteLine("T3 Serial Write Timeout");
            }
        }

        private static void T123SendOnly(string xstr, string ystr)
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
        private static void SendCounts(int[] counts, char mode = 'p')
        {
            int trajectoryThreshold = 10000;
            string xstrp = "p 0 ";
            string xstrt = "t 0 ";
            string ystrp = "p 1 ";
            string ystrt = "t 1 ";
            string strpp = " 0 0";
            string cmd;

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
                if ((Math.Abs(counts[0] - countsReal[0]) < trajectoryThreshold) && mode == 'p')
                    cmd = string.Concat(xstrp, counts[0], strpp);
                else
                    cmd = string.Concat(xstrt, counts[0]);
                T1SendOnly(cmd);
            }

            if (onTargetFlag[2] == 0)
            {
                if ((Math.Abs(counts[2] - countsReal[2]) < trajectoryThreshold) && mode == 'p')
                    cmd = string.Concat(xstrp, counts[2], strpp);
                else
                    cmd = string.Concat(xstrt, counts[2]);
                T2SendOnly(cmd);
            }

            if (onTargetFlag[4] == 0)
            {
                if ((Math.Abs(counts[4] - countsReal[4]) < trajectoryThreshold) && mode == 'p')
                    cmd = string.Concat(xstrp, counts[4], strpp);
                else
                    cmd = string.Concat(xstrt, counts[4]);
                T3SendOnly(cmd);
            }

            if (onTargetFlag[1] == 0)
            {
                if ((Math.Abs(counts[1] - countsReal[1]) < trajectoryThreshold) && mode == 'p')
                    cmd = string.Concat(ystrp, counts[1], strpp);
                else
                    cmd = string.Concat(ystrt, counts[1]);
                T1SendOnly(cmd);
            }

            if (onTargetFlag[3] == 0)
            {
                if ((Math.Abs(counts[3] - countsReal[3]) < trajectoryThreshold) && mode == 'p')
                    cmd = string.Concat(ystrp, counts[3], strpp);
                else
                    cmd = string.Concat(ystrt, counts[3]);
                T2SendOnly(cmd);
            }

            if (onTargetFlag[5] == 0)
            {
                if ((Math.Abs(counts[5] - countsReal[5]) < trajectoryThreshold) && mode == 'p')
                    cmd = string.Concat(ystrp, counts[5], strpp);
                else
                    cmd = string.Concat(ystrt, counts[5]);
                T3SendOnly(cmd);
            }
        }

        // Fetch each axis's real counts. axis = 6 fetch all 6 axis
        // 0 - 5 will fetch motor 1 - 6 individually
        public static void RealCountsFetch(sbyte axis)
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
        private static bool OnTarget(int[] counts)
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
        private static bool CheckErrors()
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
                Parameters.errorFlag = true;
                return true;
            }
            else
            {
                Parameters.errors = "";
                Parameters.errorFlag = false;
                return false;
            }
        }

        private static void ErrorCodeExplain(int[] codes)
        {
            string xstr = "r axis0.";
            string ystr = "r axis1.";
            string str0;
            Parameters.errors = "";
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
                    Parameters.errors = string.Concat(Parameters.errors, "Axis ", i + 1, " error: ", AxisErrorCode(codes[i]), "\n");
                    continue;
                }

                if (i <= 1)
                    codes[i] = int.Parse(T1Talk(str0));
                else if (i <= 3)
                    codes[i] = int.Parse(T2Talk(str0));
                else
                    codes[i] = int.Parse(T3Talk(str0));

                if (str0[8] == 'm')
                    Parameters.errors = string.Concat(Parameters.errors, "Motor ", i + 1, " error: ", MotorErrorCode(codes[i]), "\n");
                else if (str0[8] == 'e')
                    Parameters.errors = string.Concat(Parameters.errors, "Encoder ", i + 1, " error: ", EncoderErrorCode(codes[i]), "\n");
                else if (str0[8] == 'c')
                    Parameters.errors = string.Concat(Parameters.errors, "Controller ", i + 1, " error: ", ControllerErrorCode(codes[i]), "\n");
            }
        }

        private static bool SafetyCheck(int[] counts)
        {
            Parameters.errors = "";
            // for version where 0 counts is at middle
            if (limit[0] > 150000)
            {
                if (counts[0] > (limit[0] - 3000) || counts[0] < (limit[0] - rangePerAxis * countsPerMM))
                    Parameters.errors = "T1x Out of Range\n";
                else if (counts[1] > (limit[1] - 3000) || counts[1] < (limit[1] - rangePerAxis * countsPerMM))
                    Parameters.errors = "T1y Out of Range\n";
                else if (counts[2] > (limit[2] - 3000) || counts[2] < (limit[2] - rangePerAxis * countsPerMM))
                    Parameters.errors = "T2x Out of Range\n";
                else if (counts[3] > (limit[3] - 3000) || counts[3] < (limit[3] - rangePerAxis * countsPerMM))
                    Parameters.errors = "T2y Out of Range\n";
                else if (counts[4] > (limit[4] - 3000) || counts[4] < (limit[4] - rangePerAxis * countsPerMM))
                    Parameters.errors = "T3x Out of Range\n";
                else if (counts[5] > (limit[5] - 3000) || counts[5] < (limit[5] - rangePerAxis * countsPerMM))
                    Parameters.errors = "T3y Out of Range\n";
                else
                    return true;
                return false;
            }
            else // for version where 0 counts is at the end
            {
                if (counts[0] < (limit[0] + 3000) || counts[0] > (limit[0] + rangePerAxis * countsPerMM))
                    Parameters.errors = "T1x Out of Range\n";
                else if (counts[1] < (limit[1] + 3000) || counts[1] > (limit[1] + rangePerAxis * countsPerMM))
                    Parameters.errors = "T1y Out of Range\n";
                else if (counts[2] < (limit[2] + 3000) || counts[2] > (limit[2] + rangePerAxis * countsPerMM))
                    Parameters.errors = "T2x Out of Range\n";
                else if (counts[3] < (limit[3] + 3000) || counts[3] > (limit[3] + rangePerAxis * countsPerMM))
                    Parameters.errors = "T2y Out of Range\n";
                else if (counts[4] < (limit[4] + 3000) || counts[4] > (limit[4] + rangePerAxis * countsPerMM))
                    Parameters.errors = "T3x Out of Range\n";
                else if (counts[5] < (limit[5] + 3000) || counts[5] > (limit[5] + rangePerAxis * countsPerMM))
                    Parameters.errors = "T3y Out of Range\n";
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
