-- DROP TABLESPACE DA_ATBMM INCLUDING CONTENTS ;
DROP TABLESPACE DA_ATBM INCLUDING CONTENTS AND DATAFILES;



/*
-----------------
--TAO TABLESPACE
CREATE TABLESPACE DA_ATBM
   DATAFILE 'C:\Oracle\DA_ATBM_data.dbf' 
   SIZE 500m;

drop user ATBM_ADMIN cascade ;
--TAO TAI KHOAN ADMIN
ALTER SESSION SET "_ORACLE_SCRIPT" = TRUE;
CREATE USER ATBM_ADMIN identified by 123 DEFAULT TABLESPACE DA_ATBM;
GRANT ALL PRIVILEGES TO ATBM_ADMIN WITH ADMIN OPTION;
GRANT DBA TO ATBM_ADMIN;
------------------
*/



--DROP FOREIGN KEY NEU CO
BEGIN
  EXECUTE IMMEDIATE 'ALTER TABLE PHANCONG'
            || ' DROP CONSTRAINT fk_PCONG_NV' || ' DROP CONSTRAINT fk_PCONG_DA';
EXCEPTION
  WHEN OTHERS THEN
    IF SQLCODE != -2443 AND SQLCODE != -942 THEN
      RAISE;
    END IF;
END;
/
BEGIN
  EXECUTE IMMEDIATE 'ALTER TABLE NHANVIEN'
            || ' DROP CONSTRAINT fk_NQL_NV' || ' DROP CONSTRAINT fk_NV_PHGBAN';
EXCEPTION
  WHEN OTHERS THEN
    IF SQLCODE != -2443 AND SQLCODE != -942 THEN
      RAISE;
    END IF;
END;
/
BEGIN
  EXECUTE IMMEDIATE 'ALTER TABLE DEAN'
            || ' DROP CONSTRAINT fk_DA_PHGBAN' ;
EXCEPTION
  WHEN OTHERS THEN
    IF SQLCODE != -2443 AND SQLCODE != -942 THEN
      RAISE;
    END IF;
END;
/
BEGIN
  EXECUTE IMMEDIATE 'ALTER TABLE CAUHOIBAOMAT'
            || ' DROP CONSTRAINT fk_CDBM_NHANVIEN' ;
EXCEPTION
  WHEN OTHERS THEN
    IF SQLCODE != -2443 AND SQLCODE != -942 THEN
      RAISE;
    END IF;
END;
/

--xoa table neu da ton tai table
BEGIN
   EXECUTE IMMEDIATE 'DROP TABLE NHANVIEN';
EXCEPTION
   WHEN OTHERS THEN
      IF SQLCODE != -942 THEN
         RAISE;
      END IF;
END;
/
--
BEGIN
   EXECUTE IMMEDIATE 'DROP TABLE DEAN';
EXCEPTION
   WHEN OTHERS THEN
      IF SQLCODE != -942 THEN
         RAISE;
      END IF;
END;
/
--SELECT * FROM USER_CONSTRAINTS WHERE TABLE_NAME = 'DEAN';
--
BEGIN
   EXECUTE IMMEDIATE 'DROP TABLE PHONGBAN';
EXCEPTION
   WHEN OTHERS THEN
      IF SQLCODE != -942 THEN
         RAISE;
      END IF;
END;
/
--
BEGIN
   EXECUTE IMMEDIATE 'DROP TABLE PHANCONG';
EXCEPTION
   WHEN OTHERS THEN
      IF SQLCODE != -942 THEN
         RAISE;
      END IF;
END;
/
BEGIN
   EXECUTE IMMEDIATE 'DROP TABLE CAUHOIBAOMAT';
EXCEPTION
   WHEN OTHERS THEN
      IF SQLCODE != -942 THEN
         RAISE;
      END IF;
END;
/
--Tao table
create table NHANVIEN 
(	
	MANV 	varchar2(5),
    TENNV 	varchar2(30),
	PHAI 	varchar2(5),
    NGAYSINH 	date,
	DIACHI 	varchar2(50),
    SODT number(10),
	LUONG 	number(10,2),
    PHUCAP number(7,2),
    VAITRO varchar2(20),
	MANQL 	varchar2(5),
	PHG 	varchar2(5),
	CONSTRAINT pk_nv primary key (MANV)	
)TABLESPACE DA_ATBM;
/
create table PHONGBAN 
(
    MAPHG 	varchar2(5),
	TENPHG 	varchar2(30),
	TRPHG 	varchar2(5),
	CONSTRAINT pk_pb primary key (MAPHG)
)TABLESPACE DA_ATBM;
/
create table DEAN 
(
	MADA varchar2(5),
    TENDA varchar2(50),
	NGAYBD date,
	PHONG varchar2(5),
	CONSTRAINT pk_da primary key (MADA)
)TABLESPACE DA_ATBM;
/

