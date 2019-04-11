using System.Collections.Generic;

namespace AbstractProduser.Options
{
    public class ProduserOptionAgregator
    {
        public List<KafkaProduserOption> KafkaProduserOptions { get; set; }
        public List<SignalRProduserOption> SignalRProduserOptions { get; set; }
    }
}