﻿namespace ProjectHephaistos.DTOs
{
    public class ChangePasswordRequest
    {
        public string OldPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;

    }
}
