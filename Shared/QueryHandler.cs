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
        public MySqlConnectionStringBuilder conBuild= new MySqlConnectionStringBuilder
        {
            Server = "bankoki.mysql.database.azure.com",
            Database = "bankoki",
            UserID = "BankokiAdmin",
            Password = "Algobuenoyfacil01",
            SslMode = MySqlSslMode.Required,
        };

        public async Task<long?> insertTransaction(Bankoki_client_server_.Shared.Transaction? transaction,string accountNumber)
        {
            try
            {
            using (var connection = new MySqlConnection(conBuild.ConnectionString))
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
        
        public async Task<Bankoki_client_server_.Shared.Transaction?> getTransaction(int transactionID)
        {
        using (var connection = new MySqlConnection(conBuild.ConnectionString))
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
    }
    }
}
