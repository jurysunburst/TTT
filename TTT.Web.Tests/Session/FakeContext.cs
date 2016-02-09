using System.Collections.Generic;
using System.IO;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.SessionState;

namespace TTT.Web.Tests.Session
{
	public static class FakeContext
	{
		public static HttpContext FakeHttpContext(Dictionary<string, object> sessionVariables, string path)
		{
			var httpRequest = new HttpRequest(string.Empty, path, string.Empty);
			var stringWriter = new StringWriter();
			var httpResponce = new HttpResponse(stringWriter);
			var httpContext = new HttpContext(httpRequest, httpResponce);
			httpContext.User = new GenericPrincipal(new GenericIdentity("username"), new string[0]);
			Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("username"), new string[0]);
			var sessionContainer = new HttpSessionStateContainer(
			  "id",
			  new SessionStateItemCollection(),
			  new HttpStaticObjectsCollection(),
			  10,
			  true,
			  HttpCookieMode.AutoDetect,
			  SessionStateMode.InProc,
			  false);

			foreach (var var in sessionVariables)
			{
				sessionContainer.Add(var.Key, var.Value);
			}

			SessionStateUtility.AddHttpSessionStateToContext(httpContext, sessionContainer);
			return httpContext;
		}
	}
}
