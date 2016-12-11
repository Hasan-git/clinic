using System.Net.Mail;

namespace Clinic.Common.Core.Services.Web
{
    public interface IMailSender
    {
        string SmtpUsername { get; set; }
        string SmtpPassword { get; set; }
        void SendMail(MailMessage message);
    }
}