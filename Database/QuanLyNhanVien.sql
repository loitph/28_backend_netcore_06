-- Drop tables in reverse dependency order (dependent tables first)
IF OBJECT_ID('NhamChuc', 'U') IS NOT NULL DROP TABLE NhamChuc;
IF OBJECT_ID('NhanVien', 'U') IS NOT NULL DROP TABLE NhanVien;
IF OBJECT_ID('PhongBan', 'U') IS NOT NULL DROP TABLE PhongBan;

-- Create tables in dependency order (referenced tables first)
CREATE TABLE PhongBan (
  Id INT PRIMARY KEY IDENTITY(1,1),
  TenPB NVARCHAR(255)
);

CREATE TABLE NhanVien (
  Id INT PRIMARY KEY IDENTITY(1,1),
  Ten NVARCHAR(255),
  NgaySinh DATE,
  DiaChi NVARCHAR(255),
  SoDienThoai NVARCHAR(20),
  MaPB INT,
  CONSTRAINT FK_NhanVien_PhongBan FOREIGN KEY (MaPB) REFERENCES PhongBan(Id)
);

CREATE TABLE NhamChuc (
  Id INT PRIMARY KEY IDENTITY(1,1),
  TenChucVu NVARCHAR(255),
  MaNV INT,
  FOREIGN KEY (MaNV) REFERENCES NhanVien(Id)
);

INSERT INTO PhongBan (TenPB) VALUES (N'Phòng Kinh Doanh');
INSERT INTO PhongBan (TenPB) VALUES (N'Phòng Nhân Sự');
INSERT INTO PhongBan (TenPB) VALUES (N'Phòng IT');

INSERT INTO NhanVien (Ten, NgaySinh, DiaChi, SoDienThoai, MaPB) VALUES (N'Nguyen Van A', '1990-01-01', N'123 Đường ABC', '0123456789', 1);
INSERT INTO NhanVien (Ten, NgaySinh, DiaChi, SoDienThoai, MaPB) VALUES (N'Tran Thi B', '1992-02-02', N'456 Đường DEF', '0987654321', 2);
INSERT INTO NhanVien (Ten, NgaySinh, DiaChi, SoDienThoai, MaPB) VALUES (N'Le Van C', '1985-03-03', N'789 Đường GHI', '0112233445', 3);
INSERT INTO NhanVien (Ten, NgaySinh, DiaChi, SoDienThoai, MaPB) VALUES (N'Nguyen Van D', '1990-01-01', N'123 Đường ABC', '0123456789', 1);
INSERT INTO NhanVien (Ten, NgaySinh, DiaChi, SoDienThoai, MaPB) VALUES (N'Tran Thi E', '1992-02-02', N'456 Đường DEF', '0987654321', 2);
INSERT INTO NhanVien (Ten, NgaySinh, DiaChi, SoDienThoai, MaPB) VALUES (N'Le Van F', '1985-03-03', N'789 Đường GHI', '0112233445', 3);
INSERT INTO NhanVien (Ten, NgaySinh, DiaChi, SoDienThoai, MaPB) VALUES (N'Nguyen Van G', '1990-01-01', N'123 Đường ABC', '0123456789', 1);
INSERT INTO NhanVien (Ten, NgaySinh, DiaChi, SoDienThoai, MaPB) VALUES (N'Tran Thi H', '1992-02-02', N'456 Đường DEF', '0987654321', 2);
INSERT INTO NhanVien (Ten, NgaySinh, DiaChi, SoDienThoai, MaPB) VALUES (N'Le Van I', '1985-03-03', N'789 Đường GHI', '0112233445', 3);
INSERT INTO NhanVien (Ten, NgaySinh, DiaChi, SoDienThoai, MaPB) VALUES (N'Nguyen Van J', '1990-01-01', N'123 Đường ABC', '0123456789', 1);



SELECT NhanVien.Id, NhanVien.Ten, PhongBan.TenPB
FROM NhanVien
LEFT JOIN PhongBan ON NhanVien.MaPB = PhongBan.Id;

SELECT * FROM NhanVien;

BEGIN TRANSACTION
  INSERT INTO NhanVien (Ten, NgaySinh, DiaChi, SoDienThoai, MaPB) VALUES (N'Nguyen Van K', '1990-01-01', N'123 Đường ABC', '0123456789', 1)

  SELECT *
  FROM NhanVien, PhongBan
  WHERE NhanVien.MaPB = PhongBan.Id

