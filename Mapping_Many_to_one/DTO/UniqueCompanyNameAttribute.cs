
using System.ComponentModel.DataAnnotations;
using Mapping_Many_to_one.Data;

namespace Mapping_Many_to_one.DTO
{
    internal class UniqueCompanyNameAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dbContext = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext));
            var entity = validationContext.ObjectInstance as CompanyDTO;
            var companyName = value as string;

            if (dbContext.Companies.Any(e => e.Name == companyName))
            {
                return new ValidationResult("Company name must be unique.");
            }
            return ValidationResult.Success;
        }
    }
}