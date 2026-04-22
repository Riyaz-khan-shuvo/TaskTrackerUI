using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TaskTracker.Models.kendocommon;
using TaskTracker.Repo.Implementations;
using TaskTracker.Shared.ApiClients;

namespace TaskTracker.UI.Areas.Setup.Controllers
{
    [Area("Setup")]
    public class TaskController : Controller
    {
        private readonly TaskRepo _repo;

        public TaskController(TaskRepo repo)
        {
            _repo = repo;
        }

        public IActionResult Index() => View();

        [HttpPost]
        public async Task<IActionResult> GetGridData([FromBody] GridOptionVM options)
        {
            if (options == null)
                return BadRequest(new { Error = true, Message = "Invalid grid options." });

            var result = await _repo.GetGridDataAsync(options);

            if (result?.Status != "Success" || result.Data == null)
                return JsonFail("No data found.");

            var gridData = JsonConvert.DeserializeObject<GridEntity<TaskItemModel>>(result.Data.ToString());

            return Json(new
            {
                Items = gridData?.Items ?? new List<TaskItemModel>(),
                TotalCount = gridData?.TotalCount ?? 0
            });
        }

        [HttpGet]
        public async Task<IActionResult> Upsert(int id)
        {
            TaskItemModel model;

            if (id > 0)
            {
                var result = await _repo.GetListAsync(id);

                model = result?.Data != null
                    ? JsonConvert.DeserializeObject<TaskItemModel>(result.Data.ToString())
                    : new TaskItemModel();
            }
            else
            {
                model = new TaskItemModel
                {
                    DueDate = DateTime.Now.AddDays(7),
                    IsCompleted = false
                };
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(TaskItemModel vm)
        {
            if (vm == null)
                return JsonFail("Invalid data.");

            try
            {
                var result = await _repo.UpsertAsync(vm);
                return Json(result);
            }
            catch (Exception)
            {
                return JsonFail("An unexpected error occurred.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string?[] ids)
        {
            if (ids == null || !ids.Any(x => !string.IsNullOrWhiteSpace(x)))
                return JsonFail("No items selected.");

            try
            {
                var result = await _repo.DeleteAsync(ids);
                return Json(result);
            }
            catch (Exception)
            {
                return JsonFail("An unexpected error occurred.");
            }
        }

        private JsonResult JsonFail(string message)
        {
            return Json(new { Status = "Fail", Message = message });
        }
    }
}