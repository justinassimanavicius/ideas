using System.DirectoryServices.AccountManagement;

namespace Ideas.BusinessLogic.Services
{
    public class AuthenticationService
    {
        public virtual bool SourceValidateCredentials(string username, string password)
        {
            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, "WEBMEDIA"))
            {
                // validate the credentials
                return pc.ValidateCredentials(username, password);
            }
        }

    }
}