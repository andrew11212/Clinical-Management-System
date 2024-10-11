using Clinical_Management_System.Models.DB_Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clinical_Management_System.ViewModel
{
	public class PrescriptionVM

	{
		[Key]
		[Column("Id")]
		public int PrescriptionId { get; set; }
		[Display (Name ="Diagnosis")]
		public string DiagnosisName { get; set; } = string.Empty;
		[Required]
		public DateTime DateTime { get; set; }

		[Display(Name ="Category")]
		public int? AppointmentId { get; set; }
		[ValidateNever] 
		public IEnumerable<SelectListItem>? Appointments { get; set; }=Enumerable.Empty<SelectListItem>();
		
	}
}
