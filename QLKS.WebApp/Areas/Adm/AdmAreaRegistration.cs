using System.Web.Mvc;

namespace QLKS.WebApp.Areas.Adm
{
    public class AdmAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Adm";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                name: "Adm_default",
                url: "Adm/{controller}/{action}/{id}",
                defaults: new
                {
                    controller = "Dashboard",
                    action = "Index",
                    id = UrlParameter.Optional,
                },
                namespaces: new[] { "QLKS.WebApp.Areas.Adm.Controllers" }
            );
        }
    }
}