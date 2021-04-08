using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Beetle
{
    static class Parameters
    {
        // Fixture parameteres
        public static string beetleT1ComPortName = "";
        public static string beetleT2ComPortName = "";
        public static string beetleT3ComPortName = "";
        public static string arduinoComPortName = ""; // for piezo control
        public static sbyte beetleFixtureNumber = 3;
        // Pivot point coordinates relative to the center (x,y,z,1) of moving plate joint surface, z need to minus 8 to become pivotpoint to top moving part top surface distance
        public static double[] pivotPoint = { 0, 0, 42, 0 };
        public static double[] position = { 0, 0, 140, 2.4, -1.0, 0 }; // Position in mm { x, y, z, Rx, Ry, Rz}
        public static double[] initialPosition = { 0, 0, 138, 0, 0, 0 }; // This is the starting(Or Initial) position
        public static ushort[] piezoPosition = { 0x800, 0x800, 0x800 }; // Piezo position (x, y, z) in DAC value ranging from 0x000 to 0xfff. 
        public static bool usePiezo = false;
        public static byte piezoZvsGap = 1; // if piezo z DAC value increase, gap is larger, then 1; is z DAC value incease, gap is smaller, then -1
        public static bool piezoRunning = false;
        public static ushort piezoStepSize = 4; // in DAC value

        // Power Meter paramters
        public static double loss = -50.0f;
        public static double lossReference = -19.4588f;
        public static byte addr = 12;
        public static byte channel = 2;

        // Alignment/Curing parameters
        public static string errors = "";
        public static bool errorFlag = false;
        public static double lossCriteria = -0.1f;
        public static double lossCurrentMax = -50.0f;
        public static bool doublecheckFlag = false;
        public static bool stopInBetweenFlag = false;
        public static bool smallestResolution = false;
        public static bool highestAccuracy = true;

        public static string productName = "MM 1xN";
        // Product type and its Gap or focal length (to be complete)
        // Gap or focal length can be get through productName: Parameters.product[Parameters.productName]
        public static Dictionary<string, float> productGap =
            new Dictionary<string, float>()
            {
                { "VOA", 0.05f },
                { "SM 1xN", 0.2f },
                { "MM 1xN", 0.14f },
                { "UWDM", 0.05f }
            };

        public static void Save()
        {
            SaveCOMPorts();
            Properties.Fixture.Default.BeetleFixtureNum = beetleFixtureNumber;

            Properties.Powermeter.Default.Reference = lossReference;
            Properties.Powermeter.Default.Addr = addr;
            Properties.Powermeter.Default.Channel = channel;

            Properties.Alignment.Default.LossCriteria = lossCriteria;
            Properties.Alignment.Default.ProductName = productName;
            Properties.Alignment.Default.DoubleCheck = doublecheckFlag;
            Properties.Alignment.Default.StopInBetween = stopInBetweenFlag;
        }

        public static void LoadAll()
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
            beetleFixtureNumber = Properties.Fixture.Default.BeetleFixtureNum;

            lossReference = Properties.Powermeter.Default.Reference;
            addr = Properties.Powermeter.Default.Addr;
            channel = Properties.Powermeter.Default.Channel;

            lossCriteria = Properties.Alignment.Default.LossCriteria;
            productName = Properties.Alignment.Default.ProductName;
            doublecheckFlag = Properties.Alignment.Default.DoubleCheck;
            stopInBetweenFlag = Properties.Alignment.Default.StopInBetween;

            Console.WriteLine("Parameters Loaded");
            Log("Parameters Loaded");
        }

        public static void SaveCOMPorts()
        {
            Properties.Fixture.Default.T1COM = beetleT1ComPortName;
            Properties.Fixture.Default.T2COM = beetleT2ComPortName;
            Properties.Fixture.Default.T3COM = beetleT3ComPortName;
            Properties.Fixture.Default.ArdnCOM = arduinoComPortName;

            Properties.Fixture.Default.Save();
            Console.WriteLine("COM Ports Saved");
            //Log("COM Ports Saved");
        }

        public static void SavePMChl()
        {
            Properties.Powermeter.Default.Channel = channel;
            Properties.Powermeter.Default.Save();
            Console.WriteLine("Power Meter Channel Saved");
        }

        public static void SaveReference()
        {
            Properties.Powermeter.Default.Reference = lossReference;
            Properties.Powermeter.Default.Save();
            Console.WriteLine("Reference Saved");
        }

        public static void SaveInitialPosition()
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

        public static void SavePivotPoint()
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
            using (StreamWriter w = File.AppendText("log.txt"))
            {
                w.Write($"{DateTime.Now.ToLongTimeString()}: ");
                w.WriteLine($"{logMessage}");
            }
        }

    }
}
