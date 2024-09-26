using System.ComponentModel.DataAnnotations;

namespace StudentPortal.Models.Entities
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string  FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public char MiddleName {  get; set; }
        public string Course { get; set; }
        public string Remarks { get; set; }
    }
}
