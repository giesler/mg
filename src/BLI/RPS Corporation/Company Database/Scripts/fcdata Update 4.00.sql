-- fcdata update 3.48 / 4.00

if exists (select * from sysobjects where id = object_id(N'[dbo].[dlqfrmDealerGoals]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[dlqfrmDealerGoals]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[dlqrptDealerContractAlpha]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[dlqrptDealerContractAlpha]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[dlqrptDealerContractDate]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[dlqrptDealerContractDate]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[dlqrptDealerListAddress]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[dlqrptDealerListAddress]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[dlqrptDealerListAllDealers]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[dlqrptDealerListAllDealers]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[dlqrptDealerListFax]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[dlqrptDealerListFax]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[dlqrptDealerListPhone]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[dlqrptDealerListPhone]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[dlqrptMaillingLabels]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[dlqrptMaillingLabels]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[dlqrptMaillingLabelServicePartsMan]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[dlqrptMaillingLabelServicePartsMan]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[dlqrptMaillingService]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[dlqrptMaillingService]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[dmqrptDealerDemoReport]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[dmqrptDealerDemoReport]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[GetDbVersion]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetDbVersion]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[ldpqrptDirectMailLead]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ldpqrptDirectMailLead]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[ldqrptDirectMailLead]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ldqrptDirectMailLead]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[ldqrptLeadsReports]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ldqrptLeadsReports]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[ldqrptMaillingLabels]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ldqrptMaillingLabels]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[ldqrptMaillingLabelsDate]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ldqrptMaillingLabelsDate]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[ldqrptResponseMethod]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ldqrptResponseMethod]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[ldsprptDealerLeads]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ldsprptDealerLeads]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[ldsprptResponseMethod]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ldsprptResponseMethod]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[lsqrptStatesDealerList]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[lsqrptStatesDealerList]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[lsqrptStatesLastMailed]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[lsqrptStatesLastMailed]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[lsqrptStatesNotMailed]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[lsqrptStatesNotMailed]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[nwqrptAddressListing]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[nwqrptAddressListing]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[nwqrptMailingLabels]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[nwqrptMailingLabels]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orqrptEndUserLabels]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orqrptEndUserLabels]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptAcknowledgement]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptAcknowledgement]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptCustomerLabels]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptCustomerLabels]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptCustomerMailLabels]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptCustomerMailLabels]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptDealerSales]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptDealerSales]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptDealerSerial]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptDealerSerial]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptEndUser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptEndUser]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptEndUserDealer]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptEndUserDealer]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptEndUserState]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptEndUserState]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptMajorAccountSales]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptMajorAccountSales]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptMarginByModel]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptMarginByModel]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptOpenOrders]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptOpenOrders]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptPrepSheet]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptPrepSheet]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptProdSchedule]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptProdSchedule]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptSalesDealer]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptSalesDealer]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptSalesModel]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptSalesModel]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptSalesModelON]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptSalesModelON]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptSalesRepCommission]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptSalesRepCommission]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptServicingDealers]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptServicingDealers]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptUnregSweepers]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptUnregSweepers]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[paqrptPartListFinish]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[paqrptPartListFinish]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[paqryExportDealerParts]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[paqryExportDealerParts]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[paspfrmPartsFinish]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[paspfrmPartsFinish]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[paspfrmPartsModels]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[paspfrmPartsModels]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[paspfrmPartsOptions]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[paspfrmPartsOptions]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[paspfrmPartsSubParts]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[paspfrmPartsSubParts]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[paspfrmPartsVendors]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[paspfrmPartsVendors]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[pasprptBillOfMaterials]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[pasprptBillOfMaterials]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[pasprptDealerPartsList]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[pasprptDealerPartsList]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[pasprptListPrices]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[pasprptListPrices]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[pasprptPartFinishVendorSched]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[pasprptPartFinishVendorSched]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[pasprptPartLabels]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[pasprptPartLabels]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[pasprptProductionSchedule]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[pasprptProductionSchedule]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[pasprptRPSPartsPricing]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[pasprptRPSPartsPricing]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[pasprsubBillOfMaterialsPParts]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[pasprsubBillOfMaterialsPParts]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[ppfsubOrderDetail]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ppfsubOrderDetail]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[ppqrptPOCosts]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ppqrptPOCosts]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[ppqrptVendorRelease]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ppqrptVendorRelease]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[ppsprptCashFlow]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ppsprptCashFlow]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[ppsprptPartsOnHold]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ppsprptPartsOnHold]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[ppsprptProdReleases]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ppsprptProdReleases]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[ppsprptReceivingReport]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ppsprptReceivingReport]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[ppsprptVendorRelease]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ppsprptVendorRelease]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[ppsprptVendorShipSched]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ppsprptVendorShipSched]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[qrptOpenPOsDetailedB]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[qrptOpenPOsDetailedB]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[qselDealer]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[qselDealer]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[qselDealerID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[qselDealerID]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[qselFinishDesc]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[qselFinishDesc]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[qselModel]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[qselModel]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[qselOrderID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[qselOrderID]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[qselPartID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[qselPartID]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[qselPurchaseOrder]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[qselPurchaseOrder]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[qselResponseID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[qselResponseID]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[qselResponseMethod]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[qselResponseMethod]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[qselRPSPartNum]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[qselRPSPartNum]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[qselVendorName]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[qselVendorName]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[qselVendorPartNum]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[qselVendorPartNum]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[spDisplayTip]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spDisplayTip]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[spGetNTUserList]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spGetNTUserList]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[spGetSwitchboard]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spGetSwitchboard]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[spGetSwitchboardItem]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spGetSwitchboardItem]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[spGetUserSwitchboardItem]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spGetUserSwitchboardItem]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[spGetUserSwitchboardItems]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spGetUserSwitchboardItems]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[spGetUserSwitchboards]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spGetUserSwitchboards]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[spSecurityCheck]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spSecurityCheck]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[spSetSwitchboardHistory]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spSetSwitchboardHistory]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[veqrptVendorLabels]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[veqrptVendorLabels]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[waspfrmWarrantyParts]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[waspfrmWarrantyParts]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[wasprptDealerReimburse]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[wasprptDealerReimburse]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[wasprptRgaClaimDates]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[wasprptRgaClaimDates]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[wasprptWarrantyCosts]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[wasprptWarrantyCosts]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[wasprptWarrantyPending]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[wasprptWarrantyPending]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[wasprptWarrantyRGA]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[wasprptWarrantyRGA]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[wasprptWarrantyTotalCost]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[wasprptWarrantyTotalCost]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[wasprsubRGAParts]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[wasprsubRGAParts]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE dlqfrmDealerGoals
@DealerID int
AS
SELECT fkDealerID, [Year], Model, Goal
FROM tblDealersGoals
WHERE fkDealerID = @DealerID
ORDER BY [Year] DESC, Model
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[dlqfrmDealerGoals]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE Procedure dlqrptDealerContractAlpha
	@iDealerType int = 0
AS
SELECT DealerName, ContactName, StreetAddress, City, State, Zip, CurrentDealer, ContractExpires
FROM tblAllDealers
WHERE CurrentDealer = 1 AND DealerType = @iDealerType
ORDER BY DealerName, ContactName	
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[dlqrptDealerContractAlpha]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE Procedure dlqrptDealerContractDate
	@iDealerType int = 0
AS
SELECT DealerName, ContactName, StreetAddress, City, State, Zip, CurrentDealer, ContractExpires
FROM tblAllDealers
WHERE CurrentDealer = 1 AND DealerType = @iDealerType
ORDER BY ContractExpires, DealerName, ContactName	
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[dlqrptDealerContractDate]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE dlqrptDealerListAddress
	@iDealerType int = 0
AS
SELECT DealerName, ContactName, StreetAddress, City, State, Zip, CurrentDealer, ContractExpires
FROM tblAllDealers
WHERE CurrentDealer = 1 AND DealerType = @iDealerType
ORDER BY DealerName, ContactName
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[dlqrptDealerListAddress]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE dlqrptDealerListAllDealers
	@iDealerType int = 0
AS
SELECT DealerName, ContactName, City, State, Phone, TollFreeNumber
FROM tblAllDealers
WHERE DealerType = @iDealerType
ORDER BY DealerName, ContactName
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[dlqrptDealerListAllDealers]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE dlqrptDealerListFax
	@iDealerType int = 0
AS
SELECT DealerName, ContactName, City, State, Fax, TerritoryCovered, CurrentDealer
FROM tblAllDealers
WHERE CurrentDealer = 1 AND DealerType = @iDealerType
ORDER BY DealerName, ContactName
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[dlqrptDealerListFax]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE dlqrptDealerListPhone
	@iDealerType int = 0
AS
SELECT DealerName, ContactName, City, State, Phone, TollFreeNumber, CurrentDealer
FROM tblAllDealers
WHERE CurrentDealer = 1 AND DealerType = @iDealerType
ORDER BY DealerName, ContactName
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[dlqrptDealerListPhone]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE dlqrptMaillingLabels
	@iDealerType int = 0
AS
SELECT DealerName, ContactName AS Name, ContactName AS PersonName, StreetAddress, City, State, Zip, CurrentDealer, Phone, TollFreeNumber
FROM tblAllDealers
WHERE CurrentDealer = 1 AND DealerType = @iDealerType
ORDER BY DealerName
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[dlqrptMaillingLabels]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE dlqrptMaillingLabelServicePartsMan
	@iDealerType int = 0
AS
SELECT dl.DealerName, nm.PersonName, dl.StreetAddress, dl.City, dl.State, dl.Zip, dl.CurrentDealer, dl.Phone, dl.ContactName
FROM tmpNames nm INNER JOIN tblAllDealers dl ON (nm.DealerIndex=dl.DealerID)
WHERE dl.CurrentDealer=1 AND dl.DealerType = @iDealerType
ORDER BY dl.DealerName
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[dlqrptMaillingLabelServicePartsMan]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE dlqrptMaillingService
	@iDealerType int = 0
AS
SELECT DealerName, ServiceManagerName AS PersonName, StreetAddress, City, State, Zip, CurrentDealer, Phone, ContactName
FROM tblAllDealers
WHERE CurrentDealer=1 AND DealerType = @iDealerType
ORDER BY DealerName
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[dlqrptMaillingService]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE dmqrptDealerDemoReport
AS
SELECT * FROM "dmqrptDealerDemoReportView"
ORDER BY "dmqrptDealerDemoReportView".CustomerName


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[dmqrptDealerDemoReport]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE GetDbVersion AS 
  DECLARE @AppDbVer AS SMALLDATETIME 
  SELECT @AppDbVer = '6/15/2000' 
  SELECT 
    AppDbVer = '1.00', 
    AppDbOrg = 'BLI Computers, Inc.', 
    AppDbDate = @AppDbVer, 
    AppDbDesc = 'Factory Cat Database' 

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE ldpqrptDirectMailLead
	@sFromDate varchar(20),
	@sToDate varchar(20),
	@sDealerName varchar(50)
AS

DECLARE @sSQL varchar(1000)
DECLARE @sWhere varchar(200)
SELECT @sWhere = ''

SELECT @sSQL = '
SELECTDealerName, Salesman, Location, CompanyName, LeadDate, Contact, Address, City, State, Zip, ApplicationNotes, Result, ContactTitle, Phone
FROM tblPurLeads'

IF @sFromDate IS NOT NULL OR @sToDate IS NOT NULL OR @sDealerName IS NOT NULL
  BEGIN
	IF @sFromDate IS NOT NULL AND @sToDate  IS NOT NULL 
	  BEGIN
		SELECT @sWhere = 'LeadDate Between ''' + @sFromDate + ''' And ''' + @sToDAte + ''' '
	  END
  	ELSE IF @sFromDate IS NOT NULL
	  BEGIN
		SELECT @sWhere =  'LeadDate > ' + '''' + @sFromDate + ''''	
	  END
	ELSE IF @sToDate IS NOT NULL
	  BEGIN
		SELECT @sWhere =  'LeadDate > ' + '''' + @sToDate + ''''	
	  END
	IF @sDealerName IS NOT NULL
	  BEGIN
		IF @sWhere <> ''
		  BEGIN
			SELECT @sWhere = @sWhere + ' AND '
		  END
		SELECT @sWhere = @sWhere + ' DealerName = ''' + @sDealerName + ''''
	  END
	SELECT @sSQL = @sSQL + ' WHERE ' + @sWhere
  END

EXEC (@sSQL)



GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[ldpqrptDirectMailLead]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE ldqrptDirectMailLead
	@sFromDate varchar(20),
	@sToDate varchar(20),
	@sDealerName varchar(50),
	@sCompanyName varchar(50),
	@iPurchased int
AS
DECLARE @sSQL varchar(1000)
DECLARE @sWhere varchar(200)
SELECT @sWhere = ''
SELECT @sSQL = '
SELECT DealerName, Salesman, Location, CompanyName, LeadDate, Contact, Address, City, State, Zip, ApplicationNotes, Result, ContactTitle, Phone
FROM tblAllLeads
WHERE Purchased = ' + STR(@iPurchased,1,0)
IF @sFromDate IS NOT NULL OR @sToDate IS NOT NULL OR @sDealerName IS NOT NULL OR @sCompanyName IS NOT NULL
  BEGIN
	IF @sFromDate IS NOT NULL AND @sToDate  IS NOT NULL 
		SELECT @sWhere = 'LeadDate Between ''' + @sFromDate + ''' And ''' + @sToDAte + ''' '
  	ELSE IF @sFromDate IS NOT NULL
		SELECT @sWhere =  'LeadDate > ' + '''' + @sFromDate + ''''	
	ELSE IF @sToDate IS NOT NULL
		SELECT @sWhere =  'LeadDate < ' + '''' + @sToDate + ''''	
	IF @sDealerName IS NOT NULL
	  BEGIN
		IF @sWhere <> ''
			SELECT @sWhere = @sWhere + ' AND '
		SELECT @sWhere = @sWhere + ' DealerName = ''' + @sDealerName + ''''
	  END
	IF @sCompanyName IS NOT NULL
	  BEGIN
		IF @sWhere <> ''
			SELECT @sWhere = @sWhere + ' AND '
		SELECT @sWhere = @sWhere + ' CompanyName Like ''%' + @sCompanyName + '%'''
	  END
	SELECT @sSQL = @sSQL + ' AND ' + @sWhere
  END
EXEC (@sSQL)
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[ldqrptDirectMailLead]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE  ldqrptLeadsReports  AS

SELECT DealerName, CompanyName, LeadDate, Contact, State, Phone, Result, City, 
	ActiveInactive, substring(Phone, 1, 3) AS AreaCode
FROM tblLeads
ORDER BY DealerName



GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[ldqrptLeadsReports]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE ldqrptMaillingLabels
AS
SELECT * FROM "ldqrptMaillingLabelsView"
ORDER BY "ldqrptMaillingLabelsView".Zip


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[ldqrptMaillingLabels]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE ldqrptMaillingLabelsDate @Enter_Starting_Date varchar (255)
, @Enter_Ending_Date varchar (255)
AS 
SELECT tblLeads.Contact, tblLeads.CompanyName, tblLeads.Address, tblLeads.City, tblLeads.State, tblLeads.Zip, tblLeads.LeadDate
FROM tblLeads
WHERE (((tblLeads.Address) Is Not Null) AND ((tblLeads.State) Is Not Null) AND ((tblLeads.LeadDate) Between @Enter_Starting_Date And @Enter_Ending_Date))
ORDER BY tblLeads.Zip


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[ldqrptMaillingLabelsDate]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE ldqrptResponseMethod
AS
SELECT * FROM "ldqrptResponseMethodView"
ORDER BY "ldqrptResponseMethodView".DealerName


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[ldqrptResponseMethod]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE ldsprptDealerLeads
	@sFromDate varchar(20),
	@sToDate varchar(20),
	@sActiveInactive varchar(5),
	@sDealerName varchar(100),
	@iPurchased varchar(2)
AS
DECLARE @sSQL varchar(1500)
DECLARE @sWhere varchar(300)
SELECT @sWhere = ''
SELECT @sSQL = '
SELECT DealerName, CompanyName, LeadDate, Contact, State, 
    Phone, Result, City, ActiveInactive, SUBSTRING(Phone, 1, 3) 
    AS AreaCode, LeadID
FROM dbo.tblAllLeads
WHERE Purchased = ' + @iPurchased

IF @sFromDate IS NOT NULL OR @sToDate IS NOT NULL OR @sActiveInactive IS NOT NULL OR @sDealerName IS NOT NULL
  BEGIN
	IF @sFromDate IS NOT NULL And @sToDate IS NOT NULL
		SELECT @sWhere = 'LeadDate Between ''' + @sFromDate + ''' And ''' + @sToDate + ''' '
	ELSE IF @sFromDate IS NOT NULL 
		SELECT @sWhere = 'LeadDate > ''' + @sFromDate + ''' '
	ELSE IF @sToDate IS NOT NULL 
		SELECT @sWhere = 'LeadDate < ''' + @sToDate + ''' '
	IF @sActiveInactive IS NOT NULL 
	  BEGIN
		IF @sWhere <> ''
			SELECT @sWhere = @sWhere + ' AND '
		SELECT @sWhere = @sWhere + 'ActiveInactive = ''' +  @sActiveInactive + ''''
	  END
	IF @sDealerName IS NOT NULL 
	  BEGIN
		IF @sWhere <> ''
			SELECT @sWhere = @sWhere + ' AND '
		SELECT @sWhere = @sWhere + 'DealerName = ''' +  @sDealerName + ''''
	  END
	SELECT @sSQL = @sSQL + ' AND ' + @sWhere
END
EXEC (@sSQL)


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[ldsprptDealerLeads]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE ldsprptResponseMethod
	@sFromDate varchar(20),
	@sToDate varchar(20),
	@sResponseMethod varchar(25),
	@iPurchased varchar(5)
AS

DECLARE @sSQL varchar(1500)
DECLARE @sWhere varchar(300)
SELECT @sWhere = ''
SELECT @sSQL = '
SELECT DealerName, CompanyName, LeadDate, Contact, State, Phone, Result, City, ResponseMethod
FROM tblAllLeads
WHERE Purchased = ' + @iPurchased

IF @sFromDate IS NOT NULL OR @sToDate IS NOT NULL OR @sResponseMethod IS NOT NULL
  BEGIN
	IF @sFromDate IS NOT NULL And @sToDate IS NOT NULL
		SELECT @sWhere = 'LeadDate Between ''' + @sFromDate + ''' And ''' + @sToDate + ''' '
	ELSE IF @sFromDate IS NOT NULL 
		SELECT @sWhere = 'LeadDate > ''' + @sFromDate + ''' '
	ELSE IF @sToDate IS NOT NULL 
		SELECT @sWhere = 'LeadDate < ''' + @sToDate + ''' '
	IF @sResponseMethod IS NOT NULL 
	  BEGIN
		IF @sWhere <> ''
			SELECT @sWhere = @sWhere + ' AND '
		SELECT @sWhere = @sWhere + 'ResponseMethod = ''' +  @sResponseMethod + ''''
	  END
	SELECT @sSQL = @sSQL + ' AND ' + @sWhere
END
EXEC (@sSQL)

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[ldsprptResponseMethod]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE lsqrptStatesDealerList
AS
SELECT * FROM "lsqrptStatesDealerListView"
ORDER BY "lsqrptStatesDealerListView".DealerName, "lsqrptStatesDealerListView".LastDateMailed DESC


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[lsqrptStatesDealerList]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE lsqrptStatesLastMailed
AS
SELECT * FROM "lsqrptStatesLastMailedView"
ORDER BY "lsqrptStatesLastMailedView".State, "lsqrptStatesLastMailedView".LastDateMailed DESC


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[lsqrptStatesLastMailed]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE lsqrptStatesNotMailed
AS
SELECT * FROM "lsqrptStatesNotMailedView"
ORDER BY "lsqrptStatesNotMailedView".Flier, "lsqrptStatesNotMailedView".State


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[lsqrptStatesNotMailed]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE nwqrptAddressListing
AS
SELECT * FROM "nwqrptAddressListingView"
ORDER BY "nwqrptAddressListingView".DealerName, "nwqrptAddressListingView".Name


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[nwqrptAddressListing]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE nwqrptMailingLabels
AS
SELECT * FROM "nwqrptMailingLabelsView"
ORDER BY "nwqrptMailingLabelsView".DealerName


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[nwqrptMailingLabels]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE orqrptEndUserLabels
AS
SELECT * FROM "orqrptEndUserLabelsView"
ORDER BY "orqrptEndUserLabelsView".Name


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orqrptEndUserLabels]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE orsprptAcknowledgement
	@iOrderID varchar(40) = null,
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null,
	@iOrderType varchar(2) = 0
AS
-- Check input parameters
IF @iOrderID IS NULL
	SELECT @iOrderID = '%'
IF @sFromDate IS NULL
	SELECT @sFromDate = '1/1/1900'
IF @sToDate IS NULL
	SELECT @sToDate = '1/1/2100'
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
-- Run query
SELECT ao.OrderID, ao.Dealer, ao.OrderDate, ao.PurchaseOrder, ao.ShipName, ao.StreetAddress, ao.CityStateZip, 
	ao.Quantity, ao.Model, md.Description, md.Price, ao.Options, ao.PromisedDate, ao.ShipVia, ao.OrderNumber, 
	ao.CollectPrepaid, ao.Terms
FROM dbo.tblAllOrders ao, dbo.tblModels md
WHERE ao.Model = md.Model AND OrderType = @iOrderType
	AND (ao.OrderDate Between @sFromDate And @sToDate)
	AND ao.OrderID like @iOrderID
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptAcknowledgement]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE orsprptCustomerLabels
	@iOrderType varchar(3) = 0
AS
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
SELECT [Name] AS CName, Address, City, State, Zip, ContactName
FROM tblAllOrders
WHERE [Name] IS NOT NULL AND City IS NOT NULL AND OrderType = @iOrderType
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptCustomerLabels]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE orsprptCustomerMailLabels
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null,
	@iOrderType varchar(3) = 0
AS
-- Check input parameters
IF @sFromDate IS NULL
	SELECT @sFromDate = '1/1/1900'
IF @sToDate IS NULL
	SELECT @sToDate = '1/1/2100'
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
SELECT [Name] AS CName, Address, City, State, Zip, ContactName
FROM tblAllOrders
WHERE [Name] IS NOT NULL AND City IS NOT NULL and State is not null AND OrderType = @iOrderType 
	AND OrderDate between @sFromDAte and @sToDAte
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptCustomerMailLabels]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE orsprptDealerSales 
	@sDealerName varchar(40) = null,
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null,
	@iOrderType varchar(3) = 0
AS
-- Check input parameters
IF @sDealerName IS NULL
	SELECT @sDealerName = '%'
IF @sFromDate IS NULL
	SELECT @sFromDate = '1/1/1900'
IF @sToDate IS NULL
	SELECT @sToDate = '1/1/2100'
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
-- Run query
SELECT DISTINCT ao.Dealer, ao.Model, ao.OrderDate, ao.ShippedDate, ao.Quantity, ao.SalePrice, ao.OrderNumber, ao.SerialNumber
FROM tblAllOrders ao INNER JOIN tblAllDealers dl ON ao.Dealer = dl.DealerName
WHERE (dl.CurrentDealer = 1) AND SalePrice <> 0 AND ao.OrderType = @iOrderType
	AND (ao.Dealer like @sDealerName)
	AND (ao.OrderDate Between @sFromDate And @sToDate)
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptDealerSales]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE orsprptDealerSerial
	@sDealerName varchar(40) = null,
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null,
	@iOrderType varchar(3) = 0
AS
-- Check input parameters
IF @sDealerName IS NULL
	SELECT @sDealerName = '%'
IF @sFromDate IS NULL
	SELECT @sFromDate = '1/1/1900'
IF @sToDate IS NULL
	SELECT @sToDate = '1/1/2100'
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
--- Run query
SELECT ao.Dealer, ao.Model, ao.SerialNumber, ao.Name, ao.SaleDate, ao.Quantity, ao.SalePrice, ao.ShippedDate
FROM tblAllOrders ao
WHERE ao.OrderType = @iOrderType
	AND (ao.Dealer like @sDealerName)
	AND (ao.ShippedDate Between @sFromDate and @sToDate)
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptDealerSerial]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE orsprptEndUser
	@iOrderType varchar(3) = 0
AS
-- Check input parameters
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
--- Run query
SELECT PartialList,[Name], ContactName, Address, City, State, Zip, SaleDate, TypeOfBusiness, Phone, SICCode 
FROM tblAllOrders 
WHERE PartialList='X'
	AND OrderType = @iOrderType
ORDER BY [Name];
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptEndUser]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE orsprptEndUserDealer
-- also used by orrptEndUserLabels
	@sDealer varchar(50) = null,
	@iOrderType varchar(3) = 0
AS
-- Check input parameters
IF @sDealer IS NULL
	SELECT @sDealer = '%'
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
--- Run query
SELECT Dealer, Model, [Name], SaleDate, Address, City, State, Zip, ContactName, Phone, [city] + ', ' + [state] + '  ' + [zip] AS AddressLine
FROM tblAllOrders 
WHERE ([Name] IS NOT NULL) 
	AND OrderType = @iOrderType
	AND Dealer like @sDealer
ORDER BY [Name], SaleDate DESC
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptEndUserDealer]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE orsprptEndUserState
	@sState varchar(50) = null,
	@iOrderType varchar(3) = 0
AS
-- Check input parameters
IF @sState IS NULL
	SELECT @sState = '%'
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
--- Run query
SELECT Model, SaleDate, [Name], Address, City, State, Zip, ContactName, Phone
FROM tblAllOrders
WHERE (State like @sState)
	AND OrderType = @iOrderType
ORDER BY SaleDate DESC
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptEndUserState]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE orsprptMajorAccountSales
	@sMAName varchar(40) = null,
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null,
	@iOrderType varchar(3) = 0
AS
-- Check input parameters
IF @sMAName IS NULL
	SELECT @sMAName = '%'
IF @sFromDate IS NULL
	SELECT @sFromDate = '1/1/1900'
IF @sToDate IS NULL
	SELECT @sToDate = '1/1/2100'
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
--- Run query
SELECT ma.MACompName, ma.MAHeadqAddress, ma.MACity, ma.MAState, 
    ma.MAZip, ao.SaleDate, ao.[Name], ao.Address, ao.City, ao.State, ao.Zip, 
    ao.Dealer, ao.Model, ao.SalePrice, ao.OrderDate
FROM dbo.tblAllMajorAccounts ma INNER JOIN dbo.tblAllOrders ao ON 
    ma.MajorAccountID = ao.MajorAccountID
WHERE ao.OrderType = @iOrderType
	AND ao.OrderDate Between @sFromDate And @sToDate
	AND ma.MACompName like @sMAName
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptMajorAccountSales]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE orsprptMarginByModel
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null,
	@iOrderType varchar(3) = 0
AS
-- Check input parameters
IF @sFromDate IS NULL
	SELECT @sFromDate = '1/1/1900'
IF @sToDate IS NULL
	SELECT @sToDate = '1/1/2100'
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
--- Run query
SELECT ShippedDate, Dealer, Model, OrderNumber, Quantity, SalePrice, CostPrice, Margin, SerialNumber 
FROM dbo.tblAllOrders 
WHERE (SalePrice <> 0) 
	AND (OrderType = @iOrderType)
	AND (ShippedDate Between @sFromDate and @sToDate)
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptMarginByModel]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE orsprptOpenOrders
	@iOrderType varchar(3) = 0
AS
-- Check input parameters
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
--- Run query
SELECT  ShippedDate, Model, Dealer, OrderNumber, Quantity, SalePrice, CostPrice, Margin
FROM dbo.tblAllOrders
WHERE (ShippedDate Is Null)
	AND (OrderType = @iOrderType)
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptOpenOrders]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE orsprptPrepSheet 
	@sModel varchar(40) = null,
	@iOrderType varchar(3) = 0
AS
-- Check input parameters
IF @sModel IS NULL
	SELECT @sModel = '%'
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
-- Run query
SELECT Model, PromisedDate, OrderNumber, Dealer, Battery, AmpCharger, ShipVia, Notes, [Size], ShippedDate
FROM tblAllOrders
WHERE (Model<>0) 
	AND (ShippedDate Is Null)
	AND Model like @sModel
	AND OrderType = @iOrderType
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptPrepSheet]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE orsprptProdSchedule
	@iOrderType varchar(3) = 0
AS
-- Check input parameters
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
--- Run query
SELECT  PromisedDate, ShipVia, Dealer, Quantity, OrderNumber, Model, Notes, ShippedDate
FROM tblAllOrders
WHERE (ShippedDate Is Null)
	AND OrderType = @iOrderType
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptProdSchedule]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE orsprptSalesDealer
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null,
	@iOrderType varchar(3) = 0
AS
-- Check input parameters
IF @sFromDate IS NULL
	SELECT @sFromDate = '1/1/1900'
IF @sToDate IS NULL
	SELECT @sToDate = '1/1/2100'
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
--- Run query
SELECT Dealer, Model, OrderNumber, ShippedDate, Quantity, SalePrice, CostPrice, Margin, OrderDate 
FROM dbo.tblAllOrders 
WHERE (SalePrice <> 0) 
	AND (OrderType = @iOrderType)
	AND (OrderDate Between @sFromDate and @sToDate)
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptSalesDealer]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE dbo.orsprptSalesModel
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null,
	@iOrderType varchar(3) = 0
AS 
-- Check input parameters
IF @sFromDate IS NULL
	SELECT @sFromDate = '1/1/1900'
IF @sToDate IS NULL
	SELECT @sToDate = '1/1/2100'
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
--- Run query
SELECT Model, Dealer, Sum(Quantity) AS Quantity, Sum(SalePrice) AS SalePrice, Sum(CostPrice) AS CostPrice, Sum(Margin) AS Margin
FROM tblAllOrders
WHERE SalePrice <> 0
	AND (OrderType = @iOrderType)
	AND (OrderDate Between @sFromDate and @sToDate)
GROUP BY Model, Dealer
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptSalesModel]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE dbo.orsprptSalesModelON
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null,
	@iOrderType varchar(3) = 0
AS 
-- Check input parameters
IF @sFromDate IS NULL
	SELECT @sFromDate = '1/1/1900'
IF @sToDate IS NULL
	SELECT @sToDate = '1/1/2100'
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
--- Run query
SELECT Model, Dealer, OrderNumber, SalePrice, CostPrice, Margin
FROM tblAllOrders
WHERE SalePrice <> 0
	AND (OrderType = @iOrderType)
	AND (OrderDate Between @sFromDate and @sToDate)
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptSalesModelON]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  OFF 
GO



CREATE PROCEDURE dbo.orsprptSalesRepCommission
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null
AS 
-- Check input parameters
IF @sFromDate IS NULL
	SELECT @sFromDate = '1/1/1900'
IF @sToDate IS NULL
	SELECT @sToDate = '1/1/2100'

--- Run query
SELECT o.Model, o.Dealer, o.Quantity, o.SalePrice, o.OrderNumber, o.OrderType, r.DealerRepName
FROM tblAllOrders o, tblAllDealers d, tlkuDealerRep r
WHERE o.SalePrice <> 0
	AND (o.OrderDate Between @sFromDate and @sToDate)
	AND d.DealerRepID = r.DealerRepID
	AND o.Dealer = d.DealerName


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptSalesRepCommission]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE orsprptServicingDealers
	@iOrderType varchar(3) = 0
AS
-- Check input parameters
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
--- Run query
SELECT ao.Model, ao.SerialNumber, ao.[Name], ao.Phone, ao.City, ao.State, ao.SaleDate, ao.Dealer, dl.DealerName 
FROM dbo.tblAllOrders ao INNER JOIN dbo.tblAllDealers dl ON ao.fkServDealerID = dl.DealerID
WHERE OrderType = @iOrderType
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptServicingDealers]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE orsprptUnregSweepers
	@iOrderType varchar(3) = 0
AS
-- Check input parameters
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
--- Run query
SELECT DISTINCT ao.Dealer, ao.SerialNumber, ao.PurchaseOrder, ao.OrderDate, ao.ShippedDate, ao.[Name], ao.Model 
FROM dbo.tblAllDealers dl RIGHT OUTER JOIN dbo.tblAllOrders ao ON dl.DealerName = ao.Dealer 
WHERE (dl.CurrentDealer = 1) 
	AND (ao.ShippedDate IS NOT NULL) 
	AND (ao.[Name] IS NULL)
	AND OrderType = @iOrderType
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptUnregSweepers]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO




CREATE PROCEDURE paqrptPartListFinish
AS
SELECT * FROM "paqrptPartListFinishView"
ORDER BY "paqrptPartListFinishView".RPSPartNum, "paqrptPartListFinishView".PartName



GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[paqrptPartListFinish]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE paspfrmPartsFinish
	@iPartID int = 0
AS
select pf.FinishID, pf.FOrder, ve.VendorName, pf.FDescription, pf.FReadyToUse, pf.FCost, pf.FPackaging
from dbo.tblPartsFinish pf left outer join dbo.tblVendors ve on pf.VendorID = ve.VendorID
where pf.PartID = @iPartID 
order by pf.FOrder
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[paspfrmPartsFinish]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE paspfrmPartsModels
	@iPartID int = 0
AS
select PartModelID, Model, Quantity, Optional
from dbo.tblPartsModels
where fkPartID = @iPartID
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[paspfrmPartsModels]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  OFF 
GO



CREATE PROCEDURE paspfrmPartsOptions
	@iPartID int = 0
AS
select po.PartOptionID, po.ChildPartID, pa.RPSPartNum, pa.PartName
from dbo.tblPartsOptions po, dbo.tblParts pa
where po.ParentPartID = @iPartID and po.ChildPartID = pa.PartID


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[paspfrmPartsOptions]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE paspfrmPartsSubParts
	@iPartID int = 0
AS
select SubPartID, SubNum, SubDescription, SubCost, SubExtCost, SubSource, SubSourcePartNum, SubQty 
from dbo.tblPartsSubParts 
where fkPartID = @iPartID
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[paspfrmPartsSubParts]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE paspfrmPartsVendors
	@iPartID int = 0
AS
select ve.VendorName, pv.PartVendorID, pv.VendorPartName, pv.VendorPartNum, pv.VendorCostEach, pv.VendorPrimary
from dbo.tblVendors ve, dbo.tblPartsVendors pv
where ve.VendorID = pv.VendorID
	and pv.PartID = @iPartID
order by pv.VendorPrimary desc, ve.VendorName
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[paspfrmPartsVendors]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE pasprptBillOfMaterials
	@sVendorName varchar(50),
	@sModel varchar(10)
 AS
DECLARE @sSQL varchar(1500)
DECLARE @sWhere varchar(500)
SELECT @sWhere = ''
SELECT @sSQL = 
'SELECT p.RPSPartNum, p.PartName, p.VendorName, p.CostEach,  
	CONVERT (smallmoney, CONVERT (smallmoney, p.CostEach) * CONVERT (int, m.Quantity) ) AS ExtCost, p.RPSPNSort
FROM dbo.tblParts p, dbo.tblPartsModels m
WHERE p.PartID = m.fkPartID AND m.Quantity > 0 '
-- Check model
IF @sModel IS NOT NULL
	SELECT @sWhere = ' m.Model = ''' + @sModel + ''' '
-- Check vendor
IF @sVendorName IS NOT NULL
  BEGIN
	IF @sWhere <> '' 
		SELECT @sWhere = @sWhere + ' AND '
	SELECT @sWhere = @sWhere +  'p.VendorName = ''' + @sVendorName + ''''
	SELECT @sWhere = @sWhere + ' AND p.RPSPartNum IN ('
	SELECT @sWhere = @sWhere + '
		SELECT pop.RPSPartNum
		FROM dbo.tblPO po, dbo.tblPOPart pop, dbo.tblPOPartDetail popd
		WHERE po.POID = pop.fkPOID AND pop.POPartID = popd.fkPOPartID
			AND popd.RequestedShipDate IS NOT NULL
			AND popd.ReceivedDate IS NULL )'
  END
IF @sWhere <> ''
  BEGIN
	SELECT @sSQL = @sSQL + ' AND ' + @sWhere
  END
EXEC (@sSQL)
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[pasprptBillOfMaterials]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE pasprptDealerPartsList
	@iType int = null
AS
select tP.RPSPartNum, tP.PartName, tP.USADealerNet, tP.USASuggestedList, tP.Note, tP.PartName, tP.RPSPNSort
from dbo.tblParts tP, dbo.tblModels tM, dbo.tblPartsModels tPM
where tP.PartID = tPM.fkPartID and tPM.Model = tM.Model and tM.ModelType = @iType and tP.CostEach > 0 and 
	tP.RPSPartNum not like 'V%' And tP.RPSPartNum not like 'E%' and tP.RPSPartNum not like 'H%' and tP.HideOnReports = 0
group by tP.RPSPartNum, tP.PartName, tP.USADealerNet, tP.USASuggestedList, tP.Note, tP.PartName, tP.RPSPNSort
order by tP.RPSPNSort
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[pasprptDealerPartsList]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE pasprptListPrices
	@iType int = 0
AS
select tP.RPSPartNum, tP.PartName, tP.USASuggestedList, tP.Note, tP.RPSPNSort
from dbo.tblParts tP, dbo.tblModels tM, dbo.tblPartsModels tPM
where tP.PartID = tPM.fkPartID and tPM.Model = tM.Model and tM.ModelType = @iType and tP.CostEach > 0 and 
	tP.RPSPartNum not like 'V%' And tP.RPSPartNum not like 'E%' and tP.RPSPartNum not like 'H%' and tP.HideOnReports = 0
	and (tPM.Quantity > 0 or tPM.Optional > 0)
group by tP.RPSPartNum, tP.PartName, tP.USASuggestedList, tP.Note, tP.RPSPNSort
order by tP.RPSPNSort
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[pasprptListPrices]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE pasprptPartFinishVendorSched
	@sVendorName nvarchar(100) = null,
	@sFromDate nvarchar(50) = null,
	@sToDate nvarchar(50) = null
AS
if @sVendorName is null
	SELECT @sVendorName = '%'
if @sFromDate is null
	SELECT @sFromDate = '1/1/1900'
if @sToDAte is null
	SELECT @sToDate = '1/1/2233'
SELECT ve.VendorName, pd.RequestedShipDate, pa.RPSPartNum, pa.FinishDesc, pd.Quantity, pa.PartName
FROM dbo.tblParts pa, dbo.tblPOPartDetail pd, dbo.tblPO po, dbo.tblPOPart pp, dbo.tblVendors ve
where pp.POPartID = pd.fkPOPartID and pp.fkPOID = po.POID and ve.VendorID = pa.FinishVendorID 
	and pp.RPSPartNum = pa.RPSPartNum and po.Vendor = ve.VendorName
	and pd.ReceivedDate is null
	and ve.VendorName like @sVendorName
	and pd.RequestedShipDate between @sFromDate and @sToDate
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[pasprptPartFinishVendorSched]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE pasprptPartLabels 
	@sStartPart varchar(50) = null,
	@sEndPart varchar(50) = null,
	@sModel varchar(50) = null,
	@iQty int = 1
AS
IF @sStartPart IS NULL
	SELECT @sStartPart = '000000000000000000'
IF @sEndPart IS NULL
	SELECT @sEndPart = 'zzzzzzzzzzzzzzzzzzzzzz'
IF @iQty IS NULL
	SELECT @iQty = 1
IF @sModel is null
	SELECT pa.RPSPartNum, pa.PartName, pa.VendorName, pa.VendorPartName, pa.ManfPartNum, nu.Counter, pa.RPSPNSort
	FROM dbo.tblParts pa, dbo.tbl_Numbers nu
	WHERE nu.Counter <= @iQty		-- allows multiple labels
		and pa.RPSPartNum Between @sStartPart And @sEndPart
else
	SELECT DISTINCT pa.RPSPartNum, pa.PartName, pa.VendorName, pa.VendorPartName, pa.ManfPartNum, nu.Counter, pa.RPSPNSort
	FROM dbo.tblParts pa, dbo.tbl_Numbers nu, dbo.tblPartsModels pm
	WHERE nu.Counter <= @iQty		-- allows multiple labels
		and pa.RPSPartNum Between @sStartPart And @sEndPart
		and pm.Model Like @sModel
	
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[pasprptPartLabels]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE pasprptProductionSchedule
	@sFromMonth char(2) = null,
	@sFromYear char(4) = null,
	@sToMonth char(2) = null,
	@sToYear char(4) = null,
	@sVendorName varchar(50) = null
AS
IF @sFromMonth IS NULL
	SELECT @sFromMonth = '00'
IF @sFromYear IS NULL
	SELECT @sFromYear = '0000'
IF @sToMonth IS NULL
	SELECT @sToMonth = '99'
IF @sToYear IS NULL
	SELECT @sToYear = '2050'
IF @sVendorName IS NULL
	SELECT @sVendorName = '%'
DECLARE @sFrom char(6)
DECLARE @sTo char(6)
SELECT @sFrom = @sFromYear + CAST(CAST(REPLICATE('0', 2 - DATALENGTH(CAST(RTRIM(@sFromMonth) as varchar(2)))) AS varchar(3)) +  CAST(@sFromMonth as varchar(2)) AS char(2))
SELECT @sTo = @sToYear + CAST(CAST(REPLICATE('0', 2 - DATALENGTH(CAST(RTRIM(@sToMonth) as varchar(2)))) AS varchar(3)) +  CAST(@sToMonth as varchar(2)) AS char(2))
select pa.RPSPartNum, Sum(si.Quantity * pm.Quantity) AS QtyReqd, pa.VendorName, pa.RPSPNSort
from dbo.tblParts pa, dbo.tblProdSchedules ps, dbo.tblProdSchedItems si, dbo.tblPartsModels pm
where pm.Model = si.Model
	and pa.PartID = pm.fkPartID
	and ps.ScheduleID = si.ScheduleID
	and pa.HideOnReports = 0
	and CAST(CAST([Year] AS char(4)) + CAST(CAST(REPLICATE('0', 2 - DATALENGTH(CAST([Month] as varchar(2)))) AS varchar(3)) +  CAST([Month] as varchar(2)) AS char(2))  AS int)
		Between @sFrom And @sTo
	and pa.VendorName Like @sVendorName
group by pa.RPSPNSort, pa.RPSPartNum, pa.VendorName
having Sum(si.Quantity * pm.Quantity) > 0
order by pa.RPSPNSort


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[pasprptProductionSchedule]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE pasprptRPSPartsPricing 
	@iType int = 0
AS
select tP.RPSPartNum, tP.VendorName, tP.ManfPartNum, tP.SuggestedList, tP.DealerNet, tP.PartName, tP.RPSPNSort
from dbo.tblParts tP, dbo.tblModels tM, dbo.tblPartsModels tPM
where tP.PartID = tPM.fkPartID and tPM.Model = tM.Model and tM.ModelType = @iType and tPM.Quantity > 0 and
	tP.RPSPartNum not like 'V%' And tP.RPSPartNum not like 'E%' and tP.RPSPartNum not like 'H%' and tP.HideOnReports = 0
group by tP.RPSPartNum, tP.VendorName, tP.ManfPartNum, tP.SuggestedList, tP.DealerNet, tP.PartName, tP.RPSPNSort
order by tP.RPSPNSort
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[pasprptRPSPartsPricing]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO




CREATE PROCEDURE pasprsubBillOfMaterialsPParts
	@sVendorName varchar(50)
 AS

DECLARE @sSQL varchar(1500)
DECLARE @sWhere varchar(500)
SELECT @sWhere = ''

SELECT @sSQL = '
SELECT dbo.tblPOPart.RPSPartNum, dbo.tblPO.POID, dbo.tblPOPart.VendorPartNumber, dbo.tblPO.Vendor, 
	dbo.tblPOPartDetail.RequestedShipDate, dbo.tblPOPartDetail.CostEach, dbo.tblPOPartDetail.Value, 
	dbo.tblPOPartDetail.Quantity
FROM dbo.tblPO INNER JOIN dbo.tblPOPart ON dbo.tblPO.POID = dbo.tblPOPart.fkPOID INNER JOIN
	dbo.tblPOPartDetail ON dbo.tblPOPart.POPartID = dbo.tblPOPartDetail.fkPOPartID
WHERE (dbo.tblPOPartDetail.RequestedShipDate IS NOT NULL) AND 
	(dbo.tblPOPartDetail.ReceivedDate IS NULL)'

-- Check vendor
IF @sVendorName IS NOT NULL
  BEGIN
	SELECT @sSQL = @sSQL +  ' dbo.tblParts.VendorName = ''' + @sVendorName + ''''
  END

EXEC (@sSQL)


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[pasprsubBillOfMaterialsPParts]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


create proc ppfsubOrderDetail
	@POPartID int = 0
as
select * 
from dbo.tblPOPartDetail 
where fkPOPartID = @POPartID
order by RequestedShipDate
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[ppfsubOrderDetail]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE ppqrptPOCosts
	@sToDate varchar(20),
	@sPOID varchar(20)
AS
DECLARE @sSQL varchar(1200)
DECLARE @sWhere varchar(300)
SELECT @sWhere = '(dbo.tblPOPartDetail.Quantity <> 0)'
IF @sToDate IS NOT NULL
  BEGIN
	SELECT @sWhere = @sWhere + ' And dbo.tblPOPartDetail.RequestedShipDate < ''' + @sToDate + ''''
  END
IF @sPOID IS NOT NULL
  BEGIN
	SELECT @sWhere = @sWhere + ' And dbo.tblPO.POID = ' + @sPOID
  END
SELECT @sSQL = 
	'SELECT dbo.tblPO.POID, dbo.tblPOPart.RPSPartNum, dbo.tblPOPart.VendorPartNumber, dbo.tblPOPart.PartDescription, 
		SUM(dbo.tblPOPartDetail.CostEach) AS CostEach 
	FROM dbo.tblPO INNER JOIN dbo.tblPOPart ON dbo.tblPO.POID = dbo.tblPOPart.fkPOID AND 
		dbo.tblPO.POID = dbo.tblPOPart.fkPOID INNER JOIN dbo.tblPOPartDetail ON dbo.tblPOPart.POPartID = 
		dbo.tblPOPartDetail.fkPOPartID 
	WHERE ' + @sWhere + '
	GROUP BY dbo.tblPO.POID, dbo.tblPOPart.RPSPartNum, 	dbo.tblPOPart.VendorPartNumber, dbo.tblPOPart.PartDescription'
EXEC (@sSQL)
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[ppqrptPOCosts]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE ppqrptVendorRelease
AS
SELECT * FROM "ppqrptVendorReleaseView"
ORDER BY "ppqrptVendorReleaseView".RPSPartNum, "ppqrptVendorReleaseView".RequestedShipDate


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[ppqrptVendorRelease]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE ppsprptCashFlow
	@sVendorName varchar(40) = null,
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null,
	@sOpenClosed varchar(10) = null
 AS
IF @sFromDate is null
	select @sFromDate = '1/1/1900'
if @sToDate is null
	select @sToDate = '1/1/2100'
if @sVendorName is null
	select @sVendorName = '%'
if @sOpenClosed is null
	select pd.RequestedShipDate, pp.VendorPartNumber, po.Vendor, pp.RPSPartNum, pd.CostEach, pd.Quantity, pd.Value, 
		pd.ReceivedDate, po.POID
	from dbo.tblPO po, dbo.tblPOPart pp, dbo.tblPOPartDetail pd 
	where po.POID = pp.fkPOID and pp.POPartID = pd.fkPOPartID
		and pd.RequestedShipDate between @sFromDate and @sToDate
		and po.Vendor like @sVendorName
else if @sOpenClosed = 'open'
	select pd.RequestedShipDate, pp.VendorPartNumber, po.Vendor, pp.RPSPartNum, pd.CostEach, pd.Quantity, pd.Value, 
		pd.ReceivedDate, po.POID
	from dbo.tblPO po, dbo.tblPOPart pp, dbo.tblPOPartDetail pd 
	where po.POID = pp.fkPOID and pp.POPartID = pd.fkPOPartID
		and pd.RequestedShipDate between @sFromDate and @sToDate
		and po.Vendor like @sVendorName
		and pd.ReceivedDate is null
else
	select pd.RequestedShipDate, pp.VendorPartNumber, po.Vendor, pp.RPSPartNum, pd.CostEach, pd.Quantity, pd.Value, 
		pd.ReceivedDate, po.POID
	from dbo.tblPO po, dbo.tblPOPart pp, dbo.tblPOPartDetail pd 
	where po.POID = pp.fkPOID and pp.POPartID = pd.fkPOPartID
		and pd.RequestedShipDate between @sFromDate and @sToDate
		and po.Vendor like @sVendorName
		and pd.ReceivedDate is not null
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[ppsprptCashFlow]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  OFF 
GO



CREATE PROCEDURE ppsprptPartsOnHold

AS

SELECT po.POID, po.Vendor, pp.RPSPartNum, pp.PartDescription, 
	pd.RequestedShipDate, pd.Quantity, pd.Value, pd.ReceivedDate, 
	pd.CostEach, pd.Notes, pa.RPSPNSort
FROM dbo.tblPO po, dbo.tblPOPart pp, dbo.tblPOPartDetail pd, dbo.tblParts pa
WHERE po.POID = pp.fkPOID and pp.POPartID = pd.fkPOPartID
	and pa.RPSPartNum = pp.RPSPartNum
	and pd.Quantity = 0

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[ppsprptPartsOnHold]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE ppsprptProdReleases
	@sToDate varchar(20)
AS
IF @sToDate is null
	select @sToDate = '1/1/2020'
SELECT po.POID, po.Vendor, pp.VendorPartNumber, pp.RPSPartNum, pp.PartDescription, 
	pd.RequestedShipDate, pd.Quantity, pd.Value, pd.ReceivedDate, 
	pd.CostEach, pd.Quantity AS ExtCost, pd.Notes, pa.RPSPNSort, 
	DATEPART(yyyy, pd.RequestedShipDate) AS ShipYear, 
	DATEPART(wk, pd.RequestedShipDate) AS ShipWeek
FROM dbo.tblPO po, dbo.tblPOPart pp, dbo.tblPOPartDetail pd, dbo.tblParts pa
WHERE po.POID = pp.fkPOID and pp.POPartID = pd.fkPOPartID and
	pd.ReceivedDate IS NULL and pd.RequestedShipDate < @sToDate
	and pa.RPSPartNum = pp.RPSPartNum

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[ppsprptProdReleases]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE ppsprptReceivingReport
	@sVendorName varchar(40), 
	@sPOID varchar(40), 
	@iOpenFlag varchar(2)
AS

DECLARE @sSQL varchar(1300)
DECLARE @sWhere varchar(300)
SELECT @sWhere = ''

SELECT @sSQL = 
'SELECT dbo.tblPO.Vendor, dbo.tblPO.POID, dbo.tblPOPart.VendorPartNumber, dbo.tblPOPart.RPSPartNum, 
	dbo.tblPOPart.PartDescription, dbo.tblPOPartDetail.RequestedShipDate, dbo.tblPOPartDetail.Quantity, 
	dbo.tblPOPartDetail.ReceivedDate, dbo.tblPOPartDetail.QuantityReceived 
FROM dbo.tblPO INNER JOIN dbo.tblPOPart ON dbo.tblPO.POID = dbo.tblPOPart.fkPOID AND dbo.tblPO.POID = dbo.tblPOPart.fkPOID 
	INNER JOIN dbo.tblPOPartDetail ON dbo.tblPOPart.POPartID = dbo.tblPOPartDetail.fkPOPartID '

IF @iOpenFlag = 1 
  BEGIN
	SELECT @sWhere = 'ReceivedDate IS NULL'
  END

IF @sVendorName IS NOT NULL
  BEGIN
	IF @sWhere <> ''
	  BEGIN
		SELECT @sWhere = @sWhere + ' AND '
	  END
	SELECT @sWhere = @sWhere + 'Vendor = ''' + @sVendorName + ''''
  END

IF @sPOID IS NOT NULL
  BEGIN
	IF @sWhere <> ''
	  BEGIN
		SELECT @sWhere = @sWhere + ' AND '
	  END
	SELECT @sWhere = @sWhere + 'POID = ' + @sPOID
  END

IF @sWhere <> ''
  BEGIN
	SELECT @sSQL = @sSQL + ' WHERE ' + @sWhere
  END

EXEC (@sSQL)




GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[ppsprptReceivingReport]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE ppsprptVendorRelease
	@sPOID varchar(20) = null,
	@sRPSPartNum varchar(40) = null,
	@sVendorPartNum varchar(40) = null,
	@iOpenFlag varchar(2) = null
AS
if @sPOID is null
	select @sPOID = '%'
if @sRPSPartNum is null
	select @sRPSPartNum = '%'
if @sVendorPartNum is null
	select @sVendorPartNum = '%'
if @iOpenFlag = 1
	SELECT po.POID, po.Vendor, pp.VendorPartNumber, pp.RPSPartNum, pp.PartDescription, pd.RequestedShipDate,
		pd.Quantity, pd.Value, pd.ReceivedDate, pa.RPSPNSort, pp.POPartID, pd.POPartDetailID
	FROM dbo.tblPO po, dbo.tblPOPart pp, dbo.tblPOPartDetail pd, dbo.tblParts pa
	WHERE po.POID = pp.fkPOID and pp.POPartID = pd.fkPOPartID and pd.RequestedShipDate is not null
		and pa.RPSPartNum = pp.RPSPartNum
		and po.POID like @sPOID
		and pp.RPSPartNum like @sRPSPartNum
		and pp.VendorPartNumber like @sVendorPartNum
		and pd.ReceivedDate is null
	ORDER BY pp.RPSPartNum, po.POID, pp.VendorPartNumber, pd.RequestedShipDate
else
	SELECT po.POID, po.Vendor, pp.VendorPartNumber, pp.RPSPartNum, pp.PartDescription, pd.RequestedShipDate,
		pd.Quantity, pd.Value, pd.ReceivedDate, pa.RPSPNSort, pp.POPartID, pd.POPartDetailID
	FROM dbo.tblPO po, dbo.tblPOPart pp, dbo.tblPOPartDetail pd, dbo.tblParts pa
	WHERE po.POID = pp.fkPOID and pp.POPartID = pd.fkPOPartID and pd.RequestedShipDate is not null
		and pa.RPSPartNum = pp.RPSPartNum
		and po.POID like @sPOID
		and pp.RPSPartNum like @sRPSPartNum
		and pp.VendorPartNumber like @sVendorPartNum
	ORDER BY pp.RPSPartNum, po.POID, pp.VendorPartNumber, pd.RequestedShipDate
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[ppsprptVendorRelease]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE ppsprptVendorShipSched
	@sVendorName varchar(50),
	@sFromDAte varchar(20),
	@sToDAte varchar(20)
AS

DECLARE @sSQL varchar(1300)

SELECT @sSQL = '
SELECT dbo.tblPO.POID, dbo.tblPOPart.VendorPartNumber, 
    dbo.tblPO.Vendor, dbo.tblPOPart.RPSPartNum, 
    dbo.tblPOPartDetail.Quantity, dbo.tblPOPartDetail.CostEach, 
    dbo.tblPOPartDetail.Value, dbo.tblPOPart.PartDescription, 
    dbo.tblPOPartDetail.ReceivedDate, dbo.tblPOPartDetail.RequestedShipDate
FROM dbo.tblPO INNER JOIN
    dbo.tblPOPart ON dbo.tblPO.POID = dbo.tblPOPart.fkPOID AND 
    dbo.tblPO.POID = dbo.tblPOPart.fkPOID INNER JOIN
    dbo.tblPOPartDetail ON 
    dbo.tblPOPart.POPartID = dbo.tblPOPartDetail.fkPOPartID
WHERE (dbo.tblPOPartDetail.ReceivedDate IS NULL)'


IF @sVendorName IS NOT NULL
  BEGIN
	SELECT @sSQL = @sSQL + ' AND Vendor = ''' + @sVendorName + ''''
  END

IF @sFromDate IS NOT NULL And @sToDate IS NOT NULL
  BEGIN
	SELECT @sSQL = @sSQL + 'AND RequestedShipDate Between ''' + @sFromDate + ''' And ''' + @sToDate + ''' '
  END
ELSE IF @sFromDate IS NOT NULL 
  BEGIN
	SELECT @sSQL = @sSQL + 'AND RequestedShipDate > ''' + @sFromDate + ''' '
  END
ELSE IF @sToDate IS NOT NULL 
  BEGIN
	SELECT @sSQL = @sSQL + 'AND RequestedShipDate < ''' + @sToDate + ''' '
  END

EXEC (@sSQL)



GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[ppsprptVendorShipSched]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE qrptOpenPOsDetailedB
AS
SELECT * FROM "qrptOpenPOsDetailedBView"
ORDER BY "qrptOpenPOsDetailedBView".VendorPOID


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[qrptOpenPOsDetailedB]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE qselDealer
AS
SELECT DealerName
FROM tblDealers
WHERE CurrentDealer = 1
GROUP BY DealerName
ORDER BY DealerName





GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[qselDealer]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE qselDealerID
AS
SELECT DealerID, DealerName, StreetAddress, City
FROM tblDealers
ORDER BY DealerName



GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[qselDealerID]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



Create Procedure qselFinishDesc

As
	SELECT FinishDesc
	FROM tlkuFinish
	GROUP BY FinishDesc
	return 




GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[qselFinishDesc]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE qselModel
AS
SELECT Model, Description
FROM tblModels
ORDER BY Model



GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[qselModel]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



Create Procedure qselOrderID

As
	SELECT OrderID, OrderNumber, OrderDate, Dealer
	FROM tblOrders
	ORDER BY tblOrders.OrderNumber;
	return 




GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[qselOrderID]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



Create Procedure qselPartID

As
	SELECT PartID, RPSPartNum, PartName
	FROM tblParts
	ORDER BY RPSPartNum
	return 




GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[qselPartID]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO



CREATE Procedure qselPurchaseOrder

As
	SELECT POID
	FROM tblPO
	ORDER BY POID

	return 






GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[qselPurchaseOrder]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE qselResponseID
AS
SELECT ResposneID, ResponseMethod
FROM tblResponseMethod
ORDER BY ResponseMethod



GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[qselResponseID]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE qselResponseMethod
AS
SELECT ResponseMethod, ResponseMethodNotes
FROM tblResponseMethod
ORDER BY ResponseMethod



GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[qselResponseMethod]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE qselRPSPartNum
AS
SELECT RPSPartNum, PartName
FROM tblParts
ORDER BY RPSPartNum



GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[qselRPSPartNum]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE qselVendorName
AS
SELECT VendorName
FROM tblVendors
ORDER BY VendorName



GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[qselVendorName]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO

CREATE Procedure qselVendorPartNum

As
	SELECT VendorPartNumber
	FROM tblPOPart
	GROUP BY VendorPartNumber
	return 





GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[qselVendorPartNum]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE spDisplayTip
	@sUserName varchar(20),
	@iTipID int
AS

IF EXISTS ( SELECT TipID FROM dbo.tblTipDisable WHERE TipID = @iTipID 
	And UserName = @sUserName )

  BEGIN
	SELECT 1 AS flgDisplay
  END
ELSE
  BEGIN
	SELECT 0 AS flgDisplay
  END







GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[spDisplayTip]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE spGetNTUserList
AS
declare @object int
declare @hr int
declare @property varchar(2500)
declare @return varchar(255)
-- CREATE AN OBJECT
EXEC @hr = sp_OACreate 'axNTSec.ntsec', @object OUT
IF @hr <> 0
  BEGIN
	raiserror ('Error creating server component to retreive user list!', 16, 1)
	RETURN
  END
-- GET A PROPERTY BY CALLING METHOD
EXEC @hr = sp_OAMethod @object, 'GetUserStr', @property OUT
IF @hr <> 0
  BEGIN
	raiserror ('Error calling user retreival function', 16, 1)
	RETURN
  END
SELECT @property AS uList
-- DESTROY OBJECT
EXEC @hr = sp_OADestroy @object
IF @hr <> 0
  BEGIN
	raiserror ('Error destroying user object', 1, 1)
	RETURN
  END
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[spGetNTUserList]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE spGetSwitchboard
	@iSwitchboard int = 0
AS
select ItemNumber, ItemText
from dbo.[Switchboard Items]
where SwitchboardID = @iSwitchboard
order by ItemNumber
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[spGetSwitchboard]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE spGetSwitchboardItem
	@iSwitchboardID int = 0,
	@iItemNumber int = 0
AS
select ItemText, Command, Argument, OpenArgs
from dbo.[Switchboard Items]
where SwitchboardID = @iSwitchboardID and ItemNumber = @iItemNumber

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[spGetSwitchboardItem]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  OFF 
GO


CREATE PROCEDURE dbo.spGetUserSwitchboardItem
	@sUser nvarchar(50),
	@iSwitchID int
AS

select *
from dbo.[Switchboard Items]
where ID = @iSwitchID and ID not in ( select SwID from dbo.tblSecurity where UserID = @sUser and AccessType = 0)
	and ItemNumber <> 0
order by ItemNumber

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[spGetUserSwitchboardItem]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  OFF 
GO


CREATE PROCEDURE dbo.spGetUserSwitchboardItems
	@sUser nvarchar(50),
	@iSwitch int
AS

set nocount on

create table #tmp
	( ID int not null, SwitchboardID int not null, ItemNumber int not null, ItemText nvarchar(255), Command int,
	Argument nvarchar(255), AccessType int )

insert #tmp (ID, SwitchboardID, ItemNumber, ItemText, Command, Argument, AccessType)
select ID, SwitchboardID, ItemNumber, ItemText, Command, Argument, 2
from dbo.[Switchboard Items]
where SwitchboardID = @iSwitch and ItemNumber <> 0

delete #tmp
where ID in (select SwID from dbo.tblSecurity sc where sc.UserID = @sUser and sc.AccessType = 0)

update #tmp
set #tmp.AccessType = sc.AccessType
from dbo.tblSecurity sc, #tmp
where sc.SwID = #tmp.ID and sc.UserID = @sUser

select *
from #tmp
order by ItemNumber
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[spGetUserSwitchboardItems]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  OFF 
GO

CREATE PROCEDURE dbo.spGetUserSwitchboards
	@sUser nvarchar(50),
	@iSwitch int
AS


set nocount on

create table #tmp
	( ID int not null, SwitchboardID int not null, ItemNumber int not null, ItemText nvarchar(255), Command int,
	Argument nvarchar(255), AccessType int )

insert #tmp (ID, SwitchboardID, ItemNumber, ItemText, Command, Argument, AccessType)
select ID, SwitchboardID, ItemNumber, ItemText, Command, Argument, 2
from dbo.[Switchboard Items]
where SwitchboardID = @iSwitch and ItemNumber <> 0 and Command = 1

delete #tmp
where ID in (select SwID from dbo.tblSecurity sc where sc.UserID = @sUser and sc.AccessType = 0)

update #tmp
set #tmp.AccessType = sc.AccessType
from dbo.tblSecurity sc, #tmp
where sc.SwID = #tmp.ID and sc.UserID = @sUser

select *
from #tmp
order by ItemNumber
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[spGetUserSwitchboards]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE spSecurityCheck 
	@sUserID nvarchar(50),
	@iSwitchboardID int,
	@iItemNumber int
AS
SELECT AccessType 
FROM dbo.tblSecurity
WHERE UserID = @sUserID AND SwID IN (
	SELECT [ID]
	FROM dbo.[Switchboard Items]
	WHERE SwitchboardID = @iSwitchboardID AND ItemNumber = @iItemNumber )
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[spSecurityCheck]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE spSetSwitchboardHistory
	@sUser nvarchar(40) = null,
	@iSwitchboard int = 0,
	@iItemNumber int = 0
AS
set nocount on
insert tblMenuHistory (HistoryTime, HistoryUser, HistorySwitchID, HistorySwitchItem)
values (GETDATE(), @sUser, @iSwitchboard, @iItemNumber)
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[spSetSwitchboardHistory]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE veqrptVendorLabels
AS
SELECT * FROM "veqrptVendorLabelsView"
ORDER BY "veqrptVendorLabelsView".VendorName


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[veqrptVendorLabels]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE waspfrmWarrantyParts
	@WarrantyID int = 0
as
select wa.WarrantyPartID, pa.RPSPartNum, pa.PartName, wa.PartCost
from dbo.tblWarrantyParts wa, dbo.tblParts pa
where wa.PartID = pa.PartID and wa.WarrantyID = @WarrantyID
order by pa.RPSPNSort
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[waspfrmWarrantyParts]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE wasprptDealerReimburse 
	@sDealerName varchar(40) = null,
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null,
	@sOpen varchar(2) = null, 
	@iWarrantyType int = 0
AS
-- Check input parameters
IF @sDealerName IS NULL
	SELECT @sDealerName = '%'
IF @sFromDate IS NULL
	SELECT @sFromDate = '1/1/1900'
IF @sToDate IS NULL
	SELECT @sToDate = '1/1/2100'
IF @sOpen IS NULL
	SELECT @sOpen = '%'
IF @iWarrantyType IS NULL
	SELECT @iWarrantyType = 0
-- Run query
SELECT wa.Dealer, wa.Model, wa.MachineSerialNumber, wa.CreditMemoNum, wa.CreditMemoAmt, 
	wa.RGANum, wa.DateOfFailure, wa.WarrantyOpen, wa.DealerRefNum
FROM dbo.tblWarrantyParts wp INNER JOIN dbo.tblAllWarranty wa ON wp.WarrantyID = wa.WarrantyID
WHERE wa.DateOfFailure Between @sFromDate and @sToDate
	and wa.Dealer Like @sDealerName
	and wa.WarrantyOpen Like @sOpen
	and wa.WarrantyType = @iWarrantyType
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[wasprptDealerReimburse]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE wasprptRgaClaimDates
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null,
	@iWarrantyType int = 0
AS
-- Check input parameters
IF @sFromDate IS NULL
	SELECT @sFromDate = '1/1/1900'
IF @sToDate IS NULL
	SELECT @sToDate = '1/1/2100'
IF @iWarrantyType IS NULL
	SELECT @iWarrantyType = 0
SELECT dl.DealerName, dl.StreetAddress, dl.City, dl.State, dl.Zip, 
	dl.City + quotename(', ','''') + dl.State + quotename('  ', '''') + dl.Zip AS CityStateZip,
	wa.Customer, wa.DateOfFailure, wa.RGANum, wa.DateEntered, wa.Comment, wa.WarrantyID, 
	wa.MachineSerialNumber, wa.Problem, wa.Resolution, wa.DealerRefNum, wa.WarrantyType
FROM dbo.tblAllDealers dl INNER JOIN dbo.tblAllWarranty wa ON dl.DealerID = wa.fkDealerID
WHERE wa.DateEntered Between @sFromDate And @sToDate
	AND wa.WarrantyType = @iWarrantyType
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[wasprptRgaClaimDates]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE wasprptWarrantyCosts 
	@sPartName varchar(40) = null,
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null,
	@sOpen varchar(2) = null,
	@iWarrantyType int = 0
AS
-- Check input parameters
IF @sPartName IS NULL
	SELECT @sPartName = '%'
IF @sFromDate IS NULL
	SELECT @sFromDate = '1/1/1900'
IF @sToDate IS NULL
	SELECT @sToDate = '1/1/2100'
IF @sOpen IS NULL
	SELECT @sOpen = '%'
IF @iWarrantyType IS NULL
	SELECT @iWarrantyType = 0
-- Run query
SELECT     wa.MachineSerialNumber, wa.DateOfFailure, wa.CreditMemoNum, wa.Dealer, wa.Customer, wa.RGANum, wa.LaborCost, wa.Freight, wa.Problem, 
                      wa.WarrantyID, pa.RPSPartNum, pa.PartName, wp.PartCost, wa.Model, wa.Hours, wa.WarrantyOpen, pa.RPSPNSort, wa.DealerRefNum
FROM         dbo.tblParts pa RIGHT OUTER JOIN
                      dbo.tblWarrantyParts wp RIGHT OUTER JOIN
                      dbo.tblAllWarranty wa ON wp.WarrantyID = wa.WarrantyID ON pa.PartID = wp.PartID
WHERE wa.DateOfFailure Between @sFromDate and @sToDate
	and pa.RPSPartNum like @sPartName
	and wa.WarrantyOpen like @sOpen
	and wa.WarrantyType = @iWarrantyType
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[wasprptWarrantyCosts]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE wasprptWarrantyPending
	@iWarrantyType int = 0,
	@sDealerName varchar(255) = null
AS
IF @sDealerName is null
	select @sDealerName = '%'
SELECT     wa.Dealer, wa.MachineSerialNumber, wa.RGANum, pa.RPSPartNum, wa.WarrantyOpen, wa.Hours, wa.DealerRefNum
FROM         dbo.tblAllWarranty wa LEFT OUTER JOIN
                      dbo.tblWarrantyParts wp ON wa.WarrantyID = wp.WarrantyID LEFT OUTER JOIN
                      dbo.tblParts pa ON wp.PartID = pa.PartID
WHERE     (wa.WarrantyType = @iWarrantyType) AND (wa.WarrantyOpen = 1)
	and wa.Dealer like @sDealerName
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[wasprptWarrantyPending]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE wasprptWarrantyRGA
	@iWarrantyType int = 0
AS
SELECT wa.RGANum, wa.MachineSerialNumber, wa.DateOfFailure, wa.CreditMemoNum, wa.Dealer, wa.Customer, wa.LaborCost, 
	wa.Freight, wa.Problem, pa.RPSPartNum, wa.WarrantyID, pa.PartName, wp.PartCost, wa.WarrantyOpen, wa.WarrantyType, wa.DealerRefNum
FROM dbo.tblParts pa RIGHT OUTER JOIN dbo.tblWarrantyParts wp INNER JOIN dbo.tblAllWarranty wa ON 
	wp.WarrantyID = wa.WarrantyID ON pa.PartID = wp.PartID
WHERE wa.WarrantyOpen = 1 and wa.WarrantyType = @iWarrantyType
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[wasprptWarrantyRGA]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE wasprptWarrantyTotalCost
	@sDealerName varchar(40) = null,
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null,
	@sOpen varchar(2) = null,
	@sFromDate2 varchar(20) = null,
	@sToDate2 varchar(20) = null,
	@iWarrantyType int = 0
AS
-- Check input parameters
IF @sDealerName IS NULL
	SELECT @sDealerName = '%'
IF @sFromDate IS NULL
	SELECT @sFromDate = '1/1/1900'
IF @sToDate IS NULL
	SELECT @sToDate = '1/1/2100'
IF @sOpen IS NULL
	SELECT @sOpen = '%'
IF @sFromDate2 IS NULL
	SELECT @sFromDate2 = '1/1/1900'
IF @sToDate2 IS NULL
	SELECT @sToDate2 = '1/1/2100'
IF @iWarrantyType IS NULL
	SELECT @iWarrantyType = 0
-- Run query
SELECT wa.Dealer, wa.Model, wa.MachineSerialNumber, wa.DateOfFailure, wa.LaborCost, 
	SUM(wp.PartCost) AS ExtPartCost, wa.Travel, wa.Hours
FROM dbo.tblAllWarranty wa INNER JOIN dbo.tblWarrantyParts wp ON wa.WarrantyID = wp.WarrantyID 
WHERE wa.DateOfFailure Between @sFromDate and @sToDate
	and DateEntered Between @sFromDate2 and @sToDate2
	and wa.Dealer like @sDealerName
	and wa.WarrantyOpen like @sOpen
	and wa.WarrantyType = @iWarrantyType
GROUP BY wa.Dealer, wa.Model, wa.MachineSerialNumber, wa.DateOfFailure, wa.LaborCost, wa.Travel, wa.Hours
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[wasprptWarrantyTotalCost]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE wasprsubRGAParts
	@sID varchar(10) = null
AS
IF @sID = null
	SELECT @sID = 0
select pa.RPSPartNum, pa.PartName, wp.WarrantyID
from dbo.tblParts pa inner join dbo.tblWarrantyParts wp ON pa.PartID = wp.PartID
where wp.WarrantyID = @sID
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[wasprsubRGAParts]  TO [fcuser]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[ppsprptPartsOnHold]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ppsprptPartsOnHold]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  OFF 
GO



CREATE PROCEDURE ppsprptPartsOnHold

AS

SELECT po.POID, po.Vendor, pp.RPSPartNum, pp.PartDescription, 
	pd.RequestedShipDate, pd.Quantity, pd.Value, pd.CostEach, pd.Notes, pa.RPSPNSort
FROM dbo.tblPO po, dbo.tblPOPart pp, dbo.tblPOPartDetail pd, dbo.tblParts pa
WHERE po.POID = pp.fkPOID and pp.POPartID = pd.fkPOPartID
	and pa.RPSPartNum = pp.RPSPartNum
	and pd.Quantity = 0
	and pd.RequestedShipDate is not null and pd.ReceivedDate is null


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[ppsprptPartsOnHold]  TO [fcuser]
GO



insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('4.00','2000-7-1 00:00:00','- Prod Parts:  Added In House date to ''Parts on Hold'' report and restricted to open releases
- Database: Created a standalone version
- Database: New menu system 
- Warranty: Increased width of Dealer Ref # field in reports
- Orders: Margin should calculate correctly (FC/TC)
- 
')
go

update tblDBProperties
set PropertyValue = '4.00'
where PropertyName = 'DBStructVersion'
go
