using RegexStringBuilder.Text;
using System;
using System.Collections.Generic;
using System.Text;

namespace RegexStringBuilder
{
	public class RegexSB : Mono.Regex
	{
		public RegexSB() : base()
		{ }

		public RegexSB(string input) : base(input) 
		{ }

		public RegexSB(string input, RegexStringBuilder.Mono.RegexOptions options) : base(input, options)
		{ }



		#region Replace convenience methods
		public IString Replace(string input, string replacement)
		{
			return Replace(new WrappedString(input), replacement);
		}
		public IString Replace(StringBuilder input, string replacement)
		{
			return Replace(new WrappedStringBuilder(input), replacement);
		}
		#endregion
	}
}
