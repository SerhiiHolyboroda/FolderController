using FolderControler.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace FolderControler.Data {
    public class ApplicationDbContext : DbContext {


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {
        }
        public DbSet<Folder> Folders { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {


            modelBuilder.Entity<Folder>().HasData(
               new Folder { Id = 1, Name = "Creating Digital Images", ParentId = null },
            new Folder { Id = 2, Name = "Resources", ParentId = 1 },
            new Folder { Id = 3, Name = "Evidence", ParentId = 1 },
            new Folder { Id = 4, Name = "Graphic Products", ParentId = 1 },
            new Folder { Id = 5, Name = "Primary Sources", ParentId = 2 },
            new Folder { Id = 6, Name = "Secondary Sources", ParentId = 2 },
            new Folder { Id = 7, Name = "Process", ParentId = 4 },
            new Folder { Id = 8, Name = "Final Product", ParentId = 4 }
            );
        }

    }

}
