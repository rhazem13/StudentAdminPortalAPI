using Microsoft.EntityFrameworkCore;

namespace StudentAdminPortalAPI.DataModels
{
    public class StudentAdminContext : DbContext
    {
        public StudentAdminContext(DbContextOptions<StudentAdminContext> options ): base(options)
        {
        }

        public DbSet<Student> Student { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Address> Address { get; set; }
    }
}
