using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wallace.Application.Commands.Transactions.CreateTransaction;
using Wallace.Application.Queries.Transactions;

namespace Wallace.Api.Controllers
{
    [Authorize]
    public class TransactionsController : BaseController
    {
        /// <summary>
        /// Creates a new transaction for the current logged in user.
        /// </summary>
        /// <response code="201">
        /// Successfully created. Returns the GUID of the new transaction
        /// </response>
        /// <response code="400">
        /// The request was malformed or the data had an error
        /// </response>
        [Route("")]
        [HttpPost]
        public async Task<ActionResult> CreateTransaction(
            CreateTransactionCommand input
        )
        {
            var transactionId = await Mediator.Send(input);
            var location = Url.Action(
                "GetTransaction",
                "Transactions",
                new { id = transactionId }
            );

            return Created(location, transactionId);
        }

        /// <summary>
        /// Retrieves all the transactions of the current logged in user.
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        public async Task<ActionResult> GetTransactions()
        {
            return Ok(await Mediator.Send(new GetTransactionsQuery()));
        }

        /// <summary>
        /// Retrieves the data of the transaction with the specified GUID.
        /// </summary>
        /// <response code="200">Successfully fetched, returns the data of the transaction</response>
        /// <response code="400">The request was malformed or the data had an error</response>
        /// <response code="404">The transaction does not exist or does not belong to the user</response>
        [Route("{id}")]
        [HttpGet]
        public async Task<ActionResult> GetTransaction(Guid id)
        {
            return Ok(await Mediator.Send(new GetTransactionQuery { Id = id }));
        }
    }
}