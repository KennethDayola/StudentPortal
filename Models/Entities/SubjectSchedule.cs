using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentPortal.Models.Entities
{
    public class SubjectSchedule
    {
        [Key]
        [StringLength(8)]
        public required string EDPCode { get; set; }

        [StringLength(15)]
        public required string SubjectCode { get; set; } 

        public required DateTime StartTime { get; set; } 

        public required DateTime EndTime { get; set; } 

        [StringLength(3)]
        public required string Days { get; set; }

        [StringLength(3)]
        public required string Room { get; set; } 

        public required int MaxSize { get; set; } 

        public required int ClassSize { get; set; } 

        [StringLength(3)]
        public required string Status { get; set; } 

        [StringLength(3)]
        public required string XM { get; set; } 

        [StringLength(3)]
        public required string Section { get; set; } 

        public required int Year { get; set; } 

        [ForeignKey("SubjectCode")]
        public Subject Subject { get; set; }

        public ICollection<EnrollmentDetail> EnrollmentDetails { get; set; }
    }
}