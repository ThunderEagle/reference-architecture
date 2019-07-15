using BusinessUnit.Employee.BL.ViewModels;
using System;
using System.Composition;
using System.Windows.Forms;

namespace BusinessUnit.Employee.UI
{
    [Export(typeof(EmployeeProfile))]
    public partial class EmployeeProfile : Form
    {
        [ImportingConstructor]
        public EmployeeProfile(EmployeeProfileVM viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            DataContext.DataSource = ViewModel;
            DataContext.ResetBindings(false);
            button1.Click += Button1_Click;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            ViewModel?.Save();
        }

        private EmployeeProfileVM ViewModel { get; }

        public EmployeeProfile()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                button1.Click -= Button1_Click;
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
