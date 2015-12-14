using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using RealtyStore.Domain.Abstract;
using RealtyStore.Domain.Entities;
using System.Globalization;

namespace RealtyStore.Domain.Concrete
{
    public class EmailSettings
    {
        public string MailToAddress = "orders@example.com";
        public string MailFromAddress = "gamestore@example.com";
        public bool UseSsl = true;
        public string Username = "MySmtpUsername";
        public string Password = "MySmtpPassword";
        public string ServerName = "smtp.example.com";
        public int ServerPort = 587;
        public bool WriteAsFile = true;
        public string FileLocation = @"c:\realty_store_emails";
    }

    public class EmailOrderProcessor : IOrderProcessor
    {
        private EmailSettings emailSettings;

        public EmailOrderProcessor(EmailSettings settings)
        {
            emailSettings = settings;
        }

        void IOrderProcessor.ProcessOrder(Cart cart, ShippingDetails shippingInfo)
        {
            // Для отображения $ в сообщении
            CultureInfo.CurrentCulture = new CultureInfo("en-US");

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = emailSettings.UseSsl;
                smtpClient.Host = emailSettings.ServerName;
                smtpClient.Port = emailSettings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials
                    = new NetworkCredential(emailSettings.Username, emailSettings.Password);

                if (emailSettings.WriteAsFile)
                {
                    smtpClient.DeliveryMethod
                        = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = emailSettings.FileLocation;
                    smtpClient.EnableSsl = false;
                }

                StringBuilder body = new StringBuilder()
                    .AppendLine("Новый заказ обработан")
                    .AppendLine("---")
                    .AppendLine("Товары");

                foreach (var line in cart.Lines)
                {
                    var subtotal = line.Realty.Price * line.Quantity;
                    body.AppendFormat("{0} x {1} (итого: {2:c2})",
                        line.Quantity, line.Realty.Name, subtotal);
                }

                body.AppendFormat("Общая стоимость: {0:c2}", cart.ComputeTotalValue())
                    .AppendLine("---")
                    .AppendLine("Доставка:")
                    .AppendLine( shippingInfo.Name)
                    .AppendLine(shippingInfo.Line1)
                    .AppendLine(shippingInfo.Line2 ?? "")
                    .AppendLine(shippingInfo.Line3 ?? "")
                    .AppendLine(shippingInfo.City)
                    .AppendLine(shippingInfo.Country);

                MailMessage mailMessage = new MailMessage(
                    emailSettings.MailFromAddress, //от кого
                    emailSettings.MailToAddress, //кому
                    "Новый заказ отправлен!", //тема
                    body.ToString()); //тело письма

                if (emailSettings.WriteAsFile)
                {
                    mailMessage.BodyEncoding = Encoding.UTF8;
                }

                smtpClient.Send(mailMessage);
            }
        }
    }
}
