using System;

namespace Grappachu.Interop.IbmMQ.Samples
{
    public class Message
    {
        public Message()
        {
            SentDate = DateTime.Now;
        }

        public DateTime SentDate { get; }
        public string Content { get; set; }

        public override string ToString()
        {
            return string.Format("{0:HH:mm:ss} | {1}", SentDate, Content);
        }
    }
}