namespace StudentPortal.Models
{
    public class AddSubjectViewModel
    {
		public required string Code { get; set; }
		public required string Description { get; set; }
		public required int Units { get; set; }
		public required int Offering { get; set; }
		public required string Category { get; set; }
		public required string Status { get; set; }
		public required string Course { get; set; }
		public required string Curriculum { get; set; }
	}
}
