using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace QLKS.WebApp.Models
{
    // You can add profile data for the user by adding more properties to your Account class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class Account : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<Account> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        public virtual UserProfile Profile { get; set; }
        public virtual IList<Order> BuyOrders { get; set; }
        public virtual IList<Order> HandleOrder { get; set; }


        public virtual IList<Comment> Comments { get; set; }
        public virtual IList<ProductHistory> ProductHistories { get; set; }

        [NotMapped]
        public List<string> RoleTemps { get; set; }
    }
}