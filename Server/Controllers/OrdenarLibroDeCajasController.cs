using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bankoki_client_server_.Shared;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bankokiclientserver.Server.Controllers
{
    public class OrdenarLibroDeCajasController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
    }
}

