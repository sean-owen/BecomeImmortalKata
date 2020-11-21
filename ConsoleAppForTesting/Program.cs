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
            long x = 33;

            long y = 18;
            long loss = 0;
            long timeLimit = 1000000;

            ExtensionMethods.PrintGrid(x, y, loss, timeLimit);

            ExtensionMethods.CalcElderAge(x, y, loss, timeLimit);

            var firstXValue = 1;
            do
            {
                firstXValue *= 2;
            } while (firstXValue <= x);
            firstXValue /= 2;
            BigInteger core = ExtensionMethods.WipElderAge(firstXValue, y, loss, timeLimit);
            BigInteger eldersAge = core;

            if (x % firstXValue > 0)
            {
                var secondXValue = x - firstXValue;
                BigInteger additional = ExtensionMethods.ElderAgeFrom2PowNMultiple_ToX(x, y, loss, timeLimit);
                eldersAge += (secondXValue * additional);

                // calculating contribution of last rows that are not a full multiple of firstXValue
                BigInteger leftoverRows = y % firstXValue;
                BigInteger firstLeftoverRow = y - leftoverRows;
                BigInteger valueToAsumFrom = firstLeftoverRow - firstXValue;
                // aSum from valueToAsumFrom to valueToAsumFrom + leftOverRows
                BigInteger aSum = (leftoverRows * (valueToAsumFrom + valueToAsumFrom + leftoverRows - 1)) / 2;

                eldersAge += aSum;
            }
            Console.WriteLine($"eqn based elders age = {eldersAge}");

            Console.ReadLine();
        }
    }

    public static class ExtensionMethods
    {
        private static List<BigInteger> extraValues = new List<BigInteger>();

        // it makes a difference if (y / x == an even number) VS if (y / x == an odd number)
        public static BigInteger ElderAgeFrom2PowNMultiple_ToX(long x, long y, long loss, long timeLimit)
        {
            // if (y / x == odd number)
            BigInteger[] extraValuesArray = extraValues.ToArray();
            BigInteger lastLinesIfRowsLeftIsZero = extraValuesArray[extraValuesArray.Length - 2];
            extraValuesArray[extraValuesArray.Length - 2] = 0;

            BigInteger additionalAge = 0;
            foreach (BigInteger entry in extraValuesArray)
            {
                additionalAge += entry;
            }
          
            return additionalAge;
        }

        public static BigInteger WipElderAge(long x, long y, long loss, long timeLimit)
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
            extraValues.Add(aSum);
            eldersTime += aSum * y;

            BigInteger toAdd1 = 0;
            BigInteger tempTerm = y / x;
            for (int i = 1; i < tempTerm; i++)
            {
                toAdd1 += i * x * x;                
                toAdd1 -= (loss * oddNumberIndex);
                extraValues.Add(toAdd1 + aSum);
                //toAdd1 *= x - 1;
                toAdd1 *= x;


                eldersTime += toAdd1;
                toAdd1 = 0;
            }



            BigInteger toAdd = (tempTerm) * x * x;
            var sub = loss * oddNumberIndex;
            toAdd -= loss * oddNumberIndex;
            extraValues.Add(toAdd + aSum);
            toAdd *= y % x;

            eldersTime += toAdd;

            return eldersTime;
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


