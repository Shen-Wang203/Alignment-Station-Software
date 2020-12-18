using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Beetle
{
    static class GlobalVar
    {
        public static string beetleT1ComPortName = "";
        public static string beetleT2ComPortName = "";
        public static string beetleT3ComPortName = "";
        public static byte beetleFixtureNumber = 3;
        // Pivot point coordinates relative to the center (x,y,z,1) of moving plate
        public static double[] pivotPoint = { 0, 0, 42, 0 };

        public static string errors = "";
        public static bool errorFlag = false;
        // Position in mm { x, y, z, Rx, Ry, Rz}
        public static double[] position = { 0, 0, 138, 0, 0, 0 };
        public static double[] initialPosition = new double[6] { 0, 0, 138, 0, 0, 0 }; // This is the starting(Or Initial) position
        public static double loss = -50.0f;
        public static double lossReference = -14.4326;
        public static double lossCriteria = -0.2;

        public static string productName = "MM1xN";
        // Product type and its Gap or focal length (to be complete)
        // Gap or focal length can be get through productName: GlobalVar.product[GlobalVar.productName]
        public static Dictionary<string, float> productGap =
            new Dictionary<string, float>()
            {
                { "VOA", 0.05f },
                { "SM1xN", 0.2f },
                { "MM1xN", 0.14f },
                { "UWDM", 0.05f }
            };

        public static void SaveCOMPorts()
        {
            Properties.Settings.Default.T1COM = beetleT1ComPortName;
            Properties.Settings.Default.T2COM = beetleT2ComPortName;
            Properties.Settings.Default.T3COM = beetleT3ComPortName;

            Properties.Settings.Default.Save();
            Console.WriteLine("COM Ports Saved");
        }

        public static void SaveInitialPosition()
        {
            Properties.Settings.Default.InitialX = position[0];
            Properties.Settings.Default.InitialY = position[1];
            Properties.Settings.Default.InitialZ = position[2];
            Properties.Settings.Default.InitialRx = position[3];
            Properties.Settings.Default.InitialRy = position[4];
            Properties.Settings.Default.InitialRz = position[5];

            Properties.Settings.Default.Save();
            Console.WriteLine("Initial Position Saved");
        }

        public static void SavePivotPoint()
        {
            Properties.Settings.Default.PivotX = pivotPoint[0];
            Properties.Settings.Default.PivotY = pivotPoint[1];
            Properties.Settings.Default.PivotZ = pivotPoint[2];

            Properties.Settings.Default.Save();
            Console.WriteLine("Pivot Point Saved");
        }

        public static void LoadParameters()
        {
            beetleT1ComPortName = Properties.Settings.Default.T1COM;
            beetleT2ComPortName = Properties.Settings.Default.T2COM;
            beetleT3ComPortName = Properties.Settings.Default.T3COM;
            initialPosition[0] = Properties.Settings.Default.InitialX;
            initialPosition[1] = Properties.Settings.Default.InitialY;
            initialPosition[2] = Properties.Settings.Default.InitialZ;
            initialPosition[3] = Properties.Settings.Default.InitialRx;
            initialPosition[4] = Properties.Settings.Default.InitialRy;
            initialPosition[5] = Properties.Settings.Default.InitialRz;
            pivotPoint[0] = Properties.Settings.Default.PivotX;
            pivotPoint[1] = Properties.Settings.Default.PivotY;
            pivotPoint[2] = Properties.Settings.Default.PivotZ;

            Console.WriteLine("Parameters Loaded");
        }
        
    }
}
