using System.ComponentModel.DataAnnotations;

namespace StudentPortal.Models.Entities
{
    public class Subject
    {
        [Key]
        [StringLength(15)]
        public required string Code { get; set; }
        [StringLength(40)]
        public required string Description { get; set; }
        public required int Units { get; set; }
        public required int Offering { get; set; }
        [StringLength(3)]
        public required string Category { get; set; }
        [StringLength(2)]
        public required string Status { get; set; }
        [StringLength(5)]
        public required string Course { get; set; }
        [StringLength(10)]
        public required string Curriculum { get; set; }
        public ICollection<SubjectSchedule> Schedules { get; set; }
        public SubjectPreq Prerequisites { get; set; }
    }
}
