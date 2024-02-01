using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace bookstore_Application.Models
{
    public class BooksViewModel 
    {
        public long BookId { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(50)]
        [DisplayName("Book Name")]
        public string BookName { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(50)]
        [DisplayName("Author")]
        public string Author { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Genre { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(13)]
        [MinLength(10)]
        public long ISBN { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(50)]
        public string Publication { get; set; }

        [Required]
        [MaxLength(4)]
        [RegularExpression("^(19|20)\\d{2}$")]
        public int ReleaseYear { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(5)]
        public decimal price { get; set; }

        public long authorID { get; set; }
        
        public long genreId { get; set; }
    }
}