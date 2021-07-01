using System;
using System.Collections.Generic;
using System.Text;

namespace EmailNotificationSystem
{
    public interface ISmtpClient
    {
        void Send(Email email);
    }
}
