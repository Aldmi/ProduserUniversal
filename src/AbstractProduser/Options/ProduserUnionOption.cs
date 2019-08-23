﻿using System.Collections.Generic;

namespace AbstractProduser.Options
{
    public class ProduserUnionOption
    {
        /// <summary>
        /// Ключ по которому хранится ProdusersUnion в ProdusersStorage в CWS
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Имя преобразователя типа ответа.
        /// </summary>
        public string ConverterName { get; set; }

        public List<KafkaProduserOption> KafkaProduserOptions { get; set; } = new List<KafkaProduserOption>();
        public List<SignalRProduserOption> SignalRProduserOptions { get; set; }= new List<SignalRProduserOption>();
        public List<WebClientProduserOption> WebClientProduserOptions { get; set; } = new List<WebClientProduserOption>();
    }
}