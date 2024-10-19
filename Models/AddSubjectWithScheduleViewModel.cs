using System.ComponentModel.DataAnnotations;

namespace StudentPortal.Models
{
    public class AddSubjectWithScheduleViewModel
    {
        // Properties from AddSubjectViewModel
        public string Code { get; set; }
        public string Description { get; set; }
        public int Units { get; set; }
        public int Offering { get; set; }
        public string Category { get; set; }
        public string Course { get; set; }
        public string Curriculum { get; set; }

        // Properties from AddSubjectScheduleViewModel
        [StringLength(8)]
        public string EDPCode { get; set; }

        public string SubjectCode { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        [StringLength(3)]
        public string Days { get; set; }

        [StringLength(3)]
        public string Room { get; set; }

        public int MaxSize { get; set; }

        public int ClassSize { get; set; }

        [StringLength(3)]
        public string XM { get; set; }

        [StringLength(3)]
        public string Section { get; set; }

        public int Year { get; set; }
    }
}