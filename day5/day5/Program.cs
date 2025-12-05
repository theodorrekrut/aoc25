using System;
using System.IO;

namespace day5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("input.txt");
            
            int rangeCount = 0;
            int idCount = 0;
            bool countingRanges = true;

            for (int i = 0; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                {
                    countingRanges = false;
                }
                else if (countingRanges)
                {
                    rangeCount++;
                }
                else
                {
                    idCount++;
                }
            }

            long[] rangeMin = new long[rangeCount];
            long[] rangeMax = new long[rangeCount];
            long[] availableIds = new long[idCount];

            int rangeIndex = 0;
            int idIndex = 0;
            bool parsingRanges = true;

            for (int i = 0; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                {
                    parsingRanges = false;
                }
                else if (parsingRanges)
                {
                    int dashPos = lines[i].IndexOf('-');
                    rangeMin[rangeIndex] = long.Parse(lines[i].Substring(0, dashPos));
                    rangeMax[rangeIndex] = long.Parse(lines[i].Substring(dashPos + 1));
                    rangeIndex++;
                }
                else
                {
                    availableIds[idIndex] = long.Parse(lines[i]);
                    idIndex++;
                }
            }

            int freshCount = 0;
            for (int i = 0; i < idCount; i++)
            {
                int j = 0;
                while (j < rangeCount && (availableIds[i] < rangeMin[j] || availableIds[i] > rangeMax[j]))
                {
                    j++;
                }
                if (j < rangeCount)
                {
                    freshCount++;
                }
            }

            Console.WriteLine(freshCount);

            for (int i = 0; i < rangeCount - 1; i++)
            {
                for (int j = i + 1; j < rangeCount; j++)
                {
                    if (rangeMin[j] < rangeMin[i])
                    {
                        (rangeMin[i], rangeMin[j]) = (rangeMin[j], rangeMin[i]);
                        (rangeMax[i], rangeMax[j]) = (rangeMax[j], rangeMax[i]);
                    }
                }
            }

            long total = 0;
            long min = rangeMin[0];
            long max = rangeMax[0];

            for (int i = 1; i < rangeCount; i++)
            {
                if (rangeMin[i] <= max + 1)
                {
                    if (rangeMax[i] > max)
                    {
                        max = rangeMax[i];
                    }
                }
                else
                {
                    total += max - min + 1;
                    min = rangeMin[i];
                    max = rangeMax[i];
                }
            }

            total += max - min + 1;

            Console.WriteLine(total);
            }
    }
}
