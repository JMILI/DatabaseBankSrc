CREATE DATABASE bank CHARACTER SET utf8 COLLATE utf8_general_ci;
use bank;

CREATE TABLE IdCard(
	ICid int(100) auto_increment PRIMARY KEY,
	ICsex char(2) NOT NULL default '男' check (ICsex in ('男','女')),
	ICname VARCHAR(200) not null,
	ICaddress VARCHAR(200) not null
	);
alter table IdCard AUTO_INCREMENT=100001;

INSERT INTO IdCard (ICsex,ICname,ICaddress) VALUES ('男','李一','香港');
INSERT INTO IdCard (ICsex,ICname,ICaddress) VALUES ('女','李二','北京');
INSERT INTO IdCard (ICsex,ICname,ICaddress) VALUES ('男','李三','香港');
INSERT INTO IdCard (ICsex,ICname,ICaddress) VALUES ('女','李四','北京');
INSERT INTO IdCard (ICsex,ICname,ICaddress) VALUES ('男','李五','香港');
INSERT INTO IdCard (ICsex,ICname,ICaddress) VALUES ('女','李六','北京');
INSERT INTO IdCard (ICsex,ICname,ICaddress) VALUES ('男','李七','香港');
INSERT INTO IdCard (ICsex,ICname,ICaddress) VALUES ('女','李八','北京');
INSERT INTO IdCard (ICsex,ICname,ICaddress) VALUES ('男','李九','香港');
INSERT INTO IdCard (ICsex,ICname,ICaddress) VALUES ('女','李十','北京');
INSERT INTO IdCard (ICsex,ICname,ICaddress) VALUES ('男','李十一','香港');
INSERT INTO IdCard (ICsex,ICname,ICaddress) VALUES ('女','李十二','北京');
INSERT INTO IdCard (ICsex,ICname,ICaddress) VALUES ('男','李十三','香港');
INSERT INTO IdCard (ICsex,ICname,ICaddress) VALUES ('女','李十四','北京');
INSERT INTO IdCard (ICsex,ICname,ICaddress) VALUES ('男','李十五','香港');
INSERT INTO IdCard (ICsex,ICname,ICaddress) VALUES ('女','李十六','北京');



CREATE TABLE fixBalances(
	Fid int(100) auto_increment PRIMARY KEY,
	Fcid int(100) not null,
	Fyear int(50) NULL DEFAULT '0',
	FfixBalanceRate double(200,5) NULL DEFAULT '0.00000',
	FfixBalance double(200,3) NULL DEFAULT '0.00000',
	FbusinessTime datetime NULL DEFAULT CURRENT_TIMESTAMP,
-- 0代表ATM操作，1代表是储户操作，Rmid有值（管理员账号）时代表柜台管理员操作
	Fmid int(100) NULL DEFAULT '0'
	);
alter table fixBalances AUTO_INCREMENT=50001;

INSERT INTO fixBalances (Fyear,Fcid,FfixBalanceRate,FfixBalance,Fmid) VALUES (5 ,20002,0.0035,1000000.000,30002);
INSERT INTO fixBalances (Fyear,Fcid,FfixBalanceRate,FfixBalance,Fmid) VALUES (2 ,20002,0.00335,1000000.000,30002);
INSERT INTO fixBalances (Fyear,Fcid,FfixBalanceRate,FfixBalance,Fmid) VALUES (3 ,20002,0.0030,1000000.000,30002);
INSERT INTO fixBalances (Fyear,Fcid,FfixBalanceRate,FfixBalance,Fmid) VALUES (2 ,20002,0.00335,1000000.000,30002);
INSERT INTO fixBalances (Fyear,Fcid,FfixBalanceRate,FfixBalance,Fmid) VALUES (23 ,20002,0.0035,1000000.000,30002);
INSERT INTO fixBalances (Fyear,Fcid,FfixBalanceRate,FfixBalance,Fmid) VALUES (20 ,20002,0.00335,1000000.000,30002);
INSERT INTO fixBalances (Fyear,Fcid,FfixBalanceRate,FfixBalance,Fmid) VALUES (3 ,20002,0.0030,1000000.000,30001);
INSERT INTO fixBalances (Fyear,Fcid,FfixBalanceRate,FfixBalance,Fmid) VALUES (2 ,20001,0.00335,1000000.000,30002);


CREATE TABLE cards(
	Cid int(100) auto_increment PRIMARY KEY,
	Cpassword VARCHAR(200) not null,
	Cuid int(100) not null,
	Cicid int(100) not null,
	CflowBalance DOUBLE(200,5) NULL DEFAULT '0.00000',
	CflowBalanceRate DOUBLE(200,5) NULL DEFAULT '0.00000'
	);
