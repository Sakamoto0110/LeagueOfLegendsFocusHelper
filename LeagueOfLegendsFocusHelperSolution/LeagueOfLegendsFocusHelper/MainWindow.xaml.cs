using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using LeagueOfLegendsFocusHelper.Options;
using LeagueOfLegendsFocusHelper.Properties;
using Color = System.Drawing.Color;
using TextBox = System.Windows.Controls.TextBox;
using Timer = System.Threading.Timer;

namespace LeagueOfLegendsFocusHelper
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        public static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

        //Settings.Default.PersistedTime, Settings.Default.PersistedState, Settings.Default.PersistedSound, Settings.Default.PersistedVolume        
        private OptionsList Options = new()
        {
            AutoPause = Settings.Default.PersistedAutoPause,
            IsActive = Settings.Default.PersistedState,
            SoundName = Settings.Default.PersistedSound,
            Time = Settings.Default.PersistedTime,
            Volume = Settings.Default.PersistedVolume
        };
        private Timer _Timer;

        private NotifyIcon NIcon;

        private List<string> SoundList { get; set; } = new() { "Refresh" };
        private MediaPlayer MPlayer = new MediaPlayer();
        private static readonly string GameWindowName = "League of Legends (TM) Client";
        private static readonly string SoundsPath = "Sounds/";
        private bool AllowClose = false;
        private bool _IsVisible1 = true;

        private ToolStripLabel StatusLabel;
        private static MainWindow _Self;
        private Dictionary<string, Action<object, EventRouterArgs>> CommandMap = new()
        {
            { "NOP" , (s,e)=>{}},
            { "UpdateOptions", (s,e)=>_Self.UpdateOptions() },
            { "ToggleVisibility", (s, e) => _Self.ToggleVisibility() },
            { "ToggleState", (s, e) => _Self.ToggleState() },
            { "RefreshSounds", (s, e) => _Self.RefreshSoundList() },
            { "Terminate", (s, e) => _Self.Terminate()}
        };

        public MainWindow()
        {
            PreLoad();
            InitializeComponent();
            
            _Timer = new Timer(Timer_OnTick, Options, Options.IsActive ? 1000 * Options.Time : -1, 1000 * Options.Time);
            
            VolumeSlider.Value = Options.Volume;
            TimeTextBox.Text = $"{Options.Time}";
            AutoPauseCheckbox.IsChecked = Options.AutoPause;
            AutoPauseCheckbox.Click += ToggleButton_OnChecked;
            TimeTextBox.TextChanged += TimeResultTextbox_OnTextChanged;
            MPlayer.MediaEnded += (sender, args) => MPlayer.Close();
            Options.OnOptionsChanged += (s, e) => MainWindowEventRouter(s, new((string)((OptionsList)s).Tag));
            NIcon = MakeNotifyIcon();
            RefreshSoundList();
            Options.RaiseUpdate();
        }

        private void PreLoad()
        {
            _Self = this;
            if (!Directory.Exists(SoundsPath))
                Directory.CreateDirectory(SoundsPath);
        }


        private void RefreshSoundList()
        {
            SoundList.Clear();
            foreach (var file in Directory.GetFiles(SoundsPath))
                SoundList.Add(file.Replace(SoundsPath, string.Empty));

            SoundList.Add("Refresh");
            SoundListComboBox.ItemsSource = null;
            SoundListComboBox.ItemsSource = SoundList;
            SoundListComboBox.SelectedIndex = SoundList.Count == 1 ? -1 : 0;
        }
        private void SaveOptions()
        {
            Settings.Default.PersistedState = Options.IsActive;
            Settings.Default.PersistedTime = Options.Time;
            Settings.Default.PersistedSound = Options.SoundName;
            Settings.Default.PersistedVolume = Options.Volume;
            Settings.Default.PersistedAutoPause = Options.AutoPause;
            Settings.Default.Save();
        }

        private void Timer_OnTick(object state)
        {
            var options = (OptionsList)state;
            bool gameIsOpen = FindWindowByCaption(IntPtr.Zero, GameWindowName) != IntPtr.Zero;

            if (options.SoundName != string.Empty)
            {
                if (Options.AutoPause)
                {
                    if(gameIsOpen)
                        MPlayer.Dispatcher.InvokeAsync(() =>
                        {
                            MPlayer.Open(new Uri($"{SoundsPath}{options.SoundName}", UriKind.Relative));
                            MPlayer.Volume = options.Volume / 100f;
                            MPlayer.Play();
                        });
                }
                else
                {
                    MPlayer.Dispatcher.InvokeAsync(() =>
                    {
                        MPlayer.Open(new Uri($"{SoundsPath}{options.SoundName}", UriKind.Relative));
                        MPlayer.Volume = options.Volume / 100f;
                        MPlayer.Play();
                    });
                }
            }
            
        }

        
      
        private void TimeResultTextbox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            
            if (Int32.TryParse(((TextBox)sender).Text, out var result) && (result >= 0 && result <= 180))
                Options.Time = result;
            else
                ((TextBox)sender).Text = "0";

        }
        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = !AllowClose;
            if (!AllowClose)
                Hide();
        }
        private void SoundListComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SoundListComboBox.SelectedIndex >= 0 )
                if ((string)SoundListComboBox.SelectedItem != "Refresh")
                    Options.SoundName = new string((string)SoundListComboBox.SelectedItem);
                else
                    RefreshSoundList();
        }
        private void VolumeSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Options.Volume = (int)e.NewValue;
        }




        private void UpdateOptions()
        {
            _Timer.Change(Options.IsActive ? Options.Time * 1000 : -1, Options.Time * 1000);

            StatusLabel.ForeColor = Options.IsActive ? Color.Red : Color.Black;
            StatusLabel.Text = Options.IsActive ? "Active" : "Inactive";
            StatusResultLabel.Foreground = Options.IsActive ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.Black);
            StatusResultLabel.Content = Options.IsActive ? "Active" : "Inactive";

            SaveOptions();
        }
        private void ToggleVisibility()
        {
            _IsVisible1 = !_IsVisible1;
            Visibility = _IsVisible1 ? Visibility.Visible : Visibility.Hidden;
        }
        private void ToggleState()
        {
            Options.IsActive = !Options.IsActive;
        }
        private void Terminate()
        {
            AllowClose = true;
            NIcon.Visible = false;
            Close();
        }
        


        public class EventRouterArgs
        {
            private string _cmd;

            [MemberNotNull]
            public string cmd
            {
                get => _cmd ?? "NOP";
                set => _cmd = value;

            }
            public object RARGS1;
            public object RARGS2;
            public object RARGS3;
            public object RARGS4;

            public EventRouterArgs(string str)
            {
                cmd = str;
            }

            public static implicit operator EventRouterArgs(string str)
            {
                return new EventRouterArgs(str);
            }

        }
        private void MainWindowEventRouter(object sender, EventRouterArgs e)
        {
            CommandMap[e.cmd](sender,e);
        }
        private void MainWindowButtonOnClickPreRouter(object sender, RoutedEventArgs e)
        {
            MainWindowEventRouter(sender, new EventRouterArgs((sender as System.Windows.Controls.Control)?.Tag as string));
        }

        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            Options.AutoPause = (bool)AutoPauseCheckbox.IsChecked;
        }
    }
}

