using TaskTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TaskTracker.UI.Areas.Setup.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is JsonResult jsonResult && jsonResult.Value is ResultViewModel result)
            {
                if (result.Status == "Fail" && result.Message.Contains("permission", StringComparison.OrdinalIgnoreCase))
                {
                    bool isAjax = context.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
                    if (isAjax)
                    {
                        context.Result = new JsonResult(new
                        {
                            redirect = Url.Action("AccessDenied", "Account", new { area = "" })
                        });
                    }
                    else
                    {
                        context.Result = RedirectToAction("AccessDenied", "Account", new { area = "" });
                    }
                    return;
                }
            }

            base.OnActionExecuted(context);
        }

    }
}




















//using TaskTracker.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;

//namespace TaskTracker.UI.Areas.Setup.Controllers
//{
//    public class BaseController : Controller
//    {
//        public override void OnActionExecuted(ActionExecutedContext context)
//        {
//            // Handle API responses returning JsonResult
//            if (context.Result is JsonResult jsonResult)
//            {
//                if (jsonResult.Value is ResultViewModel result)
//                {
//                    if (result.Status == "Fail" && result.Message.Contains("permission", StringComparison.OrdinalIgnoreCase))
//                    {
//                        context.Result = RedirectToAction("Upsert", "Category");
//                        return; 
//                    }
//                }
//            }

//            // Handle API responses returning ObjectResult (like Ok(result))
//            else if (context.Result is ObjectResult objectResult)
//            {
//                if (objectResult.Value is ResultViewModel result)
//                {
//                    if (result.Status == "Fail" && result.Message.Contains("permission", StringComparison.OrdinalIgnoreCase))
//                    {
//                        context.Result = RedirectToAction("AccessDenied", "Home");
//                        return;
//                    }
//                }
//            }

//            base.OnActionExecuted(context);
//        }

//    }

//}
