using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls;

namespace DLC_Manager
{
    /// <summary>
    /// Interaction logic for preferences.xaml
    /// </summary>
    public partial class Preferences : MetroWindow
    {
        public void SaveSettings()
        {
            var preferences = new IniFile("preferences.ini");
            if (autoCopy.IsChecked==true)
            {
                preferences.Write("AutoCopy", "true");
            }
            else
            {
                preferences.Write("AutoCopy", "false");
            }
            if (autoExport.IsChecked == true)
            {
                preferences.Write("AutoExport", "true");
            }
            else
            {
                preferences.Write("AutoExport", "false");
            }
            if (issueWarning.IsChecked == true)
            {
                preferences.Write("IssueLimitWarning", "true");
            }
            else
            {
                preferences.Write("IssueLimitWarning", "false");
            }
        }

        public void LoadSettings()
        {
            var preferences = new IniFile("preferences.ini");
            if (preferences.Read("AutoCopy") == "true")
            {
                autoCopy.IsChecked = true;
            }
            else
            {
                autoCopy.IsChecked = false;
            }
            if (preferences.Read("AutoExport") == "true")
            {
                autoExport.IsChecked = true;
            }
            else
            {
                autoExport.IsChecked = false;
            }
            if (preferences.Read("IssueLimitWarning") == "true")
            {
                issueWarning.IsChecked = true;
            }
            else
            {
                issueWarning.IsChecked = false;
            }

        }

        public Preferences()
        {
            InitializeComponent();
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            SaveSettings();
            this.Close();
        }

        private void mahapps_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("http://mahapps.com/");
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadSettings();
            preferencesText.Text = "Designed using MahApps for XAML" + Environment.NewLine + "Background image by Razed using NaturalVision Remastered";
        }
    }
}
