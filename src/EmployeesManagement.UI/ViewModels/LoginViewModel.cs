using System.Windows.Controls;
using AutoMapper;
using EmployeesManagement.Business.Exceptions;
using EmployeesManagement.Business.Interfaces;
using EmployeesManagement.Business.Models;
using Microsoft.Extensions.Logging;
using Reactive.Bindings;

namespace EmployeesManagement.UI.ViewModels;

public sealed class LoginViewModel : BaseViewModel, IDisposable
{
    private readonly ILogger<LoginViewModel> _logger;
    private readonly IAuthenticationService _authenticationService;
    private readonly IMapper _mapper;
    private readonly ApplicationState _applicationState;
    
    private string _enterUserLogin;
    
    public AsyncReactiveCommand<object> AuthCommand { get; set; }
    public bool CanAuth { get; set; }
    public string EnterUserLogin
    {
        get => _enterUserLogin ?? string.Empty;
        set
        {
            if (value == string.Empty || value.Length == 1)
            {
                _enterUserLogin = null;

                CanAuth = false;

                OnPropertyChanged(nameof(EnterUserLogin));
                OnPropertyChanged(nameof(CanAuth));

                return;
            }
            
            _enterUserLogin = value;
            CanAuth = true;

            OnPropertyChanged(nameof(EnterUserLogin));
            OnPropertyChanged(nameof(CanAuth));
        }
    }
    public string ErrorMessage { get; set; }
    
    private bool _closeTrigger;

    /// <summary>
    /// Gets or Sets if the main window should be closed
    /// </summary>
    public bool CloseTrigger
    {
        get => _closeTrigger;
        set
        {
            _closeTrigger = value;
            OnPropertyChanged(nameof(CloseTrigger));
        }
    }

    public LoginViewModel() { }
    public LoginViewModel(
        ILogger<LoginViewModel> logger,
        IAuthenticationService authenticationService,
        IMapper mapper,
        ApplicationState applicationState)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _authenticationService =
            authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        _mapper = mapper;
        _applicationState = applicationState ?? throw new ArgumentNullException(nameof(applicationState));

        AuthCommand = new AsyncReactiveCommand<object>();
        AuthCommand.Subscribe(async passwordBox => await AuthorizationAsync(passwordBox));
    }

    private async Task AuthorizationAsync(object passwordBox)
    {
        var securePassword = (passwordBox as PasswordBox)?.Password;
        
        var model = _mapper.Map<UserCredential>(
            new UserCredentialModel(_enterUserLogin, securePassword));

        try
        {
            var user = await _authenticationService.LoginAsync(model);

            var userModel = _mapper.Map<UserModel>(user);

            await _applicationState.UserSuccessfulAuthorizedFn(userModel);

            CloseTrigger = true;
        }
        catch (UserNotExistOrWrongPasswordException)
        {
            ErrorMessage = "Авторизация не пройдена. Пожалуйста проверьте верность логина и пароля.";

            OnPropertyChanged(nameof(ErrorMessage));
        }
        catch (Exception ex)
        {
            ErrorMessage = "Ошибка подключения к базе данных.";
            
            _logger.LogError(ex, "{0} => Login failed ", nameof(AuthorizationAsync));
        }
    }

    public void Dispose()
    {
        AuthCommand.Dispose();
    }
}