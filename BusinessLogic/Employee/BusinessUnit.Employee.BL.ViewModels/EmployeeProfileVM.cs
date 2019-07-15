using System.ComponentModel;
using System.Composition;
using System.Windows.Forms;
using BusinessUnit.Core.BaseObjects;
using BusinessUnit.Core.Services;
using BusinessUnit.Employee.BL.ViewModels.Models;
using BusinessUnit.Employee.Dal.Repository;

namespace BusinessUnit.Employee.BL.ViewModels
{
    [Export(typeof(EmployeeProfileVM))]
    public class EmployeeProfileVM: BaseEdit
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMessageService _messageService;

        private Dal.Entities.Employee _currentEmployee;

        [ImportingConstructor]
        public EmployeeProfileVM(IEmployeeRepository employeeRepository, IMessageService messageService)
        {
            _employeeRepository = employeeRepository;
            _messageService = messageService;
        }

        public EmployeeModel Employee
        {
            get { return GetPropertyValue(() => Employee ); }
            set { SetPropertyValue(()=> Employee,value); }
        }

        public int EmployeeId
        {
            get { return GetPropertyValue(() => EmployeeId ); }
            set
            {
                PromptAndSave(out var cancel);
                if(!cancel)
                {
                    if(IsValueChanged(() => EmployeeId, value))
                    {
                        SetPropertyValue(()=> EmployeeId,value);
                        GetEmployee();
                    }
                }
            }
        }

        private void GetEmployee()
        {
            Employee = null;
            _currentEmployee = _employeeRepository.Get(EmployeeId);
            if(_currentEmployee != null)
            {
                var model = new EmployeeModel(_currentEmployee);
                Employee = model;
            }
            
        }

        public bool PromptAndSave(out bool cancel)
        {
            cancel = false;
            if(IsItemChanged())
            {
                var result = _messageService.Show("You have some uncommitted changes.\nWould you like to save these changes?", "Unsaved Changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button3);

                switch(result)
                {
                    case DialogResult.Yes:
                        if(!Save())
                        {
                            cancel = true;
                        }
                        break;

                    case DialogResult.No:
                        break;

                    case DialogResult.Cancel:
                        cancel = true;
                        break;
                }
            }

            return cancel;
        }
        public bool IsItemChanged()
        {
            //TODO this won't work
            return Employee != null && Employee.IsDirty;
        }

        public bool Save()
        {
            if(IsItemChanged())
            {
                //Validate();
                //if(!IsValid())
                //{
                //    _messageService.Show("Loaded Compartments are not valid. Cannot be saved.", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button3);
                //    return false;
                //}

                //if(!_freightByCompartmentRepository.Update(_freightByCompartmentEntities))
                //{
                //    _messageService.Show("Error occurred during save.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button3);
                //    return false;
                //}

                // save was successful, reload freight and compartments
                //Load();

                //todo try-catch
                _employeeRepository.Update(_currentEmployee);
            }
            return true;
        }
    }
}
