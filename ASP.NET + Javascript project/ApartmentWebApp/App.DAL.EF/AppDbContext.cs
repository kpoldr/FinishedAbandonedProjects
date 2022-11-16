using App.Domain;
using App.Domain.Identity;
using Base.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF;

    public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public DbSet<RefreshToken> RefreshTokens { get; set; } = default!;
        public DbSet<Apartment> Apartments { get; set; } = default!;
        public DbSet<Association> Associations { get; set; } = default!;
        public DbSet<Bill> Bills { get; set; } = default!;
        public DbSet<BillPayment> BillPayments { get; set; } = default!;
        public DbSet<Building> Buildings { get; set; } = default!;
        public DbSet<Contract> Contracts { get; set; } = default!;
        public DbSet<Fund> Funds { get; set; } = default!;
        public DbSet<Owner> Owners { get; set; } = default!;
        public DbSet<Payment> Payments { get; set; } = default!;
        public DbSet<Penalty> Penalties { get; set; } = default!;
        public DbSet<Person> Persons { get; set; } = default!;
        public DbSet<Utility> Utilities { get; set; } = default!;
        public DbSet<UtilityBill> UtilityBills { get; set; } = default!;




        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // remove cascade delete
            
            
            
            if (Database.ProviderName == "Microsoft.EntityFrameworkCore.InMemory")
            {
                builder
                    .Entity<Association>()
                    .Property(e => e.Name)
                    .HasConversion(
                        v => SerialiseLangStr(v),
                        v => DeserializeLangStr(v));
                
                builder
                    .Entity<Association>()
                    .Property(e => e.Description)
                    .HasConversion(
                        v => SerialiseLangStr(v),
                        v => DeserializeLangStr(v));
                
                builder
                    .Entity<Association>()
                    .Property(e => e.BankName)
                    .HasConversion(
                        v => SerialiseLangStr(v),
                        v => DeserializeLangStr(v));

                builder
                    .Entity<Building>()
                    .Property(e => e.Address)
                    .HasConversion(
                        v => SerialiseLangStr(v),
                        v => DeserializeLangStr(v));
                
                builder
                    .Entity<Contract>()
                    .Property(e => e.Name)
                    .HasConversion(
                        v => SerialiseLangStr(v),
                        v => DeserializeLangStr(v));
                
                builder
                    .Entity<Contract>()
                    .Property(e => e.Description)
                    .HasConversion(
                        v => SerialiseLangStr(v),
                        v => DeserializeLangStr(v));
                
                builder
                    .Entity<Fund>()
                    .Property(e => e.Name)
                    .HasConversion(
                        v => SerialiseLangStr(v),
                        v => DeserializeLangStr(v));
                
                builder
                    .Entity<Owner>()
                    .Property(e => e.Name)
                    .HasConversion(
                        v => SerialiseLangStr(v),
                        v => DeserializeLangStr(v));
                
                builder
                    .Entity<Penalty>()
                    .Property(e => e.PenaltyName)
                    .HasConversion(
                        v => SerialiseLangStr(v),
                        v => DeserializeLangStr(v));
                
                builder
                    .Entity<Person>()
                    .Property(e => e.Name)
                    .HasConversion(
                        v => SerialiseLangStr(v),
                        v => DeserializeLangStr(v));
                
                builder
                    .Entity<Utility>()
                    .Property(e => e.Name)
                    .HasConversion(
                        v => SerialiseLangStr(v),
                        v => DeserializeLangStr(v));
            }
            
            foreach (var relationship in builder.Model.GetEntityTypes().
                         SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            
            builder.Entity<Bill>().HasOne(b => b.PreviousBill).WithMany().HasForeignKey(f => f.PreviousBillId);

        }
        private static string SerialiseLangStr(LangStr? lStr) => System.Text.Json.JsonSerializer.Serialize(lStr);

        private static LangStr DeserializeLangStr(string jsonStr) =>
            System.Text.Json.JsonSerializer.Deserialize<LangStr>(jsonStr) ?? new LangStr();

        
        public override int SaveChanges()
        {
            FixEntities(this);
        
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            FixEntities(this);
        
            return base.SaveChangesAsync(cancellationToken);
        }

        
        private void FixEntities(AppDbContext context)
        {
            var dateProperties = context.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(DateTime))
                .Select(z => new
                {
                    ParentName = z.DeclaringEntityType.Name,
                    PropertyName = z.Name
                });

            var editedEntitiesInTheDbContextGraph = context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
                .Select(x => x.Entity);
        

            foreach (var entity in editedEntitiesInTheDbContextGraph)
            {
                var entityFields = dateProperties.Where(d => d.ParentName == entity.GetType().FullName);

                foreach (var property in entityFields)
                {
                    var prop = entity.GetType().GetProperty(property.PropertyName);

                    if (prop == null)
                        continue;

                    var originalValue = prop.GetValue(entity) as DateTime?;
                    if (originalValue == null)
                        continue;

                    prop.SetValue(entity, DateTime.SpecifyKind(originalValue.Value, DateTimeKind.Utc));
                }
            }
        }
        
    }
