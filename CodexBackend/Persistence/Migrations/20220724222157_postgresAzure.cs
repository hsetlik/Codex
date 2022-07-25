using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Persistence.Migrations
{
    public partial class postgresAzure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: true),
                    NativeLanguage = table.Column<string>(type: "text", nullable: true),
                    LastStudiedLanguage = table.Column<string>(type: "text", nullable: true),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Collections",
                columns: table => new
                {
                    CollectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatorLanguageProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatorUserName = table.Column<string>(type: "text", nullable: true),
                    IsPrivate = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Language = table.Column<string>(type: "text", nullable: true),
                    CollectionName = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collections", x => x.CollectionId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLanguageProfiles",
                columns: table => new
                {
                    LanguageProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    Language = table.Column<string>(type: "text", nullable: true),
                    UserLanguage = table.Column<string>(type: "text", nullable: true),
                    KnownWords = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLanguageProfiles", x => x.LanguageProfileId);
                    table.ForeignKey(
                        name: "FK_UserLanguageProfiles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ContentHistories",
                columns: table => new
                {
                    ContentHistoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    LanguageProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    ContentUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentHistories", x => x.ContentHistoryId);
                    table.ForeignKey(
                        name: "FK_ContentHistories_UserLanguageProfiles_LanguageProfileId",
                        column: x => x.LanguageProfileId,
                        principalTable: "UserLanguageProfiles",
                        principalColumn: "LanguageProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contents",
                columns: table => new
                {
                    ContentId = table.Column<Guid>(type: "uuid", nullable: false),
                    LanguageProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatorUsername = table.Column<string>(type: "text", nullable: true),
                    ContentUrl = table.Column<string>(type: "text", nullable: true),
                    VideoId = table.Column<string>(type: "text", nullable: true),
                    AudioUrl = table.Column<string>(type: "text", nullable: true),
                    ContentType = table.Column<string>(type: "text", nullable: true),
                    ContentName = table.Column<string>(type: "text", nullable: true),
                    Language = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NumSections = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contents", x => x.ContentId);
                    table.ForeignKey(
                        name: "FK_Contents_UserLanguageProfiles_LanguageProfileId",
                        column: x => x.LanguageProfileId,
                        principalTable: "UserLanguageProfiles",
                        principalColumn: "LanguageProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DailyProfileHistories",
                columns: table => new
                {
                    DailyProfileHistoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    LanguageProfileId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyProfileHistories", x => x.DailyProfileHistoryId);
                    table.ForeignKey(
                        name: "FK_DailyProfileHistories_UserLanguageProfiles_LanguageProfileId",
                        column: x => x.LanguageProfileId,
                        principalTable: "UserLanguageProfiles",
                        principalColumn: "LanguageProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Phrases",
                columns: table => new
                {
                    PhraseId = table.Column<Guid>(type: "uuid", nullable: false),
                    LanguageProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true),
                    TimesSeen = table.Column<int>(type: "integer", nullable: false),
                    EaseFactor = table.Column<float>(type: "real", nullable: false),
                    Rating = table.Column<int>(type: "integer", nullable: false),
                    DateTimeDue = table.Column<string>(type: "text", nullable: true),
                    SrsIntervalDays = table.Column<float>(type: "real", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phrases", x => x.PhraseId);
                    table.ForeignKey(
                        name: "FK_Phrases_UserLanguageProfiles_LanguageProfileId",
                        column: x => x.LanguageProfileId,
                        principalTable: "UserLanguageProfiles",
                        principalColumn: "LanguageProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SavedCollections",
                columns: table => new
                {
                    LanguageProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    CollectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserLanguageProfileLanguageProfileId = table.Column<Guid>(type: "uuid", nullable: true),
                    SavedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsOwner = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedCollections", x => new { x.CollectionId, x.LanguageProfileId });
                    table.ForeignKey(
                        name: "FK_SavedCollections_Collections_CollectionId",
                        column: x => x.CollectionId,
                        principalTable: "Collections",
                        principalColumn: "CollectionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SavedCollections_UserLanguageProfiles_UserLanguageProfileLa~",
                        column: x => x.UserLanguageProfileLanguageProfileId,
                        principalTable: "UserLanguageProfiles",
                        principalColumn: "LanguageProfileId");
                });

            migrationBuilder.CreateTable(
                name: "SavedContents",
                columns: table => new
                {
                    SavedContentId = table.Column<Guid>(type: "uuid", nullable: false),
                    SavedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ContentUrl = table.Column<string>(type: "text", nullable: true),
                    LanguageProfileId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedContents", x => x.SavedContentId);
                    table.ForeignKey(
                        name: "FK_SavedContents_UserLanguageProfiles_LanguageProfileId",
                        column: x => x.LanguageProfileId,
                        principalTable: "UserLanguageProfiles",
                        principalColumn: "LanguageProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTerms",
                columns: table => new
                {
                    UserTermId = table.Column<Guid>(type: "uuid", nullable: false),
                    LanguageProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    Language = table.Column<string>(type: "text", nullable: true),
                    NormalizedTermValue = table.Column<string>(type: "text", nullable: true),
                    TimesSeen = table.Column<int>(type: "integer", nullable: false),
                    EaseFactor = table.Column<float>(type: "real", nullable: false),
                    Rating = table.Column<int>(type: "integer", nullable: false),
                    DateTimeDue = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SrsIntervalDays = table.Column<float>(type: "real", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Starred = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTerms", x => x.UserTermId);
                    table.ForeignKey(
                        name: "FK_UserTerms_UserLanguageProfiles_LanguageProfileId",
                        column: x => x.LanguageProfileId,
                        principalTable: "UserLanguageProfiles",
                        principalColumn: "LanguageProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContentViewRecords",
                columns: table => new
                {
                    ContentViewRecordId = table.Column<Guid>(type: "uuid", nullable: false),
                    ContentHistoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    ContentUrl = table.Column<string>(type: "text", nullable: true),
                    AccessedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastSectionViewed = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentViewRecords", x => x.ContentViewRecordId);
                    table.ForeignKey(
                        name: "FK_ContentViewRecords_ContentHistories_ContentHistoryId",
                        column: x => x.ContentHistoryId,
                        principalTable: "ContentHistories",
                        principalColumn: "ContentHistoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CollectionContents",
                columns: table => new
                {
                    CollectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    ContentId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectionContents", x => new { x.CollectionId, x.ContentId });
                    table.ForeignKey(
                        name: "FK_CollectionContents_Collections_CollectionId",
                        column: x => x.CollectionId,
                        principalTable: "Collections",
                        principalColumn: "CollectionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CollectionContents_Contents_ContentId",
                        column: x => x.ContentId,
                        principalTable: "Contents",
                        principalColumn: "ContentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContentDifficulties",
                columns: table => new
                {
                    LanguageProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    ContentId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserLanguageProfileLanguageProfileId = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TotalWords = table.Column<int>(type: "integer", nullable: false),
                    KnownWords = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentDifficulties", x => new { x.LanguageProfileId, x.ContentId });
                    table.ForeignKey(
                        name: "FK_ContentDifficulties_Contents_ContentId",
                        column: x => x.ContentId,
                        principalTable: "Contents",
                        principalColumn: "ContentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContentDifficulties_UserLanguageProfiles_UserLanguageProfil~",
                        column: x => x.UserLanguageProfileLanguageProfileId,
                        principalTable: "UserLanguageProfiles",
                        principalColumn: "LanguageProfileId");
                });

            migrationBuilder.CreateTable(
                name: "ContentTags",
                columns: table => new
                {
                    ContentTagId = table.Column<Guid>(type: "uuid", nullable: false),
                    ContentId = table.Column<Guid>(type: "uuid", nullable: false),
                    TagValue = table.Column<string>(type: "text", nullable: true),
                    TagLanguage = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentTags", x => x.ContentTagId);
                    table.ForeignKey(
                        name: "FK_ContentTags_Contents_ContentId",
                        column: x => x.ContentId,
                        principalTable: "Contents",
                        principalColumn: "ContentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DailyProfileRecords",
                columns: table => new
                {
                    DailyProfileRecordId = table.Column<Guid>(type: "uuid", nullable: false),
                    DailyProfileHistoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    LanguageProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserLanguageProfileLanguageProfileId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    KnownWords = table.Column<int>(type: "integer", nullable: false),
                    WordsRead = table.Column<int>(type: "integer", nullable: false),
                    SecondsListened = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyProfileRecords", x => x.DailyProfileRecordId);
                    table.ForeignKey(
                        name: "FK_DailyProfileRecords_DailyProfileHistories_DailyProfileHisto~",
                        column: x => x.DailyProfileHistoryId,
                        principalTable: "DailyProfileHistories",
                        principalColumn: "DailyProfileHistoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DailyProfileRecords_UserLanguageProfiles_UserLanguageProfil~",
                        column: x => x.UserLanguageProfileLanguageProfileId,
                        principalTable: "UserLanguageProfiles",
                        principalColumn: "LanguageProfileId");
                });

            migrationBuilder.CreateTable(
                name: "PhraseTranslations",
                columns: table => new
                {
                    TranslationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true),
                    PhraseId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhraseTranslations", x => x.TranslationId);
                    table.ForeignKey(
                        name: "FK_PhraseTranslations_Phrases_PhraseId",
                        column: x => x.PhraseId,
                        principalTable: "Phrases",
                        principalColumn: "PhraseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Translations",
                columns: table => new
                {
                    TranslationId = table.Column<Guid>(type: "uuid", nullable: false),
                    TermValue = table.Column<string>(type: "text", nullable: true),
                    TermLanguage = table.Column<string>(type: "text", nullable: true),
                    UserValue = table.Column<string>(type: "text", nullable: true),
                    UserLanguage = table.Column<string>(type: "text", nullable: true),
                    UserTermId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Translations", x => x.TranslationId);
                    table.ForeignKey(
                        name: "FK_Translations_UserTerms_UserTermId",
                        column: x => x.UserTermId,
                        principalTable: "UserTerms",
                        principalColumn: "UserTermId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CollectionContents_ContentId",
                table: "CollectionContents",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentDifficulties_ContentId",
                table: "ContentDifficulties",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentDifficulties_UserLanguageProfileLanguageProfileId",
                table: "ContentDifficulties",
                column: "UserLanguageProfileLanguageProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentHistories_LanguageProfileId",
                table: "ContentHistories",
                column: "LanguageProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_LanguageProfileId",
                table: "Contents",
                column: "LanguageProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentTags_ContentId",
                table: "ContentTags",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentViewRecords_ContentHistoryId",
                table: "ContentViewRecords",
                column: "ContentHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyProfileHistories_LanguageProfileId",
                table: "DailyProfileHistories",
                column: "LanguageProfileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DailyProfileRecords_DailyProfileHistoryId",
                table: "DailyProfileRecords",
                column: "DailyProfileHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyProfileRecords_UserLanguageProfileLanguageProfileId",
                table: "DailyProfileRecords",
                column: "UserLanguageProfileLanguageProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Phrases_LanguageProfileId",
                table: "Phrases",
                column: "LanguageProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_PhraseTranslations_PhraseId",
                table: "PhraseTranslations",
                column: "PhraseId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedCollections_UserLanguageProfileLanguageProfileId",
                table: "SavedCollections",
                column: "UserLanguageProfileLanguageProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedContents_LanguageProfileId",
                table: "SavedContents",
                column: "LanguageProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Translations_UserTermId",
                table: "Translations",
                column: "UserTermId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLanguageProfiles_UserId",
                table: "UserLanguageProfiles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTerms_LanguageProfileId",
                table: "UserTerms",
                column: "LanguageProfileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CollectionContents");

            migrationBuilder.DropTable(
                name: "ContentDifficulties");

            migrationBuilder.DropTable(
                name: "ContentTags");

            migrationBuilder.DropTable(
                name: "ContentViewRecords");

            migrationBuilder.DropTable(
                name: "DailyProfileRecords");

            migrationBuilder.DropTable(
                name: "PhraseTranslations");

            migrationBuilder.DropTable(
                name: "SavedCollections");

            migrationBuilder.DropTable(
                name: "SavedContents");

            migrationBuilder.DropTable(
                name: "Translations");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Contents");

            migrationBuilder.DropTable(
                name: "ContentHistories");

            migrationBuilder.DropTable(
                name: "DailyProfileHistories");

            migrationBuilder.DropTable(
                name: "Phrases");

            migrationBuilder.DropTable(
                name: "Collections");

            migrationBuilder.DropTable(
                name: "UserTerms");

            migrationBuilder.DropTable(
                name: "UserLanguageProfiles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
