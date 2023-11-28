using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bankoki_client_server_.Shared;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bankokiclientserver.Server.Controllers
{
    [Route("api/[controller]")]
    public class VerLaCuentaController : Controller
    {

        public ActionResult<List<VerLaCuenta>> GetAllAvailableBalance()
        {
            var list = new List<VerLaCuenta>
            {
                new VerLaCuenta {Id = 1, AccountType = "Cuenta de cheques", AccountNumber = "010-00987", Balance = 100},
                new VerLaCuenta {Id = 2, AccountType = "Cuenta de Ahorro", AccountNumber = "010-00987", Balance = 10000}
            };

            return Ok(list);

        }
    }
}

