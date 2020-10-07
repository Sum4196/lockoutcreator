using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace LockoutCreatorTestProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Check for database file upon launch.
            // TODO: add all necessary steps here to help build the program.
            if (File.Exists("C:\\Users\\mmendenh\\Desktop\\allEQFiles.xls"))
            {
                Console.WriteLine("File Exists.");
            }
        }
    }
}
