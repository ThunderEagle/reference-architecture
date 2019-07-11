using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

namespace Entity.Core.BaseObjects
{
    public abstract class BaseEdit:SuppressibleBase
    {
        private readonly Dictionary<string, bool> _dirtyState = new Dictionary<string, bool>();
        private bool _isGlobalDirty = false;

        public virtual bool IsDirty
        {
            get { return _isGlobalDirty || _dirtyState.Any(x => x.Value); }
        }

        public virtual ReadOnlyDictionary<string, bool> TrackedProperties => new ReadOnlyDictionary<string, bool>(_dirtyState);

        public virtual void ResetDirty()
        {
            _isGlobalDirty = false;
            _dirtyState.Clear();
            OnPropertyChanged(() => IsDirty);
        }

        [Obsolete("Either use RaisePropertyChanged or implement the SetPropertyValue method.")]
        protected void RaiseEventPropertyChanged(string propertyName)
        {
            RaisePropertyChanged(propertyName);
        }

        protected virtual void SetDirty()
        {
            _isGlobalDirty = true;
            OnPropertyChanged(() => IsDirty);
        }


        public virtual bool IsPropertyDirty<TProperty>(Expression<Func<TProperty>> memberExpression)
        {
            return IsPropertyDirty(MemberNameResolver.GetName(GetType(), memberExpression));
        }

        public virtual bool IsPropertyDirty(string name)
        {
            if(_dirtyState.ContainsKey(name))
            {
                return _dirtyState[name];
            }
            return false;
        }


        protected virtual void SetDirty<TProperty>(Expression<Func<TProperty>> memberExpression)
        {
            SetDirty(MemberNameResolver.GetName(GetType(), memberExpression));
        }

        protected override void SetDirty(string name)
        {
            if(_dirtyState.ContainsKey(name))
            {
                _dirtyState[name] = true;
            }
            else
            {
                _dirtyState.Add(name, true);
            }

            OnPropertyChanged(() => IsDirty);
        }

    }
}
