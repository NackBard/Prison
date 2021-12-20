using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Prison.Model
{
    public partial class PrisonContext : DbContext
    {
        public PrisonContext()
        {
        }

        public PrisonContext(DbContextOptions<PrisonContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AccessLevel> AccessLevels { get; set; }
        public virtual DbSet<AccountingDiningVisit> AccountingDiningVisits { get; set; }
        public virtual DbSet<AccountingPrisoner> AccountingPrisoners { get; set; }
        public virtual DbSet<AccountingRehabilitationWork> AccountingRehabilitationWorks { get; set; }
        public virtual DbSet<AccountingType> AccountingTypes { get; set; }
        public virtual DbSet<BehaviorAssessment> BehaviorAssessments { get; set; }
        public virtual DbSet<Dish> Dishes { get; set; }
        public virtual DbSet<Gender> Genders { get; set; }
        public virtual DbSet<JournalArrivalAndDeparture> JournalArrivalAndDepartures { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Prisoner> Prisoners { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Prosecution> Prosecutions { get; set; }
        public virtual DbSet<SalesAccounting> SalesAccountings { get; set; }
        public virtual DbSet<Set> Sets { get; set; }
        public virtual DbSet<Status> Statuses { get; set; }
        public virtual DbSet<TypeProduct> TypeProducts { get; set; }
        public virtual DbSet<Warehouse> Warehouses { get; set; }
        public virtual DbSet<Work> Works { get; set; }
        public virtual DbSet<Worker> Workers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<AccessLevel>(entity =>
            {
                entity.ToTable("Access_level");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AccountingDiningVisit>(entity =>
            {
                entity.ToTable("Accounting_dining_visit");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.SetId).HasColumnName("Set_Id");
            });

            modelBuilder.Entity<AccountingPrisoner>(entity =>
            {
                entity.ToTable("Accounting_prisoner");

                entity.Property(e => e.AssessmentId).HasColumnName("Assessment_Id");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.DateOfEntry)
                    .HasColumnType("datetime")
                    .HasColumnName("Date_of_entry");

                entity.Property(e => e.PrisonerId).HasColumnName("Prisoner_Id");

                entity.Property(e => e.WorkerId).HasColumnName("Worker_Id");
            });

            modelBuilder.Entity<AccountingRehabilitationWork>(entity =>
            {
                entity.ToTable("Accounting_rehabilitation_works");

                entity.Property(e => e.PrisonerId).HasColumnName("Prisoner_Id");

                entity.Property(e => e.WorkId).HasColumnName("Work_Id");
            });

            modelBuilder.Entity<AccountingType>(entity =>
            {
                entity.ToTable("Accounting_type");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BehaviorAssessment>(entity =>
            {
                entity.ToTable("Behavior_assessment");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Dish>(entity =>
            {
                entity.ToTable("Dish");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(70)
                    .IsUnicode(false);

                entity.Property(e => e.SetId).HasColumnName("Set_Id");
            });

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.ToTable("Gender");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(7)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<JournalArrivalAndDeparture>(entity =>
            {
                entity.ToTable("Journal_arrival_and_departure");

                entity.Property(e => e.AccountingTypeId).HasColumnName("Accounting_type_Id");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.WorkerId).HasColumnName("Worker_Id");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("Post");

                entity.Property(e => e.LevelId).HasColumnName("Level_Id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Prisoner>(entity =>
            {
                entity.ToTable("Prisoner");

                entity.Property(e => e.AdditionalInformation)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasColumnName("Additional_information");

                entity.Property(e => e.DateOfBirthday)
                    .HasColumnType("datetime")
                    .HasColumnName("Date_of_birthday");

                entity.Property(e => e.DateOfConclusion)
                    .HasColumnType("datetime")
                    .HasColumnName("Date_of_conclusion");

                entity.Property(e => e.GenderId).HasColumnName("Gender_Id");

                entity.Property(e => e.MiddleName)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("Middle_name");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.ProsecutionId).HasColumnName("Prosecution_Id");

                entity.Property(e => e.StatusId).HasColumnName("Status_Id");

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Verdict)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.ProductTypeId).HasColumnName("Product_type_Id");
            });

            modelBuilder.Entity<Prosecution>(entity =>
            {
                entity.ToTable("Prosecution");

                entity.Property(e => e.Article)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SalesAccounting>(entity =>
            {
                entity.ToTable("Sales_accounting");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.PrisonerId).HasColumnName("Prisoner_Id");

                entity.Property(e => e.ProductId).HasColumnName("Product_Id");
            });

            modelBuilder.Entity<Set>(entity =>
            {
                entity.ToTable("Set");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.ToTable("Status");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(60)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TypeProduct>(entity =>
            {
                entity.ToTable("Type_product");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Warehouse>(entity =>
            {
                entity.ToTable("Warehouse");

                entity.Property(e => e.ProductId).HasColumnName("Product_Id");
            });

            modelBuilder.Entity<Work>(entity =>
            {
                entity.ToTable("Work");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Worker>(entity =>
            {
                entity.ToTable("Worker");

                entity.HasIndex(e => e.Login, "UQ_Login")
                    .IsUnique();

                entity.Property(e => e.AdditionalInformation)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasColumnName("Additional_information");

                entity.Property(e => e.DateOfBirth)
                    .HasColumnType("datetime")
                    .HasColumnName("Date_of_birth");

                entity.Property(e => e.GenderId).HasColumnName("Gender_Id");

                entity.Property(e => e.Login)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.MiddleName)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("Middle_name");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.PostId).HasColumnName("Post_Id");

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
