using System.DirectoryServices.AccountManagement;
using IdeasAPI.Models;

namespace IdeasAPI.Controllers
{
    public class AuthenticationService
    {
        public virtual bool SourceValidateCredentials(Login credentials)
        {
            using (var pc = new PrincipalContext(ContextType.Domain, "WEBMEDIA"))
            {
                
                // validate the credentials
                return pc.ValidateCredentials(credentials.Username, credentials.Password);
            }
        }
    }
}