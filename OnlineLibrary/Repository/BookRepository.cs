using OnlineLibrary.Data;
using OnlineLibrary.Models.DBObjects;
using OnlineLibrary.Models;

namespace OnlineLibrary.Repository
{
    public class BookRepository
    {
        private ApplicationDbContext dbContext;
        public BookRepository()
        {
            this.dbContext = new ApplicationDbContext();
        }
        public BookRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public List<BookModel> GetBooks()
        {
            List<BookModel> booksList = new List<BookModel>();
            foreach (Book book in dbContext.Books)
            {
                booksList.Add(MapDbObjectToModel(book));
            }
            return booksList;
        }
        public BookModel GetBookByID(Guid ID)
        {
            return MapDbObjectToModel(dbContext.Books.FirstOrDefault(x => x.Idbook == ID));
        }
        public List<BookModel> GetBookByPublicationYear(int year)
        {
            List<BookModel> booksList = new List<BookModel>();

            foreach (Book book in dbContext.Books.Where(x => x.PublicationYear == year))
            {
                booksList.Add(MapDbObjectToModel(book));
            }
            return booksList;
        }
        public List<BookModel> GetBookByLanguage(string language)
        {
            List<BookModel> booksList = new List<BookModel>();

            foreach (Book book in dbContext.Books.Where(x => x.PublicationYear.Equals(language)))
            {
                booksList.Add(MapDbObjectToModel(book));
            }
            return booksList;
        }
        public List<BookModel> GetBookAvailable()
        {
            List<BookModel> booksList = new List<BookModel>();

            foreach (Book book in dbContext.Books.Where(x => x.AvailableCopies > 0))
            {
                booksList.Add(MapDbObjectToModel(book));
            }
            return booksList;
        }
        public List<BookModel> GetBooksByGenre(Guid genreId)
        {
            List<BookModel> booksList = new List<BookModel>();

            foreach (Book book in dbContext.Books.Where(x => x.Idgenre == genreId))
            {
                booksList.Add(MapDbObjectToModel(book));
            }
            return booksList;
        }
        public List<BookModel> GetBooksByAuthor(Guid authorId)
        {
            List<BookModel> booksList = new List<BookModel>();

            foreach (Book book in dbContext.Books.Where(x => x.Idauthor == authorId))
            {
                booksList.Add(MapDbObjectToModel(book));
            }
            return booksList;
        }
        public void InsertBook(BookModel bookModel)
        {
            bookModel.Idbook = Guid.NewGuid();
            dbContext.Books.Add(MapModelToDbObject(bookModel));
            dbContext.SaveChanges();
        }

        public void UpdateBook(BookModel bookModel)
        {
            Book existingBook = dbContext.Books.FirstOrDefault(x => x.Idbook == bookModel.Idbook);
            if (existingBook != null)
            {
                existingBook.Idbook = bookModel.Idbook;
                existingBook.Title = bookModel.Title;
                existingBook.Idauthor = bookModel.Idauthor;
                existingBook.Isbn = bookModel.Isbn;
                existingBook.PublicationYear = bookModel.PublicationYear;
                existingBook.Idgenre = bookModel.Idgenre;
                existingBook.Language = bookModel.Language;
                existingBook.TotalCopies = bookModel.TotalCopies;
                existingBook.AvailableCopies = bookModel.AvailableCopies;
                existingBook.ImagePath=bookModel.ImagePath;
                dbContext.SaveChanges();
            }
        }
        public void DeleteBook(Guid id)
        {
            Book existingBook = dbContext.Books.FirstOrDefault(x => x.Idbook == id);
            if (existingBook != null)
            {
                dbContext.Books.Remove(existingBook);
                dbContext.SaveChanges();
            }
        }
        private BookModel MapDbObjectToModel(Book dbBook)
        {
            BookModel bookModel = new BookModel();
            if (dbBook != null)
            {
                bookModel.Idbook = dbBook.Idbook;
                bookModel.Title = dbBook.Title;
                bookModel.Idauthor = dbBook.Idauthor;
                bookModel.Isbn = dbBook.Isbn;
                bookModel.PublicationYear = dbBook.PublicationYear;
                bookModel.Idgenre = bookModel.Idgenre;
                bookModel.Language = dbBook.Language;
                bookModel.TotalCopies = dbBook.TotalCopies;
                bookModel.AvailableCopies = dbBook.AvailableCopies;
                bookModel.ImagePath = dbBook.ImagePath;
            }
            return bookModel;
        }
        private Book MapModelToDbObject(BookModel bookModel)
        {
            Book book = new Book();
            if (bookModel != null)
            {
                book.Idbook = bookModel.Idbook;
                book.Title = bookModel.Title;
                book.Idauthor = bookModel.Idauthor;
                book.Isbn = bookModel.Isbn;
                book.PublicationYear = bookModel.PublicationYear;
                book.Idgenre = bookModel.Idgenre;
                book.Language = bookModel.Language;
                book.TotalCopies = bookModel.TotalCopies;
                book.AvailableCopies = bookModel.AvailableCopies;
                book.ImagePath=bookModel.ImagePath;
            }
            return book;
        }

    }
}
