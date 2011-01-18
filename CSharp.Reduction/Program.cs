// Copyright 2010-2011 -- TidePowerd, Ltd. All rights reserved.
// http://www.tidepowerd.com
//
// GPU.NET Reduction Example (CSharp.Reduction)
// Modified: 17-Jan-2011
//
// More examples available at: http://github.com/tidepowerd/GPU.NET-Example-Projects
//

using System;
using System.Diagnostics;

using TidePowerd.DeviceMethods;

namespace TidePowerd.Example.CSharp.Reduction.Cli
{
    class Program
    {
        /// <summary>
        /// The number of elements to use for the reduction.
        /// </summary>
        /// <remarks>
        /// This number must currently be a power-of-two size.
        /// </remarks>
        private const int NumElements = 1 << 23;// 1 << 24;        // 1 << 22 = 4194304 // 1 << 24 = 16777216   // Must be power-of-two sizes

        static void Main(string[] args)
        {
            // Create input and output data arrays
            int[] InputData = new int[NumElements];

            // Print 'header'
            Console.Out.WriteLine("Performing shared-memory reduction tests...");
            Console.Out.WriteLine("----------------------------------------------------------------------");
            Console.Out.Write("Generating random test data ({0} {1} elements)...", NumElements, InputData.GetType().GetElementType().Name);

            // Fill the input data with random values; these values must fall between zero (0) and the maximum number, which if multiplied by the array length, would still fit in an int32 (signed int) value
            // This is to make our results easier to validate, since we don't have to deal with possible overflow behavior
            Random rand = new Random();
            const int MaxValue = Int32.MaxValue / NumElements;
            for (int i = 0; i < InputData.Length; i++)
            {
                InputData[i] = rand.Next(MaxValue);
            }
            Console.Out.WriteLine("done.");
            Console.Out.WriteLine();

            // Create the stopwatch we'll use to time how long each reduction takes
            Stopwatch Watch = new Stopwatch();

            // Compute the reduction value on the CPU first so that we can compare it to the GPU-based results
            // TODO: Perform the reduction 2 or 3 times here to get an accurate timing result
            // TODO: Create a version of this project which uses PLINQ / TPL for comparison
            Console.Out.WriteLine("Computing CPU-based result for comparison...");
            Watch.Start();
            int CpuReductionValue = 0;
            for (int i = 0; i < InputData.Length; i++) { CpuReductionValue += InputData[i]; }
            Watch.Stop();
            Console.Out.WriteLine("done. (Value = {0}, Time = {1:F02} ms)", CpuReductionValue, Watch.Elapsed.TotalMilliseconds);
            Console.Out.WriteLine();
            Console.Out.WriteLine("----------------------------------------------------------------------");
            Watch.Reset();

            #region reduce0 (Interleaved access with modulo operator)

            // Start the reduction (and the timer)
            Console.Out.WriteLine("Testing reduce0 (Interleaved access with modulo operator)...");
            Watch.Start();

            // Call the reduction method, which will iterate the reduction kernel until the entire array is reduced
            int InterleavedModuloResult = Reduction.InterleavedModulo(InputData);

            Watch.Stop();
            Console.Out.WriteLine("done. (Value = {0}, Time = {1:F02} ms)", InterleavedModuloResult, Watch.Elapsed.TotalMilliseconds);
            Console.Out.Write("Test ");
            if (InterleavedModuloResult == CpuReductionValue) { ConsoleWriteLineColored("passed!", ConsoleColor.Green); }
            else { ConsoleWriteLineColored("failed!", ConsoleColor.Red); }
            Console.Out.WriteLine();
            Watch.Reset();

            #endregion

            #region reduce1 (Interleaved contiguous access)

            // Start the reduction (and the timer)
            Console.Out.WriteLine("Testing reduce1 (Interleaved contiguous access)...");
            Watch.Start();

            // Call the reduction method, which will iterate the reduction kernel until the entire array is reduced
            int InterleavedContiguousResult = Reduction.InterleavedContiguous(InputData);

            Watch.Stop();
            Console.Out.WriteLine("done. (Value = {0}, Time = {1:F02} ms)", InterleavedContiguousResult, Watch.Elapsed.TotalMilliseconds);
            Console.Out.Write("Test ");
            if (InterleavedContiguousResult == CpuReductionValue) { ConsoleWriteLineColored("passed!", ConsoleColor.Green); }
            else { ConsoleWriteLineColored("failed!", ConsoleColor.Red); }
            Console.Out.WriteLine();
            Watch.Reset();

            #endregion

            #region reduce2 (Sequential addressing)

            // Start the reduction (and the timer)
            Console.Out.WriteLine("Testing reduce2 (Sequential addressing)...");
            Watch.Start();

            // Call the reduction method, which will iterate the reduction kernel until the entire array is reduced
            int SequentialAddressingResult = Reduction.SequentialAddressing(InputData);

            Watch.Stop();
            Console.Out.WriteLine("done. (Value = {0}, Time = {1:F02} ms)", SequentialAddressingResult, Watch.Elapsed.TotalMilliseconds);
            Console.Out.Write("Test ");
            if (SequentialAddressingResult == CpuReductionValue) { ConsoleWriteLineColored("passed!", ConsoleColor.Green); }
            else { ConsoleWriteLineColored("failed!", ConsoleColor.Red); }
            Console.Out.WriteLine();
            Watch.Reset();

            #endregion

            #region reduce3 (Sequential addressing with first reduction from global)

            // Start the reduction (and the timer)
            Console.Out.WriteLine("Testing reduce3 (Sequential addressing with reduction from global)...");
            Watch.Start();

            // Call the reduction method, which will iterate the reduction kernel until the entire array is reduced
            int FirstReductionFromGlobalResult = Reduction.FirstReductionFromGlobal(InputData);

            Watch.Stop();
            Console.Out.WriteLine("done. (Value = {0}, Time = {1:F02} ms)", FirstReductionFromGlobalResult, Watch.Elapsed.TotalMilliseconds);
            Console.Out.Write("Test ");
            if (FirstReductionFromGlobalResult == CpuReductionValue) { ConsoleWriteLineColored("passed!", ConsoleColor.Green); }
            else { ConsoleWriteLineColored("failed!", ConsoleColor.Red); }
            Console.Out.WriteLine();
            Watch.Reset();

            #endregion

            // Print the exit message
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
            Environment.Exit(0);
        }

        /// <summary>
        /// Writes the specified string value (in color) to the standard output stream.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="color"></param>
        private static void ConsoleWriteColored(string str, ConsoleColor color)
        {
            // Preconditions
            if (str == null) { throw new ArgumentNullException("str"); }

            ConsoleColor Old = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(str);
            Console.ForegroundColor = Old;
        }

        /// <summary>
        /// Writes the specified string value (in color), followed by the current line terminator, to the standard output stream.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="color"></param>
        private static void ConsoleWriteLineColored(string str, ConsoleColor color)
        {
            // Preconditions
            if (str == null) { throw new ArgumentNullException("str"); }

            ConsoleColor Old = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(str);
            Console.ForegroundColor = Old;
        }
    }
}
