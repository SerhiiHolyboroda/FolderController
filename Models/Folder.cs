using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolderControler.Models {
    public class Folder {
        [Key]
       
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }

        public Folder? Parent { get; set; }
        public List<Folder>? Children { get; set; }
    }
}

