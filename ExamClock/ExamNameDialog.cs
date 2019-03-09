using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExamClock
{
    public partial class ExamNameDialog : Form
    {
        public ExamNameDialog()
        {
            InitializeComponent();
        }
        private dynamic settings = Properties.Settings.Default;
        private void Form2_Load(object sender, EventArgs e)
        {
            ExamNametxtBox.Text = settings.ExamName;
        }
        /// <summary>
        /// Save
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ExamName = ExamNametxtBox.Text;
            this.Close();
            
        }
    }
}
