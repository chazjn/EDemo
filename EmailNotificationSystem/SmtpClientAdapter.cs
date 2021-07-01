using System.Net.Mail;

namespace EmailNotificationSystem
{
    public class SmtpClientAdapter : ISmtpClient
    {
        private readonly SmtpClientSettings _settings;

        public SmtpClientAdapter(SmtpClientSettings smtpClientSettings)
        {
            _settings = smtpClientSettings;
        }

        public void Send(Email email)
        {
            if(_settings.Enabled == false)
            {
                return;
            }
            
            using var smtpClient = new SmtpClient(_settings.SmtpServer, _settings.Port);
            using var message = new MailMessage(_settings.From, email.To)
            {
                Body = email.Body
            };
            smtpClient.SendMailAsync(message);
        }
    }
}
