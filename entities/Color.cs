using System;

namespace LightChess
{
    public enum Color
    {
        // The enumeration contains possible colors.
        
        White,
        Black
    }

    static class ColorExtension
    {
        // The class contains an extension method for Color enum.

        public static Color Change(this Color color)
        {
            return color == Color.White ? Color.Black : Color.White;
        }
    }
}
