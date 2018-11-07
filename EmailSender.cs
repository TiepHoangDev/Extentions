using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace LibraryCommon
{
    public class EmailSender
    {
        /// <summary>
        /// Send mail async.
        /// </summary>
        /// <param name="toEmail">to email</param>
        /// <param name="content">content html to send</param>
        /// <param name="subject">subject mail</param>
        /// config AppSettings["fromEmail"] and AppSettings["passwordEmail"].
        /// <returns></returns>
        public static async Task<bool> SendMail(string toEmail, object content, string subject = "THÔNG BÁO")
        {
            try
            {
                string fromEmail = ConfigurationManager.AppSettings["fromEmail"];
                string passwordEmail = ConfigurationManager.AppSettings["passwordEmail"];

                var smtp = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(fromEmail, passwordEmail),
                };
                await smtp.SendMailAsync(new MailMessage(fromEmail, toEmail, subject, content + "") { IsBodyHtml = true });
                return true;
            }
            catch (Exception ex)
            {
                ex.DebugLog("SendMail");
            }
            return false;
        }
    }
}
