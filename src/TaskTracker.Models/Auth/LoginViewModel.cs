using AutoMapper;
using TaskTracker.Shared.ApiClients;
using TaskTracker.Shared.Contracts;
using System.ComponentModel.DataAnnotations;

namespace TaskTracker.Models.Auth
{
    public class LoginViewModel : IMapFrom<LoginUserCommand>
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LoginViewModel, LoginUserCommand>();
        }
    }
}
