
using System;
using System.Threading.Tasks;
using Angular8Core3Sample.Data;
using Angular8Core3Sample.Models.DataModels.Home.MyLists;
using Angular8Core3Sample.Models.Identity;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using Newtonsoft.Json;

namespace Angular8Core3Sample.Controllers.Home
{
    [Route("API/MyLists")]
    [Authorize(Policy = "UserAccount")]
    [ApiController]
    public class MyListsController : BaseApiController
    {

        private MyListsService _myListsService;
        private JsonSerializerSettings serializerSettings;


        public MyListsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
                                    IConfiguration configuration, IHttpContextAccessor httpContextAccessor, 
                                    MyListsService myListsService)
        : base(context, userManager, configuration, httpContextAccessor)
        {
            _myListsService = myListsService;

            serializerSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            };
        }


        [HttpGet("{languageId}/{countryId}")]
        public async Task<IActionResult> Get(int languageId, int countryId)
        {
            var userAccount = await UserManager.GetUserAsync(HttpContextAccessor.HttpContext.User).ConfigureAwait(true);

            if(userAccount == null)
            {
                return NotFound();
            }

            var myLists = _myListsService.GetMyLists(userAccount.Id, languageId, countryId);

            if (myLists == null)
            {
                return new EmptyResult();
            }

            return new JsonResult(myLists, serializerSettings);
        }

        [HttpGet("MyListNames/{languageId}/{countryId}")]
        public async Task<IActionResult> GetMyListNames(int languageId, int countryId)
        {
            var userAccount = await UserManager.GetUserAsync(HttpContextAccessor.HttpContext.User).ConfigureAwait(true);

            if (userAccount == null)
            {
                return NotFound();
            }

            var myLists = _myListsService.GetMyListNames(userAccount.Id, languageId, countryId);

            if (myLists == null)
            {
                return new EmptyResult();
            }

            return new JsonResult(myLists, serializerSettings);
        }

        [HttpPost("createMyList")]
        public async Task<IActionResult> CreateMyList([FromBody] object listName)
        {
            var userAccount = await UserManager.GetUserAsync(HttpContextAccessor.HttpContext.User).ConfigureAwait(true);

            if (userAccount == null)
            {
                return NotFound();
            }

            if(listName == null)
            {
                return new BadRequestResult();
            }

            var newMyList = _myListsService.CreateMyList(listName.ToString(), userAccount.Id);

            return new JsonResult(newMyList, serializerSettings);
        }


        [HttpPut("moveMyListItem")]
        public IActionResult MoveMyListItem([FromBody] MoveMyListItem moveMyListItem)
        {
            if (moveMyListItem == null || moveMyListItem.ProductId < 1 || moveMyListItem.OriginalMyListId < 1)
            {
                return NotFound();
            }

            var result = _myListsService.MoveMyListItem(moveMyListItem);

            return new JsonResult(result, serializerSettings);
        }


        [HttpPut("addMyListItem")]
        public IActionResult AddMyListItem([FromBody] AddMyListItem addMyListItem)
        {
            if (addMyListItem == null || addMyListItem.ProductId < 1 || addMyListItem.MyListId < 1)
            {
                return NotFound();
            }

            var result = _myListsService.AddMyListItem(addMyListItem);

            return new JsonResult(result, serializerSettings);
        }


        [HttpDelete("deleteMyListItem/{productId}/{myListId}")]
        public IActionResult DeleteMyListItem(int productId, int myListId)
        {
            if (productId < 1 || myListId < 1)
            {
                return NotFound();
            }

            var result = _myListsService.DeleteMyListItem(productId, myListId);
            
            return new JsonResult(result, serializerSettings);
        }


        [HttpDelete("deleteMyList/{myListId}")]
        public IActionResult DeleteMyList(int myListId)
        {
            if (myListId < 1)
            {
                return NotFound();
            }

            var result = _myListsService.DeleteMyList(myListId);

            return new JsonResult(result, serializerSettings);
        }

    }
}
