using TaskTracker.Models;
using TaskTracker.Models.Auth;

namespace TaskTracker.Repo.Interfaces
{
    public interface IAccountRepo
    {
        Task<ResultViewModel> RegisterAsync(RegisterViewModel vm, string token = null);
        Task<ResultViewModel> LoginAsync(LoginViewModel vm, string token = null);
        Task<ResultViewModel> LogoutAsync(string token);
        Task<ResultViewModel> ForgetPasswordAsync(ForgetPasswordViewModel vm);
        Task<ResultViewModel> ResetPasswordAsync(ResetPasswordViewModel vm);
        Task<ResultViewModel> VerifyEmailAsync(VerifyEmailViewModel vm);
        Task<ResultViewModel> ResendVerificationAsync(string email);
    }

}
