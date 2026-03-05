using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Infrastructure.Context;

public class TaskFlowContext(DbContextOptions<TaskFlowContext> options) : DbContext(options)
{
    // ── DbSets ────────────────────────────────────────────────────────────────
    public DbSet<User>           Users           { get; set; }
    public DbSet<Role>           Roles           { get; set; }
    public DbSet<UserRole>       UserRoles       { get; set; }
    public DbSet<Project>        Projects        { get; set; }
    public DbSet<TaskItem>       TaskItems       { get; set; }
    public DbSet<TaskComment>    TaskComments    { get; set; }
    public DbSet<TaskAttachment> TaskAttachments { get; set; }
    public DbSet<TaskHistory>    TaskHistories   { get; set; }
    public DbSet<RefreshToken>   RefreshTokens   { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ====================================================================
        // USER — business FKs
        // ====================================================================
        // User.CreatedBy / UpdatedBy point to itself — must be Restrict
        modelBuilder.Entity<User>()
            .HasOne(e => e.CreatedByUser)
            .WithMany()
            .HasForeignKey(e => e.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<User>()
            .HasOne(e => e.UpdatedByUser)
            .WithMany()
            .HasForeignKey(e => e.UpdatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        // ====================================================================
        // ROLE — audit FKs
        // ====================================================================
        modelBuilder.Entity<Role>()
            .HasOne(e => e.CreatedByUser)
            .WithMany(u => u.CreatedRoles)
            .HasForeignKey(e => e.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Role>()
            .HasOne(e => e.UpdatedByUser)
            .WithMany(u => u.UpdatedRoles)
            .HasForeignKey(e => e.UpdatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        // ====================================================================
        // USER ROLE — composite uniqueness + audit FKs
        // ====================================================================
        modelBuilder.Entity<UserRole>()
            .HasIndex(ur => new { ur.UserId, ur.RoleId })
            .IsUnique();

        modelBuilder.Entity<UserRole>()
            .HasOne(e => e.CreatedByUser)
            .WithMany(u => u.CreatedUserRoles)
            .HasForeignKey(e => e.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserRole>()
            .HasOne(e => e.UpdatedByUser)
            .WithMany(u => u.UpdatedUserRoles)
            .HasForeignKey(e => e.UpdatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        // ====================================================================
        // PROJECT — business FK (Owner) + audit FKs
        // ====================================================================
        modelBuilder.Entity<Project>()
            .HasOne(p => p.Owner)
            .WithMany(u => u.OwnedProjects)
            .HasForeignKey(p => p.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Project>()
            .HasOne(e => e.CreatedByUser)
            .WithMany(u => u.CreatedProjects)
            .HasForeignKey(e => e.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Project>()
            .HasOne(e => e.UpdatedByUser)
            .WithMany(u => u.UpdatedProjects)
            .HasForeignKey(e => e.UpdatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        // ====================================================================
        // TASK ITEM — business FKs (AssignedTo + Project) + audit FKs
        // ====================================================================
        // AssignedTo: Restrict (User -> Tasks would create multiple cascade paths)
        modelBuilder.Entity<TaskItem>()
            .HasOne(t => t.AssignedTo)
            .WithMany(u => u.AssignedTasks)
            .HasForeignKey(t => t.AssignedToId)
            .OnDelete(DeleteBehavior.Restrict);

        // Project -> TaskItems can cascade (project deletion removes tasks)
        modelBuilder.Entity<TaskItem>()
            .HasOne(t => t.Project)
            .WithMany(p => p.Tasks)
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TaskItem>()
            .HasOne(e => e.CreatedByUser)
            .WithMany(u => u.CreatedTaskItems)
            .HasForeignKey(e => e.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TaskItem>()
            .HasOne(e => e.UpdatedByUser)
            .WithMany(u => u.UpdatedTaskItems)
            .HasForeignKey(e => e.UpdatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        // ====================================================================
        // TASK COMMENT — business FK (User) + audit FKs
        // ====================================================================
        modelBuilder.Entity<TaskComment>()
            .HasOne(c => c.User)
            .WithMany(u => u.TaskComments)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TaskComment>()
            .HasOne(e => e.CreatedByUser)
            .WithMany(u => u.CreatedComments)
            .HasForeignKey(e => e.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TaskComment>()
            .HasOne(e => e.UpdatedByUser)
            .WithMany(u => u.UpdatedComments)
            .HasForeignKey(e => e.UpdatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        // ====================================================================
        // TASK ATTACHMENT — business FK (TaskItem) + audit FKs
        // ====================================================================
        modelBuilder.Entity<TaskAttachment>()
            .HasOne(a => a.TaskItem)
            .WithMany(t => t.Attachments)
            .HasForeignKey(a => a.TaskItemId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TaskAttachment>()
            .HasOne(e => e.CreatedByUser)
            .WithMany(u => u.CreatedAttachments)
            .HasForeignKey(e => e.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TaskAttachment>()
            .HasOne(e => e.UpdatedByUser)
            .WithMany(u => u.UpdatedAttachments)
            .HasForeignKey(e => e.UpdatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        // ====================================================================
        // TASK HISTORY — business FK (TaskItem) + audit FKs
        // ====================================================================
        modelBuilder.Entity<TaskHistory>()
            .HasOne(h => h.TaskItem)
            .WithMany(t => t.Histories)
            .HasForeignKey(h => h.TaskItemId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TaskHistory>()
            .HasOne(e => e.CreatedByUser)
            .WithMany(u => u.CreatedHistories)
            .HasForeignKey(e => e.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TaskHistory>()
            .HasOne(e => e.UpdatedByUser)
            .WithMany(u => u.UpdatedHistories)
            .HasForeignKey(e => e.UpdatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        // ====================================================================
        // REFRESH TOKEN — business FK (User) + audit FKs
        // ====================================================================
        modelBuilder.Entity<RefreshToken>()
            .HasOne(r => r.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RefreshToken>()
            .HasOne(e => e.CreatedByUser)
            .WithMany(u => u.CreatedTokens)
            .HasForeignKey(e => e.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<RefreshToken>()
            .HasOne(e => e.UpdatedByUser)
            .WithMany(u => u.UpdatedTokens)
            .HasForeignKey(e => e.UpdatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        // ====================================================================
        // GLOBAL QUERY FILTER — soft deletes
        // ====================================================================
        modelBuilder.Entity<User>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Role>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Project>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<TaskItem>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<TaskComment>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<TaskAttachment>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<TaskHistory>().HasQueryFilter(e => !e.IsDeleted);
    }
}

