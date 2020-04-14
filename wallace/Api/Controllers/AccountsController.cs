using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wallace.Application.Commands.Accounts.CreateAccount;
using Wallace.Application.Commands.Accounts.EditAccount;
using Wallace.Application.Queries.Accounts;

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
        /// Retrieves all the accounts of the current logged in user.
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        public async Task<ActionResult> GetAccounts()
        {
            return Ok(await Mediator.Send(new GetAccountsQuery()));
        }

        /// <summary>
        /// Retrieves the data the account with the specified GUID.
        /// </summary>
        /// <response code="200">Successfully fetched, returns the data of the account</response>
        /// <response code="400">The request was malformed or the data had an error</response>
        /// <response code="404">The account does not exist or does not belong to the user</response>
        [Route("{id}")]
        [HttpGet]
        public async Task<ActionResult> GetAccount(Guid id)
        {
            return Ok(await Mediator.Send(new GetAccountQuery
            {
                Id = id
            }));
        }

        /// <summary>
        /// Updates an account given its GUID.
        /// </summary>
        /// <response code="200">Successfully updated, returns the ID of the account</response>
        /// <response code="400">The request was malformed or the data had an error</response>
        /// <response code="404">The account does not exist or does not belong to the user</response>
        [Route("{id}")]
        [HttpPut]
        public async Task<ActionResult> ModifyAccount(
            [FromRoute] Guid id,
            [FromBody] EditAccountCommand input
        )
        {
            input.QueryId = id;
            return Ok(await Mediator.Send(input));
        }
    }
}