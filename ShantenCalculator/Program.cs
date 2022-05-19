
namespace ShantenCalculator
{
    public class Program
    {
        public const int NUM_TILES = 3 * 9 + 7; // 3 suits times 9 tiles each plus 4 winds plus 3 dragons

        [STAThread]
        public static void Main(string[] args)
        {
            Test.ShantenTests();
            ShantenChecker();
        }

        private static void ShantenChecker()
        {
            Console.WriteLine("shanten checker v1.0\ntype hand or 'random' to check");
            while (true)
            {
                string line = Console.ReadLine();

                int[] hand;
                if (line == "random")
                {
                    hand = Hand.Generate();
                }
                else
                {
                    try
                    {
                        hand = Hand.Parse(line.Trim());
                    }
                    catch
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("error, invalid hand");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        continue;
                    }
                }

                Hand.Print(hand);
                Console.WriteLine($"shanten: {Shanten.Calculate(hand)}. validation url: https://tenhou.net/2/?q={Hand.Encode(hand)}");
            }
        }


    }
}