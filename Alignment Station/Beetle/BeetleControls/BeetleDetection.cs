using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Windows.Forms;

namespace Beetle
{
    class BeetleDetection
    {
        private Parameters parameters;
        private BeetleControl beetleControl;
        private PiezoControl piezoControl;

        public BeetleDetection(Parameters prmts, BeetleControl bc, PiezoControl pc)
        {
            parameters = prmts;
            beetleControl = bc;
            piezoControl = pc;
        }

        // Those are all the Odrive board we used in control box
        // Need to extend if we ever build more control box
        // Arduino Piezo control board's serial number is defined by ourself and its written inside the Arduino board
        private static readonly string[] T1 = { "208739844D4D", "207339A04D4D", "207639684D4D", "20803880304E", "2067387E304E", "205D388E304E", "2063388F304E" };
        private static readonly string[] T2 = { "208339834D4D", "2060388E304E", "2061397D4D4D", "2062388F304E", "204F388E304E", "20853881304E", "207B3880304E" };
        private static readonly string[] T3 = { "205C39844D4D", "20813882304E", "207B396A4D4D", "207C397D4D4D", "2065387E304E", "2086388F304E", "2087388E304E" };
        private static readonly string[] Pa = { "Ard0", "Ard1", "Ard2", "Ard3", "Ard4", "Ard5", "Ard6" };

        // port1 port2 and port3 should be the three ports connected to the Beetle control box
        // This method will automatically assign each port to its beetleTComPortName in parameters.
        // return true if success
        public bool AssignPorts()
        {
            beetleControl.ClosePorts();
            piezoControl.ClosePort();

            string[] ports = SerialPort.GetPortNames();
            List<string> beetleSerialNum = new List<string>();
            List<string> beetlePorts = new List<string>();
            string serialNum;
            foreach (string port in ports)
            {
                //Console.WriteLine(port);
                serialNum = SerialNumberIdentify(port);
                if (serialNum != "")
                {
                    //Console.WriteLine(serialNum);
                    beetleSerialNum.Add(serialNum);
                    beetlePorts.Add(port);
                }
            }
            if (beetlePorts.Count < 4 )
            {
                MessageBox.Show("Missing Beetle Control Box or Arduino for Piezo");
                return false;
            }
            // get rid of \r char in the string end
            for (int i = 0; i < beetlePorts.Count; i++)
                beetleSerialNum[i] = beetleSerialNum[i].Substring(0, beetleSerialNum[i].Length - 1);

            List<int> controlBoxNum = new List<int>();
            int index;
            foreach (string sm in beetleSerialNum)
            {
                index = Array.FindIndex(T2, element => element == sm);
                if (index != -1)
                    controlBoxNum.Add(index);
            }

            foreach (int num in controlBoxNum)
            {
                string message = "Connect to Beetle Control Box No." + num.ToString();
                DialogResult dialogResult = MessageBox.Show(message, "Connecting", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    try
                    {
                        parameters.beetleT1ComPortName = beetlePorts[beetleSerialNum.FindIndex(element => element == T1[num])];
                        parameters.beetleT2ComPortName = beetlePorts[beetleSerialNum.FindIndex(element => element == T2[num])];
                        parameters.beetleT3ComPortName = beetlePorts[beetleSerialNum.FindIndex(element => element == T3[num])];
                        parameters.arduinoComPortName = beetlePorts[beetleSerialNum.FindIndex(element => element == Pa[num])];
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Failed to Find Beetle Control Boxes");
                        return false;
                    }
                    parameters.beetleControlBoxNum = num.ToString();
                    break;
                }
                else if (dialogResult == DialogResult.No)
                {
                    if (num == controlBoxNum[controlBoxNum.Count - 1])
                    {
                        MessageBox.Show("Failed to Select One to Connect");
                        return false;
                    }
                }

            }
            return true;
        }

        // Serial Number Identification, return serial number + \r
        private static string SerialNumberIdentify(string portName)
        {
            string x;
            SerialPort port;
            try
            {
                port = new SerialPort(portName, 115200, Parity.None, 8, StopBits.One)
                {
                    ReadTimeout = 200,
                    WriteTimeout = 200
                };
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
                // if ports have been opened already, then it will return empty too
                return "";
            }
            if (port != null)
                port.Close();
            int found = x.IndexOf(": ");
            return x.Substring(found + 2);
        }
    }
}
