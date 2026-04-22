using AutoMapper;
using TaskTracker.Models;
using TaskTracker.Models.kendocommon;
using TaskTracker.Shared.ApiClients;

namespace TaskTracker.Repo.Implementations
{
    public class MenuAuthorizationRepos
    {
        private readonly MenuAuthorizationService _menuAuthorizationService;
        private readonly IMapper _mapper;
        public MenuAuthorizationRepos(MenuAuthorizationService menuAuthorizationService, IMapper mapper)
        {
            _menuAuthorizationService = menuAuthorizationService;
            _mapper = mapper;
        }




        public async Task<ResultViewModel> RoleIndexAsync(GridOptionVM options, string token = null)
        {
            var query = _mapper.Map<GetRoleGridQuery>(options);
            var apiResult = await _menuAuthorizationService.GetRoleGridDataAsync(query);
            var result = _mapper.Map<ResultViewModel>(apiResult);
            return result;
        }


        public async Task<ResultViewModel> GetAssignedMenuListAsync(string userId, string token = null)
        {
            var apiResult = await _menuAuthorizationService.GetAssignedMenuListAsync(Guid.Parse(userId));
            var result = _mapper.Map<ResultViewModel>(apiResult);
            return result;
        }

        public async Task<ResultViewModel> UpsertRoleAsync(UserRoleViewModel vm, string token = null)
        {
            if (vm == null)
                throw new ArgumentNullException(nameof(vm));
            var command = _mapper.Map<UpsertRoleCommand>(vm);
            var apiResult = await _menuAuthorizationService.UpsertRoleAsync(command);
            var result = _mapper.Map<ResultViewModel>(apiResult);
            return result;
        }
        public async Task<ResultViewModel> GetRoleListAsync(CommonViewModel vm, string token = null)
        {
            if (vm == null)
                throw new ArgumentNullException(nameof(vm));
            var query = _mapper.Map<CommonVM>(vm);
            var apiResult = await _menuAuthorizationService.GetRoleListAsync(query);
            var result = _mapper.Map<ResultViewModel>(apiResult);
            return result;
        }

        public async Task<ResultViewModel> GetMenuAccessData(CommonViewModel vm, string token = null)
        {
            var query = _mapper.Map<CommonVM>(vm);
            var apiResult = await _menuAuthorizationService.GetMenuAccessDataAsync(query);
            var result = _mapper.Map<ResultViewModel>(apiResult);
            return result;
        }




        public async Task<ResultViewModel> UpsertRoleMenuAsync(RoleMenuViewModel vm, string token = null)
        {
            if (vm == null)
                throw new ArgumentNullException(nameof(vm));
            var command = _mapper.Map<UpsertRoleMenuCommand>(vm);
            var apiResult = await _menuAuthorizationService.UpsertRoleMenuAsync(command);
            var result = _mapper.Map<ResultViewModel>(apiResult);
            return result;
        }





    }
}
