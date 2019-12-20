using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace RegexStringBuilder.Text
{
	public class WrappedCharArray : IString
	{
		protected char[] chars;

		public WrappedCharArray(char[] chars)
		{
			this.chars = chars;
		}


		public char this[int i] => chars[i];

		public int Length => chars.Length;

		public char[] GetChars(int startIndex, int length)
		{
			if (startIndex == 0 && length == chars.Length)
				return chars;	// Return internal representation if asked for entire contents

			var ch = chars.Skip(startIndex).Take(length).ToArray();
			return ch;
		}
		public char[] GetChars(int startIndex)
		{
			return GetChars(startIndex, chars.Length - startIndex);
		}
		public string Substring(int startIndex) => new string(GetChars(startIndex));
		public string Substring(int startIndex, int length) => new string(GetChars(startIndex, length));

		public void AppendTo(StringBuilder sb, int startIndex, int length)
		{
			sb.Append(chars, startIndex, length);
		}


		public override string ToString()
		{
			return new string(chars);
		}

		public static implicit operator WrappedCharArray(string s)
		{
			return new WrappedCharArray(s.ToCharArray());
		}
	}
}
