namespace ProjectHephaistos.Services
{
    using System;
    using System.Collections.Concurrent;

    public class OtpService
    {
        private static readonly ConcurrentDictionary<string, (string Otp, DateTime Expiry)> _otpStore = new();

        public OtpService()
        {
            // Constructor logic if needed
        }

        public Task<string> GenerateOtpAsync(string email)
        {
            var newOtp = new Random().Next(100000, 999999).ToString();
            var expiry = DateTime.UtcNow.AddMinutes(10); // OTP valid for 10 minutes
            _otpStore[email] = (newOtp, expiry);

            return Task.FromResult(newOtp); // Return Task for async compatibility
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