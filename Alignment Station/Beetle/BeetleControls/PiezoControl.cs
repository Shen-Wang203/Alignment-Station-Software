using System;
using System.Threading;
using System.IO.Ports;
using System.Windows.Forms;

namespace Beetle
{
    // The piezo has 0-1000V total range which is +-25um, it requires 0-6.6V input to have full range.
    // The Arduino DAC Shield has 0 - 4.096V range. So we are using only portion of the full range.
    class PiezoControl
    {
        private Parameters parameters;

        public PiezoControl(Parameters prmts)
        {
            parameters = prmts;
        }

        private SerialPort ardnPort;

        public bool Connection()
        {
            try
            {
                ardnPort = new SerialPort(parameters.arduinoComPortName, 115200, Parity.None, 8, StopBits.One)
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

        public void ClosePort()
        {
            if (ardnPort != null)
                ardnPort.Close();
        }

        public void OpenPort()
        {
            if (ardnPort != null)
                ardnPort.Open();
        }

        public bool PortIsOpen() => ardnPort.IsOpen;

        // ch is 0-2 representing x, y, z
        // dacValue is the DAC value which is 0- 0xfff
        // total command has two byte, 0x#***, wherr the # position is ch (0-2) and * positions is the code
        // will update parameters.piezoPosition whenever this function is called
        public void Send(byte ch, ushort dacValue)
        {
            if (dacValue > 0x0fff)
                dacValue = 0x0fff;
            byte[] cmd = new byte[2] { 0x00, 0x00 };
            cmd[0] = (byte)((byte)((ch & 0x0f) << 4) + (byte)((dacValue & 0x0f00) >> 8));
            cmd[1] = (byte)(dacValue & 0x00ff);
            try
            {
                ardnPort.Write(cmd, 0, 2);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\nPiezo COM Port Failed");
                parameters.errorFlag = true;
                parameters.errors = "Piezo COM Port Failed";
                return;
            }
            parameters.piezoPosition[ch] = dacValue;
        }

        // axis 0-2 means x,y,z; 3 means all axials
        public void Reset(byte axis = 3)
        {
            if (axis == 3)
            {
                Send(0, 0x800);
                Send(1, 0x800);
                Send(2, 0x800);
            }
            else
                Send(axis, 0x800);
        }

        public void TestRun()
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
