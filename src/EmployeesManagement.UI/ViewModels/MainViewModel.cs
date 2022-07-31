using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using AutoMapper;
using EmployeesManagement.Business.Exceptions;
using EmployeesManagement.Business.Interfaces;
using EmployeesManagement.Business.Models;
using EmployeesManagement.UI.Interfaces;
using Microsoft.Extensions.Logging;
using Reactive.Bindings;

namespace EmployeesManagement.UI.ViewModels;

public sealed class MainViewModel : BaseViewModel, IDisposable
{
    private readonly ILogger<MainViewModel> _logger;
    private readonly IPersonService _personService;
    private readonly ICheckUserPermissionVerifier _checkUserPermissionVerifier;
    private readonly IMapper _mapper;
    private PersonModel _selectedPerson { get; set; }
    private bool _newTheme;
    
    public Action<StyleThemeType> ChangeStyleAction { get; set; }
    public PersonModel SelectedPerson
    {
        get => _selectedPerson;
        set
        {
            _selectedPerson = value;
            OnPropertyChanged(nameof(SelectedPerson));
            
            DisableButtons(value == null);
        }
    }
    public IList<PersonModel> PersonModels { get; set; }
    public bool CanAdd { get; set; }
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public bool Enabled { get; set; }
    public UserModel CurrentUser { get; set; }
    public string ErrorMessage { get; set; }
    public bool NewTheme
    {
        get => _newTheme;
        set
        {
            _newTheme = value;

            var theme = value ? StyleThemeType.Orange : StyleThemeType.Classic;
            ChangeStyleAction(theme);
        }
    }

    public ReactiveProperty<string> Search { get; }
    public ReactiveCommand AddCommand { get; }
    public AsyncReactiveCommand<PersonModel> EditCommand { get; }
    public AsyncReactiveCommand<PersonModel> DeleteCommand { get; }

    public MainViewModel(
        ILogger<MainViewModel> logger,
        ApplicationState applicationState,
        IPersonService personService,
        IMapper mapper,
        ICheckUserPermissionVerifier checkUserPermissionVerifier)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _personService = personService ?? throw new ArgumentNullException(nameof(personService));
        _checkUserPermissionVerifier = checkUserPermissionVerifier ??
                                       throw new ArgumentNullException(nameof(checkUserPermissionVerifier));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        
        applicationState.UserSuccessfulAuthorizedFn = EnableForm;

        Search = new ReactiveProperty<string>();
        Search
            .Where(x => Enabled)
            .Delay(TimeSpan.FromMilliseconds(100))
            .Subscribe(async value => await UpdateTableAsync(value));

        AddCommand = new ReactiveCommand();
        AddCommand.Subscribe(_ => Create());
        EditCommand = new AsyncReactiveCommand<PersonModel>();
        EditCommand.Subscribe(async person => await SaveAsync(person));
        DeleteCommand = new AsyncReactiveCommand<PersonModel>();
        DeleteCommand.Subscribe(async person => await DeleteAsync(person));
    }

    private async Task EnableForm(UserModel user)
    {
        CurrentUser = user;
        Enabled = true;

        await UpdateTableAsync("");
        
        DisableButtons(true);

        OnPropertyChanged(nameof(Enabled));
    }

    private async Task UpdateTableAsync(string search)
    {
        try
        {
            var result = await _personService.GetPersonsAsync(search);

            PersonModels = _mapper.Map<IEnumerable<PersonModel>>(result).ToList();

            OnPropertyChanged(nameof(PersonModels));

            ErrorMessage = "";
            OnPropertyChanged(nameof(ErrorMessage));
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "{0} => Getting data failed", nameof(UpdateTableAsync));
            ErrorMessage = "Не удалось получить данные.";
            OnPropertyChanged(nameof(ErrorMessage));
        }
    }

    private void Create()
    {
        SelectedPerson = new PersonModel();
        OnPropertyChanged(nameof(SelectedPerson));
    }

    private async Task SaveAsync(PersonModel person)
    {
        DisableButtons(false);

        try
        {
            var model = _mapper.Map<Person>(person);
            
            if (model.Id == 0)
            {
                await _personService.AddPersonAsync(model);
                await UpdateTableAsync(Search.Value);
            }
            else
            {
                await _personService.UpdatePersonAsync(model);
            }

            ErrorMessage = "";
            OnPropertyChanged(nameof(ErrorMessage));
        }
        catch (PersonAlreadyExistException)
        {
            ErrorMessage = "Данный человек уже присутствует в базе данных.";
            OnPropertyChanged(nameof(ErrorMessage));
        }
        catch(Exception ex)
        {
            ErrorMessage = "Не удалось обновить запись.";
            OnPropertyChanged(nameof(ErrorMessage));
            
            _logger.LogError(ex, "{0} => Saving data failed (key: {1}", 
                nameof(UpdateTableAsync), person.Id);
        }
        finally
        {
            DisableButtons(true);
        }
    }

    private async Task DeleteAsync(PersonModel person)
    {
        DisableButtons(false);

        try
        {
            var model = _mapper.Map<Person>(person);
            await _personService.DeletePersonAsync(model);
            PersonModels.Remove(person);
            SelectedPerson = null;
            OnPropertyChanged(nameof(PersonModels));
            OnPropertyChanged(nameof(SelectedPerson));

            ErrorMessage = "";
            OnPropertyChanged(nameof(ErrorMessage));
        }
        catch(Exception ex)
        {
            ErrorMessage = "Не удалось удалить запись.";
            OnPropertyChanged(nameof(ErrorMessage));
            
            _logger.LogError(ex, "{0} => Removing data failed (key: {1}", 
                nameof(UpdateTableAsync), person.Id);
        }
        finally
        {
            DisableButtons(true);
        }
    }

    private void DisableButtons(bool disable)
    {
        CheckAndApplyUserPermissions(CurrentUser);
        
        if (disable)
        {
            CanEdit = false;
            CanDelete = false;
        }
        
        OnPropertyChanged(nameof(CanAdd));
        OnPropertyChanged(nameof(CanEdit));
        OnPropertyChanged(nameof(CanDelete));
    }

    private void CheckAndApplyUserPermissions(UserModel user)
    {
        CanAdd = _checkUserPermissionVerifier.CanAdd(user);
        CanEdit = _checkUserPermissionVerifier.CanEdit(user);
        CanDelete = _checkUserPermissionVerifier.CanDelete(user);
    }

    public void Dispose()
    {
        Search?.Dispose();
        AddCommand?.Dispose();
        EditCommand?.Dispose();
        DeleteCommand?.Dispose();
    }
}