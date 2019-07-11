using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace BusinessUnit.Core.BaseObjects
{
	/// <summary>
	/// Abstract class that assists in implementing the INotifyProperyChanged interface for objects.
	/// </summary>
	/// <remarks></remarks>
	[Serializable]
	public abstract partial class NotifyObject : INotifyPropertyChanged
	{
		#region Data

		private static readonly Dictionary<string, PropertyChangedEventArgs> _eventArgCache;
		private const string ERROR_MSG = "{0} is not a public property of {1}";

		#endregion // Data

		#region Constructors

		protected NotifyObject()
		{
			_propertyMap = NotifyObject.MapDependencies(() => GetType().GetProperties(_bindingFlags));
			_methodMap = NotifyObject.MapDependencies(() => GetType().GetMethods(_bindingFlags));
			VerifyDependencies();
		}

		static NotifyObject()
		{
			_eventArgCache = new Dictionary<string, PropertyChangedEventArgs>();
		}

		#endregion // Constructors

		#region Public Members

		/// <summary>
		/// 	Raised when a public property of this object is set.
		/// </summary>
		[field: NonSerialized]
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// 	Returns an instance of PropertyChangedEventArgs for 
		/// 	the specified property name.
		/// </summary>
		/// <param name = "propertyName">
		/// 	The name of the property to create event args for.
		/// </param>
		public static PropertyChangedEventArgs GetPropertyChangedEventArgs(string propertyName)
		{
			if (String.IsNullOrEmpty(propertyName))
			{
				throw new ArgumentException("propertyName cannot be null or empty.");
			}

			PropertyChangedEventArgs args;

			// Get the event args from the cache, creating them
			// and adding to the cache if necessary.
			lock (typeof(NotifyObject))
			{
				bool isCached = _eventArgCache.ContainsKey(propertyName);
				if (!isCached)
				{
					_eventArgCache.Add(propertyName, new PropertyChangedEventArgs(propertyName));
				}

				args = _eventArgCache[propertyName];
			}

			return args;
		}

		#endregion // Public Members

		#region Protected Members

		/// <summary>
		/// 	Derived classes can override this method to
		/// 	execute logic after a property is set. The 
		/// 	base implementation does nothing.
		/// </summary>
		/// <param name = "propertyName">
		/// 	The property which was changed.
		/// </param>
		protected virtual void AfterPropertyChanged(string propertyName)
		{
		}

		/// <summary>
		/// 	Attempts to raise the PropertyChanged event, and 
		/// 	invokes the virtual AfterPropertyChanged method, 
		/// 	regardless of whether the event was raised or not.
		/// </summary>
		/// <param name = "propertyName">
		/// 	The property which was changed.
		/// </param>
		protected void RaisePropertyChanged(string propertyName)
		{
			//VerifyProperty(propertyName);
			// Get the cached event args.
			PropertyChangedEventArgs args = GetPropertyChangedEventArgs(propertyName);
			OnPropertyChanged(args);
			AfterPropertyChanged(propertyName);
		}

		/// <summary>
		/// Invokes the PropertyChanged Handler, if you override this method, 
		/// you must either call the base.OnPropertyChangedMethod, or invoke
		/// PropertyChanged.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			var handler = PropertyChanged;
			if(handler != null)
			{
				// Raise the PropertyChanged event.
				handler(this, e);
			}
		}

		#endregion // Protected Members

		#region Private Helpers

		[Conditional("DEBUG")]
		private void VerifyProperty(string propertyName)
		{
			Type type = GetType();

			// Look for a public property with the specified name.
			PropertyInfo propInfo = type.GetProperty(propertyName);

			if (propInfo == null)
			{
				// The property could not be found,
				// so alert the developer of the problem.

				string msg = string.Format(ERROR_MSG, propertyName, type.FullName);

				Debug.Fail(msg);
			}
		}

		#endregion // Private Helpers
	}
}