alter table cards AUTO_INCREMENT=20001;
-- 是不是需要添加身份证号
INSERT INTO cards (Cuid,Cpassword,Cicid,CflowBalance,CflowBalanceRate) VALUES (10001,'I4PH0HvOPILm2ndBeC3kFg==' ,100001,1000,0.0025);
INSERT INTO cards (Cuid,Cpassword,Cicid,CflowBalance,CflowBalanceRate) VALUES (10002,'Zt8kPUBjU9Dp22xd0CfS1g==' ,100002,1000,0.0026);
INSERT INTO cards (Cuid,Cpassword,Cicid) VALUES (10002,'5GP5fKa8RrG6cGR04QjH4Q==',100002);
INSERT INTO cards (Cuid,Cpassword,Cicid) VALUES (10004,'6YywN/N2+lOzFMFmdm71Xg==',100002);
INSERT INTO cards (Cuid,Cpassword,Cicid,CflowBalance,CflowBalanceRate) VALUES (10002,'g4GHL6F/nctf21iAJGHEbg==' ,100002,1000,0.0025);
INSERT INTO cards (Cuid,Cpassword,Cicid,CflowBalance,CflowBalanceRate) VALUES (10002,'Krj4ZBC0873MdHaZKV61pA==' ,100002,1000,0.0026);
INSERT INTO cards (Cuid,Cpassword,Cicid,CflowBalance,CflowBalanceRate) VALUES (10002,'5MjEd9Ffcr72VlHdsixYkQ==' ,100002,1000,0.0025);
INSERT INTO cards (Cuid,Cpassword,Cicid,CflowBalance,CflowBalanceRate) VALUES (10002,'rD8ctzvIgQgweI6MaKA6Sg==' ,100002,1000,0.0026);

CREATE TABLE Depositors(
	Uid int(100)  PRIMARY KEY not null,
	Ucid int(100) NULL DEFAULT NULL,
	Uicid int(100) not null,
	Upassword VARCHAR(200)not null,
	Uidentify VARCHAR(5) NOT NULL DEFAULT '储户',
	Ustatus VARCHAR(200) NOT NULL DEFAULT '正常' check (ICsex in ('正常','冻结'))
	);

INSERT INTO Depositors (Uid,Ucid,Upassword,Uicid) VALUES (10001,20001 ,'2J86NZMcOGlWwaQCqOCZQQ==',100001);
INSERT INTO Depositors (Uid,Ucid,Upassword,Uicid) VALUES (10002,20002 ,'kQPIyCUU852DYMdDDE7lVw==',100002);
INSERT INTO Depositors (Uid,Upassword,Uicid) VALUES (10003,'9d/8ERRUsif7zfNheN/mrA==',100003);
INSERT INTO Depositors (Uid,Ucid,Upassword,Uicid) VALUES (10004,20004 ,'14OCPMYoS5KcLNjfIWfSEg==',100002);


CREATE TABLE managers(
	Mid int(100) auto_increment PRIMARY KEY,
	Mname VARCHAR(200) not null,
	Mpassword VARCHAR(200) not null,
	Midentify VARCHAR(5) NOT NULL DEFAULT '管理员'
	);
-- 主键设置初始值
alter table managers AUTO_INCREMENT=30001;

INSERT INTO managers (Mname,Mpassword) VALUES ('张一' ,'RR+7Ak0HlP/NoiWBcHQKHg==');
INSERT INTO managers (Mname,Mpassword) VALUES ('张二' ,'cF4t0gd7wG+8Xix1TnXlAA==');
INSERT INTO managers (Mname,Mpassword,Midentify) VALUES ('张三' ,'CVENUm32D0fCeX3uQiVJOQ==','行长');
INSERT INTO managers (Mname,Mpassword,Midentify) VALUES ('张四' ,'rBfYpIEmrD3dgkibNcDNMg==' ,'行长');
INSERT INTO managers (Mname,Mpassword) VALUES ('张五' ,'y2GEOk1Ueo0qGZ/oCnoa3Q==');


CREATE TABLE records(
	Rid int(100) auto_increment PRIMARY KEY,
	Ruid int(100) not null,
	Rcid int(100) not null,
	Ricid int(200) not null,
	RflowDeposit DOUBLE(200,3) NULL DEFAULT '0.00000',
	RfixDeposit DOUBLE(200,3) NULL DEFAULT '0.00000',
	Rwithdrawals DOUBLE(200,3) NULL DEFAULT '0.00000',
	RnowDateTime DateTime NULL DEFAULT CURRENT_TIMESTAMP,
-- 0代表ATM操作，1代表是储户操作，Rmid有值（管理员账号）时代表柜台管理员操作
	Rmid int(100) NULL DEFAULT '0'
	);
