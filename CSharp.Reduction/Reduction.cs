// Copyright 2010-2011 -- TidePowerd, Ltd. All rights reserved.
// http://www.tidepowerd.com
//
// GPU.NET Reduction Example (CSharp.Reduction)
// Modified: 17-Jan-2011
//
// More examples available at: http://github.com/tidepowerd/GPU.NET-Example-Projects
//

using System;
using System.ComponentModel;

using TidePowerd.DeviceMethods;

namespace TidePowerd.Example.CSharp.Reduction.Cli
{
    /// <summary>
    /// Contains methods for performing sum-reductions on arrays.
    /// </summary>
    internal static class Reduction
    {
        #region Constants

        /// <summary>
        /// The number of threads per thread-block.
        /// </summary>
        private const int MaxThreadsPerBlock = 256;

        /// <summary>
        /// The maximum number of block which we'll allow to be used.
        /// </summary>
        private const int MaxBlocks = 64;

        /// <summary>
        /// The threshold number of elements at which (or below) we finish the reduction on the CPU.
        /// </summary>
        private const int CpuThreshold = 1;

        #endregion

        #region Fields

        /// <summary>
        /// Holds intermediate results during a series of reduction steps.
        /// </summary>
        [SharedMemory(0x200)]
        private static readonly int[] IntermediateResults1 = null;  // Used for reduce0-reduce2

        /// <summary>
        /// Holds intermediate results during a series of reduction steps.
        /// </summary>
        [SharedMemory(0x200)]
        private static readonly int[] IntermediateResults2 = null;  // Used for reduce3+

        #endregion

