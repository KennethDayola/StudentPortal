using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentPortal.Models.Entities
{
    public class EnrollmentHeader
    {
        [Key]
        public int Id { get; set; }

        public required DateTime EnrollDate { get; set; }

        [StringLength(15)]
        public required string SchoolYear { get; set; }

        [StringLength(15)]
        public required string Encoder { get; set; }

        public required int TotalUnits { get; set; }

        [StringLength(2)]
        public required string Status { get; set; }

        public Student Student { get; set; }

        public ICollection<EnrollmentDetail> EnrollmentDetails { get; set; }

    }
}
