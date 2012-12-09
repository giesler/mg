-- rfcdata structure updates


ALTER TABLE dbo.Tbl_Catalog
	DROP COLUMN CustMaterial
GO



CREATE TABLE dbo.tblCatalogCustMaterial
	(
	CustMaterialID int NOT NULL IDENTITY (1, 1),
	MaterialID int NULL,
	CustID int NULL,
	CustMaterial nvarchar(50) NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.tblCatalogCustMaterial ADD CONSTRAINT
	IX_tblCatalogCustMaterial UNIQUE NONCLUSTERED 
	(
	CustMaterialID
	) ON [PRIMARY]

GO
ALTER TABLE dbo.tblCatalogCustMaterial ADD CONSTRAINT
	IX_tblCatalogCustMaterial_MaterialID_CustID UNIQUE NONCLUSTERED 
	(
	MaterialID,
	CustID
	) ON [PRIMARY]

GO
CREATE NONCLUSTERED INDEX IX_tblCatalogCustMaterial_CustID ON dbo.tblCatalogCustMaterial
	(
	CustID
	) ON [PRIMARY]
GO
ALTER TABLE dbo.tblCatalogCustMaterial ADD CONSTRAINT
	FK_tblCatalogCustMaterial_Tbl_Catalog FOREIGN KEY
	(
	MaterialID
	) REFERENCES dbo.Tbl_Catalog
	(
	apkCatalogItem
	)
GO
ALTER TABLE dbo.tblCatalogCustMaterial ADD CONSTRAINT
	FK_tblCatalogCustMaterial_tblSold FOREIGN KEY
	(
	CustID
	) REFERENCES dbo.tblSold
	(
	apkSold
	)
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Web_PO_View_Items]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Web_PO_View_Items]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE dbo.sp_Web_PO_View_Items
	(
		@SoldID int = null,
		@POID int = null
	)
AS

SET NOCOUNT ON

-- now we want to select PO items
select poi.apkPOItem POItemID, 
	poi.Material RFMaterial, cm.CustMaterial, poi.MatDescription Description, poi.Quanity Quantity, 
	poi.Unit_Price Price, poi.Unit_Quantiy Unit, 
	poi.Quanity * poi.Unit_Price Total
from Tbl_PO_Items poi
	inner join Tbl_PO po ON po.ID = poi.ID
	left outer join Tbl_Catalog cat ON poi.Material = cat.Material
	left outer join tblCatalogCustMaterial cm ON cm.MaterialID = cat.apkCatalogItem
where po.SoldID = @SoldID and poi.ID = @POID
	and cm.CustID = @SoldID

RETURN
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO




insert ztblReleaseNotes (Date, MajorVersion, MinorVersion, Notes)
values ('8/26/2001', 3.25, 0, '- Catalog:  Changed customer material number to allow multiple customer material numbers per RF material
- PO:  Changed to use new customer material numbers based on the customer chosen on the PO
- Reports:  Added ''Export'' to allow you to export reports to Excel or other formats
')
go

