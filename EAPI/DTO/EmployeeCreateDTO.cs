using System.ComponentModel.DataAnnotations;

namespace EAPI.DTO
{
    public class EmployeeCreateDTO
    {
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