        #region Methods

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
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int InterleavedModulo(int[] input)
        {
            // Preconditions
            if (input == null) { throw new ArgumentNullException("input"); }

            // Declare variables to hold the current number of threads per block and number of blocks; re-used throughout the code below
            int NumThreadsPerBlock = 0, NumBlocks = 0;

            // Calculate the block and grid sizes needed for the first "level" of the reduction
            CalculateBlockAndGridSizes(0, input.Length, out NumThreadsPerBlock, out NumBlocks);
            Launcher.SetBlockSize(NumThreadsPerBlock);
            Launcher.SetGridSize(NumBlocks);

            // Perform the first "level" of the reduction
            int[] OutputData = new int[NumBlocks];
            Reduction.InterleavedModuloKernel(input, OutputData);

            // If necessary, perform additional "levels" of reduction until we have only a single element (the result)
            while (NumBlocks > CpuThreshold)
            {
                // Calculate the number of threads and blocks based on the current input size
                CalculateBlockAndGridSizes(0, NumBlocks, out NumThreadsPerBlock, out NumBlocks);
                Launcher.SetBlockSize(NumThreadsPerBlock);
                Launcher.SetGridSize(NumBlocks);

                // Replace the current "level's" input data with the output data from the previous "level"
                input = OutputData;

                // Create a new array to hold the output data for this "level"
                OutputData = new int[NumBlocks];

                // Call the reduction method to perform the current "level" of reduction
                Reduction.InterleavedModuloKernel(input, OutputData);
            }

            // Return the final reduction value
            return OutputData[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        private static void InterleavedModuloCustomFallback(int[] input, int[] output)
        {
            //
            throw new NotImplementedException();
        }

        /// <summary>
        /// Performs a sum-reduction on an array.
        /// </summary>
        /// <param name="input"></param>
        /// <remarks>
        /// This reduction interleaves which threads are active by using the modulo operator.  This operator is very expensive on GPUs, and the interleaved inactivity means that no whole warps are active, which is also very inefficient.
        /// </remarks>
        [Kernel(CustomFallbackMethod = "InterleavedModuloCustomFallback")]
        private static void InterleavedModuloKernel(int[] input, int[] output)            // reduce0
        {
            // Get the thread index, then calculate the overall thread index (the thread index within all threads in the current execution configuration)
            int ThreadId = ThreadIndex.X;
            int OverallThreadId = (BlockIndex.X * BlockDimension.X) + ThreadIndex.X;

            // Load an element from the data array into shared memory
            IntermediateResults1[ThreadId] = (OverallThreadId < input.Length) ? input[OverallThreadId] : 0;

            // Sync all threads to make sure the data is actually stored in shared memory before beginning the reduction
            Kernel.SyncThreads();

            // Perform the reduction in shared memory
            for (int s = 1; s < BlockDimension.X; s *= 2)
            {
                // Modulo arithmetic is slow on the GPU!
                if ((ThreadId % (2 * s)) == 0)
                {
                    IntermediateResults1[ThreadId] += IntermediateResults1[ThreadId + s];
                }

                // Need to synchronize threads to ensure that the results from this step are calculated and stored back to shared memory before proceeding to the next step
                Kernel.SyncThreads();
            }

            // Write the result for this thread block to the output array in global memory
            if (ThreadId == 0)
            {
                output[BlockIndex.X] = IntermediateResults1[0];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int InterleavedContiguous(int[] input)
        {
            // Preconditions
            if (input == null) { throw new ArgumentNullException("input"); }

            // Declare variables to hold the current number of threads per block and number of blocks; re-used throughout the code below
            int NumThreadsPerBlock = 0, NumBlocks = 0;

            // Calculate the block and grid sizes needed for the first "level" of the reduction
            CalculateBlockAndGridSizes(1, input.Length, out NumThreadsPerBlock, out NumBlocks);
            Launcher.SetBlockSize(NumThreadsPerBlock);
            Launcher.SetGridSize(NumBlocks);

            // Perform the first "level" of the reduction
            int[] OutputData = new int[NumBlocks];
            Reduction.InterleavedContiguousKernel(input, OutputData);

            // If necessary, perform additional "levels" of reduction until we have only a single element (the result)
            while (NumBlocks > CpuThreshold)
            {
                // Calculate the number of threads and blocks based on the current input size
                CalculateBlockAndGridSizes(1, NumBlocks, out NumThreadsPerBlock, out NumBlocks);
                Launcher.SetBlockSize(NumThreadsPerBlock);
                Launcher.SetGridSize(NumBlocks);

                // Replace the current "level's" input data with the output data from the previous "level"
                input = OutputData;

                // Create a new array to hold the output data for this "level"
                OutputData = new int[NumBlocks];

                // Call the reduction method to perform the current "level" of reduction
                Reduction.InterleavedContiguousKernel(input, OutputData);
            }

            // Return the final reduction value
            return OutputData[0];
        }

        /// <summary>
        /// Performs a sum-reduction on an array.
        /// </summary>
        /// <param name="input"></param>
        [Kernel]
        private static void InterleavedContiguousKernel(int[] input, int[] output)        // reduce1
        {
            // Get the thread index, then calculate the overall thread index (the thread index within all threads in the current execution configuration)
            int ThreadId = ThreadIndex.X;
            int OverallThreadId = (BlockIndex.X * BlockDimension.X) + ThreadIndex.X;

            // Load an element from the data array into shared memory
            IntermediateResults1[ThreadId] = (OverallThreadId < input.Length) ? input[OverallThreadId] : 0;

            // Sync all threads to make sure the data is actually stored in shared memory before beginning the reduction
            Kernel.SyncThreads();

            // Perform the reduction in shared memory
            for (int s = 1; s < BlockDimension.X; s *= 2)
            {
                // Calculate the "right" element index?
                int Index = 2 * s * ThreadId;

                // Perform the current step of the reduction
                if (Index < BlockDimension.X)
                {
                    IntermediateResults1[Index] += IntermediateResults1[Index + s];
                }

                // Need to synchronize threads to ensure that the results from this step are calculated and stored back to shared memory before proceeding to the next step
                Kernel.SyncThreads();
            }

            // Write the result for this thread block to the output array in global memory
            if (ThreadId == 0)
            {
                output[BlockIndex.X] = IntermediateResults1[0];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int SequentialAddressing(int[] input)
        {
            // Preconditions
            if (input == null) { throw new ArgumentNullException("input"); }

            // Declare variables to hold the current number of threads per block and number of blocks; re-used throughout the code below
            int NumThreadsPerBlock = 0, NumBlocks = 0;

            // Calculate the block and grid sizes needed for the first "level" of the reduction
            CalculateBlockAndGridSizes(2, input.Length, out NumThreadsPerBlock, out NumBlocks);
            Launcher.SetBlockSize(NumThreadsPerBlock);
            Launcher.SetGridSize(NumBlocks);

            // Perform the first "level" of the reduction
            int[] OutputData = new int[NumBlocks];
            Reduction.SequentialAddressingKernel(input, OutputData);

            // If necessary, perform additional "levels" of reduction until we have only a single element (the result)
            while (NumBlocks > CpuThreshold)
            {
                // Calculate the number of threads and blocks based on the current input size
                CalculateBlockAndGridSizes(2, NumBlocks, out NumThreadsPerBlock, out NumBlocks);
                Launcher.SetBlockSize(NumThreadsPerBlock);
                Launcher.SetGridSize(NumBlocks);

                // Replace the current "level's" input data with the output data from the previous "level"
                input = OutputData;

                // Create a new array to hold the output data for this "level"
                OutputData = new int[NumBlocks];

                // Call the reduction method to perform the current "level" of reduction
                Reduction.SequentialAddressingKernel(input, OutputData);
            }

            // Return the final reduction value
            return OutputData[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        [Kernel]
        private static void SequentialAddressingKernel(int[] input, int[] output)         // reduce2
        {
            // Get the thread index, then calculate the overall thread index (the thread index within all threads in the current execution configuration)
            int ThreadId = ThreadIndex.X;
            int OverallThreadId = (BlockIndex.X * BlockDimension.X) + ThreadIndex.X;

            // Load an element from the data array into shared memory
            IntermediateResults1[ThreadId] = (OverallThreadId < input.Length) ? input[OverallThreadId] : 0;

            // Sync all threads to make sure the data is actually stored in shared memory before beginning the reduction
            Kernel.SyncThreads();

            // Perform the reduction in shared memory
            for (int s = BlockDimension.X / 2; s > 0; s >>= 1)
            {
                // Perform the current step of the reduction
                if (ThreadId < s)
                {
                    IntermediateResults1[ThreadId] += IntermediateResults1[ThreadId + s];
                }

                // Need to synchronize threads to ensure that the results from this step are calculated and stored back to shared memory before proceeding to the next step
                Kernel.SyncThreads();
            }

            // Write the result for this thread block to the output array in global memory
            if (ThreadId == 0)
            {
                output[BlockIndex.X] = IntermediateResults1[0];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int FirstReductionFromGlobal(int[] input)
        {
            // Preconditions
            if (input == null) { throw new ArgumentNullException("input"); }

            // Declare variables to hold the current number of threads per block and number of blocks; re-used throughout the code below
            int NumThreadsPerBlock = 0, NumBlocks = 0;

            // Calculate the block and grid sizes needed for the first "level" of the reduction
            CalculateBlockAndGridSizes(3, input.Length, out NumThreadsPerBlock, out NumBlocks);
            Launcher.SetBlockSize(NumThreadsPerBlock);
            Launcher.SetGridSize(NumBlocks);

            // Perform the first "level" of the reduction
            int[] OutputData = new int[NumBlocks];
            Reduction.FirstReductionFromGlobalKernel(input, OutputData);

            // If necessary, perform additional "levels" of reduction until we have only a single element (the result)
            while (NumBlocks > CpuThreshold)
            {
                // Calculate the number of threads and blocks based on the current input size
                CalculateBlockAndGridSizes(3, NumBlocks, out NumThreadsPerBlock, out NumBlocks);
                Launcher.SetBlockSize(NumThreadsPerBlock);
                Launcher.SetGridSize(NumBlocks);

                // Replace the current "level's" input data with the output data from the previous "level"
                input = OutputData;

                // Create a new array to hold the output data for this "level"
                OutputData = new int[NumBlocks];

                // Call the reduction method to perform the current "level" of reduction
                Reduction.FirstReductionFromGlobalKernel(input, OutputData);
            }

            // Return the final reduction value
            return OutputData[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        [Kernel]
        private static void FirstReductionFromGlobalKernel(int[] input, int[] output)     // reduce3
        {
            // Get the thread index, then calculate the overall thread index (the thread index within all threads in the current execution configuration)
            int ThreadId = ThreadIndex.X;
            int OverallThreadId = (BlockIndex.X * BlockDimension.X) + ThreadIndex.X;

            // Perform the first "level" of reduction, reading in two elements from global memory, performing the reduction on them, and writing the result to shared memory
            int MySum = (OverallThreadId < input.Length) ? input[OverallThreadId] : 0;
            if (OverallThreadId + BlockDimension.X < input.Length)
            {
                MySum += input[OverallThreadId + BlockDimension.X];
            }

            IntermediateResults2[ThreadId] = MySum;

            // Sync all threads to make sure the data is actually stored in shared memory before beginning the reduction
            Kernel.SyncThreads();

            // Perform the reduction in shared memory
            for (int s = BlockDimension.X / 2; s > 0; s >>= 1)
            {
                // Perform the current step of the reduction
                if (ThreadId < s)
                {
                    IntermediateResults2[ThreadId] = MySum = MySum + IntermediateResults2[ThreadId + s];
                    //IntermediateResults2[ThreadId] += IntermediateResults2[ThreadId + s];
                }

                // Need to synchronize threads to ensure that the results from this step are calculated and stored back to shared memory before proceeding to the next step
                Kernel.SyncThreads();
            }

            // Write the result for this thread block to the output array in global memory
            if (ThreadId == 0)
            {
                output[BlockIndex.X] = IntermediateResults2[0];
            }
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

        #endregion
    }
}
