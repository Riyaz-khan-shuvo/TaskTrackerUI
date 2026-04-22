using AutoMapper;
using TaskTracker.Shared.ApiClients;
using TaskTracker.Shared.Contracts;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaskTracker.Models
{
    public class UserRoleViewModel : IMapFrom<UpsertRoleCommand>
    {
        public int Id { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        public Guid? IdentityRoleId { get; set; } = Guid.Empty;
        public string? Operation { get; set; }
        public string? Status { get; set; }

        public bool IsRoleMenuView { get; set; } = false;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserRoleViewModel, UpsertRoleCommand>().ReverseMap();
        }
    }

    public class UserGroupViewModel : Entity
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Group Name")]
        public string Name { get; set; }

        public string Operation { get; set; }
        public string msg { get; set; }
        public string? CreatedBy { get; set; }
        public string? LastModifiedBy { get; set; }
        public string? LastUpdateFrom { get; set; }
        public string? CreatedOn { get; set; }
        public string? LastModifiedOn { get; set; }
        public string? CreatedFrom { get; set; }
    }

    public class MenuViewModel
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string Controller { get; set; }
        public int ParentId { get; set; }
        public int SubParentId { get; set; }
        public int DisplayOrder { get; set; }

        public bool IsChecked { get; set; }
    }

    public class RoleMenuViewModel : Entity, IMapFrom<UpsertRoleMenuCommand>
    {
        public int Id { get; set; }
        public string? RoleId { get; set; }
        public string UserGroupId { get; set; }
        public int? MenuId { get; set; }
        public bool List { get; set; }
        public bool Insert { get; set; }
        public bool Delete { get; set; }
        public bool Post { get; set; }
        public bool IsChecked { get; set; }
        public int? ParentId { get; set; }
        public int? SubParentId { get; set; }
        public string? RoleName { get; set; }
        public string? MenuName { get; set; }
        public string? UserGroupName { get; set; }

        public string BranchName { get; set; }
        [Display(Name = "Branch Name")]
        public int? BranchId { get; set; }
        public List<string> IDs { get; set; }
        public string? Operation { get; set; }
        public string Type { get; set; }
        public string msg { get; set; }
        public string Url { get; set; }
        public int DisplayOrder { get; set; }
        public string Name { get; set; }
        public string Controller { get; set; }

        public string? CreatedBy { get; set; }
        public string? LastModifiedBy { get; set; }
        public string? LastUpdateFrom { get; set; }
        public string? CreatedOn { get; set; }
        public string? LastModifiedOn { get; set; }
        public string? CreatedFrom { get; set; }

        public RoleMenuViewModel()
        {
            RoleMenuList = new List<RoleMenuViewModel>();
        }

        public List<RoleMenuViewModel> RoleMenuList { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<RoleMenuViewModel, RoleMenuVM>()
                .ForMember(d => d.RoleId,
                    o => o.MapFrom(s =>
                        string.IsNullOrEmpty(s.RoleId)
                            ? (int?)null
                            : int.Parse(s.RoleId))) 
                .ForMember(d => d.RoleMenuList,
                    o => o.MapFrom(s => s.RoleMenuList));

            profile.CreateMap<RoleMenuVM, RoleMenuViewModel>();

            profile.CreateMap<RoleMenuViewModel, UpsertRoleMenuCommand>()
                .IncludeBase<RoleMenuViewModel, RoleMenuVM>();
        }
    }

    public class UserMenuViewModel : Entity
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        //public int RoleId { get; set; }
        public string? RoleId { get; set; }
        public int? MenuId { get; set; }
        public bool List { get; set; }
        public bool Insert { get; set; }
        public bool Delete { get; set; }
        public bool Post { get; set; }
        public bool IsChecked { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? RoleName { get; set; }
        public string? MenuName { get; set; }
        public int? ParentId { get; set; }
        public int? SubParentId { get; set; }
        public int? DisplayOrder { get; set; }
        public string? Url { get; set; }
        public string? Name { get; set; }
        public string? Controller { get; set; }
        public string? BranchName { get; set; }
        [Display(Name = "Branch Name")]
        public int? BranchId { get; set; }
        public List<string> IDs { get; set; }
        public string? Operation { get; set; }
        public string? msg { get; set; }
        public string? CreatedBy { get; set; }
        public string? LastModifiedBy { get; set; }
        public string? LastUpdateFrom { get; set; }
        public string? CreatedOn { get; set; }
        public string? LastModifiedOn { get; set; }
        public string? CreatedFrom { get; set; }

        public UserMenuViewModel()
        {
            userMenuList = new List<UserMenuViewModel>();
        }

        public List<UserMenuViewModel> userMenuList { get; set; }

    }


    public class UserMenu
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int RoleId { get; set; }
        public int MenuId { get; set; }
        public bool List { get; set; }
        public bool Insert { get; set; }
        public bool Delete { get; set; }
        public bool Post { get; set; }
        public string MenuName { get; set; }
        public string IconClass { get; set; }
        public int ParentId { get; set; }
        public int SubParentId { get; set; }
        public int SubChildId { get; set; }
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
        public string Url { get; set; }
        public string Controller { get; set; }
        public int TotalChild { get; set; }
        public int TotalSubChild { get; set; }


    }

    public class UserViewModel
    {
        public int? Id { get; set; }
        public string? RoleName { get; set; }
        public string? GroupName { get; set; }
    }

}
