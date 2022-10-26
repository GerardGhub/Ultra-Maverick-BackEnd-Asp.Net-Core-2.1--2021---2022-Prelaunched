using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace MvcTaskManager.Identity
{
    public class ApplicationRole : IdentityRole
    {
    public bool Isactive { get; set; }
    public string Isactivereference { get; set; }
    public string Addedby { get; set; }
    public string Modifiedby { get; set; }
    public DateTime? Modifieddate { get; set; }
    public DateTime Dateadded { get; set; }
    }
}


