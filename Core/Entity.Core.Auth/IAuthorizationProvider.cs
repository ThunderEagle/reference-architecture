using System.Security;

namespace Entity.Core.Auth {
	public interface IAuthorizationProvider {
		bool IsValidLogin(string userId, SecureString password);
	}
}