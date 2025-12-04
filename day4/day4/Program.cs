using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace day4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string path = args.Length > 0 ? args[0] : "input.txt";
            string[] lines;

            if (File.Exists(path))
            {
                lines = File.ReadAllLines(path).Where(l => l.Length > 0).ToArray();
            }
            else
            {
                List<string> inputLines = new List<string>();
                string line;
                while ((line = Console.ReadLine()) != null)
                {
                    if (line.Length > 0)
                    {
                        inputLines.Add(line);
                    }
                }

                if (inputLines.Count == 0)
                {
                    Console.WriteLine("Keine Eingabe gefunden.");
                    return;
                }

                lines = inputLines.ToArray();
            }

            int rows = lines.Length;
            int cols = lines.Max(l => l.Length);

            char[,] grid = new char[rows, cols];
            for (int r = 0; r < rows; r++)
            {
                string l = lines[r].PadRight(cols, '.');
                for (int c = 0; c < cols; c++)
                {
                    grid[r, c] = l[c];
                }
            }

            (int, int)[] dirs = new (int, int)[]
            {
                (-1, -1), (-1, 0), (-1, 1),
                (0, -1),           (0, 1),
                (1, -1),  (1, 0),  (1, 1)
            };

            int part1 = CountAccessible(grid, rows, cols, dirs);
            int part2 = RemoveAllAccessible(grid, rows, cols, dirs);

            Console.WriteLine($"Part 1: {part1}");
            Console.WriteLine($"Part 2: {part2}");
        }

        static int CountAccessible(char[,] grid, int rows, int cols, (int, int)[] dirs)
        {
            int count = 0;
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (grid[r, c] != '@')
                    {
                        continue;
                    }

                    int neighborCount = 0;
                    for (int i = 0; i < dirs.Length; i++)
                    {
                        int nr = r + dirs[i].Item1;
                        int nc = c + dirs[i].Item2;
                        if (nr >= 0 && nr < rows && nc >= 0 && nc < cols && grid[nr, nc] == '@')
                        {
                            neighborCount++;
                            if (neighborCount >= 4)
                            {
                                break;
                            }
                        }
                    }

                    if (neighborCount < 4)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        static int RemoveAllAccessible(char[,] grid, int rows, int cols, (int, int)[] dirs)
        {
            char[,] workGrid = new char[rows, cols];
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    workGrid[r, c] = grid[r, c];
                }
            }

            int totalRemoved = 0;

            while (true)
            {
                List<(int, int)> accessible = new List<(int, int)>();

                for (int r = 0; r < rows; r++)
                {
                    for (int c = 0; c < cols; c++)
                    {
                        if (workGrid[r, c] != '@')
                        {
                            continue;
                        }

                        int neighborCount = 0;
                        for (int i = 0; i < dirs.Length; i++)
                        {
                            int nr = r + dirs[i].Item1;
                            int nc = c + dirs[i].Item2;
                            if (nr >= 0 && nr < rows && nc >= 0 && nc < cols && workGrid[nr, nc] == '@')
                            {
                                neighborCount++;
                                if (neighborCount >= 4)
                                {
                                    break;
                                }
                            }
                        }

                        if (neighborCount < 4)
                        {
                            accessible.Add((r, c));
                        }
                    }
                }

                if (accessible.Count == 0)
                {
                    break;
                }

                for (int i = 0; i < accessible.Count; i++)
                {
                    workGrid[accessible[i].Item1, accessible[i].Item2] = '.';
                    totalRemoved++;
                }
            }

            return totalRemoved;
        }
    }
}
