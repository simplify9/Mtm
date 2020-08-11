
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using SW.EfCoreExtensions;
using SW.Mtm.Api.Domain;
using SW.Mtm.Sdk.Model;
using SW.PrimitiveTypes;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SW.Mtm.Api
{
    public class MtmDbContext : DbContext
    {

        public const string ConnectionStringName = "MtmDb";

        // Mtm@dmin!2
        private readonly string defaultPasswordHash = "$SWHASH$V1$10000$VQCi48eitH4Ml5juvBMOFZrMdQwBbhuIQVXe6RR7qJdDF2bJ";
        private readonly DateTime defaultCreatedOn = DateTime.Parse("1/1/2020");
        private readonly IDomainEventDispatcher domainEventDispatcher;
        private readonly IConfiguration configuration;

        public RequestContext RequestContext { get; }

        public MtmDbContext(DbContextOptions options, RequestContext requestContext, IDomainEventDispatcher domainEventDispatcher, IConfiguration configuration) : base(options)
        {
            RequestContext = requestContext;
            this.domainEventDispatcher = domainEventDispatcher;
            this.configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Account>(b =>
            {
                b.ToTable("Accounts");
                b.HasKey(p => p.Id);
                b.Property(p => p.Id).IsUnicode(false).HasMaxLength(50);

                //b.HasOne<Tenant>().WithMany().IsRequired(false).HasForeignKey(p => p.TenantId).OnDelete(DeleteBehavior.SetNull);
                b.HasIndex(p => p.Email).IsUnique();
                b.HasIndex(p => p.Phone).IsUnique();

                b.Metadata.SetNavigationAccessMode(PropertyAccessMode.Field);

                b.Property(p => p.Email).IsUnicode(false).HasMaxLength(200);
                b.Property(p => p.Phone).IsUnicode(false).HasMaxLength(20);
                b.Property(p => p.Password).IsUnicode(false).HasMaxLength(500);
                b.Property(p => p.DisplayName).IsRequired().HasMaxLength(200);
                b.Property(p => p.EmailProvider).HasConversion<byte>();
                b.Property(p => p.LoginMethods).HasConversion<byte>();
                b.Property(p => p.SecondFactorMethod).HasConversion<byte>();
                b.Property(p => p.SecondFactorKey).IsUnicode(false).HasMaxLength(500);
                b.Property(p => p.Roles).IsSeparatorDelimited().IsUnicode(false).HasMaxLength(4000).IsRequired();


                b.OwnsMany(p => p.ApiCredentials, apicred =>
                {
                    apicred.ToTable("AccountApiCredentials");
                    apicred.Property(p => p.Name).IsRequired().HasMaxLength(500);
                    apicred.Property(p => p.Key).IsRequired().IsUnicode(false).HasMaxLength(500);
                    apicred.HasIndex(p => p.Key).IsUnique();
                    apicred.WithOwner().HasForeignKey("AccountId");

                    apicred.HasData(
                        new
                        {
                            Id = 1,
                            AccountId = Account.SystemId,
                            Name = "default",
                            Key = "dcc8edf250b04c94a31eb161fea11b5b",
                        },
                        new
                        {
                            Id = 2,
                            AccountId = "4cc3320b49af45dfb7ec13b072701acc",
                            Name = "default",
                            Key = "7facc758283844b49cc4ffd26a75b1de",
                        });
                });

                b.OwnsMany(p => p.TenantMemberships, membership =>
                {
                    membership.ToTable("AccountTenantMemberships");
                    membership.Property(p => p.Type).HasConversion<byte>();
                    membership.WithOwner().HasForeignKey("AccountId");
                    membership.HasOne<Tenant>().WithMany().HasForeignKey(p => p.TenantId).OnDelete(DeleteBehavior.Cascade);
                });


                b.HasData(
                    new
                    {
                        Id = Account.SystemId,
                        EmailProvider = EmailProvider.None,
                        LoginMethods = LoginMethod.ApiKey,
                        SecondFactorMethod = OtpType.None,
                        Landlord = false,
                        DisplayName = "System",
                        CreatedOn = defaultCreatedOn,
                        Deleted = false,
                        Disabled = false,
                        EmailVerified = false,
                        PhoneVerified = false,
                        Roles = new string[] { "Accounts.Login", "Accounts.Register" }
                    },
                    new
                    {
                        Id = "2",
                        Email = "admin@xyz.com",
                        Password = defaultPasswordHash,
                        EmailProvider = EmailProvider.None,
                        LoginMethods = LoginMethod.EmailAndPassword,
                        SecondFactorMethod = OtpType.None,
                        Landlord = true,
                        DisplayName = "Admin",
                        CreatedOn = defaultCreatedOn,
                        Deleted = false,
                        Disabled = false,
                        EmailVerified = true,
                        PhoneVerified = true,
                        Roles = new string[] { "Accounts.Register" }
                    },
                    new
                    {
                        Id = "2d3d997abdaf4e2880f2b4737aab6b0d",
                        Email = "sample@xyz.com",
                        Password = defaultPasswordHash,
                        EmailProvider = EmailProvider.None,
                        LoginMethods = LoginMethod.EmailAndPassword,
                        SecondFactorMethod = OtpType.None,
                        Landlord = false,
                        DisplayName = "Sample User",
                        CreatedOn = defaultCreatedOn,
                        Deleted = false,
                        Disabled = false,
                        EmailVerified = true,
                        PhoneVerified = false,
                        Roles = new string[] { }
                    },
                    new
                    {
                        Id = "4a64f3640d914cfa98f3c166fe22f59a",
                        Email = "samplewithmfa@xyz.com",
                        Password = defaultPasswordHash,
                        EmailProvider = EmailProvider.None,
                        LoginMethods = LoginMethod.EmailAndPassword,
                        SecondFactorMethod = OtpType.Otp,
                        Landlord = false,
                        DisplayName = "Sample User MFA",
                        CreatedOn = defaultCreatedOn,
                        Deleted = false,
                        Disabled = false,
                        EmailVerified = true,
                        PhoneVerified = false,
                        Roles = new string[] { }
                    },
                    new
                    {
                        Id = "40ec4db42e434bf5a17f2065521a5219",
                        Phone = "12345678",
                        EmailProvider = EmailProvider.None,
                        LoginMethods = LoginMethod.PhoneAndOtp,
                        SecondFactorMethod = OtpType.None,
                        Landlord = false,
                        DisplayName = "Sample User Phone",
                        CreatedOn = defaultCreatedOn,
                        Deleted = false,
                        Disabled = false,
                        EmailVerified = false,
                        PhoneVerified = true,
                        Roles = new string[] { }
                    },
                    new
                    {
                        Id = "4cc3320b49af45dfb7ec13b072701acc",
                        EmailProvider = EmailProvider.None,
                        LoginMethods = LoginMethod.ApiKey,
                        SecondFactorMethod = OtpType.None,
                        Landlord = false,
                        DisplayName = "Sample User API",
                        CreatedOn = defaultCreatedOn,
                        Deleted = false,
                        Disabled = false,
                        EmailVerified = false,
                        PhoneVerified = false,
                        Roles = new string[] { }
                    });

            });

            modelBuilder.Entity<RefreshToken>(b =>
            {
                b.ToTable("RefreshTokens");
                b.HasKey(p => p.Id);
                b.HasOne<Account>().WithMany().HasForeignKey(p => p.AccountId).OnDelete(DeleteBehavior.Cascade);

                b.Property(p => p.Id).IsUnicode(false).HasMaxLength(50);
                b.Property(p => p.AccountId).IsUnicode(false).HasMaxLength(50);
                b.Property(p => p.LoginMethod).HasConversion<byte>();

            });

            modelBuilder.Entity<OtpToken>(b =>
            {
                b.ToTable("OtpTokens");
                b.HasKey(p => p.Id);
                b.HasOne<Account>().WithMany().HasForeignKey(p => p.AccountId).OnDelete(DeleteBehavior.Cascade);

                b.Property(p => p.Id).IsUnicode(false).HasMaxLength(50);
                b.Property(p => p.AccountId).IsUnicode(false).HasMaxLength(50);
                b.Property(p => p.Password).IsUnicode(false).HasMaxLength(500);
                b.Property(p => p.Type).HasConversion<byte>();
                b.Property(p => p.LoginMethod).HasConversion<byte>();

            });

            modelBuilder.Entity<Invitation>(b =>
            {
                b.ToTable("Invitations");
                b.Property(p => p.Id).IsUnicode(false).HasMaxLength(50);

                //b.HasOne<Tenant>().WithMany().HasForeignKey(p => p.TenantId).OnDelete(DeleteBehavior.Cascade);
                b.HasOne<Account>().WithMany().HasForeignKey(p => p.AccountId).IsRequired(false).OnDelete(DeleteBehavior.Cascade);

                b.HasIndex(p => new { p.Email, p.TenantId }).IsUnique();
                b.HasIndex(p => new { p.Phone, p.TenantId }).IsUnique();
                b.HasIndex(p => new { p.AccountId, p.TenantId }).IsUnique();

                b.Property(p => p.AccountId).IsUnicode(false).HasMaxLength(50);
                b.Property(p => p.Email).IsUnicode(false).HasMaxLength(200);
                b.Property(p => p.Phone).IsUnicode(false).HasMaxLength(20);

            });

            modelBuilder.Entity<Tenant>(b =>
            {
                b.ToTable("Tenants");
                b.Property(p => p.DisplayName).IsRequired().HasMaxLength(200);
                b.Property(p => p.Id).HasSequenceGenerator();// ValueGeneratedOnAdd().HasValueGenerator<SequenceValueGenerator>();

            });

            modelBuilder.CommonProperties(b =>
            {
                b.HasTenantForeignKey<Tenant>();
                b.HasSoftDeletionQueryFilter();
                b.HasAudit();

            });


        }

        async public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                ChangeTracker.ApplyAuditValues(RequestContext.GetNameIdentifier());

                using var transaction = Database.BeginTransaction();

                var affectedRecords = await base.SaveChangesAsync(cancellationToken);

                await ChangeTracker.DispatchDomainEvents(domainEventDispatcher);

                await transaction.CommitAsync();

                return affectedRecords;

            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException == null)
                    throw new SWException($"Data Error: {dbUpdateException.Message}");
                else
                    throw new SWException($"Data Error: {dbUpdateException.InnerException.Message}");
            }
        }


    }
}

