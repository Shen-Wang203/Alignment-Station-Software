﻿using System;
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
        public static string beetleT1ComPortName = "COM10";
        public static string beetleT2ComPortName = "COM9";
        public static string beetleT3ComPortName = "COM15";
        public static byte beetleFixtureNumber = 3;
        // Pivot point coordinates relative to the center (x,y,z,1) of moving plate
        public static float[] pivotPoint = { 0, 0, 42, 0 };

        public static string errors = "";
        public static bool errorFlag = false;
        // Position in mm { x, y, z, Rx, Ry, Rz}
        //public static double[] position = { 0, 0, 140.4, 0, 0, 0 };
        public static double[] position = { 0, 0, 138, 0, 0, 0 };
        public static double loss = -50.0f;
        public static double lossReference = -14.4326;
        public static double lossCriteria = -0.2;

        public static string productName = "MM1xN";
        // Product type and its focal length (to be complete)
        // focal length can be get through productName: GlobalVar.product[GlobalVar.productName]
        public static Dictionary<string, float> product =
            new Dictionary<string, float>()
            {
                { "VOA", 0.05f },
                { "SM1xN", 0.2f },
                { "MM1xN", 0.14f },
                { "UWDM", 0.05f }
            };
        
        
    }
}
