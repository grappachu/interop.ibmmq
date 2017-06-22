namespace Grappachu.Interop.IbmMQ.Samples.Queuing
{
    public class TestConsumer : IbmMqAbstractConsumer<Message>
    {
        public TestConsumer()
            : base(ReadConfig.GetConfiguration())
        {
        }
    }
}