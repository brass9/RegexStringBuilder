using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace RegexStringBuilder.Test
{
	[TestClass]
	public class RegexSBTest
	{
		[TestMethod]
		public void TestSimpleReplace()
		{
			string s = "The rain in spain falls mostly on the plains";
			var regex = new RegexSB("ain");
			var result = regex.Replace(s, "oof");
			Assert.AreEqual("The roof in spoof falls mostly on the ploofs", result.ToString());
		}

		[TestMethod]
		public void TestSimpleReplacePerf()
		{
			string s = "The rain in spain falls mostly on the plains";
			string lookFor = "ain";
			string replace = "oof";
			int count = 10000;

			var stopwatch = new Stopwatch();
			stopwatch.Start();
			var classicRegex = new Regex(lookFor);
			for (int i = 0; i < count; i++)
				classicRegex.Replace(s, replace);
			stopwatch.Stop();
			var classicTime = stopwatch.Elapsed;

			stopwatch.Reset();
			stopwatch.Start();
			var regexSB = new RegexSB(lookFor);
			for (int i = 0; i < count; i++)
				regexSB.Replace(s, replace);
			stopwatch.Stop();
			var sbTime = stopwatch.Elapsed;
		}
	}
}
