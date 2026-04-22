using TaskTracker.Models;
using TaskTracker.Models.kendocommon;
using TaskTracker.Repo.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TaskTracker.UI.Areas.Setup.Controllers
{
    [Area("Setup")]
    public class MenuAuthorizationController : Controller
    {
        private readonly IMenuAuthorizationRepo _repo;

        public MenuAuthorizationController(IMenuAuthorizationRepo repo)
        {
            _repo = repo;
        }

        public IActionResult Index() => View();

        public IActionResult Role() => View();

        [HttpPost]
        public async Task<JsonResult> RoleIndex([FromBody] GridOptionVM options)
        {
            var token = HttpContext.Session.GetString("Token");
            var result = await _repo.RoleIndexAsync(options, token);

            if (result.Status == "Success" && result.Data != null)
            {
                var gridData = JsonConvert.DeserializeObject<GridEntity<UserRoleViewModel>>(result.Data.ToString());

                return Json(new
                {
                    gridData.Items,
                    gridData.TotalCount
                });
            }

            return Json(new { result });
        }


        public IActionResult RoleUpsert()
        {
            var vm = new UserRoleViewModel
            {
                Id = 0,
                Operation = "add"
            };
            return View("RoleUpsert", vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpsertRole(UserRoleViewModel vm)
        {

            try
            {
                var token = HttpContext.Session.GetString("Token");
                var result = await _repo.UpsertRoleAsync(vm, token);
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Status = "Fail",
                    Message = $"An unexpected error occurred: {ex.Message}"
                });
            }
        }


        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                CommonViewModel param = new CommonViewModel();
                param.IntId = int.Parse(id);
                var token = HttpContext.Session.GetString("Token");
                var result = await _repo.GetRoleListAsync(param, token);

                if (result == null || result.Data == null)
                {
                    return NotFound();
                }

                var vm = JsonConvert.DeserializeObject<List<UserRoleViewModel>>(result.Data.ToString()).FirstOrDefault();
                vm.Operation = "update";
                return View("RoleUpsert", vm);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error loading category: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        public ActionResult RoleMenu()
        {
            var vm = new UserRoleViewModel
            {
                IsRoleMenuView = true
            };
            return View("RoleMenu", vm);
        }

        [HttpGet]
        public async Task<IActionResult> RoleMenuEdit(string id, string roleName)
        {
            try
            {
                CommonViewModel param = new CommonViewModel();
                param.IntId = int.Parse(id);
                
                var token = HttpContext.Session.GetString("Token");
                var result = await _repo.GetMenuAccessData(param, token);

                if (result == null || result.Data == null)
                {
                    return NotFound();
                }
                var vm = new RoleMenuViewModel();
                vm.RoleId = id.ToString();
                vm.RoleName = roleName;
                vm.Type = "User Role";
                vm.RoleMenuList = JsonConvert.DeserializeObject<List<RoleMenuViewModel>>(result.Data.ToString());
                vm.Operation = "update";
                return View("RoleMenuUpsert", vm);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error loading category: {ex.Message}";
                return RedirectToAction("Index");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpsertRoleMenu(RoleMenuViewModel vm)
        {

            try
            {
                var token = HttpContext.Session.GetString("Token");
                var result = await _repo.UpsertRoleMenuAsync(vm, token);
                if (result.Status == "Success") 
                {
                    HttpContext.Session.Remove("UserMenu");
                }
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Status = "Fail",
                    Message = $"An unexpected error occurred: {ex.Message}"
                });
            }
        }



    }
}
