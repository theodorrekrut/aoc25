using System;
using System.IO;
using System.Numerics;

namespace day6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filePath;
            if (args.Length > 0)
                filePath = args[0];
            else
                filePath = Path.Combine(AppContext.BaseDirectory, "input.txt");

            if (!File.Exists(filePath))
            {
                Console.Error.WriteLine("Input-Datei nicht gefunden: " + filePath);
                return;
            }

            string[] rawLines = File.ReadAllLines(filePath);
            if (rawLines.Length == 0)
            {
                Console.WriteLine("Keine Eingabedaten.");
                return;
            }

            int rows = rawLines.Length;
            int cols = 0;
            for (int i = 0; i < rawLines.Length; i++)
            {
                if (rawLines[i].Length > cols) cols = rawLines[i].Length;
            }

            string[] lines = new string[rows];
            for (int r = 0; r < rows; r++)
            {
                lines[r] = rawLines[r].Replace('\t', ' ').PadRight(cols);
            }

            bool[] isSeparator = new bool[cols];
            for (int c = 0; c < cols; c++)
            {
                bool allSpace = true;
                for (int r = 0; r < rows; r++)
                {
                    if (lines[r][c] != ' ')
                    {
                        allSpace = false;
                        break;
                    }
                }
                isSeparator[c] = allSpace;
            }

            BigInteger grandTotalPart1 = BigInteger.Zero;
            BigInteger grandTotalPart2 = BigInteger.Zero;
            int col = 0;
            while (col < cols)
            {
                if (isSeparator[col])
                {
                    col++;
                }
                else
                {
                    int start = col;
                    while (col < cols && !isSeparator[col]) col++;
                    int end = col - 1;
                    int width = end - start + 1;

                    char opChar = '\0';
                    for (int c = start; c <= end; c++)
                    {
                        char ch = lines[rows - 1][c];
                        if (ch == '+' || ch == '*')
                        {
                            opChar = ch;
                            break;
                        }
                    }

                    if (opChar == '\0') continue;

                    int countNumbers1 = 0;
                    for (int r = 0; r < rows - 1; r++)
                    {
                        string cell = lines[r].Substring(start, width).Trim();
                        if (cell.Length != 0) countNumbers1++;
                    }

                    if (countNumbers1 != 0)
                    {
                        BigInteger[] numbers1 = new BigInteger[countNumbers1];
                        int idx1 = 0;
                        for (int r = 0; r < rows - 1; r++)
                        {
                            string cell = lines[r].Substring(start, width).Trim();
                            if (cell.Length != 0)
                            {
                                BigInteger parsed1;
                                if (!BigInteger.TryParse(cell, out parsed1))
                                {
                                    Console.Error.WriteLine("Zahl konnte nicht geparst werden: \"" + cell + "\" (Zeile " + (r + 1) + ")");
                                    return;
                                }
                                numbers1[idx1] = parsed1;
                                idx1++;
                            }
                        }

                        BigInteger result1;
                        if (opChar == '+')
                        {
                            result1 = BigInteger.Zero;
                            for (int ni = 0; ni < numbers1.Length; ni++) result1 += numbers1[ni];
                        }
                        else
                        {
                            result1 = BigInteger.One;
                            for (int ni = 0; ni < numbers1.Length; ni++) result1 *= numbers1[ni];
                        }

                        grandTotalPart1 += result1;
                    }

                    int countNumbers2 = 0;
                    for (int c = end; c >= start; c--)
                    {
                        char[] digits = new char[rows - 1];
                        for (int r = 0; r < rows - 1; r++) digits[r] = lines[r][c];
                        string s = new string(digits).Trim();
                        if (s.Length != 0) countNumbers2++;
                    }

                    if (countNumbers2 != 0)
                    {
                        BigInteger[] numbers2 = new BigInteger[countNumbers2];
                        int idx2 = 0;
                        for (int c = end; c >= start; c--)
                        {
                            char[] digits = new char[rows - 1];
                            for (int r = 0; r < rows - 1; r++) digits[r] = lines[r][c];
                            string s = new string(digits).Trim();
                            if (s.Length != 0)
                            {
                                BigInteger parsed2;
                                if (!BigInteger.TryParse(s, out parsed2))
                                {
                                    Console.Error.WriteLine("Zahl konnte nicht geparst werden: \"" + s + "\" (Spalte " + (c + 1) + ")");
                                    return;
                                }
                                numbers2[idx2] = parsed2;
                                idx2++;
                            }
                        }

                        BigInteger result2;
                        if (opChar == '+')
                        {
                            result2 = BigInteger.Zero;
                            for (int ni = 0; ni < numbers2.Length; ni++) result2 += numbers2[ni];
                        }
                        else
                        {
                            result2 = BigInteger.One;
                            for (int ni = 0; ni < numbers2.Length; ni++) result2 *= numbers2[ni];
                        }

                        grandTotalPart2 += result2;
                    }
                }
            }

            Console.WriteLine("Part 1: " + grandTotalPart1);
            Console.WriteLine("Part 2: " + grandTotalPart2);
        }
    }
}
