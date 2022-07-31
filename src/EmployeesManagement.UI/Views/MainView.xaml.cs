using EmployeesManagement.UI.Interfaces;
using EmployeesManagement.UI.ViewModels;

namespace EmployeesManagement.UI.Views;

public partial class MainView : Window
{
    public MainView(
        IStyleThemeMapper styleThemeMapper,
        MainViewModel model)
    {
        model.ChangeStyleAction =
            (theme) =>
            {
                Application.Current.Resources.MergedDictionaries.Clear();

                var style = styleThemeMapper.Map(theme);
                Application.Current.Resources.MergedDictionaries.Add(style);
            };

        DataContext = model;

        InitializeComponent();
    }
}