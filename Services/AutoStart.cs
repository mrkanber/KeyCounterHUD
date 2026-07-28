using System.Diagnostics;
using Microsoft.Win32;

namespace KeyCounterHUD.Services;

public static class AutoStart
{
    private const string RunKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Run";
    private const string ValueName = "KeyCounterHUD";

    private static string? ExePath => Process.GetCurrentProcess().MainModule?.FileName;

    public static bool IsEnabled()
    {
        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(RunKeyPath, writable: false);
            var current = key?.GetValue(ValueName) as string;
            return ExePath is not null && current == $"\"{ExePath}\"";
        }
        catch
        {
            return false;
        }
    }

    public static void Enable()
    {
        try
        {
            var exePath = ExePath;
            if (string.IsNullOrEmpty(exePath)) return;

            using var key = Registry.CurrentUser.OpenSubKey(RunKeyPath, writable: true)
                ?? Registry.CurrentUser.CreateSubKey(RunKeyPath);
            key?.SetValue(ValueName, $"\"{exePath}\"");
        }
        catch
        {
            // autostart kritik değil, sessiz geç
        }
    }

    public static void Disable()
    {
        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(RunKeyPath, writable: true);
            key?.DeleteValue(ValueName, throwOnMissingValue: false);
        }
        catch
        {
        }
    }
}
