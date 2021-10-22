﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProgrammerGrammar.Example.Web.Models.Apps;

namespace ProgrammerGrammar.Example.Web.Controllers
{
    [ApiController]
    [Route("api/apps")]
    public class AppsController
    {
        public IConfiguration Configuration { get; }

        public AppsController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [HttpGet]
        public ICollection<App> GetApps()
        {
            return Configuration.GetSection("Apps").Get<App[]>();
        }
    }
}
