using WebLibrary.Models;

namespace WebLibrary.Mappers
{
    public class BookMapper
    {
        public BookMapper() { }

        public static Book asBook(BookModel bookModel)
        {
            return new Book
            {
                Author = bookModel.Author,
                Title = bookModel.Title,
                Type = bookModel.Type
            };
        }

     }
}