-- 主键设置初始值
alter table records AUTO_INCREMENT=40001;
INSERT INTO records (Ruid,Rcid,Ricid,RflowDeposit,Rmid) VALUES (10001,20001,100001,10000000.000,30002);
INSERT INTO records (Ruid,Rcid,Ricid,RflowDeposit) VALUES (10001,20001,100001,10000000.000);
INSERT INTO records (Ruid,Rcid,Ricid,RfixDeposit,Rmid) VALUES (10002,20002,100002,1000.000,30002);
INSERT INTO records (Ruid,Rcid,Ricid,RfixDeposit,Rmid) VALUES (10002,20002,100002,1000.000,30002);

INSERT INTO records (Ruid,Rcid,Ricid,Rwithdrawals) VALUES (10001,20001,100001,500.000);

INSERT INTO records (Ruid,Rcid,Ricid,RflowDeposit,Rmid) VALUES (10002,20002,100002,1000000450.000,30002);
INSERT INTO records (Ruid,Rcid,Ricid,Rwithdrawals,Rmid) VALUES (10002,20002,100002,50011254.000,30002);
INSERT INTO records (Ruid,Rcid,Ricid,RflowDeposit,Rmid) VALUES (10002,20002,100002,10000450.000,30002);
INSERT INTO records (Ruid,Rcid,Ricid,Rwithdrawals,Rmid) VALUES (10002,20002,100002,50011254.000,30002);

INSERT INTO records (Ruid,Rcid,Ricid,RflowDeposit,Rmid) VALUES (10002,20002,100002,10000450.000,30002);
INSERT INTO records (Ruid,Rcid,Ricid,Rwithdrawals,Rmid) VALUES (10002,20002,100002,50011254.000,30002);

INSERT INTO records (Ruid,Rcid,Ricid,RflowDeposit,Rmid) VALUES (10002,20002,100002,1000000450.000,30002);
INSERT INTO records (Ruid,Rcid,Ricid,Rwithdrawals,Rmid) VALUES (10002,20002,100002,500114.000,30002);

INSERT INTO records (Ruid,Rcid,Ricid,RflowDeposit,Rmid) VALUES (10002,20002,100002,1000000450.000,30002);
INSERT INTO records (Ruid,Rcid,Ricid,Rwithdrawals,Rmid) VALUES (10002,20002,100002,501154.000,30002);

INSERT INTO records (Ruid,Rcid,Ricid,RflowDeposit,Rmid) VALUES (10002,20002,100002,1000000450.000,30002);
INSERT INTO records (Ruid,Rcid,Ricid,Rwithdrawals,Rmid) VALUES (10002,20002,100002,5001254.000,30002);

INSERT INTO records (Ruid,Rcid,Ricid,RflowDeposit,Rmid) VALUES (10002,20002,100002,1000000450.000,30002);
INSERT INTO records (Ruid,Rcid,Ricid,Rwithdrawals,Rmid) VALUES (10002,20002,100002,5001124.000,30002);

INSERT INTO records (Ruid,Rcid,Ricid,RflowDeposit,Rmid) VALUES (10002,20002,100002,100000450.000,30002);
INSERT INTO records (Ruid,Rcid,Ricid,Rwithdrawals,Rmid) VALUES (10002,20002,100002,5001154.000,30002);

INSERT INTO records (Ruid,Rcid,Ricid,RflowDeposit,Rmid) VALUES (10002,20002,100002,1000000450.000,30002);
INSERT INTO records (Ruid,Rcid,Ricid,Rwithdrawals,Rmid) VALUES (10002,20002,100002,5001154.000,30002);

INSERT INTO records (Ruid,Rcid,Ricid,RflowDeposit,Rmid) VALUES (10002,20002,100002,1000000450.000,30002);
INSERT INTO records (Ruid,Rcid,Ricid,Rwithdrawals,Rmid) VALUES (10002,20002,100002,5001254.000,30002);

INSERT INTO records (Ruid,Rcid,Ricid,RflowDeposit,Rmid) VALUES (10002,20002,100002,1000000450.000,30002);
INSERT INTO records (Ruid,Rcid,Ricid,Rwithdrawals,Rmid) VALUES (10002,20002,100002,5001124.000,30002);

INSERT INTO records (Ruid,Rcid,Ricid,RflowDeposit,Rmid) VALUES (10002,20002,100002,100450.000,30002);
INSERT INTO records (Ruid,Rcid,Ricid,Rwithdrawals,Rmid) VALUES (10002,20002,100002,5001254.000,30002);

INSERT INTO records (Ruid,Rcid,Ricid,RflowDeposit,Rmid) VALUES (10002,20002,100002,1000000450.000,30002);
INSERT INTO records (Ruid,Rcid,Ricid,Rwithdrawals,Rmid) VALUES (10002,20002,100002,5054.000,30002);

