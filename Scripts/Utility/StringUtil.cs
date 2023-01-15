namespace ZnZUtil
{
	public static class StringUtil
	{
		/// <summary>
		/// Wraps the given string inside of start and end string
		/// </summary>
		/// <param name="word">string to wrap</param>
		/// <param name="start">start tag</param>
		/// <param name="end">end tag</param>
		/// <returns>wrapped string</returns>
		public static string Wrap(this string word, string start, string end)
		{
			return string.Concat(start, word, end);
		}

		/// <summary>
		/// Trims the string free of given chars and wraps it in start and end string
		/// </summary>
		/// <param name="word">string to wrap</param>
		/// <param name="trim">chars to trim</param>
		/// <param name="start">start tag</param>
		/// <param name="end">end tag</param>
		/// <returns>rewrapped string</returns>
		public static string TrimWrap(this string word, char[] trim, string start, string end)
		{
			return string.Concat(start, word.Trim(trim), end);
		}
	}
}
