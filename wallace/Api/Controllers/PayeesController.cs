using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Wallace.Application.Commands.Payees;
using Wallace.Application.Queries.Payees;

namespace Wallace.Api.Controllers
{
    public class PayeesController : BaseController
    {
        /// <summary>
        /// Creates a new payee for the current logged in user.
        /// </summary>
        /// <response code="201">Successfully created. Returns the GUID of the new payee</response>
        /// <response code="400">The request was malformed or the data had an error</response>
        [Route("")]
        [HttpPost]
        public async Task<ActionResult> CreatePayee(
            CreatePayeeCommand input
        )
        {
            var payeeId = await Mediator.Send(input);
            return Created("", payeeId);
        }
        
        /// <summary>
        /// Retrieves all the payees of the current logged in user.
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        public async Task<ActionResult> GetPayees()
        {
            return Ok(await Mediator.Send(new GetPayeesQuery()));
        }
        
        /// <summary>
        /// Removes a payee given its GUID only if it belongs to the current
        /// logged in user.
        /// </summary>
        /// <response code="200">Successfully removed, returns the ID of the payee</response>
        /// <response code="400">The request was malformed or the data had an error</response>
        /// <response code="404">The payee does not exist or does not belong to the user</response>
        [Route("{id}")]
        [HttpDelete]
        public async Task<ActionResult> RemovePayee(Guid id)
        {
            return Ok(await Mediator.Send(new RemovePayeeCommand
            {
                Id = id
            }));
        }
    }
}