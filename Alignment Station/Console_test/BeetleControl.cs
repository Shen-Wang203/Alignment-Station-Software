using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO.Ports;

namespace Console_test
{
    class BeetleControl
    {
        private static SerialPort T1Port;
        private static SerialPort T2Port;
        private static SerialPort T3Port;
        private static int[] limit; //this limit is the max counts for each axis for the version where 0 count is at middle; or min for the version where 0 counts is at end  
        private static double[] countsOffset;
        private static float rangePerAxis = 17.7f; //range in mm, default full range 17.7mm
        private static int countsPerMM = 20000; // counts per mm
        private static sbyte xDirectionOld = 0;
        private static sbyte yDirectionOld = 0;
        private static sbyte zDirectionOld = 0;
        private static bool motorEngaged = false;
        
        public static double xBacklashMM = 0; // in mm
        public static double yBacklashMM = 0; // in mm
        public static double zBacklashMM = 0.0002; // in mm
        public static sbyte tolerance = 2; // in encoder counts
        public static double encoderResolution = 50e-6; // in mm/counts
        public static int[] countsReal = new int[6] { 0, 0, 0, 0, 0, 0}; // {T1x, T1y, T2x, T2y, T3x, T3y}, updates only at RealCountsFetch() or OnTarget()
        public static int[] countsOld = new int[6] { 0, 0, 0, 0, 0, 0 }; // {T1x, T1y, T2x, T2y, T3x, T3y}, updates only at SendCounts() or GotoTargetCounts()
        public static double[] resetPosition = new double[6] { 0, 0, 138, 0, 0, 0 }; // This is the starting position

