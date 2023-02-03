using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentAdminPortalAPI.DomainModels;
using StudentAdminPortalAPI.Repositories;

namespace StudentAdminPortalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentRepository studentRepository;
        private readonly IMapper mapper;
        private readonly IImageRepository imageRepository;

        public StudentsController(IStudentRepository studentRepository, IMapper mapper, IImageRepository imageRepository)
        {
            this.studentRepository = studentRepository;
            this.mapper = mapper;
            this.imageRepository = imageRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudentsAsync()
        {
            var students = await studentRepository.GetStudentsAsync();
            return Ok(mapper.Map<List<Student>>(students));
        }

        [HttpGet("{studentId:guid}")]
        [ActionName("GetStudentAsync")]
        public async Task<IActionResult> GetStudentAsync([FromRoute] Guid studentId)
        {
            // fetch student details
            var student = await studentRepository.GetStudentAsync(studentId);
            // return student
            if (student == null)
            {
                return NotFound("Student with id not found");
            }
            return Ok(mapper.Map<Student>(student));
        }

        [HttpPut("{studentId:guid}")]
        public async Task<IActionResult> UpdateStudentAsync([FromRoute] Guid studentId, [FromBody] UpdateStudentRequest request)
        {
            if (await studentRepository.Exists(studentId)) {

                //Update Details
                var updatedStudent = await studentRepository.UpdateStudent(studentId, mapper.Map<DataModels.Student>(request));

                if(updatedStudent != null)
                {
                    return Ok(mapper.Map<Student>(updatedStudent));
                }
                return NotFound();
            }
            return NotFound();
        }

        [HttpDelete("{studentId:guid}")]
        public async Task<IActionResult> DeleteStudentAsync([FromRoute] Guid studentId)
        {
            if(await studentRepository.Exists(studentId))
            {
                var student =await studentRepository.DeleteStudentAsync(studentId);
                return Ok(mapper.Map<Student>(student));
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddStudentAsync([FromBody] AddStudentRequest request)
        {

            var student = await studentRepository.AddStudentAsync(mapper.Map<DataModels.Student>(request));
            return CreatedAtAction(nameof(GetStudentAsync), new { studentId = student.Id }, mapper.Map<Student>(student));
        }

        [HttpPost]
        [Route("{studentId:guid}/upload-image")]
        public async Task<IActionResult> UploadImage([FromRoute] Guid studentId, IFormFile profileImage)
        {
            // Check if student exists
            if(await studentRepository.Exists(studentId))
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(profileImage.FileName);
                // Upload the Image to local storage
                var fileImagePath= await imageRepository.Upload(profileImage, fileName);

                // update the profile image path in the database
                if(await studentRepository.UpdateProfileImage(studentId, fileName))
                {
                    return Ok(fileName);
                }

                return StatusCode(StatusCodes.Status500InternalServerError, "Error uploading image");
            }

            return NotFound();
        }
    }
}
