﻿using System;
using GabionPlatform.Auth;

namespace GabionPlatform.DTO.Accounts
{
    public class AuthUsernameDto : EditableGabionDto
    {
        public String Username { get; set; }
        public String Password { get; set; }

        public AuthUsernameDto()
        {
            Username = "";
            Password = "";
        }

        public AuthUsernameDto(AuthUsername auth)
            : base(auth)
        {
            Username = auth.Username;
            Password = "";
        }
    }
}