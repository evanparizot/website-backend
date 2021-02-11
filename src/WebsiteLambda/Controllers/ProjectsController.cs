using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebsiteLambda.Controllers
{
    [Route("[controller]")]
    public class ProjectsController : ControllerBase
    {
        [HttpGet]
        public String GetProjects()
        {
            return "asdfasdfasf";
        }


    }
}
