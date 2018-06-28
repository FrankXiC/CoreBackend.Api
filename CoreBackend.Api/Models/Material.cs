using System;
using System.Collections.Generic;

namespace CoreBackend.Api.Models
{
    public partial class Material
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ProductId { get; set; }

    }
}
