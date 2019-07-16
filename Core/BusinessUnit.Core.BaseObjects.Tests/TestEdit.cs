using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessUnit.Core.BaseObjects.Tests
{
    class TestEdit:BaseEdit
    {
        public string PropertyA
        {
            get { return GetPropertyValue(() => PropertyA); }
            set { SetPropertyValue(() => PropertyA, value); }
        }

        [DependsUpon("PropertyA")]
        public virtual void DependantMethod()
        {
            MethodWasCalled = true;
        }

        public bool MethodWasCalled { get; private set; } = false;

        [DependsUpon("PropertyA")]
        public bool PropertyB
        {
            get { return true; }
        }

        public void CallSetDirty()
        {
            base.SetDirty();
        }

        public void SetPropertyADirty()
        {
            SetDirty(() => PropertyA);
        }
    }
}
