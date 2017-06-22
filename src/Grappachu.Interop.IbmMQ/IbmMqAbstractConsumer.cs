using System;
using System.Timers;
using Grappachu.Core.Messaging;
using IBM.XMS;
using Newtonsoft.Json;

namespace Grappachu.Interop.IbmMQ
{
    /// <summary>
    ///     This is the base class to inherit from when creating a consumer based on IbmMQ
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class IbmMqAbstractConsumer<T> : IbmMqClientBase, IConsumer<T>
    {
        private readonly Timer _timer;

        private IConnection _connection;
        private IDestination _destination;
        private ISession _session;


        /// <summary>
        ///     Acrivates a new instance of <see cref="IbmMqAbstractConsumer{T}" />
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="pollingTimeMills"></param> 
        protected IbmMqAbstractConsumer(IbmMqSettings settings, int pollingTimeMills = 2000)
            : base(settings)
        {
            _timer = new Timer
            {
                Interval = pollingTimeMills
            };
            _timer.Elapsed += OnConsumingMessages;
        }

       /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        ///     Gets multiple rows of received data and cosumes them
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="elapsedEventArgs"></param>
        /// <returns></returns>
        private void OnConsumingMessages(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            try
            {
                string consumedMessage;
                do
                {
                    consumedMessage = OnConsumingSingleMessage(_session, _destination);
                } while (consumedMessage != null);
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
        }


        /// <summary>
        ///     Gets a single row of received data and cosumes it
        /// </summary>
        /// <param name="session"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        private string OnConsumingSingleMessage(ISession session, IDestination destination)
        {
            using (var consumer = session.CreateConsumer(destination))
            {
                var msgReceived = (ITextMessage)consumer.ReceiveNoWait();
                if (msgReceived != null)
                {
                    var msgId = msgReceived.JMSMessageID;
                    var obj = OnDeserializeMessage(msgReceived.Text);

                    var queueMessage =  new Envelope<T>(msgId, obj);
                 
                    MessageReceived?.Invoke(this, queueMessage);
                    if (queueMessage.GiveAck.GetValueOrDefault())
                        msgReceived.Acknowledge();

                    return msgId;
                }
                return null;
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
        ///     Deserialize the received message into {T}
        /// </summary>
        /// <param name="messageContent"></param>
        /// <returns></returns>
        protected virtual T OnDeserializeMessage(string messageContent)
        {
            var messageObject = JsonConvert.DeserializeObject<T>(messageContent);
            return messageObject;
        }

        /// <inheritdoc />
        protected virtual void OnError(Exception exception)
        {
            throw exception;
        }


        /// <inheritdoc />
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _connection?.Dispose();
                _destination?.Dispose();
                _session?.Dispose();
                _timer?.Dispose();
            }
        }

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<Envelope<T>> MessageReceived;

        /// <summary>
        /// 
        /// </summary>
        public void BeginConsume()
        {
            _connection = OnCreateConnection();
            _session = _connection.CreateSession(false, AcknowledgeMode.ClientAcknowledge);
            _destination = _session.CreateQueue(_settings.QueueName);
            // Start the connection and the timer to receive messages.
            _connection.Start();

            _timer.Enabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void EndConsume()
        {
            _timer.Enabled = false;

            _destination.Dispose();
            _session.Dispose();
            _connection.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsConsuming
        {
            get { return _timer.Enabled; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ConsumeOnce()
        {
            using (var conn = OnCreateConnection())
            {
                using (var session = conn.CreateSession(false, AcknowledgeMode.ClientAcknowledge))
                {
                    using (var destination = session.CreateQueue(_settings.QueueName))
                    {
                        conn.Start();

                        var msgId = OnConsumingSingleMessage(session, destination);
                        return !string.IsNullOrEmpty(msgId);
                    }
                }
            }
        }

        #endregion
    }
}