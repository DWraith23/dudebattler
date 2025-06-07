using System;

namespace dudebattler.Scripts;

/// <summary>
/// This class contains utility methods and tools that can be used throughout the game.
/// It serves as a collection of static methods that can be called without instantiating an object.
/// </summary>
public static class Tools
{
    /// <summary>
    /// A static instance of the Random class for generating random numbers.
    /// This can be used throughout the game to ensure consistent random number generation.
    /// </summary>
    public static Random Rand => new();
}