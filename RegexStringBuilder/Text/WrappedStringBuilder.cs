using System;
using System.Collections.Generic;
using System.Text;

namespace RegexStringBuilder.Text
{
	/// <summary>
	/// A StringBuilder-based class that offers many operations normally offered by Strings, but in a way that
	/// minimizes use of strings wherever possible, especially interacting with other StringBuilders,
	/// char[] or char Spans.
	/// </summary>
	public class WrappedStringBuilder : IString
	{
		public StringBuilder Sb { get; protected set; }

		public WrappedStringBuilder()
		{
			Sb = new StringBuilder();
		}
		public WrappedStringBuilder(StringBuilder sb)
		{
			Sb = sb;
		}
		public WrappedStringBuilder(string s)
		{
			Sb = new StringBuilder(s);
		}


		public char this[int i]
		{
			get { return Sb[i]; }
			set { Sb[i] = value; }
		}

		public int Length => Sb.Length;

		public char[] GetChars(int startIndex, int length)
		{
			var chars = new char[length];
			Sb.CopyTo(startIndex, chars, length);
			return chars;
		}
		public string Substring(int startIndex, int length)
		{
			return new string(GetChars(startIndex, length));
		}
		public string Substring(int startIndex)
		{
			return Substring(startIndex, Sb.Length - startIndex);
		}

		public void AppendTo(StringBuilder dest, int startIndex, int length)
		{
			dest.Append(GetChars(startIndex, length));
		}


		public override string ToString()
		{
			return Sb.ToString();
		}

		public static implicit operator WrappedStringBuilder(StringBuilder sb)
		{
			return new WrappedStringBuilder(sb);
		}
	}
}
