using Microsoft.VisualStudio.TestTools.UnitTesting;

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
			Assert.AreEqual("The roof in spoof falls mostly on the ploofs", result);
		}
	}
}
