namespace StudentAdminPortalAPI.DomainModels
{
    public class AddStudentRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string DateOfBirth { get; set; } = string.Empty;
        public required string Email { get; set; }
        public long Mobile { get; set; }
        public Guid GenderId { get; set; }
        public string PhysicalAddress { get; set; } = string.Empty;
        public string PostalAddress { get; set; } = string.Empty;
    }
}