create table PHANCONG 
(
	MANV	varchar2(5),
	MADA 	varchar2(5),
	THOIGIAN	date,
	CONSTRAINT pk_pc primary key (MANV, MADA)
)TABLESPACE DA_ATBM;
/
--phai tra loi trong lan dau dang nhap, va sư dung de lay lai mat khau khi quen
create table CAUHOIBAOMAT 
(
	MANV	varchar2(5),
	CAUHOI 	number(1),
	CAUTRALOI varchar2(50),
    SOLANTRALOI int default 5,
	CONSTRAINT pk_chbm primary key (MANV)
)TABLESPACE DA_ATBM;
/
ALTER TABLE PHANCONG
ADD CONSTRAINT fk_PCONG_NV
FOREIGN KEY (MANV)
REFERENCES NHANVIEN(MANV);
/
ALTER TABLE PHANCONG
ADD CONSTRAINT fk_PCONG_DA
FOREIGN KEY (MADA)
REFERENCES DEAN(MADA);
/
ALTER TABLE NHANVIEN
ADD CONSTRAINT fk_NQL_NV
FOREIGN KEY (MANQL)
REFERENCES NHANVIEN(MANV);
/
ALTER TABLE NHANVIEN
ADD CONSTRAINT fk_NV_PHGBAN
FOREIGN KEY (PHG)
REFERENCES PHONGBAN(MAPHG);
/
ALTER TABLE DEAN
ADD CONSTRAINT fk_DA_PHGBAN
FOREIGN KEY (PHONG)
REFERENCES PHONGBAN(MAPHG);
/
ALTER TABLE CAUHOIBAOMAT
ADD CONSTRAINT fk_CDBM_NHANVIEN
FOREIGN KEY (MANV)
REFERENCES NHANVIEN(MANV);
/
--NHAP DU LIEU MAU
INSERT ALL
into PHONGBAN
values('PB01','Marketing',null)
into PHONGBAN
values('PB02','Chuyên môn',null)
into PHONGBAN
values('PB03','Tài chính',null)
into PHONGBAN
values('PB04','Nhân sự',null)
SELECT * FROM dual;
-- select * from PHONGBAN

/
INSERT ALL
into NHANVIEN 
values ('NV001', 'Trần Bá Hộ','Nam',TO_DATE('02/11/1970','dd/mm/yyyy'),'119 Cống Quỳnh, Tp HCM', 0123456789, 5000, 500, 'Trưởng phòng', null,'PB01')

into NHANVIEN 
values ('NV002', 'Nguyễn Thanh Tùng','Nam',TO_DATE('20/08/1972','dd/mm/yyyy'),'222 Nguyễn Văn Cừ, Tp HCM', 0123157789, 1000, 400, 'QL trực tiếp', null,'PB01')

into NHANVIEN 
values ('NV003', 'Bùi Ngọc Hằng','Nam',TO_DATE('3/11/1984','dd/mm/yyyy'),'332 Nguyễn Thái Học, Tp HCM', 0123157143, 2500, 200, 'QL trực tiếp', null,'PB01')

into NHANVIEN 
values ('NV004', 'Lê Quỳnh Như','Nữ',TO_DATE('02/01/1992','dd/mm/yyyy'),'291 Hồ Văn Huê,  Tp HCM', 0123157789, 1300, 200, 'Nhân viên', 'NV003','PB01')

into NHANVIEN 
values ('NV005', 'Nguyễn Mạnh Hùng','Nam',TO_DATE('03/04/1985','dd/mm/yyyy'),'95 Bà Rịa, Vũng Tàu', 0123157632, 1800, 200, 'Nhân viên', 'NV002','PB01')

into NHANVIEN 
values ('NV006', 'Trần Thanh Tâm','Nam',TO_DATE('05/04/1975','dd/mm/yyyy'),'34 Mai Thị Lựu, Tp HCM', 0123109832, 1200, 200, 'Nhân viên', 'NV003','PB01')

