using TaskTracker.Models;
using TaskTracker.Models.Auth;
using TaskTracker.Repo.Configuration;
using TaskTracker.Repo.Interfaces;

namespace TaskTracker.Repo.Implementations
{
    public class AccountRepo : IAccountRepo
    {
        private readonly HttpRequestHelper _httpRequestHelper;

        public AccountRepo(HttpRequestHelper httpRequestHelper)
        {
            _httpRequestHelper = httpRequestHelper;
        }

        public async Task<ResultViewModel> RegisterAsync(RegisterViewModel vm, string token = null)
        {
            try
            {
                string url = "api/Auth/register";
                var result = await _httpRequestHelper.PostAsync<ResultViewModel>(url, vm, token);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ACCOUNT REGISTER REPO ERROR] {ex.Message}");
                return new ResultViewModel
                {
                    Status = "false",
                    Message = ex.Message,
                    Data = null
                };
            }
        }
        public async Task<ResultViewModel> LoginAsync(LoginViewModel vm, string token = null)
        {
            try
            {
                string url = "api/Auth/Login";
                var result = await _httpRequestHelper.PostAsync<ResultViewModel>(url, vm, token);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ACCOUNT LOGIN REPO ERROR] {ex.Message}");
                return new ResultViewModel
                {
                    Status = "false",
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<ResultViewModel> LogoutAsync(string token)
        {
            try
            {
                string url = "api/Auth/logout";
                var result = await _httpRequestHelper.PostAsync<ResultViewModel>(url, null, token);

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ACCOUNT LOGOUT REPO ERROR] {ex.Message}");
                return new ResultViewModel
                {
                    Status = "false",
                    Message = ex.Message
                };
            }
        }

        public async Task<ResultViewModel> ForgetPasswordAsync(ForgetPasswordViewModel vm)
        {
            try
            {
                string url = "api/Auth/forgot-password";
                var result = await _httpRequestHelper.PostAsync<ResultViewModel>(url, vm);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ACCOUNT FORGOT PASSWORD REPO ERROR] {ex.Message}");
                return new ResultViewModel { Status = "false", Message = ex.Message };
            }
        }

        public async Task<ResultViewModel> ResetPasswordAsync(ResetPasswordViewModel vm)
        {
            try
            {
                string url = "api/Auth/reset-password";
                var result = await _httpRequestHelper.PostAsync<ResultViewModel>(url, vm);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ACCOUNT RESET PASSWORD REPO ERROR] {ex.Message}");
                return new ResultViewModel { Status = "false", Message = ex.Message };
            }
        }

        public async Task<ResultViewModel> VerifyEmailAsync(VerifyEmailViewModel vm)
        {
            try
            {
                string url = "api/Auth/verify-email";
                var result = await _httpRequestHelper.PostAsync<ResultViewModel>(url, vm);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ACCOUNT VERIFY EMAIL REPO ERROR] {ex.Message}");
                return new ResultViewModel
                {
                    Status = "false",
                    Message = ex.Message
                };
            }
        }


        public async Task<ResultViewModel> ResendVerificationAsync(string email)
        {
            try
            {
                var payload = new { email= email }; 
                string url = "api/Auth/resend-verification";
                var result = await _httpRequestHelper.PostAsync<ResultViewModel>(url, payload);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ACCOUNT RESEND VERIFICATION REPO ERROR] {ex.Message}");
                return new ResultViewModel { Status = "false", Message = ex.Message };
            }
        }




    }
}
