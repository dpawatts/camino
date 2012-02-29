using System;
using System.Web;
using System.Web.Hosting;

namespace Camino
{
	public class DefaultDocumentModule : IHttpModule
	{
		private static readonly string[] DefaultDocuments = new[] { "default.aspx" };

		public void Init(HttpApplication context)
		{
			context.BeginRequest += OnBeginRequest;
		}

		private static void OnBeginRequest(object sender, EventArgs e)
		{
			var application = (HttpApplication) sender;

			// Emulate IIS directory redirection and default document handling.
			string path = application.Request.Url.AbsolutePath;

			if (HandleDefaultDocument(application, path)) 
				return;

			// Request URL does not end with a slash, check if we have a corresponding directory
			MakeCanonical(application, path);
		}

		private static bool HandleDefaultDocument(HttpApplication application, string path)
		{
			// If we're referring to a directory but no document, try to find an existing default document.
			if (path.EndsWith("/"))
			{
				// Get an array of possible choices for default documents.
				foreach (var defaultDocument in DefaultDocuments)
				{
					var defaultUrlBuilder = new UriBuilder(application.Request.Url);
					defaultUrlBuilder.Path += defaultDocument;
					var defaultUrl = defaultUrlBuilder.Uri;

					// If this path actually exists...
					if (HostingEnvironment.VirtualPathProvider.FileExists(defaultUrl.AbsolutePath))
					{
						// ....rewrite the path to refer to this default document, and serve it up
						HttpContext.Current.RewritePath(defaultUrl.PathAndQuery);
						break;
					}
				}
				return true;
			}
			return false;
		}

		private static void MakeCanonical(HttpApplication application, string path)
		{
			if (HostingEnvironment.VirtualPathProvider.DirectoryExists(path))
			{
				// If we're referring to an existing directory, add a "/" and redirect
				// NOTE! Since we are changing the nesting level of the request, we must do a redirect to ensure
				// that the browser requesting the information is in sync with regards to the base URL.
				application.Response.Redirect(path + "/");
			}
		}

		public void Dispose()
		{
			
		}
	}
}