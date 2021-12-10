using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Apricode4.Models
{
    public interface IRepository
    {
        public Task CreateOrUpdateAsync(GameModel gameModel);

        public Task<List<GameModel>> ReadAsync(Guid gameId);

        public Task DelAsync(Guid gameId);

        public Task<List<GameModel>> SearchByGenreAsync(Guid genreId);
    }
}
