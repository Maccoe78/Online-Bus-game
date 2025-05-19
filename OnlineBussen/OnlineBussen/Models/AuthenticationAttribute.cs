using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace OnlineBussen.Models
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthenticationAttribute : Attribute, IPageFilter
    {
        public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            var username = context.HttpContext.Session.GetString("Username");

            if (string.IsNullOrEmpty(username))
            {
                context.Result = new RedirectToPageResult("/Index");
            }
        }
        public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {
        }

        public void OnPageHandlerSelected(PageHandlerSelectedContext context)
        {
        }
    }
}
