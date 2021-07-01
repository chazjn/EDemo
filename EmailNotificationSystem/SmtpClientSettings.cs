namespace EmailNotificationSystem
{
    public class SmtpClientSettings
    {
        public string From { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public bool Enabled { get; set; }
    }
}
