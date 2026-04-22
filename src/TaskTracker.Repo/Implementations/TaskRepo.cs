using AutoMapper;
using TaskTracker.Models;
using TaskTracker.Models.kendocommon;
using TaskTracker.Repo.Implementations.Common;
using TaskTracker.Shared.ApiClients;

namespace TaskTracker.Repo.Implementations
{
    public class TaskRepo : BaseApiRepo
    {
        private readonly TaskService _taskService;

        public TaskRepo(TaskService taskService, IMapper mapper)
            : base(mapper)
        {
            _taskService = taskService;
        }

        public async Task<ResultViewModel> GetGridDataAsync(GridOptionVM options)
        {
            return await ExecuteGridAsync<GetTaskGridQuery>(
                options,
                query => _taskService.GetGridDataAsync(query)
            );
        }

        public async Task<ResultVM> UpsertAsync(TaskItemModel vm)
        {
            var command = _mapper.Map<UpsertTaskCommand>(vm);
            return await _taskService.UpsertAsync(command);
        }

        public async Task<ResultVM> GetListAsync(int id)
        {
            return await _taskService.GetAsync(id);
        }

        public async Task<ResultVM> DeleteAsync(IEnumerable<string> ids)
        {
            var command = new DeleteTaskCommand
            {
                IDs = ids.ToList()
            };
            return await _taskService.DeleteAsync(command);
        }
    }
}