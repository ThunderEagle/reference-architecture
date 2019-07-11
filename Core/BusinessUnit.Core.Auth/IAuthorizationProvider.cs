using System.Security;

namespace BusinessUnit.Core.Auth {
	public interface IAuthorizationProvider {
		bool IsValidLogin(string userId, SecureString password);
	}
}