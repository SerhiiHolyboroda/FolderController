using FolderControler.Data;
using FolderControler.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Newtonsoft.Json;

namespace FolderController.Controllers {
    public class FolderExportController : Controller {
        private readonly ApplicationDbContext _db;

        public FolderExportController(ApplicationDbContext db) {
            _db = db;
        }

        public IActionResult Index() {
            var rootFolders = _db.Folders.Include(c => c.Children).Where(c => c.ParentId == null).ToList();

            return View(rootFolders);
        }
        [HttpPost]
        public IActionResult ExportFolders(int id) {
            var mainFolder = _db.Folders
                .Include(c => c.Children)
                .ThenInclude(c => c.Children)
                .Include(c => c.Parent)
                .FirstOrDefault(c => c.Id == id);

            if (mainFolder == null)
            {
                return NotFound();
            }

            var archiveFileName = $"{mainFolder.Name}_Export.zip";

            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    AddFolderToZip(mainFolder, archive, "");
                }

                return File(memoryStream.ToArray(), "application/zip", archiveFileName);
            }
        }

        private void AddFolderToZip(Folder folder, ZipArchive archive, string path) {
            if (folder != null)
            {
                var entry = archive.CreateEntry($"{path}{folder.Name}/");
                if (folder.Children != null)
                {
                    foreach (var child in folder.Children)
                    {
                        AddFolderToZip(child, archive, $"{path}{folder.Name}/");
                    }
                }
                else
                {
                    AddFolderToZip(null, archive, $"{path}{folder.Name}/");
                }
            }
            else
            {

            }
        }




        [HttpPost]
        public IActionResult ExportJson(int id) {
            var mainFolder = _db.Folders.Include(c => c.Children).FirstOrDefault(c => c.Id == id);

            if (mainFolder == null)
            {
                return NotFound();
            }

            var serializedData = JsonConvert.SerializeObject(mainFolder, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            var fileName = $"{mainFolder.Name}_Export.json";

            return File(System.Text.Encoding.UTF8.GetBytes(serializedData), "application/json", fileName);
        }
    }
}