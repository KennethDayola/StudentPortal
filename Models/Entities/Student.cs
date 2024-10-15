using System.ComponentModel.DataAnnotations;

namespace StudentPortal.Models.Entities
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        public required string  FirstName { get; set; }
        public required string LastName { get; set; }
        public char? MiddleName {  get; set; }
        public required string Course { get; set; }
        public required string Remarks { get; set; }
        public string Status { get; set; } = "AC";
        public int Year { get; set; }
    }
}
