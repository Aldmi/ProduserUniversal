using System;
using AbstractProduser.Enums;

namespace AbstractProduser.Options
{
    public class BaseProduserOption
    {
        public string Key { get; set; }
        public TimeSpan TimeRequest { get; set; }
        public int TrottlingQuantity { get; set; }
    }
}