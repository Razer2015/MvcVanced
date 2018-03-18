using System.Web;
using System.Web.Mvc;

namespace MvcVanced
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
            filters.Add(new ErrorHandler.AiHandleErrorAttribute());
        }
    }
}