COMMIT TRANSACTION;

SELECT NV.Id as 'Mã Nhân Viên', NV.Ten as 'Tên Nhân Viên', NV.NgaySinh as 'Ngày Sinh', NV.DiaChi as 'Địa Chỉ', NV.SoDienThoai as 'Số Điện Thoại', PB.TenPB as 'Tên Phòng Ban', PB.Id as 'Mã Phòng Ban'
FROM NhanVien NV, PhongBan PB
WHERE NV.MaPB = PB.Id AND PB.TenPB
LIKE N'%Phòng IT%';

SELECT NV.Id as 'Mã Nhân Viên', NV.Ten as 'Tên Nhân Viên', NV.NgaySinh as 'Ngày Sinh', NV.DiaChi as 'Địa Chỉ', NV.SoDienThoai as 'Số Điện Thoại', PB.TenPB as 'Tên Phòng Ban', PB.Id as 'Mã Phòng Ban'
FROM NhanVien NV
INNER JOIN PhongBan PB
ON NV.MaPB = PB.Id
WHERE PB.TenPB
LIKE N'%Phòng IT%';

/*
  Them table DuAn
  voi cac truong Id, TenDuAn, MoTa, NgayBatDau, NgayKetThuc
*/
CREATE TABLE DuAn (
  Id INT PRIMARY KEY IDENTITY(1,1),
  TenDuAn NVARCHAR(255) NOT NULL,
  MoTa NVARCHAR(MAX),
  NgayBatDau DATE,
  NgayKetThuc DATE
);

/*
  Them table DiaDiem
  voi cac truong Id, TenDiaDiem, DiaChi
*/
CREATE TABLE DiaDiem (
  Id INT PRIMARY KEY IDENTITY(1,1),
  TenDiaDiem NVARCHAR(255) NOT NULL,
  DiaChi NVARCHAR(255)
);

-- Them du lieu vao table DuAn
INSERT INTO DuAn (TenDuAn, MoTa, NgayBatDau, NgayKetThuc) VALUES (N'Dự Án A', N'Mô tả dự án A', '2024-01-01', '2024-06-30');
INSERT INTO DuAn (TenDuAn, MoTa, NgayBatDau, NgayKetThuc) VALUES (N'Dự Án B', N'Mô tả dự án B', '2024-02-01', '2024-07-31');
INSERT INTO DuAn (TenDuAn, MoTa, NgayBatDau, NgayKetThuc) VALUES (N'Dự Án C', N'Mô tả dự án C', '2024-03-01', '2024-08-31');
INSERT INTO DuAn (TenDuAn, MoTa, NgayBatDau, NgayKetThuc) VALUES (N'Dự Án D', N'Mô tả dự án D', '2024-04-01', '2024-09-30');
INSERT INTO DuAn (TenDuAn, MoTa, NgayBatDau, NgayKetThuc) VALUES (N'Dự Án E', N'Mô tả dự án E', '2024-05-01', '2024-10-31');

-- Them du lieu vao table DiaDiem
INSERT INTO DiaDiem (TenDiaDiem, DiaChi) VALUES (N'Dia Diem A', N'123 Đường ABC');
INSERT INTO DiaDiem (TenDiaDiem, DiaChi) VALUES (N'Dia Diem B', N'456 Đường XYZ');
INSERT INTO DiaDiem (TenDiaDiem, DiaChi) VALUES (N'Dia Diem C', N'789 Đường PQR');
INSERT INTO DiaDiem (TenDiaDiem, DiaChi) VALUES (N'Dia Diem D', N'321 Đường LMN');
INSERT INTO DiaDiem (TenDiaDiem, DiaChi) VALUES (N'Dia Diem E', N'654 Đường OPQ');

-- Them 1 cot MaDiaDiem vao table DuAn
ALTER TABLE DuAn
ADD MaDiaDiem INT;

-- Them du lieu vao cot MaDiaDiem cua table DuAn
UPDATE DuAn
SET MaDiaDiem = 1
WHERE Id = 1;

-- Them ràng buộc khóa ngoại giữa MaDiaDiem của table DuAn và Id của table DiaDiem
ALTER TABLE DuAn
ADD CONSTRAINT FK_DuAn_DiaDiem
FOREIGN KEY (MaDiaDiem) REFERENCES DiaDiem(Id);

