using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.DataObjects;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DataContext : IdentityDbContext<CodexUser>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Term> Terms { get; set; }

        public DbSet<UserLanguageProfile> UserLanguageProfiles { get; set; }

         protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<UserLanguageProfile>(x => x.HasKey(ulp => new {ulp.Language, ulp.UserId}));

            //Set up the foreign keys for the UserLanguageProfile table
            builder.Entity<UserLanguageProfile>()
            .HasOne(u => u.User)
            .WithMany(p => p.UserLanguageProfiles)
            .HasForeignKey(k => k.UserId);
        }
    }
}