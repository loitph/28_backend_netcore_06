using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
    private readonly IMapper _mapper;

    public ProductController(ProductStoreContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
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

    [HttpGet("SearchProductsByName")]
    public async Task<ActionResult> SearchProductsByName([FromQuery] string name = "")
    {      SqlParameter nameParam = new SqlParameter("@name", $"%{name}%");
      var products = await _context.Database.SqlQueryRaw<ProductDTO>("SELECT * FROM Products WHERE Name LIKE @name", nameParam).ToListAsync();

      if (products.Count() == 0)
      {
        return NotFound();
      }

      return Ok(products);
    }

    [HttpPost("CreateProduct")]
    public async Task<ActionResult> CreateProduct([FromBody] ProductDTO product)
    {
      // add product to database by using Linq
      // with auto-increase id
      // auto set CreatedAt, UpdatedAt to current date
      var newProduct = new Product
      {
        Name = product.Name,
        Price = product.Price,
        Description = "",
      };

      newProduct.CreatedAt = DateTime.Now;
      newProduct.UpdatedAt = DateTime.Now;
      // auto set Alias to name with StringToSlug
      newProduct.Alias = HelperFunction.StringToSlug(product.Name);

      newProduct.ImageUrl = "";
      newProduct.Deleted = false;

      _context.Products.Add(newProduct);
      await _context.SaveChangesAsync();


      // return the list of products after adding new product
      var products = await _context.Products.Select(p => new ProductDTO
      {
        Id = p.Id,
        Name = p.Name,
        Alias = p.Alias,
        Price = p.Price,
      }).ToListAsync();

      return Ok(products);
    }

    [HttpPost("CreateProductByMapper")]
    public async Task<ActionResult> CreateProductByMapper([FromBody] ProductDTO product)
    {

      // using Mapper to map list of products to list of ProductDTO
      Product newProduct = _mapper.Map<Product>(product);

      newProduct.Description = "";
      newProduct.ImageUrl = "";

      _context.Products.Add(newProduct);
      await _context.SaveChangesAsync();

      // return the list of products after adding new product
      var products = await _context.Products.Select(p => new ProductDTO
      {
        Id = p.Id,
        Name = p.Name,
        Alias = p.Alias,
        Price = p.Price,
      }).ToListAsync();

      return Ok(products);
    }

    [HttpPut("UpdateProductByMapper")]
    public async Task<ActionResult> UpdateProductByMapper([FromBody] ProductUpdateDTO product)
    {
      // before update, we need to check if the product exists in database by id
      var existingProduct = await _context.Products.FindAsync(product.Id);
      if (existingProduct == null)
      {
        return NotFound($"Product with id {product.Id} not found.");
      }

      // using Mapper to map list of products to list of ProductDTO
      Product updatedProduct = _mapper.Map<Product>(product);

      _context.Products.Update(updatedProduct);
      await _context.SaveChangesAsync();

      // return the list of products after adding new product
      var products = await _context.Products.Select(p => new ProductDTO
      {
        Id = p.Id,
        Name = p.Name,
        Alias = p.Alias,
        Price = p.Price,
      }).ToListAsync();


      // return 200 status code with message and data
      return Ok(new
      {
        Message = $"Product with id {product.Id} updated successfully.",
        Data = products
      });
    }
  }
}