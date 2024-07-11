using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psm32.Models;

public enum UserRole
{
    Admin,
    Tech,
    Clinician
}


public class User
{
    public User(string username, string hashedPassword, string salt, UserRole userRole)
    {
        ID = Guid.NewGuid();
        Username = username;
        HashedPassword = hashedPassword;
        Salt = salt;
        UserRole = userRole;
        Enabled = true;
        Created = DateTime.Now;
        Updated = DateTime.Now;
    }

    [Key]
    public Guid ID { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    public string HashedPassword { get; set; }

    [Required]
    public string Salt { get; set; }

    [Required]
    public UserRole UserRole { get; set; }

    [Required]
    public bool Enabled { get; set; }

    [Required]
    public DateTime Created { get; set; }

    [Required]
    public DateTime Updated { get; set; }
}
