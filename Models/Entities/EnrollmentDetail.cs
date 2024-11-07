using System.ComponentModel.DataAnnotations;

namespace StudentPortal.Models.Entities
{
    public class EnrollmentDetail
    {
        public int StudId { get; set; }

        [StringLength(15)]
        public required string SubjectCode { get; set; }

        [StringLength(8)]
        public required string EDPCode { get; set; }

        [StringLength(2)]
        public required string Status { get; set; }
        public SubjectSchedule SubjectSchedule { get; set; }
        public EnrollmentHeader EnrollmentHeader { get; set; }
    }
}
