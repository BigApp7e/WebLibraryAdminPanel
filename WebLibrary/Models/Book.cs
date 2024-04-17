using System.ComponentModel.DataAnnotations;

namespace WebLibrary.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Author {  get; set; }

        public string Type { get; set; }
    }
}
