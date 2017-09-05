using System.Web.Mvc;
using System.Web.Routing;

namespace VideoUpload.Web
{
  public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            UnityConfig.RegisterComponents();
            RouteConfig.RegisterRoutes(RouteTable.Routes);            
        }
    }
}
