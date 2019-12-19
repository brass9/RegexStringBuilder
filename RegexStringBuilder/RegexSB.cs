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
	}
}
