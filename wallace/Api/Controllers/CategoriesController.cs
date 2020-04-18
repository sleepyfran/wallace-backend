using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wallace.Application.Commands.Categories.CreateCategory;
using Wallace.Application.Queries.Categories;

namespace Wallace.Api.Controllers
{
    [Authorize]
    public class CategoriesController : BaseController
    {
        /// <summary>
        /// Creates a new category for the current logged in user.
        /// </summary>
        /// <response code="201">Successfully created. Returns the GUID of the new category</response>
        /// <response code="400">The request was malformed or the data had an error</response>
        [Route("")]
        [HttpPost]
        public async Task<ActionResult> CreateCategory(
            CreateCategoryCommand input
        )
        {
            var categoryId = await Mediator.Send(input);
            var location = Url.Action(
                "GetCategory",
                "Categories",
                new { id = categoryId }
            );

            return Created(location, categoryId);
        }
        
        /// <summary>
        /// Retrieves all the categories of the current logged in user.
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        public async Task<ActionResult> GetCategories()
        {
            return Ok(await Mediator.Send(new GetCategoriesQuery()));
        }

        /// <summary>
        /// Retrieves the data of the category with the specified GUID.
        /// </summary>
        /// <response code="200">Successfully fetched, returns the data of the category</response>
        /// <response code="400">The request was malformed or the data had an error</response>
        /// <response code="404">The category does not exist or does not belong to the user</response>
        [Route("{id}")]
        [HttpGet]
        public async Task<ActionResult> GetCategory(Guid id)
        {
            return Ok(await Mediator.Send(new GetCategoryQuery { Id = id }));
        }
    }
}