using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApp.Models
{
    public interface IUserInfoRepository
    {
        Task Add(UserInfo userInfo);
    }
}
