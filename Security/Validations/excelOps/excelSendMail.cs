using System.Net.Mail;
using System.Net;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Remake.Security.Validations.excelOps
{
    public class excelSendMail
    {
        public void SendXLMail(string UsersMail,string requsterName,string kesifAdi,string path) 
        {
            SmtpClient sc = new SmtpClient();
            sc.Port = 587;
            sc.Host = "smtp.outlook.com";
            sc.EnableSsl = true;
            sc.Credentials = new NetworkCredential("mwiertbot011@outlook.com", "mwiertbot11");

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("mwiertbot011@outlook.com", "Protalya Bilgi Teknolojileri");
            mail.To.Add(UsersMail);
            mail.Subject = "Döküm Tablosu ";
            mail.IsBodyHtml = true;
            mail.Body = requsterName + " " + "İsteği üzerine"+" "+ kesifAdi +" " + "Mali Döküm Tablosu ";
            Attachment attach;
            attach = new System.Net.Mail.Attachment(path);
            mail.Attachments.Add(attach);

            sc.Send(mail);
            
        }
    }
}
