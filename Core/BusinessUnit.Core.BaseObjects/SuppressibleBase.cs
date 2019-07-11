using System;
using System.Linq.Expressions;

namespace BusinessUnit.Core.BaseObjects
{
    public abstract class SuppressibleBase:BusinessUnit.Core.BaseObjects.NotifyObject
    {
        public bool SuppressPropertyChanges { get; set; }

        protected internal override void SetPropertyValue<TProperty>(Expression<Func<TProperty>> memberExpression, TProperty value)
        {
            SetPropertyValue(memberExpression, value, !SuppressPropertyChanges);
        }
    }
}
