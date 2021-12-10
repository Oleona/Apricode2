using Apricode4.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apricode4.Models
{
    public class GameRepository : IRepository
    {
        private readonly VideoGameShopContext dbContext;
        public GameRepository(VideoGameShopContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateOrUpdateAsync(GameModel gameModel)
        {
            if (gameModel == null)
            {
                throw new ArgumentNullException($"{gameModel}");
            }

            var dbGame = await dbContext.VideoGames.SingleOrDefaultAsync(x => x.Id == gameModel.Id);
            if (dbGame == null)
            {
                var newDbGame = await CreateNewGame(gameModel);
                dbContext.Add(newDbGame);
            }
            else
            {
                await UpdateExistingGame(dbGame, gameModel);
                dbContext.Update(dbGame);         
            }

            await dbContext.SaveChangesAsync();
        }

        public async Task<List<GameModel>> SearchByGenreAsync(Guid genreId)
        {
            var dbGames = await dbContext.VideoGames
                .Include(c => c.Genres)
                .Where(c => c.Genres.Select(g => g.Id).Contains(genreId))
                .ToListAsync();

            return dbGames.Select(dbGame => ConvertEntityToModel(dbGame)).ToList();
        }

        public async Task<List<GameModel>> ReadAsync(Guid gameId)
        {
            var dbGames = await dbContext.VideoGames
                .Include(c => c.Genres)
                .Where(c => c.Id == gameId)
                .ToListAsync();

            return dbGames.Select(dbGame => ConvertEntityToModel(dbGame)).ToList();
        }

        public async Task DelAsync(Guid gameId)
        {
            var dbGame = await dbContext.VideoGames
                .Include(c => c.Genres)
                .SingleOrDefaultAsync(c => c.Id == gameId);

            if (dbGame == null)
            {
                return;
            }

            dbContext.Remove(dbGame);
            await dbContext.SaveChangesAsync();
        }

        private async Task<VideoGame> CreateNewGame(GameModel gameModel)
        {
            var genres = await FindGenresByModel(gameModel);

            return new VideoGame
            {
                Id = gameModel.Id,
                GameName = gameModel.GameName,
                StudioDeveloper = gameModel.StudioDeveloper,
                Genres = genres,
            };
        }

        private async Task UpdateExistingGame(VideoGame dbGame, GameModel gameModel)
        {
            var allGenres = await FindGenresByModel(gameModel);
           // var newGenres = allGenres.Except(dbGame.Genres).ToList();//

            dbGame.GameName = gameModel.GameName;
            dbGame.StudioDeveloper = gameModel.StudioDeveloper;
           // dbGame.Genres.RemoveAll(_ => true);//
          //  dbGame.Genres.AddRange(newGenres);//
            dbGame.Genres = allGenres;

            
        }

        private async Task<List<Genre>> FindGenresByModel(GameModel gameModel) =>
            await dbContext.Genres
                .Where(g => gameModel.GenreNames.Contains(g.GenreName))
                .ToListAsync();

        private GameModel ConvertEntityToModel(VideoGame dbGame)
        {
            var genreNames = dbGame.Genres.Select(g => g.GenreName).ToList();

            return new GameModel
            {
                Id = dbGame.Id,
                GameName = dbGame.GameName,
                StudioDeveloper = dbGame.StudioDeveloper,
                GenreNames = genreNames,
            };
        }

    }
}
