using EmployeesManagement.UI.Interfaces;

namespace EmployeesManagement.UI.Mapping;

public class StyleThemeMapper : IStyleThemeMapper
{
    public ResourceDictionary Map(StyleThemeType type)
    {
        return type switch
        {
            StyleThemeType.Classic => GetResource("/Content/Themes/Classic.xaml"),
            StyleThemeType.Orange => GetResource("/Content/Themes/Orange.xaml"),
            _ => throw new Exception()
        };
    }

    private static ResourceDictionary GetResource(string url)
    {
        return new ResourceDictionary()
        {
            Source = new Uri(url, UriKind.Relative)
        };
    }
}