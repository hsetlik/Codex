using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.DataObjects;
using Domain.DataObjects.DailyProfileMetrics;
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

        public DbSet<Translation> Translations { get; set; }

        public DbSet<Content> Contents { get; set; }

        public DbSet<ContentViewRecord> ContentViewRecords { get; set; }
        
        public DbSet<ContentHistory> ContentHistories { get; set; }

        public DbSet<ContentTag> ContentTags { get; set; }

        public DbSet<DailyKnownWords> DailyKnownWords { get; set; }
        public DbSet<DailyProfileHistory> DailyProfileHistories { get; set; }
         protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Set up the foreign keys for the UserLanguageProfile table
            builder.Entity<UserLanguageProfile>()
            .HasOne(u => u.User)
            .WithMany(p => p.UserLanguageProfiles)
            .HasForeignKey(k => k.UserId);

            //configure both relationships for UserTerm
            builder.Entity<UserTerm>()
            .HasOne(p => p.Term)
            .WithMany();

            builder.Entity<UserTerm>()
            .HasOne(u => u.UserLanguageProfile)
            .WithMany(u => u.UserTerms)
            .HasForeignKey(p => p.LanguageProfileId);

            builder.Entity<ContentHistory>()
            .HasOne(u => u.UserLanguageProfile)
            .WithMany(u => u.ContentHistories)
            .HasForeignKey(u => u.LanguageProfileId);

            builder.Entity<ContentTag>()
            .HasOne(u => u.Content)
            .WithMany(c => c.ContentTags)
            .HasForeignKey(u => u.ContentId);

            builder.Entity<DailyProfileHistory>()
            .HasOne(d => d.UserLanguageProfile)
            .WithOne(d => d.DailyProfileHistory);

            builder.Entity<DailyKnownWords>()
            .HasOne(d => d.DailyProfileHistory)
            .WithMany(h => h.DailyKnownWords)
            .HasForeignKey(d => d.DailyProfileHistoryId);
        }
    }
}