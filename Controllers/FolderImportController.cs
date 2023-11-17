using FolderControler.Data;
using FolderControler.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FolderControler.Controllers {
    public class FolderImportController : Controller {


        private readonly ApplicationDbContext _db;
        public FolderImportController(ApplicationDbContext db) {
            _db = db;
        }
        public IActionResult Index() {
            return View();
        }
        public class FolderPathDto {
            public string Name { get; set; }
            public string Path { get; set; }
        }

        [HttpPost]
        public IActionResult ImportFromDrop([FromBody] List<FolderPathDto> folderPaths) {
            try
            {
                foreach (var folderPath in folderPaths)
                {
                    var pathComponents = folderPath.Path.Split('/');
                    int? parentId = null;

                    foreach (var componentName in pathComponents)
                    {
                        var existingFolder = _db.Folders.FirstOrDefault(f => f.Name == componentName && f.ParentId == parentId);

                        if (existingFolder == null)
                        {
                            var newFolder = new Folder
                            {
                                Name = componentName,
                                ParentId = parentId
                            };

                            _db.Folders.Add(newFolder);
                            _db.SaveChanges();

                            parentId = newFolder.Id;
                        }
                        else
                        {
                            parentId = existingFolder.Id;
                        }
                    }
                }

                return Ok(new { Message = "Folders imported successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error during folder import.", Error = ex.Message });
            }
          
        }
    }
}