using Core.Models;
using Microsoft.EntityFrameworkCore;
using Services.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repositories
{
    public class WalletDataAccess : GenericDataAccess<Wallet>
    {
        private readonly AppDbContext _context;
        public WalletDataAccess(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Wallet> GetWalletByUserId(string userId)
        {
            return await _context.Wallets.Where(c => c.UserId == userId).FirstOrDefaultAsync();
        }
    }
}
