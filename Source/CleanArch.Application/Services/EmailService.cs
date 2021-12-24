using Microsoft.Extensions.Options;
using CleanArch.Domain.Dtos;
using CleanArch.Domain.Settings;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArch.Application
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> appSettingsOptions)
        {
            _emailSettings = appSettingsOptions.Value;
        }

        public async Task SendAsync(EmailMessageDto emailMessage)
        {
            // add default sender from app settings
            if (emailMessage.FromAddresses.Count == 0)
                emailMessage.FromAddresses.Add(new EmailAddressDto(_emailSettings.EmailSender, _emailSettings.EmailSender));

            var client = new SendGridClient(_emailSettings.ApiKey);
            var from = emailMessage.FromAddresses.Select(p => new EmailAddress(p.Address, p.Name)).FirstOrDefault();
            var tos = emailMessage.ToAddresses.Select(p => new EmailAddress(p.Address, p.Name)).ToList();
            //var displayRecipients = false; // set this to true if you want recipients to see each others mail id
            var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, tos, emailMessage.Subject, string.Empty, emailMessage.Body, false);
            msg.HtmlContent = emailMessage.Body;

            foreach (var ccAddress in emailMessage.CcAddresses)
            {
                msg.AddCc(ccAddress.Address, ccAddress.Name);
            }

            foreach (var attachment in emailMessage.Attachments)
            {
                msg.AddAttachment(attachment.FileName, attachment.Base64Content);
            }

            var response = await client.SendEmailAsync(msg);
        }
    }
}