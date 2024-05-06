using Humanizer.Bytes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Data;
using MoviesApi.Dto;
using MoviesApi.Model;
using MoviesApi.Services;

namespace MoviesApi.Controllers
{
    [Authorize(Roles = "Admin, Freelancer")]
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenresService _service;
        public GenresController(IGenresService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var genres = await _service.GetAll();
            return Ok(genres);
        }
        [HttpPost]
        public async Task<IActionResult> GreateAsync(GenreDto dto)
        {
            var genre = new Genre { name = dto.Name };
            await _service.Add(genre);
            return Ok(genre);

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(byte id , [FromBody] GenreDto dto)
        {
            var genre = await _service.GetById(id);

            if (genre == null)
                return NotFound($"Not Found");

            genre.name = dto.Name;
            _service.Update(genre);
            return Ok(genre);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(byte id)
        {
            var genre = await _service.GetById(id);

            if (genre == null)
                return NotFound($"Not Found");

            _service.Delete(genre);
            return Ok(genre);
        }
    }
}
