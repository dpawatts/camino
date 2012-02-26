using System;
using System.IO;
using System.Reflection;
using System.Web.Caching;

namespace Camino
{
	public class EmbeddedResource
	{
		private readonly Assembly _assembly;
		private readonly string _resourceName;

		public EmbeddedResource(Assembly assembly, string resourceName)
		{
			_assembly = assembly;
			_resourceName = resourceName;
		}

		public CacheDependency GetCacheDependency(DateTime utcStart)
		{
			return new CacheDependency(_assembly.Location, utcStart);
		}

		public Stream GetStream()
		{
			return _assembly.GetManifestResourceStream(_resourceName);
		}
	}
}