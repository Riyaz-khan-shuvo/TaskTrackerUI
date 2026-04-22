using AutoMapper;
using TaskTracker.Shared.ApiClients;

namespace Scholify.Shared.ApiClients
{
    public class UpsertMappingProfile : Profile
    {
        public UpsertMappingProfile()
        {
            CreateMap<TaskItemModel, UpsertTaskCommand>();
           
        }
    }
}
