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
        public DbSet<UserLanguageProfile> UserLanguageProfiles { get; set; }

        public DbSet<UserTerm> UserTerms { get; set; }

        public DbSet<Phrase> Phrases { get; set; }

        public DbSet<PhraseTranslation> PhraseTranslations { get; set; }

        public DbSet<Translation> Translations { get; set; }

        public DbSet<Content> Contents { get; set; }

        public DbSet<ContentViewRecord> ContentViewRecords { get; set; }
        
        public DbSet<ContentHistory> ContentHistories { get; set; }

        public DbSet<ContentTag> ContentTags { get; set; }

        public DbSet<DailyProfileRecord> DailyProfileRecords { get; set; }

        public DbSet<DailyProfileHistory> DailyProfileHistories { get; set; }

        public DbSet<SavedContent> SavedContents { get; set; }

        public DbSet<ContentCollection> ContentCollections { get; set; }

        public DbSet<ContentCollectionEntry> ContentCollectionEntries { get; set; }

        public DbSet<SavedContentCollection> SavedContentCollections { get; set; }

         protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Set up the foreign keys for the UserLanguageProfile table
            builder.Entity<UserLanguageProfile>()
            .HasOne(u => u.User)
            .WithMany(p => p.UserLanguageProfiles)
            .HasForeignKey(k => k.UserId);

            builder.Entity<UserLanguageProfile>()
            .HasOne(u => u.DailyProfileHistory)
            .WithOne(c => c.UserLanguageProfile)
            .HasForeignKey<DailyProfileHistory>(d => d.LanguageProfileId);

            builder.Entity<ContentHistory>()
            .HasOne(c => c.UserLanguageProfile)
            .WithMany(c => c.ContentHistories)
            .HasForeignKey(k => k.LanguageProfileId);

            builder.Entity<UserTerm>()
            .HasOne(u => u.UserLanguageProfile)
            .WithMany(u => u.UserTerms)
            .HasForeignKey(p => p.LanguageProfileId);

            builder.Entity<ContentTag>()
            .HasOne(u => u.Content)
            .WithMany(c => c.ContentTags)
            .HasForeignKey(u => u.ContentId);

            builder.Entity<ContentViewRecord>()
            .HasOne(d => d.ContentHistory)
            .WithMany(h => h.ContentViewRecords)
            .HasForeignKey(h => h.ContentHistoryId);

            builder.Entity<Phrase>()
            .HasOne(p => p.UserLanguageProfile)
            .WithMany(p => p.Phrases)
            .HasForeignKey(i => i.LanguageProfileId);

            builder.Entity<PhraseTranslation>()
            .HasOne(t => t.Phrase)
            .WithMany(p => p.Translations)
            .HasForeignKey(i => i.PhraseId);

            builder.Entity<DailyProfileRecord>()
            .HasOne(r => r.DailyProfileHistory)
            .WithMany(h => h.DailyProfileRecords)
            .HasForeignKey(k => k.DailyProfileHistoryId);

            builder.Entity<DailyProfileRecord>()
            .HasOne(r => r.UserLanguageProfile)
            .WithMany();

            builder.Entity<SavedContent>()
            .HasOne(s => s.UserLanguageProfile)
            .WithMany(p => p.SavedContents)
            .HasForeignKey(s => s.LanguageProfileId);

            builder.Entity<ContentCollection>()
            .HasOne(c => c.UserLanguageProfile)
            .WithMany(p => p.CreatedCollections)
            .HasForeignKey(c => c.LanguageProfileId);

            builder.Entity<ContentCollectionEntry>()
            .HasOne(e => e.UserLanguageProfile)
            .WithMany()
            .HasForeignKey(e => e.LanguageProfileId);

            builder.Entity<ContentCollectionEntry>()
            .HasOne(e => e.Content)
            .WithMany()
            .HasForeignKey(e => e.ContentId);

            builder.Entity<SavedContentCollection>()
            .HasOne(c => c.ContentCollection)
            .WithMany()
            .HasForeignKey(c => c.ContentCollectionId);

            builder.Entity<SavedContentCollection>()
            .HasOne(s => s.UserLanguageProfile)
            .WithMany(p => p.SavedCollections)
            .HasForeignKey(s => s.LanguageProfileId);

        }
    }
}