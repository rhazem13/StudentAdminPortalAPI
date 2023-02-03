using StudentAdminPortalAPI.DataModels;
using Microsoft.EntityFrameworkCore;

namespace StudentAdminPortalAPI.Repositories
{
    public class SqlStudentRepository : IStudentRepository
    {
        private readonly StudentAdminContext context;

        public SqlStudentRepository(StudentAdminContext context)
        {
            this.context = context;
        }

        public async Task<bool> Exists(Guid studentId)
        {
            return await context.Student.AnyAsync(x=>x.Id==studentId);
        }

        public async Task<List<Gender>> GetGendersAsync()
        {
            return await context.Genders.ToListAsync();
        }

        public async Task<Student> GetStudentAsync(Guid id)
        {
            return await context.Student.Include(nameof(Gender)).Include(nameof(Address)).FirstOrDefaultAsync(s=>s.Id==id);
        }

        public async Task<List<Student>> GetStudentsAsync()
        {
            return await context.Student.Include(nameof(Gender)).Include(nameof(Address)).ToListAsync();
        }

        public async Task<Student> UpdateStudent(Guid studentId, Student request)
        {
            var existingStudent = await GetStudentAsync(studentId);
            if (existingStudent != null)
            {
                existingStudent.FirstName= request.FirstName;
                existingStudent.LastName= request.LastName;
                existingStudent.Address.PhysicalAddress= request.Address.PhysicalAddress;
                existingStudent.Address.PostalAddress= request.Address.PostalAddress;
                existingStudent.DateOfBirth= request.DateOfBirth;
                existingStudent.Email=request.Email;
                existingStudent.Mobile=request.Mobile;
                existingStudent.GenderId= request.GenderId;

                await context.SaveChangesAsync();
                return existingStudent;
            }
            return null;
        }
    }
}
