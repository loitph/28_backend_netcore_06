using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

[Table("Products")]
public class Product
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // auto-increment
  public int Id { get; set; }

  [Required]
  [StringLength(255)]
  [Column(TypeName = "nvarchar(255)")]
  [AllowNull]
  public string Name { get; set; }

  [Column(TypeName = "nvarchar(255)")]
  [AllowNull]
  public string Alias { get; set; }

  [AllowNull]
  public decimal Price { get; set; }

  [Column(TypeName = "nvarchar(255)")]
  [AllowNull]
  public string Description { get; set; }

  [Column(TypeName = "nvarchar(255)")]
  [AllowNull]
  public string ImageUrl { get; set; }

  public bool Deleted { get; set; }

  public DateTime CreatedAt { get; set; }

  public DateTime UpdatedAt { get; set; }
}