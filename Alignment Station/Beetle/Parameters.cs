using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Beetle
{
    static class Parameters
    {

        // Fixture parameteres
        public static string beetleT1ComPortName = "";
        public static string beetleT2ComPortName = "";
        public static string beetleT3ComPortName = "";
        public static sbyte beetleFixtureNumber = 3;
        // Pivot point coordinates relative to the center (x,y,z,1) of moving plate
        public static double[] pivotPoint = { 0, 0, 42, 0 };
        public static double[] position = { 0, 0, 138, 0, 0, 0 }; // Position in mm { x, y, z, Rx, Ry, Rz}
        public static double[] initialPosition = { 0, 0, 138, 0, 0, 0 }; // This is the starting(Or Initial) position

        // Power Meter paramters
        public static double loss = -50.0f;
        public static double lossReference = -14.4326;
        public static string addr = "12";
        public static string channel = "1";

        // Alignment parameters
        public static string errors = "";
        public static bool errorFlag = false;
        public static double lossCriteria = -0.2;
        public static double lossCurrentMax = -50.0;
        public static bool doublecheckFlag = false;
        public static bool stopInBetweenFlag = false;
        public static bool smallestResolution = false;

        public static string productName = "SM1xN";
        // Product type and its Gap or focal length (to be complete)
        // Gap or focal length can be get through productName: Parameters.product[Parameters.productName]
        public static Dictionary<string, float> productGap =
            new Dictionary<string, float>()
            {
                { "VOA", 0.05f },
                { "SM1xN", 0.2f },
                { "MM1xN", 0.14f },
                { "UWDM", 0.05f }
            };

        public static void SaveAll()
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
        }

        public static void SaveCOMPorts()
        {
            Properties.Fixture.Default.T1COM = beetleT1ComPortName;
            Properties.Fixture.Default.T2COM = beetleT2ComPortName;
            Properties.Fixture.Default.T3COM = beetleT3ComPortName;

            Properties.Fixture.Default.Save();
            Console.WriteLine("COM Ports Saved");
        }

        public static void SaveInitialPosition()
        {
            Properties.Fixture.Default.InitialX = position[0];
            Properties.Fixture.Default.InitialY = position[1];
            Properties.Fixture.Default.InitialZ = position[2];
            Properties.Fixture.Default.InitialRx = position[3];
            Properties.Fixture.Default.InitialRy = position[4];
            Properties.Fixture.Default.InitialRz = position[5];

            Properties.Fixture.Default.Save();
            Console.WriteLine("Initial Position Saved");
        }

        public static void SavePivotPoint()
        {
            Properties.Fixture.Default.PivotX = pivotPoint[0];
            Properties.Fixture.Default.PivotY = pivotPoint[1];
            Properties.Fixture.Default.PivotZ = pivotPoint[2];

            Properties.Fixture.Default.Save();
            Console.WriteLine("Pivot Point Saved");
        }

        
        
    }
}
