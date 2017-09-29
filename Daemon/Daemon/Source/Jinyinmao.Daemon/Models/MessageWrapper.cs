namespace Jinyinmao.Daemon.Models
{
    public class MessageWrapper<T> where T : class
    {
        public T MessageData { get; set; }

        public int MessageType { get; set; }
    }
}