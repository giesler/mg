select * from versions
go

update versions
set md5=0x876b29dd20d7946625b189d2d189c46f, length=332262, required=1
where oldversion='0.55'