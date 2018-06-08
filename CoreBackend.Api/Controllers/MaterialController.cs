using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreBackend.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/product")]
    public class MaterialController : Controller
    {
        //[HttpGet("{productId}/materials/{id}")]
        //public IActionResult GetProductsById(int productId, int id)
        //{
        //    var product = ProductServices.Current.Products.SingleOrDefault(x => x.Id == productId);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }
        //    var material = product.Materials.SingleOrDefault(y => y.Id == id);
        //    return Ok(material);
        //}
    }
}