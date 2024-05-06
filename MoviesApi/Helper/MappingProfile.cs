using AutoMapper;
using MoviesApi.Model;

namespace MoviesApi.Helper
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            CreateMap<Movie,MovieDataDto>();
            CreateMap<MovieDto,Movie>()
                .ForMember(s=>s.Poster , op => op.Ignore());
        }
    }
}
