using KYC_Form.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace KYC_Form.DBIntialize
{
    public class DBInitializer : IDBInitializer
    {
        private readonly AppDbContext _context;

        public DBInitializer(AppDbContext context)
        {
            _context = context;
        }

        public void Initialize()
        {
            try
            {
                if (_context.Database.GetPendingMigrations().Count() > 0)
                {
                    _context.Database.Migrate();
                }
            }
            catch
            {
            }
        }
    }
}