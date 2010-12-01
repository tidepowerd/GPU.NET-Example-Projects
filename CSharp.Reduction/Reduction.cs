// Copyright 2010 -- TidePowerd, Ltd. All rights reserved.
// http://www.tidepowerd.com
//
// GPU.NET Reduction Example (CSharp.Reduction)
// Modified: 01-Dec-2010
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
        /// Performs a sum-reduction on an array.
        /// </summary>
        /// <param name="input"></param>
        /// <remarks>
        /// This reduction interleaves which threads are active by using the modulo operator.  This operator is very expensive on GPUs, and the interleaved inactivity means that no whole warps are active, which is also very inefficient.
        /// </remarks>
        [Kernel]
        [Description("Interleaved access with modulo operator")]
        public static void InterleavedModulo(int[] input, int[] output)            // reduce0
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
        /// Performs a sum-reduction on an array.
        /// </summary>
        /// <param name="input"></param>
        [Kernel]
        [Description("Interleaved contiguous access")]
        public static void InterleavedContiguous(int[] input, int[] output)        // reduce1
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
        [Kernel]
        [Description("Sequential addressing")]
        public static void SequentialAddressing(int[] input, int[] output)         // reduce2
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
        [Kernel]
        [Description("Sequential addressing with first level of reduction from global memory")]
        public static void FirstReductionFromGlobal(int[] input, int[] output)     // reduce3
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

        #endregion
    }
}
