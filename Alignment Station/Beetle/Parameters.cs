using System;
using System.Collections.Generic;
using System.IO;

namespace Beetle
{
    class Parameters
    {
        private static string logPath = string.Empty;

        // Fixture parameteres
        public string beetleT1ComPortName = "";
        public string beetleT2ComPortName = "";
        public string beetleT3ComPortName = "";
        public string arduinoComPortName = ""; // for piezo control
        public byte beetleFixtureNumber = 0;
        public string beetleControlBoxNum = "*";
        // Pivot point coordinates is relative to the center (x,y,z,1) of moving plate joint surface, z need to minus 8 to become pivotpoint to top moving part top surface distance
        public double[] pivotPoint = { 0, 0, 42, 0 };
        public double[] position = { 0, 0, 140, 2.4, -1.0, 0 }; // Position in mm { x, y, z, Rx, Ry, Rz}
        public double[] initialPosition = { 0, 0, 138, 0, 0, 0 }; // This is the starting(Or Initial) position
        public ushort[] piezoPosition = { 0x800, 0x800, 0x800 }; // Piezo position (x, y, z) in DAC value ranging from 0x000 to 0xfff. 
        public bool usePiezo = false;
        //public byte piezoZvsGap = 1; // if piezo z DAC value increase, gap is larger, then 1; is z DAC value incease, gap is smaller, then -1
        public bool piezoRunning = false;
        public ushort piezoStepSize = 18; // in DAC value

        // Alignment/Curing parameters
        public string errors = "";
        public bool errorFlag = false;
        public double lossCriteria = -0.1f;
        public double lossCurrentMax = -50.0f;
        public bool doublecheckFlag = false;
        public bool stopInBetweenFlag = false;
        public bool smallestResolution = false;
        public bool highestAccuracy = true;

        // Product Parameters
        public string productName = "MM 1xN";
        // Product type and its back length when starts from contact, this back length should be longer than this product's focal length.
        // Gap or focal length can be get through productName: Parameters.product[Parameters.productName]
        public Dictionary<string, float> productGap =
            new Dictionary<string, float>()
            {
                { "VOA", 0.1f },
                { "SM 1xN", 0.25f },
                { "MM 1xN", 0.18f },
                { "UWDM", 0.1f },
                { "WOA", 0.01f }
            };

        public void Save()
        {
            SaveCOMPorts();
            Properties.Fixture.Default.BeetleFixtureNum = beetleFixtureNumber;
            Properties.Fixture.Default.BeetleControlBoxNum = beetleControlBoxNum;

            Properties.Powermeter.Default.Reference = PowerMeter.lossReference;
            Properties.Powermeter.Default.Addr = PowerMeter.addr;
            Properties.Powermeter.Default.Channel = PowerMeter.channel;

            Properties.Alignment.Default.LossCriteria = lossCriteria;
            Properties.Alignment.Default.ProductName = productName;
            Properties.Alignment.Default.DoubleCheck = doublecheckFlag;
            Properties.Alignment.Default.StopInBetween = stopInBetweenFlag;
        }

        public void Save_2()
        {
            SaveCOMPorts_2();
            Properties.Fixture.Default.BeetleFixtureNum_2 = beetleFixtureNumber;
            Properties.Fixture.Default.BeetleControlBoxNum_2 = beetleControlBoxNum;

            Properties.Powermeter.Default.Reference = PowerMeter.lossReference;
            Properties.Powermeter.Default.Addr = PowerMeter.addr;
            Properties.Powermeter.Default.Channel = PowerMeter.channel;

            Properties.Alignment.Default.LossCriteria = lossCriteria;
            Properties.Alignment.Default.ProductName = productName;
            Properties.Alignment.Default.DoubleCheck = doublecheckFlag;
            Properties.Alignment.Default.StopInBetween = stopInBetweenFlag;
        }

        public void LoadAll()
        {
            beetleT1ComPortName = Properties.Fixture.Default.T1COM;
            beetleT2ComPortName = Properties.Fixture.Default.T2COM;
            beetleT3ComPortName = Properties.Fixture.Default.T3COM;
            arduinoComPortName = Properties.Fixture.Default.ArdnCOM;
            initialPosition[0] = Properties.Fixture.Default.InitialX;
            initialPosition[1] = Properties.Fixture.Default.InitialY;
            initialPosition[2] = Properties.Fixture.Default.InitialZ;
            initialPosition[3] = Properties.Fixture.Default.InitialRx;
            initialPosition[4] = Properties.Fixture.Default.InitialRy;
            initialPosition[5] = Properties.Fixture.Default.InitialRz;
            pivotPoint[0] = Properties.Fixture.Default.PivotX;
            pivotPoint[1] = Properties.Fixture.Default.PivotY;
            pivotPoint[2] = Properties.Fixture.Default.PivotZ;
            position[0] = Properties.Fixture.Default.InitialX;
            position[1] = Properties.Fixture.Default.InitialY;
            position[2] = Properties.Fixture.Default.InitialZ;
            position[3] = Properties.Fixture.Default.InitialRx;
            position[4] = Properties.Fixture.Default.InitialRy;
            position[5] = Properties.Fixture.Default.InitialRz;
            beetleFixtureNumber = Properties.Fixture.Default.BeetleFixtureNum;
            beetleControlBoxNum = Properties.Fixture.Default.BeetleControlBoxNum;

            PowerMeter.lossReference = Properties.Powermeter.Default.Reference;
            PowerMeter.addr = Properties.Powermeter.Default.Addr;
            PowerMeter.channel = Properties.Powermeter.Default.Channel;

            lossCriteria = Properties.Alignment.Default.LossCriteria;
            productName = Properties.Alignment.Default.ProductName;
            doublecheckFlag = Properties.Alignment.Default.DoubleCheck;
            stopInBetweenFlag = Properties.Alignment.Default.StopInBetween;

            Console.WriteLine("Parameters Loaded");
            Log("Parameters Loaded");
        }

