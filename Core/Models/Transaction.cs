using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Transaction
    {
        public Guid Id { get; set; }  
        public long WalletId { get; set; }

        [JsonIgnore]
        public Wallet Wallet { get; set; }
        public decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; }

        [JsonIgnore]
        public decimal BalanceBefore { get; set; }
        [JsonIgnore]
        public decimal BalanceAfter { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
