using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace BusinessUnit.Core.BaseObjects.Tests
{
    [TestFixture]
    [Category("DeveloperUnitTest")]
    class BaseEditTests
    {
        [Test]
        public void GetPropertyValue_PropertyNotSet_DefaultReturned()
        {
            var sut = new TestEdit();
            var expected = default(string);
            Assert.AreEqual(expected, sut.PropertyA);
        }

        [Test]
        public void GetPropertyValue_PropertySet_PropertyReturned()
        {
            var expected = "foo";
            var sut = new TestEdit();
            sut.PropertyA = expected;
            Assert.AreEqual(expected, sut.PropertyA);
        }

        [Test]
        public void IsDirty_SuppressChangesFalse_DirtyTrue()
        {
            var sut = new TestEdit();
            sut.SuppressPropertyChanges = false;
            sut.PropertyA = "foo";
            Assert.IsTrue(sut.IsDirty);
        }

        [Test]
        public void IsDirty_SuppressChangesTrue_DirtyFalse()
        {
            var sut = new TestEdit();
            sut.SuppressPropertyChanges = true;
            sut.PropertyA = "foo";
            Assert.IsFalse(sut.IsDirty);    
        }

        [Test]
        public void ResetDirty_SutIsDirty_IsDirtyFalse()
        {
            var sut = new TestEdit();
            sut.PropertyA = "foo";
            Assert.IsTrue(sut.IsDirty);
            sut.ResetDirty();
            Assert.IsFalse(sut.IsDirty);
        }

        [Test]
        public void SetDirty_NoValuesChangedSetDirty_IsDirtyTrue()
        {
            var sut = new TestEdit();
            Assert.IsFalse(sut.IsDirty);
            sut.CallSetDirty();
            Assert.IsTrue(sut.IsDirty);
            
        }

        [Test]
        public void SetDirty_PropertyAlreadyTracked_IsDirtyTrue()
        {
            var sut = new TestEdit();
            sut.SetPropertyADirty();
            sut.SetPropertyADirty();
            Assert.IsTrue(sut.IsPropertyDirty(()=>sut.PropertyA));

        }

        [Test]
        public void IsPropertyDirty_IsDirty_True()
        {
            var sut = new TestEdit();
            sut.PropertyA = "Foo";
            Assert.IsTrue(sut.IsPropertyDirty(()=>sut.PropertyA));
        }

        [Test]
        public void IsPropertyDirty_IsNotDirty_False()
        {
            var sut = new TestEdit();
            Assert.IsFalse(sut.IsPropertyDirty(()=>sut.PropertyA));
        }

        [Test]
        public void DependantUpon_PropertyChanges_DependantMethodCalled()
        {
            var sut = new TestEdit();
            sut.PropertyA = "foo";
            Assert.IsTrue(sut.MethodWasCalled);
        }

        [Test]
        public void PropertyChanged_SuppressChangesFalse_PropertyChangedCalled()
        {
            var sut = new TestEdit();
            bool propertyChangedForPropertyARaised = false;
            sut.PropertyChanged += (o, e) =>
                                   {
                                       if(e.PropertyName == "PropertyA")
                                       {
                                           propertyChangedForPropertyARaised = true;
                                       }
                                   };
            sut.PropertyA = "foo";
            Assert.IsTrue(propertyChangedForPropertyARaised);
        }

        [Test]
        public void PropertyChanged_SuppressChangesTrue_PropertyChangedNotCalled()
        {
            var sut = new TestEdit();
            sut.SuppressPropertyChanges = true;
            bool propertyChangedForPropertyARaised = false;
            sut.PropertyChanged += (o, e) =>
                                   {
                                       if(e.PropertyName == "PropertyA")
                                       {
                                           propertyChangedForPropertyARaised = true;
                                       }
                                   };
            sut.PropertyA = "foo";
            Assert.IsFalse(propertyChangedForPropertyARaised);            
        }

        [Test]
        public void PropertyChanged_SuppressChangesFalse_DependantPropertyChangedCalled()
        {
            var sut = new TestEdit();
            bool propertyChangedForPropertyBRaised = false;
            sut.PropertyChanged += (o, e) =>
                                   {
                                       if(e.PropertyName == "PropertyB")
                                       {
                                           propertyChangedForPropertyBRaised = true;
                                       }
                                   };
            sut.PropertyA = "foo";
            Assert.IsTrue(propertyChangedForPropertyBRaised);

        }


        [Test]
        public void PropertyChanged_ValueNotChanged_PropertyChangedNotCalled()
        {
            var sut = new TestEdit();
            sut.PropertyA = "foo";

            bool propertyChangedForPropertyARaised = false;
            sut.PropertyChanged += (o, e) =>
                                   {
                                       if(e.PropertyName == "PropertyA")
                                       {
                                           propertyChangedForPropertyARaised = true;
                                       }
                                   };
            sut.PropertyA = "foo";
            Assert.IsFalse(propertyChangedForPropertyARaised);            
        }


    }
}
