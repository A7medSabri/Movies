using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using MoviesApi.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System;
using Humanizer;
using MoviesApi.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace MoviesApi.Controllers
{
    
    [Authorize(Roles = "User")]
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private new List<string> _allowExtention = new List<string> { ".jpg" ,".png" ,"", ".JPEG" , ".GIF" };
            private long _posterSize = 5000000;
        private readonly IMoviesService _service;
        private readonly IGenresService _Gservice;
        private readonly IMapper _mapper;

        public MoviesController(IMoviesService service, IGenresService gservice ,IMapper mapper )
        {
            _service = service;
            _Gservice = gservice;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var movise = await _service.GetAll();
            var data = _mapper.Map<IEnumerable<MovieDataDto>>(movise);
            return Ok(data);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var movie = await _service.GetById(id);
            if (movie == null)
                return NotFound();

            var data = _mapper.Map<MovieDataDto>(movie);
            return Ok(data);
        }

        [HttpGet("GetByGenerId")]
        public async Task<IActionResult> GetByGenerIdAsync(byte Gid)
        {
            var movies = await _service.GetAll(Gid);
            var data = _mapper.Map<IEnumerable<MovieDataDto>>(movies);
            return Ok(data);
        }


        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm]MovieDto dto)
        {
            if (dto.Poster == null)
                return BadRequest("Poster Is Rquire");
            if (!_allowExtention.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
            {
                return BadRequest("Only .png and .jpg Allow");
            } 
            if(dto.Poster.Length > _posterSize)
            {
                return BadRequest("Only Minmum 5Mb Allow");
            }
            var isValedGenre = await _Gservice.isValedGenre(dto.GenreId);
            if (!isValedGenre)
            {
                return BadRequest("Invalid Genre Id");
            }
            using var datastream = new MemoryStream();
            await dto.Poster.CopyToAsync(datastream);

            var movie = _mapper.Map<Movie>(dto);
            movie.Poster = datastream.ToArray(); 
            await _service.Add(movie);

            return Ok(movie);
        }



        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateAsync(int id, [FromBody] MovieDto dto)
        //{
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromForm] MovieDto dto)
        {
            var movie = await _service.GetById(id);
            if (movie == null)
                return NotFound($"Not Found");

            var isValedGenre = await _Gservice.isValedGenre(dto.GenreId);
            if (!isValedGenre)
            {
                return BadRequest("Invalid Genre Id");
            }
            if(dto.Poster != null)
            {
                if (!_allowExtention.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                {
                    return BadRequest("Only .png and .jpg Allow");
                }
                if (dto.Poster.Length > _posterSize)
                {
                    return BadRequest("Only Minmum 5Mb Allow");
                }

                using var datastream = new MemoryStream();

                await dto.Poster.CopyToAsync(datastream);

                movie.Poster = datastream.ToArray();

            }
            movie.Title = dto.Title;
            movie.GenreId = dto.GenreId;
            movie.Rate = dto.Rate;
            movie.Year = dto.Year;
            movie.Storeline = dto.Storeline;

            _service.Update(movie);
            return Ok(movie);

        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var movie = await _service.GetById(id);

            if (movie == null)
                return NotFound($"Not Found");

            _service.Delete(movie);
            return Ok(movie);
        }
    }
}
