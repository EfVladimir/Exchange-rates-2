using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace Task3
{
    public class Program
    {
        static List<TableExchangeRate> tableExchangeRates = new List<TableExchangeRate>();
        static string msg;
        static void Main(string[] args)
        {
            tableExchangeRates = DbWorks.ReadXml(out msg);
            DbWorks.CheckData();
            int num = 0;
            TimeSpan min = TimeSpan.FromMinutes(1);
            TimerCallback tm = new TimerCallback(Loop);
            Timer timer = new Timer(tm, num, TimeSpan.Zero, min);
            Console.ReadLine(); 
            SendEmailAsync(msg).GetAwaiter();
        }
        public static void Loop(object obj)
        {
            DbWorks.CheckData();
            DbWorks.Print();
            Console.WriteLine("=======================");
        }
        private static async Task SendEmailAsync(string message)
        {
            MailAddress from = new MailAddress("vova.efremov.04@mail.ru", "Vladimir");
            MailAddress to = new MailAddress("fun.player.kz@gmail.com");
            MailMessage m = new MailMessage(from, to);
            m.Subject = "Ошибка в программе";
            m.Body = message;
            SmtpClient smtp = new SmtpClient("smtp.mail.ru", 587);
            smtp.Credentials = new NetworkCredential("vova.efremov.04@mail.ru", "FwJFwuVvAmTyGBw8xydn");
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(m);
            Console.WriteLine("Письмо отправлено");
        }
    }
}