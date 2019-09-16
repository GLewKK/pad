namespace MessageChannel.Models
{
    public class MessageResult
    {
        public MessageType Type { get; set; }
        public string Text { get; set; }
        public bool isSuccess { get; set; }
    }
}
