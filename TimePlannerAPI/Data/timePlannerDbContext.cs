using Microsoft.EntityFrameworkCore;
using TimePlannerAPI.Models;

namespace TimePlannerAPI.Data
{
    public class timePlannerDbContext :DbContext
    {
        //Dbset Properties represent database tables
        public DbSet<User> Users { get; set; }
        public DbSet<Schedule> Schedules { get; set; }   
        public DbSet<TimeBlock> TimeBlocks { get; set; }
        public DbSet<Activity> Activities { get; set; }


        //Constructor accepting DbContextOptions for Congiguration
        public timePlannerDbContext(DbContextOptions<timePlannerDbContext>oPtions) : base(oPtions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //configure Entity relationships and constraints

            //User-Schedule relationship (1-to-many)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Schedules)
                .WithOne(s => s.Users)
                .HasForeignKey(s => s.UserId);


            //Schedule-TimeBlock relationship (1-to-many)
            modelBuilder.Entity<Schedule>()
                .HasMany(s => s.TimeBlocks)
                .WithOne(tb => tb.Schedule)
                .HasForeignKey(tb => tb.ScheduleId);



            //Setup indexes for better Query Performance
            // In your TimePlannerDbContext.cs
            modelBuilder.Entity<TimeBlock>()
                .HasIndex(tb => tb.StartTime);

            modelBuilder.Entity<TimeBlock>(entity =>
            {
                entity.HasKey(tb => tb.Id);

                entity.Property(tb => tb.Title)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(tb => tb.Description)
                    .HasMaxLength(500);

                entity.Property(tb => tb.StartTime)
                    .IsRequired();

                entity.Property(tb => tb.EndTime)
                    .IsRequired();

                entity.HasOne(tb => tb.Schedule)
                    .WithMany(s => s.TimeBlocks)
                    .HasForeignKey(tb => tb.ScheduleId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(tb => tb.ScheduleId);
                entity.HasIndex(tb => tb.StartTime);
                entity.HasIndex(tb => tb.EndTime);
            });

        }
        
        }
    }

