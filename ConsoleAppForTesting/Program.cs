using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Linq;

namespace ConsoleAppForTesting
{
    public class Program
    {
        public static int currentValue = 0;
        static void Main(string[] args)
        {
            // fails for below
            // 545, 435, 342, 1000007
            //long x = 545;
            //long y = 435;
            //long loss = 342;
            //long timeLimit = 1000007;

            long x = 45;
            long y = 35;
            //long loss = 2;
            long loss = 0;
            long timeLimit = 1000007;



            ExtensionMethods.PrintGrid(x, y, loss, timeLimit);

            ExtensionMethods.CalcElderAge(x, y, loss, timeLimit);



            // another attempt
            long eldersAge = MagicRectangle.CalculateElderAge(x, y, loss, timeLimit);

            Console.WriteLine($"eqn based elders age = {eldersAge}");

            Console.ReadLine();
        }
    }

    public static class ExtensionMethods
    {
        public static void PrintGrid(long x, long y, long loss, long timeLimit)
        {
            long eldersTime = 0;
            long xor = 0;
            //long largerAxis = n > m ? n : m;
            //long smallerAxis = n > m ? m : n;

            long smallerAxis = y;
            long largerAxis = x;

            long sumFirstRow = 0;
            for (int i = 0; i < smallerAxis; i++)
            {
                long sumOfRow = 0;
                StringBuilder sb = new StringBuilder();
                for (int j = 0; j < largerAxis; j++)
                {
                    xor = i ^ j;
                    if (xor > loss)
                    {
                        long calcValue = xor - loss;
                        sb.Append(calcValue.ToString("00"));
                        sumOfRow += calcValue;
                        if (i == 0)
                        {
                            sumFirstRow += calcValue;
                        }
                    }
                    else
                    {
                        sb.Append(0.ToString("00"));
                    }
                    sb.Append(" ");

                }

                sb.Append($" -- Row {i.ToString("00")}");

                Console.WriteLine($"{sb}   sum={sumOfRow}, diff 1st row = {sumOfRow - sumFirstRow}");
            }

            //for (long i = loss+1; i < n; i++)
            //{
            //    for (long j = 0; j < m; j++)
            //    {
            //        xor = i ^ j;
            //        if (xor > loss)
            //        {

            //            eldersTime += xor - loss;
            //            //eldersTime = AddTimeToElder(eldersTime, timeLimit, (xor - loss));
            //        }
            //    }
            //}

            //for (long i = 0; i < loss+1; i++)
            //{
            //    for (long j = loss+1; j < m; j++)
            //    {
            //        xor = i ^ j;
            //        if (xor > loss)
            //        {

            //            eldersTime += xor - loss;
            //            //eldersTime = AddTimeToElder(eldersTime, timeLimit, (xor - loss));
            //        }
            //    }
            //}
        }

        internal static void CalcElderAge(long n, long m, long loss, long timeLimit)
        {
            long eldersTime = 0;
            long xor = 0;
            //long largerAxis = n > m ? n : m;
            //long smallerAxis = n > m ? m : n;

            long largerAxis = m;
            long smallerAxis = n;


            for (int i = 0; i < smallerAxis; i++)
            {
                for (int j = 0; j < largerAxis; j++)
                {
                    xor = i ^ j;
                    if (xor > loss)
                    {
                        long calcValue = xor - loss;
                        eldersTime += calcValue;
                    }
                }
            }
            Console.WriteLine($"naive Elder age = {eldersTime}");
        }


    }
}


