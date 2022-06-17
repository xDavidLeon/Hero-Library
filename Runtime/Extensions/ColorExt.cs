using UnityEngine;

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
	}
}
