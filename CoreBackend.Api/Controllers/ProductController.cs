using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoreBackend.Api.Models;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;

namespace CoreBackend.Api.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        #region 数据库连接

        public Connection conn;
        public ProductController(IOptions<Connection> option)
        {
            conn = option.Value;
        }

        #endregion

        #region 获取所有的产品
        [HttpGet("GetProducts")]
        public IActionResult GetProducts()
        {
            ProductWebApiContext productWebApiContext = new ProductWebApiContext(conn.DefaultConnection);
            var products = (from p in productWebApiContext.Product select p).ToList();
            return Ok(products);
        }
        #endregion

        #region 通过Id get 产品
        [HttpGet("GetProductByid/{id}")]
        public IActionResult GetProductByid(int id)
        {
            ProductWebApiContext productWebApiContext = new ProductWebApiContext(conn.DefaultConnection);
            //List<Product> products = (from p in productWebApiContext.Product where p.Id==id select p).ToList();
            var products = (from p in productWebApiContext.Product where p.Id == id select p).ToList();
            if (products.Count == 0)
            {
                return NotFound();
            }
            return Ok(products);
        }
        #endregion

        #region 通过Id get 产品和产品下面的材料
        //[Route("{productid}/{materialid}", Name = "GetMaterial")]
        [HttpGet("GetProductAndMaterial/{productid}/{materialid}")]
        public IActionResult GetProductAndMaterial(int productid,int materialid)
        {
            ProductWebApiContext productWebApiContext = new ProductWebApiContext(conn.DefaultConnection);
            //List<Product> products = (from p in productWebApiContext.Product where p.Id==id select p).ToList();
            var products = (from p in productWebApiContext.Product
                            join m in productWebApiContext.Material on p.Id equals m.ProductId
                            where p.Id == productid && m.Id==materialid
                            select new { Product = p,Material=m }).ToList();
            //var materials = (from m in productWebApiContext.Material where m.ProductId == productid && m.Id == materialid select m);
            if (products.Count == 0)
            {
                return NotFound();
            }
            return Ok(products);
        }
        #endregion

        #region 废弃的get
        //[Route("{id}")]
        //public IActionResult GetProductsById(int id)
        //{
        //    var product = ProductServices.Current.Products.SingleOrDefault(x => x.Id == id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(product);
        //}
        #endregion

        #region 新增产品
        [HttpPost("InsertProductsById")]
        public IActionResult InsertProductsById([FromBody] Product product)
        {
            ProductWebApiContext productWebApiContext = new ProductWebApiContext(conn.DefaultConnection);

            if (product == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var maxId = (from p in productWebApiContext.Product select p.Id).Max();
            var newProduct = new Product
            {
                Id = ++maxId,
                Name = product.Name,
                Price = product.Price,
            };
            productWebApiContext.Add(newProduct);
            productWebApiContext.SaveChanges();
            return Ok(newProduct);
        }
        #endregion

        #region 根据产品id修改产品
        [HttpPut("ModifiedProductsById/{id}")]
        public IActionResult ModifiedProductsById(int id, [FromBody] Product product)
        {
            var products = new Product();
            ProductWebApiContext productWebApiContext = new ProductWebApiContext(conn.DefaultConnection);
            if (product == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != 0)
            {
                products = (from p in productWebApiContext.Product where p.Id == id select p).FirstOrDefault();
                products.Name = product.Name;
                products.Price = product.Price;
                productWebApiContext.SaveChanges();
            }
            return Ok(products);
        }
        #endregion

        #region 根据id删除产品
        [HttpDelete("DeleteProductsById/{id}")]
        public IActionResult DeleteProductsById(int id)
        {
            ProductWebApiContext productWebApiContext = new ProductWebApiContext(conn.DefaultConnection);
            var product = (from p in productWebApiContext.Product where p.Id == id select p).FirstOrDefault();
            productWebApiContext.Remove(product);
            productWebApiContext.SaveChanges();
            return Ok();
        }
        #endregion
    }
}
