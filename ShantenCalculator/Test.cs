using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ShantenCalculator.MeldFinder;

namespace ShantenCalculator
{
    public static class Test
    {
        public static void ShantenTests()
        {
            int numTests = 0;
            int numFailed = 0;
            List<ShantenTest> tests = new List<ShantenTest>();
            string[] lines = File.ReadAllLines("shanten_tests.txt");
            foreach (string line in lines)
            {
                if (line.Trim() == "" || line.StartsWith("//"))
                {
                    continue;
                }
                string[] split = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                tests.Add(new ShantenTest(split[0], int.Parse(split[1])));
            }

            foreach (ShantenTest testCase in tests)
            {
                int[] hand = Hand.Parse(testCase.Hand);
                numTests++;
                if (Shanten.Calculate(hand) != testCase.Shanten)
                {
                    numFailed++;
                    Console.WriteLine($"failed test {testCase}");
                }
            }
            Report(numTests, numFailed);
        }

        private static void Report(int numTests, int numFailed)
        {
            Console.Write($"ran tests: ");
            if (numFailed != 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            Console.WriteLine($"({numTests - numFailed}/{numTests})");
            Console.ForegroundColor = ConsoleColor.Gray;
        }


        private struct ShantenTest
        {
            public string Hand { get; set; }
            public int Shanten { get; set; }
            public ShantenTest(string hand, int shanten)
            {
                Hand = hand;
                Shanten = shanten;
            }

            public override string ToString()
            {
                return Hand + ' ' + Shanten;
            }
        }
    }
}
