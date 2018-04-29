using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GameReviewApp.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public Genre FavoriteGenre { get; set; }
        public string FavoriteGame { get; set; }
        public int ReviewCount { get; set; } = 0;


    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("GameInfo", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<GameReviewApp.Models.Game> Games { get; set; }

        public System.Data.Entity.DbSet<GameReviewApp.Models.Review> Reviews { get; set; }

        public System.Data.Entity.DbSet<GameReviewApp.Models.Publisher> Publishers { get; set; }

        public System.Data.Entity.DbSet<GameReviewApp.Models.News> News { get; set; }
    }
}