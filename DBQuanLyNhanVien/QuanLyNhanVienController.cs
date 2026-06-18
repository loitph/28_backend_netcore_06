using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;
using _28_backend_netcore_06.Models.DBQuanLyNhanVien;
using AutoMapper;
using backend_netcore_06.Models.DTO;
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

    // <ChiTietDuAnDTO> Using store procedure SP_LayDanhSachNhanVienTheoDuAn
    // CREATE PROCEDURE SP_LayDanhSachNhanVienTheoDuAn (
    //   @MaDuAn int
    // ) AS
    // BEGIN
    //   SELECT MaDuAn, COUNT(MaDuAn) as SoNhanVien,
    //   (
    //     SELECT NV.Id as 'Ma Nhan Vien', Ten, NgaySinh, SoDienThoai
    //     FROM NhanVien NV, NhanVienDuAn NVDA
    //     WHERE NV.Id = NVDA.MaNV AND NVDA.MaNV = NhanVienDuAn.MaDuAn FOR JSON PATH
    //   ) as DanhSachNhanVien
    //   FROM NhanVienDuAn
    //   WHERE MaDuAn = @MaDuAn
    //   GROUP BY MaDuAn;
    // END;
    [HttpGet("LayDanhSachNhanVienTheoDuAn")]
    public async Task<ActionResult> LayDanhSachNhanVienTheoDuAn([FromQuery] string maDuAn)
    {
      var lstNhanVien = await _context.Database.SqlQueryRaw<ChiTietDuAnDTO>("exec SP_LayDanhSachNhanVienTheoDuAn @MaDuAn", new SqlParameter("@MaDuAn", maDuAn)).ToListAsync();
      foreach (var duAn in lstNhanVien)
      {
        duAn.ConvertJsonNhanVienDuAn();
      }
      return Ok(lstNhanVien);
    }

    // -- Su dung store procedure: Lay tat ca du an theo moi nhan vien
    // CREATE PROCEDURE SP_LayDanhSachDuAnTheoNhanVien (
    //   @MaNV int
    // ) AS
    // BEGIN
    //   SELECT MaNV, COUNT(MaNV) as SoDuAn,
    //   (
    //     SELECT NV.Id as 'Ma Nhan Vien', Ten, NgaySinh, SoDienThoai
    //     FROM NhanVien NV, NhanVienDuAn NVDA
    //     WHERE NV.Id = NVDA.MaNV AND NVDA.MaNV = NhanVienDuAn.MaNV FOR JSON PATH
    //   ) as DanhSachDuAn
    //   FROM NhanVienDuAn
    //   WHERE MaNV = @MaNV
    //   GROUP BY MaNV;
    // END;
    // [HttpGet("LayDanhSachDuAnTheoNhanVien")]
    // public async Task<ActionResult> LayDanhSachDuAnTheoNhanVien([FromQuery] int maNV)
    // {
    //   var lstDuAn = await _context.Database.SqlQueryRaw<ChiTietNhanVienDTO>("exec SP_LayDanhSachDuAnTheoNhanVien @MaNV", new SqlParameter("@MaNV", maNV)).ToListAsync();
    //   return Ok(lstDuAn);
    // }

    [HttpPost("ThemNhanVienNhanh")]
    public async Task<ActionResult> ThemNhanVienNhanh([FromBody] ThemNhanVienNhanhDTO model)
    {
      /*
        @TableName: NhanVien
        @DanhSachTenCot: TenNV, SoDienThoai, MaPB
        @DanhSachGiaTri: ['Nguyen Van AAA', '99999', 2]
      */


      // Lay table name
      string tableName = typeof(NhanVien).GetCustomAttribute<TableAttribute>()?.Name ?? typeof(NhanVien).Name;

      // Lay danh sach ten cot
      PropertyInfo[] properties = typeof(ThemNhanVienNhanhDTO).GetProperties();
      string danhSachTenCot = string.Join(", ", properties.Select(item => item.Name));

      // Lay gia tri dong
      string danhSachGiaTri = $"N'[{string.Join(", ", properties.Select(item => item.GetValue(model)))}]";

      SqlParameter paramTableName = new SqlParameter("@TableName",DbType.String) { Value = tableName };
      SqlParameter paramDanhSachTenCot = new SqlParameter("@DanhSachTenCot",DbType.String) { Value = danhSachTenCot };
      SqlParameter paramDanhSachGiaTri = new SqlParameter("@DanhSachGiaTri",DbType.String) { Value = danhSachGiaTri };
      
      var ketQua = await _context.Database.SqlQueryRaw<int>("EXEC InsertDynamicData_JSON @TableName, @DanhSachTenCot, @DanhSachGiaTri", paramTableName, paramDanhSachTenCot, paramDanhSachGiaTri).ToListAsync();

      var res = _context.NhanViens.ToListAsync();

      return Ok(res);
    }

    [HttpGet("TestErrorGetDanhSachNhanVien")]
    public async Task<ActionResult> TestErrorGetDanhSachNhanVien()
    {
      // trying to throw error sample
      throw new Exception("Error message");
    }
  }
}