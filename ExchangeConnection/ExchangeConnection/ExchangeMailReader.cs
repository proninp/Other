using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Exchange.WebServices.Data;

namespace ExchangeConnection
{
    class ExchangeMailReader
    {
        public ExchangeVersion ExchangeVers { get; set; }
        public string UserName { get; set; }
        public string Password{ get; set; }
        public string Domain { get; set; }
        public string EmailAddress { get; set; }
        public int EMailsReadingDaysPeriod { get; set; }
        public List<IncomeMessage> IncomeMessages { get; set; }
        private SearchFilter.IsGreaterThanOrEqualTo searchFilter;
        private ExchangeService exchangeService;
        public ExchangeService ExchangeService 
        {
            get => this.exchangeService;
        }

        public ExchangeMailReader()
        {

        }
        public ExchangeMailReader(ExchangeVersion exchVersion, string username, string password, string domain, string email, int readingPeriod)
        {
            ExchangeVers = exchVersion;
            UserName = username;
            Password = password;
            Domain = domain;
            EmailAddress = email;
            EMailsReadingDaysPeriod = readingPeriod > 0 ? -readingPeriod : readingPeriod;
        }
        public void CreateExchangeServiceConnection()
        {
            exchangeService = new ExchangeService(ExchangeVers);
            ExchangeService.Credentials = new WebCredentials(UserName, Password, Domain);
            ExchangeService.AutodiscoverUrl(EmailAddress);
            TimeSpan ts = new TimeSpan(EMailsReadingDaysPeriod, 0, 0, 0);
            DateTime date = DateTime.Now.Add(ts);
            searchFilter = new SearchFilter.IsGreaterThanOrEqualTo(ItemSchema.DateTimeReceived, date);
        }
        public void ReadMessages()
        {
            FindItemsResults<Item> findResults = ExchangeService.FindItems(WellKnownFolderName.Inbox, searchFilter, new ItemView(200));
            IncomeMessages = new List<IncomeMessage>();
            foreach (Item item in findResults)
            {
                EmailMessage message = EmailMessage.Bind(ExchangeService, item.Id);
                IncomeMessages.Add(new IncomeMessage(message));
            }
        }
        public int GetMessagesCount() => (IncomeMessages != null) ? IncomeMessages.Count() : 0;
        
        public IncomeMessage GetMessage(int messageIndex)
        {
            if ((IncomeMessages == null) || messageIndex >= IncomeMessages.Count)
            {
                return null;
            }
            return(IncomeMessages.ElementAt(messageIndex));
        }

    }
}
