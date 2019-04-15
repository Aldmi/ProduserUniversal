using System;
using AbstractProduser.Enums;

namespace AbstractProduser.Options
{
    public class BaseProduserOption
    {
        public ProduserType ProduserType { get; set; }
        public string Key { get; set; }

        public TimeSpan TimeRequest { get; set; }
        public int TrottlingQuantity { get; set; }
    }
}