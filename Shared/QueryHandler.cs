using Bankoki_client_server_.Shared;
using System;
using System.Data;
using MySql.Data.MySqlClient;
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
        public class QueryException : Exception
        {
            public string ex { get; set; } = string.Empty;
            public QueryException(string message)

            : base(message)
                
            {
                
                this.ex = message;
            }

        }
        /*  hostname=bankoki.mysql.database.azure.com
            username=BankokiAdmin
            password=Algobuenoyfacil01
            ssl-mode=require
        
		private MySqlConnectionStringBuilder conBuild = new MySqlConnectionStringBuilder
		{
			Server = "bankoki.mysql.database.azure.com",
            Port=3306,
			Database = "bankoki",
			UserID = "BankokiAdmin",
			Password = "Algobuenoyfacil01",
			//SslMode = MySqlSslMode.VerifyCA,
            //SslCa = "DigiCertGlobalRootCA.crt.pem"
		};

		public MySqlConnectionStringBuilder ConBuild { get => conBuild; set => conBuild = value; }
        */
        string ConnectionString = "server=bankoki.mysql.database.azure.com;uid=BankokiAdmin;password=Algobuenoyfacil01;ssl-mode=required;ssl-ca=DigiCertGlobalRootCA.crt.pem";
		public async Task<long?> insertTransactionAsync(Bankoki_client_server_.Shared.Transaction? transaction,string accountNumber)
        {
            try
            {
            using (var connection = new MySqlConnection( ConnectionString))
                {
                    if (transaction != null)
                    {
                        long? transactionID;
                        await connection.OpenAsync();
                        using (var command = connection.CreateCommand())
                        {
                            await command.ExecuteNonQueryAsync();


                            command.CommandText = @"INSERT INTO Trnsaction (ammount, credit, filingDate origin)VALUES (@ammount, @credit, @filingFate, @origin);";
                            command.Parameters.AddWithValue("@ammount", transaction.Ammount);
                            command.Parameters.AddWithValue("@credit", transaction.Credit);
                            command.Parameters.AddWithValue("@filingDate", transaction.TransactionDate);
                            command.Parameters.AddWithValue("@origin", transaction.Origin);
                            transactionID=command.LastInsertedId;
                        }
                        if (transactionID != null)
                        {
                            using (var command = connection.CreateCommand())
                            {
                                command.CommandText = @"INSERT INTO `account_transaction` (`transactions`,`account`)
                                                        VALUES (@transaction,@account);";
                                command.Parameters.AddWithValue("@transaction", transactionID.Value);
                                command.Parameters.AddWithValue("@account", accountNumber );
                            }
                        }
                        else
                        {
                            throw new QueryException("Failed to insert transaction.");
                        }
                        return transactionID;
                    }
                    else
                    {
                        throw new QueryException("Attempted to insert a Null transaction.");
                        
                    }
                }
            }catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return -1;
            }
            
        }
        
        public async Task<Bankoki_client_server_.Shared.Transaction?> getTransactionAsync(int transactionID)
        {
            try { 
            using (var connection = new MySqlConnection( ConnectionString))
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
                                transaction.Ammount = reader.GetInt32("Ammount");
                                transaction.Credit = reader.GetBoolean("Credit");
                            }
                        };
                        return transaction;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return null;
                }
           
        }
            }catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        

    }

        public async Task<Accounts?> getAccountsAsync(string AccountsNumber)
        {
            try
            {
                using (var connection = new MySqlConnection(ConnectionString))
                {
                    try
                    {
						Console.WriteLine(connection.ConnectionString); Console.WriteLine((string) connection.ConnectionString);
						connection.Open();
						
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
								command.CommandText = @"SELECT * FROM  `account_transaction` with `account`=@number;";
								command.Parameters.AddWithValue("@number", AccountsNumber);


								using (MySqlDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection))
								{

									while (await reader.ReadAsync())
									{
										account.appendTransaction2History(reader.GetInt32("`transactions`"));
									}
								}
							}
							return account;
						}catch (Exception ex)
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

        public async void closeAccount(string accountNumber)
        {
            try
            {
                using (var connection = new MySqlConnection( ConnectionString))
                {
                    using (var command = connection.CreateCommand())
                    {
                        await command.ExecuteNonQueryAsync();
                        command.CommandText = "UPDATE `account` SET `closeDATE` = CAST( GETDATE() AS Date );" +
                            "UPDATE `account` SET `openStatus` = 0";

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

    }

}
