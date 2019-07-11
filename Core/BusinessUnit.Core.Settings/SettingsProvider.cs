using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessUnit.Core.Settings
{
	public class SettingsProvider:ISettingsProvider
	{
		public virtual string GetString(string key)
		{
			switch(key)
			{
				case SettingKeys.StringSetting:
					return "Foobar";

				default:
					throw new ArgumentException($"{key} is not a valid String setting.");
			}
		}

		public virtual int GetInteger(string key)
		{
			switch(key)
			{
				case SettingKeys.IntegerSetting:
					return 105;
				default:
					throw new ArgumentException($"{key} is not a valid Integer setting.");
			}
		}

		public virtual bool GetBool(string key)
		{
			switch(key)
			{
				case SettingKeys.BoolSetting:
					return true;
				default:
					throw new ArgumentException($"{key} is not a valid Bool setting.");
			}
		}

		public virtual decimal GetDecimal(string key)
		{
			switch(key)
			{
				case SettingKeys.DecimalSetting:
					return .7443m;
				default:
					throw new ArgumentException($"{key} is not a valid Decimal setting.");
			}
		}
	}
}
