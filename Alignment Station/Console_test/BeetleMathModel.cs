using System;
using System.Collections.Generic;
using System.Text;

namespace Console_test
{
    static class BeetleMathModel
    {
        // Rotation first, relative to the pivot point; Translate second
        // Pivot point is based on center of the moving plate
        // Angle is in degree. Rx Pitch; Ry Roll; Rz Yaw
        // r is the moving plate hex radius, L is the arm length
        private static float R = 50f;
        private static double r = R / Math.Cos(Math.PI / 6);
        private static float L = 78.5f;
        // This is the height of base and top moving part thickness, this is a fixture fixed parameter, find it from the 3D model. 
        private static float baseZ = 65.9221f + 8f;
        // Pivot point coordinates relative to the center (x,y,z,1) of moving plate
        //GlobalVar.pivotPoint = { 0, 0, 0, 0 };

        public static float SetR
        {
            //get { return R; }
            set { R = value; r = value / Math.Cos(Math.PI / 6); }
        }

        public static float SetL
        {
            //get { return L; }
            set { L = value; }
        }

        public static float SetBaseZ
        {
            //get { return baseZ; }
            set { baseZ = value + 8f; }
        }

        public static float[] SetPivotPoint
        {
            //get { return pivotPoint; }
            set { GlobalVar.pivotPoint = value; }
        }

