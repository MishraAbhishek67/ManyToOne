using System.Text.Json.Serialization;

namespace Mapping_Many_to_one.Entity
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public int DepartmentId { get; set; } // Foreign key for Department
        [JsonIgnore]
        public Department Department { get; set; } // Navigation property for Department
      //  public bool IsDeleted { get; set; } // Soft delete flag



    }
}

