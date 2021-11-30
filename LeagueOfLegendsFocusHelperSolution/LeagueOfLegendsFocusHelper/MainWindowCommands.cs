using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Media;
using LeagueOfLegendsFocusHelper.Options;
using LeagueOfLegendsFocusHelper.Properties;
using Color = System.Drawing.Color;

namespace LeagueOfLegendsFocusHelper
{
    public partial class MainWindow : Window
    {

        

        //private void RefreshSoundList()
        //{
        //    SoundList.Clear();

        //    foreach (var file in Directory.GetFiles(SoundsPath))
        //    {
        //        SoundList.Add(file
        //            .Replace(SoundsPath, string.Empty)
        //        );
        //    }

        //    SoundList.Add("Refresh");
        //    SoundListComboBox.ItemsSource = null;
        //    SoundListComboBox.ItemsSource = SoundList;
        //    SoundListComboBox.SelectedIndex = SoundList.Count == 1 ? -1 : 0;
        //}

        //private void OptionsChanged(object sender, OptionsChangedEventArgs e)
        //{
            
        //    OptionsList opt = sender as OptionsList;
        //    if (opt == null)
        //    {
        //        Debug.WriteLine("aaaaaaaaaaaaaaaa");
        //    }
        //        _Timer.Change(Options.IsActive ? opt.Time * 1000 : -1, opt.Time * 1000);
        //    if (opt.IsActive)
        //    {
        //        StatusLabel.ForeColor = Color.Red;
        //        StatusLabel.Text = "Active";
        //        StatusResultLabel.Foreground = new SolidColorBrush(Colors.Red);
        //        StatusResultLabel.Content = "Active";
        //    }
        //    else
        //    {
        //        StatusLabel.ForeColor = Color.Black;
        //        StatusLabel.Text = "Not active";
        //        StatusResultLabel.Foreground = new SolidColorBrush(Colors.Black);
        //        StatusResultLabel.Content = "Inactive";
        //    }

        //    Settings.Default.PersistedState = Options.IsActive;
        //    Settings.Default.PersistedTime = Options.Time;
        //    Settings.Default.PersistedSound = Options.SoundName;
        //    Settings.Default.PersistedVolume = Options.Volume;
        //    Settings.Default.Save();

        //}
       
    }
}