namespace Grappachu.Interop.IbmMQ
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QueueMessage<T>
    {
        /// <summary>
        /// Indica se per questo messaggio verrà inviato Ack (true) o Nack (false)
        /// </summary>
        public bool? GiveAck { get; set; }

        /// <summary>
        /// identifier of the message
        /// </summary>
        public string MessageId { get; }

        /// <summary>
        /// 
        /// </summary>
        public T Value { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="value"></param>
        public QueueMessage(string messageId, T value)
        {
            MessageId = messageId;
            Value = value;
        }
    }
}