into NHANVIEN 
values ('NV007', 'Trần Hồng Quang','Nam',TO_DATE('09/01/1981','dd/mm/yyyy'),'80 Lê Hồng Phong, Tp HCM', 0125609832, 2500, 200, 'Nhân viên', 'NV003','PB01')

into NHANVIEN 
values ('NV008', 'Phạm Văn Vinh','Nữ',TO_DATE('01/01/1999','dd/mm/yyyy'),'5 Trưng Vương, Hà Nội', 0125609832, 2200, 200, 'Nhân viên', 'NV002','PB01')

into NHANVIEN 
values ('NV009', 'Đinh Văn Hoàng','Nam',TO_DATE('02/01/1971','dd/mm/yyyy'),'119 Cống Quỳnh, Tp HCM', 0123456789, 3300, 200, 'Trưởng phòng', null,'PB02')

into NHANVIEN 
values ('NV010', 'Nguyễn Thanh Minh','Nam',TO_DATE('20/08/1997','dd/mm/yyyy'),'222 Nguyễn Văn Mách, Tp HCM', 0123157789, 2000, 200, 'QL trực tiếp', null,'PB02')

into NHANVIEN 
values ('NV011', 'Hà Văn Nam','Nam',TO_DATE('02/11/1987','dd/mm/yyyy'),'332 Nguyễn Thái Công, Tp HCM', 0123157143, 2500, 300, 'QL trực tiếp', null,'PB02')


into NHANVIEN 
values ('NV012', 'Lê Bảo Ngọc','Nữ',TO_DATE('03/01/1997','dd/mm/yyyy'),'211 Hồ Văn Cường,  Tp HCM', 0123157789, 4300, 200, 'Nhân viên', 'NV010','PB02')

into NHANVIEN 
values ('NV013', 'Nguyễn Hùng Chung','Nam',TO_DATE('01/04/1997','dd/mm/yyyy'),'95 Bà Rịa, Long Thành', 0123157632, 3800, 200, 'Nhân viên', 'NV011','PB02')

into NHANVIEN 
values ('NV014', 'Trần Tâm Như','Nam',TO_DATE('05/04/1987','dd/mm/yyyy'),'156 Mai Thị Lựu, Tp HCM', 0123109832, 3200, 200, 'Nhân viên', 'NV010','PB02')

into NHANVIEN 
values ('NV015', 'Quách Đại Hiệp','Nam',TO_DATE('09/01/1995','dd/mm/yyyy'),'90 Lê Hồng Phong, Tp HCM', 0125609832, 2500, 200, 'Nhân viên', 'NV011','PB02')

into NHANVIEN 
values ('NV016', 'Hà Ánh Tuyết','Nữ',TO_DATE('01/01/1985','dd/mm/yyyy'),'10 Trưng Vương, Hà Nội', 0125609832, 2200, 200, 'Nhân viên', 'NV010','PB02')

into NHANVIEN 
values ('NV017', 'Lê Văn Hải','Nam',TO_DATE('01/11/1960','dd/mm/yyyy'),'119 Cống Quỳnh, Bình Dương', 0373456789, 3000, 200, 'Nhân viên', 'NV010','PB02')

into NHANVIEN 
values ('NV018', 'Nguyễn Hải Châu','Nam',TO_DATE('26/08/2000','dd/mm/yyyy'),'222 Nguyễn Văn Cừ, Bình Dương', 0373157789, 4000, 200, 'Nhân viên', 'NV010','PB02')

into NHANVIEN 
values ('NV019', 'Bùi Minh Long','Nam',TO_DATE('3/11/1974','dd/mm/yyyy'),'332 Nguyễn Thái Học, Bình Dương', 0373157143, 2500, 200, 'Nhân viên', 'NV010','PB02')


into NHANVIEN 
values ('NV020', 'Lê Thị Quỳnh','Nữ',TO_DATE('02/09/1982','dd/mm/yyyy'),'291 Hồ Văn Huê,  Bình Dương', 0373157789, 4300, 500, 'Trưởng phòng', null,'PB03')

into NHANVIEN 
values ('NV021', 'Nguyễn Mạnh Cường','Nam',TO_DATE('03/08/1985','dd/mm/yyyy'),'95 Bà Rịa, Gia Lai', 0373157632, 2000, 400, 'Tài chính', null,'PB03')

