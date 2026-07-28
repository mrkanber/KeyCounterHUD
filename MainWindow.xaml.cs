using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using KeyCounterHUD.Models;
using KeyCounterHUD.Services;
using Color = System.Windows.Media.Color;
using ColorConverter = System.Windows.Media.ColorConverter;
using SolidColorBrush = System.Windows.Media.SolidColorBrush;

namespace KeyCounterHUD;

public partial class MainWindow : Window
{
    private readonly AppConfig _config;
    private readonly KeyboardHook _hook = new();
    private readonly DispatcherTimer _saveTimer;
    private System.Windows.Forms.NotifyIcon? _trayIcon;
    private long _sessionCountAtLastSave;

    public MainWindow()
    {
        InitializeComponent();

        _config = AppConfig.Load();
        Left = _config.Left;
        Top = _config.Top;
        _sessionCountAtLastSave = _config.TotalCount;

        UpdateCountText();
        ApplyColor(_config.TextColor);
        AutoStartMenuItem.IsChecked = AutoStart.IsEnabled();

        _hook.KeyPressed += OnKeyPressed;
        _hook.Start();

        _saveTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(5) };
        _saveTimer.Tick += (_, _) => SaveIfChanged();
        _saveTimer.Start();

        CreateTrayIcon();

        Closing += (_, _) =>
        {
            SaveIfChanged();
            _hook.Dispose();
            _trayIcon?.Dispose();
        };
    }

    private void OnKeyPressed()
    {
        _config.TotalCount++;
        UpdateCountText();
    }

    private void UpdateCountText()
    {
        CountText.Text = _config.TotalCount.ToString("N0");
    }

    private void SaveIfChanged()
    {
        if (_config.TotalCount == _sessionCountAtLastSave) return;

        _config.Left = Left;
        _config.Top = Top;
        _config.Save();
        _sessionCountAtLastSave = _config.TotalCount;
    }

    private void CreateTrayIcon()
    {
        _trayIcon = new System.Windows.Forms.NotifyIcon
        {
            Icon = System.Drawing.SystemIcons.Application,
            Visible = true,
            Text = "KeyCounterHUD",
        };

        var menu = new System.Windows.Forms.ContextMenuStrip();
        menu.Items.Add("Sıfırla", null, (_, _) => ResetCount());

        var autoStartItem = new System.Windows.Forms.ToolStripMenuItem("Başlangıçta Aç")
        {
            CheckOnClick = true,
            Checked = AutoStart.IsEnabled(),
        };
        autoStartItem.CheckedChanged += (_, _) =>
        {
            if (autoStartItem.Checked) AutoStart.Enable();
            else AutoStart.Disable();
        };
        menu.Items.Add(autoStartItem);

        menu.Items.Add(new System.Windows.Forms.ToolStripSeparator());
        menu.Items.Add("Çıkış", null, (_, _) => System.Windows.Application.Current.Shutdown());

        _trayIcon.ContextMenuStrip = menu;
    }

    private void ResetCount()
    {
        _config.TotalCount = 0;
        _sessionCountAtLastSave = 0;
        UpdateCountText();
        _config.Save();
    }

    private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        DragMove();
    }

    private void ApplyColor(string hex)
    {
        try
        {
            CountText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hex));
        }
        catch
        {
            CountText.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        }
    }

    private void ColorMenuItem_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not System.Windows.Controls.MenuItem { Tag: string hex }) return;

        _config.TextColor = hex;
        ApplyColor(hex);
        _config.Save();
    }

    private void ResetMenuItem_Click(object sender, RoutedEventArgs e) => ResetCount();

    private void AutoStartMenuItem_Click(object sender, RoutedEventArgs e)
    {
        if (AutoStartMenuItem.IsChecked) AutoStart.Enable();
        else AutoStart.Disable();
    }

    private void ExitMenuItem_Click(object sender, RoutedEventArgs e) => System.Windows.Application.Current.Shutdown();
}
