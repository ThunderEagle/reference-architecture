namespace Entity.Core.Settings {
	public interface ISettingsProvider {
		string GetString(string key);
		int GetInteger(string key);
		bool GetBool(string key);
		decimal GetDecimal(string key);
	}
}