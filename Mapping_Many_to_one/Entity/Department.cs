using System.Text.Json.Serialization;

namespace Mapping_Many_to_one.Entity
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int CompanyId { get; set; } // Foreign key for Company
        [JsonIgnore]
        public Company Company { get; set; } // Navigation property for Company

        public ICollection<Employee> Employees { get; set; } // Navigation property for Employees
}
        
        
    }

