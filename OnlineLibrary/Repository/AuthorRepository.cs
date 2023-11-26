using OnlineLibrary.Data;
using OnlineLibrary.Models.DBObjects;
using OnlineLibrary.Models;

namespace OnlineLibrary.Repository
{
    public class AuthorRepository
    {
        private ApplicationDbContext dbContext;
        public AuthorRepository()
        {
            this.dbContext = new ApplicationDbContext();
        }
        public AuthorRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public List<AuthorModel> GetAuthors()
        {
            List<AuthorModel> authorsList = new List<AuthorModel>();
            foreach (Author author in dbContext.Authors)
            {
                authorsList.Add(MapDbObjectToModel(author));
            }
            return authorsList;
        }
        public AuthorModel GetAuthorByID(Guid ID)
        {
            return MapDbObjectToModel(dbContext.Authors.FirstOrDefault(x => x.Idauthor == ID));
        }
        public List<AuthorModel> GetAuthorByNationality(string nationality)
        {
            List<AuthorModel> authorsList = new List<AuthorModel>();

            foreach (Author author in dbContext.Authors.Where(x => x.Nationality.Equals(nationality)))
            {
                authorsList.Add(MapDbObjectToModel(author));
            }
            return authorsList;
        }
        public void InsertAuthor(AuthorModel authorModel)
        {
            authorModel.Idauthor = Guid.NewGuid();
            dbContext.Authors.Add(MapModelToDbObject(authorModel));
            dbContext.SaveChanges();
        }

        public void UpdateAuthor(AuthorModel authorModel)
        {
            Author existingAuthor = dbContext.Authors.FirstOrDefault(x => x.Idauthor == authorModel.Idauthor);
            if (existingAuthor != null)
            {
                existingAuthor.Idauthor = authorModel.Idauthor;
                existingAuthor.Name = authorModel.Name;
                existingAuthor.BirthDate = authorModel.BirthDate;
                existingAuthor.Nationality = authorModel.Nationality;
                dbContext.SaveChanges();
            }
        }
        public void DeleteAuthor(Guid id)
        {
            Author existingAuthor = dbContext.Authors.FirstOrDefault(x => x.Idauthor == id);
            if (existingAuthor != null)
            {
                dbContext.Authors.Remove(existingAuthor);
                dbContext.SaveChanges();
            }
        }
        private AuthorModel MapDbObjectToModel(Author dbAuthor)
        {
            AuthorModel authorModel = new AuthorModel();
            if (dbAuthor != null)
            {
                authorModel.Idauthor = dbAuthor.Idauthor;
                authorModel.Name = dbAuthor.Name;
                authorModel.BirthDate = dbAuthor.BirthDate;
                authorModel.Nationality = dbAuthor.Nationality;
            }
            return authorModel;
        }
        private Author MapModelToDbObject(AuthorModel authorModel)
        {
            Author author = new Author();
            if (authorModel != null)
            {
                author.Idauthor = authorModel.Idauthor;
                author.Name = authorModel.Name;
                author.BirthDate = authorModel.BirthDate;
                author.Nationality = authorModel.Nationality;
            }
            return author;
        }

    }
}
