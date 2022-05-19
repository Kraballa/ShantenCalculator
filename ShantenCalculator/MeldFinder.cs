using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShantenCalculator
{
    public static class MeldFinder
    {
        /// <summary>
        /// Disassemble a set of tiles of one suit (int[9]) into a list of Melds. 
        /// This list contains the best combo of sets, pairs and partial sets where sets are valued 2 points and the rest 1.
        /// </summary>
        public static List<SmallMeld> Find(int[] tiles) => FindBestMeldCombo(tiles, new List<SmallMeld>(), new List<SmallMeld>());

        /// <summary>
        /// Disassemble a set of honor tiles (int[7]) into a number of pairs and triplets.
        /// </summary>
        public static void CountZ(int[] zTiles, ref int pairs, ref int triplets)
        {
            for (int i = 0; i < zTiles.Length; i++)
            {
                switch (zTiles[i])
                {
                    case >= 3:
                        triplets++;
                        break;
                    case 2:
                        pairs++;
                        break;
                }
            }
        }

        /// <summary>
        /// From a given set of tiles (int[9]) list all possible combinations into sets, pSets and pairs.
        /// </summary>
        public static List<SmallMeld> FindAllGroups(int[] tiles)
        {
            List<SmallMeld> groups = new List<SmallMeld>();

            for (int i = 0; i < tiles.Length; i++)
            {
                if (tiles[i] >= 3) // 111
                {
                    int[] group = new int[tiles.Length];
                    group[i] = 3;
                    groups.Add(new SmallMeld(group, MeldType.Set));
                }
                //we have to prioritize pairs because of chiitoitsu.
                if (tiles[i] >= 2) // 11
                {
                    int[] group = new int[tiles.Length];
                    group[i] = 2;
                    groups.Add(new SmallMeld(group, MeldType.Pair));
                }
            }

            //we have to prioritize runs over partial sets because that way we can get better shanten with less groups
            for (int i = 0; i < 7; i++)
            {
                if (tiles[i] >= 1 && tiles[i + 1] >= 1 && tiles[i + 2] >= 1)
                {
                    int[] group = new int[tiles.Length];
                    group[i] = 1; group[i + 1] = 1; group[i + 2] = 1;
                    groups.Add(new SmallMeld(group, MeldType.Set));
                }
            }

            for (int i = 0; i < tiles.Length; i++)
            {
                if (i < 7) //13
                {
                    if (tiles[i] >= 1 && tiles[i + 2] >= 1)
                    {
                        int[] group = new int[tiles.Length];
                        group[i] = 1; group[i + 2] = 1;
                        groups.Add(new SmallMeld(group, MeldType.PSet));
                    }
                }
                if (i < 8) //12
                {
                    if (tiles[i] >= 1 && tiles[i + 1] >= 1)
                    {
                        int[] group = new int[tiles.Length];
                        group[i] = 1; group[i + 1] = 1;
                        groups.Add(new SmallMeld(group, MeldType.PSet));
                    }
                }
            }
            return groups;
        }

        /// <summary>
        /// Value a list of melds. A set is worth 2 points while a pair or pSet is worth 1 point.
        /// </summary>
        public static int ValueMelds(ICollection<SmallMeld> melds)
        {
            int value = 0;
            foreach (SmallMeld meld in melds)
            {
                value += (meld.MeldType == MeldType.Set) ? 2 : 1;
            }
            return value;
        }

        /// <summary>
        /// Find the best possible meld combo and increment a reference value accordingly.
        /// </summary>
        public static void Count(int[] tiles, ref int sets, ref int pSets, ref int pairs)
        {
            foreach (SmallMeld meld in Find(tiles))
            {
                switch (meld.MeldType)
                {
                    case MeldType.Set:
                        sets++;
                        break;
                    case MeldType.PSet:
                        pSets++;
                        break;
                    case MeldType.Pair:
                        pairs++;
                        break;
                }
            }
        }

        /// <summary>
        /// Recursively search best melds from a set of tiles (int[9]).<br/>
        /// We apply a recursive DFS algorithm to find the most value set of (partial) melds.
        /// </summary>
        private static List<SmallMeld> FindBestMeldCombo(int[] tiles, List<SmallMeld> bestMelds, List<SmallMeld> currentMelds)
        {
            List<SmallMeld> groups = FindAllGroups(tiles);

            if (groups.Count > 0) //check if we can recourse further
            {
                foreach (SmallMeld meld in groups)
                {
                    //apply hand
                    for (int i = 0; i < tiles.Length; i++)
                    {
                        tiles[i] -= meld.Tiles[i];
                    }
                    currentMelds.Add(meld);
                    FindBestMeldCombo(tiles, bestMelds, currentMelds);
                    currentMelds.RemoveAt(currentMelds.Count - 1);
                    //unapply hand
                    for (int i = 0; i < tiles.Length; i++)
                    {
                        tiles[i] += meld.Tiles[i];
                    }
                }
            }
            else //end of recursion
            {
                if (ValueMelds(currentMelds) > ValueMelds(bestMelds))
                {
                    bestMelds.Clear();
                    bestMelds.AddRange(currentMelds);
                }
            }
            return bestMelds;
        }

        public static string Stringify(List<SmallMeld> melds)
        {
            string str = "";
            int sets = 0;
            int pSets = 0;
            int pairs = 0;
            foreach (SmallMeld meld in melds)
            {
                switch (meld.MeldType)
                {
                    case MeldType.Set:
                        sets++;
                        break;
                    case MeldType.PSet:
                        pSets++;
                        break;
                    case MeldType.Pair:
                        pairs++;
                        break;
                }
            }
            str += string.Format($"{sets} sets, {pSets} pSets, {pairs} pairs: {ValueMelds(melds)}");
            return str;
        }

        public enum MeldType
        {
            Set,
            PSet,
            Pair
        }

        /// <summary>
        /// SmallMeld represents a meld from a set of 9 tiles. Used individually for man, pin and sou.
        /// </summary>
        public struct SmallMeld
        {
            public int[] Tiles;
            public MeldType MeldType;

            public SmallMeld(int[] tiles, MeldType meld)
            {
                Tiles = tiles;
                MeldType = meld;
            }
        }
    }
}
