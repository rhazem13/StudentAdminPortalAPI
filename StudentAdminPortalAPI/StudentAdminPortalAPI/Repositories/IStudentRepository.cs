using StudentAdminPortalAPI.DataModels;

namespace StudentAdminPortalAPI.Repositories
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetStudentsAsync();
        Task<Student> GetStudentAsync(Guid id);

        Task<List<Gender>> GetGendersAsync();

        Task<bool> Exists(Guid studentId);
        Task<Student> UpdateStudent(Guid studentId, Student request);

        Task<Student> DeleteStudentAsync(Guid studentId);

        Task<Student> AddStudentAsync(Student request);

        Task<bool> UpdateProfileImage(Guid studentId, string imageUrl);
    }
}
