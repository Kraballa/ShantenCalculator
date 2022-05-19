using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShantenCalculator
{
    /// <summary>
    /// Static class that provides the ability to calculate the shanten of a set of tiles.<br/>
    /// 
    /// Shanten is a number that is supposed to represent the difference to Tenpai.
    /// A Shanten of 0 means Tenpai. A shanten of -1 (probably) means Ron.
    /// Anything above 0 means the hand needs some unspecified Tile to get Shanten - 1.
    /// </summary>
    public static class Shanten
    {
        #region API

        /// <summary>
        /// We can determine shanten algorithmically by calculating 3 possible tenpai configurations and taking the minimum.
        /// </summary>
        public static int Calculate(int[] tiles)
        {
            //13 - #term/honor - if(pair;1;0)
            int kokushi = CalculateKokushi(tiles);
            //6 - #pairs
            int sevenPairs = CalculateChiitoitsu(tiles);
            //8 - 2 * #blocks - #uncompleted block (special cases notwithstanding)
            int normalShanten = CalculateOtherHands(tiles);

            return Math.Min(Math.Min(kokushi, sevenPairs), normalShanten);
        }

        /// <summary>
        /// Calculate tenpai when only looking for Kokushi (13 orphans).
        /// </summary>
        public static int CalculateKokushi(int[] tiles)
        {
            return 13 - Analysis.CountTerminalsHonors(tiles, out int pairs) - Math.Min(pairs, 1);
        }

        /// <summary>
        /// Calculate tenpai when only looking for Chiitoitsu (7 pairs).
        /// </summary>
        public static int CalculateChiitoitsu(int[] tiles)
        {
            return 6 - Analysis.CountPairs(tiles);
        }

        /// <summary>
        /// Calculate tenpai when only looking at 4 groups + 1 pair (all other yaku).
        /// </summary>
        public static int CalculateOtherHands(int[] tiles)
        {
            Analysis.Scan(tiles, out int pSet, out int set, out int pairs);

            //normal shanten calculation.
            int shanten = 8 - (pSet + pairs) - 2 * set;

            //special case: 5 blocks, no pairs
            if (pSet + set == 5 && pairs == 0)
            {
                shanten++;
            }

            //special case 2: 6 blocks. there cannot be 6 blocks in a completed hand so count it as 5 blocks
            if (pSet + set + pairs >= 6)
            {
                //5 blocks. -1 for every set. e.g. we have 2 sets and 4 partial sets. we can only count 5-2=3 partial sets
                while (set + pSet + pairs >= 6)
                {
                    if (pSet > 0)
                        pSet--;
                    else if (pairs > 0)
                        pairs--;
                }
                int normPSet = Math.Min(5 - set, pSet);
                shanten = 8 - 2 * set - (normPSet + pairs);
            }
            return shanten;
        }

        #endregion
    }
}
