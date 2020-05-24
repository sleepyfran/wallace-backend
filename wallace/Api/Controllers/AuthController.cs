using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Wallace.Application.Commands.Auth.Login;
using Wallace.Application.Commands.Auth.Refresh;
using Wallace.Application.Commands.Auth.SignUp;
using Wallace.Application.Common.Dto;
using Wallace.Domain.Identity.Entities;

namespace Wallace.Api.Controllers
{
    public class AuthController : BaseController
    {
        /// <summary>
        /// Creates a new user and returns a JWT token to authenticate future
        /// API calls.
        /// </summary>
        /// <response code="200">Successfully authenticated</response>
        /// <response code="400">The request was malformed or the data had an error</response>
        [HttpPost]
        [Route("signup")]
        public async Task<ActionResult<AuthDto>> SignUp(SignUpCommand input)
        {
            return Ok(await Mediator.Send(input));
        }
        
        /// <summary>
        /// Checks credential from an user and returns a pair of JWT tokens,
        /// one for access and one for refreshing the access token.
        /// </summary>
        /// <response code="200">Successfully authenticated</response>
        /// <response code="400">The request was malformed or the data had an error</response>
        /// <response code="401">The credentials are invalid</response>
        /// <response code="404">There is no user with the given email</response>
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<AuthDto>> Login(LoginCommand input)
        {
            return Ok(await Mediator.Send(input));
        }

        /// <summary>
        /// Refreshes an access token given a refresh token. Same response as a
        /// login or a sign up.
        /// </summary>
        /// <response code="200">Successfully authenticated</response>
        /// <response code="400">The request was malformed</response>
        /// <response code="401">The refresh token has expired</response>
        /// <returns></returns>
        [HttpPost]
        [Route("refresh")]
        public async Task<ActionResult<Token>> Refresh(RefreshTokenCommand input)
        {
            var accessToken = await Mediator.Send(input);
            return Ok(new
            {
                access = accessToken.Jwt,
                refresh = input.RefreshToken
            });
        }
    }
}