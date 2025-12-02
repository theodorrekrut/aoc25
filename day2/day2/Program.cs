using System;

namespace day2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string input = "4487-9581,755745207-755766099,954895848-955063124,4358832-4497315,15-47,1-12,9198808-9258771,657981-762275,6256098346-6256303872,142-282,13092529-13179528,96201296-96341879,19767340-19916378,2809036-2830862,335850-499986,172437-315144,764434-793133,910543-1082670,2142179-2279203,6649545-6713098,6464587849-6464677024,858399-904491,1328-4021,72798-159206,89777719-90005812,91891792-91938279,314-963,48-130,527903-594370,24240-602125-12154422,2323146444-2323289192,37-57,101-137,46550018-46679958,79-96,317592-341913,495310-629360,33246-46690,14711-22848,1-17,2850-4167,3723700171-3723785996,190169-242137,272559-298768,275-365,7697-11193,61-78,75373-110112,425397-451337,9796507-9899607,991845-1013464,77531934-77616074";
            // test
            string[] parts = input.Split(',');
            int partsCount = parts.Length;

            long[] lo = new long[partsCount];
            long[] hi = new long[partsCount];

            for (int i = 0; i < partsCount; i++)
            {
                string[] p = parts[i].Split('-', 2);
                lo[i] = long.Parse(p[0]);
                hi[i] = long.Parse(p[1]);
            }

            int maxDigits = 0;
            for (int i = 0; i < partsCount; i++)
            {
                int d = hi[i].ToString().Length;
                if (d > maxDigits) maxDigits = d;
            }

            int maxHalfLen = maxDigits / 2;

            long[] found = new long[16];
            int foundCount = 0;

            for (int halfLen = 1; halfLen <= maxHalfLen; halfLen++)
            {
                long pow = 1;
                for (int k = 0; k < halfLen; k++) pow *= 10L;

                long start = pow / 10L;
                long end = pow - 1L;

                for (long half = start; half <= end; half++)
                {
                    long n = half * pow + half;

                    for (int r = 0; r < partsCount; r++)
                    {
                        if (n >= lo[r] && n <= hi[r])
                        {
                            bool exists = false;
                            for (int e = 0; e < foundCount; e++)
                            {
                                if (found[e] == n)
                                {
                                    exists = true;
                                    break;
                                }
                            }

                            if (!exists)
                            {
                                if (foundCount == found.Length)
                                {
                                    long[] tmp = new long[found.Length * 2];
                                    for (int c = 0; c < found.Length; c++) tmp[c] = found[c];
                                    found = tmp;
                                }

                                found[foundCount] = n;
                                foundCount++;
                            }

                            break;
                        }
                    }
                }
            }

            long sum = 0L;
            for (int i = 0; i < foundCount; i++) sum += found[i];

            Console.WriteLine(sum);

            long sumPart2 = SumPart2(input);
            Console.WriteLine(sumPart2);
        }

        static long SumPart2(string input)
        {
            string[] parts = input.Split(',', StringSplitOptions.RemoveEmptyEntries);
            int partsCount = parts.Length;

            long[] lo = new long[partsCount];
            long[] hi = new long[partsCount];

            for (int i = 0; i < partsCount; i++)
            {
                string[] p = parts[i].Split('-', 2);
                lo[i] = long.Parse(p[0]);
                hi[i] = long.Parse(p[1]);
            }

            int maxDigits = 0;
            for (int i = 0; i < partsCount; i++)
            {
                int d = hi[i].ToString().Length;
                if (d > maxDigits) maxDigits = d;
            }

            int maxBaseLen = maxDigits / 2; 

            long[] found = new long[16];
            int foundCount = 0;

            for (int baseLen = 1; baseLen <= maxBaseLen; baseLen++)
            {
                long pow = 1L;
                for (int t = 0; t < baseLen; t++) pow *= 10L;

                long start = pow / 10L;
                long end = pow - 1L;

                for (long baseNum = start; baseNum <= end; baseNum++)
                {
                    int rep = 2;
                    while (baseLen * rep <= maxDigits)
                    {
                        long n = 0L;
                        for (int r = 0; r < rep; r++)
                        {
                            n = n * pow + baseNum;
                        }

                        for (int j = 0; j < partsCount; j++)
                        {
                            if (n >= lo[j] && n <= hi[j])
                            {
                                bool exists = false;
                                for (int e = 0; e < foundCount; e++)
                                {
                                    if (found[e] == n)
                                    {
                                        exists = true;
                                        break;
                                    }
                                }

                                if (!exists)
                                {
                                    if (foundCount == found.Length)
                                    {
                                        long[] tmp = new long[found.Length * 2];
                                        for (int c = 0; c < found.Length; c++) tmp[c] = found[c];
                                        found = tmp;
                                    }

                                    found[foundCount] = n;
                                    foundCount++;
                                }

                                break;
                            }
                        }

                        rep++;
                    }
                }
            }

            long sum = 0L;
            for (int i = 0; i < foundCount; i++) sum += found[i];

            return sum;
        }
    }
}
