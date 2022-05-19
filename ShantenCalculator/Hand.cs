using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShantenCalculator
{
    public static class Hand
    {
        public static int[] Generate()
        {
            List<int> allTiles = new List<int>();
            for (int i = 0; i < Program.NUM_TILES; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    allTiles.Add(i);
                }
            }
            Random rand = new Random();
            int[] hand = new int[Program.NUM_TILES];
            for (int i = 0; i < 14; i++)
            {
                int roll = rand.Next(allTiles.Count);
                hand[allTiles[roll]]++;
                allTiles.RemoveAt(roll);
            }
            return hand;
        }

        public static int[] Parse(string line)
        {
            List<int> indices = new List<int>();
            int[] tiles = new int[Program.NUM_TILES];
            for (int i = 0; i < line.Length; i++)
            {
                int offset = 0;
                bool isNumber = false;
                switch (line[i])
                {
                    case 'm': break;
                    case 'p': offset = 9; break;
                    case 's': offset = 18; break;
                    case 'z': offset = 27; break;
                    default: isNumber = true; break;
                }

                if (isNumber)
                {
                    indices.Add(int.Parse("" + line[i]) - 1);
                }
                else
                {
                    indices.ForEach((i) => tiles[i + offset]++);
                    indices.Clear();
                }
            }
            return tiles;
        }

        public static void Print(int[] tiles)
        {
            Console.WriteLine(Encode(tiles));
        }

        public static string Encode(int[] tiles)
        {
            string code = "";
            string[] suffix = new string[] { "m", "p", "s", "z" };
            for (int i = 0; i < 4; i++)
            {
                bool hadTile = false;
                for (int j = 0; j < 9 && (i * 9 + j) < 3 * 9 + 7; j++)
                {
                    int num = tiles[i * 9 + j];
                    if (num != 0)
                    {
                        hadTile = true;
                        code += new string((char)(j + 49), num);
                    }
                }
                if (hadTile)
                {
                    code += suffix[i];
                }
            }
            return code;
        }

    }
}
