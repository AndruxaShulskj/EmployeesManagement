using EmployeesManagement.UI.ViewModels;

namespace EmployeesManagement.UI.Views;

public partial class LoginView : Window
{
    public LoginView(LoginViewModel loginViewModel)
    {
        DataContext = loginViewModel;

        InitializeComponent();
    }
}