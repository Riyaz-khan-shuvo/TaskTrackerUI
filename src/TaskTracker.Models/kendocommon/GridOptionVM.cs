using AutoMapper;
using TaskTracker.Shared.ApiClients;
using TaskTracker.Shared.Contracts;
using static TaskTracker.Models.kendocommon.AzFilter;

namespace TaskTracker.Models.kendocommon
{
    public class GridOptionVM : IMapFrom<GridOptions>
    {
        public int skip { get; set; }
        public int take { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }
        public List<AzFilter.GridSortVM> sort { get; set; } = new List<AzFilter.GridSortVM>();
        public AzFilter.GridFiltersVM filter { get; set; } = new AzFilter.GridFiltersVM();
        public CommonViewModel vm { get; set; }

        public GridOptionVM()
        {
            vm = new CommonViewModel();
        }
        public void Mapping(Profile profile)
        {
            // Base mapping
            profile.CreateMap<GridOptionVM, GridOptions>()
               .ForMember(d => d.Sort, opt => opt.MapFrom(s => s.sort.ToList()))
               .ForMember(d => d.Filter, opt => opt.MapFrom(s => s.filter))
               .ForMember(d => d.Vm, opt => opt.MapFrom(s => s.vm));

            // IncludeBase targets
            //profile.CreateMap<GridOptions, GetBannerGridQuery>();
            //profile.CreateMap<GridOptions, GetCategoryGridQuery>();
            //profile.CreateMap<GridOptions, GetSubCategoryGridQuery>();
            //profile.CreateMap<GridOptions, GetProductGridQuery>();
            profile.CreateMap<GridOptions, GetRoleGridQuery>();

            // Final mappings
            //profile.CreateMap<GridOptionVM, GetBannerGridQuery>()
            //    .IncludeBase<GridOptionVM, GridOptions>();

            //profile.CreateMap<GridOptionVM, GetCategoryGridQuery>()
            //    .IncludeBase<GridOptionVM, GridOptions>();

            //profile.CreateMap<GridOptionVM, GetSubCategoryGridQuery>()
            //    .IncludeBase<GridOptionVM, GridOptions>();

            //profile.CreateMap<GridOptionVM, GetProductGridQuery>()
            //    .IncludeBase<GridOptionVM, GridOptions>();

            profile.CreateMap<GridOptionVM, GetRoleGridQuery>()
              .IncludeBase<GridOptionVM, GridOptions>();
        }

    }

    public class AutoCompOptions
    {
        public int skip { get; set; }
        public int take { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }
        public AzFilter.AutoCompFilters filter { get; set; }
    }

    public class AutoCompOptions1
    {
        public int skip { get; set; }
        public int take { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }
        public AzFilter.AutoCompFilters1 filter { get; set; }
    }

    public class GridColumns
    {
        public string field { get; set; }
        public string title { get; set; }
        public string width { get; set; }
        public bool filterable { get; set; }
        public bool sortable { get; set; }
        public bool hidden { get; set; }
    }

    public class GridResult<T>
    {

        public GridEntity<T> Data(List<T> list, int totalCount)
        {
            var dEntity = new GridEntity<T>();
            dEntity.Items = list ?? new List<T>();
            dEntity.TotalCount = totalCount;
            dEntity.Columnses = new List<GridColumns>();
            return dEntity;
        }

    }

    public class GridEntity<T>
    {
        public IList<T> Items { get; set; }
        public int TotalCount { get; set; }
        public IList<GridColumns> Columnses { get; set; }

    }
}
