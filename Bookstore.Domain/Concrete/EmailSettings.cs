using Bookstore.Domain.Adstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookstore.Domain.Entities;
using System.Net;
using System.Net.Mail;
using System.Diagnostics;

namespace Bookstore.Domain.Concrete
{
    public class EmailSettings
    {
        public string MailToAddress = "order@xBookstore.com";
        public string MailFromAddress = "infoxBookstore@gmail.com";
        public bool UseSsl = true;
        public string Username = "infoxBookstore@gmail.com";
        public string Password = "myPassword";
        public string ServerName = "smtp.gamil.com";
        public int ServerPort = 578;
        public bool WriteAsFile = false;
        public string FileLocation = @"c:\order_bookstore_emails";
    }
    public class EmailOrderProcessor : IOrderProcessor
    {
        private EmailSettings emailSetting;
        public EmailOrderProcessor(EmailSettings setting)
        {
            emailSetting = setting;
        }
        public void ProcessorOrder(Cart cart, ShippingDetails ShippingDetails)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = emailSetting.UseSsl;
                smtpClient.Host = emailSetting.ServerName;
                smtpClient.Port = emailSetting.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(emailSetting.Username, emailSetting.Password);
                if(emailSetting.WriteAsFile)
                {
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = emailSetting.FileLocation;
                    smtpClient.EnableSsl = false;
                }
                StringBuilder body = new StringBuilder()
                    .AppendLine("A new order has been submitted")
                    .AppendLine("----------")
                    .AppendLine("Books: ");
                foreach(var line in cart.Lines)
                {
                    var subtotal = line.Book.Price * line.Quantity;
                    body.AppendFormat("{0} x {1} (subtotal: {2:c})", line.Quantity, line.Book.Title, subtotal);
                }
                body.AppendFormat("Total order Value:{0:c}", cart.ComputeTotalValue())
                    .AppendLine("-------------")
                    .AppendLine("Ship to:")
                    .AppendLine(ShippingDetails.Name)
                    .AppendLine(ShippingDetails.Line1)
                    .AppendLine(ShippingDetails.Line2)
                    .AppendLine(ShippingDetails.State)
                    .AppendLine(ShippingDetails.City)
                    .AppendLine(ShippingDetails.Country)
                    .AppendLine("-----")
                    .AppendFormat("Gift Wrap:{0}", ShippingDetails.GiftWrap ? "Yes" : "No");
                MailMessage mailMessage = new MailMessage(emailSetting.MailFromAddress, emailSetting.MailToAddress, "New order submitted", body.ToString());
                if(emailSetting.WriteAsFile)
                mailMessage.BodyEncoding = Encoding.ASCII;
                try
                { 
                    smtpClient.Send(mailMessage);
                }
                catch(Exception ex)
                {
                    Debug.Print(ex.Message);
                }
            }
        }       
    }
}
