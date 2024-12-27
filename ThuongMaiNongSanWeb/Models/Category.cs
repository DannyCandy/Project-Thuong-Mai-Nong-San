using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public partial class Category
    {
        public Category()
        {
            SanPhams = new HashSet<SanPham>();
        }

        public string Idcategory { get; set; } = null!;
        public string NameCategory { get; set; } = null!;
        public bool IsDeleted { get; set; } = false;
        public virtual ICollection<SanPham> SanPhams { get; set; }
    }
}
