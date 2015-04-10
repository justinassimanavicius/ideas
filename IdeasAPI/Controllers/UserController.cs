using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Security.Principal;
using IdeasAPI.Helpers;

namespace IdeasAPI.Controllers
{
    public class UserController : ApiController
    {
        [Authorize]
        public IHttpActionResult Get()
        {
            return Ok(UserHelper.DisplayUser(User.Identity));
        }

        [Authorize]
        [HttpGet]
        [Route("api/user/getImage")]
        public IHttpActionResult GetImage()
        {
            var userInfo = UserHelper.DisplayUser(User.Identity);

            var thumbnailBase64 = userInfo.FirstOrDefault(x => x.Key.ToString() == "thumbnailphoto-0");

            if (thumbnailBase64.Value == null)
            {
                return NotFound();
            }

            return Ok(UserHelper.Base64ToImage(thumbnailBase64.Value as Byte[]));
        }
    }
}
