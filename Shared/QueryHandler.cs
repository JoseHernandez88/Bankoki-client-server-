using Bankoki_client_server_.Shared;
using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace Bankoki_client_server.Shared
{
    public class QueryHandler
    {

        public int insertTransaction(Transaction? transaction)
        {
            return 0;
        }
        public Transaction? getTransaction(int transactionID)
        {
            return null;
        }
    }
}