        //Matrix multiply C = A.B, A and B can be any dimension
        private static double[,] MxM(double[,] A, double[,] B)
        {
            int row = A.GetLength(0);
            int column = B.GetLength(1);
            int element = A.GetLength(1);
            double[,] C = new double[row, column];
            
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    for (int k = 0; k < element; k++)
                    {
                        C[i, j] += A[i, k] * B[k, j];
                    }
                }
            }
            return C;
        }

        //Matrix times Array C = A.B, B is [n,1] array
        private static double[] MxA(double[,] A, double[] B)
        {
            int row = A.GetLength(0);
            int element = A.GetLength(1);
            double[] C = new double[row];

            for (int i = 0; i < row; i++)
            {
                for (int k = 0; k < element; k++)
                {
                    C[i] += A[i, k] * B[k];
                }
            }
            return C;
        }

        private static double[,] MRz(double Rz)
        {
            double angle = (Math.PI / 180) * Rz;
            double[,] mrz = { { Math.Cos(angle), -Math.Sin(angle), 0, 0 },
                              { Math.Sin(angle), Math.Cos(angle),  0, 0 },
                              { 0,               0,                1, 0 },
                              { 0,               0,                0, 1 } };
            return mrz;
        }

        private static double[,] MRy(double Ry)
        {
            double angle = (Math.PI / 180) * Ry;
            double[,] mry = { { Math.Cos(angle),  0, Math.Sin(angle), 0 },
                              { 0,                1, 0,               0 },
                              { -Math.Sin(angle), 0, Math.Cos(angle), 0 },
                              { 0,                0, 0,               1 } };
            return mry;
        }

        private static double[,] MRx(double Rx)
        {
            double angle = (Math.PI / 180) * Rx;
            double[,] mrx = { { 1,               0, 0,                0 },
                              { 0, Math.Cos(angle), -Math.Sin(angle), 0 },
                              { 0, Math.Sin(angle), Math.Cos(angle),  0 },
                              { 0,               0, 0,                1 } };
            return mrx;
        }

        private static double[,] MT(double x, double y, double z)
        {
            double[,] mt = {{ 1, 0, 0, x },
                            { 0, 1, 0, y },
                            { 0, 0, 1, z },
                            { 0, 0, 0, 1 }};
            return mt;
        }

        // Find each axis's position in mm based on platform's target position
        public static double[] FindAxialPosition(double x, double y, double z, double Rx, double Ry, double Rz)
        {
            // The z in the model excludes the base height and top moving plate thickness
             z -= baseZ;
            // Coordinate center is in pivot point, let's name it pivot point coordinate
            // Original coordinate's center is in the geometry center
            double[] a = { 0, r, 0, 1 };
            for (int i = 0; i < 4; i++)
                a[i] -= GlobalVar.pivotPoint[i];
            double[] b = { r * Math.Cos(Math.PI * 5 / 6), r * Math.Sin(Math.PI * 5 / 6), 0, 1 };
            for (int i = 0; i < 4; i++)
                b[i] -= GlobalVar.pivotPoint[i];
            double[] c = { r * Math.Cos(-Math.PI * 5 / 6), r * Math.Sin(-Math.PI * 5 / 6), 0, 1 };
            for (int i = 0; i < 4; i++)
                c[i] -= GlobalVar.pivotPoint[i];
            double[] d = { 0, -r, 0, 1 };
            for (int i = 0; i < 4; i++)
                d[i] -= GlobalVar.pivotPoint[i];
            double[] e = { r * Math.Cos(-Math.PI / 6), r * Math.Sin(-Math.PI / 6), 0, 1 };
            for (int i = 0; i < 4; i++)
                e[i] -= GlobalVar.pivotPoint[i];
            double[] f = { r * Math.Cos(Math.PI / 6), r * Math.Sin(Math.PI / 6), 0, 1 };
            for (int i = 0; i < 4; i++)
                f[i] -= GlobalVar.pivotPoint[i];

            // Coordinate transform based on pivot point
            double[] aa = MxA(MT(x, y, z), MxA(MRx(Rx), MxA(MRy(Ry), MxA(MRz(Rz), a))));
            double[] bb = MxA(MT(x, y, z), MxA(MRx(Rx), MxA(MRy(Ry), MxA(MRz(Rz), b))));
            double[] cc = MxA(MT(x, y, z), MxA(MRx(Rx), MxA(MRy(Ry), MxA(MRz(Rz), c))));
            double[] dd = MxA(MT(x, y, z), MxA(MRx(Rx), MxA(MRy(Ry), MxA(MRz(Rz), d))));
            double[] ee = MxA(MT(x, y, z), MxA(MRx(Rx), MxA(MRy(Ry), MxA(MRz(Rz), e))));
            double[] ff = MxA(MT(x, y, z), MxA(MRx(Rx), MxA(MRy(Ry), MxA(MRz(Rz), f))));

            // Middle point of edges
            double[] Mbc = { (bb[0] + cc[0]) / 2, (bb[1] + cc[1]) / 2, (bb[2] + cc[2]) / 2, 1 };
            double[] Mde = { (dd[0] + ee[0]) / 2, (dd[1] + ee[1]) / 2, (dd[2] + ee[2]) / 2, 1 };
            double[] Mfa = { (ff[0] + aa[0]) / 2, (ff[1] + aa[1]) / 2, (ff[2] + aa[2]) / 2, 1 };

            // Vector of edges
            double[] Vbc = { cc[0] - bb[0], cc[1] - bb[1], cc[2] - bb[2] };
            double[] Vde = { ee[0] - dd[0], ee[1] - dd[1], ee[2] - dd[2] };
            double[] Vfa = { aa[0] - ff[0], aa[1] - ff[1], aa[2] - ff[2] };

            // Find the position of T1(x,y,-pivotPoint(3))
            // Check pdf file of Find_T_in_World
            double A, B, C;
            double T1x, T1y, T2x, T2y, T3x, T3y;
            // Find the position of T1
            double s = Mbc[0]; double t = Mbc[1]; double u = Mbc[2] + GlobalVar.pivotPoint[2];
            double p = Vbc[0]; double q = Vbc[1]; double v = Vbc[2];
            if (p == 0)
            {
                T1y = t + v * u / q;
                T1x = s - Math.Sqrt(L * L - u * u - (v * u) * (v * u) / (q * q)); // pick the smaller one
            }
            // for T1, q is not possible to be 0(turn 90 degree is not possible)
            else if (q == 0)
            {
                T1x = s + v * u / p;
                T1y = t - Math.Sqrt(L * L - u * u - (v * u) * (v * u) / (q * q));
            }
            // T1y2 = t + sqrt(L ^ 2 - u ^ 2 - (r * u) ^ 2 / (q ^ 2));
            else
            {
                A = 1 + (p / q) * (p / q);
                B = 2 * p * v * u / (q * q);
                C = (v * u) * (v * u) / (q * q) + u * u - L * L;
            
                double T1x1 = s + 0.5 * B / A - 0.5 * Math.Sqrt(B * B - 4 * A * C) / A;
                double T1x2 = s + 0.5 * B / A + 0.5 * Math.Sqrt(B * B - 4 * A * C) / A;
                double T1y1 = t + v * u / q + p * s / q - p * T1x1 / q;
                double T1y2 = t + v * u / q + p * s / q - p * T1x2 / q;

                if (T1x1 > T1x2)
                {
                    T1x = T1x2;
                    T1y = T1y2;
                }
                else
                {
                    T1x = T1x1;
                    T1y = T1y1;
                }
            }

            // Find the position of T2
            s = Mde[0]; t = Mde[1]; u = Mde[2] + GlobalVar.pivotPoint[2];
            p = Vde[0]; q = Vde[1]; v = Vde[2];
            if (p == 0)
            {
                T2y = t + v * u / q;
                T2x = s + Math.Sqrt(L * L - u * u - (v * u) * (v * u) / (q * q)); // pick the larger one
            }
            else if (q == 0)
            {
                T2x = s + v * u / p;
                T2y = t - Math.Sqrt(L * L - u * u - (v * u) * (v * u) / (q * q)); // pick the smaller one
            }
            // T2y2 = t + sqrt(L^2-u^2-(r*u)^2/(q^2));
            else
            {
                A = 1 + (p / q) * (p / q);
                B = 2 * p * v * u / (q * q);
                C = (v * u) * (v * u) / (q * q) + u * u - L * L;

                double T2x1 = s + 0.5 * B / A - 0.5 * Math.Sqrt(B * B - 4 * A * C) / A;
                double T2x2 = s + 0.5 * B / A + 0.5 * Math.Sqrt(B * B - 4 * A * C) / A;
                double T2y1 = t + v * u / q + p * s / q - p * T2x1 / q;
                double T2y2 = t + v * u / q + p * s / q - p * T2x2 / q;

                if (T2x1 < T2x2)
                {
                    T2x = T2x2;
                    T2y = T2y2;
                }
                else
                {
                    T2x = T2x1;
                    T2y = T2y1;
                }
            }

            // Find the position of T3
            s = Mfa[0]; t = Mfa[1]; u = Mfa[2] + GlobalVar.pivotPoint[2];
            p = Vfa[0]; q = Vfa[1]; v = Vfa[2];
            if (p == 0)
            {
                T3y = t + v * u / q;
                T3x = s + Math.Sqrt(L * L - u * u - (v * u) * (v * u) / (q * q)); // pick the larger one
            }
            else if (q == 0)
            {
                T3x = s + v * u / p;
                T3y = t + Math.Sqrt(L * L - u * u - (v * u) * (v * u) / (q * q)); // pick the larger one
            }
            else
            {
                A = 1 + (p / q) * (p / q);
                B = 2 * p * v * u / (q * q);
                C = (v * u) * (v * u) / (q * q) + u * u - L * L;

                double T3x1 = s + 0.5 * B / A - 0.5 * Math.Sqrt(B * B - 4 * A * C) / A;
                double T3x2 = s + 0.5 * B / A + 0.5 * Math.Sqrt(B * B - 4 * A * C) / A;
                double T3y1 = t + v * u / q + p * s / q - p * T3x1 / q;
                double T3y2 = t + v * u / q + p * s / q - p * T3x2 / q;

                if (T3x1 < T3x2)
                {
                    T3x = T3x2;
                    T3y = T3y2;
                }
                else
                {
                    T3x = T3x1;
                    T3y = T3y1;
                }
            }

            // The T points location at original/center coordinate
            T1x += GlobalVar.pivotPoint[0];
            T1y += GlobalVar.pivotPoint[1];
            T2x += GlobalVar.pivotPoint[0];
            T2y += GlobalVar.pivotPoint[1];
            T3x += GlobalVar.pivotPoint[0];
            T3y += GlobalVar.pivotPoint[1];
            double[] T = { T1x, T1y, T2x, T2y, T3x, T3y };

            return T;
        }
    }
}
