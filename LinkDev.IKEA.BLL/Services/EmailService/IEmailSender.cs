using LinkDev.IKEA.DAL.Entities.Identity;

namespace LinkDev.IKEA.BLL.Services.EmailService
{
    public interface IEmailSender
    {
        void SendEmail(Email email);
    }
}
