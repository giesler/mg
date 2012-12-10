-- rfcdata structure updates

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Util_GetStoreInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Util_GetStoreInfo]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Util_ShipNameList]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Util_ShipNameList]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE dbo.sp_Util_GetStoreInfo
	@StoreNumber nvarchar(50),
	@StoreName nvarchar(50) = null
AS
SELECT Ship_Name, Ship_Address1, Ship_Address2, Ship_City, Ship_State, Ship_Zip, Ship_Attn, Ship_Phone,
	svia.Ship_Via
FROM Tbl_PO po
	LEFT OUTER JOIN Tbl_Ship_Via svia ON svia.State = po.Ship_State
WHERE po.Date_Ordered = (SELECT Max(Date_Ordered) FROM Tbl_PO WHERE Ship_StoreNo = @StoreNumber)
	and po.Ship_StoreNo = @StoreNumber and po.Ship_Name = @StoreName
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE dbo.sp_Util_ShipNameList
AS

select Ship_Name
from Tbl_PO
where Ship_Name <> '' and Ship_Name not like '%#%'

group by Ship_Name
having count(*) > 1
order by Ship_Name
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


insert ztblReleaseNotes (Date, MajorVersion, MinorVersion, Notes)
values ('1/14/2002', 3.27, 0, '- POs:  Changed to allow entering store name then store number
')
go

