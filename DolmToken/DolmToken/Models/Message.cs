
namespace DolmToken.Models
{
    public class Message
    {
        public string Header { get; set; }
        public string MessageText { get; set; }

        public string Solution { get; set; }

        public Message() : this("", "", "") { }
        // für Erfolgsmeldungen
        public Message(string header, string message)
            : this(header, message, "") { }
        // für Fehlermeldungen
        public Message(string header, string message, string resolution)
        {
            this.Header = header;
            this.MessageText = message;
            this.Solution = resolution;
        }
    }
}