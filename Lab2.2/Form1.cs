using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Lab2._2
{
    public partial class Form1 : Form
    {
        private const string PluginsPath = "./Plugins";

        private static bool PluginsDirExists() => Directory.Exists(PluginsPath);
        private static string[] GetDllPathFiles() => Directory.GetFiles(PluginsPath, "*.dll", SearchOption.TopDirectoryOnly);

        private readonly List<NativeMethods> methods = new List<NativeMethods>();

        public Form1()
        {
            InitializeComponent();
        }

        private void LoadDll()
        {
            if (!PluginsDirExists())
                return;

            foreach (string path in GetDllPathFiles())
            {
                try
                {
                    this.methods.Add(new NativeMethods(path));
                }
                catch
                {
                }
            }
        }

        private void DrawGraph(Func<string> getName, Func<double, double> getPointY, double x0, double x1, double step = .01)
        {
            var series = this.chart1.Series[0];

            series.LegendText = getName();
            series.Points.Clear();
            for (double x = x0; x <= x1; x += step)
                series.Points.AddXY(x, getPointY(x));
        }

        private void SelectFunctionAndDraw()
        {
            Form2 form2 = new Form2(this.methods);

            if (form2.ShowDialog() != DialogResult.OK)
            {
                Environment.Exit(0);
            }

            this.DrawGraph(form2.FuncName, form2.TheFunc, 0, 10);
        }

        private void CheckMethodsAndShowGraphic()
        {
            switch (methods.Count)
            {
                case 0:
                    Environment.Exit(0);
                    break;
                case 1:
                    this.DrawGraph(methods[0].FuncName, methods[0].TheFunc, 0, 10);
                    break;
                default:
                    this.SelectFunctionAndDraw();
                    break;
            }
        }

        private void Init()
        {
            this.LoadDll();
            this.CheckMethodsAndShowGraphic();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            this.Init();
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }
    }
}
