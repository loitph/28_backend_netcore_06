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
      // NOTE: ONLY NEED new variable newProduct when we want to add more properties to the new product, if we don't need to add more properties, we can directly map to the existing product variable
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

      // without AsNoTracking(), the existingProduct will be tracked by EF Core, and when we update it, it will cause an error because we are trying to update a tracked entity with another tracked entity (the product parameter)

      // before update, we need to check if the product exists in database by id
      var existingProduct = await _context.Products.SingleOrDefaultAsync(p => p.Id == product.Id);
      if (existingProduct == null)
      {
        return NotFound($"Product with id {product.Id} not found.");
      }

      // using Mapper to map list of products to list of ProductDTO
      _mapper.Map(product, existingProduct);

      // note: we don't need to call SaveChangesAsync() here because we are using the tracked entity existingProduct, only Update() function could update the entity in database
       await _context.SaveChangesAsync();
      _context.Products.Update(existingProduct);

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

    [HttpDelete("DeleteProductById")]
    public async Task<ActionResult> DeleteProductById([FromQuery] int id)
    {
      var existingProduct = await _context.Products.SingleOrDefaultAsync(p => p.Id == id);
      if (existingProduct == null)      {
        return NotFound($"Product with id {id} not found.");
      }

      _context.Products.Remove(existingProduct);
      await _context.SaveChangesAsync();

      return Ok($"Product with id {id} deleted successfully.");
    }

    [HttpPost("UploadFile")]
    public async Task<ActionResult> UploadFile(IFormFile file)
    {
      if (file == null || file.Length == 0)
      {
        return BadRequest("No file uploaded.");
      }

      var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", file.FileName);
      using (var stream = new FileStream(filePath, FileMode.Create))
      {
        await file.CopyToAsync(stream);
      }

      return Ok($"File {file.FileName} uploaded successfully.");
    }
  }
}