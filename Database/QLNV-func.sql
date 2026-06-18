SELECT * FROM NhanVien;

SELECT Id, UPPER(Ten), CONVERT(VARCHAR(10), NgaySinh, 103)
FROM NhanVien
WHERE YEAR(NgaySinh) = 1990

CREATE FUNCTION CalcuteTax(@price DECIMAL(18, 2))
RETURNS DECIMAL(10, 2)
AS
BEGIN
  DECLARE @TienThue DECIMAL(18, 2)
  SET @TienThue = @price * 0.1
  RETURN @TienThue
END


SELECT *, 1000 as "Tiền Lương", dbo.CalcuteTax(1000) as "Tien thue"
FROM NhanVien

/*
Function: get info PhongBan
IdDuAn, TenDuAn, DanhSachNhanVien[Json path]
*/
CREATE FUNCTION LayThongTinNhanVienDuAn()
RETURNS @TblDuAnNhanVien TABLE (
    IdDuAn INT,
    TenDuAn NVARCHAR(50),
    DanhSachNhanVien NVARCHAR(MAX)
)
AS
BEGIN
    INSERT INTO @TblDuAnNhanVien
    SELECT
        DA.Id,
        DA.TenDuAn,
        (
            SELECT
                NV.Id AS MaNV,
                NV.Ten,
                NV.MaPB
            FROM NhanVienDuAn NVDA
            INNER JOIN NhanVien NV
                ON NV.Id = NVDA.MaNV
            WHERE NVDA.MaDuAn = DA.Id
            FOR JSON PATH
        ) AS DanhSachNhanVien
    FROM DuAn DA;

    RETURN;
END;


CREATE VIEW  View_LayThongTinNhanVienDuAn()
RETURNS @TblDuAnNhanVien TABLE (
    IdDuAn INT,
    TenDuAn NVARCHAR(50),
    DanhSachNhanVien NVARCHAR(MAX)
)
AS
BEGIN
    INSERT INTO @TblDuAnNhanVien
    SELECT
        DA.Id,
        DA.TenDuAn,
        (
            SELECT
                NV.Id AS MaNV,
                NV.Ten,
                NV.MaPB
            FROM NhanVienDuAn NVDA
            INNER JOIN NhanVien NV
                ON NV.Id = NVDA.MaNV
            WHERE NVDA.MaDuAn = DA.Id
            FOR JSON PATH
        ) AS DanhSachNhanVien
    FROM DuAn DA;

    RETURN;
END;
GO

/*
SQL tasks:
CREATE VIEW get all NhanVien info from NhanVienDuAn with MaDuAn input
*/
SELECT *
FROM NhanVien nv
LEFT JOIN NhanVienDuAn nvda
    ON nv.Id = nvda.MaNV
WHERE nvda.MaDuAn = 10;

CREATE VIEW VIEW_NhanVienDuAn
AS
SELECT
    nv.*,
    nvda.MaDuAn
FROM NhanVien nv
LEFT JOIN NhanVienDuAn nvda
    ON nv.Id = nvda.MaNV;

SELECT * FROM VIEW_NhanVienDuAn WHERE MaDuAn = 3;

-- CREATE or ALTER VIEW VIEW_DanhSachDuAnCuaNhanVien
-- AS
-- SELECT NV.Id, NV.Ten, NV.SoDienThoai, (
--   SELECT DA.Id, DA.TenDuAn, DD.TenDiaDiem
--   FROM DuAn DA, NhanVienDuAn NVDA, DiaDiem DD
--   WHERE DA.Id = NhanVienDuAn.MaDuAn
-- )