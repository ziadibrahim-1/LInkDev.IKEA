using LinkDev.IKEA.PL.Models.SMS;
using Twilio.Rest.Api.V2010.Account;

namespace LinkDev.IKEA.PL.Helper
{
    public interface ISMSService
    {
        MessageResource SendSMS(SMSMessage message);
    }
}
