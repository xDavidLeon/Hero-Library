namespace HeroLib
{
	public static partial class ByteExt
	{
		// PUBLIC METHODS

		/// <summary>
		/// returns an integer with the bit specified set to 1
		/// example: Bit(1) returns an int with all zeros but the first bit
		/// </summary>
		/// <returns></returns>
		public static int BIT(int b)
		{
			return 1 << b;
		}
		
		public static bool IsBitSet(this byte flags, int bit)
		{
			return (flags & (1 << bit)) == (1 << bit);
		}

		public static byte SetBit(ref this byte flags, int bit, bool value)
		{
			if (value == true)
			{
				return flags |= (byte)(1 << bit);
			}
			else
			{
				return flags &= unchecked((byte)~(1 << bit));
			}
		}
	}
}
