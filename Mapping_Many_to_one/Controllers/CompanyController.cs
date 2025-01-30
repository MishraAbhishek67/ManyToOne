using System.Net;
using Mapping_Many_to_one.Data;
using Mapping_Many_to_one.DTO;
using Mapping_Many_to_one.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mapping_Many_to_one.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ApplicationDbContext DbContext;

        public CompanyController(ApplicationDbContext Dbcontext)
        {
            this.DbContext = Dbcontext;
        }

        [HttpGet]
        [Route("api/companies")]
        public async Task<IActionResult> GetCompanies()
        {
            var companies = await DbContext.Companies

                .Include(c => c.Departments)
               .ThenInclude(d => d.Employees)
              .ToListAsync();
            return Ok(companies);
        }




        [HttpGet]
        [Route("api/companies/{id:int}")]
        public async Task<IActionResult> GetCompanyById(int id)
        {
            // Fetch the company by ID, including its departments and employees
            var company = await DbContext.Companies
                .Include(c => c.Departments)
                .ThenInclude(d => d.Employees)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (company == null)
            {
                return NotFound($"Company with ID {id} not found");
            }

            return Ok(company);
        }


        [HttpPost]
        [Route("api/employees")]
        public async Task<IActionResult> PostEmployee([FromBody] CompanyDTO dto)
        {
            if (dto == null)
            {
                return BadRequest("Dto was null");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Create the company object
            var companyData = new Company
            {
                Name = dto.CompanyName,
                Departments = new List<Department>
        {
               new Department
            {
                Name = dto.DepartmentName,
                Employees = new List<Employee>
                {
                    new Employee
                    {
                        Name = dto.EmployeeName,
                        Email = dto.Email
                    }
                }
            }
        }
            };

            // Add the company (including nested departments and employees)
            await DbContext.Companies.AddAsync(companyData);

            // Save changes to the database
            await DbContext.SaveChangesAsync();

            return Ok("Employee created successfully");
        }
         

        [HttpPut]
        [Route("UpdateEmployee/{id:int}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] CompanyDTO dto)
        {
            if (dto == null)
            {
                return BadRequest("DTO was null");
            }

            // Find the employee with related data
            var existingEmployee = await DbContext.Employees
                .Include(e => e.Department)
                .ThenInclude(d => d.Company)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (existingEmployee == null)
            {
                return NotFound("Employee does not exist");
            }

            // Update employee fields
            existingEmployee.Name = dto.EmployeeName;
            existingEmployee.Email = dto.Email;

            // Update or add the department
            if (existingEmployee.Department == null || existingEmployee.Department.Name != dto.DepartmentName)
            {
                var department = await DbContext.Departments
                    .Include(d => d.Company)
                    .FirstOrDefaultAsync(d => d.Name == dto.DepartmentName && d.Company.Name == dto.CompanyName);

                if (department == null)
                {
                    // Create a new department and company if they don't exist
                    department = new Department
                    {
                        Name = dto.DepartmentName,
                        Company = new Company
                        {
                            Name = dto.CompanyName
                        }
                    };
                    await DbContext.Departments.AddAsync(department);
                }

                // Assign the new or updated department to the employee
                existingEmployee.Department = department;
            }

            // Save changes to the database
            DbContext.Employees.Update(existingEmployee);
            await DbContext.SaveChangesAsync();

            return Ok("Employee updated successfully");

        }

        [HttpDelete]
        [Route("api/companies/{id:int}")]
        public async Task<IActionResult> DeleteCompanyById(int id)
        {
            // Fetch the company by ID, including its related departments and employees
            var company = await DbContext.Companies
                .Include(c => c.Departments)
                .ThenInclude(d => d.Employees)
                .FirstOrDefaultAsync(c => c.Id == id);

            // Check if the company exists
            if (company == null)
            {
                return NotFound($"Company with ID {id} not found.");
            }

            // Remove the company from the database
            DbContext.Companies.Remove(company);

            // Save changes to the database
            await DbContext.SaveChangesAsync();

            // Return success message
            return Ok($"Company with ID {id} and its related departments and employees have been deleted successfully.");
        }




        //[HttpDelete]
        //[Route("SoftDeleteEmployee/{id:int}")]
        //public async Task<IActionResult> SoftDeleteEmployee(int id)
        //{
        //    // Find the employee, including related data, but ensure they are not already soft deleted
        //    var existingEmployee = await DbContext.Employees
        //        .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);

        //    if (existingEmployee == null)
        //    {
        //        return NotFound("Employee does not exist or has already been deleted");
        //    }

        //    // Mark the employee as deleted (soft delete)
        //    existingEmployee.IsDeleted = true;
        //    await DbContext.SaveChangesAsync();

        //    return Ok("Employee deleted successfully (soft delete)");
        //}

        //[HttpPut]
        //[Route("UpdateEmployee/{id:int}")]
        //public async Task<IActionResult> UpdateEmployee(int id, [FromBody] CompanyDTO dto)
        //{
        //    if (dto == null)
        //    {
        //        return BadRequest("DTO was null");
        //    }

        //    // Find the employee and include the department and company
        //    var existingEmployee = await DbContext.Employees
        //        .Include(e => e.Department)
        //        .ThenInclude(d => d.Company)
        //        .FirstOrDefaultAsync(e => e.Id == id);

        //    if (existingEmployee == null)
        //    {
        //        return NotFound("Employee does not exist");
        //    }

        //    // Check if the company name matches the existing one
        //    if (existingEmployee.Department.Company.Name != dto.CompanyName)
        //    {
        //        return BadRequest("Company name cannot be changed");
        //    }

        //    // Update employee fields
        //    existingEmployee.Name = dto.EmployeeName;
        //    existingEmployee.Email = dto.Email;

        //    // Update department if the name is different
        //    if (existingEmployee.Department.Name != dto.DepartmentName)
        //    {
        //        // Check if the department already exists under the same company
        //        var existingDepartment = await DbContext.Departments
        //            .FirstOrDefaultAsync(d => d.Name == dto.DepartmentName && d.CompanyId == existingEmployee.Department.CompanyId);

        //        if (existingDepartment == null)
        //        {
        //            // Create a new department under the same company
        //            existingDepartment = new Department
        //            {
        //                Name = dto.DepartmentName,
        //                CompanyId = existingEmployee.Department.CompanyId
        //            };
        //            await DbContext.Departments.AddAsync(existingDepartment);
        //        }

        //        // Assign the new department to the employee
        //        existingEmployee.Department = existingDepartment;
        //    }

        //    // Save changes to the database
        //    DbContext.Employees.Update(existingEmployee);
        //    await DbContext.SaveChangesAsync();

        //    return Ok("Employee updated successfully");
        //}
    }
}



