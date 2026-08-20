using AutoService.Entity.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.Entity.Entities
{
    public class Brand : BaseEntity
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; } = null!;
        public ICollection<Model> Models { get; set; } = new List<Model>();
    }
}