into NHANVIEN 
values ('NV022', 'Trần Thanh Hà','Nam',TO_DATE('05/07/1995','dd/mm/yyyy'),'34 Mai Thị Lựu, Bình Dương', 0373109832, 2200, 500, 'Tài chính', null,'PB03')

into NHANVIEN 
values ('NV023', 'Trần Hồng Bàng','Nam',TO_DATE('09/05/1981','dd/mm/yyyy'),'80 Lê Hồng Phong, Bình Dương', 0375609832, 3000, 100, 'Trưởng phòng',null,'PB04')

into NHANVIEN 
values ('NV024', 'Phạm Văn Vũ','Nữ',TO_DATE('01/01/1989','dd/mm/yyyy'),'5 Trưng Vương, Hà Nội', 0375609832, 1800, 200, 'Nhân sự',null,'PB04')

into NHANVIEN 
values ('NV025', 'Đinh Ngọc Hoàng','Nam',TO_DATE('02/01/1998','dd/mm/yyyy'),'119 Cống Quỳnh, Bình Dương', 0373412789, 1500, 200, 'Nhân sự', null,'PB04')

into NHANVIEN 
values ('NV026', 'Nguyễn Hải Minh','Nam',TO_DATE('20/08/1996','dd/mm/yyyy'),'222 Nguyễn Văn Mách, Bình Dương', 0373150089, 2000, 200, 'Tài chính', null,'PB03')

into NHANVIEN 
values ('NV027', 'Hà Hoàng Nam','Nam',TO_DATE('02/11/1989','dd/mm/yyyy'),'332 Nguyễn Thái Công, Bình Dương', 0373157103, 1500, 200,'Nhân viên','NV002','PB01')


into NHANVIEN 
values ('NV028', 'Nguyễn Ngọc Bích','Nữ',TO_DATE('03/01/1977','dd/mm/yyyy'),'211 Hồ Văn Cường,  Bình Dương', 0373157709, 2300, 200, 'Trưởng đề án','NV010','PB02')

into NHANVIEN 
values ('NV029', 'Nguyễn Hải Minh','Nam',TO_DATE('01/04/1999','dd/mm/yyyy'),'95 Bà Rịa, Long Thành', 0373157637, 3800, 200, 'Trưởng đề án', 'NV003','PB01')

into NHANVIEN 
values ('NV030', 'Trần Đại Phong','Nam',TO_DATE('05/04/1989','dd/mm/yyyy'),'156 Mai Thị Lựu, Bình Dương', 0373109831, 2000, 200, 'QL trực tiếp', null,'PB02')

into NHANVIEN 
values ('NV031', 'Quách Văn Tỉnh','Nam',TO_DATE('09/01/1999','dd/mm/yyyy'),'90 Lê Hồng Phong, Bình Dương', 0375609832, 1000, 200, 'Nhân viên', 'NV030','PB02')

into NHANVIEN 
values ('NV032', 'Hà Hồi Hạnh','Nữ',TO_DATE('01/01/1985','dd/mm/yyyy'),'10 Trưng Vương, Hà Nội', 0375609832, 1600, 120, 'Nhân viên', 'NV030','PB02')

into NHANVIEN 
values ('NV033', 'Hoàng Minh Tiến','Nam',TO_DATE('01/04/1979','dd/mm/yyyy'),'95 Bà Rịa, Đà Nẵng', 0373157637, 3800, 200, 'Trưởng đề án', 'NV003','PB04')

SELECT * FROM dual;



INSERT ALL
into DEAN
values('DA01','Thiết kế mạng ABC company', TO_DATE('01/01/2016','dd/mm/yyyy'), 'PB02')

into DEAN
values('DA02','Thử nghiệm cáp quang CQ1', TO_DATE('01/01/2018','dd/mm/yyyy'), 'PB02')

into DEAN
values('DA03','Tuyển nhân sự tháng 1_2019', TO_DATE('01/01/2019','dd/mm/yyyy'), 'PB04')

into DEAN
values('DA04','Thử nghiệm hệ thống M1 lần 1', TO_DATE('01/06/2019','dd/mm/yyyy'), 'PB02')

into DEAN
values('DA05','Lắp đặt cáp quang CQ1', TO_DATE('01/12/2019','dd/mm/yyyy'), 'PB02')

