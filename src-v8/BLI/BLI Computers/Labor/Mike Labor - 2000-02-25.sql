
-- fc labor log id - 3, mike's id - 3
insert tblLaborLog (afkLLogLabor, afkLLogSM, LLogHours, LLogDate, LLogDesc)
values (3, 3, 2, '1/4/00', 'Database v3.26')
go

insert tblLaborLog (afkLLogLabor, afkLLogSM, LLogHours, LLogDate, LLogDesc)
values (3, 3, 0.5, '1/6/00', 'Database v3.27')
go

insert tblLaborLog (afkLLogLabor, afkLLogSM, LLogHours, LLogDate, LLogDesc)
values (3, 3, 4.25, '2/3/00', 'Database v3.28')
go

insert tblLaborLog (afkLLogLabor, afkLLogSM, LLogHours, LLogDate, LLogDesc)
values (3, 3, 2.5, '2/11/00', 'Database v3.29')
go

insert tblLaborLog (afkLLogLabor, afkLLogSM, LLogHours, LLogDate, LLogDesc)
values (3, 3, 2, '2/12/00', 'Database v3.30')
go

insert tblLaborLog (afkLLogLabor, afkLLogSM, LLogHours, LLogDate, LLogDesc)
values (3, 3, 7.5, '2/13/00', 'Database v3.31')
go

insert tblLaborLog (afkLLogLabor, afkLLogSM, LLogHours, LLogDate, LLogDesc)
values (3, 3, 4.5, '2/25/00', 'Database v3.32')
go

insert tblLaborLog (afkLLogLabor, afkLLogSM, LLogHours, LLogDate, LLogDesc)
values (3, 3, 0.5, '2/25/00', 'Database v3.33')
go

insert tblEmpLogins (afkEmp, LoginName) values (3, 'Mike Giesler')
go

update tblEmpAddr
set EAddrDesc = '702 Cherrywood Ct #1
Madison, WI  53715', EAddrNote = 'Madison'
where apkEAddr = 5
go

update tblEmpPhone
set EPhoneDesc = 'Madison', EPhoneNumber = '6082456412', EPhoneNote = 'Madison apartment'
where apkEPhone = 11
go
