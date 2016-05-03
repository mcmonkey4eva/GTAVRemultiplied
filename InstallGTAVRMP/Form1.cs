using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace InstallGTAVRMP
{
    public partial class Form1 : Form
    {
        string edir = Environment.CurrentDirectory;

        const string file_dinput8 = "/dinput8.dll";

        const string file_scripthookv = "/ScriptHookV.dll";

        const string file_scripthookvdotnet = "/ScriptHookVDotNet.asi";

        const string file_freneticscriptdll = "/FreneticScript.dll";

        const string file_freneticscriptpdb = "/FreneticScript.pdb";

        const string file_yamldotnetdll = "/YamlDotNet.dll";

        const string file_yamldotnetpdb = "/YamlDotNet.pdb";

        const string file_gtavrmpdll = "/GTAVRemultiplied.dll";

        const string file_gtavrmppdb = "/GTAVRemultiplied.pdb";


        string[] downloadFiles = new string[]
        {
            file_dinput8,
            file_scripthookv,
        };
        
        string[] coreFiles = new string[]
        {
            file_scripthookvdotnet,
            file_freneticscriptdll,
            file_freneticscriptpdb,
            file_yamldotnetdll,
            file_yamldotnetpdb,
            file_gtavrmpdll,
            file_gtavrmppdb
        };

        string[] allFiles = new string[]
        {
            file_dinput8,
            file_scripthookv,
            file_scripthookvdotnet,
            file_freneticscriptdll,
            file_freneticscriptpdb,
            file_yamldotnetdll,
            file_yamldotnetpdb,
            file_gtavrmpdll,
            file_gtavrmppdb
        };

        string[] mainDirFiles = new string[]
        {
            file_dinput8,
            file_scripthookv,
            file_scripthookvdotnet,
        };

        string[] scriptsFolderFiles = new string[]
        {
            file_freneticscriptdll,
            file_freneticscriptpdb,
            file_yamldotnetdll,
            file_yamldotnetpdb,
            file_gtavrmpdll,
            file_gtavrmppdb
        };

        const string folder_scripts = "/scripts";

        const string folder_serverscripts = "/frenetic/server/scripts";

        const string folder_clientscripts = "/frenetic/client/scripts";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (string str in coreFiles)
            {
                if (!File.Exists(edir + str))
                {
                    MessageBox.Show("Missing required release file: " + str, "GTAV RMP Installer - Error");
                    Close();
                    return;
                }
            }
            foreach (string str in downloadFiles)
            {
                if (!File.Exists(edir + str))
                {
                    MessageBox.Show("Missing ScriptHookV file: " + str + Environment.NewLine
                        + "You can acquire that here: http://www.dev-c.com/gtav/scripthookv/ "+ Environment.NewLine
                        +"Just put the contents of the download in the installer's directory, and restart this installer.", "GTAV RMP Installer - Info");
                    Close();
                    return;
                }
            }
            CheckButtonLabel();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        bool mode = false;

        void CheckButtonLabel()
        {
            string GTAV = textBox1.Text;
            if (!File.Exists(GTAV) || !GTAV.ToLower().EndsWith("gta5.exe"))
            {
                button2.Enabled = false;
                return;
            }
            button2.Enabled = true;
            string folder = Path.GetDirectoryName(GTAV);
            if (File.Exists(folder + file_scripthookv))
            {
                button2.Text = "Remove GTAV RMP";
                mode = false;
            }
            else
            {
                button2.Text = "Install GTAV RMP";
                mode = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Find Your GTAV.exe file!";
            if (ofd.ShowDialog() != DialogResult.Cancel)
            {
                textBox1.Text = ofd.FileName;
                CheckButtonLabel();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string GTAV = textBox1.Text;
            string folder = Path.GetDirectoryName(GTAV);
            if (mode)
            {
                Directory.CreateDirectory(folder + folder_scripts);
                Directory.CreateDirectory(folder + folder_clientscripts);
                Directory.CreateDirectory(folder + folder_serverscripts);
                foreach (string basefile in mainDirFiles)
                {
                    File.Copy(Environment.CurrentDirectory + basefile, folder + basefile);
                }
                foreach (string basefile in scriptsFolderFiles)
                {
                    if (File.Exists(folder + "/scripts/" + basefile))
                    {
                        File.Delete(folder + "/scripts/" + basefile);
                    }
                    File.Copy(Environment.CurrentDirectory + basefile, folder + "/scripts/" + basefile);
                }
            }
            else
            {
                foreach (string basefile in mainDirFiles)
                {
                    File.Delete(folder + basefile);
                }
            }
            CheckButtonLabel();
        }
    }
}
