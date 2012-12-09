-- fcdata update 3.25
-- - update rel notes, db version

update [Switchboard Items] 
set Argument = 'isrptMailLabels'
where SwitchboardID = 35 and ItemNumber = 3
go