INSERT INTO records (Ruid,Rcid,Ricid,RflowDeposit,Rmid) VALUES (10002,20002,100002,1000000450.000,30002);
INSERT INTO records (Ruid,Rcid,Ricid,Rwithdrawals,Rmid) VALUES (10002,20002,100002,5054.000,30002);

INSERT INTO records (Ruid,Rcid,Ricid,RflowDeposit,Rmid) VALUES (10002,20002,100002,100450.000,30002);
INSERT INTO records (Ruid,Rcid,Ricid,Rwithdrawals,Rmid) VALUES (10002,20002,100002,50011254.000,30002);

INSERT INTO records (Ruid,Rcid,Ricid,RflowDeposit,Rmid) VALUES (10002,20002,100002,1000050.000,30002);
INSERT INTO records (Ruid,Rcid,Ricid,Rwithdrawals,Rmid) VALUES (10002,20002,100002,50011254.000,30002);

INSERT INTO records (Ruid,Rcid,Ricid,RflowDeposit,Rmid) VALUES (10002,20002,100002,1000000450.000,30002);
INSERT INTO records (Ruid,Rcid,Ricid,Rwithdrawals,Rmid) VALUES (10002,20002,100002,500154.000,30002);

INSERT INTO records (Ruid,Rcid,Ricid,RflowDeposit,Rmid) VALUES (10002,20002,100002,10000450.000,30002);
INSERT INTO records (Ruid,Rcid,Ricid,Rwithdrawals,Rmid) VALUES (10002,20002,100002,500154.000,30002);




-- 活期转账：甲方：partyA,乙方：partyB,转账为同行转账，设置外键为双方cid(银行卡账号)，转账过程1.修改甲乙上方活期存款修改，2.TransferRecords表生成记录。
CREATE TABLE TransferRecords(
	Tid int(100) auto_increment PRIMARY KEY,
	TpartyACid int(100) not null,
	TpartyAName varchar(200) not null,
	TpartyBCid int(100) not null,
	TpartyBName varchar(200) not null,
	TtransferBalance DOUBLE(200,3) NULL DEFAULT '0.00000',
	TtransferTime DateTime NULL DEFAULT CURRENT_TIMESTAMP,
-- 0代表ATM操作，1代表是储户操作，Rmid有值（管理员账号）时代表柜台管理员操作,默认为储户系统转账
	Tmid int(100) NULL DEFAULT '1'
	);
alter table TransferRecords AUTO_INCREMENT=60001;
INSERT INTO TransferRecords (TpartyACid,TpartyAName,TpartyBCid,TpartyBName,TtransferBalance) VALUES (20002,'李二',20001,'李一',25000);
INSERT INTO TransferRecords (TpartyACid,TpartyAName,TpartyBCid,TpartyBName,TtransferBalance) VALUES (20001,'李一',20002,'李二',200);

alter table cards add constraint FK_Reference_1 foreign key (Cicid)
      references IdCard (ICid) on delete restrict on update restrict;

-- alter table Depositors add constraint FK_Reference_2 foreign key (Uicid)
--       references IdCard (ICid) on delete restrict on update restrict;

alter table records add constraint FK_Reference_3 foreign key (Rcid)
      references cards (Cid) on delete restrict on update restrict;

alter table records add constraint FK_Reference_4 foreign key (Ricid)
      references IdCard (ICid) on delete restrict on update restrict;

alter table fixBalances add constraint FK_Reference_5 foreign key (Fcid)
      references cards (Cid) on delete restrict on update restrict;


alter table TransferRecords add constraint FK_Reference_6 foreign key (TpartyACid)
      references cards (Cid) on delete restrict on update restrict;
alter table TransferRecords add constraint FK_Reference_7 foreign key (TpartyBCid)
      references cards (Cid) on delete restrict on update restrict;

-- 创建视图，用来检索重要信息，不必多表查找
CREATE view information(
	Irid,
	Iname,
	Iuid,
	Icid,
	Iicid,
	IflowDeposit,
	IfixDeposit,
	Iwithdrawals,
	Ioldtime,
	Imid
	)
as select distinct
records.Rid,
IdCard.ICname,
records.Ruid,
records.Rcid,
records.Ricid,
records.RflowDeposit,
records.RfixDeposit,
records.Rwithdrawals,
records.RnowDateTime,
records.Rmid
from IdCard,records
where records.Ricid = IdCard.Icid;


CREATE view DepositorAndCard(
	Dname,
	Duid,
	Dcid,
	Dicid
)
as select distinct
IdCard.ICname,
Cards.Cuid,
Cards.Cid,
IdCard.ICid
from IdCard,cards
where cards.Cicid = IdCard.ICid;



