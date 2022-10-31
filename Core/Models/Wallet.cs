using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Wallet
    {
        public Wallet()
        {
            Transactions = new List<Transaction>();
        }
        public long Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }  
        public WalletType Type { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public decimal Balance { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
