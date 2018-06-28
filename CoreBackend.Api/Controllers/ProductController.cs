using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreBackend.Api.Dtos;
using CoreBackend.Api.Helper;
using Microsoft.AspNetCore.Mvc;
using CoreBackend.Api.Models;
using CoreBackend.Api.ResourceParameters;
using log4net;
using Microsoft.AspNetCore.Mvc.Routing;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;
using CoreBackend.Api.Services;

namespace CoreBackend.Api.Controllers {
    [Route("api/[controller]")]
    public class ProductController : Controller {
        private static readonly ILog log = LogManager.GetLogger(SDKProperties.LogRepository.Name, typeof(ProductController));
        private readonly IPropertyMappingContainer _propertyMappingContainer;
        private IUrlHelper _urlHelper;
        #region 数据库连接
        public Connection conn;
        public ProductController(IOptions<Connection> option,IUrlHelper urlHelper,IPropertyMappingContainer propertyMappingContainer) {
            conn = option.Value;
            _urlHelper = urlHelper;
            _propertyMappingContainer = propertyMappingContainer;
        }

        #endregion

        #region 获取所有的产品
        [HttpGet(Name = "GetProducts")]
        public async Task<IActionResult>GetProducts(ResourceParameter parameter) {
            log.Info("获取所有的产品list");
            List<Product> products = new List<Product>();
            var query = products.AsQueryable();
            int count = 0;
            try {
                if (!_propertyMappingContainer.ValidMappingExistsFor<ResourceParameter,Product>(parameter.OrderBy))
                {
                    return BadRequest("找不到要排序的字段");
                }
                ProductWebApiContext productWebApiContext = new ProductWebApiContext(conn.DefaultConnection);
                var propertiesMap=new Dictionary<string,Expression<Func<Product,object>>>
                {
                    {"Id",p=>p.Id },
                    {"Name",p=>p.Name },
                    {"Price",p=>p.Price }
                };
                if (!string.IsNullOrEmpty(parameter.Name))
                {
                    query = (from p in productWebApiContext.Product where p.Name.Contains(parameter.Name) select p)
                        .AsQueryable();
                    count = await (from p in productWebApiContext.Product where p.Name.Contains(parameter.Name) select p).CountAsync();
                }
                else
                {
                    query = (from p in productWebApiContext.Product select p)
                       .AsQueryable();
                    count = await (from p in productWebApiContext.Product select p).CountAsync();
                }

                query = query.ApplySort(parameter.OrderBy, _propertyMappingContainer.Resolve<ResourceParameter,Product>());
                products = query.Skip(parameter.PageIndex*parameter.PageSize).Take(parameter.PageSize).ToList();
                var returnlist = new PaginatedList<Product>(parameter.PageIndex, parameter.PageSize, count, products);
                var preLink = returnlist.HasPrevious
                    ? new CreateProductResourceUrl(_urlHelper).CreateResouceUrl(parameter, PaginationResourceUriType.PreviousPage,
                        "GetProducts"):null;
                var nextLink = returnlist.HasNext
                    ? new CreateProductResourceUrl(_urlHelper).CreateResouceUrl(parameter, PaginationResourceUriType.NextPage,
                        "GetProducts") : null;
                var mata = new
                {
                    returnlist.TotalItemsCount,
                    returnlist.PaginationBase.PageSize,
                    returnlist.PaginationBase.PageIndex,
                    returnlist.PageCount,
                    preLink,
                    nextLink
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(mata));
            }
            catch (Exception e) {
                log.Error(e);
            }
           
            return Ok(products);
        }
        #endregion

