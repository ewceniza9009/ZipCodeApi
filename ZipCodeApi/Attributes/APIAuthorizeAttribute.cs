using System;
using System.Web;
using System.Web.Mvc;

namespace ZipCodeApi.Attributes
{
    public class APIAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (Authorize(filterContext))
            {
                return;
            }

            HandleUnauthorizedRequest(filterContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);
        }

        private bool Authorize(AuthorizationContext actionContext)
        {
            try
            {
                HttpRequestBase request = actionContext.RequestContext.HttpContext.Request;

                string[] urlPart = request.Url.ToString().Split('/');
                string token = urlPart[urlPart.Length - 1];

                return Authorization.SecurityManager.IsTokenValid(token);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}