namespace EAPI.DTO
{
    public class EmployeeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string PermanentAddress { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public int? RecordCount { get; set; }

    }
}
