using AutoMapper;
using TaskTracker.Models;
using TaskTracker.Models.kendocommon;
using TaskTracker.Shared.ApiClients;

namespace TaskTracker.Repo.Implementations.Common
{
    public abstract class BaseApiRepo
    {
        protected readonly IMapper _mapper;

        protected BaseApiRepo(IMapper mapper)
        {
            _mapper = mapper;
        }

        protected TQuery MapGridQuery<TQuery>(GridOptionVM options)
            where TQuery : GridOptions, new()
        {
            var baseQuery = _mapper.Map<GridOptions>(options);
            return _mapper.Map<TQuery>(baseQuery);
        }

        // Map NSwag ResultVM → your internal ResultViewModel
        protected ResultViewModel MapResult(ResultVM result)
        {
            return _mapper.Map<ResultViewModel>(result);
        }

        // Generic executor: call any service method with a TQuery
        protected async Task<ResultViewModel> ExecuteGridAsync<TQuery>(
            GridOptionVM options,
            Func<TQuery, Task<ResultVM>> apiCall)
            where TQuery : GridOptions, new()
        {
            var query = MapGridQuery<TQuery>(options);
            var result = await apiCall(query);
            return MapResult(result);
        }
    }
}