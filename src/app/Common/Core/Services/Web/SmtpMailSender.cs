using System.Net;
using System.Net.Mail;
using Clinic.Common.Core.Services.Configuration;

namespace Clinic.Common.Core.Services.Web
{
    public class SmtpMailSender : IMailSender
    {
        private readonly SmtpClient _client;
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }

        public SmtpMailSender(IApplicationSettings applicationSettings)
        {
            IApplicationSettings appSettings = applicationSettings;
            string smtpServer = appSettings.SmtpServer;
            int smtpServerPort = appSettings.SmtpServerPort;

            _client = new SmtpClient(smtpServer, smtpServerPort);

            if (appSettings.GetSmtpAuthenticationNecessary())
            {
                _client.UseDefaultCredentials = false;
                var smtpUserInfo = new NetworkCredential(SmtpUsername ?? appSettings.GetSmtpUsername(),
                                                         SmtpPassword ?? appSettings.GetSmtpPassword());
                _client.Credentials = smtpUserInfo;
                _client.EnableSsl = appSettings.GetSmtpServerRequireSsl();
            }
        }

        public void SendMail(MailMessage mailMessage)
        {
            _client.Send(mailMessage);
        }
    }
}