into DEAN
values('DA06', 'Workshop hệ thống mạng',TO_DATE('01/01/2020','dd/mm/yyyy'), 'PB01')

into DEAN
values('DA07','Tuyển nhân sự tháng 6_2020', TO_DATE('01/06/2020','dd/mm/yyyy'), 'PB04')

into DEAN
values('DA08','Thử nghiệm hệ thống M1 lần 2', TO_DATE('01/08/2020','dd/mm/yyyy'), 'PB02')

into DEAN
values('DA09', 'Marketing hệ thống M1',TO_DATE('01/01/2021','dd/mm/yyyy'), 'PB01')

into DEAN
values('DA10','Training nhân viên 1_2022', TO_DATE('01/01/2022','dd/mm/yyyy'), 'PB04')
SELECT * FROM dual;

INSERT ALL
into PHANCONG 
values ('NV001','DA03',TO_DATE('01/01/2019','dd/mm/yyyy'))
into PHANCONG 
values ('NV001','DA06',TO_DATE('01/01/2020','dd/mm/yyyy'))
into PHANCONG 
values ('NV001','DA07',TO_DATE('01/06/2020','dd/mm/yyyy'))
into PHANCONG 
values ('NV001','DA09',TO_DATE('01/01/2021','dd/mm/yyyy'))

into PHANCONG 
values ('NV002','DA03',TO_DATE('01/01/2019','dd/mm/yyyy'))
into PHANCONG 
values ('NV002','DA06',TO_DATE('01/01/2020','dd/mm/yyyy'))
into PHANCONG 
values ('NV002','DA09',TO_DATE('01/01/2021','dd/mm/yyyy'))

into PHANCONG 
values ('NV003','DA06',TO_DATE('01/01/2020','dd/mm/yyyy'))
into PHANCONG 
values ('NV003','DA07',TO_DATE('01/06/2020','dd/mm/yyyy'))
into PHANCONG 
values ('NV003','DA09',TO_DATE('01/01/2021','dd/mm/yyyy'))


into PHANCONG 
values ('NV004','DA06',TO_DATE('01/01/2020','dd/mm/yyyy'))


into PHANCONG 
values ('NV005','DA09',TO_DATE('01/01/2021','dd/mm/yyyy'))
into PHANCONG 
values ('NV005','DA10',TO_DATE('01/01/2022','dd/mm/yyyy'))

into PHANCONG 
values ('NV006','DA06',TO_DATE('01/01/2020','dd/mm/yyyy'))
into PHANCONG 
values ('NV006','DA10',TO_DATE('01/01/2022','dd/mm/yyyy'))

into PHANCONG 
values ('NV007','DA09',TO_DATE('01/01/2021','dd/mm/yyyy'))

into PHANCONG 
values ('NV008','DA09',TO_DATE('01/01/2021','dd/mm/yyyy'))
into PHANCONG 
values ('NV008','DA10',TO_DATE('01/01/2022','dd/mm/yyyy'))

into PHANCONG 
values ('NV009','DA01',TO_DATE('01/01/2016','dd/mm/yyyy'))
into PHANCONG 
values ('NV009','DA02',TO_DATE('01/01/2018','dd/mm/yyyy'))
into PHANCONG 
values ('NV009','DA03',TO_DATE('01/01/2019','dd/mm/yyyy'))
into PHANCONG 
values ('NV009','DA04',TO_DATE('01/06/2019','dd/mm/yyyy'))
into PHANCONG 
values ('NV009','DA05',TO_DATE('01/12/2019','dd/mm/yyyy'))
into PHANCONG 
values ('NV009','DA08',TO_DATE('01/01/2020','dd/mm/yyyy'))


into PHANCONG 
values ('NV010','DA01',TO_DATE('01/01/2016','dd/mm/yyyy'))
into PHANCONG 
values ('NV010','DA02',TO_DATE('01/01/2018','dd/mm/yyyy'))
into PHANCONG 
values ('NV010','DA03',TO_DATE('01/01/2019','dd/mm/yyyy'))
into PHANCONG 
values ('NV010','DA04',TO_DATE('01/06/2019','dd/mm/yyyy'))

