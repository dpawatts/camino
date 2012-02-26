using System.IO;
using System.Web.Hosting;

namespace Camino
{
	public class EmbeddedResourceVirtualFile : VirtualFile
	{
		private readonly EmbeddedResource _resource;

		public EmbeddedResourceVirtualFile(string virtualPath, EmbeddedResource resource)
			: base(virtualPath)
		{
			_resource = resource;
		}

		public override Stream Open()
		{
			return _resource.GetStream();
		}
	}
}