using System;
using GabionPlatform.Accounts;

namespace GabionPlatform.DTO.Accounts
{
    public class UserRoleDto : EditableGabionDto
    {
        public String Name { get; set; }

        public UserRoleDto()
        {
            Name = "";
        }

        public UserRoleDto(UserRole role)
            : base(role)
        {
            Name = role.Name;
        }
    }
}