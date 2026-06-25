using LinkDev.IKEA.DAL.Entities.Identity;
using System.Net;
using System.Net.Mail;

namespace LinkDev.IKEA.BLL.Services.EmailService
{
    public class EmailService : IEmailSender
    {
        public void SendEmail(Email email)
        {
            var client = new SmtpClient("smtp.gmail.com" , 587) ;// TLS port
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("ziadibr2912@gmail.com", "zldzewwcwswsanlq");
            client.Send("ziadibr2912@gmail.com", email.To, email.Subject, email.Body);
        }
    }
}
