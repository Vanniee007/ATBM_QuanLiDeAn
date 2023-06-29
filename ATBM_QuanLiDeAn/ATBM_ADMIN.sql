--DROP TABLESPACE DA_ATBM INCLUDING CONTENTS;

/*
-----------------
--TAO TABLESPACE
CREATE TABLESPACE DA_ATBM
   DATAFILE 'D:\temp\DA_ATBM_data.dbf' 
   SIZE 500m;
--drop user ATBM_ADMIN
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

BEGIN
  EXECUTE IMMEDIATE 'ALTER TABLE NHANVIEN'
            || ' DROP CONSTRAINT keys_ma_nv_fk';
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
BEGIN
   EXECUTE IMMEDIATE 'DROP TABLE COUPLE_OF_KEYS';
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
	LUONG 	varchar2(4000),
    PHUCAP varchar2(4000),
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
CREATE TABLE COUPLE_OF_KEYS (
    MANV VARCHAR2(5) PRIMARY KEY,
    public_key VARCHAR2(4000),
    private_key VARCHAR2(4000)
)TABLESPACE DA_ATBM;
/

--phai tra loi trong lan dau dang nhap, va sư dung de lay lai mat khau khi quen

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
ALTER TABLE COUPLE_OF_KEYS ADD CONSTRAINT keys_ma_nv_fk
    FOREIGN KEY (MANV)
    REFERENCES NHANVIEN (MANV);
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

INSERT ALL
into NHANVIEN
values ('NV001', 'Trần Bá Hộ','Nam',TO_DATE('02/11/1970','dd/mm/yyyy'),'119 Cống Quỳnh, Tp HCM',123456789,'5000','500', 'Trưởng phòng', null,'PB01')
	
into NHANVIEN
values ('NV002', 'Nguyễn Thanh Tùng','Nam',TO_DATE('20/08/1972','dd/mm/yyyy'),'222 Nguyễn Văn Cừ, Tp HCM',123157789,'1000','400', 'QL trực tiếp', null,'PB01')
	
into NHANVIEN
values ('NV003', 'Bùi Ngọc Hằng','Nam',TO_DATE('3/11/1984','dd/mm/yyyy'),'332 Nguyễn Thái Học, Tp HCM',123157143,'2500','200', 'QL trực tiếp', null,'PB01')
	
into NHANVIEN
values ('NV004', 'Lê Quỳnh Như','Nữ',TO_DATE('02/01/1992','dd/mm/yyyy'),'291 Hồ Văn Huê,  Tp HCM',123157789,'1300','200', 'Nhân viên', 'NV003','PB01')
	
into NHANVIEN
values ('NV005', 'Nguyễn Mạnh Hùng','Nam',TO_DATE('03/04/1985','dd/mm/yyyy'),'95 Bà Rịa, Vũng Tàu',123157632,'1800','200', 'Nhân viên', 'NV002','PB01')
	
into NHANVIEN
values ('NV006', 'Trần Thanh Tâm','Nam',TO_DATE('05/04/1975','dd/mm/yyyy'),'34 Mai Thị Lựu, Tp HCM',123109832,'1200','200', 'Nhân viên', 'NV003','PB01')
	
into NHANVIEN
values ('NV007', 'Trần Hồng Quang','Nam',TO_DATE('09/01/1981','dd/mm/yyyy'),'80 Lê Hồng Phong, Tp HCM',125609832,'2500','200', 'Nhân viên', 'NV003','PB01')
	
into NHANVIEN
values ('NV008', 'Phạm Văn Vinh','Nữ',TO_DATE('01/01/1999','dd/mm/yyyy'),'5 Trưng Vương, Hà Nội',125609832,'2200','200', 'Nhân viên', 'NV002','PB01')
	
into NHANVIEN
values ('NV009', 'Đinh Văn Hoàng','Nam',TO_DATE('02/01/1971','dd/mm/yyyy'),'119 Cống Quỳnh, Tp HCM',123456789,'3300','200', 'Trưởng phòng', null,'PB02')
	
into NHANVIEN
values ('NV010', 'Nguyễn Thanh Minh','Nam',TO_DATE('20/08/1997','dd/mm/yyyy'),'222 Nguyễn Văn Mách, Tp HCM',123157789,'2000','200', 'QL trực tiếp', null,'PB02')
	
into NHANVIEN
values ('NV011', 'Hà Văn Nam','Nam',TO_DATE('02/11/1987','dd/mm/yyyy'),'332 Nguyễn Thái Công, Tp HCM',123157143,'2500','300', 'QL trực tiếp', null,'PB02')
	
into NHANVIEN
values ('NV012', 'Lê Bảo Ngọc','Nữ',TO_DATE('03/01/1997','dd/mm/yyyy'),'211 Hồ Văn Cường,  Tp HCM',123157789,'4300','200', 'Nhân viên', 'NV010','PB02')
	
into NHANVIEN
values ('NV013', 'Nguyễn Hùng Chung','Nam',TO_DATE('01/04/1997','dd/mm/yyyy'),'95 Bà Rịa, Long Thành',123157632,'3800','200', 'Nhân viên', 'NV011','PB02')
	
into NHANVIEN
values ('NV014', 'Trần Tâm Như','Nam',TO_DATE('05/04/1987','dd/mm/yyyy'),'156 Mai Thị Lựu, Tp HCM',123109832,'3200','200', 'Nhân viên', 'NV010','PB02')
	
into NHANVIEN
values ('NV015', 'Quách Đại Hiệp','Nam',TO_DATE('09/01/1995','dd/mm/yyyy'),'90 Lê Hồng Phong, Tp HCM',125609832,'2500','200', 'Nhân viên', 'NV011','PB02')
	
into NHANVIEN
values ('NV016', 'Hà Ánh Tuyết','Nữ',TO_DATE('01/01/1985','dd/mm/yyyy'),'10 Trưng Vương, Hà Nội',125609832,'2200','200', 'Nhân viên', 'NV010','PB02')
	
into NHANVIEN
values ('NV017', 'Lê Văn Hải','Nam',TO_DATE('01/11/1960','dd/mm/yyyy'),'119 Cống Quỳnh, Bình Dương',373456789,'3000','200', 'Nhân viên', 'NV010','PB02')
	
into NHANVIEN
values ('NV018', 'Nguyễn Hải Châu','Nam',TO_DATE('26/08/2000','dd/mm/yyyy'),'222 Nguyễn Văn Cừ, Bình Dương',373157789,'4000','200', 'Nhân viên', 'NV010','PB02')
	
into NHANVIEN
values ('NV019', 'Bùi Minh Long','Nam',TO_DATE('3/11/1974','dd/mm/yyyy'),'332 Nguyễn Thái Học, Bình Dương',373157143,'2500','200', 'Nhân viên', 'NV010','PB02')
	
into NHANVIEN
values ('NV020', 'Lê Thị Quỳnh','Nữ',TO_DATE('02/09/1982','dd/mm/yyyy'),'291 Hồ Văn Huê,  Bình Dương',373157789,'4300','500', 'Trưởng phòng', null,'PB03')
	
into NHANVIEN
values ('NV021', 'Nguyễn Mạnh Cường','Nam',TO_DATE('03/08/1985','dd/mm/yyyy'),'95 Bà Rịa, Gia Lai',373157632,'2000','400', 'Tài chính', null,'PB03')
	
into NHANVIEN
values ('NV022', 'Trần Thanh Hà','Nam',TO_DATE('05/07/1995','dd/mm/yyyy'),'34 Mai Thị Lựu, Bình Dương',373109832,'2200','500', 'Tài chính', null,'PB03')
	
into NHANVIEN
values ('NV023', 'Trần Hồng Bàng','Nam',TO_DATE('09/05/1981','dd/mm/yyyy'),'80 Lê Hồng Phong, Bình Dương',375609832,'3000','100', 'Trưởng phòng',null,'PB04')
	
into NHANVIEN
values ('NV024', 'Phạm Văn Vũ','Nữ',TO_DATE('01/01/1989','dd/mm/yyyy'),'5 Trưng Vương, Hà Nội',375609832,'1800','200', 'Nhân sự',null,'PB04')
	
into NHANVIEN
values ('NV025', 'Đinh Ngọc Hoàng','Nam',TO_DATE('02/01/1998','dd/mm/yyyy'),'119 Cống Quỳnh, Bình Dương',373412789,'1500','200', 'Nhân sự', null,'PB04')
	
into NHANVIEN
values ('NV026', 'Nguyễn Hải Minh','Nam',TO_DATE('20/08/1996','dd/mm/yyyy'),'222 Nguyễn Văn Mách, Bình Dương',373150089,'2000','200', 'Tài chính', null,'PB03')
	
into NHANVIEN
values ('NV027', 'Hà Hoàng Nam','Nam',TO_DATE('02/11/1989','dd/mm/yyyy'),'332 Nguyễn Thái Công, Bình Dương',373157103,'1500','200','Nhân viên','NV002','PB01')
	
into NHANVIEN
values ('NV028', 'Nguyễn Ngọc Bích','Nữ',TO_DATE('03/01/1977','dd/mm/yyyy'),'211 Hồ Văn Cường,  Bình Dương',373157709,'2300','200', 'Trưởng đề án','NV010','PB02')
	
into NHANVIEN
values ('NV029', 'Nguyễn Hải Minh','Nam',TO_DATE('01/04/1999','dd/mm/yyyy'),'95 Bà Rịa, Long Thành',373157637,'3800','200', 'Trưởng đề án', 'NV003','PB01')
	
into NHANVIEN
values ('NV030', 'Trần Đại Phong','Nam',TO_DATE('05/04/1989','dd/mm/yyyy'),'156 Mai Thị Lựu, Bình Dương',373109831,'2000','200', 'QL trực tiếp', null,'PB02')
	
into NHANVIEN
values ('NV031', 'Quách Văn Tỉnh','Nam',TO_DATE('09/01/1999','dd/mm/yyyy'),'90 Lê Hồng Phong, Bình Dương',375609832,'1000','200', 'Nhân viên', 'NV030','PB02')
	
into NHANVIEN
values ('NV032', 'Hà Hồi Hạnh','Nữ',TO_DATE('01/01/1985','dd/mm/yyyy'),'10 Trưng Vương, Hà Nội',375609832,'1600','120', 'Nhân viên', 'NV030','PB02')
	
into NHANVIEN
values ('NV033', 'Hoàng Minh Tiến','Nam',TO_DATE('01/04/1979','dd/mm/yyyy'),'95 Bà Rịa, Đà Nẵng',373157637,'3800','200', 'Trưởng đề án', 'NV003','PB04')

SELECT * FROM dual;
/



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
/
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
      ELSIF usr.VaiTro = 'Trưởng đề án' THEN
         execute immediate('GRANT TRUONGDEAN TO ' || usr.MANV);
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
CREATE OR REPLACE VIEW LayVaiTro AS
SELECT VAITRO 
FROM NHANVIEN 
WHERE MANV = SYS_CONTEXT('USERENV', 'SESSION_USER');
/
GRANT SELECT ON LayVaiTro TO NHANVIEN;
/
------------------------------------------------------------------------------------
------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------
-----------------------------------CHÍNH SÁCH: CS#1-----------------------------------
------------------------------------------------------------------------------------
------------------------------------------------------------------------------------
--NhanVien quyền 1: xem thông tin cá nhân của chính mình
create or replace view NV_XemThongTinChinhMinh
as
    select* from NhanVien
    where MaNV= SYS_CONTEXT('USERENV', 'SESSION_USER');
/

--NhanVien quyen 2: xem thong tin phan cong cua chinh minh
create or replace view NV_XemThongTinPhanCong
as
    select* from PhanCong
    where MaNV= SYS_CONTEXT('USERENV', 'SESSION_USER');
--NhanVien quyen 3: 

/
--Nhan vien update thong tin NGAYSINH, DIACHI,SODT cua chinh minh
CREATE OR REPLACE PROCEDURE NV_SUATHONGTIN(
    NGAYSINH_ IN DATE,
    DIACHI_ IN VARCHAR2,
    SODT_ IN NUMBER
)
IS
BEGIN
    UPDATE ATBM_ADMIN.NV_XemThongTinChinhMinh
    SET  NGAYSINH=NGAYSINH_,DIACHI=DIACHI_,SODT=SODT_
    WHERE MANV = SYS_CONTEXT('USERENV', 'SESSION_USER') ;
    COMMIT;
END;
/
--NhanVien quyen 4: xem tat ca phong ban
create or replace view NV_XemThongTinPhongBan
as
    select* from PhongBan;
--NhanVien quyen 5: xem tat ca
/
create or replace view NV_XemThongTinDeAn
as
    select* from DeAn;
/


--Grant cac quyen cho role NHANVIEN
grant select,update On NV_XemThongTinChinhMinh to NhanVien;
grant select On NV_XemThongTinPhanCong to NhanVien;
grant select On NV_XemThongTinPhongBan to NhanVien;
grant select On NV_XemThongTinDeAn to NhanVien;
grant execute On NV_SUATHONGTIN to NhanVien;
/
---------------------- CHÍNH SÁCH #2 ----------------------
---------------------- CHÍNH SÁCH #2 ----------------------
---------------------- CHÍNH SÁCH #2 ----------------------
---------------------- CHÍNH SÁCH #2 ----------------------
---------------------- CHÍNH SÁCH #2 ----------------------
--cs2
--Q có quyền như là một nhân viên thông thường (vai trò “Nhân viên”). Ngoài ra, với các dòng
--dữ liệu trong quan hệ NHANVIEN liên quan đến các nhân viên N mà Q quản lý trực tiếp thì
--Q được xem tất cả các thuộc tính, trừ thuộc tính LUONG và PHUCAP.

create or replace view QL_XEMNHANVIEN
as
    select 	MANV,
    TENNV,
	PHAI,
    NGAYSINH,
	DIACHI,
    SODT, DECODE(MANV,sys_context('USERENV', 'CURRENT_USER'),luong,NULL) LUONG ,
    DECODE(MANV,sys_context('USERENV', 'CURRENT_USER'),PHUCAP,NULL) PHUCAP, 
    VAITRO ,
	MANQL ,
	PHG
    from nhanvien
    where MANV = sys_context('USERENV', 'CURRENT_USER') 
        or MANQL = (select MANV
                            from nhanvien
                            where MANV = sys_context('USERENV', 'CURRENT_USER'));
/

--Có thể xem các dòng trong quan hệ PHANCONG liên quan đến chính Q và các nhân viên N
--được quản lý trực tiếp bởi Q.
create or replace view QL_XEMPHANCONG
as
    select 	MANV,
	MADA ,
	THOIGIAN
    from PHANCONG
    where MANV = sys_context('USERENV', 'CURRENT_USER') 
        or MANV in (select MANV
                            from nhanvien
                            where MANQL = sys_context('USERENV', 'CURRENT_USER'));
/
--alter session set "_ORACLE_SCRIPT"=true;
--GAN VIEW CHO ROLE
GRANT SELECT ON QL_XEMNHANVIEN TO QLTRUCTIEP;
GRANT SELECT ON QL_XEMPHANCONG TO QLTRUCTIEP;
/

---------------------- Chính sách #3 Trưởng Phòng ----------------------
---------------------- Chính sách #3 Trưởng Phòng ----------------------
---------------------- Chính sách #3 Trưởng Phòng ----------------------
---------------------- Chính sách #3 Trưởng Phòng ----------------------
---------------------- Chính sách #3 Trưởng Phòng ----------------------
-- T có quyền như là một nhân viên thông thường (vai trò “Nhân viên”). Ngoài ra, với các dòng trong quan hệ NHANVIEN liên quan đến các nhân viên thuộc phòng ban mà T làm trưởng phòng thì T có quyền xem tất cả các thuộc tính, trừ thuộc tính LUONG và PHUCAP.
CREATE VIEW TP_NHANVIEN AS
SELECT MANV, TENNV,	PHAI, NGAYSINH,	DIACHI, SODT, DECODE(MANV,SYS_CONTEXT('USERENV', 'SESSION_USER'),LUONG,NULL) LUONG, DECODE(MANV,SYS_CONTEXT('USERENV', 'SESSION_USER'),PHUCAP,NULL) PHUCAP, VAITRO,	MANQL, PHG
FROM NHANVIEN 
WHERE PHG = (select PHG from NHANVIEN where MANV =  SYS_CONTEXT('USERENV', 'SESSION_USER'));
/
grant select on ATBM_ADMIN.TP_NHANVIEN to TRUONGPHONG
/
-- Có thể thêm, xóa, cập nhật trên quan hệ PHANCONG liên quan đến các nhân viên thuộc phòng ban mà T làm trưởng phòng.
CREATE or replace VIEW TP_PHANCONG AS
SELECT *
FROM PHANCONG 
WHERE MANV in (select MANV from NHANVIEN where PHG = (select PHG from NHANVIEN where MANV =  SYS_CONTEXT('USERENV', 'SESSION_USER')));
/
grant select, update, DELETE on TP_PHANCONG to TRUONGPHONG;
/
CREATE OR REPLACE PROCEDURE TP_ThemPhanCong(
    MaNV_ IN VARCHAR2,
    MaDA_ IN VARCHAR2,
    ThoiGian_ IN DATE
)
IS
BEGIN
    INSERT INTO ATBM_ADMIN.TP_PHANCONG (MANV, MADA, THOIGIAN)
    VALUES (MaNV_, MaDA_, ThoiGian_);
    COMMIT;
END;
/
CREATE OR REPLACE PROCEDURE TP_SuaPhanCong(
    MaNV_ IN VARCHAR2,
    MaDA_ IN VARCHAR2,
    ThoiGian_ IN DATE
)
IS
BEGIN
    UPDATE ATBM_ADMIN.TP_PHANCONG
    SET THOIGIAN = ThoiGian_
    WHERE MANV = MaNV_ AND MADA = MaDA_;
    COMMIT;
END;
/
create or replace PROCEDURE TP_XoaPhanCong(
    MaNV_ in varchar2,
    MaDA_ in VARCHAR2
)
IS
BEGIN
    delete from ATBM_ADMIN.TP_PHANCONG
    where MANV = MaNV_ and MADA = MaDA_;
END;
/
grant EXECUTE ON TP_ThemPhanCong to TRUONGPHONG;
grant EXECUTE ON TP_SuaPhanCong to TRUONGPHONG;
grant EXECUTE ON TP_XoaPhanCong to TRUONGPHONG;
/
------------------------------------------------------------------------------------
------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------
-----------------------------------CHÍNH SÁCH: CS#4: TÀI CHÍNH------------------------
------------------------------------------------------------------------------------
------------------------------------------------------------------------------------
CREATE OR REPLACE VIEW TC_XEMNHANVIEN AS
SELECT *
FROM ATBM_ADMIN.NHANVIEN ;
/
CREATE OR REPLACE VIEW TC_XEMPHANCONG AS
SELECT *
FROM ATBM_ADMIN.PHANCONG ;
/
-- Grant các quyền cho người dùng

GRANT UPDATE (LUONG, PHUCAP) ON ATBM_ADMIN.TC_XEMNHANVIEN TO TAICHINH;
/
CREATE OR REPLACE PROCEDURE TC_UPD_LUONG_PHUCAP(
  p_manv IN VARCHAR2,
  LUONGMOI IN VARCHAR2,
  PHUCAPMOI IN VARCHAR2
) AS
  v_count NUMBER;
BEGIN
    -- Kiểm tra sự tồn tại của MANV trong bảng TC_XEMNHANVIEN
    SELECT COUNT(*) INTO v_count
    FROM ATBM_ADMIN.TC_XEMNHANVIEN
    WHERE MANV = p_manv;

    IF v_count = 0 THEN
        -- Thêm bản ghi mới nếu MANV không tồn tại
        INSERT INTO ATBM_ADMIN.TC_XEMNHANVIEN (MANV, LUONG, PHUCAP)
        VALUES (p_manv, LUONGMOI, PHUCAPMOI);
    ELSE
        -- Cập nhật LUONG và PHUCAP nếu MANV đã tồn tại
        UPDATE ATBM_ADMIN.TC_XEMNHANVIEN
        SET LUONG = LUONGMOI,
            PHUCAP = PHUCAPMOI
        WHERE MANV = p_manv;
    END IF;

    COMMIT;
END;

/
GRANT EXECUTE ON TC_UPD_LUONG_PHUCAP TO TAICHINH;
GRANT SELECT ON ATBM_ADMIN.TC_XEMNHANVIEN TO TAICHINH;
GRANT SELECT ON ATBM_ADMIN.TC_XEMPHANCONG TO TAICHINH;
/
CREATE OR REPLACE FUNCTION keys_access_predicate (
    schema_name IN VARCHAR2,
    object_name IN VARCHAR2
) RETURN VARCHAR2
IS
    predicate VARCHAR2(4000);
    role_count NUMBER;
BEGIN
    SELECT COUNT(*)
    INTO role_count
    FROM ATBM_ADMIN.TC_XEMNHANVIEN
    WHERE MANV = SYS_CONTEXT('USERENV', 'SESSION_USER')
    AND VAITRO = 'Tài chính';

    IF role_count = 1 THEN
        predicate := '1 = 1'; -- Cho phép truy cập toàn bộ thông tin cho vai trò TAICHINH
    ELSE
        predicate := 'MANV = SYS_CONTEXT(''USERENV'', ''SESSION_USER'')'; -- Chỉ cho phép truy cập thông tin của riêng người dùng
    END IF;
    
    RETURN predicate;
END;
/

DECLARE
    v_policy_exists NUMBER;
BEGIN
    -- Kiểm tra xem policy đã tồn tại hay chưa
    SELECT COUNT(*)
    INTO v_policy_exists
    FROM DBA_POLICIES
    WHERE object_owner = 'ATBM_ADMIN'
      AND object_name = 'COUPLE_OF_KEYS'
      AND policy_name = 'COUPLE_OF_KEYS_POLICY';

    -- Nếu policy đã tồn tại, xóa nó đi trước khi tạo lại
    IF v_policy_exists > 0 THEN
        EXECUTE IMMEDIATE 'BEGIN DBMS_RLS.DROP_POLICY(
            object_schema   => ''ATBM_ADMIN'',
            object_name     => ''COUPLE_OF_KEYS'',
            policy_name     => ''couple_of_keys_policy''
        ); END;';
    END IF;

    -- Tạo policy mới
    DBMS_RLS.ADD_POLICY(
        object_schema   => 'ATBM_ADMIN',
        object_name     => 'COUPLE_OF_KEYS',
        policy_name     => 'couple_of_keys_policy',
        policy_function => 'keys_access_predicate',
        statement_types => 'SELECT',
        update_check    => FALSE,
        enable          => TRUE
    );
    
    DBMS_OUTPUT.PUT_LINE('Policy has been created successfully.');
EXCEPTION
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Error occurred: ' || SQLERRM);
END;
/

CREATE OR REPLACE PROCEDURE manage_couple_of_keys (
    p_MANV IN COUPLE_OF_KEYS.MANV%TYPE,
    p_public_key IN COUPLE_OF_KEYS.public_key%TYPE,
    p_private_key IN COUPLE_OF_KEYS.private_key%TYPE
)
IS
    v_count NUMBER;
BEGIN
    -- Kiểm tra xem MANV đã tồn tại trong bảng hay chưa
    SELECT COUNT(*)
    INTO v_count
    FROM COUPLE_OF_KEYS
    WHERE MANV = p_MANV;

    IF v_count > 0 THEN
        -- Nếu MANV đã tồn tại, thực hiện cập nhật dữ liệu
        UPDATE COUPLE_OF_KEYS
        SET public_key = p_public_key,
            private_key = p_private_key
        WHERE MANV = p_MANV;
        
        DBMS_OUTPUT.PUT_LINE('Data has been updated for MANV: ' || p_MANV);
    ELSE
        -- Nếu MANV chưa tồn tại, thực hiện chèn dữ liệu mới
        INSERT INTO COUPLE_OF_KEYS (MANV, public_key, private_key)
        VALUES (p_MANV, p_public_key, p_private_key);
        
        DBMS_OUTPUT.PUT_LINE('Data has been inserted for MANV: ' || p_MANV);
    END IF;
    
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error occurred: ' || SQLERRM);
END;
/
GRANT EXECUTE ON ATBM_ADMIN.manage_couple_of_keys TO TAICHINH;
/

GRANT SELECT ON COUPLE_OF_KEYS TO NHANVIEN;
/
---------------------- CHÍNH SÁCH #5 ----------------------
---------------------- CHÍNH SÁCH #5 ----------------------
---------------------- CHÍNH SÁCH #5 ----------------------
---------------------- CHÍNH SÁCH #5 ----------------------
--Thêm, cập nhật dữ liệu trong quan hệ NHANVIEN với giá trị các trường LUONG, PHUCAP
--là mang giá trị mặc định là NULL, không được xem LUONG, PHUCAP của người khác và
--không được cập nhật trên các trường LUONG, PHUCAP.
create or replace view NS_XEMNHANVIEN
as
    select 	MANV,
    TENNV,
	PHAI,
    NGAYSINH,
	DIACHI,
    SODT, 
    DECODE(MANV,sys_context('USERENV', 'CURRENT_USER'),luong,NULL) LUONG ,
    DECODE(MANV,sys_context('USERENV', 'CURRENT_USER'),PHUCAP,NULL) PHUCAP, 
    VAITRO ,
	MANQL ,
	PHG
    from nhanvien
/
create or replace view NS_CNNHANVIEN
as
    select 	MANV,
    TENNV,
	PHAI,
    NGAYSINH,
	DIACHI,
    SODT,
    VAITRO ,
	MANQL ,
	PHG
    from nhanvien
/
grant SELECT on NS_XEMNHANVIEN to NHANSU;
grant SELECT on NS_CNNHANVIEN to NHANSU;
/
--Được quyền thêm, cập nhật trên quan hệ PHONGBAN.
grant select on PHONGBAN to NHANSU;
/
CREATE OR REPLACE PROCEDURE NS_THEM_PHONGBAN (
    P_MAPHG     IN PHONGBAN.MAPHG%TYPE,
    P_TENPHG    IN PHONGBAN.TENPHG%TYPE,
    P_TRPHG     IN PHONGBAN.TRPHG%TYPE
) AS
BEGIN
    -- Thêm mới PHONGBAN
    INSERT INTO PHONGBAN (MAPHG, TENPHG, TRPHG)
    VALUES (P_MAPHG, P_TENPHG, P_TRPHG);
END;
/
CREATE OR REPLACE PROCEDURE NS_SUA_PHONGBAN (
    P_MAPHG     IN PHONGBAN.MAPHG%TYPE,
    P_TENPHG    IN PHONGBAN.TENPHG%TYPE,
    P_TRPHG     IN PHONGBAN.TRPHG%TYPE
) AS
BEGIN
    -- Cập nhật PHONGBAN
    UPDATE PHONGBAN
    SET TENPHG = P_TENPHG,
        TRPHG = P_TRPHG
    WHERE MAPHG = P_MAPHG;
END;
/
begin ATBM_ADMIN.NS_THEM_NHANVIEN('NV056','Lê Quỳnh Như','Nữ',TO_DATE('01/02/1997','dd/mm/yyyy'),'291 Hồ Văn Huê,  Tp HCM','0123157789','Nhân viên','NV003','PB01'); end;
CREATE OR REPLACE PROCEDURE NS_THEM_NHANVIEN (
    P_MANV      IN NHANVIEN.MANV%TYPE,
    P_TENNV     IN NHANVIEN.TENNV%TYPE,
    P_PHAI      IN NHANVIEN.PHAI%TYPE,
    P_NGAYSINH  IN NHANVIEN.NGAYSINH%TYPE,
    P_DIACHI    IN NHANVIEN.DIACHI%TYPE,
    P_SODT      IN NHANVIEN.SODT%TYPE,
    P_VAITRO    IN NHANVIEN.VAITRO%TYPE,
    P_MANQL     IN NHANVIEN.MANQL%TYPE,
    P_PHG       IN NHANVIEN.PHG%TYPE
) AS
BEGIN
    -- Thêm mới NHANVIEN
    INSERT INTO NHANVIEN (MANV, TENNV, PHAI, NGAYSINH, DIACHI, SODT, LUONG, PHUCAP, VAITRO, MANQL, PHG)
    VALUES (P_MANV, P_TENNV, P_PHAI, P_NGAYSINH, P_DIACHI, P_SODT, NULL, NULL, P_VAITRO, P_MANQL, P_PHG);
    EXECUTE IMMEDIATE 'ALTER SESSION SET "_ORACLE_SCRIPT" = TRUE';
       execute immediate('CREATE USER ' || P_MANV || ' IDENTIFIED BY 1 DEFAULT TABLESPACE DA_ATBM');
      execute immediate('GRANT CREATE SESSION TO ' || P_MANV);
      execute immediate('GRANT NHANVIEN TO ' || P_MANV);
      IF P_VAITRO = 'Trưởng phòng' THEN
         execute immediate('GRANT TRUONGPHONG TO ' || P_MANV);
      ELSIF P_VAITRO = 'QL trực tiếp' THEN
         execute immediate('GRANT QLTRUCTIEP TO ' || P_MANV);
      ELSIF P_VAITRO = 'Tài chính' THEN
         execute immediate('GRANT TAICHINH TO ' || P_MANV);
      ELSIF P_VAITRO = 'Nhân sự' THEN
         execute immediate('GRANT NHANSU TO ' || P_MANV);
      END IF;
      COMMIT;
END;
/
CREATE OR REPLACE PROCEDURE NS_SUA_NHANVIEN (
    P_MANV      IN NHANVIEN.MANV%TYPE,
    P_TENNV     IN NHANVIEN.TENNV%TYPE,
    P_PHAI      IN NHANVIEN.PHAI%TYPE,
    P_NGAYSINH  IN NHANVIEN.NGAYSINH%TYPE,
    P_DIACHI    IN NHANVIEN.DIACHI%TYPE,
    P_SODT      IN NHANVIEN.SODT%TYPE,
    P_VAITRO    IN NHANVIEN.VAITRO%TYPE,
    P_MANQL     IN NHANVIEN.MANQL%TYPE,
    P_PHG       IN NHANVIEN.PHG%TYPE
) AS
BEGIN
    -- Kiểm tra quyền của người dùng
    -- Ở đây bạn có thể thêm các điều kiện phù hợp để kiểm tra quyền của người dùng trước khi cho phép cập nhật

    -- Cập nhật NHANVIEN
    UPDATE NHANVIEN
    SET TENNV = P_TENNV,
        PHAI = P_PHAI,
        NGAYSINH = P_NGAYSINH,
        DIACHI = P_DIACHI,
        SODT = P_SODT,
        VAITRO = P_VAITRO,
        MANQL = P_MANQL,
        PHG = P_PHG
    WHERE MANV = P_MANV;
END;
/
grant EXECUTE on NS_THEM_PHONGBAN to NHANSU;
grant EXECUTE on NS_SUA_PHONGBAN to NHANSU;
grant EXECUTE on NS_THEM_NHANVIEN to NHANSU;
grant EXECUTE on NS_SUA_NHANVIEN to NHANSU;
/


------------------------------------------------------------------------------------
------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------
-----------------------------------CHÍNH SÁCH: CS#6: TRƯỞNG ĐỀ ÁN-----------------------------------
------------------------------------------------------------------------------------
------------------------------------------------------------------------------------

--Thêm đề án
CREATE OR REPLACE PROCEDURE TRGDA_THEMDEAN(
    MADA_ IN VARCHAR2,
    TENDA_ IN VARCHAR2,
    NGAYBD_ IN DATE,
    PHONG_ IN VARCHAR2
)
AS
BEGIN
    INSERT INTO NV_XemThongTinDeAn (MADA, TENDA, NGAYBD, PHONG)
    VALUES (MADA_, TENDA_, NGAYBD_, PHONG_);
    COMMIT;
END;
/
--Xóa đề án
CREATE OR REPLACE PROCEDURE TRGDA_XOADA(MADA_ IN VARCHAR2) 
AS
BEGIN
    DELETE FROM NV_XemThongTinDeAn WHERE MADA = MADA_;
    COMMIT;
END;
/
--Sửa đề án
CREATE OR REPLACE PROCEDURE TRGDA_UPDATEDA(MADA_ IN VARCHAR2, TENDA_ IN VARCHAR2, NGAYBD_ IN DATE, PHONG_ IN VARCHAR2) 
AS
BEGIN
    UPDATE NV_XemThongTinDeAn
    SET TENDA = TENDA_, NGAYBD = NGAYBD_, PHONG = PHONG_
    WHERE MADA = MADA_;
    COMMIT;
END;
/
CREATE OR REPLACE VIEW TRGDA_XEMPHANCONG
AS
    SELECT distinct MADA FROM PHANCONG;

GRANT INSERT, DELETE, UPDATE ON NV_XemThongTinDeAn TO TRUONGDEAN;
GRANT SELECT ON TRGDA_XEMPHANCONG TO TRUONGDEAN;
GRANT EXECUTE ON TRGDA_THEMDEAN TO TRUONGDEAN;
GRANT EXECUTE ON TRGDA_UPDATEDA TO TRUONGDEAN;
GRANT EXECUTE ON TRGDA_XOADA TO TRUONGDEAN;
/
--------------------------------------------ADMIN---------------------------------------
--Thêm nhân viên của ADMIN
CREATE OR REPLACE PROCEDURE THEM_NHANVIEN (
    P_MANV      IN NHANVIEN.MANV%TYPE,
    P_PASSWORD      varchar,
    P_VAITRO    IN NHANVIEN.VAITRO%TYPE
) AS
BEGIN
    -- Thêm mới NHANVIEN
    INSERT INTO NHANVIEN (MANV, TENNV, PHAI, NGAYSINH, DIACHI, SODT, LUONG, PHUCAP, VAITRO, MANQL, PHG)
    VALUES (P_MANV, null, null, null, null, null, NULL, NULL, P_VAITRO, null, null);
    EXECUTE IMMEDIATE 'ALTER SESSION SET "_ORACLE_SCRIPT" = TRUE';
       execute immediate('CREATE USER ' || P_MANV || ' IDENTIFIED BY ' ||  P_PASSWORD ||' DEFAULT TABLESPACE DA_ATBM');
      execute immediate('GRANT CREATE SESSION TO ' || P_MANV);
      execute immediate('GRANT NHANVIEN TO ' || P_MANV);
      IF P_VAITRO = 'Trưởng phòng' THEN
         execute immediate('GRANT TRUONGPHONG TO ' || P_MANV);
      ELSIF P_VAITRO = 'QL trực tiếp' THEN
         execute immediate('GRANT QLTRUCTIEP TO ' || P_MANV);
      ELSIF P_VAITRO = 'Tài chính' THEN
         execute immediate('GRANT TAICHINH TO ' || P_MANV);
      ELSIF P_VAITRO = 'Nhân sự' THEN
         execute immediate('GRANT NHANSU TO ' || P_MANV);
      END IF;
      COMMIT;
END;



