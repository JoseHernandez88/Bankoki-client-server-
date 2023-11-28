using System;
using System.Net.Mail;
using System.Net;
using System.Web;
using GemBox.Email;
using GemBox.Email.Smtp;


namespace Bankoki_client_server_.Shared
{
    public class OrdenarLibroDeCajas
    {

        public required string Email { get; set; }

        public required string Password { get; set; }

        public required string Razon { get; set; }

        public required string Direccion { get; set; }

        public required string AccountNum { get; set; }

        public class LibroDeCajasException : Exception
        {

            public string ex { get; set; } = string.Empty;
            public LibroDeCajasException(string message)

            : base(message)

            {

                this.ex = message;
            }


        }


        public OrdenarLibroDeCajas(string email, string password, string razon, string direccion, string accNum)
        {
            Email = email;
            Password = password;
            Razon = razon;
            Direccion = direccion;
            AccountNum = accNum;
            User cl = new User();

            if (cl.VerifyPassword(Password, Email))
            {
                User client = new User(Email);
                if (client.VerifyAccountOwner(AccountNum))
                {
                    string message = "Client Mx." + client.UserLastName +
                        " request an account book." +
                        " For account number: " + accNum +
                        ". Do to: " + razon +
                        "/n Email: " + client.Email +
                        "/n Mailing Address: " + direccion;
                    this.EmailHandler(message);
                }
                else
                {
                    throw new LibroDeCajasException("Client's account was not found in the database.");
                }
            }
            else
            {
                throw new LibroDeCajasException("Email and password do not match.");
            }

        }

        private void EmailHandler(string msg)
        {
			// Original code provided by GemBox @ https://www.gemboxsoftware.com/email/examples/getting-started/201.
			ComponentInfo.SetLicense("FREE-LIMITED-KEY");

			// Create new email message.
			GemBox.Email.MailMessage message = new GemBox.Email.MailMessage("bankokiservices@example.com", "bankokiservices@example.com");

			// Add subject.
			message.Subject = "Account Book Request";


			// Add text body 
			message.BodyText =
				msg+"\n" +
				"This message was created and sent with GemBox.Email (https://www.gemboxsoftware.com/email)";

			
			// Create new SMTP client and send an email message.
			using (GemBox.Email.Smtp.SmtpClient smtp = new GemBox.Email.Smtp.SmtpClient("bankokiservices@gmail.com"))
			{
				smtp.Connect();
				smtp.Authenticate("bankokiservices@gmail.com", "algobuenoyfacil01");
				smtp.SendMessage(message);
			}
		}
	}
    }
}


