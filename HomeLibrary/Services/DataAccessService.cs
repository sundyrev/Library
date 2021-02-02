using HomeLibrary.Model;
using System.Collections.ObjectModel;
using System.Data.Entity;

namespace HomeLibrary.Services
{
    /// <summary>
    /// Class implementing IDataAccessService interface and implementing
    /// its methods by making call to the Entities using LibraryEntities object
    /// </summary>
    public class DataAccessService : IDataAccessService
    {
        LibraryEntities context;
        public DataAccessService()
        {
            context = new LibraryEntities();
        }

        public ObservableCollection<Book> GetBooks()
        {
            ObservableCollection<Book> books = new ObservableCollection<Book>();
            
            foreach (var item in context.Books)
            {
                context.Entry(item).Reference("Genre").Load();
                books.Add(item);

            }
            return books;
        }

        public ObservableCollection<Genre> GetGenres()
        {
            ObservableCollection<Genre> genres = new ObservableCollection<Genre>();

            foreach (var item in context.Genres)
            {
                genres.Add(item);
            }
            return genres;
        }

        public int CreateBook(Book book)
        {
            if (string.IsNullOrEmpty(book.Title) || string.IsNullOrEmpty(book.Author))
            {
                return 0;
            }
            context.Books.Add(book);
            context.SaveChanges();
            return book.Id;
        }

        public void RemoveBook(Book book)
        {
            context.Books.Remove(book);
            context.SaveChanges();
        }

        public bool IsExistsBook(Book book)
        {
            return (context.Books.Find(book.Id) != null) ? true : false;
        }

        public int UpdateBook(Book book)
        {
            context.Entry(book).State = EntityState.Modified;
            context.SaveChanges();
            return book.Id;
        }
    }
}
