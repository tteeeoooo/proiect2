using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Store.Models;
using Microsoft.EntityFrameworkCore;

namespace Store.Models{
    public class AdminProfile
    {
        public string Id { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Department { get; set; }
        public DateTime CreatedAt { get; set; }

        public IdentityUser User { get; set; }
    }
}
