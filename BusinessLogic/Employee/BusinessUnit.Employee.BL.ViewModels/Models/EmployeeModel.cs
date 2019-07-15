using BusinessUnit.Core.BaseObjects;
using System;
using System.ComponentModel;

namespace BusinessUnit.Employee.BL.ViewModels.Models
{
    public class EmployeeModel : BaseEdit, IDisposable
    {
        private readonly Dal.Entities.Employee _entity;


        public EmployeeModel(Dal.Entities.Employee entity)
        {
            _entity = entity;
            PropertyChanged += EmployeeModel_PropertyChanged;
        }

        private void EmployeeModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(LastName):
                    _entity.LastName = LastName;
                    break;
                case nameof(FirstName):
                    _entity.FirstName = FirstName;
                    break;
                case nameof(Birthdate):
                    _entity.BirthDate = Birthdate;
                    break;
            }
        }

        public string LastName
        {
            get { return GetPropertyValue(() => LastName, () => _entity.LastName); }
            set { SetPropertyValue(() => LastName, value); }
        }

        public string FirstName
        {
            get { return GetPropertyValue(() => FirstName, () => _entity.FirstName); }
            set { SetPropertyValue(() => FirstName, value); }
        }

        public DateTime Birthdate
        {
            get { return GetPropertyValue(() => Birthdate, () => _entity.BirthDate); }
            set { SetPropertyValue(() => Birthdate, value); }
        }

        private void ReleaseResources()
        {
            PropertyChanged -= EmployeeModel_PropertyChanged;
        }

        public void Dispose()
        {
            ReleaseResources();
            GC.SuppressFinalize(this);
        }

    }
}
