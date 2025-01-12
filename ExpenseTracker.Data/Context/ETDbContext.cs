using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseTracker.Data.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace ExpenseTracker.Data.Context
{
    public class ETDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ETDbContext(IHttpContextAccessor httpContextAccessor, DbContextOptions<ETDbContext> options) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable(nameof(Users));

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

        }


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var entries = ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

                string currentUser = GetCurrentUser() ?? "SYSTEM";

                foreach (var entry in entries)
                {
                    if (entry.Entity is EntityBase entity)
                    {
                        if (entry.State == EntityState.Added)
                        {
                            entity.CreatedDateTimeUTC = DateTime.UtcNow;
                            entity.CreatedBy = currentUser;
                        }
                        entity.UpdatedBy = currentUser;
                        entity.UpdatedDateTimeUTC = DateTime.UtcNow;
                    }
                }
                return await base.SaveChangesAsync(cancellationToken);
            } 
            catch (Exception e)
            {
                throw;
            }
            
        }

        private string GetCurrentUser()
        {
            var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");
            string? currentUser = null;

            if (!string.IsNullOrEmpty(accessToken))
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(accessToken) as JwtSecurityToken;

                if (jsonToken != null)
                {
                    currentUser = jsonToken.Claims.FirstOrDefault(claim => claim.Type == "upn")?.Value ?? "Undefined";
                }
            }
            return currentUser;
        }

    }
}
