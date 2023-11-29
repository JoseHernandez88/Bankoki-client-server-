using Bankoki_client_server_.Shared;
using System;
using System.Data;
//using MySql.Data.MySqlClient;
using MySqlConnector;
using MySqlX.XDevAPI;
using System.Runtime.CompilerServices;
using static Bankoki_client_server.Shared.QueryHandler;
using System.Transactions;
using System.Data.Common;
using System.Reflection.PortableExecutable;
using static System.Net.Mime.MediaTypeNames;

namespace Bankoki_client_server.Shared
{
	public class QueryHandler
	{
		//int test = 0;
		public QueryHandler() { }
		public class QueryException : Exception
		{
			public string ex { get; set; } = string.Empty;
			public QueryException(string message)

			: base(message)

			{

				this.ex = message;
			}

		}

		/* --Connection String-- */
		private MySqlConnector.MySqlConnectionStringBuilder conBuild = new MySqlConnector.MySqlConnectionStringBuilder
		{
			Server = "bankoki.mysql.database.azure.com",
			Port = 3306,
			Database = "bankoki",
			UserID = "BankokiAdmin",
			Password = "Algobuenoyfacil01",
			SslMode = MySqlSslMode.VerifyCA,
			SslCa = "DigiCertGlobalRootCA.crt.pem"
		};

		public MySqlConnectionStringBuilder ConBuild { get => conBuild; set => conBuild = value; }

