using bookstore_Application.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO.Pipelines;
using System.Net;

namespace bookstore_Application.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private SqlConnection _connection;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        #region sortbyName
        public IActionResult sortByName()
        {
            List<BooksViewModel> list = new List<BooksViewModel>();
            connectToDB();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "GetBooksByName";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _connection;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                BooksViewModel book = new BooksViewModel();
                {

                    book.BookId = reader.GetInt64("BookId");
                    book.BookName = reader.GetString("BookName");
                    book.Author = reader.GetString("AuthorName");
                    book.ISBN = reader.GetInt64("ISBN");
                    book.ReleaseYear = reader.GetInt16("ReleaseYear");
                    book.price = reader.GetDecimal("price");
                    book.Publication = reader.GetString("Publication");
                    book.Genre = reader.GetString("genreName");
                    book.authorID = reader.GetInt64("AuthorId");
                    book.genreId = reader.GetInt64("GenreId");
                }
                list.Add(book);
            }

            return View("BookList",list);
        }
        #endregion
        //done
        #region sortbyISBN
        public IActionResult sortByISBN()
        {
            List<BooksViewModel> list = new List<BooksViewModel>();
            connectToDB();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "GetBooksByisbn";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = _connection;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                BooksViewModel book = new BooksViewModel()
                {

                    BookId = reader.GetInt64("BookId"),
                    BookName = reader.GetString("BookName"),
                    Author = reader.GetString("AuthorName"),
                    ISBN = reader.GetInt64("ISBN"),
                    ReleaseYear = reader.GetInt16("ReleaseYear"),
                    price = reader.GetDecimal("price"),
                    Publication = reader.GetString("Publication"),
                    Genre = reader.GetString("genreName"),
                    authorID = reader.GetInt64("AuthorId"),
                    genreId = reader.GetInt64("GenreId"),
                };
                list.Add(book);
            }

            return View("BookList", list);
        }
        #endregion
        //done
        #region search
        public IActionResult search([FromForm] IFormCollection form)
        {
            List<BooksViewModel> list = new List<BooksViewModel>();
            connectToDB();
            string BookName = form["search"];
            SqlCommand cmd = new SqlCommand();
            //cmd.CommandText = "SELECT  M.BookId,M.AuthorId,M.GenreId, BookName, ReleaseYear, price, ISBN, Publication, genreName,AuthorName" +
            //    " FROM MappingTable M JOIN BookDetails b ON M.BookId = b.BookId " +
            //    " JOIN Authors a ON M.AuthorId = a.AuthorId JOIN Genres g ON M.GenreId = g.GenreId " +
            //    " Where BookName = '" + BookName + "'";
            cmd.CommandText = "searchByName";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@BookName", BookName));

            cmd.Connection = _connection;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                BooksViewModel book = new BooksViewModel()
                {

                    BookId = reader.GetInt64("BookId"),
                    BookName = reader.GetString("BookName"),
                    Author = reader.GetString("AuthorName"),
                    ISBN = reader.GetInt64("ISBN"),
                    ReleaseYear = reader.GetInt16("ReleaseYear"),
                    price = reader.GetDecimal("price"),
                    Publication = reader.GetString("Publication"),
                    Genre = reader.GetString("genreName"),
                    authorID = reader.GetInt64("AuthorId"),
                    genreId = reader.GetInt64("GenreId"),
                };
                list.Add(book);
            }

            return View("booklist", list);
        }
        #endregion
        //done
        #region EditPage
        public IActionResult EditPage(int bookId)
        {
            connectToDB();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = $"SELECT  M.BookId,M.AuthorId,M.GenreId, BookName, ReleaseYear, price, ISBN, Publication, genreName,AuthorName\r\nFROM MappingTable M\r\nJOIN BookDetails b ON M.BookId = b.BookId\r\nJOIN Authors a ON M.AuthorId = a.AuthorId\r\nJOIN Genres g ON M.GenreId = g.GenreId\r\nWHERE M.BookId = {bookId};";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = _connection;
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
                BooksViewModel book = new BooksViewModel()
                {

                    BookId = reader.GetInt64("BookId"),
                    BookName = reader.GetString("BookName"),
                    Author = reader.GetString("AuthorName"),
                    ISBN = reader.GetInt64("ISBN"),
                    ReleaseYear = reader.GetInt16("ReleaseYear"),
                    price = reader.GetDecimal("price"),
                    Publication = reader.GetString("Publication"),
                    Genre = reader.GetString("genreName"),
                    authorID = reader.GetInt64("AuthorId"),
                    genreId = reader.GetInt64("GenreId")
                };
                
            

            return View(book);
        }
        #endregion
        //done
        #region delete
        public IActionResult Delete(long book, long author)
        {
            long a = book;
            long b= author;
            connectToDB();
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "DeleteBook";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("BookID", book));
            cmd.Parameters.Add(new SqlParameter("authorId", author));
            cmd.Connection = _connection;
            int rval = cmd.ExecuteNonQuery();
            return RedirectToAction("DeletePage");
        }
        #endregion
        //done
        #region DeletePage
        public IActionResult DeletePage() { 
            return View();
        }
        #endregion
        //done
        #region BOOKLIST
        public IActionResult BookList()
        {
            List<BooksViewModel> list = new List<BooksViewModel>();
            connectToDB();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "GetBooks";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _connection;
            SqlDataReader reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                BooksViewModel book = new BooksViewModel()
                {

                    BookId = reader.GetInt64("BookId"),
                    BookName = reader.GetString("BookName"),
                    Author = reader.GetString("AuthorName"),
                    ISBN = reader.GetInt64("ISBN"),
                    ReleaseYear = reader.GetInt16("ReleaseYear"),
                    price = reader.GetDecimal("price"),
                    Publication = reader.GetString("Publication"),
                    Genre = reader.GetString("genreName"),
                    authorID = reader.GetInt64("AuthorId"),
                    genreId = reader.GetInt64("GenreId"),
                };
                list.Add(book);
            }

            return View(list);
        }
        #endregion
        //done
        #region NewBook
        public IActionResult NewBook()
        {
            return View(new BooksViewModel());
        }
        #endregion
        //done
        #region AddnewBook
        public IActionResult submit([FromForm] IFormCollection form)
        {
            if (ModelState.IsValid)
            {
                BooksViewModel books = new BooksViewModel()
                {
                    BookName = form["BookName"],
                    Author = form["Author"],
                    Genre = form["Genre"],
                    ReleaseYear = Convert.ToInt32(form["ReleaseYear"]),
                    ISBN = Convert.ToInt64(form["ISBN"]),
                    price = Convert.ToDecimal(form["price"]),
                    Publication = form["Publication"],


                };
                saveToDB(books);
                //RedirectToAction("BookList");
                Console.Write("done");
                return RedirectToAction("BookList");
            }
            else
            {
                //RedirectToAction("Privacy");
                return RedirectToAction("Privacy");
            }
            
        }
        #endregion
        //done
        #region edit
        public IActionResult edit([FromForm] IFormCollection form)
        {
            if (ModelState.IsValid)
            {
                
                BooksViewModel b = new BooksViewModel()
                {
                    BookId = Convert.ToInt64(form["BookId"]),
                    BookName = form["BookName"],
                    Author = form["Author"],
                    Genre = form["Genre"],
                    ReleaseYear = Convert.ToInt16(form["ReleaseYear"]),
                    ISBN = Convert.ToInt64(form["ISBN"]),
                    price = Convert.ToDecimal(form["price"]),
                    Publication = form["Publication"],
                    authorID = Convert.ToInt64(form["authorId"]),
                    genreId = Convert.ToInt64(form["genreId"]),

                };
                int rval = updateToDB(b);

                return RedirectToAction("BookList");
            }
            else
            {
                //RedirectToAction("Privacy");
                return RedirectToAction("Privacy");
            }

        }
        #endregion
        //done
        #region update
        public int updateToDB(BooksViewModel book)
        {
            connectToDB();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "UpdateTable";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("BookId", book.BookId));
            cmd.Parameters.Add(new SqlParameter("AuthorId", book.authorID));
            cmd.Parameters.Add(new SqlParameter("GenerId", book.genreId));
            cmd.Parameters.Add(new SqlParameter("BookName", book.BookName));
            cmd.Parameters.Add(new SqlParameter("ReleaseYear", book.ReleaseYear));
            cmd.Parameters.Add(new SqlParameter("price", book.price));
            cmd.Parameters.Add(new SqlParameter("ISBN", book.ISBN));
            cmd.Parameters.Add(new SqlParameter("Publication", book.Publication));
            cmd.Parameters.Add(new SqlParameter("genreName", book.Genre));
            cmd.Parameters.Add(new SqlParameter("AuthorName", book.Author));
            cmd.Connection = _connection;
            int rval = cmd.ExecuteNonQuery();

            return rval;

        }
        #endregion
        //done
        #region SaveToDB
        public void saveToDB(BooksViewModel book)
        {
            connectToDB();
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "AddBook";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("BookName", book.BookName));
            cmd.Parameters.Add(new SqlParameter("ReleaseYear",book.ReleaseYear));
            cmd.Parameters.Add(new SqlParameter("price",book.price ));
            cmd.Parameters.Add(new SqlParameter("ISBN",book.ISBN) );
            cmd.Parameters.Add(new SqlParameter("Publication",book.Publication ));
            cmd.Parameters.Add(new SqlParameter("genreName", book.Genre));
            cmd.Parameters.Add(new SqlParameter("AuthorName", book.Author));
            cmd.Connection = _connection;
            cmd.ExecuteNonQuery();
            
        }
        #endregion
        //done
        #region connectToDB
        public void connectToDB()
        {
            _connection = new SqlConnection();
            string cString = "Server=G1-5M8T5Y2-L\\SQLEXPRESS;Database=Books;Trusted_Connection=True;";
            _connection.ConnectionString = cString;
            _connection.Open();
        }
        #endregion
        //done

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
