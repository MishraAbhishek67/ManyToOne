using System.ComponentModel.DataAnnotations;

namespace Mapping_Many_to_one.DTO
{
    public class CompanyDTO
    {
        [Required(ErrorMessage = "Company name is required.")]
        [StringLength(100, ErrorMessage = "Company name cannot exceed 100 characters.")]
        public string CompanyName { get; set; }


        [Required(ErrorMessage = "Department name is required.")]
        [StringLength(50, ErrorMessage = "Department name cannot exceed 50 characters.")]
        public string DepartmentName { get; set; }


        [Required(ErrorMessage = "Employee name is required.")]
        [StringLength(50, ErrorMessage = "Employee name cannot exceed 50 characters.")]
        [RegularExpression(@"^[A-Z][a-zA-Z]*$", ErrorMessage = "Employee name must start with a capital letter.")]
        public string EmployeeName { get; set; }



        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@gmail\.com$", ErrorMessage = "Email must end with @gmail.com")]
        public string Email { get; set; }

    }
}
