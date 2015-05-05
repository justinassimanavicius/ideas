using System.Linq;
using System.Web.Http;
using Ideas.BusinessLogic.Configurations;
using Ideas.BusinessLogic.Services.ConfigurationLoaderService;
using IdeasAPI.Code;

namespace IdeasAPI.Controllers
{
    public class UserController : ApiController
    {
        private readonly IdeasGlobalSettings _config;
        private readonly UserContext _userContext;

        public UserController(IConfigurationLoaderService configurationLoaderService, UserContext userContext)
        {
            _userContext = userContext;
            _config = configurationLoaderService.LoadConfig<IdeasGlobalSettings>();
        }

        [Authorize]
        public IHttpActionResult Get()
        {
            //Tries to get cacheable user info from AD
            var user = _userContext.GetUserInfo(User.Identity.Name);

            if (user == null)
            {
                return NotFound();
            }

            user.IsModerator = _config.ModeratorCollection.Cast<ModeratorElement>().ToList().Any(x => x.Username == user.DomainName);

	        return Ok(user);
        }

        [Authorize]
        [HttpGet]
        [Route("api/user/{userName}")]
        public IHttpActionResult GetInfo(string userName)
        {
            //Tries to get cacheable user info from AD
            var user = _userContext.GetUserInfo(userName);

            if (user == null)
            {
                return NotFound();
            }

            user.IsModerator = _config.ModeratorCollection.Cast<ModeratorElement>().ToList().Any(x => x.Username == user.DomainName);

            return Ok(user);
        }
    }
}
