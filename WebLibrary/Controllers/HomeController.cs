using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Linq;
using WebLibrary.Data;
using WebLibrary.Mappers;
using WebLibrary.Models;

namespace WebLibrary.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly AppDbContext _context;



        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {          
            return View();
        }

        [HttpPost]
        public IActionResult Index(BookModel searchModel)
        {
            IQueryable<Book> query = _context.books;

            if (!string.IsNullOrEmpty(searchModel.Author))
            {
                query = query.Where(p => p.Author.Contains(searchModel.Author));
            }

            if (!string.IsNullOrEmpty(searchModel.Title))
            {
                query = query.Where(p => p.Title.Contains(searchModel.Title));
            }

            if (!string.IsNullOrEmpty(searchModel.Type) && !searchModel.Type.Equals("both"))
            {
                query = query.Where(p => p.Type == searchModel.Type);
            }

            var viewModel = new IndexViewModel
            {
                searchModel = searchModel,
                books = query.ToList()
            };
     
            return View(viewModel);
        }

        public IActionResult AddBook()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBook(BookModel newBook)
        {
            if (ModelState.IsValid)
            {
                _context.books.Add(BookMapper.asBook(newBook));
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpPost]
        public IActionResult ReceiveDataToEdit([FromBody] Book editedBook)
        {
            try
            {
                Book existingBook = _context.books.Find(editedBook.Id);

                if (existingBook != null)
                {
                    existingBook.Author = editedBook.Author;
                    existingBook.Title = editedBook.Title;
                    existingBook.Type = editedBook.Type;
                    _context.Update(existingBook);
                    _context.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                return Json(new { Success = false });
            }

            return Json(new { Success = true });
        }

        [HttpPost]
        public IActionResult ReceiveDataToDelete([FromBody] DeleteObject obj)
        {

            var book = _context.books.Find(obj.Id);

            if (book != null)
            {
                _context.books.Remove(book);
                _context.SaveChanges();
            }

            return Json(new { Success = true });
          }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public int sumTwo(int a, int b)
        {
            return a + b;
        }
    }
}
