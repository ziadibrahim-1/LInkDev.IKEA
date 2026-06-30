using LinkDev.IKEA.PL.Models.SMS;
using LinkDev.IKEA.PL.Settings;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace LinkDev.IKEA.PL.Helper
{
    public class SMSService : ISMSService
    {
        private readonly IOptions<SMSSettings> _options;

        public SMSService(IOptions<SMSSettings> options)
        {
           _options = options;
        }
        public MessageResource SendSMS(SMSMessage message)
        {
            TwilioClient.Init(_options.Value.AccountSID , _options.Value.AuthToken);
            var smsMessage = MessageResource.Create(body: message.Body,
                from: new Twilio.Types.PhoneNumber(_options.Value.FromNumber),
                to:new Twilio.Types.PhoneNumber(message.PhoneNumber)
            );
            return smsMessage;
        }
    }
}
