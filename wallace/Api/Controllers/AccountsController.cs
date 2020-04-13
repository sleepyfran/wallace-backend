using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wallace.Application.Commands.Accounts.CreateAccount;
using Wallace.Application.Queries.Accounts.GetAccount;

namespace Wallace.Api.Controllers
{
    [Authorize]
    public class AccountsController : BaseController
    {
        /// <summary>
        /// Creates a new account for the current logged in user.
        /// </summary>
        /// <response code="201">Successfully created. Returns the GUID of the new account</response>
        /// <response code="400">The request was malformed or the data had an error</response>
        [Route("")]
        [HttpPost]
        public async Task<ActionResult> CreateAccount(CreateAccountCommand input)
        {
            var accountId = await Mediator.Send(input);
            var location = Url.Action(
                "GetAccount",
                "Accounts",
                new { id = accountId }
            );
            return Created(location, accountId);
        }

        /// <summary>
        /// Retrieves the data the account with the specified GUID.
        /// </summary>
        /// <response code="200">Successfully fetched, returns the data of the account</response>
        /// <response code="400">The request was malformed or the data had an error</response>
        [Route("{id}")]
        [HttpGet]
        public async Task<ActionResult> GetAccount(Guid id)
        {
            return Ok(await Mediator.Send(new GetAccountQuery
            {
                Id = id
            }));
        } 
    }
}