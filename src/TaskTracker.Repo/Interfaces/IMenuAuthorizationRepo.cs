using TaskTracker.Models;
using TaskTracker.Models.kendocommon;

namespace TaskTracker.Repo.Interfaces
{
    public interface IMenuAuthorizationRepo
    {
        Task<ResultViewModel> RoleIndexAsync(GridOptionVM options, string token = null);
        Task<ResultViewModel> GetAssignedMenuListAsync(string userId, string token = null);
        Task<ResultViewModel> UpsertRoleAsync(UserRoleViewModel Vm, string token = null);
        Task<ResultViewModel> GetRoleListAsync(CommonViewModel dto, string token = null);
        Task<ResultViewModel> GetMenuAccessData(CommonViewModel dto, string token = null);
        Task<ResultViewModel> UpsertRoleMenuAsync(RoleMenuViewModel Vm, string token = null);
    }
}
