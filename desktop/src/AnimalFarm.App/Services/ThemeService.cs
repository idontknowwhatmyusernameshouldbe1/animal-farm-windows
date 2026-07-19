using System.Windows;
using System.Windows.Media;
using AnimalFarm.Core.I18n;

namespace AnimalFarm.App.Services;

public sealed class ThemeService
{
    public AppTheme Current { get; private set; } = AppTheme.Light;

    public event EventHandler? ThemeChanged;

    public void Apply(AppTheme theme)
    {
        Current = theme;
        var r = Application.Current.Resources;

        if (theme == AppTheme.Dark)
        {
            SetBrush(r, "InkBrush", "#E8F0F5");
            SetBrush(r, "InkSoftBrush", "#C5D0DC");
            SetBrush(r, "MutedBrush", "#8FA3B5");
            SetBrush(r, "AccentBrush", "#4A9FD8");
            SetBrush(r, "AccentHoverBrush", "#6BB3E3");
            SetBrush(r, "DangerBrush", "#E07A6A");
            SetBrush(r, "SurfaceBrush", Color.FromArgb(0x9E, 0x12, 0x1A, 0x28));
            SetBrush(r, "SurfaceStrongBrush", Color.FromArgb(0xE0, 0x18, 0x24, 0x36));
            SetBrush(r, "LineBrush", Color.FromArgb(0x1A, 0xE8, 0xF0, 0xF5));
            SetBrush(r, "HeaderBrush", Color.FromArgb(0xD1, 0x0C, 0x14, 0x20));
            SetBrush(r, "ImageFallbackBrush", "#2A3A4E");
            SetBrush(r, "SecondaryHoverBrush", Color.FromArgb(0x33, 0x4A, 0x9F, 0xD8));
            SetBrush(r, "PrimaryForegroundBrush", "#F0F7FC");
            SetBrush(r, "BgBaseBrush", "#0B1018");
            SetGradient(r, "AppBackgroundBrush", "#121A28", "#0E1520", "#080E16");
        }
        else
        {
            SetBrush(r, "InkBrush", "#0E1712");
            SetBrush(r, "InkSoftBrush", "#2A3A32");
            SetBrush(r, "MutedBrush", "#5C6F64");
            SetBrush(r, "AccentBrush", "#1A7A55");
            SetBrush(r, "AccentHoverBrush", "#146346");
            SetBrush(r, "DangerBrush", "#9B3B2E");
            SetBrush(r, "SurfaceBrush", Color.FromArgb(0xC6, 0xFF, 0xFF, 0xFF));
            SetBrush(r, "SurfaceStrongBrush", Color.FromArgb(0xC7, 0xFF, 0xFF, 0xFF));
            SetBrush(r, "LineBrush", Color.FromArgb(0x1A, 0x0E, 0x17, 0x12));
            SetBrush(r, "HeaderBrush", Color.FromArgb(0xB8, 0xE8, 0xF0, 0xEA));
            SetBrush(r, "ImageFallbackBrush", "#9EB5A6");
            SetBrush(r, "SecondaryHoverBrush", Color.FromArgb(0xE8, 0xFF, 0xFF, 0xFF));
            SetBrush(r, "PrimaryForegroundBrush", "#F4FFF9");
            SetBrush(r, "BgBaseBrush", "#D8E4DC");
            SetGradient(r, "AppBackgroundBrush", "#E8F0EA", "#D4E2D8", "#B9CEC0");
        }

        ThemeChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Toggle() => Apply(Current == AppTheme.Dark ? AppTheme.Light : AppTheme.Dark);

    private static void SetBrush(ResourceDictionary r, string key, string hex)
    {
        SetBrush(r, key, (Color)ColorConverter.ConvertFromString(hex)!);
    }

    private static void SetBrush(ResourceDictionary r, string key, Color color)
    {
        if (r[key] is SolidColorBrush existing && !existing.IsFrozen)
        {
            existing.Color = color;
            return;
        }

        r[key] = new SolidColorBrush(color);
    }

    private static void SetGradient(ResourceDictionary r, string key, string c0, string c1, string c2)
    {
        var brush = new LinearGradientBrush
        {
            StartPoint = new Point(0, 0),
            EndPoint = new Point(1, 1),
        };
        brush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString(c0)!, 0));
        brush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString(c1)!, 0.45));
        brush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString(c2)!, 1));
        r[key] = brush;
    }
}
