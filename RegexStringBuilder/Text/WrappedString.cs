using System;
using System.Collections.Generic;
using System.Text;

namespace RegexStringBuilder.Text
{
	public class WrappedString : IString
	{
		protected string internalString;

		public WrappedString(string s)
		{
			internalString = s;
		}


		public char this[int i] => internalString[i];

		public int Length => internalString.Length;

		public char[] GetChars(int startIndex, int length)
		{
			var chars = new char[length];
			internalString.CopyTo(startIndex, chars, 0, length);
			return chars;
		}
		public string Substring(int startIndex) => internalString.Substring(startIndex);
		public string Substring(int startIndex, int length) => internalString.Substring(startIndex, length);

		public void AppendTo(StringBuilder sb, int startIndex, int length)
		{
			sb.Append(internalString, startIndex, length);
		}


		public override string ToString()
		{
			return internalString;
		}

		public static implicit operator WrappedString(string s)
		{
			return new WrappedString(s);
		}
	}
}
