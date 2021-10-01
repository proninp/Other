using Microsoft.Exchange.WebServices.Data;
using System.Text;

namespace ExchangeConnection
{
    public class IncomeMessage
    {
        public string MessageID { get; set; }
        public string ConversationId { get; set; }
        public string DateTimeReceived { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Subject { get; set; }
        public string BodyText { get; set; }
        public bool HasAttachments { get; set; }
        public IncomeMessage()
        {

        }
        public IncomeMessage(EmailMessage message)
        {
            if (message != null)
            {
                MessageID = message.Id.ToString();
                DateTimeReceived = message.DateTimeReceived.ToString();
                Name = message.From.Name.ToString();
                Address = message.From.Address.ToString();
                Subject = message.Subject;
                HasAttachments = message.HasAttachments;
                BodyText = "<!DOCTYPE HTML>\n" + message.Body.Text;
                ConversationId = message.ConversationId;
            }

        }
        public string MessageDescription()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"{Name};\t{Address};\t{DateTimeReceived};\t{Subject}\t");
            stringBuilder.Append((HasAttachments ? "Yes" : "No"));
            stringBuilder.Append($"Conversation ID: {ConversationId}");
            stringBuilder.Append("\t");
            stringBuilder.Append(BodyText);
            stringBuilder.Append("\t");
            return (stringBuilder.ToString());
        }

    }
}
