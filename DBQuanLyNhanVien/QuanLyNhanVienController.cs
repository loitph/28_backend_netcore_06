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

    [HttpGet("LayThongTinNhanVienPhongBan")]
    public async Task<ActionResult> LayThongTinNhanVienPhongBan()
    {
      // get all nhan vien voi phong ban co id = 3, dung lenh Include
      // var nhanViens = await _context.NhanViens.Include(nv => nv.MaPbNavigation).Where(nv => nv.MaPb == 3).Select(nv => new {
      //   MaNV = nv.Id,
      //   Ten = nv.Ten,
      //   MaPB = (nv.MaPbNavigation ?? new PhongBan()).TenPb
      // }).ToListAsync();
      // return Ok(nhanViens);



      // get all nhan vien voi phong ban IT, dung lenh context Join
      var lstNhanVien = await _context.NhanViens.Join(_context.PhongBans, nv => nv.MaPb, pb => pb.Id, (nv, pb) => new {
        MaNV = nv.Id,
        Ten = nv.Ten,
        TenPB = (nv.MaPbNavigation ?? new PhongBan()).TenPb,
        MaPB = nv.MaPb
      }).Where(nv => nv.MaPB == 3).ToListAsync();
      return Ok(lstNhanVien);
    }

    [HttpGet("LayThongTinNhanVienPhongBanA")]
    public async Task<ActionResult> LayThongTinNhanVienPhongBanA([FromQuery] string tenDuAn)
    {
      var lstNhanVien = await _context.NhanVienDuAns.Select(item => new
      {
        MaNV = item.MaNv,
        MaDuAn = item.MaDuAn,
        TenDuAn = item.MaDuAnNavigation.TenDuAn,
        TenNhanVien = item.MaNvNavigation.Ten,
        DiaDiem = item.MaDuAnNavigation.MaDiaDiemNavigation.TenDiaDiem,
        TenPB = item.MaNvNavigation.MaPbNavigation.TenPb
      }).Where(item => item.TenDuAn.Contains(tenDuAn)).ToListAsync();
      return Ok(lstNhanVien);
    }
  }
}