using StudentAdminPortalAPI.DataModels;

namespace StudentAdminPortalAPI.Repositories
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetStudentsAsync();
    }
}
