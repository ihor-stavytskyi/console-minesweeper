/* 
Using Python I would generate K black holes with uniform distribution this way:

import random

holeLocations = random.sample(range(0, boardSize * boardSize), holesCount)

But in .NET I don't have such functionality out of the box, so I have to implement it on my own.
*/

public static class RandomNumberGenerator
{
    /* The idea is to generate a range from 0 to the maxValue (which is the total number of cells on the board), 
    then order this sequence by a random number, and then take the first K elements.
    It can guarantee that the numbers are unique.
    */
    public static int[] Sample(int maxValue, int numbersCount)
    {
        var random = new Random();
        return Enumerable.Range(0, maxValue)
            .OrderBy(_ => random.Next()) 
            .Take(numbersCount)
            .ToArray();
    }
}