into PHANCONG 
values ('NV011','DA04',TO_DATE('01/06/2019','dd/mm/yyyy'))
into PHANCONG 
values ('NV011','DA05',TO_DATE('01/12/2019','dd/mm/yyyy'))
into PHANCONG 
values ('NV011','DA08',TO_DATE('01/01/2020','dd/mm/yyyy'))

into PHANCONG 
values ('NV012','DA01',TO_DATE('01/01/2016','dd/mm/yyyy'))
into PHANCONG 
values ('NV012','DA02',TO_DATE('01/01/2018','dd/mm/yyyy'))

into PHANCONG 
values ('NV013','DA02',TO_DATE('01/01/2018','dd/mm/yyyy'))
into PHANCONG 
values ('NV013','DA04',TO_DATE('01/06/2019','dd/mm/yyyy'))
into PHANCONG 
values ('NV013','DA05',TO_DATE('01/12/2019','dd/mm/yyyy'))

into PHANCONG 
values ('NV014','DA01',TO_DATE('01/01/2016','dd/mm/yyyy'))
into PHANCONG 
values ('NV014','DA02',TO_DATE('01/01/2018','dd/mm/yyyy'))

into PHANCONG 
values ('NV015','DA04',TO_DATE('01/06/2019','dd/mm/yyyy'))

into PHANCONG 
values ('NV016','DA01',TO_DATE('01/01/2016','dd/mm/yyyy'))
into PHANCONG 
values ('NV016','DA08',TO_DATE('01/01/2020','dd/mm/yyyy'))

into PHANCONG 
values ('NV017','DA02',TO_DATE('01/01/2018','dd/mm/yyyy'))
into PHANCONG 
values ('NV017','DA05',TO_DATE('01/12/2019','dd/mm/yyyy'))

into PHANCONG 
values ('NV018','DA04',TO_DATE('01/06/2019','dd/mm/yyyy'))
into PHANCONG 
values ('NV018','DA05',TO_DATE('01/12/2019','dd/mm/yyyy'))
into PHANCONG 
values ('NV018','DA08',TO_DATE('01/01/2020','dd/mm/yyyy'))

into PHANCONG 
values ('NV019','DA05',TO_DATE('01/12/2019','dd/mm/yyyy'))

into PHANCONG 
values ('NV020','DA02',TO_DATE('01/01/2018','dd/mm/yyyy'))
into PHANCONG 
values ('NV020','DA04',TO_DATE('01/06/2019','dd/mm/yyyy'))
into PHANCONG 
values ('NV020','DA05',TO_DATE('01/12/2019','dd/mm/yyyy'))

into PHANCONG 
values ('NV021','DA01',TO_DATE('01/01/2016','dd/mm/yyyy'))
into PHANCONG 
values ('NV021','DA02',TO_DATE('01/01/2018','dd/mm/yyyy'))

into PHANCONG 
values ('NV022','DA05',TO_DATE('01/12/2019','dd/mm/yyyy'))
into PHANCONG 
values ('NV022','DA08',TO_DATE('01/01/2020','dd/mm/yyyy'))

into PHANCONG 
values ('NV023','DA03',TO_DATE('01/01/2019','dd/mm/yyyy'))
into PHANCONG 
values ('NV023','DA07',TO_DATE('01/06/2020','dd/mm/yyyy'))
into PHANCONG 
values ('NV023','DA10',TO_DATE('01/01/2022','dd/mm/yyyy'))

into PHANCONG 
values ('NV024','DA03',TO_DATE('01/01/2019','dd/mm/yyyy'))
into PHANCONG 
values ('NV024','DA07',TO_DATE('01/06/2020','dd/mm/yyyy'))

into PHANCONG 
values ('NV025','DA07',TO_DATE('01/06/2020','dd/mm/yyyy'))
into PHANCONG 
values ('NV025','DA10',TO_DATE('01/01/2022','dd/mm/yyyy'))

into PHANCONG 
values ('NV026','DA04',TO_DATE('01/06/2019','dd/mm/yyyy'))

into PHANCONG 
values ('NV027','DA09',TO_DATE('01/01/2021','dd/mm/yyyy'))
into PHANCONG 
values ('NV027','DA10',TO_DATE('01/01/2022','dd/mm/yyyy'))

