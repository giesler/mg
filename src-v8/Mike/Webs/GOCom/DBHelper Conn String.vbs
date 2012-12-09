
dim obj

set obj = CreateObject("GOCom.Version")

msgbox obj.ConnectionString, 64, "Version: " + obj.Version

set obj = nothing
