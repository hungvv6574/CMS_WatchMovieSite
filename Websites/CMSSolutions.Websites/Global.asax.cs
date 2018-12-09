using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CMSSolutions.Web;
using CMSSolutions.Web.UI;
using CMSSolutions.Web.UI.ControlForms;
using CMSSolutions.Websites.Controllers;
using CMSSolutions.Websites.Extensions;
using System.Diagnostics;

namespace CMSSolutions.Websites
{
    public class MvcApplication : HttpApplicationBase
    {
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            var statusCode = 500;
            if (exception.GetType() == typeof(HttpException))
            {
                statusCode = ((HttpException) exception).GetHttpCode(); 
            }

            Utilities.WriteEventLog(statusCode + ":" + exception.Message);
        }

        protected override void OnApplicationStart()
        {
            base.OnApplicationStart();
            ControlFormProvider.DefaultFormProvider = () => new BootstrapControlFormProvider();
            AuthConfig.RegisterAuth();
        }

        protected override void LookupResources(object sender, ResourcesLookupEventArgs e)
        {

        }

        protected override IEnumerable<Type> GetExportedTypes()
        {
            return typeof(MvcApplication).Assembly.GetExportedTypes();
        }

        protected override IEnumerable<string> GetDependencies()
        {
            yield return Constants.Areas.Dashboard;
            yield return Constants.Areas.Security;
            yield return Constants.Areas.Accounts;
        }
    }
}