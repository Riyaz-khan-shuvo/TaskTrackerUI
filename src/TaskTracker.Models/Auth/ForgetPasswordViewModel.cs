using AutoMapper;
using TaskTracker.Shared.ApiClients;
using TaskTracker.Shared.Contracts;
using System.ComponentModel.DataAnnotations;

namespace TaskTracker.Models.Auth
{
    public class ForgetPasswordViewModel : IMapFrom<ForgotPasswordCommand>
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ForgetPasswordViewModel, ForgotPasswordCommand>();
        }
    }
}
