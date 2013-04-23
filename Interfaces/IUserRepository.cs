using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using photoshare.Models;

namespace photoshare.Interfaces
{
    public interface IUsersRepository : IDalRepository<UserEntity>
    {
        PortfolioModel GetPortfolio(PortfolioModel model);
        List<FavoriteAlbumEntity> GetFavorites(string id);
        UserEntity Get(Guid id);
        string[] GetRights(string userName);
    }
}