into PHANCONG 
values ('NV028','DA01',TO_DATE('01/01/2016','dd/mm/yyyy'))
into PHANCONG 
values ('NV028','DA02',TO_DATE('01/01/2018','dd/mm/yyyy'))
into PHANCONG 
values ('NV028','DA04',TO_DATE('01/06/2019','dd/mm/yyyy'))
into PHANCONG 
values ('NV028','DA05',TO_DATE('01/12/2019','dd/mm/yyyy'))
into PHANCONG 
values ('NV028','DA08',TO_DATE('01/01/2020','dd/mm/yyyy'))


into PHANCONG 
values ('NV029','DA06',TO_DATE('01/01/2020','dd/mm/yyyy'))
into PHANCONG 
values ('NV029','DA09',TO_DATE('01/01/2021','dd/mm/yyyy'))

into PHANCONG 
values ('NV030','DA04',TO_DATE('01/06/2019','dd/mm/yyyy'))
into PHANCONG 
values ('NV030','DA05',TO_DATE('01/12/2019','dd/mm/yyyy'))
into PHANCONG 
values ('NV030','DA08',TO_DATE('01/01/2020','dd/mm/yyyy'))

into PHANCONG 
values ('NV031','DA04',TO_DATE('01/06/2019','dd/mm/yyyy'))

into PHANCONG 
values ('NV032','DA05',TO_DATE('01/12/2019','dd/mm/yyyy'))
into PHANCONG 
values ('NV032','DA08',TO_DATE('01/01/2020','dd/mm/yyyy'))

into PHANCONG 
values ('NV033','DA03',TO_DATE('01/01/2019','dd/mm/yyyy'))
into PHANCONG 
values ('NV033','DA07',TO_DATE('01/06/2020','dd/mm/yyyy'))
into PHANCONG 
values ('NV033','DA10',TO_DATE('01/01/2022','dd/mm/yyyy'))
SELECT * FROM dual;
/
--tạo các role
DECLARE
    MA_TRPHOG VARCHAR2(50):='123';
    MA_PHG VARCHAR2(50);
    COUNT_MANV INT :=0;
BEGIN
    FOR N IN (SELECT * FROM PhongBan) LOOP
        MA_PHG := N.MAPHG;
        SELECT COUNT(MANV) INTO COUNT_MANV
        FROM NHANVIEN
        WHERE PHG = MA_PHG AND VAITRO = 'Trưởng phòng';

        IF COUNT_MANV = 1 THEN

            SELECT MANV INTO MA_TRPHOG
            FROM NHANVIEN
            WHERE PHG = MA_PHG AND VAITRO = 'Trưởng phòng';

            UPDATE PHONGBAN
            SET TRPHG = MA_TRPHOG
            WHERE MAPHG = MA_PHG;
        end if;
        -- Kiểm tra điều kiện để thoát khỏi vòng lặp
        -- Ví dụ: Nếu đã cập nhật thành công cho một bản ghi, ta có thể thoát khỏi vòng lặp

    END LOOP;
END;
/
--TAO ROLE
DECLARE
    COUNT_ROLE NUMBER;
BEGIN
    EXECUTE IMMEDIATE 'ALTER SESSION SET "_ORACLE_SCRIPT" = TRUE';
    -- Tạo vai trò NHANVIEN
    SELECT COUNT(*) INTO COUNT_ROLE
    FROM DBA_ROLES
    WHERE ROLE = 'NHANVIEN';

    -- Kiểm tra và thực hiện các hành động tương ứng
    IF COUNT_ROLE > 0 THEN
        EXECUTE IMMEDIATE 'DROP ROLE NHANVIEN';
    END IF;
    EXECUTE IMMEDIATE 'CREATE ROLE NHANVIEN';

     SELECT COUNT(*) INTO COUNT_ROLE
    FROM DBA_ROLES
    WHERE ROLE = 'TRUONGPHONG';

    IF COUNT_ROLE > 0 THEN
        EXECUTE IMMEDIATE 'DROP ROLE TRUONGPHONG';
    END IF;
    EXECUTE IMMEDIATE 'CREATE ROLE TRUONGPHONG';

    -- Tiếp tục tương tự cho các vai trò khác
    -- Ví dụ: QLTRUCTIEP, TAICHINH, NHANSU, TRUONGDEAN

    SELECT COUNT(*) INTO COUNT_ROLE
    FROM DBA_ROLES
    WHERE ROLE = 'QLTRUCTIEP';

    IF COUNT_ROLE > 0 THEN
        EXECUTE IMMEDIATE 'DROP ROLE QLTRUCTIEP';
    END IF;
    EXECUTE IMMEDIATE 'CREATE ROLE QLTRUCTIEP';

    SELECT COUNT(*) INTO COUNT_ROLE
    FROM DBA_ROLES
    WHERE ROLE = 'TAICHINH';

    IF COUNT_ROLE > 0 THEN
        EXECUTE IMMEDIATE 'DROP ROLE TAICHINH';
    END IF;
    EXECUTE IMMEDIATE 'CREATE ROLE TAICHINH';
    --Nhan su
    SELECT COUNT(*) INTO COUNT_ROLE
    FROM DBA_ROLES
    WHERE ROLE = 'NHANSU';

    IF COUNT_ROLE > 0 THEN
        EXECUTE IMMEDIATE 'DROP ROLE NHANSU';
    END IF;
    EXECUTE IMMEDIATE 'CREATE ROLE NHANSU';
    
    SELECT COUNT(*) INTO COUNT_ROLE
    FROM DBA_ROLES
    WHERE ROLE = 'TRUONGDEAN';

    IF COUNT_ROLE > 0 THEN
        EXECUTE IMMEDIATE 'DROP ROLE TRUONGDEAN';
    END IF;
    EXECUTE IMMEDIATE 'CREATE ROLE TRUONGDEAN';
