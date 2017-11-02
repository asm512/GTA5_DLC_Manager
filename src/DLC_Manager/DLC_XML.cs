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
                return path + @"\mods\update\dlcpacks\";
            }
            else
            {
                return path + @"\update\dlcpacks\";
            }
        }

        public static void GenerateDLCList(string[] DLCs, bool useModFolder, string outputPath = "dlclist.xml")
        {
            string top = @"<?xml version=""1.0"" encoding=""UTF - 8""?>" + Environment.NewLine + "" + Environment.NewLine + "<SMandatoryPacksData>" + Environment.NewLine + "	<Paths>" + Environment.NewLine;
            string bottom = "	</Paths>" + Environment.NewLine + "</SMandatoryPacksData>";
            File.WriteAllText(outputPath, top, Encoding.UTF8);
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

            File.AppendAllText(outputPath, bottom);
            
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
