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

        public DbSet<Translation> Translations { get; set; }

        public DbSet<Transcript> Transcripts { get; set; }

        public DbSet<TranscriptChunk> TranscriptChunks { get; set; }

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
            .WithOne();

            builder.Entity<UserTerm>()
            .HasOne(u => u.UserLanguageProfile)
            .WithMany(u => u.UserTerms)
            .HasForeignKey(p => p.LanguageProfileId);

        }
    }
}