        #region 通过Name获取所有的产品
        //[HttpGet("~/api/name")]
        //public async Task<IActionResult> GetProductsSelectByName(ResourceParameter parameter) {
        //    log.Info("通过name查询产品,name=" + parameter.Name);
        //    ProductWebApiContext productWebApiContext = new ProductWebApiContext(conn.DefaultConnection);
        //    var propertiesMap = new Dictionary<string, Expression<Func<Product, object>>>
        //    {
        //        {"Id",p=>p.Id },
        //        {"Name",p=>p.Name },
        //        {"Price",p=>p.Price }
        //    };
        //    List<Product> products = new List<Product>();
        //    var query = products.AsQueryable();
        //    int count = 0;
        //    try {
        //        if (string.IsNullOrEmpty(parameter.Name)) {
        //            query = (from p in productWebApiContext.Product select p).AsQueryable();
        //            count = await(from p in productWebApiContext.Product select p).CountAsync();
        //        }
        //        else {
        //            query = (from p in productWebApiContext.Product where p.Name.Contains(parameter.Name) select p).AsQueryable();
        //            count = await (from p in productWebApiContext.Product where p.Name.Contains(parameter.Name) select p).CountAsync();
        //        }

        //        if (string.IsNullOrEmpty(parameter.OrderBy))
        //        {
        //            if (parameter.OrderBy.EndsWith(" desc")) {
        //                var property = parameter.OrderBy.Split(" ")[0];
        //                products = await query.OrderByDescending(propertiesMap[property])
        //                    .Skip(parameter.PageSize * parameter.PageIndex).Take(parameter.PageSize).ToListAsync();
        //            }
        //            else {
        //                products = await query.OrderBy(propertiesMap[parameter.OrderBy])
        //                    .Skip(parameter.PageSize * parameter.PageIndex).Take(parameter.PageSize).ToListAsync();
        //            }
        //        }
        //        var returnlist = new PaginatedList<Product>(parameter.PageIndex, parameter.PageSize, count, products);
        //        var preLink = returnlist.HasPrevious
        //            ? new CreateProductResourceUrl(_urlHelper).CreateResouceUrl(parameter, PaginationResourceUriType.PreviousPage,
        //                "GetProducts") : null;
        //        var nextLink = returnlist.HasNext
        //            ? new CreateProductResourceUrl(_urlHelper).CreateResouceUrl(parameter, PaginationResourceUriType.NextPage,
        //                "GetProducts") : null;
        //        var mata = new {
        //            returnlist.TotalItemsCount,
        //            returnlist.PaginationBase.PageSize,
        //            returnlist.PaginationBase.PageIndex,
        //            returnlist.PageCount,
        //            preLink,
        //            nextLink
        //        };
        //        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(mata));
        //    }

        //    catch (Exception e) {
        //        log.Error(e);
        //    }

        //    return Ok(products);
        //}
        #endregion

        #region 通过Id get 产品
        [HttpGet("{id}")]
        public IActionResult GetProductByid(int id) {
            ProductWebApiContext productWebApiContext = new ProductWebApiContext(conn.DefaultConnection);
            //List<Product> products = (from p in productWebApiContext.Product where p.Id==id select p).ToList();
            var products = (from p in productWebApiContext.Product where p.Id == id select p).ToList();
            if (products.Count == 0) {
                return NotFound();
            }
            return Ok(products);
        }
        #endregion

        #region 通过Id get 产品和产品下面的材料
        //[Route("{productid}/{materialid}", Name = "GetMaterial")]
        [HttpGet("{productid}/Material")]
        public IActionResult GetProductAndMaterial(int productid, int materialid) {
            ProductWebApiContext productWebApiContext = new ProductWebApiContext(conn.DefaultConnection);
            //List<Product> products = (from p in productWebApiContext.Product where p.Id==id select p).ToList();
            var products = (from p in productWebApiContext.Product
                            join m in productWebApiContext.Material on p.Id equals m.ProductId
                            where p.Id == productid
                            select new { Product = p, Material = m }).ToList();
            //var materials = (from m in productWebApiContext.Material where m.ProductId == productid && m.Id == materialid select m);
            if (products.Count == 0) {
                return NotFound();
            }
            return Ok(products);
        }
        #endregion

