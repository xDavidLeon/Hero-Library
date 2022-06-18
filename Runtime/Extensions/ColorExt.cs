using UnityEngine;
using Random = UnityEngine.Random;

namespace HeroLib
{
	public static partial class ColorExt
	{
		public static Color WithAlpha(this Color color, float alpha)
		{
			return new Color(color.r, color.g, color.b, alpha);
		}
		
		public static Color RandomColor()
		{
			return new Color(Random.value, Random.value, Random.value);
		}
		
		// Get Hex Color FF00FF
        public static string GetStringFromColor(Color color)
        {
            string red = MathExt.Dec01_to_Hex(color.r);
            string green = MathExt.Dec01_to_Hex(color.g);
            string blue = MathExt.Dec01_to_Hex(color.b);
            return red + green + blue;
        }

        // Get Hex Color FF00FFAA
        public static string GetStringFromColorWithAlpha(Color color)
        {
            string alpha = MathExt.Dec01_to_Hex(color.a);
            return GetStringFromColor(color) + alpha;
        }

        // Sets out values to Hex String 'FF'
        public static void GetStringFromColor(Color color, out string red, out string green, out string blue,
            out string alpha)
        {
            red = MathExt.Dec01_to_Hex(color.r);
            green = MathExt.Dec01_to_Hex(color.g);
            blue = MathExt.Dec01_to_Hex(color.b);
            alpha = MathExt.Dec01_to_Hex(color.a);
        }

        // Get Hex Color FF00FF
        public static string GetStringFromColor(float r, float g, float b)
        {
            string red = MathExt.Dec01_to_Hex(r);
            string green = MathExt.Dec01_to_Hex(g);
            string blue = MathExt.Dec01_to_Hex(b);
            return red + green + blue;
        }

        // Get Hex Color FF00FFAA
        public static string GetStringFromColor(float r, float g, float b, float a)
        {
            string alpha = MathExt.Dec01_to_Hex(a);
            return GetStringFromColor(r, g, b) + alpha;
        }

        // Get Color from Hex string FF00FFAA
        public static Color GetColorFromString(string color)
        {
            float red = MathExt.Hex_to_Dec01(color.Substring(0, 2));
            float green = MathExt.Hex_to_Dec01(color.Substring(2, 2));
            float blue = MathExt.Hex_to_Dec01(color.Substring(4, 2));
            float alpha = 1f;
            if (color.Length >= 8)
            {
                // Color string contains alpha
                alpha = MathExt.Hex_to_Dec01(color.Substring(6, 2));
            }

            return new Color(red, green, blue, alpha);
        }
	}
}
