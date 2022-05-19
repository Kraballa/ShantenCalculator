using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShantenCalculator
{
    /// <summary>
    /// Various methods surrounding the analysis of a set of tiles.
    /// </summary>
    internal class Analysis
    {
        /// <summary>
        /// Count the number of unique pairs.
        /// </summary>
        public static int CountPairs(int[] tiles)
        {
            int numPairs = 0;
            for (int i = 0; i < Program.NUM_TILES; i++)
            {
                if (tiles[i] >= 2)
                {
                    numPairs++;
                }
            }
            return numPairs;
        }

        public static int CountTerminalsHonors(int[] tiles, out int pairs)
        {
            int num = 0;
            pairs = 0;
            for (int i = 0; i < Program.NUM_TILES; i++)
            {
                if (IsTerminalOrHonor(i))
                {
                    if (tiles[i] >= 1)
                        num++;
                    if (tiles[i] > 1)
                        pairs++;
                }
            }
            return num;
        }

        /// <summary>
        /// Scan a list of tiles from a single suit. we need to count pairs because a full hand needs a pair
        /// </summary>
        public static void Scan(int[] tiles, out int pSet, out int set, out int pairs)
        {
            pSet = 0;
            set = 0;
            pairs = 0;
            //split into sections so it's easier to disassemble
            Split(tiles, out int[] man, out int[] pin, out int[] sou, out int[] z);
            MeldFinder.CountZ(z, ref pairs, ref set);
            MeldFinder.Count(man, ref set, ref pSet, ref pairs);
            MeldFinder.Count(pin, ref set, ref pSet, ref pairs);
            MeldFinder.Count(sou, ref set, ref pSet, ref pairs);
        }

        public static void Split(int[] tiles, out int[] man, out int[] pin, out int[] sou, out int[] z)
        {
            man = new int[9];
            pin = new int[9];
            sou = new int[9];
            z = new int[7];
            for (int i = 0; i < 9; i++)
            {
                man[i] = tiles[i];
                pin[i] = tiles[i + 9];
                sou[i] = tiles[i + 18];
                if (i < 7)
                {
                    z[i] = tiles[i + 27];
                }
            }
        }

        public static bool IsTerminalOrHonor(int tile)
        {
            return tile % 9 == 8 || tile % 9 == 0 || tile >= 27;
        }
    }
}
