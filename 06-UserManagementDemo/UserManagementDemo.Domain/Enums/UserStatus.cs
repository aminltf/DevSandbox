﻿using System.ComponentModel.DataAnnotations;

namespace UserManagementDemo.Domain.Enums;

public enum UserStatus
{
    Active = 1,
    Inactive = 2,
    Locked = 3,
    Deleted = 4
}
