using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Wallace.Application.Commands.Auth.Login;
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
            var tokenResult = await Mediator.Send(input);
            return Ok(tokenResult.Jwt);
        }
        
        /// <summary>
        /// Checks credential from an user and returns a JWT token to use
        /// if they were valid.
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
        ///     eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiU3lsdGVrIn0.xOu8aTgaf5CcDkeL3GsTK2DXhtI96cXheU2c99dFAQQ
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
        public async Task<ActionResult<Token>> Login(LoginCommand input)
        {
            var tokenResult = await Mediator.Send(input);
            return Ok(tokenResult.Jwt);
        }
    }
}