using System;
using System.Numerics;
using System.Text;

namespace ConsoleAppForTesting
{
    public class Program
    {
        public static int currentValue = 0;
        static void Main(string[] args)
        {
            long x = 128;
            long y = 130;
            long loss = 11;
            long timeLimit = 1000000;

            ExtensionMethods.PrintGrid(x, y, loss, timeLimit);

            ExtensionMethods.CalcElderAge(x, y, loss, timeLimit);

            ExtensionMethods.WipElderAge(x, y, loss, timeLimit);


            Console.ReadLine();
        }
    }

    public static class ExtensionMethods
    {
        public static void WipElderAge(long x, long y, long loss, long timeLimit)
        {
            BigInteger eldersTime = 0;
            // if loss is 7, this is the 4th odd so 7 * 4 (calculated as below)
            var oddNumberIndex = (loss + 1) / 2;

            BigInteger numTerms = 0;
            if (loss > 0)
            {

                numTerms = x - (loss + 1);
            }
            else
            {
                numTerms = x - 1;
            }


            BigInteger aSum = (numTerms * ( 1 + x - (loss + 1))) / 2;

            eldersTime += aSum * y;

            BigInteger toAdd1 = 0;
            BigInteger tempTerm = y / x;
            for (int i = 1; i < tempTerm; i++)
            {
                toAdd1 += i * x * x;                
                toAdd1 -= (loss * oddNumberIndex);
                //toAdd1 *= x - 1;
                toAdd1 *= x;


                eldersTime += toAdd1;
                toAdd1 = 0;
            }
            //eldersTime += toAdd1;



            BigInteger toAdd = (tempTerm) * x * x;
            var sub = loss * oddNumberIndex;
            toAdd -= loss * oddNumberIndex;
            toAdd *= y % x;

            eldersTime += toAdd;

            Console.WriteLine($"Elder age = {eldersTime}");
        }

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

                sb.Append($" -- Row {i}");

                Console.WriteLine($"{sb}   = {sumOfRow}, diff 1st row = {sumOfRow - sumFirstRow}");
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
            Console.WriteLine($"Elder age = {eldersTime}");
        }
    }
}


