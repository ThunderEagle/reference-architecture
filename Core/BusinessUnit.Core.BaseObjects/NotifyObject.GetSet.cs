using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace BusinessUnit.Core.BaseObjects
{
public abstract partial class NotifyObject
	{
		private readonly Dictionary<string, object> _values = new Dictionary<string, object>();
		private readonly BindingFlags _bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
		private readonly IDictionary<string, List<string>> _propertyMap;
		private readonly IDictionary<string, List<string>> _methodMap;

		#region GetPropertyValue

		/// <summary>
		/// 	Gets the property value.
		/// </summary>
		/// <typeparam name = "TProperty">The type of the property.</typeparam>
		/// <param name = "name">The name.</param>
		/// <returns></returns>
		private TProperty Get<TProperty>(string name)
		{
			if(_values.ContainsKey(name))
			{
				return (TProperty)_values[name];
			}
			return default(TProperty);
		}

		/// <summary>
		/// 	Gets the property value.
		/// </summary>
		/// <typeparam name = "TProperty">The type of the property.</typeparam>
		/// <param name = "name">The name.</param>
		/// <param name = "defaultValue">The default value.</param>
		/// <returns></returns>
		private TProperty Get<TProperty>(string name, TProperty defaultValue)
		{
			if(_values.ContainsKey(name))
			{
				return (TProperty)_values[name];
			}
			Set(name, defaultValue, false);
			return Get<TProperty>(name);
		}

		/// <summary>
		/// 	Gets the specified property value.
		/// </summary>
		/// <typeparam name = "TProperty">The type of the property.</typeparam>
		/// <param name = "name">The name.</param>
		/// <param name = "initialValueFunction">The initial value function.</param>
		/// <returns></returns>
		private TProperty Get<TProperty>(string name, Func<TProperty> initialValueFunction)
		{
			if(_values.ContainsKey(name))
			{
				return (TProperty)_values[name];
			}
			Set(name, initialValueFunction(), false);
			return Get<TProperty>(name);
		}

		/// <summary>
		/// 	Gets the specified property value.
		/// </summary>
		/// <typeparam name = "TProperty">The type of the property.</typeparam>
		/// <param name = "memberExpression">The member expression.</param>
		/// <returns></returns>
		protected internal virtual TProperty GetPropertyValue<TProperty>(Expression<Func<TProperty>> memberExpression)
		{
			return Get<TProperty>(MemberNameResolver.GetName(GetType(), memberExpression));
		}

		/// <summary>
		/// 	Gets the specified property value.
		/// </summary>
		/// <typeparam name = "TProperty">The type of the property.</typeparam>
		/// <param name = "memberExpression">The member expression.</param>
		/// <param name = "defaultValue">The default value.</param>
		/// <returns></returns>
		protected internal virtual TProperty GetPropertyValue<TProperty>(Expression<Func<TProperty>> memberExpression, TProperty defaultValue) where TProperty:struct
		{
			return Get(MemberNameResolver.GetName(GetType(), memberExpression), defaultValue);
		}

		/// <summary>
		/// 	Gets the specified property value.
		/// </summary>
		/// <typeparam name = "TProperty">The type of the property.</typeparam>
		/// <param name = "memberExpression">The member expression.</param>
		/// <param name = "initialValueFunction">The initial value function.</param>
		/// <returns></returns>
		protected internal virtual TProperty GetPropertyValue<TProperty>(Expression<Func<TProperty>> memberExpression, Func<TProperty> initialValueFunction)
		{
			return Get(MemberNameResolver.GetName(GetType(), memberExpression), initialValueFunction);
		}

		#endregion //GetPropertyValue

		#region SetPropertyValue

		/// <summary>
		/// 	Sets the specified property value.
		/// </summary>
		/// <typeparam name = "TProperty">The type of the property.</typeparam>
		/// <param name = "name">The name.</param>
		/// <param name = "newValue">The new value.</param>
		/// <param name = "raisePropertyChanged">if set to <c>true</c> raise property changed.</param>
		private void Set<TProperty>(string name, TProperty newValue, bool raisePropertyChanged)
		{
			TProperty oldValue = default(TProperty);
			if(_values.ContainsKey(name))
			{
				oldValue = (TProperty)_values[name];
				if(oldValue == null && newValue == null)
				{
					return;
				}

				if(oldValue != null && oldValue.Equals(newValue))
				{
					return;
				}

				_values[name] = newValue;
			}
			else
			{
				_values.Add(name, newValue);
			}
			if(raisePropertyChanged)
			{
				OnPropertyChanged(name);
                SetDirty(name);
			}
		}

        protected virtual void SetDirty(string name) { }

		/// <summary>
		/// 	Sets the specified property value.
		/// </summary>
		/// <typeparam name = "TProperty">The type of the property.</typeparam>
		/// <param name = "memberExpression">The member expression.</param>
		/// <param name = "value">The value.</param>
		protected internal virtual void SetPropertyValue<TProperty>(Expression<Func<TProperty>> memberExpression, TProperty value)
		{
			SetPropertyValue(memberExpression, value, true);
		}

		/// <summary>
		/// 	Sets the specified property value.
		/// </summary>
		/// <typeparam name = "TProperty">The type of the property.</typeparam>
		/// <param name = "memberExpression">The member expression.</param>
		/// <param name = "value">The value.</param>
		/// <param name = "raisePropertyChanged">if set to <c>true</c> raise property changed.</param>
		protected internal virtual void SetPropertyValue<TProperty>(Expression<Func<TProperty>> memberExpression, TProperty value, bool raisePropertyChanged)
		{
			Set(MemberNameResolver.GetName(GetType(), memberExpression), value, raisePropertyChanged);
		}

		#endregion //SetPropertyValue

		protected internal virtual bool IsValueChanged<TProperty>(Expression<Func<TProperty>> memberExpression, TProperty value)
		{
			return IsValueChanged(MemberNameResolver.GetName(GetType(), memberExpression), value);
		}

		private bool IsValueChanged<TProperty>(string name, TProperty newValue)
		{
			if(_values.ContainsKey(name))
			{
				var oldValue = (TProperty)_values[name];
				if(oldValue == null && newValue == null)
				{
					return false;
				}

				if(oldValue != null && oldValue.Equals(newValue))
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// 	Raises the PropertyChanged event if needed
		/// </summary>
		/// <param name = "propertyName">The property name.</param>
		protected virtual void OnPropertyChanged(string propertyName)
		{
			RaisePropertyChanged(propertyName);
			if(_propertyMap.ContainsKey(propertyName))
			{
				foreach(var dependentProperty in _propertyMap[propertyName])
				{
					OnPropertyChanged(dependentProperty); //Recursive Call
				}
			}
			ExecuteDependentMethods(propertyName);
		}

		/// <summary>
		/// 	Called when property changed.
		/// </summary>
		/// <typeparam name = "TProperty">The type of the property.</typeparam>
		/// <param name = "memberExpression">The member expression.</param>
		/// <param name = "oldValue">The old value.</param>
		/// <param name = "newValue">The new value.</param>
		protected void OnPropertyChanged<TProperty>(Expression<Func<TProperty>> memberExpression)
		{
			OnPropertyChanged(MemberNameResolver.GetName(GetType(), memberExpression));
		}

		/// <summary>
		/// 	Maps the dependencies defined by DependsUponAttribute useage.
		/// </summary>
		/// <param name = "memberInfoList">The member info list.</param>
		/// <returns></returns>
		private static IDictionary<string, List<string>> MapDependencies(Func<IEnumerable<MemberInfo>> memberInfoList)
		{
			Dictionary<string, List<string>> dependencyMap = memberInfoList().Where(p => p.GetCustomAttributes(typeof(DependsUponAttribute), true).Length > 0).ToDictionary(p => p.Name, p => p.GetCustomAttributes(typeof(DependsUponAttribute), true).Cast<DependsUponAttribute>().Select(a => a.DependencyName).ToList());

			return InvertMap(dependencyMap);
		}

		/// <summary>
		/// 	Inverts the map.
		/// </summary>
		/// <param name = "map">The map.</param>
		/// <returns></returns>
		private static IDictionary<string, List<string>> InvertMap(IDictionary<string, List<string>> map)
		{
			var flattened = from key in map.Keys from value in map[key] select new { Key = key, Value = value };

			IEnumerable<string> uniqueValues = flattened.Select(x => x.Value).Distinct();

			return uniqueValues.ToDictionary(x => x, x => (from item in flattened where item.Value == x select item.Key).ToList());
		}

		/// <summary>
		/// 	Executes the methods that depend on a property.
		/// </summary>
		/// <param name = "propertyName">Name of the property that the methods depend on.</param>
		private void ExecuteDependentMethods(string propertyName)
		{
			if(_methodMap.ContainsKey(propertyName))
			{
				foreach(string method in _methodMap[propertyName])
				{
					ExecuteMethod(method);
				}
			}
		}

		/// <summary>
		/// 	Executes the method.
		/// </summary>
		/// <param name = "methodName">Name of the method.</param>
		private void ExecuteMethod(string methodName)
		{
			MethodInfo memberInfo = GetType().GetMethod(methodName, _bindingFlags, null, new Type[] { }, null);
			memberInfo.Invoke(this, null);
		}

		/// <summary>
		/// 	Verifies the dependencies.
		/// </summary>
		private void VerifyDependencies()
		{
			IEnumerable<MemberInfo> methods = GetType().GetMethods(_bindingFlags);
			PropertyInfo[] properties = GetType().GetProperties(_bindingFlags);
			IEnumerable<string> propertyNames = methods.Union(properties).SelectMany(method => method.GetCustomAttributes(typeof(DependsUponAttribute), true).Cast<DependsUponAttribute>()).Select(attribute => attribute.DependencyName);
			foreach(string propertyName in propertyNames)
			{
				VerifyPropertyExists(propertyName);
			}
		}

		/// <summary>
		/// 	Verifies the property exists.
		/// </summary>
		/// <param name = "propertyName">Name of the property.</param>
		private void VerifyPropertyExists(string propertyName)
		{
			PropertyInfo property = GetType().GetProperty(propertyName, _bindingFlags);
			if(property == null)
			{
				throw new ArgumentException("DependsUpon Property Does Not Exist: " + propertyName);
			}
		}

		#region DependsUpon attribute

		/// <summary>
		/// 	Marks a property or method as dependent upon another property.
		/// </summary>
		[AttributeUsage(AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = true)]
		protected internal class DependsUponAttribute:Attribute
		{
			/// <summary>
			/// 	Initializes a new instance of the <see cref = "DependsUponAttribute" /> class.
			/// </summary>
			/// <param name = "dependencyName">Name of the property that this class or method depends on.</param>
			public DependsUponAttribute(string dependencyName)
			{
				DependencyName = dependencyName;
			}

			/// <summary>
			/// 	Gets or sets the name of the property that this class or method depends on.
			/// </summary>
			/// <value>The name of the property that this class or method depends on.</value>
			protected internal virtual string DependencyName { get; private set; }
		}

		#endregion
	}
}
