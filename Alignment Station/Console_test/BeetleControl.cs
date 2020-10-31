using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Net.Http.Headers;

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
        public static sbyte tolerance = 2;
        public static double encoderResolution = 50e-6;
        public static int[] countsReal = { 0, 0, 0, 0, 0, 0}; // {T1x, T1y, T2x, T2y, T3x, T3y}
        public static int[] countsOld = { 0, 0, 0, 0, 0, 0 }; // {T1x, T1y, T2x, T2y, T3x, T3y}

        public BeetleControl()
        {
            try
            {
                T1Port = new SerialPort(GlobalVar.T1ComPortName, 115200, Parity.None, 8, StopBits.One);
                T2Port = new SerialPort(GlobalVar.T2ComPortName, 115200, Parity.None, 8, StopBits.One);
                T3Port = new SerialPort(GlobalVar.T3ComPortName, 115200, Parity.None, 8, StopBits.One);
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
            catch (Exception ex)
            {
                //TODO: message box
            }

            int x1 = 183000, x2 = 183000, x3 = 183000, y1 = 183000, y2 = 183000, y3 = 183000;
            double A1x, A1y, A2x, A2y, A3x, A3y;
            switch(GlobalVar.fixtureNumber)
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
            A1x = x1 - (-85.796144) / 50e-6;
            A1y = y1 - 9.55 / 50e-6;
            A2x = x2 + 38.123072 / 50e-6;
            A2y = y2 - (-73.022182) / 50e-6;
            A3x = x3 + 38.123072 / 50e-6;
            A3y = y3 - 92.122182 / 50e-6;

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

        }

        public static void DisengageMotors()
        {

        }

        private static int[] TranslateToCounts(double[] Tmm)
        {
            int[] counts = { 0, 0, 0, 0, 0, 0};
            counts[0] = (int)Math.Round(Tmm[0] / encoderResolution + countsOffset[0]);
            counts[1] = (int)Math.Round(Tmm[1] / encoderResolution + countsOffset[1]);
            counts[2] = (int)Math.Round(Tmm[2] / encoderResolution + countsOffset[2]);
            counts[3] = (int)Math.Round(Tmm[3] / encoderResolution + countsOffset[3]);
            counts[4] = (int)Math.Round(Tmm[4] / encoderResolution + countsOffset[4]);
            counts[5] = (int)Math.Round(Tmm[5] / encoderResolution + countsOffset[5]);
            return counts;
        }

        private static string AxisErrorCode(int code)
        {
            switch(code)
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
                    return "Invalid Code";
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
                    return "Invalid Code";
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
                    return "Invalid Code";
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
                    return "Invalid Code";
            }
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

        // Send counts in X direction
        private static void TxCountsSend(int x1, int x2, int x3, char mode)
        {
            string str1, str2, str3;
            if (mode == 't')
            {
                string var0 = "t 0 ";
                str1 = string.Concat(var0, x1);
                str2 = string.Concat(var0, x2);
                str3 = string.Concat(var0, x3);
            }
            else
            {
                string var0 = "p 0 ";
                string var00 = " 0 0";
                str1 = string.Concat(var0, x1, var00);
                str2 = string.Concat(var0, x2, var00);
                str3 = string.Concat(var0, x3, var00);
            }
            T1Port.WriteLine(str1);
            T2Port.WriteLine(str2);
            T3Port.WriteLine(str3);

            countsOld[0] = x1;
            countsOld[2] = x2;
            countsOld[4] = x3;
        }

        // Send counts in Y direction
        private static void TyCountsSend(int y1, int y2, int y3, char mode)
        {
            string str1, str2, str3;
            if (mode == 't')
            {
                string var0 = "t 1 ";
                str1 = string.Concat(var0, y1);
                str2 = string.Concat(var0, y2);
                str3 = string.Concat(var0, y3);
            }
            else
            {
                string var0 = "p 1 ";
                string var00 = " 0 0";
                str1 = string.Concat(var0, y1, var00);
                str2 = string.Concat(var0, y2, var00);
                str3 = string.Concat(var0, y3, var00);
            }
            T1Port.WriteLine(str1);
            T2Port.WriteLine(str2);
            T3Port.WriteLine(str3);

            countsOld[1] = y1;
            countsOld[3] = y2;
            countsOld[5] = y3;
        }

        // Fetch each axis's real counts. axis = 0 fetch all 6 axis
        private static void RealCountsFetch(byte axis)
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
            else if (axis == 1)
            {
                try
                { countsReal[0] = int.Parse(T1Talk(strx)); }
                catch (Exception)
                { countsReal[0] = 1000000; }
            }
            else if (axis == 2)
            {
                try
                { countsReal[1] = int.Parse(T1Talk(stry)); }
                catch(Exception)
                { countsReal[1] = 1000000; }
            }
            else if (axis == 3)
            {
                try
                { countsReal[2] = int.Parse(T2Talk(strx)); }
                catch (Exception)
                { countsReal[2] = 1000000; }
            }
            else if (axis == 4)
            {
                try
                { countsReal[3] = int.Parse(T2Talk(stry)); }
                catch (Exception)
                { countsReal[3] = 1000000; }
            }
            else if (axis == 5)
            {
                try
                { countsReal[4] = int.Parse(T3Talk(strx)); }
                catch (Exception)
                { countsReal[4] = 1000000; }
            }
            else if (axis == 6)
            {
                try
                { countsReal[5] = int.Parse(T3Talk(stry)); }
                catch (Exception)
                { countsReal[5] = 1000000; }
            }
        }

        // Check if x motors are at counts x1, x2, x3
        private static bool TxOnTarget(int x1, int x2, int x3)
        {
            RealCountsFetch(1);
            RealCountsFetch(3);
            RealCountsFetch(5);
            if (countsReal[0] < (x1 - tolerance) || countsReal[0] > (x1 + tolerance))
            { return false; }
            else if (countsReal[2] < (x2 - tolerance) || countsReal[2] > (x2 + tolerance))
            { return false; }
            else if (countsReal[4] < (x3 - tolerance) || countsReal[4] > (x3 + tolerance))
            { return false; }
            else
            { return true; }
        }

        // Check if y motors are at counts y1, y2, y3
        private static bool TyOnTarget(int y1, int y2, int y3)
        {
            RealCountsFetch(2);
            RealCountsFetch(4);
            RealCountsFetch(6);
            if (countsReal[1] < (y1 - tolerance) || countsReal[1] > (y1 + tolerance))
            { return false; }
            else if (countsReal[3] < (y2 - tolerance) || countsReal[3] > (y2 + tolerance))
            { return false; }
            else if (countsReal[5] < (y3 - tolerance) || countsReal[5] > (y3 + tolerance))
            { return false; }
            else
            { return true; }
        }

        private static bool OnTarget(int[] counts) => TxOnTarget(counts[0], counts[2], counts[4]) && TyOnTarget(counts[1], counts[3], counts[5]);

        // if error exists return true, else return false
        private static bool CheckErrors()
        {
            string xstr = "r axis0.error";
            string ystr = "r axis1.error";
            int[] errorCodes = { };
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

        private static void SendCounts(int[] counts)
        {
            int trajectoryThreshold = 20000;
            string xstrp = "p 0 ";
            string xstrt = "t 0 ";
            string ystrp = "p 1 ";
            string ystrt = "t 1 ";
            string strpp = " 0 0";
            string cmd;
            if (Math.Abs(counts[0] - countsOld[0]) < trajectoryThreshold)
                cmd = string.Concat(xstrp, counts[0], strpp);
            else
                cmd = string.Concat(xstrt, counts[0]);
            T1SendOnly(cmd);

            if (Math.Abs(counts[1] - countsOld[1]) < trajectoryThreshold)
                cmd = string.Concat(ystrp, counts[1], strpp);
            else
                cmd = string.Concat(ystrt, counts[1]);
            T1SendOnly(cmd);

            if (Math.Abs(counts[2] - countsOld[2]) < trajectoryThreshold)
                cmd = string.Concat(xstrp, counts[2], strpp);
            else
                cmd = string.Concat(xstrt, counts[2]);
            T2SendOnly(cmd);

            if (Math.Abs(counts[3] - countsOld[3]) < trajectoryThreshold)
                cmd = string.Concat(ystrp, counts[3], strpp);
            else
                cmd = string.Concat(ystrt, counts[3]);
            T2SendOnly(cmd);

            if (Math.Abs(counts[4] - countsOld[4]) < trajectoryThreshold)
                cmd = string.Concat(xstrp, counts[4], strpp);
            else
                cmd = string.Concat(xstrt, counts[4]);
            T3SendOnly(cmd);

            if (Math.Abs(counts[5] - countsOld[5]) < trajectoryThreshold)
                cmd = string.Concat(ystrp, counts[5], strpp);
            else
                cmd = string.Concat(ystrt, counts[5]);
            T3SendOnly(cmd);

            countsOld = counts;
        }

    }
}
