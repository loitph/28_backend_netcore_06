-- Get top 1000 records from Users table
SELECT TOP (1000) [Id]
      ,[Name]
      ,[Password]
  FROM [dotnetcore_06_db].[dbo].[Users]

-- Insert a new user with an empty password
INSERT INTO Users (Name, Password)
VALUES ('JohnDoe', '');

INSERT INTO Users (Name, Password)
VALUES ('MaryJane', '');

-- Retrieve all users to verify the insertion
SELECT ID as MSSV, Name FROM Users;

-- Get only top 2 rows
SELECT TOP 2 ID as MSSV, Name FROM Users;

-- Create table with named is SINHVIEN (HoTen, NgaySinh, GioiTinh, DiaChi, DaXoa)
CREATE TABLE SINHVIEN (
  HoTen NVARCHAR(100),
  NgaySinh DATE,
  GioiTinh NVARCHAR(10),
  DiaChi NVARCHAR(255),
  DaXoa INT
);

-- add Id column as primary key with auto increment
ALTER TABLE SINHVIEN
ADD Id INT IDENTITY(1,1) PRIMARY KEY;

INSERT INTO SINHVIEN (HoTen, NgaySinh, GioiTinh, DiaChi, DaXoa) VALUES (N'Nguyễn Văn A', '2000-01-01', 1, N'Hà Nội', 0);
INSERT INTO SINHVIEN (HoTen, NgaySinh, GioiTinh, DiaChi, DaXoa) VALUES (N'Nguyễn Văn B', '2000-02-01', 1, N'Hà Nội', 0);
INSERT INTO SINHVIEN (HoTen, NgaySinh, GioiTinh, DiaChi, DaXoa) VALUES (N'Nguyễn Văn C', '2000-03-01', 1, N'Hà Nội', 0);
INSERT INTO SINHVIEN (HoTen, NgaySinh, GioiTinh, DiaChi, DaXoa) VALUES (N'Nguyễn Văn D', '2000-04-01', 1, N'Hồ Chí Minh', 0);
INSERT INTO SINHVIEN (HoTen, NgaySinh, GioiTinh, DiaChi, DaXoa) VALUES (N'Nguyễn Văn E', '2000-05-01', 1, N'Đà Nẵng', 0);
INSERT INTO SINHVIEN (HoTen, NgaySinh, GioiTinh, DiaChi, DaXoa) VALUES (N'Trần Thị A', '2001-01-10', 0, N'Cần Thơ', 0);
INSERT INTO SINHVIEN (HoTen, NgaySinh, GioiTinh, DiaChi, DaXoa) VALUES (N'Trần Thị B', '2001-02-10', 0, N'Hải Phòng', 0);
INSERT INTO SINHVIEN (HoTen, NgaySinh, GioiTinh, DiaChi, DaXoa) VALUES (N'Lê Văn A', '1999-03-15', 1, N'Huế', 0);
INSERT INTO SINHVIEN (HoTen, NgaySinh, GioiTinh, DiaChi, DaXoa) VALUES (N'Lê Văn B', '1999-04-15', 1, N'Nha Trang', 0);
INSERT INTO SINHVIEN (HoTen, NgaySinh, GioiTinh, DiaChi, DaXoa) VALUES (N'Phạm Thị A', '2002-05-20', 0, N'Vũng Tàu', 0);

