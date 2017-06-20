namespace Grappachu.Interop.IbmMQ
{
    /// <summary>
    /// Define a set of parameters to configure a connection to a IbmMQ Exchange
    /// </summary>
    public class IbmMqSettings
    {
        /// <summary>
        /// Name of server
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// Port to be used. If null the system default will be used
        /// </summary>
        public int? Port { get; set; }

        /// <summary>
        /// username to connect the queue
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// password to connect the queue
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// the name of the queue
        /// </summary>
        public string QueueName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IbmMqConnectionType ConnectionMode { get; set; }

        /// <summary>
        /// the name of the channel
        /// </summary>
        public string Channel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string QueueManager { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? BrokerVersion { get; set; }
    }
}