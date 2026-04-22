using TaskTracker.Repo.Configuration;
using TaskTracker.Repo.Interfaces;
using TaskTracker.Models;
using TaskTracker.Models.kendocommon;
using System.Text.Json;

namespace TaskTracker.Repo.Implementations
{
    public class MenuAuthorizationRepo : IMenuAuthorizationRepo
    {
        private readonly HttpRequestHelper _httpRequestHelper;

        public MenuAuthorizationRepo(HttpRequestHelper httpRequestHelper)
        {
            _httpRequestHelper = httpRequestHelper;
        }

        public async Task<ResultViewModel> RoleIndexAsync(GridOptionVM options, string token = null)
        {
            try
            {
                string url = "api/MenuAuthorization/GetRoleGridData";
                var result = await _httpRequestHelper.PostAsync<ResultViewModel>(url, options, token);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MENU AUTH REPO ERROR] {ex.Message}");
                return new ResultViewModel
                {
                    Status = "false",
                    Message = ex.Message,
                    Data = new List<UserRoleViewModel>(),
                    Count = 0
                };
            }
        }


        public async Task<ResultViewModel> GetAssignedMenuListAsync(string userId, string token = null)
        {
            try
            {
                string apiUrl = $"api/MenuAuthorization/AssignedMenuList/{userId}";

                // Get raw JSON from API
                var result = await _httpRequestHelper.GetAsync<ResultViewModel>(apiUrl, token);

                List<UserMenu> menuList = new List<UserMenu>();
                if (result.Data != null)
                {
                    if (result.Data is JsonElement jsonElement && jsonElement.ValueKind == JsonValueKind.Array)
                    {
                        menuList = JsonSerializer.Deserialize<List<UserMenu>>(jsonElement.GetRawText(), new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                    }
                }
                return new ResultViewModel
                {
                    Status = "true",
                    Message = "Success",
                    Data = menuList
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[COMMON REPO ERROR] {ex.Message}");
                return new ResultViewModel
                {
                    Status = "false",
                    Message = ex.Message,
                    Data = new List<UserMenu>()
                };
            }
        }



        public async Task<ResultViewModel> UpsertRoleAsync(UserRoleViewModel Vm, string token=null)
        {
            try
            {
                string url = "api/MenuAuthorization/UpsertRole";
                var result = await _httpRequestHelper.PostAsync<ResultViewModel>(url, Vm, token);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Role REPO ERROR] {ex.Message}");
                return new ResultViewModel
                {
                    Status = "false",
                    Message = ex.Message,
                    Data = null
                };
            }
        }
        public async Task<ResultViewModel> GetRoleListAsync(CommonViewModel dto, string token = null)
        {
            try
            {
                string url = "api/MenuAuthorization/GetRoleList";
                var result = await _httpRequestHelper.PostAsync<ResultViewModel>(url, dto, token);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Banner LIST REPO ERROR] {ex.Message}");
                return new ResultViewModel
                {
                    Status = "false",
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<ResultViewModel> GetMenuAccessData(CommonViewModel dto, string token = null)
        {
            try
            {
                string url = "api/MenuAuthorization/GetMenuAccessData";
                var result = await _httpRequestHelper.PostAsync<ResultViewModel>(url, dto, token);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GetMenuAccessData LIST REPO ERROR] {ex.Message}");
                return new ResultViewModel
                {
                    Status = "false",
                    Message = ex.Message,
                    Data = null
                };
            }
        }


        public async Task<ResultViewModel> UpsertRoleMenuAsync(RoleMenuViewModel Vm, string token = null)
        {
            try
            {
                string url = "api/MenuAuthorization/UpsertRoleMenu";
                var result = await _httpRequestHelper.PostAsync<ResultViewModel>(url, Vm, token);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Role REPO ERROR] {ex.Message}");
                return new ResultViewModel
                {
                    Status = "false",
                    Message = ex.Message,
                    Data = null
                };
            }
        }
    }
}
