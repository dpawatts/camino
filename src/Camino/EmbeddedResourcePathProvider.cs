using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;

namespace Camino
{
	public class EmbeddedResourcePathProvider : VirtualPathProvider
	{
		private readonly Dictionary<string, EmbeddedResource> _resources;
		private readonly Dictionary<EmbeddedResource, string> _urls;
		private readonly List<string> _directories;

		public EmbeddedResourcePathProvider()
		{
			_resources = new Dictionary<string, EmbeddedResource>();
			_urls = new Dictionary<EmbeddedResource, string>();
			_directories = new List<string>();
		}
 
		public void AddAssembly(Assembly assembly, string prefix)
		{
			prefix = prefix.Trim('/');
			string assemblyName = assembly.GetName().Name;
			string prefixAsKey = GetResourceKey(prefix);
			foreach (var resourceName in assembly.GetManifestResourceNames().Where(n => n.StartsWith(assemblyName)))
			{
				var key = prefixAsKey + "." + resourceName.ToLower().Substring(assemblyName.Length + 1);
				var resource = _resources[key] = new EmbeddedResource(assembly, resourceName);

				var tempUrl = key.Replace('.', '/');
				var fileExtensionDot = tempUrl.LastIndexOf('/');
				var url = "/" + tempUrl.Substring(0, fileExtensionDot) + "." + tempUrl.Substring(fileExtensionDot + 1);
				_urls[resource] = url;

				var directory = VirtualPathUtility.GetDirectory(url).Trim('/');
				if (!_directories.Contains(directory))
					_directories.Add(directory);
			}
		}

		public string GetUrl(Assembly assembly, string resourceName)
		{
			var resource = _resources.Values.SingleOrDefault(r => r.Assembly == assembly && r.ResourceName == resourceName);
			if (resource == null)
				throw new InvalidOperationException("Could not find an embedded resource matching the specified assembly and resource name.");

			return _urls[resource];
		}

		private static string GetResourceKey(string virtualPath)
		{
			return virtualPath.TrimStart('~', '/').Replace('/', '.').ToLower();
		}

		private EmbeddedResource GetResource(string virtualPath)
		{
			var resourceKey = GetResourceKey(virtualPath);

			EmbeddedResource result;
			_resources.TryGetValue(resourceKey, out result);
			return result;
		}

		public override bool DirectoryExists(string virtualDir)
		{
			var formattedVirtualDir = virtualDir.TrimStart('~', '/').TrimEnd('/').ToLower();
			return base.DirectoryExists(virtualDir) || _directories.Contains(formattedVirtualDir);
		}

		public override bool FileExists(string virtualPath)
		{
			return base.FileExists(virtualPath) || GetResource(virtualPath) != null;
		}

		public override CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
		{
			var dependencies = new List<CacheDependency>();

			// Handle chained dependencies
			if (virtualPathDependencies != null)
				dependencies.AddRange(virtualPathDependencies
					.Cast<string>()
					.Where(s => s != virtualPath)
					.Select(d => GetCacheDependency(d, new[] { d }, utcStart))
					.Where(d => d != null));

			// Handle the primary file
			var resource = GetResource(virtualPath);
			CacheDependency primaryDependency = (resource != null)
				? resource.GetCacheDependency(utcStart)
				: base.GetCacheDependency(virtualPath, new[] { virtualPath }, utcStart);

			if (primaryDependency != null)
				dependencies.Add(primaryDependency);

			if (dependencies.Any())
			{
				if (dependencies.Count == 1)
					return dependencies[0];

				var result = new AggregateCacheDependency();
				result.Add(dependencies.ToArray());
				return result;
			}
			return null;
		}

		public override VirtualFile GetFile(string virtualPath)
		{
			if (base.FileExists(virtualPath))
				return base.GetFile(virtualPath);

			var resource = GetResource(virtualPath);
			if (resource != null)
				return new EmbeddedResourceVirtualFile(virtualPath, resource);

			return base.GetFile(virtualPath);
		}
	}
}