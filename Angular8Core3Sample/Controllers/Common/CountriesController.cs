using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using Angular8Core3Sample.Data;
using Angular8Core3Sample.Models.Identity;

using Mapster;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Angular8Core3Sample.Models.DataModels.Catalog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Angular8Core3Sample.Controllers.Common
{
    [Route("api/Common/Countries")]
    public class CountriesController : BaseApiController
    {

        public CountriesController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager,
                                    UserManager<ApplicationUser> userManager, IConfiguration configuration)
            : base(context, userManager, configuration)
        {
            TypeAdapterConfig.GlobalSettings.Default.PreserveReference(true);
        }

        // GET: api/<controller>
        [HttpGet]
        [HttpGet("All")]
        public async Task<IActionResult> Get()
        {
            var list = await DbContext.Countries.ToListAsync().ConfigureAwait(false);

            return new JsonResult(list.OrderBy(x => x.Name).Adapt<Country[]>(),
                new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented, ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize, PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects });

        }


        [HttpGet]
        [HttpGet("Provinces")]
        public async Task<IActionResult> GetProvinces()
        {
            var list = await DbContext.Provinces.ToListAsync().ConfigureAwait(false);

            return new JsonResult(list.OrderBy(x => x.Name).Adapt<Province[]>(),
                new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented, ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize, PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects });

        }


        [HttpGet]
        [HttpGet("States")]
        public async Task<IActionResult> GetStates()
        {
            var list = await DbContext.States.ToListAsync().ConfigureAwait(false);

            return new JsonResult(list.OrderBy(x => x.Name).Adapt<State[]>(),
                new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented, ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize, PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects });

        }


        // GET api/<controller>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var country = DbContext.Countries.FirstOrDefault(x => x.CountryID == id);

            return new JsonResult(country.Adapt<Country>(),
                new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented, ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize, PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects });

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
