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
            // only works with even condition extra lines commented out
            //long x = 1000;
            //long y = 15;

            //fails for these values
            //long x = 500;
            //long y = 15;

            long x = 45;
            long y = 5;

            long loss = 0;
            long timeLimit = 1000000;

            ExtensionMethods.PrintGrid(x, y, loss, timeLimit);

            ExtensionMethods.CalcElderAge(x, y, loss, timeLimit);



            // another attempt
            BigInteger eldersAge = ExtensionMethods.Wip2ElderAge(x, y, loss, timeLimit);

            Console.WriteLine($"eqn based elders age = {eldersAge}");

            Console.ReadLine();
        }
    }

    public static class ExtensionMethods
    {
        internal static BigInteger Wip2ElderAge(long x, long y, long loss, long timeLimit)
        {
            // TODO - make sure the x value is bigger...?

            var firstXValue = 1;
            do
            {
                firstXValue *= 2;
            } while (firstXValue <= x);
            firstXValue /= 2;

            var firstYValue = 1;
            do
            {
                firstYValue *= 2;
            } while (firstYValue <= y);
            firstYValue /= 2;


            // calculate values for the square y * y (because y is the smaller square)
            BigInteger sumFirstSquare = AriSum(firstYValue, firstYValue, loss);


            // calculate remaining rows firstYValue -> y

            // aSum each row should be aSum from firstYValue to (2 * firstYValue) - 1
            // multiply that aSum by y - firstYValue
            BigInteger numTerms = firstYValue;
            BigInteger a = (2 * firstYValue) - 1;
            BigInteger aSum = (numTerms * (firstYValue + a)) / 2;
            BigInteger aSumRemainingRows = aSum * (y - firstYValue);

            // ----- dealing with loss
            if (firstYValue > loss)
            {
                // then above eqns are good to go
            }
            else if ((firstYValue < loss) && (a > loss))
            {
                numTerms = a - loss;
                aSum = (numTerms * (loss + 1 + a)) / 2;
                aSumRemainingRows = aSum * (y - firstYValue);
            }
            else
            {
                aSumRemainingRows = 0;
            }


            // Use to DEBUG if remaining row eqns are correct -------------- 
            //BigInteger remainingRowXors = 0;
            //for (var i = 0; i < firstYValue; i++)
            //{
            //    for (var j = firstYValue; j < y; j++)
            //    {
            //        var xor = i ^ j;
            //        if (xor > loss)
            //        {
            //            remainingRowXors += xor - loss;
            //        }
            //    }
            //}
            //aSumRemainingRows = remainingRowXors;
            // -----------------------------------------------------------------------




            // calculate remaining columns from firstYValue -> x

            // aSum each column should be aSum from firstYValue -> (2 * firstYValue) - 1
            // multiply that aSum by x - firstYValue
            numTerms = x - firstYValue;
            aSum = (numTerms * (firstYValue + x - 1)) / 2;

            // ----- dealing with loss
            if (firstYValue > loss)
            {
                // then above eqns are good to go
            }
            else if ((firstYValue < loss) && ((x - 1) > loss))
            {
                numTerms = (x - 1) - loss;
                aSum = (numTerms * (loss + 1 + (x - 1))) / 2;
            }
            else
            {
                aSum = 0;
            }


            BigInteger aSumRemainingColumns = 0;
            if (firstXValue == x) // x is multiple 2^n
            {
                aSumRemainingColumns = aSum * firstYValue;
            }
            else if (x % 2 != 0) // x is odd
            {
                aSumRemainingColumns = aSum * firstYValue;


                // +1 each row incrementing, up to row number #, where # = largest multiple of 2^n that divides x+1 or x-1 to give a whole number
                // after row number #, the increment jumps to ((# - 1) ^ 2) + # and then increments +1 each row up to row number 2 * #
                // after row number 2 * #, the increment jumps to 2 * ((# - 1) ^ 2) + increment value at row 2 * #
                // and so on, each # rows, incrementing by (row / #) * (# - 1)^2 + last increment value

                BigInteger rowOnWhichIncrementSquares = 0;
                BigInteger iterator = 1;
                while (iterator <= x + 1)
                {
                    iterator *= 2;
                    if ((x - 1) % iterator == 0 && iterator > rowOnWhichIncrementSquares)
                    {
                        rowOnWhichIncrementSquares = iterator;
                    }
                    if ((x + 1) % iterator == 0 && iterator > rowOnWhichIncrementSquares)
                    {
                        rowOnWhichIncrementSquares = iterator;
                    }
                }


                numTerms = firstYValue - 1;
                if (numTerms < rowOnWhichIncrementSquares)
                {
                    // ----- dealing with loss
                    if (1 > loss)
                    {
                        aSumRemainingColumns += (numTerms * (1 + firstYValue - 1)) / 2;
                    }
                    else if ((1 < loss) && ((firstYValue - 1) > loss))
                    {
                        aSumRemainingColumns += ((numTerms - loss) * (loss + firstYValue - 1)) / 2;
                    }
                    else
                    {
                        // dont add anything
                    }
                }
                else
                {
                    //aSumRemainingColumns += (numTerms * (1 + firstYValue - 1)) / 2; // +1 from first term to last term
                    //aSumRemainingColumns -= numTerms / rowOnWhichIncrementSquares; // dont get +1 at the row change where the square is added, so remove 1 for each change
                    //aSumRemainingColumns += (rowOnWhichIncrementSquares - 1) * (rowOnWhichIncrementSquares - 1) * (numTerms / rowOnWhichIncrementSquares);

                    // aSum from row 0 -> rowOnWhichIncrementSquares
                    BigInteger nestedNumTerms = rowOnWhichIncrementSquares - 1;

                    // ----- dealing with loss
                    BigInteger firstSeries = 0;
                    if (1 > loss)
                    {
                        firstSeries = (nestedNumTerms * (1 + nestedNumTerms)) / 2;
                        aSumRemainingColumns += firstSeries;
                    }
                    else if ((1 < loss) && (nestedNumTerms > loss))
                    {
                        firstSeries = ((nestedNumTerms - loss) * (loss + nestedNumTerms)) / 2;
                        aSumRemainingColumns += firstSeries;
                    }
                    else
                    {
                        // dont add anything
                    }


                    BigInteger firstIncVal = (rowOnWhichIncrementSquares - 1); // 3
                    BigInteger firstIncrement = (rowOnWhichIncrementSquares - 1) * (rowOnWhichIncrementSquares - 1); // 9
                    // aSumRemainingColumns += firstIncrement;

                    BigInteger subsequentIncrements = firstIncVal + firstIncrement; // 12
                    BigInteger finalIncValue = 0;
                    int i = 1;
                    while (i < ((numTerms + 1) / rowOnWhichIncrementSquares)) // the division that gives how many times we jump by increment as a whole number
                    {
                        finalIncValue = i * subsequentIncrements;

                        // ----- dealing with loss
                        if (finalIncValue > loss)
                        {
                            aSumRemainingColumns += finalIncValue * rowOnWhichIncrementSquares;
                            aSumRemainingColumns += firstSeries; // TODO - not sure if this should be inside this if statement
                        }

                        i++;
                    }

                    // this condition should technically never be entered...?
                    if ((numTerms + 1) % rowOnWhichIncrementSquares != 0)
                    {
                        // ----- dealing with loss
                        if ((finalIncValue + subsequentIncrements) > loss)
                        {
                            aSumRemainingColumns += (finalIncValue + subsequentIncrements) * (numTerms % rowOnWhichIncrementSquares);
                        }
                        if (((numTerms % rowOnWhichIncrementSquares) - 1) > loss)
                        {
                            aSumRemainingColumns += (numTerms % rowOnWhichIncrementSquares) - 1;
                        }

                        // ----- dealing with loss
                        if (1 > loss)
                        {
                            nestedNumTerms = (numTerms % rowOnWhichIncrementSquares) - 1;
                            BigInteger lastSeries = (nestedNumTerms * (1 + nestedNumTerms)) / 2;
                            aSumRemainingColumns += lastSeries;
                        }
                        else if ((1 < loss) && (nestedNumTerms > loss))
                        {
                            nestedNumTerms = (numTerms % rowOnWhichIncrementSquares) - 1;
                            BigInteger lastSeries = ((nestedNumTerms - loss) * (loss + nestedNumTerms)) / 2;
                            aSumRemainingColumns += lastSeries;
                        }
                        else
                        {
                            // dont add anything 
                        }

                    }

                }


            }
            else // x is even
            {
                aSumRemainingColumns = aSum * firstYValue;

                // check if rows is > rows required for a pattern change --------

                // get largest multiple 2^n that divides x to give a whole number
                BigInteger rowOfPatternChange = 0;
                BigInteger multiple2PowN = 1;
                while (multiple2PowN < x)
                {
                    if (x % multiple2PowN == 0)
                    {
                        rowOfPatternChange = multiple2PowN;
                    }
                    multiple2PowN *= 2;
                }


                if (firstYValue > rowOfPatternChange)
                {
                    // +4, +4 each 2 rows (not including first 2 rows) --- need to update to match pattern written out in notebook
                    numTerms = firstYValue - rowOfPatternChange;

                    BigInteger firstTerm = rowOfPatternChange * rowOfPatternChange;
                    BigInteger subsequentIncrements = firstTerm;
                    int i = 1;

                    BigInteger finalIncValue = 1;
                    while (i < (firstYValue / rowOfPatternChange)) // the division that gives how many times we jump by increment as a whole number
                    {
                        finalIncValue = i * subsequentIncrements;

                        // ----- dealing with loss
                        if (finalIncValue > loss)
                        {
                            aSumRemainingColumns += finalIncValue * rowOfPatternChange;
                        }

                        i++;
                    }

                    //BigInteger lastTerm = 4 * (numTerms / 2);

                    //aSum = ((numTerms / 2) * (firstTerm + lastTerm)) / 2;

                    //aSumRemainingColumns += 2 * aSum;
                    if ((firstYValue % rowOfPatternChange) != 0)
                    {
                        // ----- dealing with loss
                        if (finalIncValue > loss)
                        {
                            aSumRemainingColumns += (firstYValue % rowOfPatternChange) * finalIncValue;
                        }
                    }
                }


            }

            // Use to DEBUG if remaining columns eqns are correct -------------- 
            //BigInteger remainingColumnXors = 0;
            //for (var i = 0; i < firstYValue; i++)
            //{
            //    for (var j = firstYValue; j < x; j++)
            //    {
            //        var xor = i ^ j;
            //        if (xor > loss)
            //        {
            //            remainingColumnXors += xor - loss;
            //        }
            //    }
            //}
            //aSumRemainingColumns = remainingColumnXors;
            // -----------------------------------------------------------------------




            // calculate remaining rectangle sums
            BigInteger remainingXors = 0;
            for (var i = firstYValue; i < x; i++)
            {
                for (var j = firstYValue; j < y; j++)
                {
                    var xor = i ^ j;
                    if (xor > loss)
                    {
                        remainingXors += xor - loss;
                    }
                }
            }


            BigInteger total = sumFirstSquare + aSumRemainingRows + aSumRemainingColumns + remainingXors;

            var timeWrapped = total % timeLimit;

            return total;
        }


        // y is multiplier
        internal static BigInteger AriSum(BigInteger x, BigInteger y, BigInteger loss)
        {
            BigInteger numTerms = 0;
            if (loss > 0)
            {
                numTerms = x - (loss + 1);
            }
            else
            {
                numTerms = x - 1;
            }


            BigInteger aSum = (numTerms * (1 + x - (loss + 1))) / 2;
            return aSum * y;
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


