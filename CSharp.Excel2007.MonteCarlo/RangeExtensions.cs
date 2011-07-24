/*  The MIT License

Copyright (c) 2011 TidePowerd Limited (http://www.tidepowerd.com)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.

*/

// GPU.NET Example Project : Monte Carlo (C# Excel2007)
// More examples available at http://github.com/tidepowerd

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.Tools.Applications.Runtime;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;

namespace TidePowerd.Example.CSharp.Excel2007.MonteCarlo
{
    /// <summary>
    /// Provides extension methods for Excel's <seealso cref="Excel.Range"/> interface type.
    /// </summary>
    internal static class RangeExtensions
    {
        //
        public static T[] GetColumn<T>(this Excel.Range range, Converter<object, T> converter)
        {
            // Preconditions
            if (range == null) { throw new ArgumentNullException("range"); }
            else if (converter == null) { throw new ArgumentNullException("converter"); }

            // Postconditions
            // TODO

            // Get the values in the range of cells as a 2-dimensional (rectangular) array of objects
            // The array uses 1-based indices!
            object[,] RangeValues = range.Value2 as object[,];

            // DEBUG : Make sure that RangeValues represents a column.
            Debug.Assert(RangeValues.GetLength(1) == 1, "The range specified by 'cell1' and 'cell2' does not represent a column.");

            // Create a one-dimensional, zero-based array to hold the column values.
            object[] ColumnValues = new object[RangeValues.GetLength(0)];

            // Copy the column values from the 2d RangeValues array into the ColumnValues array.
            for (int i = 0; i < ColumnValues.Length; i++)
            {
                ColumnValues[i] = RangeValues[i + 1, 1];
            }

            // Convert the column values using the specified converter delegate, then return the array of converted values.
            return Array.ConvertAll(ColumnValues, converter);
        }

        //
        public static object[] GetColumn(this Excel.Range range, object cell1, object cell2)
        {
            // Preconditions
            if (range == null) { throw new ArgumentNullException("range"); }
            else if (cell1 == null) { throw new ArgumentNullException("cell1"); }
            else if (cell2 == null) { throw new ArgumentNullException("cell2"); }

            // Postconditions
            // TODO

            // Get the values in the range of cells as a 2-dimensional (rectangular) array of objects
            // The array uses 1-based indices!
            object[,] RangeValues = range.get_Range(cell1, cell2).Value2 as object[,];

            // DEBUG : Make sure that RangeValues represents a column.
            Debug.Assert(RangeValues.GetLength(1) == 1, "The range specified by 'cell1' and 'cell2' does not represent a column.");

            // Create a one-dimensional, zero-based array to hold the column values.
            object[] ColumnValues = new object[RangeValues.GetLength(0)];

            // Copy the column values from the 2d RangeValues array into the ColumnValues array.
            for (int i = 0; i < ColumnValues.Length; i++)
            {
                ColumnValues[i] = RangeValues[i + 1, 1];
            }

            // Return the array containing the column values.
            return ColumnValues;
        }

        //
        public static T[] GetColumn<T>(this Excel.Range range, object cell1, object cell2, Converter<object, T> converter)
        {
            // Preconditions
            if (range == null) { throw new ArgumentNullException("range"); }
            else if (cell1 == null) { throw new ArgumentNullException("cell1"); }
            else if (cell2 == null) { throw new ArgumentNullException("cell2"); }
            else if (converter == null) { throw new ArgumentNullException("converter"); }

            // Postconditions
            // TODO

            // Call the "standard" GetColumn() method, then use the converter delegate to convert the values to the output type.
            return Array.ConvertAll(range.GetColumn(cell1, cell2), converter);
        }
    }
}
