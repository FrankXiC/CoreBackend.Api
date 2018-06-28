using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;

namespace CoreBackend.Api.Models
{
    public partial class Product :IEntity
    {
        public int Id { get; set; }

        [FieldHasWhiteSpace][Required][MaxLength(10,ErrorMessage="{0}的长度不能超过{1}")]
        public string Name { get; set; }

        [Range(10,100000000)]
        public double? Price { get; set; }

        public ICollection<Material> Materials { get; set; }

        public bool IsTransient() {
            throw new NotImplementedException();
        }
    }
}
