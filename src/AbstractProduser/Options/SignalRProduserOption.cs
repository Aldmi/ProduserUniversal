namespace AbstractProduser.Options
{
    public class SignalRProduserOption : BaseProduserOption
    {
        public string HubEndpoints { get; set; }
        public string MethodeName { get; set; }
    }
}