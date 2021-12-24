using CleanArch.Domain.Dtos;
using System.IO;

namespace CleanArch.Domain.Extensions
{
    public static class EmailMessageDtoExtension
    {
        private static string newUserEmailTemplate;
        private static string newUserConfirmationEmailTemplate;
        private static string newUserNotificationEmailTemplate;
        private static string passwordResetTemplate;
        private static string forgotPasswordTemplate;

        public static EmailMessageDto BuildNewUserConfirmationEmail(this EmailMessageDto emailMessage, string recepientName, string userName, string callbackUrl, string userId, string token)
        {
            if (newUserConfirmationEmailTemplate == null)
                newUserConfirmationEmailTemplate = ReadPhysicalFile("Templates/NewUserConfirmationEmail.template");

            emailMessage.Body = newUserConfirmationEmailTemplate
                //.Replace("{name}", recepientName) // Uncomment if you want to add name to the registration form
                .Replace("{userName}", userName)
                .Replace("{callbackUrl}", callbackUrl)
                .Replace("{userId}", userId)
                .Replace("{token}", token);

            emailMessage.Subject = $"Welcome {recepientName}";

            return emailMessage;
        }

        public static EmailMessageDto BuildNewUserEmail(this EmailMessageDto emailMessage, string fullName, string userName, string emailAddress, string password)
        {
            if (newUserEmailTemplate == null)
                newUserEmailTemplate = ReadPhysicalFile("Templates/NewUserEmail.template");

            emailMessage.Body = newUserEmailTemplate
                //.Replace("{fullName}", fullName) // Uncomment if you want to add name to the registration form has First / Last Name
                .Replace("{fullName}", userName) //Comment out if you want have First / Last Name in registration form.
                .Replace("{userName}", userName)
                .Replace("{email}", emailAddress)
                .Replace("{password}", password);

            emailMessage.Subject = $"Welcome {fullName}";

            return emailMessage;
        }

        public static EmailMessageDto BuilNewUserNotificationEmail(this EmailMessageDto emailMessage, string creator, string name, string userName, string company, string roles)
        {
            //placeholder not actually implemented
            if (newUserNotificationEmailTemplate == null)
                newUserNotificationEmailTemplate = ReadPhysicalFile("Templates/NewUserEmail.template");

            emailMessage.Body = newUserNotificationEmailTemplate
                .Replace("{creator}", creator)
                .Replace("{name}", name)
                .Replace("{userName}", userName)
                .Replace("{roles}", roles)
                .Replace("{company}", company);

            emailMessage.Subject = $"A new user [{userName}] has registered";

            return emailMessage;
        }

        public static EmailMessageDto BuildForgotPasswordEmail(this EmailMessageDto emailMessage, string name, string callbackUrl, string userId, string token)
        {
            if (forgotPasswordTemplate == null)
                forgotPasswordTemplate = ReadPhysicalFile("Templates/ForgotPassword.template");

            emailMessage.Body = forgotPasswordTemplate
                .Replace("{name}", name)
                .Replace("{userId}", userId)
                .Replace("{token}", token)
                .Replace("{callbackUrl}", callbackUrl);

            emailMessage.Subject = string.Format("Forgot your Password? [{0}]", name);

            return emailMessage;
        }

        public static EmailMessageDto BuildPasswordResetEmail(this EmailMessageDto emailMessage, string userName)
        {
            if (passwordResetTemplate == null)
                passwordResetTemplate = ReadPhysicalFile("Templates/PasswordReset.template");

            emailMessage.Body = passwordResetTemplate
                .Replace("{userName}", userName);

            emailMessage.Subject = string.Format("Password Reset for {0}", userName);

            return emailMessage;
        }

        private static string ReadPhysicalFile(string path)
        {
            return File.ReadAllText(path);

            //var fileInfo = new FileInfo(path);
            //if (!fileInfo.Exists)
            //    throw new FileNotFoundException($"Template file located at \"{path}\" was not found");

            //using var fs = fileInfo.CreateReadStream();
            //using var sr = new StreamReader(fs);
            //return sr.ReadToEnd();
        }
    }
}