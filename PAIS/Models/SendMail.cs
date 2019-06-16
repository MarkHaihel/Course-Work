using System.Net.Mail;
using System.Net;
using System;

namespace PAIS.Models
{
    public class SendMail
    {
        private FeedBack feedBack;

        public SendMail(FeedBack feedBack)
        {
            this.feedBack = feedBack;
        }

        public void SendFeedBack()
        {
            MailAddress from = new MailAddress(feedBack.Mail, feedBack.Name);
            MailAddress to = new MailAddress("markiyanse@gmail.com");
            MailMessage message = new MailMessage(from, to);

            message.Subject = feedBack.Subject;
            message.Body = "<p>" + feedBack.Message + "</p>";
            message.Body += string.Format("<p>------Sent from the Site------<br>" +
                "From: {0}<br>To: {1}<br>Date: {2}<br>Subject: {3}</p>",
                feedBack.Mail, to.Address, DateTime.Now.ToString("MM/dd/yyyy"), feedBack.Subject);
            message.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("paisfeedback@gmail.com", "PAIS123456");
            smtp.EnableSsl = true;

            smtp.Send(message);
        }
    }
}
