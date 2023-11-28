using System;
namespace Bankoki_client_server_.Shared
{ 

    public class Transfering
	{

        public required int Id1 { get; set; }
                
        public required string AccountType1 { get; set; }
                
        public required string AccountNumber1 { get; set; }
                
        public required int Balance1 { get; set; }
                
        public required int Id2 { get; set; }
                
        public required string AccountType2 { get; set; }
                
        public required string AccountNumber2 { get; set; }
                
        public required int Balance2 { get; set; }
  
	}
}

