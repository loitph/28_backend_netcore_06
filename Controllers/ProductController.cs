using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
// using backend_netcore_06.Models;

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

    static ProductController()
    {
      foreach (var item in lstProductDTO)
      {
        item.Alias = HelperFunction.StringToSlug(item.Name ?? "");
      }
    }

    public ProductController()
    {
    }

    [HttpGet("GetAll")]
    public List<string> GetAll()
    {
      return lstProduct;
    }

    [HttpGet("GetAllDTO")]
    public async Task<IActionResult> GetAllDTO()
    {
      var response = new ResponseTypeDTO<List<ProductDTO>>()
      {
        StatusCode = 200,
        Content = lstProductDTO,
        Message = "Success",
        DateTime = DateTime.Now
      };
      return StatusCode(StatusCodes.Status200OK, response);
    }

    [HttpGet("GetById/{productId}")]
    public async Task<IActionResult> GetById([FromRoute] string productId)
    {
      var product = await Task.FromResult(lstProductDTO.FirstOrDefault(p => p.Id == int.Parse(productId)));

      var response = new ResponseTypeDTO<ProductDTO>()
      {
        StatusCode = 200,
        Content = product,
        Message = "Success",
        DateTime = DateTime.Now
      };

      if (product == null)
      {
        response = new ResponseTypeDTO<ProductDTO>()
        {
          StatusCode = 400,
          Content = product,
          Message = "Success",
          DateTime = DateTime.Now
        };

        return StatusCode(StatusCodes.Status400BadRequest, response);
      }

      response = new ResponseTypeDTO<ProductDTO>()
      {
        StatusCode = 200,
        Content = product,
        Message = "Success",
        DateTime = DateTime.Now
      };
      return StatusCode(StatusCodes.Status200OK, response);
    }

    [HttpPost("AddProduct")]
    public async Task<IActionResult> AddProduct([FromBody]ProductDTO product)
    {
      product.Alias = HelperFunction.StringToSlug(product.Name ?? "");

      var isExist = await Task.FromResult(lstProductDTO.FirstOrDefault(p => p.Name == product.Name || p.Id == product.Id));
      if (isExist != null)
      {
        return StatusCode(StatusCodes.Status400BadRequest, "Product already exist");
      }

      lstProduct.Add(product.Name);
      lstProductDTO.Add(product);

      var response = new ResponseTypeDTO<List<ProductDTO>>()
      {
        StatusCode = 200,
        Content = lstProductDTO,
        Message = "Success",
        DateTime = DateTime.Now
      };

      return StatusCode(StatusCodes.Status200OK, response);
    }

    [HttpDelete("DeleteProduct/{productId}")]
    public async Task<IActionResult> DeleteProduct([FromRoute]string productId)
    {
      var item = lstProductDTO.FirstOrDefault(p => p.Id == int.Parse(productId));

      var response = new ResponseTypeDTO<List<ProductDTO>>()
      {
        StatusCode = 200,
        Content = lstProductDTO,
        Message = "Success",
        DateTime = DateTime.Now
      };

      if (item != null)
      {
        lstProductDTO.Remove(item);
        lstProduct.Remove(item.Name);

        response = new ResponseTypeDTO<List<ProductDTO>>()
        {
          StatusCode = 200,
          Content = lstProductDTO,
          Message = "Success",
          DateTime = DateTime.Now
        };
        return StatusCode(StatusCodes.Status200OK, response);
      }
      return StatusCode(StatusCodes.Status400BadRequest, "Product not found to delete");
    }

    [HttpGet("SearchProduct")]
    public async Task<IActionResult> SearchProduct([FromQuery]string keyword)
    {
      string productName = HelperFunction.StringToSlug(keyword ?? "");
      var response = new ResponseTypeDTO<List<ProductDTO>>()
      {
        StatusCode = 200,
        Content = lstProductDTO,
        Message = "Success",
        DateTime = DateTime.Now
      };

      if (string.IsNullOrEmpty(productName))
      {
        return StatusCode(StatusCodes.Status400BadRequest, "Product name is required");
      }

      response = new ResponseTypeDTO<List<ProductDTO>>()
      {
        StatusCode = 200,
        Content = lstProductDTO.Where(p => p.Alias.Contains(productName)).ToList(),
        Message = "Success",
        DateTime = DateTime.Now
      };

      return StatusCode(StatusCodes.Status200OK, response);
      // return lstProductDTO.Where(p => p.Name.Contains(productName)).ToList();
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