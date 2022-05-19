# Riichi Mahjong Shanten Calculator
This is a calculator for the shanten of a hand in Riichi Mahjong. We utilize a recursive depth first search to deconstruct the hand and get the best result. Other approaches to this problem utilize a database of hands and their shanten to minimize calculation time down to O(1) though this does come with a runtime overhead of a dictionary with ~500k entries. Here I wanted to see how challenging the algorithmic approach would be. It was challenging indeed.

## Installation
Requires the dotnet6 runtime. Just pull and run.

The program comes with a small console ui that allows you to input a hand and have it checked.

## Shanten
A hand in Riichi Mahjong consists of 14 tiles. Shanten is the minimum number of tiles necessary to get to a "ready" (tenpai) hand. A shanten of 0 means the hand is tenpai. A shanten of 1 means one more tile is necessary to get to tenpai.

## Approach
We can calculate a simplified shanten easily in our head. 8 - (number of triplets or runs) * 2 - (number of partial sets or pairs). However we also may choose to go for a hand consisting of 7 pairs, or one consisting of 13 unique terminals or honors. Also, a regular hand consists of 5 groups of which one must be a pair. So if we have 6 groups, we may only count 5. Or if we have 5 groups without a single pair we can only count 4 groups (see [this wiki](https://riichi.wiki/Shanten#Counting_shanten) for more info). 

These extra conditions make an algorithmic approach nontrivial from a complexity perspective, which is why the data-driven precalculated approach is quite popular especially in online engines, see [this blogpost](http://blog.ezyang.com/2014/04/calculating-shanten-in-mahjong/) for more info.

So we end up splitting the hand into its 4 suits and tackle each individually. The honor tiles are trivial as they cannot form runs. On each suit we perform a recursive depth first search where we recurse over all possible sets, runs, partial groups and pairs (see `MeldFinder.cs`). The above specified conditions and final number are implemented in `Shanten.cs`. There are several more hidden implementation details such as prioritizing triplets and pairs over runs over partial sets because of group count restrictions and the requirement for a pair.

## Testing
[Tenhou](https://tenhou.net/2/) provides an easy webclient for double checking results. We can randomly generate a hand, manually run it through Tenhou and then compare. See `shanten_tests.txt` for test cases.