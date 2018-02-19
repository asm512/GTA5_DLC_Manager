using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DLC_Manager
{
    class DLC_XML
    {
        //Library for creating Rage files for cpp and c# available at on my Github.
        //This class is used to generate dlclist.xml for Grand Theft Auto V (update.rpf\common\data\dlclist.xml)

        public static string GetDLCPacks(string path, bool useModFolder)
        {
            if (useModFolder)
            {
                return path + @"\mods\update\x64\dlcpacks\";
            }
            else
            {
                return path + @"\update\x64\dlcpacks\";
            }
        }

        
        // i cannot be bothered to change this to a string builder. 
        public static void GenerateDLCList(string[] DLCs, bool useModFolder, string outputPath = "dlclist.xml", bool now = false)
        {
            if (now)
            {
                var preferences = new IniFile("preferences.ini");
                if (preferences.Read("ExportToCurrentDir") == "true")
                {

                }
                else
                {
                    outputPath = preferences.Read("GamePath");
                }
            }
            string top = @"<?xml version=""1.0"" encoding=""UTF-8""?>" + Environment.NewLine + "" + Environment.NewLine + "<SMandatoryPacksData>" + Environment.NewLine + "	<Paths>" + Environment.NewLine;
            string platforms = @"  		<Item>platform:\dlcPacks\mpBeach\</Item>" + Environment.NewLine + @"  		<Item>platform:\dlcPacks\mpBeach\</Item>" + Environment.NewLine + @"		<Item>platform:\dlcPacks\mpBusiness\</Item>" + Environment.NewLine + @"		<Item>platform:\dlcPacks\mpChristmas\</Item>" + Environment.NewLine + @"		<Item>platform:\dlcPacks\mpValentines\</Item>" + Environment.NewLine + @"		<Item>platform:\dlcPacks\mpBusiness2\</Item>" + Environment.NewLine + @"		<Item>platform:\dlcPacks\mpHipster\</Item>" + Environment.NewLine + @"		<Item>platform:\dlcPacks\mpIndependence\</Item>" + Environment.NewLine + @"		<Item>platform:\dlcPacks\mpPilot\</Item>" + Environment.NewLine + @"		<Item>platform:\dlcPacks\spUpgrade\</Item>" + Environment.NewLine + @"		<Item>platform:\dlcPacks\mpLTS\</Item>" + Environment.NewLine;
            string bottom = "	</Paths>" + Environment.NewLine + "</SMandatoryPacksData>";
            File.WriteAllText(outputPath, top, Encoding.UTF8);
            File.AppendAllText(outputPath, platforms, Encoding.UTF8);
            if(DLCs.Length > 15)
            {
                MessageBox.Show("DLC Limit exceeds 15, crashes at strartup may be related to this",
                "Possible DLC limit reached",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            }
            foreach(string DLC in DLCs)
            {
                File.AppendAllText(outputPath, @"		<Item>dlcpacks:\" + DLC + @"\</Item>" + Environment.NewLine);
            }

            File.AppendAllText(outputPath, bottom, Encoding.UTF8);
            
        }

        public static void CopyDLCToCLipboard(string[] DLCs)
        {
            var preferences = new IniFile("preferences.ini");
            MainWindow mw = new MainWindow();
            GenerateDLCList(DLCs, useModFolder:mw.UseMods());
            Clipboard.SetText(File.ReadAllText("dlclist.xml"));
            CleanUpDLC();
        }

        public static void CleanUpDLC()
        {
            if (File.Exists("dlclist.xml"))
            {
                File.Delete("dlclist.xml");
            }
        }






    }
}
