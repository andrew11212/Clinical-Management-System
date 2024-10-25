﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Clinical_Management_System.Models.DB_Entities
{
    public class ChronicDisease
    {
        [Required(ErrorMessage = "Please Provide the name of the Chronic Disease")]
        [MaxLength(100, ErrorMessage = "Chronic Disease name can't be longer than 100 characters")]
        public string Name { get; set; } = string.Empty;

		public string PatientId { get; set; } =string.Empty;
        [ValidateNever]
        public Patient Patient { get; set; } = default!;
    }

}
