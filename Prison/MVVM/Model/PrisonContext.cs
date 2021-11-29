using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

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

                entity.HasOne(d => d.PrisonerNavigation)
                    .WithMany(p => p.AccountingDiningVisits)
                    .HasForeignKey(d => d.Prisoner)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Accountin__Priso__787EE5A0");

                entity.HasOne(d => d.Set)
                    .WithMany(p => p.AccountingDiningVisits)
                    .HasForeignKey(d => d.SetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Accountin__Set_I__778AC167");
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

                entity.HasOne(d => d.Assessment)
                    .WithMany(p => p.AccountingPrisoners)
                    .HasForeignKey(d => d.AssessmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Accountin__Asses__04E4BC85");

                entity.HasOne(d => d.Prisoner)
                    .WithMany(p => p.AccountingPrisoners)
                    .HasForeignKey(d => d.PrisonerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Accountin__Priso__03F0984C");

                entity.HasOne(d => d.Worker)
                    .WithMany(p => p.AccountingPrisoners)
                    .HasForeignKey(d => d.WorkerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Accountin__Worke__02FC7413");
            });

            modelBuilder.Entity<AccountingRehabilitationWork>(entity =>
            {
                entity.ToTable("Accounting_rehabilitation_works");

                entity.Property(e => e.PrisonerId).HasColumnName("Prisoner_Id");

                entity.Property(e => e.WorkId).HasColumnName("Work_Id");

                entity.HasOne(d => d.Prisoner)
                    .WithMany(p => p.AccountingRehabilitationWorks)
                    .HasForeignKey(d => d.PrisonerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Accountin__Priso__7C4F7684");

                entity.HasOne(d => d.Work)
                    .WithMany(p => p.AccountingRehabilitationWorks)
                    .HasForeignKey(d => d.WorkId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Accountin__Work___7B5B524B");
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

                entity.HasOne(d => d.Set)
                    .WithMany(p => p.Dishes)
                    .HasForeignKey(d => d.SetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Dish__Set_Id__619B8048");
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

                entity.HasOne(d => d.AccountingType)
                    .WithMany(p => p.JournalArrivalAndDepartures)
                    .HasForeignKey(d => d.AccountingTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Journal_a__Accou__00200768");

                entity.HasOne(d => d.Worker)
                    .WithMany(p => p.JournalArrivalAndDepartures)
                    .HasForeignKey(d => d.WorkerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Journal_a__Worke__7F2BE32F");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("Post");

                entity.Property(e => e.LevelId).HasColumnName("Level_Id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.HasOne(d => d.Level)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.LevelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Post__Level_Id__5EBF139D");
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

                entity.HasOne(d => d.Gender)
                    .WithMany(p => p.Prisoners)
                    .HasForeignKey(d => d.GenderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Prisoner__Gender__6477ECF3");

                entity.HasOne(d => d.Prosecution)
                    .WithMany(p => p.Prisoners)
                    .HasForeignKey(d => d.ProsecutionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Prisoner__Prosec__656C112C");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Prisoners)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Prisoner__Status__66603565");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.ProductTypeId).HasColumnName("Product_type_Id");

                entity.HasOne(d => d.ProductType)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ProductTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Product__Product__5AEE82B9");
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

                entity.HasOne(d => d.Prisoner)
                    .WithMany(p => p.SalesAccountings)
                    .HasForeignKey(d => d.PrisonerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Sales_acc__Priso__6FE99F9F");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.SalesAccountings)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Sales_acc__Produ__6EF57B66");
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

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Warehouses)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Warehouse__Produ__6C190EBB");
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

                entity.HasOne(d => d.Gender)
                    .WithMany(p => p.Workers)
                    .HasForeignKey(d => d.GenderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Worker__Gender_I__73BA3083");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Workers)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Worker__Post_Id__74AE54BC");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