        // TODO: save system 2 all other parameters
        public void LoadAll_2()
        {
            beetleT1ComPortName = Properties.Fixture.Default.T1COM_2;
            beetleT2ComPortName = Properties.Fixture.Default.T2COM_2;
            beetleT3ComPortName = Properties.Fixture.Default.T3COM_2;
            arduinoComPortName = Properties.Fixture.Default.ArdnCOM_2;
            initialPosition[0] = Properties.Fixture.Default.InitialX;
            initialPosition[1] = Properties.Fixture.Default.InitialY;
            initialPosition[2] = Properties.Fixture.Default.InitialZ;
            initialPosition[3] = Properties.Fixture.Default.InitialRx;
            initialPosition[4] = Properties.Fixture.Default.InitialRy;
            initialPosition[5] = Properties.Fixture.Default.InitialRz;
            pivotPoint[0] = Properties.Fixture.Default.PivotX;
            pivotPoint[1] = Properties.Fixture.Default.PivotY;
            pivotPoint[2] = Properties.Fixture.Default.PivotZ;
            position[0] = Properties.Fixture.Default.InitialX;
            position[1] = Properties.Fixture.Default.InitialY;
            position[2] = Properties.Fixture.Default.InitialZ;
            position[3] = Properties.Fixture.Default.InitialRx;
            position[4] = Properties.Fixture.Default.InitialRy;
            position[5] = Properties.Fixture.Default.InitialRz;
            beetleFixtureNumber = Properties.Fixture.Default.BeetleFixtureNum_2;
            beetleControlBoxNum = Properties.Fixture.Default.BeetleControlBoxNum_2;

            PowerMeter.lossReference = Properties.Powermeter.Default.Reference;
            PowerMeter.addr = Properties.Powermeter.Default.Addr;
            PowerMeter.channel = Properties.Powermeter.Default.Channel;

            lossCriteria = Properties.Alignment.Default.LossCriteria;
            productName = Properties.Alignment.Default.ProductName;
            doublecheckFlag = Properties.Alignment.Default.DoubleCheck;
            stopInBetweenFlag = Properties.Alignment.Default.StopInBetween;

            Console.WriteLine("Sys2 Parameters Loaded");
            Log("Sys2 Parameters Loaded");
        }

        public void SaveCOMPorts()
        {
            Properties.Fixture.Default.T1COM = beetleT1ComPortName;
            Properties.Fixture.Default.T2COM = beetleT2ComPortName;
            Properties.Fixture.Default.T3COM = beetleT3ComPortName;
            Properties.Fixture.Default.ArdnCOM = arduinoComPortName;

            Properties.Fixture.Default.Save();
            Console.WriteLine("COM Ports Saved");
            //Log("COM Ports Saved");
        }

        // for system 2
        public void SaveCOMPorts_2()
        {
            Properties.Fixture.Default.T1COM_2 = beetleT1ComPortName;
            Properties.Fixture.Default.T2COM_2 = beetleT2ComPortName;
            Properties.Fixture.Default.T3COM_2 = beetleT3ComPortName;
            Properties.Fixture.Default.ArdnCOM_2 = arduinoComPortName;

            Properties.Fixture.Default.Save();
            Console.WriteLine("SYS2 COM Ports Saved");
            //Log("COM Ports Saved");
        }

        public void SavePMChl()
        {
            Properties.Powermeter.Default.Channel = PowerMeter.channel;
            Properties.Powermeter.Default.Save();
            Console.WriteLine("Power Meter Channel Saved");
        }

        public void SaveReference()
        {
            Properties.Powermeter.Default.Reference = PowerMeter.lossReference;
            Properties.Powermeter.Default.Save();
            Console.WriteLine("Reference Saved");
        }

        public void SaveInitialPosition()
        {
            Properties.Fixture.Default.InitialX = initialPosition[0];
            Properties.Fixture.Default.InitialY = initialPosition[1];
            Properties.Fixture.Default.InitialZ = initialPosition[2];
            Properties.Fixture.Default.InitialRx = initialPosition[3];
            Properties.Fixture.Default.InitialRy = initialPosition[4];
            Properties.Fixture.Default.InitialRz = initialPosition[5];

            Properties.Fixture.Default.Save();
            Console.WriteLine("Initial Position Saved");
            Log("Initial Position Saved");
        }

        public void SavePivotPoint()
        {
            Properties.Fixture.Default.PivotX = pivotPoint[0];
            Properties.Fixture.Default.PivotY = pivotPoint[1];
            Properties.Fixture.Default.PivotZ = pivotPoint[2];

            Properties.Fixture.Default.Save();
            Console.WriteLine("Pivot Point Saved");
            Log("Pivot Point Saved");
        }

        public static void Log(string logMessage)
        {
            logPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            //Console.WriteLine(logPath);

            using (StreamWriter w = File.AppendText(Path.Combine(logPath,"BeetleLog.txt")))
            {
                w.Write($"{DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")}: ");
                w.WriteLine($"{logMessage}");
            }
        }

    }
}