END;
/

--drop user
CREATE OR REPLACE PROCEDURE usp_dropUserFromNHANVIEN
AS
   CURSOR cur IS
      SELECT MANV
      FROM NHANVIEN;
   username VARCHAR2(30);
BEGIN
   OPEN cur;
   LOOP
      FETCH cur INTO username;
      EXIT WHEN cur%NOTFOUND;
      BEGIN
         begin
         EXECUTE IMMEDIATE 'DROP USER ' || username || ' CASCADE';
            DBMS_OUTPUT.PUT_LINE('User ' || username || ' dropped.');
         EXCEPTION
            WHEN OTHERS THEN
               DBMS_OUTPUT.PUT_LINE('Error dropping user ' || username || ': ' || SQLERRM);
         end;
      EXCEPTION
         WHEN OTHERS THEN
            DBMS_OUTPUT.PUT_LINE('Error dropping user ' || username || ': ' || SQLERRM);
      END;
   END LOOP;
   CLOSE cur;
END;
/
execute usp_dropUserFromNHANVIEN;
/

--create user
CREATE OR REPLACE PROCEDURE sp_CreateUser AS
   CURSOR cur_nv IS
      SELECT *
      FROM NHANVIEN;
      --WHERE MANV NOT IN (SELECT USERNAME FROM ALL_USERS);
     usr NHANVIEN%ROWTYPE;
BEGIN
   OPEN cur_nv;
   execute immediate 'ALTER SESSION SET "_ORACLE_SCRIPT" = TRUE';
   
   LOOP
      FETCH cur_nv INTO usr;
      EXIT WHEN cur_nv%NOTFOUND;
      
      execute immediate('CREATE USER ' || usr.MANV || ' IDENTIFIED BY 1 DEFAULT TABLESPACE DA_ATBM');
      execute immediate('GRANT CREATE SESSION TO ' || usr.MANV);
      execute immediate('GRANT NHANVIEN TO ' || usr.MANV);
      IF usr.VaiTro = 'Trưởng phòng' THEN
         execute immediate('GRANT TRUONGPHONG TO ' || usr.MANV);
      ELSIF usr.VaiTro = 'QL trực tiếp' THEN
         execute immediate('GRANT QLTRUCTIEP TO ' || usr.MANV);
      ELSIF usr.VaiTro = 'Tài chính' THEN
         execute immediate('GRANT TAICHINH TO ' || usr.MANV);
      ELSIF usr.VaiTro = 'Nhân sự' THEN
         execute immediate('GRANT NHANSU TO ' || usr.MANV);
      END IF;
   END LOOP;

   CLOSE cur_nv;
END;
/
BEGIN 
    sp_CreateUser;
END;
/

-- Gắn view này cho role NHANVIEN
CREATE VIEW LayVaiTro AS
SELECT VAITRO 
FROM NHANVIEN 
WHERE MANV = SYS_CONTEXT('USERENV', 'SESSION_USER');
/
GRANT SELECT ON LayVaiTro TO NHANVIEN;
/

