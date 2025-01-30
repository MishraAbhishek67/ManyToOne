namespace Mapping_Many_to_one.Entity
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Department> Departments { get; set; } // Navigation property for Departments
    }
}
