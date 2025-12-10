using System;
using System.IO;

namespace day8
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] rawLines = ReadInputLines(args);

            int countNonEmpty = 0;
            for (int i = 0; i < rawLines.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(rawLines[i])) countNonEmpty++;
            }

            if (countNonEmpty == 0)
            {
                Console.WriteLine("Keine Eingabepunkte gefunden.");
                return;
            }

            Point[] points = new Point[countNonEmpty];
            int fillIndex = 0;
            for (int i = 0; i < rawLines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(rawLines[i])) continue;
                points[fillIndex++] = ParsePoint(rawLines[i]);
            }

            Edge[] edges = BuildAllEdges(points);
            Array.Sort(edges, (Edge a, Edge b) =>
            {
                int cmp = a.DistanceSquared.CompareTo(b.DistanceSquared);
                if (cmp != 0) return cmp;
                cmp = a.I.CompareTo(b.I);
                if (cmp != 0) return cmp;
                return a.J.CompareTo(b.J);
            });

            int n = points.Length;

            // Lösung 1: nach 1000 kürzesten Verbindungen Produkt der drei größten Komponenten
            UnionFind uf1 = new UnionFind(n);
            int takeEdges = Math.Min(1000, edges.Length);
            for (int k = 0; k < takeEdges; k++)
            {
                Edge e = edges[k];
                uf1.Union(e.I, e.J);
            }

            int[] sizes1All = uf1.ComponentSizes();
            int posCount = 0;
            for (int i = 0; i < sizes1All.Length; i++)
            {
                if (sizes1All[i] > 0) posCount++;
            }

            int[] posSizes = new int[posCount];
            int p = 0;
            for (int i = 0; i < sizes1All.Length; i++)
            {
                if (sizes1All[i] > 0) posSizes[p++] = sizes1All[i];
            }

            Array.Sort(posSizes); // aufsteigend
            long product1 = 1;
            int take = Math.Min(3, posSizes.Length);
            for (int i = 0; i < take; i++)
            {
                int idx = posSizes.Length - 1 - i;
                checked { product1 = product1 * posSizes[idx]; }
            }

            // Lösung 2: fortlaufend verbinden bis alle in einer Komponente sind
            UnionFind uf2 = new UnionFind(n);
            long productOfLastConnectedX = 0;
            bool foundLast = false;
            for (int k = 0; k < edges.Length; k++)
            {
                Edge e = edges[k];
                if (uf2.Union(e.I, e.J))
                {
                    if (uf2.ComponentsCount == 1)
                    {
                        checked { productOfLastConnectedX = (long)points[e.I].X * points[e.J].X; }
                        foundLast = true;
                        break;
                    }
                }
            }

            Console.WriteLine(product1);
            if (foundLast)
                Console.WriteLine(productOfLastConnectedX);
            else
                Console.WriteLine("Keine letzte Verbindung gefunden, die alle Knoten vereint.");
        }

        static string[] ReadInputLines(string[] args)
        {
            if (args != null && args.Length > 0 && File.Exists(args[0]))
                return File.ReadAllLines(args[0]);

            if (File.Exists("input.txt"))
                return File.ReadAllLines("input.txt");

            string all = Console.In.ReadToEnd();
            if (string.IsNullOrEmpty(all))
                return new string[0];

            return all.Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
        }

        static Point ParsePoint(string line)
        {
            string[] parts = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 3) throw new FormatException("Ungültige Zeile: " + line);
            int x = int.Parse(parts[0].Trim());
            int y = int.Parse(parts[1].Trim());
            int z = int.Parse(parts[2].Trim());
            return new Point(x, y, z);
        }

        static Edge[] BuildAllEdges(Point[] points)
        {
            int n = points.Length;
            long mLong = (long)n * (n - 1) / 2;
            if (mLong > int.MaxValue) throw new OverflowException("Zu viele Kanten für diese Implementierung.");
            int m = (int)mLong;
            Edge[] edges = new Edge[m];
            int idx = 0;
            for (int i = 0; i < n; i++)
            {
                Point pi = points[i];
                for (int j = i + 1; j < n; j++)
                {
                    Point pj = points[j];
                    long dx = (long)pi.X - pj.X;
                    long dy = (long)pi.Y - pj.Y;
                    long dz = (long)pi.Z - pj.Z;
                    long distSq = dx * dx + dy * dy + dz * dz;
                    edges[idx++] = new Edge(i, j, distSq);
                }
            }
            return edges;
        }

        struct Point
        {
            public int X;
            public int Y;
            public int Z;
            public Point(int x, int y, int z) { X = x; Y = y; Z = z; }
        }

        struct Edge
        {
            public int I;
            public int J;
            public long DistanceSquared;
            public Edge(int i, int j, long d) { I = i; J = j; DistanceSquared = d; }
        }

        class UnionFind
        {
            private readonly int[] parent;
            private readonly int[] size;
            public int ComponentsCount { get; private set; }

            public UnionFind(int n)
            {
                parent = new int[n];
                size = new int[n];
                ComponentsCount = n;
                for (int i = 0; i < n; i++)
                {
                    parent[i] = i;
                    size[i] = 1;
                }
            }

            public int Find(int x)
            {
                if (parent[x] == x) return x;
                parent[x] = Find(parent[x]);
                return parent[x];
            }

            public bool Union(int a, int b)
            {
                int ra = Find(a);
                int rb = Find(b);
                if (ra == rb) return false;
                if (size[ra] < size[rb])
                {
                    int tmp = ra; ra = rb; rb = tmp;
                }
                parent[rb] = ra;
                checked { size[ra] += size[rb]; }
                size[rb] = 0;
                ComponentsCount--;
                return true;
            }

            public int[] ComponentSizes()
            {
                int[] copy = new int[size.Length];
                for (int i = 0; i < size.Length; i++) copy[i] = size[i];
                return copy;
            }
        }
    }
}