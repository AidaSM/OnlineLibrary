using OnlineLibrary.Data;
using OnlineLibrary.Models.DBObjects;
using OnlineLibrary.Models;

namespace OnlineLibrary.Repository
{
    public class GenreRepository
    {
        private ApplicationDbContext dbContext;
        public GenreRepository()
        {
            this.dbContext = new ApplicationDbContext();
        }
        public GenreRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public List<GenreModel> GetGenres()
        {
            List<GenreModel> genresList = new List<GenreModel>();
            foreach (Genre genre in dbContext.Genres)
            {
                genresList.Add(MapDbObjectToModel(genre));
            }
            return genresList;
        }
        public GenreModel GetGenreByID(Guid ID)
        {
            return MapDbObjectToModel(dbContext.Genres.FirstOrDefault(x => x.Idgenre == ID));
        }
        public List<GenreModel> GetGenreByName(string name)
        {
            List<GenreModel> genresList = new List<GenreModel>();

            foreach (Genre genre in dbContext.Genres.Where(x => x.Name.Equals(name)))
            {
                genresList.Add(MapDbObjectToModel(genre));
            }
            return genresList;
        }
        public void InsertGenre(GenreModel genreModel)
        {
            genreModel.Idgenre = Guid.NewGuid();
            dbContext.Genres.Add(MapModelToDbObject(genreModel));
            dbContext.SaveChanges();
        }

        public void UpdateGenre(GenreModel genreModel)
        {
            Genre existingGenre = dbContext.Genres.FirstOrDefault(x => x.Idgenre == genreModel.Idgenre);
            if (existingGenre != null)
            {
                existingGenre.Idgenre = genreModel.Idgenre;
                existingGenre.Name = genreModel.Name;
                dbContext.SaveChanges();
            }
        }
        public void DeleteGenre(Guid id)
        {
            Genre existingGenre = dbContext.Genres.FirstOrDefault(x => x.Idgenre == id);
            if (existingGenre != null)
            {
                dbContext.Genres.Remove(existingGenre);
                dbContext.SaveChanges();
            }
        }
        private GenreModel MapDbObjectToModel(Genre dbGenre)
        {
            GenreModel genreModel = new GenreModel();
            if (dbGenre != null)
            {
                genreModel.Idgenre = dbGenre.Idgenre;
                genreModel.Name = dbGenre.Name;
            }
            return genreModel;
        }
        private Genre MapModelToDbObject(GenreModel genreModel)
        {
            Genre genre = new Genre();
            if (genreModel != null)
            {
                genre.Idgenre = genreModel.Idgenre;
                genre.Name = genreModel.Name;
            }
            return genre;
        }
    }
}
