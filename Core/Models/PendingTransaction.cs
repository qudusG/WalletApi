using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class PendingTransaction
    {
        public long Id { get; set; }   
        public long WalletId { get; set; }
        public Wallet Wallet { get; set; }
        public decimal Amount { get; set; }
    }
}
