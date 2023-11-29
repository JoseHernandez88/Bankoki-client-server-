using Bankoki_client_server_.Server.Data;
using Bankoki_client_server_.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bankoki_client_server_.Server.Controllers
{
    [Route("api/[controller]")]
[ApiController]
public class AccountDB : ControllerBase
{
        private readonly DataContext _dataContext;

		public AccountDB(DataContext dataContext)
		{
			_dataContext = dataContext;
		}

		public async Task<ActionResult<Accounts>> Account()
		{
			var account = await _dataContext.AccountDBSet.Where(a=> a.AccountNumber== "000000000000000002").ToListAsync();
			return Ok(account);
		}
	}
}
