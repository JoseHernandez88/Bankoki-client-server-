using Bankoki_client_server_.Shared;
using Microsoft.EntityFrameworkCore;
namespace Bankoki_client_server_.Server.Data
{
	public class DataContext: DbContext
	{
		static readonly string connectionString = "Server=bankoki.mysql.database.azure.com;" +
														"Port=3306;" +
														"User ID=BankokiAdmin;" +
														"Password=Algobuenoyfacil01;" +
														"Database=bankoki;" +
														"SSL Mode=VerifyCA;" +
														"SSL CA=DigiCertGlobalRootCA.crt.pem";
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
		}
	
		public DataContext(DbContextOptions<DataContext>options): base(options) 
			{

			}
		public DbSet<Accounts> AccountDBSet { get; set; }

	}
}
