using Demo.DAL.Models;
using System.Net;
using System.Net.Mail;

namespace Demo.PL.Helper
{
    public class EmailSettings
    {
        public static void SendEmail(Email email)
        {
            string HostName = "smtp.gmail.com", SenderUserName = "Mohamed.kamaldev24@gmail.com", AppPass = "nugsazuepabsbbza";
            var Client = new SmtpClient(HostName, 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(SenderUserName, AppPass)
            };

            Client.Send(SenderUserName, email.Recipient, email.Subject, email.Body);

        }
    }
}
