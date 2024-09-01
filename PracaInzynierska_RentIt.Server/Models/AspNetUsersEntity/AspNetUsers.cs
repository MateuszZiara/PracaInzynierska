﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using PracaInzynierska_RentIt.Server.Models.Application;

namespace PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity;

public class AspNetUsers : IdentityUser
{
    public AspNetUsers() : base()
    {
        Avatar = false;
        ModifiedTime = null;
        ModifiedBy = null;
        CreateTime = DateTime.Now;
        Admin = false;
    }
    public virtual string? FirstName { get; set; }
    public virtual string? LastName { get; set; }
    public virtual bool Admin { get; set; }
    public virtual string? Provider { get; set; }
    public virtual string? Name { get; set; } 
    public virtual bool? Avatar {get; set; }
    public virtual DateTime? ModifiedTime { get; set; }
    public virtual String? ModifiedBy { get; set; }
    [Required(ErrorMessage = "Pole CreateTime nie może być puste")]
    public virtual DateTime CreateTime { get; set; }
    [Required(ErrorMessage = "Pole CreateBy nie może być puste")]
    public virtual String CreateBy { get; set; }
}