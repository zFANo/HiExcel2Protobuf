using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HiProtobuf.Lib;
using HiFramework.Log;

namespace HiProtobuf.UI
{
    public partial class HiProtobuf : Form
    {
        private ExportSetting _exportSetting;
        public HiProtobuf()
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(Settings.Export_Folder)) textBox1.Text = Settings.Export_Folder;
            if (!string.IsNullOrEmpty(Settings.Excel_Folder)) textBox2.Text = Settings.Excel_Folder;
            if (!string.IsNullOrEmpty(Settings.Compiler_Path)) textBox5.Text = Settings.Compiler_Path;
            Log.OnInfo += (x) =>
            {
                textBox6.Text = Logger.Log;
            };
            Log.OnWarning += (x) =>
            {
                textBox6.Text = Logger.Log;
            };
            Log.OnError += (x) =>
            {
                textBox6.Text = Logger.Log;
            };

            _exportSetting = ExportSetting.Instance;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            checkBox1.DataBindings.Add("Checked", _exportSetting, "ExportCs");
            checkBox2.DataBindings.Add("Checked", _exportSetting, "ExportCpp");
            checkBox3.DataBindings.Add("Checked", _exportSetting, "ExportGo");
            checkBox4.DataBindings.Add("Checked", _exportSetting, "ExportJava");
            checkBox5.DataBindings.Add("Checked", _exportSetting, "ExportPython");
            checkBox6.DataBindings.Add("Checked", _exportSetting, "ExportData");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = dialog.SelectedPath;
                Settings.Export_Folder = textBox1.Text;
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Settings.Export_Folder = textBox1.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = dialog.SelectedPath;
                Settings.Excel_Folder = textBox2.Text;
            }
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            Settings.Excel_Folder = textBox2.Text;
        }


        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "csc(*.exe)|*.exe";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBox5.Text = dialog.FileName;
                Settings.Compiler_Path = textBox5.Text;
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            Settings.Compiler_Path = textBox5.Text;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Log.Info("开始导出");
            Manager.Export();
            Log.Info("导出结束");
            Config.Save();
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            textBox6.SelectionStart = textBox6.TextLength;
            textBox6.ScrollToCaret();
        }

        // private void checkBox1_CheckedChanged(object sender, EventArgs e)
        // {
        //     _exportSetting.ExportCs = !_exportSetting.ExportCs;
        // }
        //
        // private void checkBox2_CheckedChanged(object sender, EventArgs e)
        // {
        //     _exportSetting.ExportCpp = !_exportSetting.ExportCpp;
        // }
        //
        // private void checkBox3_CheckedChanged(object sender, EventArgs e)
        // {
        //     _exportSetting.ExportGo = !_exportSetting.ExportGo;
        // }
        //
        // private void checkBox4_CheckedChanged(object sender, EventArgs e)
        // {
        //     _exportSetting.ExportJava = !_exportSetting.ExportJava;
        // }
        //
        // private void checkBox5_CheckedChanged(object sender, EventArgs e)
        // {
        //     _exportSetting.ExportPython = !_exportSetting.ExportPython;
        // }
        //
        // private void checkBox6_CheckedChanged(object sender, EventArgs e)
        // {
        //     _exportSetting.ExportData = !_exportSetting.ExportData;
        // }
    }
}
