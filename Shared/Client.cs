namespace Bankoki_client_server_.Shared
{
    public class User
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        = string.Empty;
        public string Password { get; set; }
        = string.Empty;
        public string Email { get; set; }
        = string.Empty;
        public bool LoggedOn { get; set; }
        public required List<Accounts> Accounts { get; set; }
    }
}
