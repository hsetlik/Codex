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

        public DbSet<UserTerm> UserTerms { get; set; }

         protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Set up the foreign keys for the UserLanguageProfile table
            builder.Entity<UserLanguageProfile>()
            .HasOne(u => u.User)
            .WithMany(p => p.UserLanguageProfiles)
            .HasForeignKey(k => k.UserId);

            //set up the compound key for UserTerms
            builder.Entity<UserTerm>(x => x.HasKey(aa => new {aa.LanguageProfileId, aa.TermId}));

            //configure both relationships for UserTerm
            builder.Entity<UserTerm>()
            .HasOne(p => p.Term)
            .WithMany(p => p.UserTerms)
            .HasForeignKey(p => p.TermId);

            builder.Entity<UserTerm>()
            .HasOne(u => u.UserLanguageProfile)
            .WithMany(u => u.UserTerms)
            .HasForeignKey(p => p.LanguageProfileId);

            //manually configure translations
            builder.Entity<UserTerm>()
            .HasMany(u => u.Translations)
            .WithOne();
            
        }
    }
}