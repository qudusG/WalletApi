using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repositories
{
    public class TransactionDataAccess : GenericDataAccess<Transaction>
    {
        private readonly AppDbContext _context;
        public TransactionDataAccess(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<Transaction>> GetByWalletId(long walletId, DateTime? start, DateTime? end)
        {
            var transactions = _context.Transactions.Where(c => c.WalletId == walletId);
            if (start.HasValue && end.HasValue)
                transactions = transactions.Where(c => c.TransactionDate.Date >= start.Value.Date 
                && c.TransactionDate.Date <= end.Value.Date);
            if (start.HasValue && !end.HasValue)
                transactions = transactions.Where(c => c.TransactionDate.Date >= start.Value.Date);

            if (end.HasValue && !start.HasValue)
                transactions = transactions.Where(c => c.TransactionDate.Date <= end.Value.Date);

            return await transactions.ToListAsync();

        }
    }
}
