using System.ComponentModel.DataAnnotations;

namespace StudentPortal.Models.Entities
{
    public class Subject
    {
        [Key]
        public required string Code { get; set; }
        public required string Description { get; set; }
        public required int Units { get; set; }
        public required int Offering { get; set; }
        public required string Category { get; set; }
        public required string Status { get; set; }
        public required string Course { get; set; }
        public required int Curriculum { get; set; }
    }
}
