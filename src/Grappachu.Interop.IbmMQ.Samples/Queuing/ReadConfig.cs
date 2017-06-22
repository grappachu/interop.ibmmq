using System.Configuration;

namespace Grappachu.Interop.IbmMQ.Samples.Queuing
{
    internal static class ReadConfig
    {
        public static IbmMqSettings GetConfiguration()
        {
            return new IbmMqSettings
            {
                HostName = ConfigurationManager.AppSettings["IBM.MQ.HostName"],
                User = ConfigurationManager.AppSettings["IBM.MQ.User"],
                Password = ConfigurationManager.AppSettings["IBM.MQ.Password"],
                QueueName = ConfigurationManager.AppSettings["IBM.MQ.QueueName"],
                Channel = ConfigurationManager.AppSettings["IBM.MQ.Channel"],
                QueueManager = ConfigurationManager.AppSettings["IBM.MQ.QueueManager"],
              //  Port = int.Parse(ConfigurationManager.AppSettings["IBM.MQ.Port"]),
                ConnectionMode = IbmMqConnectionTypeParser.Parse(ConfigurationManager.AppSettings["IBM.MQ.ConnectionMode"])
            };
        }
    }
}