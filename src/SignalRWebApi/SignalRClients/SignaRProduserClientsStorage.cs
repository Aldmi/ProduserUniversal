using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace WebApi.SignalRClients
{
    /// <summary>
    /// Хранит информацию о клиентах.
    /// Concurrentsy
    /// </summary>
    public class SignaRProduserClientsStorage
    {
        private readonly ConcurrentDictionary<string, SignaRClientsInfo> _clientsInfos  = new ConcurrentDictionary<string, SignaRClientsInfo>();



        #region prop

        public List<SignaRClientsInfo> GetClientsInfo => _clientsInfos.Values.ToList();
        public bool Any => _clientsInfos.Count > 0;

        #endregion



        #region Methode

        public bool AddClient(string key, SignaRClientsInfo value)
        {
           var res= _clientsInfos.TryAdd(key, value);
           return res;
        }


        public bool RemoveClient(string key)
        {
            var res = _clientsInfos.TryRemove(key, out var value);
            return res;
        }


        public bool GetClient(string key, out SignaRClientsInfo value)
        {
            var res = _clientsInfos.TryGetValue(key, out var val);
            value = val;
            return res;
        }

        #endregion
    }
}