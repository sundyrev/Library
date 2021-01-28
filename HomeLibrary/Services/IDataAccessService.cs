using HomeLibrary.Model;
using System.Collections.ObjectModel;

namespace HomeLibrary.Services
{
    /// <summary>
    /// The Interface defining methods for save book and look for all books  
    /// </summary>
    public interface IDataAccessService
    {
        ObservableCollection<Book> GetBooks();
        int CreateBook(Book book);
        void RemoveBook(Book book);
        bool IsExistsBook(Book book);
        int UpdateBook(Book book);
    }
}
