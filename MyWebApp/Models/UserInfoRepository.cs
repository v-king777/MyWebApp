using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApp.Models
{
    public class UserInfoRepository : IUserInfoRepository
    {
        private readonly MyWebAppContext _context;

        public UserInfoRepository(MyWebAppContext context)
        {
            _context = context;
        }

        public async Task Add(UserInfo userInfo)
        {
            var entry = _context.Entry(userInfo);

            if (entry.State == EntityState.Detached)
            {
                await _context.UserInfos.AddAsync(userInfo);
            }

            await _context.SaveChangesAsync();
        }
    }
}
