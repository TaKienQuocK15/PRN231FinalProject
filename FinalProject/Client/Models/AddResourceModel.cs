﻿using System.ComponentModel.DataAnnotations;

namespace Client.Models
{
	public class AddResourceModel
	{
		[Required(ErrorMessage = "Name is required")]
		public string Name { get; set; } = null!;
		[Required(ErrorMessage = "File is required")]
		public IFormFile File { get; set; } = null!;

	}
}
