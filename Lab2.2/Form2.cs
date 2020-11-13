using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Lab2._2
{
    public partial class Form2 : Form
    {
        private readonly List<NativeMethods> nativeMethods;

        public Func<string> FuncName = null;
        public Func<double, double> TheFunc = null;

        public Form2(List<NativeMethods> nativeMethods)
        {
            InitializeComponent();

            this.nativeMethods = nativeMethods;

            foreach (var method in this.nativeMethods)
                comboBox1.Items.Add(method.FuncName());
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            var nameMethod = comboBox1.Text;
            foreach (var method in this.nativeMethods)
            {
                if (nameMethod == method.FuncName())
                {
                    DialogResult = DialogResult.OK;
                    FuncName = method.FuncName;
                    TheFunc = method.TheFunc;
                    return;
                }
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
