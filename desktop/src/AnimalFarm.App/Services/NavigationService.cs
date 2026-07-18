using System.Windows;
using System.Windows.Controls;

namespace AnimalFarm.App.Services;

public sealed class NavigationService
{
    private Frame? _frame;

    public void Attach(Frame frame)
    {
        _frame = frame;
    }

    public void Navigate(Page page)
    {
        if (_frame is null)
        {
            throw new InvalidOperationException("Navigation frame is not attached.");
        }

        _frame.Navigate(page);
    }

    public bool CanGoBack => _frame?.CanGoBack == true;

    public void GoBack()
    {
        if (CanGoBack)
        {
            _frame!.GoBack();
        }
    }

    public void ClearHistoryAndNavigate(Page page)
    {
        if (_frame is null)
        {
            throw new InvalidOperationException("Navigation frame is not attached.");
        }

        while (_frame.CanGoBack)
        {
            _frame.RemoveBackEntry();
        }

        _frame.Navigate(page);
    }
}
