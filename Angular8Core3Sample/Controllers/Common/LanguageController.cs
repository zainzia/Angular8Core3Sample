using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Angular8Core3Sample.Models.DataModels.Catalog;
using Angular8Core3Sample.Models.Identity;
using Angular8Core3Sample.Data;

using Mapster;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Angular8Core3Sample.Controllers.Common
{
    [Route("API/Common/Languages")]
    public class LanguageController : BaseApiController
    {

        public LanguageController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager,
                                    UserManager<ApplicationUser> userManager, IConfiguration configuration) 
            : base(context, userManager, configuration)
        {
        }

        // GET: api/<controller>
        [HttpGet("GetAll")]
        public async Task<IActionResult> Get()
        { 
            try
            {
                var list = await DbContext.Languages.ToListAsync();

                return new JsonResult(list.OrderBy(x => x.Name).Adapt<Language[]>(),
                    new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
            }
            catch (Exception ex)
            {
                var x = ex.ToString();
            }

            return new EmptyResult();
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {

                var language = DbContext.Languages.FirstOrDefault(x => x.LanguageID == id);

                return new JsonResult(language.Adapt<Language>(),
                    new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
            }
            catch (Exception ex)
            {
                var x = ex.ToString();
            }

            return new EmptyResult();
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
