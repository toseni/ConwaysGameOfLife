using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using ConwaysGameOfLife;
using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<LiveNeighborsCount>();
        }
    }

    [MemoryDiagnoser]
    public class LiveNeighborsCount
    {
        private const int Size = 20;
        private readonly bool[,] Frame;

        public LiveNeighborsCount()
        {
            Frame = new bool[Size, Size];
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Frame[i, j] = (i + 1) * (j + 1) % 2 == 0;
                }
            }
        }

        [Benchmark(Baseline = true)]
        public int Normal()
        {
            var (i, j) = (0, 15);

            Span<(int I, int J)> neighborsPos = stackalloc (int, int)[]
            {
                (-1 + i, -1 + j),
                (-1 + i, 0 + j),
                (-1 + i, 1 + j),
                (0 + i, -1 + j),
                (0 + i, 1 + j),
                (1 + i, -1 + j),
                (1 + i, 0 + j),
                (1 + i, 1 + j),
            };

            var count = 0;
            for (int index = 0; index < neighborsPos.Length; index++)
            {
                var (I, J) = neighborsPos[index];

                if (I >= 0 && I < Size && J >= 0 && J < Size)
                {
                    if (Frame[I, J])
                    {
                        count += 1;
                    }
                }
            }

            return count;
        }

        [Benchmark]
        public int Branchless()
        {
            var (i, j) = (0, 15);

            Span<(int I, int J)> neighborsPos = stackalloc (int, int)[]
            {
                (-1 + i, -1 + j),
                (-1 + i, 0 + j),
                (-1 + i, 1 + j),
                (0 + i, -1 + j),
                (0 + i, 1 + j),
                (1 + i, -1 + j),
                (1 + i, 0 + j),
                (1 + i, 1 + j),
            };

            var count = 0;
            for (int index = 0; index < neighborsPos.Length; index++)
            {
                var (I, J) = neighborsPos[index];

                var isLiveNeihbor = I >= 0 && I < Size && J >= 0 && J < Size && Frame[I, J];
                count += Unsafe.As<bool, byte>(ref isLiveNeihbor);
            }

            return count;
        }
    }
}
