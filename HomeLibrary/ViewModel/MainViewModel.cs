using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using HomeLibrary.Infrastructure;
using HomeLibrary.Model;
using HomeLibrary.Services;
using System.Collections.ObjectModel;
using System.Linq;

namespace HomeLibrary.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        IDataAccessService _repo;
        ObservableCollection<Book> _books;

        public ObservableCollection<Book> Books
        {
            get { return _books; }
            set
            {
                _books = value;
                RaisePropertyChanged("Books");
            }
        }

        Book _book;

        public Book Book
        {
            get { return _book; }
            set
            {
                _book = value;
                RaisePropertyChanged("Book");
            }
        }

        string _bookName;

        public string BookName
        {
            get { return _bookName; }
            set
            {
                _bookName = value;
                RaisePropertyChanged("BookName");
            }
        }

        #region Command Object Declarations
        public RelayCommand ReadAllCommand { get; set; }
        public RelayCommand<Book> SaveCommand { get; set; }

        public RelayCommand<Book> DeleteCommand { get; set; }

        public RelayCommand SearchCommand { get; set; }

        public RelayCommand<Book> SendBookCommand { get; set; }
        #endregion

        public MainViewModel(IDataAccessService repo)
        {
            _repo = repo;
            Books = new ObservableCollection<Book>();
            Book = new Book();
            ReadAllCommand = new RelayCommand(GetBooks);
            SaveCommand = new RelayCommand<Book>(SaveBook);
            DeleteCommand = new RelayCommand<Book>(DeleteBook);
            SearchCommand = new RelayCommand(SearchBook);
            SendBookCommand = new RelayCommand<Book>(SendBookInfo);
            ReceiveBookInfo();
            GetBooks();
            BookName = "Поиск по названию...";
        }

        void GetBooks()
        {
            Books.Clear();
            foreach (var item in _repo.GetBooks())
            {
                Books.Add(item);
            }
        }

        void SaveBook(Book b)
        {
            if (_repo.IsExistsBook(b))
            {
                Book.Id = _repo.UpdateBook(b);
            }
            else
            {
                Book.Id = _repo.CreateBook(b);
                if (Book.Id != 0)
                {
                    Books.Add(Book);
                    RaisePropertyChanged("Book");
                }
            }
        }

        void SearchBook()
        {
            Books.Clear();
            var Res = from e in _repo.GetBooks()
                      where e.Title.StartsWith(BookName)
                      select e;
            foreach (var item in Res)
            {
                Books.Add(item);
            }
        }
        void SendBookInfo(Book b)
        {
            if (b != null)
            {
                Messenger.Default.Send<MessageCommunicator>(new MessageCommunicator()
                {
                    Book = b
                });
            }
        }

        void ReceiveBookInfo()
        {
            if (Book != null)
            {
                Messenger.Default.Register<MessageCommunicator>(this, (emp) =>
                {
                    this.Book = emp.Book;
                });
            }
        }

        void DeleteBook(Book b)
        {
            _repo.RemoveBook(b);
            Books.Remove(Book);
            RaisePropertyChanged("Book");
        }
    }
}