/*
-----------------
--TAO TABLESPACE
CREATE TABLESPACE DA_ATBM
   DATAFILE 'DA_ATBM_data.dbf' 
   SIZE 500m;

--TAO TAI KHOAN ADMIN
ALTER SESSION SET "_ORACLE_SCRIPT" = TRUE
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
create table NHANVIEN 
(	
	MANV 	varchar2(10),
    TENNV 	varchar2(30),
	PHAI 	varchar2(5),
    NGAYSINH 	date,
	DIACHI 	varchar2(50),
    SODT number(10),
	LUONG 	number(10,2),
    PHUCAP number(7,2),
    VAITRO varchar2(20),
	MANQL 	varchar2(10),
	PHG 	number(5),
	CONSTRAINT pk_nv primary key (MANV)	
)TABLESPACE DA_ATBM;
/
create table PHONGBAN 
(
	TENPHG 	varchar2(30),
	MAPHG 	number(5),
	TRPHG 	varchar2(30),
	CONSTRAINT pk_pb primary key (MAPHG)
)TABLESPACE DA_ATBM;
/
create table DEAN 
(
	MADA number(5),
    TENDA varchar2(30),
	NGAYBD date,
	PHONG number(5),
	CONSTRAINT pk_da primary key (MADA)
)TABLESPACE DA_ATBM;
/
create table PHANCONG 
(
	MANV	varchar2(10),
	MADA 	number(5),
	THOIGIAN	number(5),
	CONSTRAINT pk_pc primary key (MANV, MADA)
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
--NHAP DU LIEU MAU
INSERT ALL
into PHONGBAN
values('Nghiên cứu', 5, null)
into PHONGBAN
values('Điều hành', 4, null)
into PHONGBAN
values('Quản lý', 1, null)
SELECT * FROM dual;

INSERT ALL
into NHANVIEN 
values ('TP01', 'Đinh Bá Tiến','Nam',TO_DATE('02/11/1960','dd/mm/yyyy'),'119 Cống Quỳnh, Tp HCM', 0123456789, 30000, 2000, 'Trưởng phòng', null,5)

into NHANVIEN 
values ('GD01', 'Nguyễn Thanh Tùng','Nam',TO_DATE('20/08/1962','dd/mm/yyyy'),'222 Nguyễn Văn Cừ, Tp HCM', 0123157789, 40000, 2000, 'Ban giám đốc', null,4)

into NHANVIEN 
values ('QL01', 'Bùi Ngọc Hằng','Nam',TO_DATE('3/11/1954','dd/mm/yyyy'),'332 Nguyễn Thái Học, Tp HCM', 0123157143, 25000, 2000, 'QL trực tiếp', null,4)


into NHANVIEN 
values ('TC01', 'Lê Quỳnh Như','Nữ',TO_DATE('02/01/1967','dd/mm/yyyy'),'291 Hồ Văn Huê,  Tp HCM', 0123157789, 43000, 2000, 'Tài chính', null,4)

into NHANVIEN 
values ('NS01', 'Nguyễn Mạnh Hùng','Nam',TO_DATE('03/04/1967','dd/mm/yyyy'),'95 Bà Rịa, Vũng Tàu', 0123157632, 38000, 2000, 'Tài chính', null,5)

into NHANVIEN 
values ('TDA01', 'Trần Thanh Tâm','Nam',TO_DATE('05/04/1957','dd/mm/yyyy'),'34 Mai Thị Lựu, Tp HCM', 0123109832, 32000, 2000, 'Trưởng đề án', null,5)

into NHANVIEN 
values ('NV01', 'Trần Hồng Quang','Nam',TO_DATE('09/01/1967','dd/mm/yyyy'),'80 Lê Hồng Phong, Tp HCM', 0125609832, 25000, 2000, 'Nhân viên', 'QL01',4)

into NHANVIEN 
values ('NV02', 'Phạm Văn Vinh','Nữ',TO_DATE('01/01/1965','dd/mm/yyyy'),'5 Trưng Vương, Hà Nội', 0125609832, 22000, 2000, 'Nhân viên', 'QL01',4)
SELECT * FROM dual;



INSERT ALL
into DEAN
values(1,'Sản phẩm X', TO_DATE('01/01/1965','dd/mm/yyyy'), 5)

into DEAN
values(2,'Sản phẩm Y', TO_DATE('01/01/1965','dd/mm/yyyy'), 5)

into DEAN
values(3,'Sản phẩm Z', TO_DATE('01/01/1965','dd/mm/yyyy'), 5)

into DEAN
values(10,'Tin học hóa', TO_DATE('01/01/1965','dd/mm/yyyy'), 4)

into DEAN
values(20, 'Cáp quang',TO_DATE('01/01/1965','dd/mm/yyyy'), 1)

into DEAN
values(30,'Đào tạo', TO_DATE('01/01/1965','dd/mm/yyyy'), 4)
SELECT * FROM dual;

INSERT ALL
into PHANCONG 
values ('NV01',	30,	5)

into PHANCONG 
values ('NV02',	30,	20)

into PHANCONG
values ('TC01',	20,	15)

into PHANCONG 
values ('TP01',	20,30)

into PHANCONG
values ('TDA01', 3,	10)

into PHANCONG
values ('NV02',	10, 10)

into PHANCONG 
values ('NV01',	20,10)

into PHANCONG 
values ('QL01',	30,	30)

into PHANCONG
values ('QL01',	10,	10)
SELECT * FROM dual;
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
CREATE OR REPLACE PROCEDURE sp_CreateUser
as
   CURSOR cur_nv IS
      SELECT MANV
      FROM NHANVIEN;
      --WHERE MANV NOT IN (SELECT USERNAME FROM ALL_USERS);
     usr VARCHAR2(30);
Begin
    OPEN cur_nv;
        execute immediate 'ALTER SESSION SET "_ORACLE_SCRIPT" = TRUE';
       LOOP
      FETCH cur_nv INTO usr;
      EXIT WHEN cur_nv%NOTFOUND;
        execute immediate('Create user '|| usr||' identified by '||'1'|| ' DEFAULT TABLESPACE DA_ATBM');
        execute immediate('grant create session to '||usr);
        END LOOP;
   CLOSE cur_nv;
End;
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
