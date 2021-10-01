using System;


namespace ExchangeConnection
{
    class Program
    {
        static void Main(string[] args)
        {
            string username = "";
            string password = "";
            string domain = "";
            string mailAddress = "";
            ExchangeMailReader mailReader = new ExchangeMailReader(Microsoft.Exchange.WebServices.Data.ExchangeVersion.Exchange2010, username, password, domain, mailAddress, 1);
            mailReader.CreateExchangeServiceConnection();
            mailReader.GetMessages();
            foreach (var mail in mailReader.IncomeMessages)
            {
                if (mail == null)
                {
                    Console.WriteLine("Null");
                }
                else
                {
                    Console.WriteLine(mail.MessageDescription());
                }
            }
            Console.ReadLine();
        }
    }
}