INSERT INTO SINHVIEN (HoTen, NgaySinh, GioiTinh, DiaChi, DaXoa) VALUES (N'Phạm Thị B', '2002-06-20', 0, N'Bình Dương', 0);
INSERT INTO SINHVIEN (HoTen, NgaySinh, GioiTinh, DiaChi, DaXoa) VALUES (N'Hoàng Văn A', '2001-07-11', 1, N'Đồng Nai', 0);
INSERT INTO SINHVIEN (HoTen, NgaySinh, GioiTinh, DiaChi, DaXoa) VALUES (N'Hoàng Văn B', '2001-08-11', 1, N'Long An', 0);
INSERT INTO SINHVIEN (HoTen, NgaySinh, GioiTinh, DiaChi, DaXoa) VALUES (N'Đỗ Thị A', '2000-09-09', 0, N'Tây Ninh', 0);
INSERT INTO SINHVIEN (HoTen, NgaySinh, GioiTinh, DiaChi, DaXoa) VALUES (N'Đỗ Thị B', '2000-10-09', 0, N'Nam Định', 0);
INSERT INTO SINHVIEN (HoTen, NgaySinh, GioiTinh, DiaChi, DaXoa) VALUES (N'Bùi Văn A', '1998-11-30', 1, N'Thanh Hóa', 0);
INSERT INTO SINHVIEN (HoTen, NgaySinh, GioiTinh, DiaChi, DaXoa) VALUES (N'Bùi Văn B', '1998-12-30', 1, N'Nghệ An', 0);
INSERT INTO SINHVIEN (HoTen, NgaySinh, GioiTinh, DiaChi, DaXoa) VALUES (N'Võ Thị A', '2003-01-25', 0, N'Quảng Nam', 0);
INSERT INTO SINHVIEN (HoTen, NgaySinh, GioiTinh, DiaChi, DaXoa) VALUES (N'Võ Thị B', '2003-02-25', 0, N'Quảng Ngãi', 0);
INSERT INTO SINHVIEN (HoTen, NgaySinh, GioiTinh, DiaChi, DaXoa) VALUES (N'Dương Văn A', '2002-03-03', 1, N'Bến Tre', 0);

INSERT INTO SINHVIEN (HoTen, NgaySinh, GioiTinh, DiaChi, DaXoa) VALUES (N'Dương Văn B', '2002-04-03', 1, N'Trà Vinh', 0);
INSERT INTO SINHVIEN (HoTen, NgaySinh, GioiTinh, DiaChi, DaXoa) VALUES (N'Ngô Thị A', '2001-05-05', 0, N'Sóc Trăng', 0);
INSERT INTO SINHVIEN (HoTen, NgaySinh, GioiTinh, DiaChi, DaXoa) VALUES (N'Ngô Thị B', '2001-06-05', 0, N'An Giang', 0);
INSERT INTO SINHVIEN (HoTen, NgaySinh, GioiTinh, DiaChi, DaXoa) VALUES (N'Đặng Văn A', '2000-07-07', 1, N'Kiên Giang', 0);
INSERT INTO SINHVIEN (HoTen, NgaySinh, GioiTinh, DiaChi, DaXoa) VALUES (N'Đặng Văn B', '2000-08-07', 1, N'Cà Mau', 0);

-- Tiếp tục tương tự đến đủ 100 sinh viên

INSERT INTO SINHVIEN (HoTen, NgaySinh, GioiTinh, DiaChi, DaXoa) VALUES (N'Sinh Viên 26', '2001-01-01', 1, N'Hà Nội', 0);
INSERT INTO SINHVIEN (HoTen, NgaySinh, GioiTinh, DiaChi, DaXoa) VALUES (N'Sinh Viên 27', '2001-02-01', 0, N'Hồ Chí Minh', 0);
INSERT INTO SINHVIEN (HoTen, NgaySinh, GioiTinh, DiaChi, DaXoa) VALUES (N'Sinh Viên 28', '2001-03-01', 1, N'Đà Nẵng', 0);
INSERT INTO SINHVIEN (HoTen, NgaySinh, GioiTinh, DiaChi, DaXoa) VALUES (N'Sinh Viên 29', '2001-04-01', 0, N'Hải Phòng', 0);
INSERT INTO SINHVIEN (HoTen, NgaySinh, GioiTinh, DiaChi, DaXoa) VALUES (N'Sinh Viên 30', '2001-05-01', 1, N'Cần Thơ', 0);

INSERT INTO SINHVIEN (HoTen, NgaySinh, GioiTinh, DiaChi, DaXoa) VALUES (N'Sinh Viên 31', '2001-06-01', 0, N'Hà Nội', 0);
INSERT INTO SINHVIEN (HoTen, NgaySinh, GioiTinh, DiaChi, DaXoa) VALUES (N'Sinh Viên 32', '2001-07-01', 1, N'Hồ Chí Minh', 0);
INSERT INTO SINHVIEN (HoTen, NgaySinh, GioiTinh, DiaChi, DaXoa) VALUES (N'Sinh Viên 33', '2001-08-01', 0, N'Đà Nẵng', 0);
INSERT INTO SINHVIEN (HoTen, NgaySinh, GioiTinh, DiaChi, DaXoa) VALUES (N'Sinh Viên 34', '2001-09-01', 1, N'Hải Phòng', 0);
INSERT INTO SINHVIEN (HoTen, NgaySinh, GioiTinh, DiaChi, DaXoa) VALUES (N'Sinh Viên 35', '2001-10-01', 0, N'Cần Thơ', 0);

