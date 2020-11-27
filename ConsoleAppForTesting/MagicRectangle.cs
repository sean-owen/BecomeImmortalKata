using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ConsoleAppForTesting
{
    public static class MagicRectangle
    {
        public static long CalculateElderAge(long x, long y, long loss, long timeLimit)
        {
            SortInputs(ref x, ref y);
            long squareSideLength = FindLargest2PowN(y);

            long sumSquare = SumSquare(squareSideLength, loss);


            long sumRemainingRows = SumRemainingRows(squareSideLength, y, loss);
            long debugRemRows = NaiveRemainingRows(y, squareSideLength, loss);


            long sumRemainingColumns = SumRemainingColumns(x, squareSideLength, loss);
            long debugRemColumns = NaiveRemainingColumns(x, squareSideLength, loss);


            long sumRemainingCoords = SumRemainingCoords(x, y, squareSideLength, loss);


            long total = sumSquare + sumRemainingRows + sumRemainingColumns + sumRemainingCoords;
            long timeWrapped = total % timeLimit;

            return total;
        }

        public static long NaiveRemainingRows(long y, long squareSideLength, long loss)
        {
            long remainingRowXors = 0;
            for (var i = 0; i < squareSideLength; i++)
            {
                for (var j = squareSideLength; j < y; j++)
                {
                    var xor = i ^ j;
                    if (xor > loss)
                    {
                        remainingRowXors += xor - loss;
                    }
                }
            }
            return remainingRowXors;
        }

        public static long NaiveRemainingColumns(long x, long squareSideLength, long loss)
        {
            long remainingColumnXors = 0;
            for (var i = 0; i < squareSideLength; i++)
            {
                for (var j = squareSideLength; j < x; j++)
                {
                    var xor = i ^ j;
                    if (xor > loss)
                    {
                        remainingColumnXors += xor - loss;
                    }
                }
            }
            return remainingColumnXors;
        }

        /// <summary>
        /// Compares the x and y inputs and re-assigns them, if neccesary, so that x is larger than y.
        /// </summary>
        /// <param name="x">Will be assigned to store the larger value of the inputs.</param>
        /// <param name="y">Will be assigned to store the smaller value of the inputs.</param>
        public static void SortInputs(ref long x, ref long y)
        {
            long largerAxis = x > y ? x : y;
            long smallerAxis = x > y ? y : x;

            x = largerAxis;
            y = smallerAxis;
        }

        /// <summary>
        /// Finds the largest multiple of 2^n that is less than the input.
        /// </summary>
        /// <param name="input">Value to compare multiples of 2^n against.</param>
        /// <returns>Largest multiple of 2^n that is less than the input.</returns>
        public static long FindLargest2PowN(long input)
        {
            long multiple2PowN = 1;
            do
            {
                multiple2PowN *= 2;
            } while (multiple2PowN <= input);

            multiple2PowN /= 2;

            return multiple2PowN;
        }

        /// <summary>
        /// Calculates the xor sum of a square of dimensions x, x.
        /// Uses loss as a threshold which a xor sum must exceed to be added to the total xor sum of the square.
        /// </summary>
        /// <param name="x">The size of a side of the square.</param>
        /// <param name="loss">The loss threshold that a xor sum must exceed to contribute to the total sum.</param>
        /// <returns>Calculated total xor sum.</returns>
        public static long SumSquare(long x, long loss)
        {
            long numTerms;
            if (loss > 0)
            {
                numTerms = x - (loss + 1);
            }
            else
            {
                numTerms = x - 1;
            }

            if (x > loss)
            {
                long aSum = (numTerms * (1 + x - (loss + 1))) / 2;
                return aSum * x;
            }
            else
            {
                return 0;
            }
        }        

        /// <summary>
        /// Calculates the xor sum of the rows below a square of side length x.        
        /// </summary>
        /// <param name="squareSide">Length of any side of the square.</param>
        /// /// <param name="y">The length of side y of a rectangle.</param>
        /// <param name="loss">The loss threshold that a xor sum must exceed to contribute to the total sum.</param>
        /// <returns>Calculated total xor sum.</returns>
        public static long SumRemainingRows(long squareSide, long y, long loss)
        {
            // aSum each row should be aSum from firstYValue to (2 * firstYValue) - 1
            // multiply that aSum by y - firstYValue
            BigInteger numTerms = squareSide;
            BigInteger a = (2 * squareSide) - 1;
            BigInteger aSum = (numTerms * (squareSide - loss + a - loss)) / 2;
            BigInteger aSumRemainingRows = aSum * (y - squareSide);

            // ----- dealing with loss
            if (squareSide > loss)
            {
                // then above eqns are good to go
                return (long)aSumRemainingRows;
            }
            else if ((squareSide < loss) && (a > loss))
            {
                numTerms = a - loss;
                aSum = (numTerms * (loss + 1 + a)) / 2; // TODO should be a - loss? but then what if a - loss becomes lower than loss? 
                aSumRemainingRows = aSum * (y - squareSide);
                return (long)aSumRemainingRows;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Calculates the xor sum of the columns to the right of a square of side length x.
        /// </summary>
        /// <param name="x">The length of side x of a rectangle.</param>
        /// <param name="squareSide">Length of any side of the square.</param>
        /// <param name="loss">The loss threshold that a xor sum must exceed to contribute to the total sum.</param>
        /// <returns>Calculated total xor sum.</returns>
        public static long SumRemainingColumns(long x, long squareSide, long loss)
        {
            // calculate remaining columns from firstYValue -> x

            BigInteger firstXValue = FindLargest2PowN(x);

            BigInteger numTerms;

            // aSum each column should be aSum from firstYValue -> (2 * firstYValue) - 1
            // multiply that aSum by x - firstYValue
            numTerms = x - squareSide;
            BigInteger aSum = (numTerms * (squareSide - loss + x - 1 - loss)) / 2;

            // ----- dealing with loss
            if (squareSide > loss)
            {
                // then above eqns are good to go
            }
            else if ((squareSide < loss) && ((x - 1) > loss))
            {
                numTerms = (x - 1) - loss;
                //aSum = (numTerms * (loss + 1 + (x - 1))) / 2; // TODO should be (x - 1) - loss? but then what if (x - 1) - loss becomes lower than loss?
                aSum = (numTerms * (1 + (x - 1) - loss)) / 2; // believe this is the way to deal with this scenario
            }
            else
            {
                aSum = 0;
            }


            BigInteger aSumRemainingColumns = 0;
            if (firstXValue == x) // x is multiple 2^n
            {
                aSumRemainingColumns = aSum * squareSide;
            }
            else if (x % 2 != 0) // x is odd
            {
                aSumRemainingColumns = aSum * squareSide;


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


                numTerms = squareSide - 1;
                if (numTerms < rowOnWhichIncrementSquares)
                {
                    // ----- dealing with loss
                    if (1 > loss)
                    {
                        aSumRemainingColumns += (numTerms * (1 + squareSide - 1)) / 2;
                    }
                    //else if ((1 < loss) && ((firstYValue - 1) > loss))
                    //{
                    //    //aSumRemainingColumns += ((numTerms - loss) * (loss + firstYValue - 1)) / 2; // TODO should be (firstYValue - 1) - loss? but then what if (firstYValue - 1) - loss becomes lower than loss? 
                    //    aSumRemainingColumns += ((numTerms - loss) * (1 + firstYValue - 1 - loss)) / 2; // believe this is the way to deal with this scenario
                    //}
                    else
                    {
                        aSumRemainingColumns += ((numTerms) * (1 + squareSide - 1)) / 2; // believe this is the way to deal with this scenario
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
                    //else if ((1 < loss) && (nestedNumTerms > loss))
                    //{
                    //    //firstSeries = ((nestedNumTerms - loss) * (loss + nestedNumTerms)) / 2; // TODO should be (nestedNumTerms) - loss? but then what if (nestedNumTerms) - loss becomes lower than loss? 
                    //    firstSeries = ((nestedNumTerms - loss) * (1 + nestedNumTerms - loss)) / 2; // believe this is the way to deal with this scenario (maybe missing nestedNumTerms - loss)                        
                    //    aSumRemainingColumns += firstSeries;
                    //}
                    else
                    {
                        firstSeries = ((nestedNumTerms) * (1 + nestedNumTerms)) / 2; // believe this is the way to deal with this scenario (maybe missing nestedNumTerms - loss)                        
                        aSumRemainingColumns += firstSeries;
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

                        // Todo - PRIORITY
                        // final inc value goes to 48 but should actually go to 208.... ? 
                        // this does not work, even with 0 loss!!!!

                        // ----- dealing with loss
                        if (finalIncValue > loss)
                        {
                            aSumRemainingColumns += (finalIncValue) * rowOnWhichIncrementSquares;
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
                            aSumRemainingColumns += ((finalIncValue + subsequentIncrements) - loss) * (numTerms % rowOnWhichIncrementSquares);
                        }
                        if (((numTerms % rowOnWhichIncrementSquares) - 1) > loss)
                        {
                            aSumRemainingColumns += (numTerms % rowOnWhichIncrementSquares) - 1 - loss;
                        }

                        // ----- dealing with loss
                        if (1 > loss)
                        {
                            nestedNumTerms = (numTerms % rowOnWhichIncrementSquares) - 1;
                            BigInteger lastSeries = (nestedNumTerms * (1 + nestedNumTerms)) / 2;
                            aSumRemainingColumns += lastSeries;
                        }
                        //else if ((1 < loss) && (nestedNumTerms > loss))
                        //{
                        //    nestedNumTerms = (numTerms % rowOnWhichIncrementSquares) - 1;
                        //    //BigInteger lastSeries = ((nestedNumTerms - loss) * (loss + nestedNumTerms)) / 2; // TODO should be (nestedNumTerms) - loss? but then what if (nestedNumTerms) - loss becomes lower than loss? 
                        //    BigInteger lastSeries = ((nestedNumTerms - loss) * (1 + nestedNumTerms - loss)) / 2; // believe this is the way to deal with this scenario (maybe missing a -1 as indicated in line above)                            
                        //    aSumRemainingColumns += lastSeries;
                        //}
                        else
                        {
                            // dont add anything 
                            nestedNumTerms = (numTerms % rowOnWhichIncrementSquares) - 1;
                            BigInteger lastSeries = ((nestedNumTerms - loss) * (1 + nestedNumTerms - loss)) / 2; // believe this is the way to deal with this scenario (maybe missing a -1 as indicated in line above)                            
                            aSumRemainingColumns += lastSeries;
                        }

                    }

                }


            }
            else // x is even
            {
                aSumRemainingColumns = aSum * squareSide;

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


                if (squareSide > rowOfPatternChange)
                {
                    // +4, +4 each 2 rows (not including first 2 rows) --- need to update to match pattern written out in notebook
                    numTerms = squareSide - rowOfPatternChange;

                    BigInteger firstTerm = rowOfPatternChange * rowOfPatternChange;
                    BigInteger subsequentIncrements = firstTerm;
                    int i = 1;

                    BigInteger finalIncValue = 1;
                    while (i < (squareSide / rowOfPatternChange)) // the division that gives how many times we jump by increment as a whole number
                    {
                        finalIncValue = i * subsequentIncrements;

                        // ----- dealing with loss
                        if (finalIncValue > loss)
                        {
                            aSumRemainingColumns += (finalIncValue - loss) * rowOfPatternChange;
                        }

                        i++;
                    }

                    //BigInteger lastTerm = 4 * (numTerms / 2);

                    //aSum = ((numTerms / 2) * (firstTerm + lastTerm)) / 2;

                    //aSumRemainingColumns += 2 * aSum;
                    if ((squareSide % rowOfPatternChange) != 0)
                    {
                        // ----- dealing with loss
                        if (finalIncValue > loss)
                        {
                            aSumRemainingColumns += (squareSide % rowOfPatternChange) * (finalIncValue - loss);
                        }
                    }
                }

            }

            return (long)aSumRemainingColumns;
        }

        /// <summary>
        /// Sums the remaining coordinates in the rectangle, that are not parallel to any side of the internal square.
        /// </summary>
        /// <param name="x">Rectangle side x.</param>
        /// <param name="y">Rectangle side y.</param>
        /// <param name="squareSide">The size of any side of the internal square.</param>
        /// <param name="loss">The loss threshold that a xor sum must exceed to contribute to the total sum.</param>
        /// <returns>Calculated total xor sum.</returns>
        public static long SumRemainingCoords(long x, long y, long squareSide, long loss)
        {
            BigInteger remainingXors = 0;
            for (var i = squareSide; i < x; i++)
            {
                for (var j = squareSide; j < y; j++)
                {
                    var xor = i ^ j;
                    if (xor > loss)
                    {
                        remainingXors += xor - loss;
                    }
                }
            }

            return (long)remainingXors;
        }
    }

}
