using System;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using System.Windows.Forms;

namespace DLC_Manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        public bool UseMods()
        {
            return Convert.ToBoolean(useModsSwitch.IsChecked);
        }

        private void settings_Click(object sender, RoutedEventArgs e)
        {
            Preferences preferences = new Preferences();
            preferences.ShowDialog();
        }

        public string[] GetCheckedDLCs()
        {
            List<string> DLCs = new List<string>();
            foreach (ToggleSwitch ts in FindVisualChildren<ToggleSwitch>(rightPanel))
            {
                if (ts.IsChecked == true)
                {
                    DLCs.Add(ts.Content.ToString());
                }
            }
            return DLCs.ToArray();
        }


        //Only call this once you're sure you have the right folder
        public void RevealUI()
        {
            rightPanel.Visibility = Visibility.Visible;
            leftPanel.Visibility = Visibility.Visible;
        }


        public void UpdateFolderPanel()
        {
            ToggleSwitch DLCSwitch;
            var preferences = new IniFile("preferences.ini");
            foreach (string folder in Directory.GetDirectories(DLC_XML.GetDLCPacks(preferences.Read("GamePath"), false)))
            {
                DLCSwitch = new MahApps.Metro.Controls.ToggleSwitch();
                DLCSwitch.Tag = System.IO.Path.GetFileName(folder);
                DLCSwitch.Content = System.IO.Path.GetFileName(folder);
                Thickness margin = DLCSwitch.Margin;
                margin.Top = 10;
                DLCSwitch.Margin = margin;
                rightPanel.Children.Add(DLCSwitch);
            }
        }

        public async void InformUser(string error)
        {
            userMessage.Visibility = Visibility.Visible;
            userMessage.Text = error;
            await PutTaskDelay();
            userMessage.Visibility = Visibility.Hidden;
        }

        //DO checks on load here
        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Utilities.InitialGamePathCheck();
            UpdateFolderPanel();
        }

        public void RefreshDisplay()
        {
            var preferences = new IniFile("preferences.ini");
            DLC_XML.GenerateDLCList(preferences.Read("GamePath"), "", UseMods());
            //DLCListDisplay.AppendText(File.ReadAllText("dlclist.xml"));
        }

        private void locateGameFolder_Click(object sender, RoutedEventArgs e)
        {
            Utilities.NewGamePath();
        }

        private void exportNow_Click(object sender, RoutedEventArgs e)
        {
            var preferences = new IniFile("preferences.ini");
            DLC_XML.GenerateDLCList(preferences.Read("GamePath"), "", UseMods());
            GetCheckedDLCs();
        }

        private void exportAs_Click(object sender, RoutedEventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var preferences = new IniFile("preferences.ini");
                    DLC_XML.GenerateDLCList(preferences.Read("GamePath"), dialog.SelectedPath, UseMods());
                }
            }
        }

        public async Task PutTaskDelay()
        {
            await Task.Delay(500);
        }

        private async void useModsSwitch_Click(object sender, RoutedEventArgs e)
        {
            var preferences = new IniFile("preferences.ini");
            if(!Directory.Exists(preferences.Read("GamePath") + @"\mods"))
            {
                InformUser("Mods folder was not found");
                await PutTaskDelay();
                useModsSwitch.IsChecked = false;
            }
        }

        private void revertVanilla_Click(object sender, RoutedEventArgs e)
        {
            Utilities.VanillaDLCList();
        }

        private void copyClipboard_Click(object sender, RoutedEventArgs e)
        {
            DLC_XML.CopyDLCToCLipboard();
        }
    }
}
