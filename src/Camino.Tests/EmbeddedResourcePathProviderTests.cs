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
			bool result = pathProvider.FileExists("/test/resources/page.aspx");

			// Assert.
			Assert.That(result, Is.True);
		}
	}
}