        public BeetleControl()
        {
            try
            {
                T1Port = new SerialPort(GlobalVar.beetleT1ComPortName, 115200, Parity.None, 8, StopBits.One);
                T2Port = new SerialPort(GlobalVar.beetleT2ComPortName, 115200, Parity.None, 8, StopBits.One);
                T3Port = new SerialPort(GlobalVar.beetleT3ComPortName, 115200, Parity.None, 8, StopBits.One);
                T1Port.ReadTimeout = 200;
                T1Port.WriteTimeout = 200;
                T2Port.ReadTimeout = 200;
                T2Port.WriteTimeout = 200;
                T3Port.ReadTimeout = 200;
                T3Port.WriteTimeout = 200;
                T1Port.Open();
                T2Port.Open();
                T3Port.Open();
            }
            catch (Exception)
            {
                //TODO: message box
                Console.WriteLine("Serial Connection Error");
            }

            int x1 = 183000, x2 = 183000, x3 = 183000, y1 = 183000, y2 = 183000, y3 = 183000;
            double A1x, A1y, A2x, A2y, A3x, A3y;
            switch(GlobalVar.beetleFixtureNumber)
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

        public static void ClearErrors()
        {
            GlobalVar.errors = "";
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
        }

        public static void EngageMotors()
        {
            string xstr = "w axis0.requested_state 8";
            string ystr = "w axis1.requested_state 8";
            T123SendOnly(xstr, ystr);
        }

        public static void DisengageMotors()
        {
            string xstr = "w axis0.requested_state 1";
            string ystr = "w axis1.requested_state 1";
            T123SendOnly(xstr, ystr);
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
        // will update GlobalVar.position
        public bool GotoPosition(double[] position, bool stopInBetween = true, bool ignoreError = false, bool doubleCheck = false, char mode = 'p', bool checkOnTarget = true)
        {
            double[] Tmm = BeetleMathModel.FindAxialPosition(position[0], position[1], position[2], position[3], position[4], position[5]);
            int[] targetCounts = TranslateToCounts(Tmm);
            position.CopyTo(GlobalVar.position, 0);
            return GotoTargetCounts(targetCounts, freedom: 'a', mode: mode, doubleCheck: doubleCheck, stopInBetween: stopInBetween, ignoreError: ignoreError, checkOnTarget: checkOnTarget);
        }

        // XAbs is the platform x absolute position in mm
        // will update GlobalVar.position
        public bool XMoveTo(double XAbs, bool stopInBetween = true, bool ignoreError = false, bool applyBacklash = false, bool doubleCheck = false, char mode = 'p', bool checkOnTarget = true)
        {
            sbyte xDirec;
            double counter = 0;
            if (XAbs > GlobalVar.position[0])
                xDirec = 1;
            else if (XAbs < GlobalVar.position[0])
                xDirec = -1;
            else
                xDirec = xDirectionOld;
            if (applyBacklash && xDirec != xDirectionOld)
                counter = xBacklashMM * xDirec;
            xDirectionOld = xDirec;

            int[] targetCounts = new int[6];
            countsOld.CopyTo(targetCounts, 0);
            int deltacounts = (int)Math.Round((XAbs + counter - GlobalVar.position[0]) / encoderResolution);
            targetCounts[0] = countsOld[0] + deltacounts;
            targetCounts[2] = countsOld[2] - deltacounts;
            targetCounts[4] = countsOld[4] - deltacounts;

            GlobalVar.position[0] = XAbs;

            return GotoTargetCounts(targetCounts, freedom: 'x', mode: mode, doubleCheck: doubleCheck, stopInBetween: stopInBetween, ignoreError: ignoreError, checkOnTarget: checkOnTarget);
        }

        // YAbs is the platform y absolute position in mm
        // will update GlobalVar.position
        public bool YMoveTo(double YAbs, bool stopInBetween = true, bool ignoreError = false, bool applyBacklash = false, bool doubleCheck = false, char mode = 'p', bool checkOnTarget = true)
        {
            sbyte yDirec;
            double counter = 0;
            if (YAbs > GlobalVar.position[1])
                yDirec = 1;
            else if (YAbs < GlobalVar.position[1])
                yDirec = -1;
            else
                yDirec = yDirectionOld;
            if (applyBacklash && yDirec != yDirectionOld)
                counter = yBacklashMM * yDirec;
            yDirectionOld = yDirec;

            int[] targetCounts = new int[6];
            countsOld.CopyTo(targetCounts, 0);
            int deltacounts = (int)Math.Round((YAbs + counter - GlobalVar.position[1]) / encoderResolution);
            targetCounts[1] = countsOld[1] + deltacounts;
            targetCounts[3] = countsOld[3] + deltacounts;
            targetCounts[5] = countsOld[5] + deltacounts;

            GlobalVar.position[1] = YAbs;

            return GotoTargetCounts(targetCounts, freedom: 'y', mode: mode, doubleCheck: doubleCheck, stopInBetween: stopInBetween, ignoreError: ignoreError, checkOnTarget: checkOnTarget);
        }

        // ZAbs is the platform z absolute position in mm
        // will update GlobalVar.position
        public bool ZMoveTo(double ZAbs, bool stopInBetween = true, bool ignoreError = false, bool applyBacklash = false, bool doubleCheck = false, char mode = 'p', bool checkOnTarget = true)
        {
            sbyte zDirec;
            double counter = 0;
            if (ZAbs > GlobalVar.position[2])
                zDirec = 1;
            else if (ZAbs < GlobalVar.position[2])
                zDirec = -1;
            else
                zDirec = zDirectionOld;
            if (applyBacklash && zDirec != zDirectionOld)
                counter = zBacklashMM * zDirec;
            zDirectionOld = zDirec;

            double[] targetPosition = new double[6];
            GlobalVar.position.CopyTo(targetPosition, 0);
            targetPosition[2] = ZAbs + counter;

            return GotoPosition(targetPosition, stopInBetween: stopInBetween, ignoreError: ignoreError, doubleCheck: doubleCheck, mode: mode, checkOnTarget: checkOnTarget);
        }

        // RxAbs is the absolute Rx value in degree
        // will update GlobalVar.position
        public bool RxMoveTo(double RxAbs, bool stopInBetween = true, bool ignoreError = false, bool doubleCheck = false, char mode = 'p', bool checkOnTarget = true)
        {
            double[] targetPosition = new double[6];
            GlobalVar.position.CopyTo(targetPosition, 0);
            targetPosition[3] = RxAbs;

            return GotoPosition(targetPosition, stopInBetween: stopInBetween, ignoreError: ignoreError, doubleCheck: doubleCheck, mode: mode, checkOnTarget: checkOnTarget);
        }

        // RyAbs is the absolute Ry value in degree
        // will update GlobalVar.position
        public bool RyMoveTo(double RyAbs, bool stopInBetween = true, bool ignoreError = false, bool doubleCheck = false, char mode = 'p', bool checkOnTarget = true)
        {
            double[] targetPosition = new double[6];
            GlobalVar.position.CopyTo(targetPosition, 0);
            targetPosition[4] = RyAbs;

            return GotoPosition(targetPosition, stopInBetween: stopInBetween, ignoreError: ignoreError, doubleCheck: doubleCheck, mode: mode, checkOnTarget: checkOnTarget);
        }

        // RzAbs is the absolute Rz value in degree
        // will update GlobalVar.position
        public bool RzMoveTo(double RzAbs, bool stopInBetween = true, bool ignoreError = false, bool doubleCheck = false, char mode = 'p', bool checkOnTarget = true)
        {
            double[] targetPosition = new double[6];
            GlobalVar.position.CopyTo(targetPosition, 0);
            targetPosition[5] = RzAbs;

            return GotoPosition(targetPosition, stopInBetween: stopInBetween, ignoreError: ignoreError, doubleCheck: doubleCheck, mode: mode, checkOnTarget: checkOnTarget);
        }

        public bool GotoReset() => GotoPosition(resetPosition);

        public bool GotoClose() => GotoTargetCounts(new int[6] { -1000, -1000, -1000, -1000, -1000, -1000 });

        // return false when timeout or driver board errors or out of range
        private bool GotoTargetCounts(int[] targetCounts, char freedom = 'a', bool stopInBetween = true, bool ignoreError = false, bool doubleCheck = false, char mode = 'p', bool checkOnTarget = true)
        {
            int timeoutloop, timeout = 50;
            if (mode == 't')
                timeout = 100;
            // try three times on doublecheck
            for (int i = 0; i < 3; i++)
            {
                if (!motorEngaged)
                {
                    EngageMotors();
                    motorEngaged = true;
                }
                if (SafetyCheck(targetCounts))
                    SendCounts(targetCounts, freedom: freedom, mode: mode);
                else
                {
                    DisengageMotors();
                    motorEngaged = false;
                    GlobalVar.errors = "Out of Range\n";
                    GlobalVar.errorFlag = true;
                    return false;
                }
                // timeout for about 5s
                timeoutloop = 0;
                while (checkOnTarget && timeoutloop < timeout)
                {
                    Thread.Sleep(100);
                    if (OnTarget(targetCounts, freedrom: freedom))
                        break;
                    timeoutloop++;
                }
                if (!ignoreError && timeoutloop >= 49)
                {
                    DisengageMotors();
                    motorEngaged = false;
                    if (!CheckErrors())
                    {
                        for (int j = 0; j < 6; j++)
                        {
                            if (Math.Abs(targetCounts[i] - countsReal[i]) > tolerance)
                                GlobalVar.errors = string.Concat(GlobalVar.errors, "Axis ", j + 1, " Timeout Error\n");
                        }
                    }
                    GlobalVar.errorFlag = true;
                    return false;
                }
                if (doubleCheck || stopInBetween)
                {
                    DisengageMotors();
                    motorEngaged = false;
                    if (doubleCheck)
                    {
                        Thread.Sleep(100);
                        if (OnTarget(targetCounts, freedrom: freedom))
                            return true;
                        else
                            continue;
                    }
                }
                return true;
            }
            return true;
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
            T1Port.WriteLine(str);
            return T1Port.ReadLine();
        }

        // If the return is a number, it can be convert to int or float directly , no line feed sign
        // If the return is a description, print it out using WriteLine(), don't use Write()
        private static string T2Talk(string str)
        {
            T2Port.WriteLine(str);
            return T2Port.ReadLine();
        }

        // If the return is a number, it can be convert to int or float directly , no line feed sign
        // If the return is a description, print it out using WriteLine(), don't use Write()
        private static string T3Talk(string str)
        {
            T3Port.WriteLine(str);
            return T3Port.ReadLine();
        }

        private static void T1SendOnly(string str) => T1Port.WriteLine(str);

        private static void T2SendOnly(string str) => T2Port.WriteLine(str);

        private static void T3SendOnly(string str) => T3Port.WriteLine(str);

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
        //mode can be 't' or 'p' meaning trajectory or stepping method
        private static void SendCounts(int[] counts, char freedom = 'a', char mode = 'p')
        {
            int trajectoryThreshold = 20000;
            string xstrp = "p 0 ";
            string xstrt = "t 0 ";
            string ystrp = "p 1 ";
            string ystrt = "t 1 ";
            string strpp = " 0 0";
            string cmd;

            if (freedom == 'x' || freedom == 'a')
            {
                if ((Math.Abs(counts[0] - countsOld[0]) < trajectoryThreshold) && mode == 'p')
                    cmd = string.Concat(xstrp, counts[0], strpp);
                else
                    cmd = string.Concat(xstrt, counts[0]);
                T1SendOnly(cmd);

                if ((Math.Abs(counts[2] - countsOld[2]) < trajectoryThreshold) && mode == 'p')
                    cmd = string.Concat(xstrp, counts[2], strpp);
                else
                    cmd = string.Concat(xstrt, counts[2]);
                T2SendOnly(cmd);

                if ((Math.Abs(counts[4] - countsOld[4]) < trajectoryThreshold) && mode == 'p')
                    cmd = string.Concat(xstrp, counts[4], strpp);
                else
                    cmd = string.Concat(xstrt, counts[4]);
                T3SendOnly(cmd);
            }
            
            if (freedom == 'y' || freedom == 'a')
            {
                if ((Math.Abs(counts[1] - countsOld[1]) < trajectoryThreshold) && mode == 'p')
                    cmd = string.Concat(ystrp, counts[1], strpp);
                else
                    cmd = string.Concat(ystrt, counts[1]);
                T1SendOnly(cmd);

                if ((Math.Abs(counts[3] - countsOld[3]) < trajectoryThreshold) && mode == 'p')
                    cmd = string.Concat(ystrp, counts[3], strpp);
                else
                    cmd = string.Concat(ystrt, counts[3]);
                T2SendOnly(cmd);

                if ((Math.Abs(counts[5] - countsOld[5]) < trajectoryThreshold) && mode == 'p')
                    cmd = string.Concat(ystrp, counts[5], strpp);
                else
                    cmd = string.Concat(ystrt, counts[5]);
                T3SendOnly(cmd);
            }

            counts.CopyTo(countsOld, 0);
        }

        // Fetch each axis's real counts. axis = 0 fetch all 6 axis
        // 0 - 5 will fetch motor 1 - 6 individually
        public static void RealCountsFetch(sbyte axis)
        {
            string strx = "r axis0.encoder.shadow_count";
            string stry = "r axis1.encoder.shadow_count";
            if (axis == 0)
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
        private static bool OnTarget(int[] counts, char freedrom = 'a')
        {
            if (freedrom == 'x' || freedrom == 'a')
            {
                RealCountsFetch(0);
                RealCountsFetch(2);
                RealCountsFetch(4);
                if (countsReal[0] < (counts[0] - tolerance) || countsReal[0] > (counts[0] + tolerance))
                    return false; 
                else if (countsReal[2] < (counts[2] - tolerance) || countsReal[2] > (counts[2] + tolerance))
                    return false; 
                else if (countsReal[4] < (counts[4] - tolerance) || countsReal[4] > (counts[4] + tolerance))
                    return false; 
            }
            
            if (freedrom == 'y' || freedrom == 'a')
            {
                RealCountsFetch(1);
                RealCountsFetch(3);
                RealCountsFetch(5);
                if (countsReal[1] < (counts[1] - tolerance) || countsReal[1] > (counts[1] + tolerance))
                    return false;
                else if (countsReal[3] < (counts[3] - tolerance) || countsReal[3] > (counts[3] + tolerance))
                    return false;
                else if (countsReal[5] < (counts[5] - tolerance) || countsReal[5] > (counts[5] + tolerance))
                    return false;
            }
            return true;
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
                DisengageMotors();
                motorEngaged = false;
                return true;
            }
            else
            {
                GlobalVar.errors = "";
                return false;
            }
        }

        private static void ErrorCodeExplain(int[] codes)
        {
            string xstr = "r axis0.";
            string ystr = "r axis1.";
            string str0;
            GlobalVar.errors = "";
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
                    GlobalVar.errors = string.Concat(GlobalVar.errors, "Axis ", i + 1, " error: ", AxisErrorCode(codes[i]), "\n");
                    continue;
                }

                if (i <= 1)
                    codes[i] = int.Parse(T1Talk(str0));
                else if (i <= 3)
                    codes[i] = int.Parse(T2Talk(str0));
                else
                    codes[i] = int.Parse(T3Talk(str0));

                if (str0[8] == 'm')
                    GlobalVar.errors = string.Concat(GlobalVar.errors, "Motor ", i + 1, " error: ", MotorErrorCode(codes[i]), "\n");
                else if (str0[8] == 'e')
                    GlobalVar.errors = string.Concat(GlobalVar.errors, "Encoder ", i + 1, " error: ", EncoderErrorCode(codes[i]), "\n");
                else if (str0[8] == 'c')
                    GlobalVar.errors = string.Concat(GlobalVar.errors, "Controller ", i + 1, " error: ", ControllerErrorCode(codes[i]), "\n");
            }
        }

