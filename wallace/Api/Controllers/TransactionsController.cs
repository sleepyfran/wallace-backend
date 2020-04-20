using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wallace.Application.Commands.Transactions.CreateTransaction;

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
        public async Task<ActionResult> CreateCategory(
            CreateTransactionCommand input
        )
        {
            var categoryId = await Mediator.Send(input);
            var location = Url.Action(
                "GetTransaction",
                "Transactions",
                new { id = categoryId }
            );

            return Created(location, categoryId);
        }
    }
}