using Bankoki_client_server.Shared;
using Org.BouncyCastle.Utilities.Collections;
using System.ComponentModel.DataAnnotations;
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
        public  List<Accounts> Accounts { get; set; }= new List<Accounts>();

		public class UserException:Exception {
			public string ex { get; set; } = string.Empty;
			public UserException(string message)

			: base(message)

			{

				this.ex = message;
			}
		}
		public User() { }
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
		public void SetLoggedOn()
		{
			LoggedOn = true;
			QueryHandler qh = new QueryHandler();
			qh.LoggedOn(Email);
		}
		public bool Transfer(string AccountFromNumber,string AccountTooNumber, int centAmount)
		{
			try 
			{
				if(this.VerifyAccountOwner(AccountTooNumber) && this.VerifyAccountOwner(AccountFromNumber))
				{
					Bankoki_client_server_.Shared.Accounts from = new Bankoki_client_server_.Shared.Accounts(AccountFromNumber);
					if (from.balance() >= centAmount / 100.0)
					{
						Bankoki_client_server_.Shared.Accounts too = new Bankoki_client_server_.Shared.Accounts(AccountTooNumber);
						string origin = "Transfer from: " + AccountFromNumber;
						bool credit = true;
						Transaction transfer = new Transaction(origin,centAmount,credit);
						too.addTransaction(transfer);
						origin = "Transfer too: " + AccountTooNumber;
						credit = false;
						transfer=new Transaction(origin,centAmount,credit);
						from.addTransaction(transfer);
						return true;
					}
					return false;

				}
				throw new UserException("One or more accounts do not match client."); 
			}
			catch (Exception ex) 
			{ 
				Console.WriteLine(ex);
				return false;
			
			}
		}

	}
}
