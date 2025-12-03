using System;
using System.IO;
using System.Numerics;
using System.Linq;
using System.Collections.Generic;

class Program
{
    static string MaxSubsequence(string digits, int k)
    {
        int n = digits.Length;
        if (k <= 0) return "";
        if (k >= n) return digits;

        int toRemove = n - k;
        var stack = new List<char>();

        foreach (char c in digits)
        {
            while (stack.Count > 0 && toRemove > 0 && stack[stack.Count - 1] < c)
            {
                stack.RemoveAt(stack.Count - 1);
                toRemove--;
            }
            stack.Add(c);
        }

        // Falls noch Zeichen zu entfernen sind (z.B. alle Ziffern non-increasing), entferne vom Ende
        while (toRemove > 0)
        {
            stack.RemoveAt(stack.Count - 1);
            toRemove--;
        }

        return new string(stack.Take(k).ToArray());
    }

    static void Main(string[] args)
    {
        string path = args.Length > 0 ? args[0] : "large_number_dataset.txt";
        if (!File.Exists(path))
        {
            Console.WriteLine($"Datei '{path}' nicht gefunden im Verzeichnis {Directory.GetCurrentDirectory()}.");
            Console.WriteLine("Lege die Datei in dasselbe Verzeichnis wie das Programm oder übergebe den Pfad als Argument.");
            return;
        }

        int K1 = 2;
        int K2 = 12;
        BigInteger totalK1 = 0;
        BigInteger totalK2 = 0;
        int lineNo = 0;

        using (var reader = new StreamReader(path))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                lineNo++;
                // nur Ziffern extrahieren (falls Zeile unerwartete Zeichen enthält)
                string digits = new string(line.Where(char.IsDigit).ToArray());
                if (digits.Length == 0)
                {
                    Console.WriteLine($"Zeile {lineNo}: keine Ziffern, übersprungen.");
                    continue;
                }

                if (digits.Length < K1 || digits.Length < K2)
                {
                    Console.WriteLine($"Zeile {lineNo}: nur {digits.Length} Ziffern (< {K2}) — wird für die jeweiligen K entsprechend behandelt.");
                }

                string chosen1 = digits.Length >= K1 ? MaxSubsequence(digits, K1) : digits.PadRight(K1, '0').Substring(0, K1);
                string chosen2 = digits.Length >= K2 ? MaxSubsequence(digits, K2) : digits.PadRight(K2, '0').Substring(0, K2);

                BigInteger value1 = BigInteger.Parse(chosen1);
                BigInteger value2 = BigInteger.Parse(chosen2);

                totalK1 += value1;
                totalK2 += value2;

                Console.WriteLine($"Zeile {lineNo}: chosen({K1}) = {chosen1}  value = {value1} | chosen({K2}) = {chosen2}  value = {value2}");
            }
        }

        Console.WriteLine();
        Console.WriteLine($"Summe aller {K1}-stelligen Maxima: {totalK1}");
        Console.WriteLine($"Summe aller {K2}-stelligen Maxima: {totalK2}");
    }
}