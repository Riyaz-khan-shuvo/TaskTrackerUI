using AutoMapper;
using TaskTracker.Models;
using TaskTracker.Models.Auth;
using TaskTracker.Shared.ApiClients;

namespace TaskTracker.Repo.Implementations
{
    public class AccountRepos
    {
        private readonly AuthService _authService;
        private readonly IMapper _mapper;
        public AccountRepos(AuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        public async Task<ResultViewModel> RegisterAsync(RegisterViewModel vm, string token = null)
        {
            if (vm == null)
                throw new ArgumentNullException(nameof(vm));
            var command = _mapper.Map<RegisterUserCommand>(vm);
            var apiResult = await _authService.RegisterAsync(command);
            var result = _mapper.Map<ResultViewModel>(apiResult);
            return result;
        }

        public async Task<ResultViewModel> LoginAsync(LoginViewModel vm, string token = null)
        {
            if (vm == null)
                throw new ArgumentNullException(nameof(vm));
            var command = _mapper.Map<LoginUserCommand>(vm);
            var apiResult = await _authService.LoginAsync(command);
            var result = _mapper.Map<ResultViewModel>(apiResult);
            return result;
        }

        public async Task<ResultViewModel> LogoutAsync(string token)
        {
            var apiResult = await _authService.LogoutAsync();
            var result = _mapper.Map<ResultViewModel>(apiResult);
            return result;
        }

        public async Task<ResultViewModel> ForgetPasswordAsync(ForgetPasswordViewModel vm)
        {
            if (vm == null)
                throw new ArgumentNullException(nameof(vm));
            var command = _mapper.Map<ForgotPasswordCommand>(vm);
            var apiResult = await _authService.ForgotPasswordAsync(command);
            var result = _mapper.Map<ResultViewModel>(apiResult);
            return result;
        }

        public async Task<ResultViewModel> ResetPasswordAsync(ResetPasswordViewModel vm)
        {
            if (vm == null)
                throw new ArgumentNullException(nameof(vm));
            var command = _mapper.Map<ResetPasswordCommand>(vm);
            var apiResult = await _authService.ResetPasswordAsync(command);
            var result = _mapper.Map<ResultViewModel>(apiResult);
            return result;
        }

        public async Task<ResultViewModel> VerifyEmailAsync(VerifyEmailViewModel vm)
        {
            if (vm == null)
                throw new ArgumentNullException(nameof(vm));
            var command = _mapper.Map<VerifyEmailCommand>(vm);
            var apiResult = await _authService.VerifyEmailAsync(command);
            var result = _mapper.Map<ResultViewModel>(apiResult);
            return result;
        }

        public async Task<ResultViewModel> ResendVerificationAsync(string email)
        {
            var command = new ResendVerificationCommand
            {
                Email = email
            };
            var apiResult = await _authService.ResendVerificationAsync(command);
            var result = _mapper.Map<ResultViewModel>(apiResult);
            return result;
        }
    }
}