-- Tao table NhanVienDuAn vi nhan vien co the tham gia vao nhieu du an va du an co the co nhieu nhan vien
CREATE TABLE NhanVienDuAn (
  Id INT PRIMARY KEY IDENTITY(1,1),
  MaNV INT,
  MaDuAn INT,
  FOREIGN KEY (MaNV) REFERENCES NhanVien(Id),
  FOREIGN KEY (MaDuAn) REFERENCES DuAn(Id)
);

-- Them du lieu vao table NhanVienDuAn
INSERT INTO NhanVienDuAn (MaNV, MaDuAn) VALUES (1, 1);
INSERT INTO NhanVienDuAn (MaNV, MaDuAn) VALUES (1, 2);
INSERT INTO NhanVienDuAn (MaNV, MaDuAn) VALUES (2, 1);
INSERT INTO NhanVienDuAn (MaNV, MaDuAn) VALUES (2, 3);
INSERT INTO NhanVienDuAn (MaNV, MaDuAn) VALUES (3, 2);
INSERT INTO NhanVienDuAn (MaNV, MaDuAn) VALUES (3, 3);
INSERT INTO NhanVienDuAn (MaNV, MaDuAn) VALUES (4, 4);
INSERT INTO NhanVienDuAn (MaNV, MaDuAn) VALUES (4, 5);
INSERT INTO NhanVienDuAn (MaNV, MaDuAn) VALUES (5, 4);
INSERT INTO NhanVienDuAn (MaNV, MaDuAn) VALUES (5, 5);
INSERT INTO NhanVienDuAn (MaNV, MaDuAn) VALUES (6, 1);
INSERT INTO NhanVienDuAn (MaNV, MaDuAn) VALUES (6, 2);
INSERT INTO NhanVienDuAn (MaNV, MaDuAn) VALUES (7, 3);
INSERT INTO NhanVienDuAn (MaNV, MaDuAn) VALUES (7, 4);
INSERT INTO NhanVienDuAn (MaNV, MaDuAn) VALUES (8, 5);
INSERT INTO NhanVienDuAn (MaNV, MaDuAn) VALUES (8, 1);
INSERT INTO NhanVienDuAn (MaNV, MaDuAn) VALUES (9, 2);
INSERT INTO NhanVienDuAn (MaNV, MaDuAn) VALUES (9, 3);
INSERT INTO NhanVienDuAn (MaNV, MaDuAn) VALUES (10, 4);
INSERT INTO NhanVienDuAn (MaNV, MaDuAn) VALUES (10, 5);

-- Truy van de lay thong tin nhan vien va du an ma nhan vien do tham gia
SELECT NV.Ten AS 'Tên Nhân Viên', DA.TenDuAn AS 'Tên Dự Án'
FROM NhanVien NV
JOIN NhanVienDuAn NVDA ON NV.Id = NVDA.MaNV
JOIN DuAn DA ON NVDA.MaDuAn = DA.Id
ORDER BY NV.Ten, DA.TenDuAn;

-- Them table DiaDiemDuAn de luu tru thong tin dia diem cua du an
CREATE TABLE DiaDiemDuAn (
  Id INT PRIMARY KEY IDENTITY(1,1),
  MaDuAn INT,
  MaDiaDiem INT,
  FOREIGN KEY (MaDuAn) REFERENCES DuAn(Id),
  FOREIGN KEY (MaDiaDiem) REFERENCES DiaDiem(Id)
);

-- Them du lieu vao table DiaDiemDuAn
INSERT INTO DiaDiemDuAn (MaDuAn, MaDiaDiem) VALUES (1, 1);
INSERT INTO DiaDiemDuAn (MaDuAn, MaDiaDiem) VALUES (2, 2);
INSERT INTO DiaDiemDuAn (MaDuAn, MaDiaDiem) VALUES (3, 3);
INSERT INTO DiaDiemDuAn (MaDuAn, MaDiaDiem) VALUES (4, 4);
INSERT INTO DiaDiemDuAn (MaDuAn, MaDiaDiem) VALUES (5, 5);

