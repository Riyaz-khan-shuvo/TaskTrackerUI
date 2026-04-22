using AutoMapper;
using TaskTracker.Shared.ApiClients;
using TaskTracker.Shared.Contracts;
using System.ComponentModel.DataAnnotations;

namespace TaskTracker.Models.Auth
{
    public class ResetPasswordViewModel : IMapFrom<ResetPasswordCommand>
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ResetPasswordViewModel, ResetPasswordCommand>();
        }
    }
}
