using System.Net;
using System.Net.Mail;

namespace Remake.Security.Validations.ResetPassword
{
    public class rePassword
    {
        bool isTrue = false;
        static int number;
        public void SendVerifyMail(string UsersMail)
        {
            Random rnd = new Random();
            number = rnd.Next(10000, 90000);

            SmtpClient sc = new SmtpClient();
            sc.Port = 587;
            sc.Host = "smtp.outlook.com";
            sc.EnableSsl = true;
            sc.Credentials = new NetworkCredential("mwiertbot011@outlook.com", "mwiertbot11");

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("mwiertbot011@outlook.com", "Protalya Bilgi Teknolojileri");
            mail.To.Add(UsersMail);
            mail.Subject = "Doğrulama Kodu";
            mail.IsBodyHtml = true;
            mail.Body = number.ToString();
            sc.Send(mail);

        }
        public bool CheckVerifyCode(int Code)
        {
            if (Code == number)
            {
                isTrue = true;
                return isTrue;
            }
            else {
                isTrue = false;
                return isTrue;
            }
        }
    }
}
