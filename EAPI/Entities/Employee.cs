using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EAPI.Entities
{
    [Table("Employees")]
    public class Employee
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "The field with name {0} is required")]
        [StringLength(150)]
        public string Name { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [MaxLength(15)]
        public string Gender { get; set; }

        [Required]
        [MaxLength(250)]
        public string PermanentAddress { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }
    }
}
