using System;
using System.IO;

class Program
{
    static char[,] grid;
    static int rows;
    static int cols;
    static long totalTimelines = 0;

    static void Main()
    {
        // Input einlesen
        var lines = File.ReadAllLines("input.txt");
        rows = lines.Length;
        cols = lines[0].Length;

        grid = new char[rows, cols];
        int startCol = 0;

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                grid[r, c] = lines[r][c];
                if (lines[r][c] == 'S')
                    startCol = c;
            }
        }

        // Rekursiver Start vom S
        totalTimelines = CountTimelines(1, startCol);

        Console.WriteLine("Total timelines: " + totalTimelines);
    }

    // Gibt die Anzahl der Zeitleisten zurück, die von dieser Position ausgehen
    static long CountTimelines(int row, int col)
    {
        // Abbruch: außerhalb des Grids
        if (row >= rows || col < 0 || col >= cols) 
            return 1;

        if (grid[row, col] == '.')
        {
            // Weiter nach unten
            return CountTimelines(row + 1, col);
        }
        else if (grid[row, col] == '^')
        {
            // Splitter: links + rechts
            long left = CountTimelines(row + 1, col - 1);
            long right = CountTimelines(row + 1, col + 1);
            return left + right;
        }
        else
        {
            // alles andere (theoretisch nur S oben) → Strahl weiter
            return CountTimelines(row + 1, col);
        }
    }
}
