// Copyright 2010 -- TidePowerd, Ltd. All rights reserved.
// http://www.tidepowerd.com
//
// GPU.NET Reduction Example (CSharp.Reduction)
// Modified: 01-Dec-2010
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
        // NOTE: This value cannot be greater than 1 << 23 unless you write the example to use a 2-dimensional grid
        internal const int NumElements = 1 << 23;   // Must be power-of-two sizes

        /// <summary>
        /// The number of threads per thread-block.
        /// </summary>
        internal const int MaxThreadsPerBlock = 256;

        /// <summary>
        /// The maximum number of block which we'll allow to be used.
        /// </summary>
        internal const int MaxBlocks = 64;

        /// <summary>
        /// The threshold number of elements at which (or below) we finish the reduction on the CPU.
        /// </summary>
        private const int CpuThreshold = 1;

        static void Main(string[] args)
        {
            // Create input and output data arrays
            int[] InputData = new int[NumElements];
            int[] ClonedInputData = null;   // We'll re-initalize this for each reduction and copy the input data into it (since it's overwritten during each reduction)
            int[] OutputData = null;    // This will be initialized (to the correct size) by each reduction

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

            // Declare variables to hold the current number of threads per block and number of blocks; re-used throughout the code below
            int NumThreadsPerBlock = 0, NumBlocks = 0;

            // NOTE: reduce0 currently fails due to a compiler bug
            #region reduce0 (Interleaved access with modulo operator)

            // Clone the input data since we overwrite whatever data is used as input for the reduction
            ClonedInputData = new int[NumElements];
            Array.Copy(InputData, ClonedInputData, NumElements);

            // Calculate the block and grid sizes needed for the first "level" of the reduction
            CalculateBlockAndGridSizes(0, NumElements, out NumThreadsPerBlock, out NumBlocks);
            Launcher.SetBlockSize(NumThreadsPerBlock);
            Launcher.SetGridSize(NumBlocks);

            // Start the reduction (and the timer)
            Console.Out.WriteLine("Testing reduce0 (Interleaved access with modulo operator)...");
            Watch.Start();

            // Perform the first "level" of the reduction
            OutputData = new int[NumBlocks];
            Reduction.InterleavedModulo(ClonedInputData, OutputData);

            // If necessary, perform additional "levels" of reduction until we have only a single element (the result)
            while (NumBlocks > CpuThreshold)
            {
                // Calculate the number of threads and blocks based on the current input size
                CalculateBlockAndGridSizes(0, NumBlocks, out NumThreadsPerBlock, out NumBlocks);
                Launcher.SetBlockSize(NumThreadsPerBlock);
                Launcher.SetGridSize(NumBlocks);

                // Replace the current "level's" input data with the output data from the previous "level"
                ClonedInputData = OutputData;

                // Create a new array to hold the output data for this "level"
                OutputData = new int[NumBlocks];

                // Call the reduction method to perform the current "level" of reduction
                Reduction.InterleavedModulo(ClonedInputData, OutputData);
            }
            Watch.Stop();
            Console.Out.WriteLine("done. (Value = {0}, Time = {1:F02} ms)", OutputData[0], Watch.Elapsed.TotalMilliseconds);
            Console.Out.Write("Test ");
            if (OutputData[0] == CpuReductionValue) { ConsoleWriteLineColored("passed!", ConsoleColor.Green); }
            else { ConsoleWriteLineColored("failed!", ConsoleColor.Red); }
            Console.Out.WriteLine();
            Watch.Reset();

            #endregion

            #region reduce1 (Interleaved contiguous access)

            // Clone the input data since we overwrite whatever data is used as input for the reduction
            ClonedInputData = new int[NumElements];
            Array.Copy(InputData, ClonedInputData, NumElements);

            // Calculate the block and grid sizes needed for the first "level" of the reduction
            CalculateBlockAndGridSizes(1, NumElements, out NumThreadsPerBlock, out NumBlocks);
            Launcher.SetBlockSize(NumThreadsPerBlock);
            Launcher.SetGridSize(NumBlocks);

            // Start the reduction (and the timer)
            Console.Out.WriteLine("Testing reduce1 (Interleaved contiguous access)...");
            Watch.Start();

            // Perform the first "level" of the reduction
            OutputData = new int[NumBlocks];
            Reduction.InterleavedContiguous(ClonedInputData, OutputData);

            // If necessary, perform additional "levels" of reduction until we have only a single element (the result)
            while (NumBlocks > CpuThreshold)
            {
                // Calculate the number of threads and blocks based on the current input size
                CalculateBlockAndGridSizes(1, NumBlocks, out NumThreadsPerBlock, out NumBlocks);
                Launcher.SetBlockSize(NumThreadsPerBlock);
                Launcher.SetGridSize(NumBlocks);

                // Replace the current "level's" input data with the output data from the previous "level"
                ClonedInputData = OutputData;

                // Create a new array to hold the output data for this "level"
                OutputData = new int[NumBlocks];

                // Call the reduction method to perform the current "level" of reduction
                Reduction.InterleavedContiguous(ClonedInputData, OutputData);
            }
            Watch.Stop();
            Console.Out.WriteLine("done. (Value = {0}, Time = {1:F02} ms)", OutputData[0], Watch.Elapsed.TotalMilliseconds);
            Console.Out.Write("Test ");
            if (OutputData[0] == CpuReductionValue) { ConsoleWriteLineColored("passed!", ConsoleColor.Green); }
            else { ConsoleWriteLineColored("failed!", ConsoleColor.Red); }
            Console.Out.WriteLine();
            Watch.Reset();

            #endregion

            #region reduce2 (Sequential addressing)

            // Clone the input data since we overwrite whatever data is used as input for the reduction
            ClonedInputData = new int[NumElements];
            Array.Copy(InputData, ClonedInputData, NumElements);

            // Calculate the block and grid sizes needed for the first "level" of the reduction
            CalculateBlockAndGridSizes(2, NumElements, out NumThreadsPerBlock, out NumBlocks);
            Launcher.SetBlockSize(NumThreadsPerBlock);
            Launcher.SetGridSize(NumBlocks);

            // Start the reduction (and the timer)
            Console.Out.WriteLine("Testing reduce2 (Sequential addressing)...");
            Watch.Start();

            // Perform the first "level" of the reduction
            OutputData = new int[NumBlocks];
            Reduction.SequentialAddressing(ClonedInputData, OutputData);

            // If necessary, perform additional "levels" of reduction until we have only a single element (the result)
            while (NumBlocks > CpuThreshold)
            {
                // Calculate the number of threads and blocks based on the current input size
                CalculateBlockAndGridSizes(2, NumBlocks, out NumThreadsPerBlock, out NumBlocks);
                Launcher.SetBlockSize(NumThreadsPerBlock);
                Launcher.SetGridSize(NumBlocks);

                // Replace the current "level's" input data with the output data from the previous "level"
                ClonedInputData = OutputData;

                // Create a new array to hold the output data for this "level"
                OutputData = new int[NumBlocks];

                // Call the reduction method to perform the current "level" of reduction
                Reduction.SequentialAddressing(ClonedInputData, OutputData);
            }
            Watch.Stop();
            Console.Out.WriteLine("done. (Value = {0}, Time = {1:F02} ms)", OutputData[0], Watch.Elapsed.TotalMilliseconds);
            Console.Out.Write("Test ");
            if (OutputData[0] == CpuReductionValue) { ConsoleWriteLineColored("passed!", ConsoleColor.Green); }
            else { ConsoleWriteLineColored("failed!", ConsoleColor.Red); }
            Console.Out.WriteLine();
            Watch.Reset();

            #endregion

            // NOTE: reduce3 currently fails due to a compiler bug
            #region reduce3 (Sequential addressing with first reduction from global)

            // Clone the input data since we overwrite whatever data is used as input for the reduction
            ClonedInputData = new int[NumElements];
            Array.Copy(InputData, ClonedInputData, NumElements);

            // Calculate the block and grid sizes needed for the first "level" of the reduction
            CalculateBlockAndGridSizes(3, NumElements, out NumThreadsPerBlock, out NumBlocks);
            Launcher.SetBlockSize(NumThreadsPerBlock);
            Launcher.SetGridSize(NumBlocks);

            // Start the reduction (and the timer)
            Console.Out.WriteLine("Testing reduce3 (Sequential addressing with reduction from global)...");
            Watch.Start();

            // Perform the first "level" of the reduction
            OutputData = new int[NumBlocks];
            Reduction.SequentialAddressing(ClonedInputData, OutputData);

            // If necessary, perform additional "levels" of reduction until we have only a single element (the result)
            while (NumBlocks > CpuThreshold)
            {
                // TEMP: Calculate the result from the partial block sums (for testing purposes)
                int TempResult = 0;
                for (int i = 0; i < OutputData.Length; i++) { TempResult += OutputData[i]; }



                // Calculate the number of threads and blocks based on the current input size
                CalculateBlockAndGridSizes(3, NumBlocks, out NumThreadsPerBlock, out NumBlocks);
                Launcher.SetBlockSize(NumThreadsPerBlock);
                Launcher.SetGridSize(NumBlocks);

                // Replace the current "level's" input data with the output data from the previous "level"
                ClonedInputData = OutputData;

                // Create a new array to hold the output data for this "level"
                OutputData = new int[NumBlocks];

                // Call the reduction method to perform the current "level" of reduction
                Reduction.SequentialAddressing(ClonedInputData, OutputData);
            }
            Watch.Stop();
            Console.Out.WriteLine("done. (Value = {0}, Time = {1:F02} ms)", OutputData[0], Watch.Elapsed.TotalMilliseconds);
            Console.Out.Write("Test ");
            if (OutputData[0] == CpuReductionValue) { ConsoleWriteLineColored("passed!", ConsoleColor.Green); }
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
        /// 
        /// </summary>
        /// <param name="kernelNumber"></param>
        /// <param name="numElements"></param>
        /// <param name="numThreadsPerBlock"></param>
        /// <param name="numBlocks"></param>
        private static void CalculateBlockAndGridSizes(int kernelNumber, int numElements, out int numThreadsPerBlock, out int numBlocks)
        {
            // Preconditions
            if (kernelNumber < 0) { throw new ArgumentOutOfRangeException("kernelNumber", "The kernel number cannot be negative."); }
            else if (kernelNumber > 3) { throw new ArgumentOutOfRangeException("kernelNumber", "The kernel number cannot be greater than three (3)."); }
            else if (numElements < 1) { throw new ArgumentOutOfRangeException("numElements", "The number of elements cannot be less than one (1)."); }

            // Calculate and set the block and grid sizes
            if (kernelNumber < 3)
            {
                numThreadsPerBlock = (numElements < MaxThreadsPerBlock) ? NextPowerOfTwo(numElements) : MaxThreadsPerBlock;
                numBlocks = (numElements + numThreadsPerBlock - 1) / numThreadsPerBlock;
            }
            else
            {
                numThreadsPerBlock = (numElements < MaxThreadsPerBlock * 2) ? NextPowerOfTwo((numElements + 1) / 2) : MaxThreadsPerBlock;
                numBlocks = (numElements + (numThreadsPerBlock * 2 - 1)) / (numThreadsPerBlock * 2);
            }
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private static int NextPowerOfTwo(int n)
        {
            // Preconditions
            if (n < 1) { throw new ArgumentOutOfRangeException("n", "The input cannot be less than one (1)."); }

            // Compute the next-greatest power of two using a bit-manipulation formula
            int Result = n - 1;
            Result |= Result >> 1;
            Result |= Result >> 2;
            Result |= Result >> 4;
            Result |= Result >> 8;
            Result |= Result >> 16;
            return (Result + 1);
        }
    }
}
