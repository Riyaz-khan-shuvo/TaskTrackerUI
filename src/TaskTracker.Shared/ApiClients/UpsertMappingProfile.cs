using AutoMapper;

namespace TaskTracker.Shared.ApiClients
{
    public class UpsertMappingProfile : Profile
    {
        public UpsertMappingProfile()
        {
            CreateMap<TaskItemModel, UpsertTaskCommand>();
           
        }
    }
}
