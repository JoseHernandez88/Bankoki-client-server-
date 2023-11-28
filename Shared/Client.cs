using Bankoki_client_server.Shared;
using Org.BouncyCastle.Utilities.Collections;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Transactions;
using static Bankoki_client_server_.Shared.Accounts;

namespace Bankoki_client_server_.Shared
{
    public class User
    {
        public string UserFirstName { get; set; }
        = string.Empty;
		public string UserLastName { get; set; } = string.Empty;
        public string Password { get; set; }
        = string.Empty;
        public string Email { get; set; }
        = string.Empty;
        public bool LoggedOn { get; set; }
        public  List<Accounts> Accounts { get; set; }
        public User()
        {
            Accounts = new List<Accounts>();
        }
        public User(string email)
        {
			Email = email;
			QueryHandler qh = new QueryHandler();
			try
			{
				User? temp = qh.GetUserAsync(email).Result;
				
				if (temp != null)
				{
					UserFirstName=temp.UserFirstName; 
					UserLastName=temp.UserLastName;
					LoggedOn = temp.LoggedOn;
					Accounts= temp.Accounts;
				}
				else
				{
					throw new AccountException("No user with that email was not found in the database.");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

		}
		public void AppendAccount2Accounts(string accNumber)
		{
			this.Accounts.Append<Accounts>(new Accounts(accNumber));
		}
		public bool VerifyPassword(string password, string email)
		{
			User temp= new User(email);
			return temp.Password.Equals(password);
		}

		public void RegisterClient( string email, string password, string firstName, string lastName)
		{
			User temp = new User()
			{
				Email = email,
				Password = password,
				UserFirstName = firstName,
				UserLastName = lastName
			};
			QueryHandler qh = new QueryHandler();
			qh.InsertUserAsync(temp);
		}

		public bool VerifyAccountOwner(string accountNumber) 
		{
			return Accounts.Contains<Accounts>(new Accounts(accountNumber));
		}

	}
}
