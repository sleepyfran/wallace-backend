using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Wallace.Application.Commands.Auth.Login;
using Wallace.Application.Commands.Auth.Refresh;
using Wallace.Application.Commands.Auth.SignUp;
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
        /// <remarks>
        /// Happy path sample request:
        /// 
        ///     POST /api/auth/signup
        ///
        ///     {
        ///        "email": "example@test.com",
        ///        "name": "Example",
        ///        "password": "examplepassword"
        ///     }
        ///     
        /// Happy path sample response:
        ///
        ///     eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiU3lsdGVrIn0.xOu8aTgaf5CcDkeL3GsTK2DXhtI96cXheU2c99dFAQQ
        ///
        /// Error sample request:
        ///
        ///    POST /api/auth/signup
        ///
        ///     {
        ///        "email": "duplicated@email.com",
        ///        "name": "Example",
        ///        "password": "examplepassword"
        ///     }
        ///
        /// Error sample response:
        ///
        ///     {
        ///        "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
        ///        "title": "Validation error",
        ///        "status": 400,
        ///        "errors": {
        ///            "Email": [
        ///                "This email is already in use by another user"
        ///            ]
        ///        }
        ///    } 
        /// </remarks>
        [HttpPost]
        [Route("signup")]
        public async Task<ActionResult<Token>> SignUp(SignUpCommand input)
        {
            var (accessToken, refreshToken) = await Mediator.Send(input);
            return Ok(new
            {
                access = accessToken.Jwt,
                refresh = refreshToken.Jwt
            });
        }
        
        /// <summary>
        /// Checks credential from an user and returns a pair of JWT tokens,
        /// one for access and one for refreshing the access token.
        /// </summary>
        /// <response code="200">Successfully authenticated</response>
        /// <response code="400">The request was malformed or the data had an error</response>
        /// <response code="401">The credentials are invalid</response>
        /// <response code="404">There is no user with the given email</response>
        /// <remarks>
        /// Happy path sample request:
        /// 
        ///     POST /api/auth/login
        ///
        ///     {
        ///        "email": "example@test.com",
        ///        "password": "examplepassword"
        ///     }
        ///     
        /// Happy path sample response:
        ///
        ///    {
        ///        "access": "eyJhbGciOiJIUzI1NiIsInR5cCI6I...",
        ///        "refresh": "eyJhbGciOiJIUzI1NiIsInR5cCI6I..."
        ///    }
        ///
        /// Error sample request:
        ///
        ///    POST /api/auth/login
        ///
        ///     {
        ///        "email": "a@e",
        ///        "password": "examplepassword"
        ///     }
        ///
        /// Error sample response:
        ///
        ///     {
        ///        "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
        ///        "title": "Validation error",
        ///        "status": 400,
        ///        "errors": {
        ///            "Email": [
        ///                "'Email' is not a valid email address."
        ///            ]
        ///        }
        ///    } 
        /// </remarks>
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login(LoginCommand input)
        {
            var (accessToken, refreshToken) = await Mediator.Send(input);
            return Ok(new
            {
                access = accessToken.Jwt,
                refresh = refreshToken.Jwt
            });
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
        public async Task<ActionResult> Refresh(RefreshTokenCommand input)
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