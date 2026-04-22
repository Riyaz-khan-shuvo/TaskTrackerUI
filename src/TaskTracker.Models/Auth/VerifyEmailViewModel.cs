using AutoMapper;
using TaskTracker.Shared.ApiClients;
using TaskTracker.Shared.Contracts;

namespace TaskTracker.Models.Auth
{
    public class VerifyEmailViewModel : IMapFrom<VerifyEmailCommand>
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<VerifyEmailViewModel, VerifyEmailCommand>();
        }
    }
}
