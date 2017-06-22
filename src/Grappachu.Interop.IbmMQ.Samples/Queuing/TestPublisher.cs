namespace Grappachu.Interop.IbmMQ.Samples.Queuing
{
    public class TestPublisher : IbmMqAbstractPublisher<Message>
    {
        public TestPublisher()
            : base(ReadConfig.GetConfiguration())
        {
        }
    }
}