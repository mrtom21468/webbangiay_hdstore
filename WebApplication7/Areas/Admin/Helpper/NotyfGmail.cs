using System.Net;
using System.Net.Mail;

namespace WebApplication7.Areas.Admin.Helpper
{
    public static class NotyfGmail
    {
        public static void SenGmail(string? toemail, int? ordernumber, string? thongtin)
        {
            string smtpServer = "smtp.gmail.com";
            int smtpPort = 587;
            string smtpUsername = "hieu2001zzz@gmail.com";
            string smtpPassword = "mazw smly oejs yczf";

            string fromEmail = "hieu2001zzz@gmail.com";
            string toEmail = toemail;

            try
            {
                    MailMessage mail = new MailMessage(fromEmail, toEmail);
                    mail.Subject = "Thông báo đơn hàng";
                    mail.IsBodyHtml = true;
                    string body = string.Format("<p>Mã đơn hàng của bạn là: <i>{0}</i></p> Thông tin đơn hàng: {1}\n", ordernumber,thongtin);
                    mail.Body = "<h1 style=\"color: green;\">HD STORE</h1>\n" + body;
                    SmtpClient smtpClient = new SmtpClient(smtpServer);
                    smtpClient.Port = smtpPort;
                    smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                    smtpClient.EnableSsl = true; // Sử dụng TLS
                    smtpClient.Send(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static bool SenGmailHoTro(string? toemail,string?name ,string? thongtin)
        {
            string smtpServer = "smtp.gmail.com";
            int smtpPort = 587;
            string smtpUsername = "hieu2001zzz@gmail.com";
            string smtpPassword = "mazw smly oejs yczf";

            string fromEmail = "hieu2001zzz@gmail.com";
            string toEmail = toemail;

            try
            {
                MailMessage mail = new MailMessage(fromEmail, toEmail);
                mail.Subject = "Thông tin hỗ trợ";
                mail.IsBodyHtml = true;
                string body = string.Format("<p>Cảm ơn <i>{0}</i> đã để lại thông tin hỗ trợ!\n </p> Thông tin hỗ trợ của bạn đã gửi là: {1}\n",name, thongtin);
                mail.Body = "<h1 style=\"color: green;\">HD STORE</h1>\n" + body;
                SmtpClient smtpClient = new SmtpClient(smtpServer);
                smtpClient.Port = smtpPort;
                smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                smtpClient.EnableSsl = true; // Sử dụng TLS
                smtpClient.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
