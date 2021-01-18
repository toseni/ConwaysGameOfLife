using System;
using System.Runtime.CompilerServices;

namespace ConwaysGameOfLife
{
    public class Plane
    {
        public int Height { get; }
        public int Width { get; }
        public bool[,] Frame { get; private set; }

        public Plane(int size) : this(size, size) { }

        public Plane(int height, int width)
        {
            Height = height;
            Width = width;
            Frame = new bool[Height, Width];
        }

        public void Tick()
        {
            var oldFrame = Frame;
            Frame = new bool[Height, Width];

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    ref var wasAlive = ref oldFrame[i, j];
                    var liveNeighbors = LiveNeighborsCount(oldFrame, i, j);

                    var isAlive = (wasAlive, liveNeighbors) switch
                    {
                        (true, 2 or 3) => true,
                        (false, 3) => true,
                        _ => false
                    };

                    Frame[i, j] = isAlive;
                }
            }
        }

        public void RandomizeFrame()
        {
            var random = new Random();

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Frame[i, j] = random.Next(0, 2) == 0;
                }
            }
        }

        private int LiveNeighborsCount(bool[,] frame, int i, int j)
        {
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

                var isLiveNeihbor = IndexInsideBounds(I, J) && frame[I, J];
                count += Unsafe.As<bool, byte>(ref isLiveNeihbor);
            }

            return count;
        }

        private bool IndexInsideBounds(int i, int j) => i >= 0 && i < Height && j >= 0 && j < Width;
    }
}
