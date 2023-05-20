﻿using Microsoft.AspNetCore.Identity;

namespace CloneRepo.Entities;

public class User : IdentityUser<Guid>
{
    public string Name { get; set; }
}