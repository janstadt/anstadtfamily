using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using photoshare.Models;

namespace photoshare.Interfaces
{
    public interface IUsersRepository : IDalRepository<UserEntity>
    {
        UserEntity Get(string Username);
        string[] GetRights(string userName);
    }
}