-- Lay thong tin nhan vien cua du an tai dia diem "Dia Diem A"
SELECT NhanVien.Ten, DuAn.TenDuAn, DiaDiem.TenDiaDiem
FROM NhanVien
LEFT JOIN NhanVienDuAn ON NhanVien.Id = NhanVienDuAn.MaNV
LEFT JOIN DuAn ON NhanVienDuAn.MaDuAn = DuAn.Id
LEFT JOIN DiaDiemDuAn ON DuAn.Id = DiaDiemDuAn.MaDuAn
LEFT JOIN DiaDiem ON DiaDiemDuAn.MaDiaDiem = DiaDiem.Id
WHERE DiaDiem.TenDiaDiem = N'Dia Diem A';

SELECT NV.Ten, DA.TenDuAn, DD.TenDiaDiem
FROM NhanVien NV, NhanVienDuAn NVDA, DuAn DA, DiaDiemDuAn DDDA, DiaDiem DD, PhongBan PB
WHERE PB.Id = NV.MaPB AND NV.Id = NVDA.MaNV AND DA.Id = NVDA.MaDuAn AND DD.Id = DDDA.MaDiaDiem AND DA.Id = DDDA.MaDuAn AND DD.TenDiaDiem = N'Dia Diem A';

SELECT * FROM NhanVien
WHERE YEAR(NhanVien.NgaySinh) <= (
  SELECT Min(YEAR(NhanVien.NgaySinh)) FROM NhanVien
);

-- Cho biet moi du an co bao nhieu nhan vien
SELECT MaDuAn, COUNT(MaDuAn) as SoNhanVien,
(
  SELECT NV.Id as 'Ma Nhan Vien', Ten, NgaySinh, SoDienThoai
  FROM NhanVien NV, NhanVienDuAn NVDA
  WHERE NV.Id = NVDA.MaNV AND NVDA.MaNV = NhanVienDuAn.MaDuAn FOR JSON PATH
) as DanhSachNhanVien
FROM NhanVienDuAn
WHERE MaDuAn = 4
GROUP BY MaDuAn;


CREATE PROCEDURE SP_LayDanhSachNhanVienTheoDuAn (
  @MaDuAn int
) AS
BEGIN
  SELECT MaDuAn, COUNT(MaDuAn) as SoNhanVien,
  (
    SELECT NV.Id as 'Ma Nhan Vien', Ten, NgaySinh, SoDienThoai
    FROM NhanVien NV, NhanVienDuAn NVDA
    WHERE NV.Id = NVDA.MaNV AND NVDA.MaNV = NhanVienDuAn.MaDuAn FOR JSON PATH
  ) as DanhSachNhanVien
  FROM NhanVienDuAn
  WHERE MaDuAn = @MaDuAn
  GROUP BY MaDuAn;
END;

-- Goi thu tuc
EXEC SP_LayDanhSachNhanVienTheoDuAn @MaDuAn = 3;

-- alter procedure
ALTER PROCEDURE SP_LayDanhSachNhanVienTheoDuAn (
  @MaDuAn int
) AS
BEGIN
  SELECT MaDuAn, COUNT(MaDuAn) as SoNhanVien,
  (
    SELECT NV.Id as 'Ma Nhan Vien', Ten, NgaySinh, SoDienThoai
    FROM NhanVien NV, NhanVienDuAn NVDA
    WHERE NV.Id = NVDA.MaNV AND NVDA.MaNV = NhanVienDuAn.MaDuAn FOR JSON PATH
  ) as DanhSachNhanVien
  FROM NhanVienDuAn
  WHERE MaDuAn = @MaDuAn
  GROUP BY MaDuAn;
END

-- Su dung store procedure: Lay tat ca du an theo moi nhan vien
CREATE PROCEDURE SP_LayDanhSachDuAnTheoNhanVien (
  @MaNV int
) AS
BEGIN
  SELECT MaNV, COUNT(MaNV) as SoDuAn,
  (
    SELECT NV.Id as 'Ma Nhan Vien', Ten, NgaySinh, SoDienThoai
    FROM NhanVien NV, NhanVienDuAn NVDA
    WHERE NV.Id = NVDA.MaNV AND NVDA.MaNV = NhanVienDuAn.MaNV FOR JSON PATH
  ) as DanhSachDuAn
  FROM NhanVienDuAn
  WHERE MaNV = @MaNV
  GROUP BY MaNV;
END

