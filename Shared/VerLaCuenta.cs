using System;
namespace Bankoki_client_server_.Shared
{
	public class VerLaCuenta
	{

			public required int Id { get; set; }

			public required string AccountType { get; set; }

			public required string AccountNumber { get; set; }

			public required int Balance { get; set; }


	}
}

