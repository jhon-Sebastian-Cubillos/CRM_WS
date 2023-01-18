using Inmersys.Domain.DB.Schema.Client;
using Inmersys.Domain.DB.Schema.Definition;
using Inmersys.Domain.DB.Schema.History;
using Inmersys.Domain.DB.Schema.Profile;
using Inmersys.Domain.DB.Schema.Security;
using Microsoft.EntityFrameworkCore;

namespace Inmersys.Domain.DB
{
    public class CRM_DB : DbContext
    {
        public CRM_DB(DbContextOptions<CRM_DB> opt) : base(opt) { }

        #region Client

        public DbSet<Cli_Activity> cli_activities { get; set; }
        public DbSet<Cli_Asignment> cli_asignments { get; set; }
        public DbSet<Cli_Info> cli_info { get; set; }
        public DbSet<Cli_Tab> cli_tabs { get; set; }

        #endregion

        #region Definition

        public DbSet<Def_Action> def_actions { get; set; }
        public DbSet<Def_Gender> def_genders { get; set; }
        public DbSet<Def_Rol> def_rols { get; set; }
        public DbSet<Def_Window> def_windows { get; set; }

        #endregion

        #region History

        public DbSet<His_Changes> his_changes { get; set; }
        public DbSet<His_Session> his_sessions { get; set; }
        public DbSet<His_Transaction> his_transactions { get; set; }

        #endregion

        #region Profile

        public DbSet<Pro_Profile> pro_profiles { get; set; }

        #endregion

        #region Security

        public DbSet<Sec_Action> sec_actions { get; set; }
        public DbSet<Sec_Rol> sec_rols { get; set; }
        public DbSet<Sec_Window> sec_windows { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {
            #region Client

            builder.Entity<Cli_Activity>(ent =>
            {
                ent
                .ToTable("activity", "client");

                ent
                .HasOne(reg => reg.client_tab_info)
                .WithMany(reg => reg.tab_activities_info)
                .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Cli_Asignment>(ent =>
            {
                ent
                .ToTable("asignment", "client");

                ent
                .HasOne(reg => reg.profile_info)
                .WithMany(reg => reg.client_asignments_info)
                .OnDelete(DeleteBehavior.Cascade);

                ent
                .HasOne(reg => reg.activity_info)
                .WithMany(reg => reg.asignments_info)
                .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Cli_Info>(ent =>
            {
                ent
                .ToTable("info", "client");
            });

            builder.Entity<Cli_Tab>(ent =>
            {
                ent
                .ToTable("tab", "client");

                ent
                .HasOne(reg => reg.client_info)
                .WithMany(reg => reg.cient_tabs_info)
                .OnDelete(DeleteBehavior.Cascade);

            });

            #endregion

            #region Definition

            builder.Entity<Def_Action>(ent =>
            {
                ent
                .ToTable("action", "definition");
            });

            builder.Entity<Def_Gender>(ent =>
            {
                ent
                .ToTable("gender", "definition");
            });

            builder.Entity<Def_Rol>(ent =>
            {
                ent
                .ToTable("rol", "definition");
            });

            builder.Entity<Def_Window>(ent =>
            {
                ent
                .ToTable("window", "definition");
            });

            #endregion

            #region History

            builder.Entity<His_Changes>(ent =>
            {
                ent
                .ToTable("change", "history");

                ent
                .HasOne(reg => reg.transaction_info)
                .WithMany(reg => reg.changes_info)
                .OnDelete(DeleteBehavior.Restrict);

            });

            builder.Entity<His_Session>(ent =>
            {
                ent
                .ToTable("session", "history");

                ent
                .HasIndex(reg => reg.Jti)
                .IsUnique();

            });

            builder.Entity<His_Transaction>(ent =>
            {
                ent
                .ToTable("transaction", "history");

                ent
                .HasOne(reg => reg.session_info)
                .WithMany(reg => reg.transactions_info)
                .OnDelete(DeleteBehavior.Restrict);

            });

            #endregion

            #region Profile

            builder.Entity<Pro_Profile>(ent =>
            {
                ent
                .ToTable("profile", "profile");

                ent
                .HasOne(reg => reg.gender_info)
                .WithMany(reg => reg.profiles_info)
                .OnDelete(DeleteBehavior.Restrict);

            });

            #endregion

            #region Security

            builder.Entity<Sec_Action>(ent =>
            {
                ent
                .ToTable("action", "security");

                ent
                .HasOne(reg => reg.action_info)
                .WithMany(reg => reg.security_roles_info)
                .OnDelete(DeleteBehavior.Cascade);

                ent
                .HasOne(reg => reg.rol_info)
                .WithMany(reg => reg.security_actions_info)
                .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Sec_Rol>(ent =>
            {
                ent
                .ToTable("rol", "security");

                ent
                .HasOne(reg => reg.profile_info)
                .WithMany(reg => reg.security_roles_info)
                .OnDelete(DeleteBehavior.Cascade);

                ent
                .HasOne(reg => reg.rol_info)
                .WithMany(reg => reg.security_profiles_info)
                .OnDelete(DeleteBehavior.Cascade);
            
            });

            builder.Entity<Sec_Window>(ent =>
            {
                ent
                .ToTable("window", "security");

                ent
                .HasOne(reg => reg.window_info)
                .WithMany(reg => reg.security_roles_info)
                .OnDelete(DeleteBehavior.Cascade);

                ent
                .HasOne(reg => reg.rol_info)
                .WithMany(reg => reg.security_windows_info)
                .OnDelete(DeleteBehavior.Cascade);

            });

            #endregion
        }
    }
}
