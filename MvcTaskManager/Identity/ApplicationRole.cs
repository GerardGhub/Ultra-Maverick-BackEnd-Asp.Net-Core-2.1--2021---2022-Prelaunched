using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace MvcTaskManager.Identity
{
    public class ApplicationRole : IdentityRole
    {
    public bool Isactive { get; set; }
    public string Isactivereference { get; set; }
    }
}


