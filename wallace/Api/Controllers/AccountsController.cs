using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wallace.Application.Commands.Accounts.CreateAccount;

namespace Wallace.Api.Controllers
{
    [Authorize]
    public class AccountsController : BaseController
    {
        [Route("")]
        [HttpPost]
        public async Task<ActionResult> CreateAccount(CreateAccountCommand input)
        {
            var accountId = await Mediator.Send(input);
            var location = Url.Action("GetAccount");
            return Created(location, accountId);
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<ActionResult> GetAccount(int id)
        {
            return null;
        } 
    }
}