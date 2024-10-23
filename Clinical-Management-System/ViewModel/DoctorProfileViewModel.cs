using Clinical_Management_System.Models.DB_Entities;

namespace Clinical_Management_System.ViewModel
{
	public class DoctorProfileViewModel
	{
		public string DoctorId { get; set; } =string.Empty;
		public string DoctorName { get; set; } =string.Empty;

		public Schedule schedule { get; set; } = default!;
		public List<string> AvailableSchedules { get; set; } = default!;

		public string ClinicName { get; set; } = string.Empty;
		public string ClinicAddress { get; set; } = string.Empty;
	}
}
