using System;
using System.IO;
using System.Text.RegularExpressions;

namespace day10
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (!File.Exists("input.txt"))
            {
                return;
            }

            string input = File.ReadAllText("input.txt");
            string[] lines = input.Split('\n');
            long totalMinPressesPart1 = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                Machine machine = ParseMachine(line);
                int minPresses = SolveMachinePart1(machine);
                totalMinPressesPart1 += minPresses;
            }

            Console.WriteLine("Part 1: " + totalMinPressesPart1);
        }

        static Machine ParseMachine(string line)
        {
            Machine machine = new Machine();

            Match lightsMatch = Regex.Match(line, @"\[([.#]+)\]");
            string lightsStr = lightsMatch.Groups[1].Value;
            machine.NumLights = lightsStr.Length;
            machine.TargetLights = new bool[machine.NumLights];
            for (int i = 0; i < lightsStr.Length; i++)
            {
                machine.TargetLights[i] = lightsStr[i] == '#';
            }

            MatchCollection buttonsMatches = Regex.Matches(line, @"\(([0-9,]+)\)");
            machine.NumButtons = buttonsMatches.Count;
            machine.Buttons = new int[machine.NumButtons][];

            for (int i = 0; i < buttonsMatches.Count; i++)
            {
                string[] parts = buttonsMatches[i].Groups[1].Value.Split(',');
                machine.Buttons[i] = new int[parts.Length];
                for (int j = 0; j < parts.Length; j++)
                {
                    machine.Buttons[i][j] = int.Parse(parts[j]);
                }
            }

            return machine;
        }

        static int SolveMachinePart1(Machine machine)
        {
            int minPresses = int.MaxValue;
            if (machine.NumButtons >= 31) return 0;
            int maxMask = 1 << machine.NumButtons;

            for (int mask = 0; mask < maxMask; mask++)
            {
                bool[] lights = new bool[machine.NumLights];
                int presses = 0;

                for (int i = 0; i < machine.NumButtons; i++)
                {
                    if ((mask & (1 << i)) != 0)
                    {
                        presses++;
                        for (int j = 0; j < machine.Buttons[i].Length; j++)
                        {
                            int lightIndex = machine.Buttons[i][j];
                            if (lightIndex >= 0 && lightIndex < machine.NumLights)
                            {
                                lights[lightIndex] = !lights[lightIndex];
                            }
                        }
                    }
                }

                bool matches = true;
                for (int i = 0; i < machine.NumLights; i++)
                {
                    if (lights[i] != machine.TargetLights[i])
                    {
                        matches = false;
                        break;
                    }
                }

                if (matches && presses < minPresses)
                {
                    minPresses = presses;
                }
            }

            if (minPresses == int.MaxValue) return 0;
            return minPresses;
        }
    }

    class Machine
    {
        public int NumLights;
        public bool[] TargetLights;
        public int NumButtons;
        public int[][] Buttons;
        public int[] JoltageRequirements;
    }
}