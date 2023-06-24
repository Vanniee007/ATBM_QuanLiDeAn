-- Chính sách #3 Trưởng Phòng

-- T có quyền như là một nhân viên thông thường (vai trò “Nhân viên”). Ngoài ra, với các dòng trong quan hệ NHANVIEN liên quan đến các nhân viên thuộc phòng ban mà T làm trưởng phòng thì T có quyền xem tất cả các thuộc tính, trừ thuộc tính LUONG và PHUCAP.
CREATE VIEW TP_NHANVIEN AS
SELECT MANV, TENNV,	PHAI, NGAYSINH,	DIACHI, SODT, DECODE(MANV,SYS_CONTEXT('USERENV', 'SESSION_USER'),LUONG,NULL) LUONG, DECODE(MANV,SYS_CONTEXT('USERENV', 'SESSION_USER'),PHUCAP,NULL) PHUCAP, VAITRO,	MANQL, PHG
FROM NHANVIEN 
WHERE PHG = (select PHG from NHANVIEN where MANV =  SYS_CONTEXT('USERENV', 'SESSION_USER'));

/*
grant select on ATBM_ADMIN.TP_NHANVIEN to TP01
    select * from ATBM_ADMIN.TP_NHANVIEN
*/

-- Có thể thêm, xóa, cập nhật trên quan hệ PHANCONG liên quan đến các nhân viên thuộc phòng ban mà T làm trưởng phòng.
CREATE or replace VIEW TP_PHANCONG AS
SELECT *
FROM PHANCONG 
WHERE MANV in (select MANV from NHANVIEN where PHG = (select PHG from NHANVIEN where MANV =  SYS_CONTEXT('USERENV', 'SESSION_USER')));

grant select on TP_PHANCONG to TP01
select * from ATBM_ADMIN.TP_PHANCONG
