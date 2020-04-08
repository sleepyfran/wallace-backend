using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Wallace.Application.Commands.Setup;

namespace Wallace.Api.Controllers
{
    public class SetupController : BaseController
    {
        [HttpPost]
        public async Task<ActionResult<int>> Post(SetupCommand input)
        {
            return await Mediator.Send(input);
        }
    }
}