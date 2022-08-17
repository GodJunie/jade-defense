using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace B409.Jade.Data {
    public interface ISale {
        public int BuyPrice { get; }
        public int SellPrice { get; }
    }
}