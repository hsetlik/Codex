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
        private static string Host = "codex-flex-db.postgres.database.azure.com";

        // keep credentials in ignored file
        private static string User = "hsetlik";
        private static string DBname = "postgres";
        private static string Password = "Plinkun21";
        private static string Port = "5432";
        private string connString = $"Host={Host};Database={DBname};User Id={User};Password={Password}";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            Console.WriteLine($"User: {User} Password: {Password}");
            optionsBuilder.UseNpgsql(connString);
            Console.WriteLine("Data context configured");
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

        public DbSet<Collection> Collections { get; set; }

        public DbSet<CollectionContent> CollectionContents { get; set; }
        public DbSet<SavedCollection> SavedCollections { get; set; }
        public DbSet<ContentDifficulty> ContentDifficulties { get; set; }

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

            builder.Entity<Content>()
            .HasOne(c => c.UserLanguageProfile)
            .WithMany(p => p.CreatedContents)
            .HasForeignKey(c => c.LanguageProfileId);

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

            builder.Entity<CollectionContent>()
            .HasKey(cc => new { cc.CollectionId, cc.ContentId });

            builder.Entity<SavedCollection>()
            .HasKey(sc => new { sc.CollectionId, sc.LanguageProfileId });

            builder.Entity<ContentDifficulty>()
            .HasKey(k => new { k.LanguageProfileId, k.ContentId });
        }
    }
}