        #region 新增产品
        [HttpPost]
        public IActionResult InsertProductsById([FromBody] Product product) {
            ProductWebApiContext productWebApiContext = new ProductWebApiContext(conn.DefaultConnection);

            if (product == null) {
                return BadRequest();
            }
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            var maxId = (from p in productWebApiContext.Product select p.Id).Max();
            var newProduct = new Product {
                Id = ++maxId,
                Name = product.Name,
                Price = product.Price,
            };
            productWebApiContext.Add(newProduct);
            productWebApiContext.SaveChanges();
            return Ok(newProduct);
        }
        #endregion

        #region 批量新增产品
        [HttpPost("~/api/productcollections")]
        public IActionResult InsertProductsCollection([FromBody] IEnumerable<Product> products) {
            ProductWebApiContext productWebApiContext = new ProductWebApiContext(conn.DefaultConnection);

            if (products == null) {
                return BadRequest();
            }
            foreach (var product in products) {
                var maxId = (from p in productWebApiContext.Product select p.Id).Max();
                var newProduct = new Product {
                    Id = ++maxId,
                    Name = product.Name,
                    Price = product.Price,
                };
                productWebApiContext.Add(newProduct);
                productWebApiContext.SaveChanges();
            }

            return Ok();
        }
        #endregion


        #region 新增产品和物料
        [HttpPost("material")]
        public IActionResult InsertProductsAndMaterials([FromBody] ProductMaterialInput input) {
            ProductWebApiContext productWebApiContext = new ProductWebApiContext(conn.DefaultConnection);

            if (input == null) {
                return BadRequest();
            }
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            var maxId = (from p in productWebApiContext.Product select p.Id).Max();
            var newProduct = new Product {
                Id = ++maxId,
                Name = input.Name,
                Price = input.Price,
            };
            var maxMaterialId = (from m in productWebApiContext.Material select m.Id).Max();
            var newMaterial = new Material {
                Id = ++maxMaterialId,
                Name = input.Materials.First().Name,
                ProductId = maxId
            };
            productWebApiContext.Add(newProduct);
            productWebApiContext.Add(newMaterial);
            productWebApiContext.SaveChanges();
            return Ok(newProduct);
        }
        #endregion


        #region 根据产品id修改产品
        [HttpPut("{id}")]
        public IActionResult ModifiedProductsById(int id, [FromBody] Product product) {
            var products = new Product();
            ProductWebApiContext productWebApiContext = new ProductWebApiContext(conn.DefaultConnection);
            if (product == null) {
                return BadRequest();
            }
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            if (id != 0) {
                products = (from p in productWebApiContext.Product where p.Id == id select p).FirstOrDefault();
                if (products != null)
                {
                    products.Name = product.Name;
                    products.Price = product.Price;
                }

                productWebApiContext.SaveChanges();
            }
            return Ok(products);
        }
        #endregion

        #region 根据id删除产品的物料
        [HttpDelete("{productid}/material/{materialid}")]
        public IActionResult DeleteMaterialById(int productid, int materialid) {
            ProductWebApiContext productWebApiContext = new ProductWebApiContext(conn.DefaultConnection);
            var product = (from p in productWebApiContext.Product where p.Id == productid select p).FirstOrDefault();
            if (product == null) {
                return NotFound();
            }
            var material = (from m in productWebApiContext.Material where m.Id == materialid select m).FirstOrDefault();
            if (material == null) {
                return NotFound();
            }
            productWebApiContext.Remove(material);
            productWebApiContext.SaveChanges();
            return Ok();
        }
        #endregion

        #region 根据id删除产品以及物料
        [HttpDelete("{productid}/material")]
        public async Task<IActionResult> DeleteProductsById(int productid) {
            ProductWebApiContext productWebApiContext = new ProductWebApiContext(conn.DefaultConnection);
            var product = await (from p in productWebApiContext.Product where p.Id == productid select p).ToListAsync();
            if (product == null) {
                return NotFound();
            }
            productWebApiContext.Remove(product.FirstOrDefault());
            if (await productWebApiContext.SaveChangesAsync()<=0)
            {
                return StatusCode(500, $"删除产品{product.First().Id}失败");
            }
            return NoContent();
        }
        #endregion
    }
}
