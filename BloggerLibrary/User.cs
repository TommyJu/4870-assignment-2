using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;


namespace BlogLibrary;

public class User : IdentityUser
{

    public User() : base() { }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

}
