using System;
using System.Collections.Generic;
using System.Text;

namespace RegexStringBuilder.Text
{
	public interface IString
	{
		char this[int i] { get; }
		int Length { get; }
		char[] GetChars(int startIndex, int length);
		string Substring(int startIndex);
		string Substring(int startIndex, int length);

		void AppendTo(StringBuilder dest, int startIndex, int length);
	}
}
