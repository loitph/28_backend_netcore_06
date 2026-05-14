using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
//using backend_netcore_06.Models;

namespace backend_netcore_06.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ProductController : ControllerBase
  {
    public static List<string> lstProduct = new List<string>() {"Product 1", "Product 2", "Product 3"};

    public static List<ProductDTO> lstProductDTO = new List<ProductDTO>() {
      new ProductDTO() {Id = 1, Name = "Product 1", Price = 100.5m},
      new ProductDTO() {Id = 2, Name = "Product 2", Price = 200m},
      new ProductDTO() {Id = 3, Name = "Product 3", Price = 300.00300m}
    };

    public ProductController()
    {
    }

    [HttpGet("GetAll")]
    public List<string> GetAll()
    {
      return lstProduct;
    }

    [HttpGet("GetAllDTO")]
    public List<ProductDTO> GetAllDTO()
    {
      return lstProductDTO;
    }

    [HttpGet("GetById/{productId}")]
    public async Task<IActionResult> GetById([FromRoute] string productId)
    {
      // return lstProductDTO.FirstOrDefault(p => p.Id == int.Parse(productId));

      var product = await Task.FromResult(lstProductDTO.FirstOrDefault(p => p.Id == int.Parse(productId)));
      if (product == null)
      {
        // return NotFound();

        return BadRequest("Product not found: " + productId); // 400
      }

      return Ok(lstProductDTO.FirstOrDefault(p => p.Id == int.Parse(productId)));
    }

    [HttpPost("AddProduct")]
    public void AddProduct([FromBody]ProductDTO product)
    {
      lstProduct.Add(product.Name);
      lstProductDTO.Add(product);
    }

    [HttpDelete("DeleteProduct/{productId}")]
    public List<ProductDTO> DeleteProduct([FromRoute]string productId)
    {
      var item = lstProductDTO.FirstOrDefault(p => p.Id == int.Parse(productId));

      if (item != null)
      {
        lstProductDTO.Remove(item);
        lstProduct.Remove(item.Name);
      }

      return lstProductDTO;
    }

    [HttpGet("SearchProduct")]
    public List<ProductDTO> SearchProduct([FromQuery]string productName)
    {
      return lstProductDTO.Where(p => p.Name.Contains(productName)).ToList();
    }

    [HttpPut("UpdateProduct")]
    public List<ProductDTO> UpdateProduct([FromBody]ProductDTO product)
    {
      var item = lstProductDTO.FirstOrDefault(p => p.Id == product.Id);
      if (item != null)
      {
        item.Name = product.Name;
        item.Price = product.Price;
      }

      return lstProductDTO;
    }

    // Patch product discount 10%
    [HttpPatch("PatchProduct")]
    public List<ProductDTO> PatchProduct([FromBody]ProductDTO product)
    {
      var item = lstProductDTO.FirstOrDefault(p => p.Id == product.Id);
      if (item != null)
      {
        item.Price = item.Price - item.Price * 0.1m;
      }

      return lstProductDTO;
    }
  }
}