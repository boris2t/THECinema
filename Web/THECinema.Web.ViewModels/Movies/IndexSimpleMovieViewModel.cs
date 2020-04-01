namespace THECinema.Web.ViewModels.Movies
{
    using System.Collections.Generic;

    public class IndexSimpleMovieViewModel
    {
        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }

        public IEnumerable<SimpleMovieViewModel> Movies { get; set; }
    }
}
