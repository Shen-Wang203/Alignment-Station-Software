using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Windows.Forms;

namespace Beetle
{
    static class BeetleConnection
    {
        // Those are all the Odrive board we used in control box
        // Need to extend if we ever build more control box
        private static readonly string[] T1 = { "208739844D4D", "207339A04D4D", "207639684D4D", "20803880304E", "2067387E304E", "205D388E304E", "2063388F304E" };
        private static readonly string[] T2 = { "208339834D4D", "2060388E304E", "2061397D4D4D", "2062388F304E", "204F388E304E", "20853881304E", "207B3880304E" };
        private static readonly string[] T3 = { "205C39844D4D", "20813882304E", "207B396A4D4D", "207C397D4D4D", "2065387E304E", "2086388F304E", "2087388E304E" };

        // port1 port2 and port3 should be the three ports connected to the Beetle control box
        // This method will automatically assign each port to its beetleTComPortName in Parameters.
        // return true if success
        public static bool AssignPorts()
        {
            string[] ports = SerialPort.GetPortNames();
            string[] beetleSerialNum = new string[3];
            string[] beetlePorts = new string[3];
            sbyte portCount = 0;
            string serialNum;
            foreach (string port in ports)
            {
                //Console.WriteLine(port);
                serialNum = SerialNumberIdentify(port);
                if (serialNum != "")
                {
                    if (serialNum == "Arduino\r")
                    {
                        Parameters.arduinoComPortName = port;
                        continue;
                    }
                    //Console.WriteLine(serialNum);
                    beetleSerialNum[portCount] = serialNum;
                    beetlePorts[portCount] = port;
                    portCount += 1;
                }
            }
            if (portCount != 3)
            {
                Console.WriteLine("Beetle Connection Failed");
                return false;
            }
            // get rid of \r char in the string end
            beetleSerialNum[0] = beetleSerialNum[0].Substring(0, beetleSerialNum[0].Length - 1);
            beetleSerialNum[1] = beetleSerialNum[1].Substring(0, beetleSerialNum[1].Length - 1);
            beetleSerialNum[2] = beetleSerialNum[2].Substring(0, beetleSerialNum[2].Length - 1);
            bool port1Assigned = false, port2Assigned = false, port3Assigned = false;
            foreach (string sm in T1)
            {
                if (!port1Assigned && beetleSerialNum[0] == sm)
                {
                    Parameters.beetleT1ComPortName = beetlePorts[0];
                    port1Assigned = true;
                }
                else if (!port2Assigned && beetleSerialNum[1] == sm)
                {
                    Parameters.beetleT1ComPortName = beetlePorts[1];
                    port2Assigned = true;
                }
                else if (!port3Assigned && beetleSerialNum[2] == sm)
                {
                    Parameters.beetleT1ComPortName = beetlePorts[2];
                    port3Assigned = true;
                }
            }
            foreach (string sm in T2)
            {
                if (!port1Assigned && beetleSerialNum[0] == sm)
                {
                    Parameters.beetleT2ComPortName = beetlePorts[0];
                    port1Assigned = true;
                }
                else if (!port2Assigned && beetleSerialNum[1] == sm)
                {
                    Parameters.beetleT2ComPortName = beetlePorts[1];
                    port2Assigned = true;
                }
                else if (!port3Assigned && beetleSerialNum[2] == sm)
                {
                    Parameters.beetleT2ComPortName = beetlePorts[2];
                    port3Assigned = true;
                }
            }
            foreach (string sm in T3)
            {
                if (!port1Assigned && beetleSerialNum[0] == sm)
                {
                    Parameters.beetleT3ComPortName = beetlePorts[0];
                    port1Assigned = true;
                }
                else if (!port2Assigned && beetleSerialNum[1] == sm)
                {
                    Parameters.beetleT3ComPortName = beetlePorts[1];
                    port2Assigned = true;
                }
                else if (!port3Assigned && beetleSerialNum[2] == sm)
                {
                    Parameters.beetleT3ComPortName = beetlePorts[2];
                    port3Assigned = true;
                }
            }
            if (port1Assigned && port2Assigned && port3Assigned)
                return true;
            else
                return false;
        }

        // Serial Number Identification, return serial number + \r
        private static string SerialNumberIdentify(string portName)
        {
            SerialPort port = new SerialPort(portName, 115200, Parity.None, 8, StopBits.One)
            {
                ReadTimeout = 200,
                WriteTimeout = 200
            };
            string x;
            try
            {
                port.Open();
                port.WriteLine("i");
                port.ReadLine();
                port.ReadLine();
                x = port.ReadLine();
            }
            catch (Exception)
            {
                //Console.WriteLine("Wrong Connection On " + portName);
                //MessageBox.Show(e.Message + "\nFail to Connect COM Port");
                return "";
            }
            port.Close();
            int found = x.IndexOf(": ");
            return x.Substring(found + 2);
        }
    }
}
