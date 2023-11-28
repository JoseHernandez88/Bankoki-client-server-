using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bankoki_client_server_.Shared;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bankokiclientserver.Server.Controllers
{
    [Route("api/[controller]")]
    public class TransferingController : Controller
    {

        public async Task<ActionResult<List<Transfering>>> GetAllAvailableBalance()
        {
            var list = new List<Transfering>
            {
                new Transfering {Id1 = 1, AccountType1 = "Checkings", AccountNumber1 = "010-00987", Balance1 = 100, Id2 = 2, AccountType2 = "Savings", AccountNumber2 = "010-00987", Balance2 = 10000}
            };

            return Ok(list);

        }
    }
}

