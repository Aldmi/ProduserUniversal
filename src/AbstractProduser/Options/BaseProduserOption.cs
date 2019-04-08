using System;

namespace AbstractProduser.Options
{
    public class BaseProduserOption
    {
        public TimeSpan TimeRequest { get; set; }
        public int TrottlingQuantity { get; set; }
    }
}