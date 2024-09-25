using Microsoft.AspNetCore.Identity.UI.Services;

namespace Clinical_Management_System.Utitlity
{
	public class EmailSender : IEmailSender
	{
		public Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			return Task.CompletedTask;
		}
	}
}
