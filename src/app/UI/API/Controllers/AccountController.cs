﻿using System.Threading.Tasks;
using System.Web.Http;
using AngularJSAuthentication.API.Models;
using Clinic.Core.Domain.Models;
using Microsoft.AspNet.Identity;

namespace Api.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : BaseController
    {
        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new User
            {
                Username = userModel.UserName,
                Password = userModel.Password
            };

            var result = await Uow.UserRepository.RegisterUser(user);

            var errorResult = GetErrorResult(result);

            return errorResult ?? Ok();
        }

        #region Helpers

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        #endregion
    }
}
