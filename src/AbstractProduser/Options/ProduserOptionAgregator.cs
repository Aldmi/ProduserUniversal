using System.Collections.Generic;

namespace AbstractProduser.Options
{
    public class ProduserOptionAgregator
    {
        public List<KafkaProduserOption> KafkaProduserOptions { get; set; } = new List<KafkaProduserOption>();
        public List<SignalRProduserOption> SignalRProduserOptions { get; set; }= new List<SignalRProduserOption>();
    }
}