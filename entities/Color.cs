using System;

namespace LightChess
{

    public enum Color
    {
        /*
            The enumeration contains possible colors.
        */
        
        White,
        Black
    }

    static class ColorExtension
    {

        public static Color Change(this Color color)
        {
            return color == Color.White ? Color.Black : Color.White;
        }
    }
}
