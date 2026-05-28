CREATE DATABASE QuanLyBanHang;
USE QuanLyBanHang;

CREATE TABLE KhachHang (
  Id INT PRIMARY KEY IDENTITY(1,1),
  TenKH NVARCHAR(255) NOT NULL,
  Email NVARCHAR(255),
  SoDienThoai NVARCHAR(20),
  DiaChi NVARCHAR(255)
);

CREATE TABLE DonHang (
  Id INT PRIMARY KEY IDENTITY(1,1),
  NgayDat DATETIME,
  TongTien DECIMAL(18, 2),
  MaKhachHang INT
);

CREATE TABLE SanPham (
  Id INT PRIMARY KEY IDENTITY(1,1),
  TenSP NVARCHAR(255) NOT NULL,
  Gia DECIMAL(18, 2),
  SoLuong INT,
  HinhAnh NVARCHAR(255)
);

CREATE TABLE ChiTietDonHang (
  IdDonHang INT,
  IdSanPham INT,
  SoLuong INT,
  Gia DECIMAL(18, 2),
);

ALTER TABLE DonHang
ADD CONSTRAINT FK_DonHang_KhachHang
FOREIGN KEY (MaKhachHang)
REFERENCES KhachHang(Id);

ALTER TABLE ChiTietDonHang
ADD CONSTRAINT FK_ChiTietDonHang_DonHang
FOREIGN KEY (IdDonHang)
REFERENCES DonHang(Id);

ALTER TABLE ChiTietDonHang
ADD CONSTRAINT FK_ChiTietDonHang_SanPham
FOREIGN KEY (IdSanPham)
REFERENCES SanPham(Id);

-- Thêm 10 dong dữ liệu mẫu
INSERT INTO KhachHang (TenKH, Email, SoDienThoai, DiaChi) VALUES
('Nguyen Van A', 'L4IbM@example.com', '1234567890', '123 Main St, Anytown, USA'),
('Le Thi B', 'L4IbM@example.com', '1234567890', '123 Main St, Anytown, USA'),
('Tran Van C', 'L4IbM@example.com', '1234567890', '123 Main St, Anytown, USA'),
('Pham Thi D', 'L4IbM@example.com', '1234567890', '123 Main St, Anytown, USA'),
('Hoang Van E', 'L4IbM@example.com', '1234567890', '123 Main St, Anytown, USA'),
('Vu Thi F', 'L4IbM@example.com', '1234567890', '123 Main St, Anytown, USA'),
('Do Van G', 'L4IbM@example.com', '1234567890', '123 Main St, Anytown, USA'),
('Nguyen Thi H', 'L4IbM@example.com', '1234567890', '123 Main St, Anytown, USA'),
('Le Van I', 'L4IbM@example.com', '1234567890', '123 Main St, Anytown, USA'),
('Tran Thi J', 'L4IbM@example.com', '1234567890', '123 Main St, Anytown, USA');

-- Mỗi khách hàng có 10 đơn hàng, mỗi đơn hàng có 3 sản phẩm

-- Thêm 10 sản phẩm mẫu
INSERT INTO SanPham (TenSP, Gia, SoLuong, HinhAnh) VALUES
(N'Áo thun',     150000, 100, 'ao_thun.jpg'),
(N'Quần jean',   350000,  80, 'quan_jean.jpg'),
(N'Giày sneaker',500000,  60, 'giay_sneaker.jpg'),
(N'Túi xách',    250000,  50, 'tui_xach.jpg'),
(N'Mũ lưỡi trai',120000, 120, 'mu_luoi_trai.jpg'),
(N'Kính mắt',    200000,  40, 'kinh_mat.jpg'),
(N'Thắt lưng',   180000,  70, 'that_lung.jpg'),
(N'Vớ/Tất',       50000, 200, 'vo_tat.jpg'),
(N'Áo khoác',    450000,  35, 'ao_khoac.jpg'),
(N'Đồng hồ',     800000,  25, 'dong_ho.jpg');

-- Thêm 10 đơn hàng cho mỗi khách hàng (tổng 100 đơn hàng)
DECLARE @KhachHangId INT = 1;
DECLARE @DonHangCount INT;

WHILE @KhachHangId <= 10
BEGIN
    SET @DonHangCount = 1;
    WHILE @DonHangCount <= 10
    BEGIN
        INSERT INTO DonHang (NgayDat, TongTien, MaKhachHang)
        VALUES (
            DATEADD(DAY, -(@KhachHangId * 10 + @DonHangCount), GETDATE()),
            0,
            @KhachHangId
        );
        SET @DonHangCount = @DonHangCount + 1;
    END
    SET @KhachHangId = @KhachHangId + 1;
END;

-- Thêm 3 sản phẩm cho mỗi đơn hàng (tổng 300 chi tiết đơn hàng)
-- Dùng cursor để lấy Id thực tế từ bảng DonHang, tránh lỗi FK
DECLARE @DonHangId INT;
DECLARE @SanPhamOffset INT;
DECLARE @IdSanPham INT;
DECLARE @GiaSP DECIMAL(18, 2);
DECLARE @SoLuongMua INT;
DECLARE @RowNum INT = 1;

DECLARE cur CURSOR FOR SELECT Id FROM DonHang ORDER BY Id;
OPEN cur;
FETCH NEXT FROM cur INTO @DonHangId;

WHILE @@FETCH_STATUS = 0
BEGIN
    SET @SanPhamOffset = 0;
    WHILE @SanPhamOffset <= 2
    BEGIN
        SET @IdSanPham = ((@RowNum + @SanPhamOffset - 1) % 10) + 1;
        SET @SoLuongMua = (@RowNum + @SanPhamOffset) % 5 + 1;
        SELECT @GiaSP = Gia FROM SanPham WHERE Id = @IdSanPham;

        INSERT INTO ChiTietDonHang (IdDonHang, IdSanPham, SoLuong, Gia)
        VALUES (@DonHangId, @IdSanPham, @SoLuongMua, @GiaSP);

        SET @SanPhamOffset = @SanPhamOffset + 1;
    END

    SET @RowNum = @RowNum + 1;
    FETCH NEXT FROM cur INTO @DonHangId;
END;

CLOSE cur;
DEALLOCATE cur;

-- Cập nhật TongTien cho mỗi đơn hàng dựa trên chi tiết đơn hàng
UPDATE DonHang
SET TongTien = (
    SELECT SUM(ct.SoLuong * ct.Gia)
    FROM ChiTietDonHang ct
    WHERE ct.IdDonHang = DonHang.Id
);