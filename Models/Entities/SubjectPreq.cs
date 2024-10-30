using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentPortal.Models.Entities
{
    public class SubjectPreq
    {
        [ForeignKey(nameof(Subject))]
        [StringLength(15)]
        public required string SubjectCode { get; set; }

        [StringLength(15)]
        public required string PreCode { get; set; }

        [StringLength(2)]
        public required string Category { get; set; } // "CR" for co-requisite, "PR" for prerequisite

        // Navigation property
        public Subject Subject { get; set; }
    }
}
