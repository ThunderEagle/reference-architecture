
using BusinessUnit.Core.IoC;
using BusinessUnit.Employee.UI;
using System;
using System.Composition;
using System.Windows.Forms;

namespace BusinessUnit.Application.Profiles
{
    [Export(typeof(MainForm))]
    public partial class MainForm : Form
    {
        [ImportingConstructor]
        public MainForm(EmployeeProfile employeeProfile)
        {
            InitializeComponent();
            EmployeeProfileForm = employeeProfile;
            exitToolStripMenuItem.Click += ExitToolStripMenuItem_Click;
            employeeProfileToolStripMenuItem.Click += EmployeeProfileToolStripMenuItem_Click;
        }

        private EmployeeProfile EmployeeProfileForm { get; }

        private void EmployeeProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EmployeeProfileForm.MdiParent = this;
            EmployeeProfileForm.Show();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }


        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                exitToolStripMenuItem.Click -= ExitToolStripMenuItem_Click;
                employeeProfileToolStripMenuItem.Click -= EmployeeProfileToolStripMenuItem_Click;
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
