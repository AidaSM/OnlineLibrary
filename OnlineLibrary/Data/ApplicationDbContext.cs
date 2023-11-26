using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using OnlineLibrary.Models.DBObjects;

namespace OnlineLibrary.Data
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; } = null!;
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; } = null!;
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; } = null!;
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; } = null!;
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; } = null!;
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; } = null!;
        public virtual DbSet<Author> Authors { get; set; } = null!;
        public virtual DbSet<Book> Books { get; set; } = null!;
        public virtual DbSet<Fee> Fees { get; set; } = null!;
        public virtual DbSet<Genre> Genres { get; set; } = null!;
        public virtual DbSet<Member> Members { get; set; } = null!;
        public virtual DbSet<Review> Reviews { get; set; } = null!;
        public virtual DbSet<Transaction> Transactions { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=DefaultConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetRoleClaim>(entity =>
            {
                entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);

                entity.HasMany(d => d.Roles)
                    .WithMany(p => p.Users)
                    .UsingEntity<Dictionary<string, object>>(
                        "AspNetUserRole",
                        l => l.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                        r => r.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                        j =>
                        {
                            j.HasKey("UserId", "RoleId");

                            j.ToTable("AspNetUserRoles");

                            j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                        });
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<Author>(entity =>
            {
                entity.HasKey(e => e.Idauthor)
                    .HasName("PK__Authors__FE2889A4BE910CCD");

                entity.Property(e => e.Idauthor)
                    .ValueGeneratedNever()
                    .HasColumnName("IDAuthor");

                entity.Property(e => e.BirthDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.Nationality)
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.Idbook)
                    .HasName("PK__Books__2339855F103A2256");

                entity.Property(e => e.Idbook)
                    .ValueGeneratedNever()
                    .HasColumnName("IDBook");

                entity.Property(e => e.Idauthor).HasColumnName("IDAuthor");

                entity.Property(e => e.Idgenre).HasColumnName("IDGenre");

                entity.Property(e => e.Isbn)
                    .HasMaxLength(50)
                    .HasColumnName("ISBN")
                    .IsFixedLength();

                entity.Property(e => e.Language)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Title)
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.HasOne(d => d.IdauthorNavigation)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.Idauthor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Books_Authors");

                entity.HasOne(d => d.IdgenreNavigation)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.Idgenre)
                    .HasConstraintName("FK_Books_Genres");
            });

            modelBuilder.Entity<Fee>(entity =>
            {
                entity.HasKey(e => e.Idfee)
                    .HasName("PK__Fees__92682FE9743DF9AE");

                entity.Property(e => e.Idfee)
                    .ValueGeneratedNever()
                    .HasColumnName("IDFee");

                entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.Idbook).HasColumnName("IDBook");

                entity.Property(e => e.Idmember).HasColumnName("IDMember");

                entity.HasOne(d => d.IdbookNavigation)
                    .WithMany(p => p.Fees)
                    .HasForeignKey(d => d.Idbook)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Fees_Books");

                entity.HasOne(d => d.IdmemberNavigation)
                    .WithMany(p => p.Fees)
                    .HasForeignKey(d => d.Idmember)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Fees_Members");
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.HasKey(e => e.Idgenre)
                    .HasName("PK__Genres__23FDA2F009DF3C53");

                entity.Property(e => e.Idgenre)
                    .ValueGeneratedNever()
                    .HasColumnName("IDGenre");

                entity.Property(e => e.Name)
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.HasKey(e => e.Idmember)
                    .HasName("PK__Members__7EB75A6325E7EF64");

                entity.Property(e => e.Idmember)
                    .ValueGeneratedNever()
                    .HasColumnName("IDMember");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.RegistrationDate).HasColumnType("datetime");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasKey(e => e.Idreview)
                    .HasName("PK__Reviews__E5003B6F196F4F86");

                entity.Property(e => e.Idreview)
                    .ValueGeneratedNever()
                    .HasColumnName("IDReview");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Idbook).HasColumnName("IDBook");

                entity.Property(e => e.Idmember).HasColumnName("IDMember");

                entity.Property(e => e.Rating).HasColumnType("decimal(5, 1)");

                entity.Property(e => e.Text).HasColumnType("text");

                entity.HasOne(d => d.IdbookNavigation)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.Idbook)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Reviews_Books");

                entity.HasOne(d => d.IdmemberNavigation)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.Idmember)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Reviews_Members");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.Idtransaction)
                    .HasName("PK__Transact__A3F081DFEDCC2144");

                entity.Property(e => e.Idtransaction)
                    .ValueGeneratedNever()
                    .HasColumnName("IDTransaction");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Idbook).HasColumnName("IDBook");

                entity.Property(e => e.Idmember).HasColumnName("IDMember");

                entity.Property(e => e.Retrun).HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.HasOne(d => d.IdbookNavigation)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.Idbook)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transactions_Books");

                entity.HasOne(d => d.IdmemberNavigation)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.Idmember)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transactions_Members");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
