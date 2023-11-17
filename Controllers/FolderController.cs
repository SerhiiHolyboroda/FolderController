using FolderControler.Data;
using FolderControler.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FolderControler.Controllers {
    public class FolderController : Controller {
        private readonly ApplicationDbContext _db;
        public FolderController(ApplicationDbContext db) {
            _db = db;
        }
        public IActionResult Index(int? id) {
            List<Folder> folders;

            if (id.HasValue)
            {
                var folder = _db.Folders.Include(c => c.Children).FirstOrDefault(c => c.Id == id);
                folders = new List<Folder> { folder };
            }
            else
            {
                folders = _db.Folders.Include(c => c.Children).Where(c => c.ParentId == null).ToList();
            }

            return View(folders);
        }
    }
}