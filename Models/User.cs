﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CV_v2.Models
{
    public class User : IdentityUser
    {
        [Required(ErrorMessage = "Förnamn måste anges")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Efternamn måste anges")]
        public string Lastname { get; set; }
        // Adress för användaren
        public string Address { get; set; } // Lägg till en enkel adress

        public bool IsProfilePrivate { get; set; }

        public int? CVID { get; set; }

        // Relation till CV
        [ForeignKey(nameof(CVID))]
        public virtual CV? CV { get; set; }

        // Samling av projekt som användaren har skapat
        public virtual ICollection<Project> CreatedProjects { get; set; } = new List<Project>();

        // Samling av UserInProject för att representera användarens associationer till projekt
        public virtual ICollection<UserInProject> UserInProjects { get; set; } = new List<UserInProject>();

        // Samling av projekt via UserInProject
        public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}

