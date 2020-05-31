using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wallace.Application.Queries.Currencies;

namespace Wallace.Api.Controllers
{
    [Authorize]
    public class CurrenciesController : BaseController
    {
        /// <summary>
        /// Retrieves all the currencies available in the app.
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        public async Task<ActionResult> GetCurrencies()
        {
            return Ok(await Mediator.Send(new GetCurrenciesQuery()));
        }
    }
}
