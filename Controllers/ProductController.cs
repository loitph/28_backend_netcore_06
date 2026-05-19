using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Models.Models;

namespace backend_netcore_06.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ProductController : ControllerBase
  {
    private readonly ProductStoreContext _context;

    public ProductController(ProductStoreContext context)
    {
      _context = context;
    }

    [HttpGet("GetAllProducts")]
    public async Task<ActionResult> GetAllProducts()
    {
      // Using FromSqlRaw to execute raw SQL query and map results to ProductDTO
      // var products = await _context.Products.FromSqlRaw("SELECT * FROM Products").ToListAsync();

      // If you want to map to a DTO instead of the entity, you can use Database.SqlQueryRaw
      var productBaseOnDTO = await _context.Database.SqlQueryRaw<ProductDTO>("SELECT * FROM Products").ToListAsync();

      return Ok(productBaseOnDTO);

      // return Ok(products);
    }

    [HttpGet("GetProductsByLinq")]
    public async Task<ActionResult> GetProductsByLinq([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10 )
    {
      // Using FromSqlRaw to execute raw SQL query and map results to ProductDTO
      // similar to SELECT * FROM Products OFFSET 0 ROWS FETCH NEXT 10 ROWS ONLY
      var products = await _context.Products.Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(p => new ProductDTO
      {
        Id = p.Id,
        Name = p.Name,
        Alias = p.Alias,
        Price = p.Price,
      }).ToListAsync();

      return Ok(products);
    }

    [HttpGet("GetProductById")]
    public async Task<ActionResult> GetProductById([FromQuery] int id = 101)
    {
      SqlParameter idParam = new SqlParameter("@id", id);
      var product = await _context.Database.SqlQueryRaw<ProductDTO>("SELECT * FROM Products WHERE Id = @id", idParam).ToListAsync();

      return Ok(product);
    }

    [HttpGet("GetProductByIdLinq")]
    public async Task<ActionResult> GetProductByIdLinq([FromQuery] int id = 101)
    {
      var product = await _context.Products.Where(p => p.Id == id).Select(p => new ProductDTO
      {
        Id = p.Id,
        Name = p.Name,
        Alias = p.Alias,
        Price = p.Price,
      }).ToListAsync();

      if (product.Count() == 0)
      {
        return NotFound();
      }

      return Ok(product);
    }
  }
}