using NUnit.Framework;

namespace Camino.Tests
{
	[TestFixture]
	public class EmbeddedResourcePathProviderTests
	{
		[Test]
		public void FileExistsReturnsTrueWhenEmbeddedResourceExists()
		{
			// Arrange.
			var pathProvider = new EmbeddedResourcePathProvider();
			pathProvider.AddAssembly(GetType().Assembly, "test");

			// Act.
			var result = pathProvider.FileExists("/test/resources/page.aspx");

			// Assert.
			Assert.That(result, Is.True);
		}

		[Test]
		public void CanGetUrl()
		{
			// Arrange.
			var pathProvider = new EmbeddedResourcePathProvider();
			pathProvider.AddAssembly(GetType().Assembly, "test");

			// Act.
			var result = pathProvider.GetUrl(GetType().Assembly, "Camino.Tests.Resources.Page.aspx");

			// Assert.
			Assert.That(result, Is.EqualTo("/test/resources/page.aspx"));
		}
	}
}