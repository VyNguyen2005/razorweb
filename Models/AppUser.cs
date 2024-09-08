using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace razor09_razorweb.models{
    //razor09_razorweb.models
    public class AppUser : IdentityUser{
        // [Column(TypeName = "nvarchar")]
        // [StringLength(400)]
        // public string HomeAddress { get; set; }
    }
}