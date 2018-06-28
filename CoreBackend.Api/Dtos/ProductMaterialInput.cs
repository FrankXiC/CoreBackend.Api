using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBackend.Api.Dtos
{
    public class ProductMaterialInput
    {
        public ProductMaterialInput()
        {
            Materials = new List<MaterialInput>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public List<MaterialInput> Materials { get; set; }
       
    }
    public class MaterialInput
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
