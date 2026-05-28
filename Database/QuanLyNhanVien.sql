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