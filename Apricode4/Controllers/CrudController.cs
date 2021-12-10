using Apricode4.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Apricode4.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CrudController : ControllerBase
    {
        private readonly IRepository repository;
        
        public CrudController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpPut("CreateOrUpdate")]
        public async Task CreateOrUpdateAsync(GameModel gameModel)
        {
            await repository.CreateOrUpdateAsync(gameModel);
        }

        [HttpGet("Read")]
        public async Task<List<GameModel>> ReadAsync(Guid gameId)
        {
           return await repository.ReadAsync(gameId);
        }

        [HttpDelete("Del")]
        public async Task DelAsync(Guid gameId)
        {
            await repository.DelAsync(gameId);
        }

        [HttpGet("SearchGenres")]
        public async Task<List<GameModel>> SearchGenres(Guid genreId)
        {
            return await repository.SearchByGenreAsync( genreId);
        }
    }
}
