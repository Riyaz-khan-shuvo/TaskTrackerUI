using AutoMapper;
using TaskTracker.Shared.ApiClients;
using TaskTracker.Shared.Contracts;

namespace TaskTracker.Models
{
    public class CommonViewModel : IMapFrom<CommonVM>
    {
        public string? Id { get; set; }
        public int? IntId { get; set; }
        public string? UserId { get; set; }
        public string? ParentId { get; set; }
        public string? CompanyId { get; set; }
        public string? BranchId { get; set; }
        public string?[] IDs { get; set; } = Array.Empty<string?>();
        public string? RouteId { get; set; }
        public string? ModifyBy { get; set; }
        public string? ModifyFrom { get; set; }
        public string? SettingValue { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
        public string? IsPost { get; set; }


        public string? Status { get; set; }
        public string? Type { get; set; }
        public string? TableName { get; set; }
        public string? Name { get; set; }
        public string? BanglaName { get; set; }
        public string? EnumType { get; set; }
        public string? Group { get; set; }
        public string? Value { get; set; }
        public string[] ConditionalFields { get; set; } = Array.Empty<string>();
        public string[] ConditionalValues { get; set; } = Array.Empty<string>();
        public void Mapping(Profile profile)
        {

            //profile.CreateMap<CommonVM, CommonViewModel>()
            //       .ForMember(dest => dest.IDs, opt => opt.MapFrom(src => src.IDs.ToArray()))
            //       .ForMember(dest => dest.ConditionalFields, opt => opt.MapFrom(src => src.ConditionalFields.ToArray()))
            //       .ForMember(dest => dest.ConditionalValues, opt => opt.MapFrom(src => src.ConditionalValues.ToArray()));


            // Map from CommonVM → CommonViewModel
            profile.CreateMap<CommonVM, CommonViewModel>()
                .ForMember(dest => dest.IDs, opt => opt.MapFrom(src => src.IDs.ToArray()))
                .ForMember(dest => dest.ConditionalFields, opt => opt.MapFrom(src => src.ConditionalFields.ToArray()))
                .ForMember(dest => dest.ConditionalValues, opt => opt.MapFrom(src => src.ConditionalValues.ToArray()));

            // Map from CommonViewModel → CommonVM
            profile.CreateMap<CommonViewModel, CommonVM>()
                .ForMember(dest => dest.IDs, opt => opt.MapFrom(src => src.IDs))
                .ForMember(dest => dest.ConditionalFields, opt => opt.MapFrom(src => src.ConditionalFields))
                .ForMember(dest => dest.ConditionalValues, opt => opt.MapFrom(src => src.ConditionalValues));


        }

    }
}
