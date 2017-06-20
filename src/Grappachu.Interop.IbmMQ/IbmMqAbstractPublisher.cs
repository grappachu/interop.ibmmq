using Grappachu.Core.Messaging;
using IBM.XMS;
using Newtonsoft.Json;

namespace Grappachu.Interop.IbmMQ
{
    /// <summary>
    ///     This is the base class to inherit from when creating a publihser based on IbmMQ
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class IbmMqAbstractPublisher<T> : IbmMqClientBase, IPublisher<T>
    {
        /// <summary>
        ///     Activates a new instance of <see cref="IbmMqAbstractPublisher{T}" />
        /// </summary>
        /// <param name="settings"></param>
        protected IbmMqAbstractPublisher(IbmMqSettings settings)
            : base(settings)
        {
        }

        /// <inheritdoc />
        public void Publish(T message)
        {
            var messageString = OnSerializeMessage(message);

            try
            {
                using (var connection = OnCreateConnection())
                {
                    OnPublishing(messageString, connection);
                }
            }
            catch (XMSException isex)
            {
                int errorCode;
                if (int.TryParse(isex.LinkedException.Message, out errorCode))
                    throw new QueueException(IbmMqErrorCodes.GetMessage(errorCode), isex);
                throw;
            }
        }


        private IConnection OnCreateConnection()
        {
            var cFactory = GetConnectionFactory();
            var mqConnection = cFactory.CreateConnection();
            return mqConnection;
        }

        // ReSharper disable once VirtualMemberNeverOverridden.Global
        /// <summary>
        ///     Serializes the message to a string before sending and sets its content type
        /// </summary>
        /// <param name="messageObject"></param>
        /// <returns></returns>
        protected virtual string OnSerializeMessage(T messageObject)
        {
            var messageJson = JsonConvert.SerializeObject(messageObject);
            return messageJson;
        }

        // ReSharper disable once VirtualMemberNeverOverridden.Global
        /// <summary>
        ///     Publishes a message on the exchange
        /// </summary>
        /// <param name="messageContent"></param>
        /// <param name="connection"></param>
        protected virtual void OnPublishing(string messageContent, IConnection connection)
        {
            using (var session = connection.CreateSession(false, AcknowledgeMode.AutoAcknowledge))
            using (var destination = session.CreateQueue(_settings.QueueName))
            {
                destination.SetIntProperty(XMSC.DELIVERY_MODE, (int)DeliveryMode.Persistent);

                // Publishing message
                using (var producer = session.CreateProducer(destination))
                {
                    producer.Send(session.CreateTextMessage(messageContent));
                }
            }
        }
    }
}