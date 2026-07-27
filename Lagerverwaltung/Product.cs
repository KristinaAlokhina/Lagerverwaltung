using System.ComponentModel.DataAnnotations;

namespace Lagerverwaltung
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string ArticleNumber { get; set; } = string.Empty;

        [Required]
        public int Stock { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}