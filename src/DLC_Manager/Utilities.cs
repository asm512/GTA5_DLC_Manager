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
using System.Reflection;

namespace DLC_Manager
{
    class Utilities
    {
        public static void NewGamePath()
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.ShowNewFolderButton = false;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var preferences = new IniFile("preferences.ini");
                    preferences.Write("GamePath", dialog.SelectedPath.ToString());
                    if (CheckValidityPath(dialog.SelectedPath))
                    {
                        MainWindow mw = new MainWindow();
                        mw.UpdateFolderPanel(false);
                    }
                    else
                    {
                        NewGamePath();
                    }
                }
            }
        }

        public static void NotifyInvalidPath()
        {
            System.Windows.Forms.MessageBox.Show("The path selected was invalid, path must be set to root game dir");
        }

        public static bool CheckValidityPath(string path)
        {
            if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path);
                foreach (string file in files)
                {
                    if (file.ToLower().Contains("gta5.exe"))
                    {
                        return true;
                    }
                    else
                    {
                        NotifyInvalidPath();
                        return false;
                    }
                }
                NotifyInvalidPath();
                return false;
            }
            else
            {
                NotifyInvalidPath();
                return false;
            }
        }

        public static void InitialGamePathCheck()
        {
            var preferences = new IniFile("preferences.ini");
            if (preferences.KeyExists("GamePath"))
            {
                if (CheckValidityPath(preferences.Read("GamePath"))==true)
                {
                    MainWindow mw = new MainWindow();
                    mw.RevealUI();
                }
                else
                {
                    NewGamePath();
                }
            }
            else
            {
                NewGamePath();
            }
        }

        public static void VanillaDLCList()
        {
            if (File.Exists("dlclist.xml"))
            {
                File.Delete("dlclist.xml");
            }
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("DLC_Manager.resources.dlclist.txt");
            FileStream fileStream = new FileStream("dlclist.xml", FileMode.CreateNew);
            for (int i = 0; i < stream.Length; i++)
                fileStream.WriteByte((byte)stream.ReadByte());
            fileStream.Close();
        }



    }
}
