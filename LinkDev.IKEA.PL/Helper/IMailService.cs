using LinkDev.IKEA.DAL.Entities.Identity;

namespace LinkDev.IKEA.PL.Helper
{
    public interface IMailService
    {
         void Send(Email email);
    }
}
