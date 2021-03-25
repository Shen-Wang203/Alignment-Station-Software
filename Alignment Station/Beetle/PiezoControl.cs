using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO.Ports;
using System.Windows.Forms;

namespace Beetle
{
    // The piezo has 0-1000V total range which is +-25um, it requires 0-6.6V input to have full range.
    // The Arduino DAC Shield has 0 - 4.096V range. So we are using only portion of the full range.
    static class PiezoControl
    {
        private static SerialPort ardnPort;

        public static bool Connection()
        {
            try
            {
                ardnPort = new SerialPort(Parameters.arduinoComPortName, 115200, Parity.None, 8, StopBits.One)
                {
                    ReadTimeout = 200,
                    WriteTimeout = 200
                };
                ardnPort.Open();
                return true;
            }
            catch (Exception e)
            {
                //TODO: message box
                //Console.WriteLine("Arduino Connection Error");
                MessageBox.Show(e.Message + "\nFailed to Connect Arduino For Piezo Control");
                return false;
            }
        }

        // ch is 0-2 representing x, y, z
        // dacValue is the DAC value which is 0- 0xfff
        // total command has two byte, 0x#***, wherr the # position is ch (0-2) and * positions is the code
        // will update Parameters.piezoPosition whenever this function is called
        public static void Send(byte ch, ushort dacValue)
        {
            if (dacValue > 0x0fff)
                dacValue = 0x0fff;
            byte[] cmd = new byte[2] { 0x00, 0x00 };
            cmd[0] = (byte)((byte)((ch & 0x0f) << 4) + (byte)((dacValue & 0x0f00) >> 8));
            cmd[1] = (byte)(dacValue & 0x00ff);
            ardnPort.Write(cmd, 0, 2);
            Parameters.piezoPosition[ch] = dacValue;
        }

        public static void Reset()
        {
            Send(0, 0x800);
            Send(1, 0x800);
            Send(2, 0x800);
        }

        public static void Test()
        {
            Thread.Sleep(5000);
        }

        public static void TestRun()
        {
            Send(0, 0x800);
            Send(1, 0x800);
            Send(2, 0x800);
            while (true)
            {
                for (ushort i = 0x801; i < 0xD32; i++)
                {
                    Send(0, i);
                    Thread.Sleep(20);
                    //Console.WriteLine(i);
                }
                Send(0, 0x800);
                Thread.Sleep(500);
                for (ushort i = 0x800; i > 0x2CA; i--)
                {
                    Send(0, i);
                    Thread.Sleep(20);
                    //Console.WriteLine(i);
                }
                Send(0, 0x800);
                Thread.Sleep(500);

                for (ushort i = 0x801; i < 0xD32; i++)
                {
                    Send(1, i);
                    Thread.Sleep(20);
                    //Console.WriteLine(i);
                }
                Send(1, 0x800);
                Thread.Sleep(500);
                for (ushort i = 0x800; i > 0x2CA; i--)
                {
                    Send(1, i);
                    Thread.Sleep(20);
                    //Console.WriteLine(i);
                }
                Send(1, 0x800);
                Thread.Sleep(500);
            }

        }


    }
}