		/* --Insert-- */
		public async Task<long?> insertTransactionAsync(Bankoki_client_server_.Shared.Transaction? transaction, string accountNumber)
		{
			try
			{
				using (var connection = new MySqlConnection(ConBuild.ConnectionString))
				{
					if (transaction != null)
					{
						long? transactionID;
						await connection.OpenAsync();
						using (var command = connection.CreateCommand())
						{
							await command.ExecuteNonQueryAsync();


							command.CommandText = @"INSERT INTO Trnsaction (ammount, credit, filingDate origin)VALUES (@ammount, @credit, @filingFate, @origin);";
							command.Parameters.AddWithValue("@ammount", transaction.Amount);
							command.Parameters.AddWithValue("@credit", transaction.Credit);
							command.Parameters.AddWithValue("@filingDate", transaction.TransactionDate);
							command.Parameters.AddWithValue("@origin", transaction.Origin);
							transactionID = command.LastInsertedId;
						}
						if (transactionID != null)
						{
							using (var command = connection.CreateCommand())
							{
								command.CommandText = @"INSERT INTO `account_transaction` (`transactions`,`account`)
                                                        VALUES (@transaction,@account);";
								command.Parameters.AddWithValue("@transaction", transactionID.Value);
								command.Parameters.AddWithValue("@account", accountNumber);
							}
						}
						else
						{
							throw new QueryException("Failed to insert transaction.");
						}
						await connection.CloseAsync();
						return transactionID;
					}
					else
					{
						throw new QueryException("Attempted to insert a Null transaction.");

					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return -1;
			}

		}
		public async void InsertUserAsync(User client)
		{
			try
			{
				using (var connection = new MySqlConnection(ConBuild.ConnectionString))
				{
					if (client != null)
					{
						await connection.OpenAsync();
						using (var command = connection.CreateCommand())
						{
							await command.ExecuteNonQueryAsync();


							command.CommandText = @"INSERT INTO `client` (`email`, `password`, `firstName`, `lastNames`)" +
								"VALUES @email, @password, @firstName, @lastName);";
							command.Parameters.AddWithValue("@email", client.Email);
							command.Parameters.AddWithValue("@password", client.Password);
							command.Parameters.AddWithValue("@firstname", client.UserFirstName);
							command.Parameters.AddWithValue("@lastNames", client.UserLastName);


						}
						await connection.CloseAsync();
					}
					else
					{
						throw new QueryException("Attempted to insert a Null User.");

					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}
		/* --Get-- */

		public async Task<Bankoki_client_server_.Shared.Transaction?> getTransactionAsync(int transactionID)
		{
			/*try { 
                using (var connection = new MySqlConnection(ConBuild.ConnectionString))
                {
            
                    await connection.OpenAsync();
                    try
                    {
                        using (var command = connection.CreateCommand())
                        {
                            command.CommandText = @"select * from Trnsaction with transactionID=@ID;";
                            command.Parameters.AddWithValue("@ID", transactionID);
                            Bankoki_client_server_.Shared.Transaction transaction = new Bankoki_client_server_.Shared.Transaction();
                            transaction.TransactionID = transactionID;
                            using (MySqlDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                            {

                                while (await reader.ReadAsync())
                                {
                                    transaction.Origin = reader.GetString("Origin");
                                    transaction.TransactionDate = DateOnly.FromDateTime(reader.GetDateTime("FillingDate"));
                                    transaction.Amount = reader.GetInt32("Ammount");
                                    transaction.Credit = reader.GetBoolean("Credit");
                                }
                            };
                            await connection.CloseAsync();
                            return transaction;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        return null;
                    }
           
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }*/

			try
			{
				var connection = new MySqlConnection(ConBuild.ConnectionString);

				await connection.OpenAsync();
				try
				{
					using (var command = connection.CreateCommand())
					{
						command.CommandText = @"select * from Trnsaction with transactionID=@ID;";
						command.Parameters.AddWithValue("@ID", transactionID);
						Bankoki_client_server_.Shared.Transaction transaction = new Bankoki_client_server_.Shared.Transaction();
						transaction.TransactionID = transactionID;
						using (MySqlDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection))
						{

							while (await reader.ReadAsync())
							{
								transaction.Origin = reader.GetString("Origin");
								transaction.TransactionDate = DateOnly.FromDateTime(reader.GetDateTime("FillingDate"));
								transaction.Amount = reader.GetInt32("Ammount");
								transaction.Credit = reader.GetBoolean("Credit");
							}
						};
						await connection.CloseAsync();
						return transaction;
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.ToString());
					return null;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return null;
			}

		}

		public async Task<Accounts?> getAccountsAsync(string AccountsNumber)
		{
			try
			{
				using (var connection = new MySqlConnection(ConBuild.ConnectionString))
				{
					try
					{
						//Console.WriteLine(connection.ConnectionString); Console.WriteLine( connection.ConnectionString);
						await connection.OpenAsync();

						Accounts account = new();
						try
						{
							using (var command = connection.CreateCommand())
							{
								command.CommandText = @"SELECT * FROM `account` with accountNumber=@number;";
								command.Parameters.AddWithValue("@number", AccountsNumber);


								using (MySqlDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection))
								{

									while (await reader.ReadAsync())
									{
										account.AccountNumber = AccountsNumber;
										account.OpenDate = DateOnly.FromDateTime(reader.GetDateTime("openDate"));
										account.AccountName = reader.GetString("accountName");
										account.Open = reader.GetBoolean("openStatus");
										if (!account.Open)
										{
											account.CloseDate = DateOnly.FromDateTime(reader.GetDateTime("closeDATE"));
										}
									}
								}
							}
							using (var command = connection.CreateCommand())
							{
								command.CommandText = @"SELECT `transactions` FROM  `account_transaction` with `account`=@number;";
								command.Parameters.AddWithValue("@number", AccountsNumber);


								using (MySqlDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection))
								{

									while (await reader.ReadAsync())
									{
										account.appendTransaction2History(reader.GetInt32("`transactions`"));
									}
								}
							}
							await connection.CloseAsync();
							return account;
						}
						catch (Exception ex)
						{
							Console.WriteLine(ex.ToString());
							return null;
						}


					}
					catch (Exception ex)
					{
						Console.WriteLine(ex.ToString());
						return null;
					}

				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return null;
			}
		}

		public async Task<User?> GetUserAsync(string email)
		{
			try
			{
				using (var connection = new MySqlConnection(ConBuild.ConnectionString))
				{
					try
					{
						//Console.WriteLine(connection.ConnectionString); Console.WriteLine((string)connection.ConnectionString);
						await connection.OpenAsync();

						User client = new();
						try
						{
							using (var command = connection.CreateCommand())
							{
								command.CommandText = @"SELECT * FROM `client` with `email`=@address;";
								command.Parameters.AddWithValue("@adress", email);


								using (MySqlDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection))
								{

									while (await reader.ReadAsync())
									{
										client.Email = email;
										client.UserFirstName = reader.GetString("firstName");
										client.UserLastName = reader.GetString("lastNames");
										client.LoggedOn = reader.GetBoolean("loggedIn");
									}
								}
							}
							using (var command = connection.CreateCommand())
							{
								command.CommandText = @"SELECT `account` FROM `client_account` with `client`=@email;";
								command.Parameters.AddWithValue("@email", email);


								using (MySqlDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection))
								{

									while (await reader.ReadAsync())
									{
										client.AppendAccount2Accounts(reader.GetString("`client_account`"));
									}
								}
							}
							return client;
						}
						catch (Exception ex)
						{
							Console.WriteLine(ex.ToString());
							return null;
						}

					}
					catch (Exception ex)
					{
						Console.WriteLine(ex.ToString());
						return null;
					}

				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return null;
			}

		}
		/* --Missaleniouse-- */
		public async void CloseAccount(string accountNumber)
		{
			try
			{
				using (var connection = new MySqlConnection(ConBuild.ConnectionString))
				{
					await connection.OpenAsync();
					using (var command = connection.CreateCommand())
					{
						await command.ExecuteNonQueryAsync();
						command.CommandText = "UPDATE `account` SET `closeDATE` = CAST( GETDATE() AS Date ) WHERE `accountNumber`=@number;" +
							"UPDATE `account` SET `openStatus` = 0 WHERE `accountNumber`=@number;";
						command.Parameters.AddWithValue("@number", accountNumber);



					}
					await connection.CloseAsync();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}

		public async void LoggedOn(string email)
		{
			using (var connection = new MySqlConnection())
			{
				await connection.OpenAsync();
				using (var command = connection.CreateCommand())
				{
					await command.ExecuteNonQueryAsync();
					command.CommandText = @"UPDATE `client` SET `loggedIn` = 1 WHERE email=@email;";
					command.Parameters.AddWithValue("@email", email);

				}
				await connection.CloseAsync();
			}
		}

		public void test()
		{
			try
			{
				using (var connection = new MySqlConnection())
				{
					connection.Open();
					connection.Close();
				}
			}catch(Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}




	}
}