INSERT INTO SINHVIEN (HoTen, NgaySinh, GioiTinh, DiaChi, DaXoa) VALUES (N'Sinh Viên 36', '2001-11-01', 1, N'Hà Nội', 0);
INSERT INTO SINHVIEN (HoTen, NgaySinh, GioiTinh, DiaChi, DaXoa) VALUES (N'Sinh Viên 37', '2001-12-01', 0, N'Hồ Chí Minh', 0);
INSERT INTO SINHVIEN (HoTen, NgaySinh, GioiTinh, DiaChi, DaXoa) VALUES (N'Sinh Viên 38', '2002-01-01', 1, N'Đà Nẵng', 0);
INSERT INTO SINHVIEN (HoTen, NgaySinh, GioiTinh, DiaChi, DaXoa) VALUES (N'Sinh Viên 39', '2002-02-01', 0, N'Hải Phòng', 0);
INSERT INTO SINHVIEN (HoTen, NgaySinh, GioiTinh, DiaChi, DaXoa) VALUES (N'Sinh Viên 40', '2002-03-01', 1, N'Cần Thơ', 0);

-- ...
-- COPY tiếp pattern này tới Sinh Viên 100

INSERT INTO SINHVIEN (HoTen, NgaySinh, GioiTinh, DiaChi, DaXoa) VALUES (N'Sinh Viên 100', '2005-12-01', 1, N'Hồ Chí Minh', 0);

-- select all records from SINHVIEN table
SELECT * FROM SINHVIEN;

-- where clause: select students born after 2001
SELECT * FROM SINHVIEN
WHERE NgaySinh > '2001-01-01';

-- like clause: select students whose name contains 'A'
SELECT * FROM SINHVIEN
WHERE HoTen LIKE '%A%';

-- order by clause: select all students ordered by name
SELECT * FROM SINHVIEN
ORDER BY HoTen;

-- order by clause: select all students ordered by name and then by birth date
SELECT * FROM SINHVIEN
ORDER BY HoTen, NgaySinh;

-- select partial data with pagination: select students with pagination (page 1, page size 10)
SELECT * FROM SINHVIEN
ORDER BY HoTen
OFFSET 0 ROWS FETCH NEXT 10 ROWS ONLY;

-- select partial data with pagination with cursor (assuming we have a cursor for pagination)
DECLARE @PageNumber INT = 2;
DECLARE @PageSize INT = 10;
DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

SELECT * FROM SINHVIEN
ORDER BY HoTen
OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

-- update a student's address where the name is 'Nguyễn Văn A'
UPDATE SINHVIEN
SET DiaChi = N'Hà Nội Mới'
WHERE HoTen = N'Nguyễn Văn A';


-- 1. get all students whose name start with 'Nguyễn' and order by birth date
SELECT * FROM SINHVIEN
WHERE HoTen LIKE N'Nguyễn%'
ORDER BY NgaySinh;

-- 2. update address for students who has id is 5
UPDATE SINHVIEN
SET DiaChi = N'Đà Nẵng Mới'
WHERE Id = 5;

SELECT * FROM SINHVIEN
WHERE Id = 5;

-- 3. get students with page 2 and page size 10, ordered by name
DECLARE @PageNum INT = 2;
DECLARE @PageSz INT = 10;
DECLARE @Ost INT = (@PageNum - 1) * @PageSz;

SELECT * FROM SINHVIEN
ORDER BY HoTen
OFFSET @Ost ROWS FETCH NEXT @PageSz ROWS ONLY;

-- 4. delete students who has id is 10
DELETE FROM SINHVIEN
WHERE Id = 10;

SELECT * FROM SINHVIEN
WHERE Id = 10;

-- 5. create a query to count student boys and girls and print out as table
SELECT 
  SUM(CASE WHEN GioiTinh = 1 THEN 1 ELSE 0 END) AS SoLuongNam,
  SUM(CASE WHEN GioiTinh = 0 THEN 1 ELSE 0 END) AS SoLuongNu
FROM SINHVIEN;


-- get GioiTinh and count of students for each gender, only show gender with more than 20 students
SELECT
  GioiTinh,
  COUNT(*) AS SoLuong
FROM SINHVIEN
GROUP BY GioiTinh
HAVING COUNT(*) > 20;