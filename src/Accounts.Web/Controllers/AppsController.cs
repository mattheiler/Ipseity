﻿using System.Collections.Generic;
using Accounts.Web.Models.Apps;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Accounts.Web.Controllers
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
