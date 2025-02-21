namespace ProjectHephaistos.Services
{
	using System;
	using System.Collections.Concurrent;

	public class OtpService
	{
		private static ConcurrentDictionary<string, string> _otpStore = new ConcurrentDictionary<string, string>();

		public string GenerateOtp(string email)
		{
			var newOtp = new Random().Next(100000, 999999).ToString();
			_otpStore[email] = newOtp;
			return newOtp;
		}

		public bool VerifyOtp(string email, string otp)
		{
			return _otpStore.TryGetValue(email, out var storedOtp) && storedOtp == otp;
		}
	}
}