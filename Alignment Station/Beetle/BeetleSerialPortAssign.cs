using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace Beetle
{
    static class BeetleSerialPortAssign
    {
        // Those are all the Odrive board we used in control box
        // Need to extend if we ever build more control box
        private static readonly string[] T1 = { "208739844D4D", "207339A04D4D", "207639684D4D", "20803880304E", "2067387E304E", "205D388E304E", "2063388F304E" };
        private static readonly string[] T2 = { "208339834D4D", "2060388E304E", "2061397D4D4D", "2087388E304E", "204F388E304E", "20853881304E", "207B3880304E" };
        private static readonly string[] T3 = { "205C39844D4D", "20813882304E", "207B396A4D4D", "207C397D4D4D", "2065387E304E", "2087388E304E", "2087388E304E" };

        // port1 port2 and port3 should be the three ports connected to the Beetle control box
        // This method will automatically assign each port to its beetleTComPortName in GlobalVar.
        // return true if success
        public static bool AssignPorts(string port1, string port2, string port3)
        {
            string serialNum1 = SerialNumberIdentify(port1);
            string serialNum2 = SerialNumberIdentify(port2);
            string serialNum3 = SerialNumberIdentify(port3);
            if (serialNum1 == "" || serialNum2 == "" || serialNum3 == "")
                return false;
            serialNum1 = serialNum1.Substring(0, serialNum1.Length - 1);
            serialNum2 = serialNum2.Substring(0, serialNum2.Length - 1);
            serialNum3 = serialNum3.Substring(0, serialNum3.Length - 1);
            bool port1Assigned = false, port2Assigned = false, port3Assigned = false;
            foreach (string sm in T1)
            {
                if (!port1Assigned && serialNum1 == sm)
                {
                    GlobalVar.beetleT1ComPortName = port1;
                    port1Assigned = true;
                }
                else if (!port2Assigned && serialNum2 == sm)
                {
                    GlobalVar.beetleT1ComPortName = port2;
                    port2Assigned = true;
                }
                else if (!port3Assigned && serialNum3 == sm)
                {
                    GlobalVar.beetleT1ComPortName = port3;
                    port3Assigned = true;
                }
            }
            foreach (string sm in T2)
            {
                if (!port1Assigned && serialNum1 == sm)
                {
                    GlobalVar.beetleT2ComPortName = port1;
                    port1Assigned = true;
                }
                else if (!port2Assigned && serialNum2 == sm)
                {
                    GlobalVar.beetleT2ComPortName = port2;
                    port2Assigned = true;
                }
                else if (!port3Assigned && serialNum3 == sm)
                {
                    GlobalVar.beetleT2ComPortName = port3;
                    port3Assigned = true;
                }
            }
            foreach (string sm in T3)
            {
                if (!port1Assigned && serialNum1 == sm)
                {
                    GlobalVar.beetleT3ComPortName = port1;
                    port1Assigned = true;
                }
                else if (!port2Assigned && serialNum2 == sm)
                {
                    GlobalVar.beetleT3ComPortName = port2;
                    port2Assigned = true;
                }
                else if (!port3Assigned && serialNum3 == sm)
                {
                    GlobalVar.beetleT3ComPortName = port3;
                    port3Assigned = true;
                }
            }
            if (port1Assigned && port2Assigned && port3Assigned)
                return true;
            else
                return false;
        }

        // Serial Number Identification, return serial number
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
                Console.WriteLine("Wrong Connection On" + portName);
                return "";
            }
            port.Close();
            int found = x.IndexOf(": ");
            return x.Substring(found + 2);
        }

    }
}
