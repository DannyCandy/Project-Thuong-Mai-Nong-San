using Microsoft.AspNetCore.Routing.Template;
using System.Text;
using WebApp.Models;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using System.Security.Authentication;

namespace WebApp.Services
{
    public class EmailService : IEmailService
    {
        private const string templatePath = @"UserServiceTempl/{0}.html";
        private readonly SMTPConfig _smtpConfig;
        public EmailService(IOptions<SMTPConfig> smtpConfig)
        {
            _smtpConfig = smtpConfig.Value;
        }
        public async Task SendEmailForForgotPassword(UserEmailOptions userEmailOptions)
        {
            userEmailOptions.Subject = UpdatePlaceHolders("Xin chào {{UserName}}, đặt lại mật khẩu cho bạn.", userEmailOptions.PlaceHolders);

            userEmailOptions.Body = UpdatePlaceHolders(GetEmailBody("ForgotPassword"), userEmailOptions.PlaceHolders);

            await SendEmail(userEmailOptions);
        }
        private async Task SendEmail(UserEmailOptions userEmailOptions)
        {
            var builder = new BodyBuilder()
            {
                HtmlBody = userEmailOptions.Body,
            };
            MimeMessage mail = new MimeMessage()
            {
                Subject = userEmailOptions.Subject,
                Body = builder.ToMessageBody(),
            };
            mail.From.Add(new MailboxAddress(_smtpConfig.SenderDisplayName,_smtpConfig.SenderAddress));

            foreach (var toEmail in userEmailOptions.ToEmails)
            {
                mail.To.Add(new MailboxAddress("Customer",toEmail));
            }

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Connect(_smtpConfig.Host, _smtpConfig.Port, _smtpConfig.EnableSSL);
            smtpClient.Authenticate(_smtpConfig.UserName, _smtpConfig.Password);
            await smtpClient.SendAsync(mail);
        }

        private string GetEmailBody(string templateName)
        {
            var body = File.ReadAllText(string.Format(templatePath, templateName));
            return body;
        }

        private string UpdatePlaceHolders(string text, List<KeyValuePair<string, string>> keyValuePairs)
        {
            if (!string.IsNullOrEmpty(text) && keyValuePairs != null)
            {
                foreach (var placeholder in keyValuePairs)
                {
                    if (text.Contains(placeholder.Key))
                    {
                        text = text.Replace(placeholder.Key, placeholder.Value);
                    }
                }
            }

            return text;
        }
    }
}
