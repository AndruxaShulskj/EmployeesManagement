using System.Windows;
using Microsoft.Xaml.Behaviors;

namespace EmployeesManagement.UI.Core.Behaviors;

public class CloseWindowBehavior : Behavior<Window>
{
    public bool CloseTrigger
    {
        get => (bool)GetValue(CloseTriggerProperty);
        set => SetValue(CloseTriggerProperty, value);
    }

    public static readonly DependencyProperty CloseTriggerProperty =
        DependencyProperty.Register(
            "CloseTrigger",
            typeof(bool),
            typeof(CloseWindowBehavior),
            new PropertyMetadata(false, OnCloseTriggerChanged));

    private static void OnCloseTriggerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is CloseWindowBehavior behavior)
        {
            behavior.OnCloseTriggerChanged();
        }
    }

    private void OnCloseTriggerChanged()
    {
        AssociatedObject.DialogResult = CloseTrigger;
    }
}