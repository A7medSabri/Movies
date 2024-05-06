using Microsoft.EntityFrameworkCore;
using MoviesApi.Model;

namespace MoviesApi.Services
{
    public class MoviesService : IMoviesService
    {
        private readonly ApplicationDbContext _context;

        public MoviesService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Movie> Add(Movie movie)
        {
            await _context.AddAsync(movie);
            await _context.SaveChangesAsync();

            return movie;
        }

        public Movie Delete(Movie movie)
        {
            _context.Remove(movie);
            _context.SaveChangesAsync();

            return movie;
        }

        public async Task<IEnumerable<Movie>> GetAll(byte genreId = 0)
        {
            var data = await _context.movies
                            .Include(m => m.Genre)
                            .Where(m => m.GenreId == genreId || genreId ==0)
                            .OrderByDescending(m => m.Rate)
                            .ToListAsync();
            return data;
        }

        public async Task<Movie> GetById(int id)
        {
            var movie = await _context.movies.Include(m => m.Genre).SingleOrDefaultAsync(c => c.Id == id);
            return movie;
        }

        public Movie Update(Movie movie)
        {
            _context.Update(movie);
            _context.SaveChangesAsync();

            return movie;
        }
    }
}
