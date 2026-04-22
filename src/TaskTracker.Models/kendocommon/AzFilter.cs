using AutoMapper;
using TaskTracker.Shared.ApiClients;
using TaskTracker.Shared.Contracts;

namespace TaskTracker.Models.kendocommon
{
    public class AzFilter
    {
        public class GridFilterVM : IMapFrom<GridFilter>
        {
            public string Operator { get; set; }
            public string Field { get; set; }
            public string Value { get; set; }
            public void Mapping(Profile profile)
            {
                profile.CreateMap<GridFilter, GridFilterVM>().ReverseMap();
            }
        }
        public class AutoCompFilter
        {

            public string field { get; set; }
            public bool ignore { get; set; }
            public string @operator { get; set; }
            public string value { get; set; }
        }
        public class GridFiltersVM : IMapFrom<GridFilters>
        {
            public List<GridFilterVM> Filters { get; set; } = new List<GridFilterVM>();
            public string Logic { get; set; } = string.Empty;
            public void Mapping(Profile profile)
            {
                profile.CreateMap<GridFilters, GridFiltersVM>()
                       .ForMember(dest => dest.Filters, opt => opt.MapFrom(src => src.Filters.ToList()))
                       .ReverseMap();
            }
        }
        public class AutoCompFilters
        {
            public List<AutoCompFilter> filters { get; set; }
            public string logic { get; set; }

        }
        public class AutoCompFilters1
        {
            public AutoCompFilter filters { get; set; }
            public string Logic { get; set; }
        }
        public class GridSortVM : IMapFrom<GridSort>
        {
            public string field { get; set; }

            public string dir { get; set; }
            public void Mapping(Profile profile)
            {
                profile.CreateMap<GridSort, GridSortVM>().ReverseMap();
            }
        }




        //-------------------------------------------------------------------
        public class SortDescription
        {

            public string field { get; set; }

            public string dir { get; set; }

        }
        public class FilterContainer
        {

            public List<FilterDescription> filters { get; set; }

            public string logic { get; set; }

        }
        public class FilterDescription
        {

            public string @operator { get; set; }

            public string field { get; set; }

            public string value { get; set; }

        }

        public class OrderForPaggging
        {
            public string OrderByField { get; set; }

            public string OrderByType { get; set; }
        }
    }
}
