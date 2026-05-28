CREATE TABLE Phim (
  Id INT PRIMARY KEY IDENTITY(1,1),
  TenPhim NVARCHAR(255),
  HinhAnh NVARCHAR(255),
  MoTa NVARCHAR(MAX),
  NgayChieu DATETIME,
  DanhGia FLOAT
    CONSTRAINT CK_DanhGia
    CHECK (DanhGia >= 0 AND DanhGia <= 5)
);

CREATE TABLE TheLoai (
  Id INT PRIMARY KEY IDENTITY(1,1),
  TenTheLoai NVARCHAR(255)
);

CREATE TABLE QuocGia (
  Id INT PRIMARY KEY IDENTITY(1,1),
  TenQuocGia NVARCHAR(255)
);

CREATE TABLE SanXuat (
  IdQuocGia INT,
  IdPhim INT,
  PRIMARY KEY (IdQuocGia, IdPhim),
  FOREIGN KEY (IdQuocGia) REFERENCES QuocGia(Id),
  FOREIGN KEY (IdPhim) REFERENCES Phim(Id)
);

-- Bảng liên kết giữa Phim và TheLoai (nhiều-nhiều)
CREATE TABLE PhimTheLoai (
  IdPhim   INT,
  IdTheLoai INT,
  PRIMARY KEY (IdPhim, IdTheLoai),
  FOREIGN KEY (IdPhim)    REFERENCES Phim(Id),
  FOREIGN KEY (IdTheLoai) REFERENCES TheLoai(Id)
);

-- Dữ liệu mẫu
INSERT INTO TheLoai (TenTheLoai) VALUES
  (N'Hành động'),
  (N'Tình cảm'),
  (N'Kinh dị'),
  (N'Hài hước'),
  (N'Khoa học viễn tưởng');

INSERT INTO Phim (TenPhim, HinhAnh, MoTa, NgayChieu, DanhGia) VALUES
  (N'Avengers: Endgame',   'avengers.jpg',  N'Các siêu anh hùng Marvel tập hợp lần cuối.',  '2019-04-26', 4.8),
  (N'Titanic',             'titanic.jpg',   N'Chuyện tình lãng mạn trên con tàu định mệnh.', '1997-12-19', 4.7),
  (N'It',                  'it.jpg',        N'Gã hề Pennywise gieo rắc nỗi kinh hoàng.',     '2017-09-08', 4.2),
  (N'The Mask',            'mask.jpg',      N'Bộ mặt nạ ma thuật biến người thường thành hề.','1994-07-29', 4.3),
  (N'Interstellar',        'interstellar.jpg', N'Hành trình xuyên không gian để cứu nhân loại.','2014-11-07', 4.9);

-- Gán thể loại cho từng phim (nhiều-nhiều)
INSERT INTO PhimTheLoai (IdPhim, IdTheLoai) VALUES
  (1, 1),  -- Avengers: Hành động
  (1, 5),  -- Avengers: Khoa học viễn tưởng
  (2, 2),  -- Titanic: Tình cảm
  (3, 3),  -- It: Kinh dị
  (4, 4),  -- The Mask: Hài hước
  (4, 1),  -- The Mask: Hành động
  (5, 5),  -- Interstellar: Khoa học viễn tưởng
  (5, 1);  -- Interstellar: Hành động

-- Truy vấn: lấy danh sách phim kèm các thể loại
SELECT
  p.TenPhim,
  STRING_AGG(t.TenTheLoai, ', ') AS TheLoai
FROM Phim p
JOIN PhimTheLoai pt ON p.Id = pt.IdPhim
JOIN TheLoai t      ON t.Id = pt.IdTheLoai
GROUP BY p.TenPhim;

-- Truy vấn: lấy tất cả phim thuộc thể loại 'Hành động'
SELECT p.*
FROM Phim p
JOIN PhimTheLoai pt ON p.Id = pt.IdPhim
JOIN TheLoai t      ON t.Id = pt.IdTheLoai
WHERE t.TenTheLoai = N'Hành động';
