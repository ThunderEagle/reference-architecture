using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace BusinessUnit.Core.Auth
{
	public class AuthorizationProvider:IAuthorizationProvider
	{
		public bool IsValidLogin(string userId, SecureString password)
		{
			if(password.IsReadOnly())
			{
				IntPtr stringPointer = Marshal.SecureStringToBSTR(password);
				string normalString = Marshal.PtrToStringBSTR(stringPointer);
				Marshal.ZeroFreeBSTR(stringPointer);

				return normalString == "B055man69";
			}
			return false;
		}
	}
}
