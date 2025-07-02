using System.Web.Mvc;

namespace UserManagementSystem.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Users", action = "Dashboard", id = UrlParameter.Optional }
            );
        }
    }
} 