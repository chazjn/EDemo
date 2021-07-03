using System.Threading.Tasks;

namespace EmailNotificationSystem
{
    public interface ISmtpClient
    {
        Task SendAsync(Email email);
    }
}
