using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLC_Manager
{
    class DLC_XML
    {
        //Class written by nova
        //Used to generate dlclist.xml for Grand Theft Auto V (update.rpf\common\data\dlclist.xml)

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


        
        public static void GenerateDLCList(string gamePath, string outputPath, bool useModFolder)
        {
            string exportPath;
            if (outputPath == "")
            {
                exportPath = "dlclist.xml";
            }
            else
            {
                exportPath = outputPath + @"\dlclist.xml";
            }
            string DLCPacks = GetDLCPacks(gamePath, useModFolder);
            string top = @"<?xml version=""1.0"" encoding=""UTF - 8""?>" + Environment.NewLine + "" + Environment.NewLine + "<SMandatoryPacksData>" + Environment.NewLine + "	<Paths>" + Environment.NewLine;
            string bottom = "	</Paths>" + Environment.NewLine + "</SMandatoryPacksData>";
            File.WriteAllText(exportPath, top, Encoding.UTF8);
            if(Directory.GetDirectories(DLCPacks).Length > 15)
            {
                System.Windows.Forms.MessageBox.Show("DLC's exceed 15, game crashes during loading may be related to this depending on your game config");
            }
            foreach(string folder in Directory.GetDirectories(DLCPacks))
            {
                File.AppendAllText(exportPath, @"		<Item>dlcpacks:\" + Path.GetFileName(folder) + @"\</Item>" + Environment.NewLine);
            }

            File.AppendAllText(exportPath, bottom);
            
        }

        public static void CopyDLCToCLipboard()
        {
            var preferences = new IniFile("preferences.ini");
            MainWindow mw = new MainWindow();
            GenerateDLCList(preferences.Read("GamePath"), "", mw.UseMods());
            System.Windows.Forms.Clipboard.SetText(File.ReadAllText("dlclist.xml"));
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
