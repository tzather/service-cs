using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tzather.Tools;

namespace Tzather.Tools.UnitTests;

[TestClass]
public class StringExtensionTest
{
  [TestMethod]
  public void ToCamelCase() => Assert.AreEqual("helloWorld", "HelloWorld".ToCamelCase());

  [TestMethod]
  public void ToUpperCaseFirst() => Assert.AreEqual("HelloWorld", "helloWorld".ToUpperCaseFirst());

}
