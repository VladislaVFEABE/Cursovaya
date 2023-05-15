using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestReload
{
    public partial class Oblojka : Form
    {
        public Oblojka()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Reg register = new Reg();
            this.Hide();
            register.ShowDialog();
            this.Show();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Loginka log = new Loginka();
            this.Hide();
            log.ShowDialog();
            this.Show();
        }
    }
}
