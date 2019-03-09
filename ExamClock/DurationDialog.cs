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
    public partial class DurationDialog : Form
    {
        public DurationDialog()
        {
            InitializeComponent();
        }
        private dynamic settings = Properties.Settings.Default;
        private void Form2_Load(object sender, EventArgs e)
        {
            DurationtxtBox.Text = settings.Duration.ToString();
        }
        /// <summary>
        /// Save
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Duration = TimeSpan.Parse(DurationtxtBox.Text);
            this.Close();
            
        }
    }
}
