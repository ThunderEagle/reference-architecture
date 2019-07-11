using System;
using System.Linq.Expressions;

namespace Entity.Core.BaseObjects
{
    public abstract class SuppressibleBase:Entity.Core.BaseObjects.NotifyObject
    {
        public bool SuppressPropertyChanges { get; set; }

        protected internal override void SetPropertyValue<TProperty>(Expression<Func<TProperty>> memberExpression, TProperty value)
        {
            SetPropertyValue(memberExpression, value, !SuppressPropertyChanges);
        }
    }
}
