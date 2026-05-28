using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using _28_backend_netcore_06.Models.DBQuanLyNhanVien;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Models.Models;

namespace backend_netcore_06.DBQuanLyNhanVien
{
  [Route("api/[controller]")]
  [ApiController]
  public class QuanLyNhanVienController : Controller
  {
    private readonly DBQuanLyNhanVienContext _context;
    private readonly IMapper _mapper;
    public QuanLyNhanVienController(DBQuanLyNhanVienContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    [HttpGet("")]
    public async Task<ActionResult> GetAllNhanVien()
    {
      // uing linq query all nhan vien
      var nhanViens = await _context.NhanViens.Include(nv => nv.MaPbNavigation).Select(nv => new {
        MaNV = nv.Id,
        Ten = nv.Ten,
        MaPB = (nv.MaPbNavigation ?? new PhongBan()).TenPb
      }).ToListAsync();
      return Ok(nhanViens);
    }
  }
}