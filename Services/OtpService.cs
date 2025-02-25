namespace ProjectHephaistos.Services
{
	using System;
	using System.Collections.Concurrent;
	using System.Threading.Tasks;
	using Microsoft.Extensions.Options;

	public class OtpService
	{
		private static ConcurrentDictionary<string, (string Otp, DateTime Expiry)> _otpStore = new ConcurrentDictionary<string, (string, DateTime)>();
		private readonly EmailService _emailService;
		private readonly EmailSettings _emailSettings;

		public OtpService(EmailService emailService, IOptions<EmailSettings> emailSettings)
		{
			_emailService = emailService;
			_emailSettings = emailSettings.Value;
		}

		public async Task<string> GenerateOtpAsync(string email)
		{
			var newOtp = new Random().Next(100000, 999999).ToString();
			var expiry = DateTime.UtcNow.AddMinutes(10); // OTP valid for 10 minutes
			_otpStore[email] = (newOtp, expiry);

			
			return newOtp;
		}

		public bool VerifyOtp(string email, string otp)
		{
			if (_otpStore.TryGetValue(email, out var storedOtp) && storedOtp.Otp == otp && storedOtp.Expiry > DateTime.UtcNow)
			{
				_otpStore.TryRemove(email, out _); // Invalidate OTP after use
				return true;
			}
			return false;
		}
	}
}