        private static bool SafetyCheck(int[] counts)
        {
            GlobalVar.errors = "";
            // for version where 0 counts is at middle
            if (limit[0] > 150000)
            {
                if (counts[0] > (limit[0] - 3000) || counts[0] < (limit[0] - rangePerAxis * countsPerMM))
                    GlobalVar.errors = "T1x Out of Range\n";
                else if (counts[1] > (limit[1] - 3000) || counts[1] < (limit[1] - rangePerAxis * countsPerMM))
                    GlobalVar.errors = "T1y Out of Range\n";
                else if (counts[2] > (limit[2] - 3000) || counts[2] < (limit[2] - rangePerAxis * countsPerMM))
                    GlobalVar.errors = "T2x Out of Range\n";
                else if (counts[3] > (limit[3] - 3000) || counts[3] < (limit[3] - rangePerAxis * countsPerMM))
                    GlobalVar.errors = "T2y Out of Range\n";
                else if (counts[4] > (limit[4] - 3000) || counts[4] < (limit[4] - rangePerAxis * countsPerMM))
                    GlobalVar.errors = "T3x Out of Range\n";
                else if (counts[5] > (limit[5] - 3000) || counts[5] < (limit[5] - rangePerAxis * countsPerMM))
                    GlobalVar.errors = "T3y Out of Range\n";
                else
                    return true;
                return false;
            }
            else // for version where 0 counts is at the end
            {
                if (counts[0] < (limit[0] + 3000) || counts[0] > (limit[0] + rangePerAxis * countsPerMM))
                    GlobalVar.errors = "T1x Out of Range\n";
                else if (counts[1] < (limit[1] + 3000) || counts[1] > (limit[1] + rangePerAxis * countsPerMM))
                    GlobalVar.errors = "T1y Out of Range\n";
                else if (counts[2] < (limit[2] + 3000) || counts[2] > (limit[2] + rangePerAxis * countsPerMM))
                    GlobalVar.errors = "T2x Out of Range\n";
                else if (counts[3] < (limit[3] + 3000) || counts[3] > (limit[3] + rangePerAxis * countsPerMM))
                    GlobalVar.errors = "T2y Out of Range\n";
                else if (counts[4] < (limit[4] + 3000) || counts[4] > (limit[4] + rangePerAxis * countsPerMM))
                    GlobalVar.errors = "T3x Out of Range\n";
                else if (counts[5] < (limit[5] + 3000) || counts[5] > (limit[5] + rangePerAxis * countsPerMM))
                    GlobalVar.errors = "T3y Out of Range\n";
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
