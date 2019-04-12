namespace WebApi.SignalRClients
{
    public class SignaRClientsInfo
    {
        public string ClientId { get; }
        public string GroupName { get; }
        public string UserName { get; }


        public SignaRClientsInfo(string clientId, string groupName)
        {
            ClientId = clientId;
            GroupName = groupName;
        }
    }
}