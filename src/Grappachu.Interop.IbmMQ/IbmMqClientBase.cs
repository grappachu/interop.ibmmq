using IBM.XMS;


namespace Grappachu.Interop.IbmMQ
{
    /// <summary>
    ///     Defines a basic client for both publisher and consumer
    /// </summary>
    public abstract class IbmMqClientBase
    {
        /// <summary>
        ///     Gets the settings collection used by for this client
        /// </summary>
        protected readonly IbmMqSettings _settings;

        /// <summary>
        ///     Acrivates a new instance of <see cref="IbmMqClientBase" />
        /// </summary>
        /// <param name="settings"></param>
        protected IbmMqClientBase(IbmMqSettings settings)
        {
            _settings = settings;
        }

        /// <summary>
        ///     Gets a new connection factory for this client
        /// </summary>
        /// <returns></returns>
        protected IConnectionFactory GetConnectionFactory()
        {
            var cf = XMSFactoryFactory.GetInstance(XMSC.CT_WMQ).CreateConnectionFactory();
            cf.SetStringProperty(XMSC.WMQ_HOST_NAME, _settings.HostName);
            if (_settings.Port.HasValue)
                cf.SetIntProperty(XMSC.WMQ_PORT, _settings.Port.Value);
            cf.SetStringProperty(XMSC.WMQ_CHANNEL, _settings.Channel);
            cf.SetIntProperty(XMSC.WMQ_CONNECTION_MODE, (int)_settings.ConnectionMode);
            cf.SetStringProperty(XMSC.WMQ_QUEUE_MANAGER, _settings.QueueManager);
            if (_settings.BrokerVersion.HasValue)
                cf.SetIntProperty(XMSC.WMQ_BROKER_VERSION, _settings.BrokerVersion.Value);

            cf.SetStringProperty(XMSC.WMQ_CONNECTION_NAME_LIST,
                $"{_settings.HostName}({_settings.Port.GetValueOrDefault(0)})");

            if (_settings.ConnectionMode != IbmMqConnectionType.Local)
            {
                cf.SetStringProperty(XMSC.USERID, _settings.User);
                cf.SetStringProperty(XMSC.PASSWORD, _settings.Password);
            }

            return cf;
        }
    }
}