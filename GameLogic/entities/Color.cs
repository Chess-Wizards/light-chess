using System;

namespace GameLogic
{
    // The enumeration contains possible colors.
    public enum Color
    {
        White,
        Black
    }

    // The class contains an extension method for Color enum.
    static class ColorExtension
    {
        public static Color Change(this Color color)
        {
            return color == Color.White ? Color.Black : Color.White;
        }
    }
}
