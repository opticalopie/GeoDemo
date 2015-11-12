using GeocachingExample.Controllers;
using GeocachingExample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace GeocachingExample.Filters
{
    /// <summary>
    /// Handle any uncought exceptions.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class HandleUncheckedExceptionAttribute : ExceptionFilterAttribute
    {
        public HandleUncheckedExceptionAttribute() : base() { }

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception is GeocacheException)
            {
                GeocacheException except = actionExecutedContext.Exception as GeocacheException;
                actionExecutedContext.Response = actionExecutedContext.Request
                    .CreateErrorResponse(except.ResopnseCode, except.Message);
            }
            else
            {
                actionExecutedContext.Response = actionExecutedContext.Request
                    .CreateErrorResponse(HttpStatusCode.ExpectationFailed, "Error on server");
            }
        }
    }
}