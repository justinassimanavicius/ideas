using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Security.Principal;
using IdeasAPI.Code;
using IdeasAPI.Helpers;

namespace IdeasAPI.Controllers
{
    public class UserController : ApiController
    {
        [Authorize]
        public IHttpActionResult Get()
        {
            //Tries to get cacheable user info from AD
	        var user = UserContext.GetUserInformation(User.Identity);

            if (user == null)
            {
                return NotFound();
            }

            //TODO: Remove hardcode
            user.IsModerator = true;

	        return Ok(user);
        }

        [Authorize]
        [HttpGet]
        [Route("api/user/{userName}")]
        public IHttpActionResult GetInfo(string userName)
        {
            //Tries to get cacheable user info from AD
            var user = UserContext.GetUserInformationByUserName(User.Identity, userName);

            if (user == null)
            {
                return NotFound();
            }

            //TODO: Remove hardcode
            user.IsModerator = true;

            return Ok(user);
        }
    }
}
