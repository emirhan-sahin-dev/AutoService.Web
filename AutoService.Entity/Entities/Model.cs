using AutoService.Entity.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.Entity.Entities
{
    public class Model : BaseEntity
    {
        public int ModelId { get; set; }
        public string ModelName { get; set; } = null!;
        public int BrandId { get; set; }
        public Brand Brand { get; set; } = null!;
        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    }
}
