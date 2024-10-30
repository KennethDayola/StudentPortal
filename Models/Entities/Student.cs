using System.ComponentModel.DataAnnotations;

namespace StudentPortal.Models.Entities
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        [StringLength(15)]
        public required string FirstName { get; set; }
        [StringLength(15)]
        public required string LastName { get; set; }
        [StringLength(15)]
        public string? MiddleName { get; set; }
        [StringLength(10)]
        public required string Course { get; set; }
        [StringLength(15)]
        public required string Remarks { get; set; }
        [StringLength(2)]
        public string Status { get; set; } = "AC";
        public int Year { get; set; }
    }
}
