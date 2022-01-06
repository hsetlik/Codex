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

        public DbSet<Collection> Collections { get; set; }

        public DbSet<CollectionContent> CollectionContents { get; set; }

        public DbSet<SavedCollection> SavedCollections { get; set; }

        public DbSet<ProfileFollowing> ProfileFollowings { get; set; }

        public DbSet<ProfileFollower> ProfileFollowers { get; set; }

         protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // UserLanguageProfile
            builder.Entity<UserLanguageProfile>()
            .HasOne(u => u.User)
            .WithMany(p => p.UserLanguageProfiles)
            .HasForeignKey(k => k.UserId);

            builder.Entity<UserLanguageProfile>()
            .HasOne(u => u.DailyProfileHistory)
            .WithOne(c => c.UserLanguageProfile)
            .HasForeignKey<DailyProfileHistory>(d => d.LanguageProfileId);

            //ContentHistory
            builder.Entity<ContentHistory>()
            .HasOne(c => c.UserLanguageProfile)
            .WithMany(c => c.ContentHistories)
            .HasForeignKey(k => k.LanguageProfileId);

            //UserTerm
            builder.Entity<UserTerm>()
            .HasOne(u => u.UserLanguageProfile)
            .WithMany(u => u.UserTerms)
            .HasForeignKey(p => p.LanguageProfileId);

            //contentTag
            builder.Entity<ContentTag>()
            .HasOne(u => u.Content)
            .WithMany(c => c.ContentTags)
            .HasForeignKey(u => u.ContentId);

            //ContentViewRecord
            builder.Entity<ContentViewRecord>()
            .HasOne(d => d.ContentHistory)
            .WithMany(h => h.ContentViewRecords)
            .HasForeignKey(h => h.ContentHistoryId);

            //Phrase
            builder.Entity<Phrase>()
            .HasOne(p => p.UserLanguageProfile)
            .WithMany(p => p.Phrases)
            .HasForeignKey(i => i.LanguageProfileId);

            builder.Entity<PhraseTranslation>()
            .HasOne(t => t.Phrase)
            .WithMany(p => p.Translations)
            .HasForeignKey(i => i.PhraseId);

            // DailyProfileRecord
            builder.Entity<DailyProfileRecord>()
            .HasOne(r => r.DailyProfileHistory)
            .WithMany(h => h.DailyProfileRecords)
            .HasForeignKey(k => k.DailyProfileHistoryId);

            builder.Entity<DailyProfileRecord>()
            .HasOne(r => r.UserLanguageProfile)
            .WithMany();

            //Saved
            builder.Entity<SavedContent>()
            .HasOne(s => s.UserLanguageProfile)
            .WithMany(p => p.SavedContents)
            .HasForeignKey(s => s.LanguageProfileId);
            
            builder.Entity<SavedCollection>()
            .HasKey(sc => new {sc.CollectionId, sc.LanguageProfileId});

            //CollectionContent
            builder.Entity<CollectionContent>()
            .HasKey(cc => new {cc.CollectionId, cc.ContentId});           

            //follower/following
            builder.Entity<ProfileFollower>()
            .HasOne(f => f.UserLanguageProfile)
            .WithMany(p => p.Followers)
            .HasForeignKey(k => k.LanguageProfileId);

            builder.Entity<ProfileFollowing>()
            .HasOne(f => f.UserLanguageProfile)
            .WithMany(p => p.Followings)
            .HasForeignKey(f => f.LanguageProfileId);
        }
    }
}