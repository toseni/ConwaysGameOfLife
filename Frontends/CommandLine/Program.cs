using System;
using System.Text;
using System.Threading.Tasks;
using ConwaysGameOfLife;

namespace CommandLine
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var plane = new Plane(45, 85);
            plane.RandomizeFrame();

            if (OperatingSystem.IsWindows()) 
            { 
                Console.SetWindowSize(100, 50);
            }

            for (int i = 0; i < 120; i++)
            {
                DisplayFrameInConsole(plane);
                plane.Tick();
                await Task.Delay(50);
            }
        }

        public static void DisplayFrameInConsole(Plane p)
        {
            var output = new StringBuilder((p.Height + 2) * (p.Width + 4));
            output.AppendLine(new string('-', p.Width + 2));
            for (int i = 0; i < p.Height; i++)
            {
                output.Append("|");
                for (int j = 0; j < p.Width; j++)
                {
                    output.Append(p.Frame[i, j] ? "+" : " ");
                }

                output.AppendLine("|");
            }
            output.AppendLine(new string('-', p.Width + 2));

            Console.Clear();
            Console.Write(output);
        }
    }
}
