using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bookworm.Models;

public class Category
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string? Name {get; set;}
    [Required]
    [DisplayName("Display Order")]
    public int DisplayOrder { get; set; }
}
