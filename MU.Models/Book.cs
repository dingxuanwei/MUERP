using System.ComponentModel.DataAnnotations;

namespace MU.Models
{
    public class Book
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string Author { get; set; }
        public decimal Price { get; set; }
    }
}
