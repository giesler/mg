ALTER TABLE [dbo].[tblDealerAdvert] DROP CONSTRAINT tblDealerAdvert_FK00
GO

ALTER TABLE [dbo].[tblDealersGoals] DROP CONSTRAINT tblDealersGoals_FK00
GO

ALTER TABLE [dbo].[tblPOPart] DROP CONSTRAINT tblPODetail_FK00
GO

ALTER TABLE [dbo].[tblPOPartDetail] DROP CONSTRAINT FK_tblPOPartDetail_tblPOPart
GO

ALTER TABLE [dbo].[tblWarrantyParts] DROP CONSTRAINT FK_tblWarrantyParts_tblWarranty
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblLeads_UTrig]') and OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [dbo].[tblLeads_UTrig]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblLeads_ITrig]') and OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [dbo].[tblLeads_ITrig]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblParts_ITrig]') and OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [dbo].[tblParts_ITrig]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblParts_UTring]') and OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [dbo].[tblParts_UTring]
GO

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

if exists (select * from sysobjects where id = object_id(N'[dbo].[pasprptBillOfMaterials]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[pasprptBillOfMaterials]
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

if exists (select * from sysobjects where id = object_id(N'[dbo].[ppqrptPOCosts]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ppqrptPOCosts]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[ppqrptVendorRelease]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ppqrptVendorRelease]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[ppsprptCashFlow]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ppsprptCashFlow]
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

if exists (select * from sysobjects where id = object_id(N'[dbo].[spSecurityCheck]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spSecurityCheck]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[ut_qry33]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ut_qry33]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[veqrptVendorLabels]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[veqrptVendorLabels]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[waqrptWarrantyRGANums]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[waqrptWarrantyRGANums]
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

if exists (select * from sysobjects where id = object_id(N'[dbo].[wasprptWarrantyTotalCost]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[wasprptWarrantyTotalCost]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[wasprsubRGAParts]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[wasprsubRGAParts]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[dlqrptAdvertAlloc]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[dlqrptAdvertAlloc]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[dlqrptDealerListAddressView]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[dlqrptDealerListAddressView]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[dlqrptDealerListAllDealersView]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[dlqrptDealerListAllDealersView]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[dlqrptDealerListFaxView]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[dlqrptDealerListFaxView]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[dlqrptDealerListPhoneView]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[dlqrptDealerListPhoneView]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[dlqrptMaillingLabelServicePartsManView]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[dlqrptMaillingLabelServicePartsManView]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[dlqrptMaillingLabelsView]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[dlqrptMaillingLabelsView]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[dlqrptMaillingServiceView]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[dlqrptMaillingServiceView]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[dmqrptDealerDemoReportView]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[dmqrptDealerDemoReportView]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[ldqfrmLeads]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[ldqfrmLeads]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[ldqrptMaillingLabelsView]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[ldqrptMaillingLabelsView]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[ldqrptResponseMethodView]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[ldqrptResponseMethodView]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[lsqrptStatesDealerListView]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[lsqrptStatesDealerListView]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[lsqrptStatesLastMailedView]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[lsqrptStatesLastMailedView]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[lsqrptStatesNotMailedView]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[lsqrptStatesNotMailedView]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[maqrptMajorAccountInfoPage]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[maqrptMajorAccountInfoPage]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[mlvCompany]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[mlvCompany]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[mlvContact]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[mlvContact]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[mlvMaillist]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[mlvMaillist]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[nwqrptAddressListingView]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[nwqrptAddressListingView]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[nwqrptMailingLabelsView]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[nwqrptMailingLabelsView]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[paqrptListPricesView]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[paqrptListPricesView]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[paqrptPartLabels]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[paqrptPartLabels]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[paqrptPartLabelsMulti]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[paqrptPartLabelsMulti]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[paqrptPartListFinishView]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[paqrptPartListFinishView]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[paqrptPartsPerModel2]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[paqrptPartsPerModel2]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[parptRPSPartsPricingView]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[parptRPSPartsPricingView]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[parqryDealerPartsListView]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[parqryDealerPartsListView]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[ppqfrmProdParts]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[ppqfrmProdParts]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[ppqrptOpenPOsDetailed]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[ppqrptOpenPOsDetailed]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[qselContactTitle]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[qselContactTitle]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblLeads]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[tblLeads]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblOrders]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[tblOrders]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblPurLeads]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[tblPurLeads]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblTCLeads]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[tblTCLeads]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblTCOrders]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[tblTCOrders]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tempView]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[tempView]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[veqrptVendorLabelsView]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[veqrptVendorLabelsView]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[vSwitchboard]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[vSwitchboard]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[waqfrmWarranty]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[waqfrmWarranty]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[waqrptRgaClaim]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[waqrptRgaClaim]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[waqrptWarrantyPending]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[waqrptWarrantyPending]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[pltblImportedFile]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[pltblImportedFile]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[Switchboard Items]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Switchboard Items]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[Tbl_Numbers]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Tbl_Numbers]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblAllLeads]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblAllLeads]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblAllOrders]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblAllOrders]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblDBProperties]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblDBProperties]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblDealerAdvert]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblDealerAdvert]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblDealerDemos]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblDealerDemos]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblDealerPartsList]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblDealerPartsList]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblDealers]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblDealers]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblDealersGoals]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblDealersGoals]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblLists]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblLists]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblMajorAccounts]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblMajorAccounts]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblMenuHistory]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblMenuHistory]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblModels]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblModels]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblNewsletter]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblNewsletter]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblOrderPrepByList]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblOrderPrepByList]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblParts]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblParts]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblPartsModels]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblPartsModels]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblPartsSubParts]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblPartsSubParts]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblPO]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblPO]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblPOPart]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblPOPart]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblPOPartDetail]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblPOPartDetail]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblProdSchedItems]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblProdSchedItems]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblProdSchedules]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblProdSchedules]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblProspects]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblProspects]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblResponseMethod]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblResponseMethod]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblSecurity]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblSecurity]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblSysMessage]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblSysMessage]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblTip]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblTip]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblTipDisable]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblTipDisable]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblToDo]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblToDo]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblUser]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblUser]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblVendors]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblVendors]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblVersion]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblVersion]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblWarranty]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblWarranty]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblWarrantyParts]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblWarrantyParts]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tlkuContactTitles]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tlkuContactTitles]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tlkuFinish]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tlkuFinish]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tmpLabelQtys]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tmpLabelQtys]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tmpNames]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tmpNames]
GO

if not exists (select * from master..syslogins where name = N'fcuser')
BEGIN
	declare @logindb nvarchar(132), @loginlang nvarchar(132) select @logindb = N'fcdata', @loginlang = N'us_english'
	if @logindb is null or not exists (select * from master..sysdatabases where name = @logindb)
		select @logindb = N'master'
	if @loginlang is null or (not exists (select * from master..syslanguages where name = @loginlang) and @loginlang <> N'us_english')
		select @loginlang = @@language
	exec sp_addlogin N'fcuser', null, @logindb, @loginlang
END
GO

if not exists (select * from master..syslogins where name = N'stocks_login')
BEGIN
	declare @logindb nvarchar(132), @loginlang nvarchar(132) select @logindb = N'stocks', @loginlang = N'us_english'
	if @logindb is null or not exists (select * from master..sysdatabases where name = @logindb)
		select @logindb = N'master'
	if @loginlang is null or (not exists (select * from master..syslanguages where name = @loginlang) and @loginlang <> N'us_english')
		select @loginlang = @@language
	exec sp_addlogin N'stocks_login', null, @logindb, @loginlang
END
GO

if not exists (select * from master..syslogins where name = N'BUILTIN\Users')
	exec sp_grantlogin N'BUILTIN\Users'
	exec sp_defaultdb N'BUILTIN\Users', N'blidata'
	exec sp_defaultlanguage N'BUILTIN\Users', N'us_english'
GO

exec sp_addsrvrolemember N'stocks_login', sysadmin
GO

if not exists (select * from sysusers where name = N'fcgenuser' and uid > 16399)
	EXEC sp_addrole N'fcgenuser'
GO

exec sp_addrolemember N'db_datareader', N'fcuser'
GO

exec sp_addrolemember N'db_datawriter', N'fcuser'
GO

CREATE TABLE [dbo].[pltblImportedFile] (
	[DateOfLead] [datetime] NULL ,
	[CompanyName] [nvarchar] (50) NULL ,
	[ContactName] [nvarchar] (50) NULL ,
	[Title] [nvarchar] (50) NULL ,
	[Address] [nvarchar] (50) NULL ,
	[City] [nvarchar] (50) NULL ,
	[State] [nvarchar] (2) NULL ,
	[Zip] [nvarchar] (5) NULL ,
	[Phone] [nvarchar] (50) NULL ,
	[SIC] [nvarchar] (50) NULL ,
	[Entry] [ntext] NULL ,
	[Include] [bit] NOT NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[Switchboard Items] (
	[ID] [int] IDENTITY (1, 1) NOT NULL ,
	[SwitchboardID] [int] NOT NULL ,
	[ItemNumber] [smallint] NOT NULL ,
	[ItemText] [nvarchar] (255) NULL ,
	[Command] [smallint] NULL ,
	[Argument] [nvarchar] (255) NULL ,
	[Tooltip] [nvarchar] (100) NULL ,
	[OpenArgs] [nvarchar] (100) NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Tbl_Numbers] (
	[Number] [real] NULL ,
	[Counter] [int] IDENTITY (1, 1) NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblAllLeads] (
	[LeadID] [int] IDENTITY (1, 1) NOT NULL ,
	[DealerName] [nvarchar] (50) NULL ,
	[Location] [nvarchar] (50) NULL ,
	[LeadDate] [datetime] NULL ,
	[Salesman] [nvarchar] (50) NULL ,
	[CompanyName] [nvarchar] (100) NULL ,
	[Contact] [nvarchar] (100) NULL ,
	[ContactTitle] [nvarchar] (50) NULL ,
	[Address] [nvarchar] (50) NULL ,
	[City] [nvarchar] (20) NULL ,
	[State] [nvarchar] (20) NULL ,
	[Zip] [nvarchar] (20) NULL ,
	[Phone] [nvarchar] (30) NULL ,
	[SIC] [nvarchar] (20) NULL ,
	[Size] [nvarchar] (20) NULL ,
	[ApplicationNotes] [ntext] NULL ,
	[ResponseMethod] [nvarchar] (20) NULL ,
	[Purchase] [nvarchar] (50) NULL ,
	[Code] [nvarchar] (50) NULL ,
	[ActiveInactive] [nchar] (10) NULL ,
	[Result] [ntext] NULL ,
	[SendInfoOn] [nvarchar] (50) NULL ,
	[DealerNumber] [nvarchar] (50) NULL ,
	[Purchased] [tinyint] NOT NULL ,
	[upsize_ts] [timestamp] NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblAllOrders] (
	[OrderID] [int] IDENTITY (1, 1) NOT NULL ,
	[Model] [smallint] NULL ,
	[OrderDate] [datetime] NULL ,
	[OrderNumber] [nvarchar] (50) NULL ,
	[OrderKey] [int] NULL ,
	[Dealer] [nvarchar] (50) NULL ,
	[PurchaseOrder] [nvarchar] (50) NULL ,
	[Quantity] [smallint] NULL ,
	[PromisedDate] [datetime] NULL ,
	[ShippedDate] [datetime] NULL ,
	[Battery] [nchar] (10) NULL ,
	[Size] [nchar] (10) NULL ,
	[AmpCharger] [nchar] (10) NULL ,
	[HourMeter] [nchar] (10) NULL ,
	[SerialNumber] [nchar] (30) NULL ,
	[TwelveVMotor] [nchar] (15) NULL ,
	[Eighteen1hpMotor] [nchar] (10) NULL ,
	[TwoHP] [nchar] (10) NULL ,
	[SalePrice] [money] NULL ,
	[CostPrice] [money] NULL ,
	[Margin] [money] NULL ,
	[Terms] [nvarchar] (50) NULL ,
	[ShipVia] [nvarchar] (50) NULL ,
	[CollectPrepaid] [nchar] (30) NULL ,
	[Notes] [ntext] NULL ,
	[Plus2Batteries] [nchar] (10) NULL ,
	[FortyAmp] [nchar] (10) NULL ,
	[Horn] [nchar] (10) NULL ,
	[Alarm] [nchar] (10) NULL ,
	[Name] [nvarchar] (100) NULL ,
	[SaleDate] [datetime] NULL ,
	[Address] [nvarchar] (100) NULL ,
	[City] [nvarchar] (50) NULL ,
	[State] [nvarchar] (20) NULL ,
	[Zip] [nvarchar] (20) NULL ,
	[ContactName] [nvarchar] (100) NULL ,
	[Phone] [nvarchar] (35) NULL ,
	[EighteenMonthOption] [nchar] (10) NULL ,
	[DealerDemo] [nchar] (10) NULL ,
	[StandardWarranty] [nchar] (10) NULL ,
	[LastDateMailedInfoTo] [datetime] NULL ,
	[Note] [ntext] NULL ,
	[ShipName] [nvarchar] (100) NULL ,
	[StreetAddress] [nvarchar] (100) NULL ,
	[CityStateZip] [nvarchar] (100) NULL ,
	[TagForEndUserReport] [nchar] (10) NULL ,
	[TypeOfBusiness] [nvarchar] (50) NULL ,
	[PartialList] [nvarchar] (50) NULL ,
	[LastUsedDate] [datetime] NULL ,
	[ContactedDate] [datetime] NULL ,
	[ContactedBy] [nvarchar] (50) NULL ,
	[Options] [ntext] NULL ,
	[SICCode] [float] NULL ,
	[TermsInfo] [nvarchar] (50) NULL ,
	[fkDealerID] [int] NULL ,
	[MajorAccount] [bit] NULL ,
	[MajorAccountID] [int] NULL ,
	[fkServDealerID] [int] NULL ,
	[OrderType] [tinyint] NOT NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblDBProperties] (
	[PropertyName] [nvarchar] (20) NOT NULL ,
	[PropertyValue] [nvarchar] (20) NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblDealerAdvert] (
	[AdvertID] [int] IDENTITY (1, 1) NOT NULL ,
	[fkDealerID] [int] NULL ,
	[AdvertDate] [datetime] NULL ,
	[AdvertAmt] [money] NULL ,
	[AdvertNote] [ntext] NULL ,
	[upsize_ts] [timestamp] NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblDealerDemos] (
	[DealerDemoID] [int] IDENTITY (1, 1) NOT NULL ,
	[fkDealerID] [int] NULL ,
	[CustomerName] [nvarchar] (50) NULL ,
	[ContactName] [nvarchar] (50) NULL ,
	[Phone] [nvarchar] (50) NULL ,
	[Address] [nvarchar] (50) NULL ,
	[City] [nvarchar] (30) NULL ,
	[State] [nvarchar] (2) NULL ,
	[Zip] [nvarchar] (11) NULL ,
	[DateOfDemo] [datetime] NULL ,
	[EquipmentDemoed] [ntext] NULL ,
	[Notes] [ntext] NULL ,
	[Purchased] [bit] NOT NULL ,
	[Model40] [smallint] NULL ,
	[Model27] [smallint] NULL ,
	[Model10] [smallint] NULL ,
	[Model39] [smallint] NULL ,
	[Model34] [smallint] NULL ,
	[Model48] [smallint] NULL ,
	[upsize_ts] [timestamp] NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblDealerPartsList] (
	[DealerPartID] [int] IDENTITY (1, 1) NOT NULL ,
	[RPSPartNum] [nvarchar] (50) NULL ,
	[PartName] [nvarchar] (150) NULL ,
	[DealerNetPrice] [money] NULL ,
	[SuggestedListPrice] [money] NULL ,
	[QuantityRequired] [int] NULL ,
	[PageReference] [int] NULL ,
	[Notes] [nvarchar] (255) NULL ,
	[upsize_ts] [timestamp] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblDealers] (
	[DealerID] [int] IDENTITY (1, 1) NOT NULL ,
	[CurrentDealer] [bit] NOT NULL ,
	[DealerName] [nvarchar] (100) NULL ,
	[TollFreeNumber] [nvarchar] (20) NULL ,
	[ContactName] [nvarchar] (50) NULL ,
	[ContactTitle] [nvarchar] (50) NULL ,
	[StreetAddress] [nvarchar] (50) NULL ,
	[City] [nvarchar] (50) NULL ,
	[State] [nvarchar] (20) NULL ,
	[Zip] [nvarchar] (20) NULL ,
	[Phone] [nvarchar] (20) NULL ,
	[Fax] [nvarchar] (20) NULL ,
	[Num] [nvarchar] (20) NULL ,
	[SweeperDealer] [bit] NOT NULL ,
	[CarPhoneNumber] [nvarchar] (20) NULL ,
	[TerritoryCovered] [nvarchar] (100) NULL ,
	[NumSalesman] [nvarchar] (20) NULL ,
	[MajorProducts] [nvarchar] (100) NULL ,
	[SalesmensNames] [nvarchar] (100) NULL ,
	[FirstName] [nvarchar] (50) NULL ,
	[LastName] [nvarchar] (50) NULL ,
	[HomePhoneNumber] [nvarchar] (20) NULL ,
	[Notes] [ntext] NULL ,
	[ContractExpires] [datetime] NULL ,
	[SalesmanName] [nvarchar] (50) NULL ,
	[WarrentyAdministrator] [nvarchar] (50) NULL ,
	[ServiceManagerName] [nvarchar] (50) NULL ,
	[LaborRate] [money] NULL ,
	[PartsManagerName] [nvarchar] (50) NULL ,
	[OfficeManagerName] [nvarchar] (50) NULL ,
	[Terms] [nvarchar] (50) NULL ,
	[upsize_ts] [timestamp] NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblDealersGoals] (
	[fkDealerID] [int] NOT NULL ,
	[Year] [nvarchar] (4) NOT NULL ,
	[Model] [smallint] NOT NULL ,
	[Goal] [smallint] NULL ,
	[upsize_ts] [timestamp] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblLists] (
	[ListID] [int] IDENTITY (1, 1) NOT NULL ,
	[fkDealerID] [int] NULL ,
	[State] [nvarchar] (50) NULL ,
	[County] [nvarchar] (50) NULL ,
	[DateListOrdered] [datetime] NULL ,
	[LastDateMailed] [datetime] NULL ,
	[Flier] [smallint] NULL ,
	[ListCompany] [nvarchar] (50) NULL ,
	[Notes] [ntext] NULL ,
	[upsize_ts] [timestamp] NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblMajorAccounts] (
	[MajorAccountID] [int] IDENTITY (1, 1) NOT NULL ,
	[fkMADealerID] [int] NULL ,
	[MACompName] [nvarchar] (50) NULL ,
	[MAHeadqAddress] [nvarchar] (50) NULL ,
	[MACity] [nvarchar] (30) NULL ,
	[MAState] [nvarchar] (2) NULL ,
	[MAZip] [nvarchar] (10) NULL ,
	[MAPurContact] [nvarchar] (50) NULL ,
	[MAManageContact] [nvarchar] (50) NULL ,
	[MAPhone] [nvarchar] (20) NULL ,
	[MAFax] [nvarchar] (20) NULL ,
	[MANumLocations] [int] NULL ,
	[MALocations] [nvarchar] (20) NULL ,
	[MASerialNums] [ntext] NULL ,
	[MAInitialPO] [ntext] NULL ,
	[MAApproved] [bit] NOT NULL ,
	[MADenied] [bit] NOT NULL ,
	[MAAccountNum] [nvarchar] (15) NULL ,
	[MAAccountNumKey] [int] NULL ,
	[MANotes] [ntext] NULL ,
	[MADateApproved] [datetime] NULL ,
	[upsize_ts] [timestamp] NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblMenuHistory] (
	[HistoryID] [int] IDENTITY (1, 1) NOT NULL ,
	[HistoryTime] [datetime] NULL ,
	[HistoryUser] [char] (20) NULL ,
	[HistorySwitchID] [int] NULL ,
	[HistorySwitchItem] [int] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblModels] (
	[ModelID] [int] IDENTITY (1, 1) NOT NULL ,
	[Model] [smallint] NULL ,
	[Description] [ntext] NULL ,
	[Price] [money] NULL ,
	[upsize_ts] [timestamp] NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblNewsletter] (
	[NewsletterID] [int] IDENTITY (1, 1) NOT NULL ,
	[DealerFlag] [bit] NOT NULL ,
	[DealerName] [nvarchar] (40) NULL ,
	[Name] [nvarchar] (37) NULL ,
	[Salutation] [nvarchar] (17) NULL ,
	[StreetAddress] [nvarchar] (26) NULL ,
	[City] [nvarchar] (19) NULL ,
	[State] [nvarchar] (4) NULL ,
	[Zip] [nvarchar] (13) NULL ,
	[Phone] [nvarchar] (20) NULL ,
	[Fax] [nvarchar] (22) NULL ,
	[Notes] [ntext] NULL ,
	[fkDealerID] [smallint] NULL ,
	[upsize_ts] [timestamp] NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblOrderPrepByList] (
	[PrepByName] [nvarchar] (50) NOT NULL ,
	[PrepByDefault] [bit] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblParts] (
	[PartID] [int] IDENTITY (1, 1) NOT NULL ,
	[Model] [nvarchar] (30) NULL ,
	[RPSPurchased] [bit] NOT NULL ,
	[BlanketRelease] [nvarchar] (20) NULL ,
	[DateOrdered] [datetime] NULL ,
	[LeadTime] [nvarchar] (20) NULL ,
	[Qty] [smallint] NULL ,
	[PartName] [nvarchar] (150) NULL ,
	[Location] [nvarchar] (150) NULL ,
	[VendorName] [nvarchar] (100) NULL ,
	[VendorPartName] [nvarchar] (150) NULL ,
	[ManfPartNum] [nvarchar] (50) NULL ,
	[RPSPartNum] [nvarchar] (50) NULL ,
	[CostEach] [money] NULL ,
	[Notes] [nvarchar] (255) NULL ,
	[QuoteDate] [datetime] NULL ,
	[QtyReq] [int] NULL ,
	[TotalCostPerMachine] [money] NULL ,
	[TotalCostPerUnitForPart] [money] NULL ,
	[PageRef] [int] NULL ,
	[VendorCost] [money] NULL ,
	[AutoCalcPrice] [nvarchar] (10) NULL ,
	[SubPartTotal] [money] NULL ,
	[DealerNet] [money] NULL ,
	[SuggestedList] [money] NULL ,
	[Note] [nvarchar] (255) NULL ,
	[CanadianDealerNet] [money] NULL ,
	[Quantity] [nvarchar] (50) NULL ,
	[Total] [float] NULL ,
	[CanadianSuggestedList] [money] NULL ,
	[GSAList] [money] NULL ,
	[FrenchPartDescription] [nvarchar] (150) NULL ,
	[SubFlag] [bit] NOT NULL ,
	[ShelfNo] [float] NULL ,
	[Section] [nvarchar] (10) NULL ,
	[PartLevel] [float] NULL ,
	[HideOnReports] [bit] NOT NULL ,
	[PartOption] [bit] NOT NULL ,
	[EffectiveDate] [datetime] NULL ,
	[SNEffective] [nvarchar] (50) NULL ,
	[FinishDesc] [nvarchar] (50) NULL ,
	[RunQuantity] [int] NULL ,
	[LeadTimeDays] [nvarchar] (50) NULL ,
	[PartCode] [nvarchar] (1) NULL ,
	[DrawingNum] [nvarchar] (50) NULL ,
	[RevisionNum] [nvarchar] (50) NULL ,
	[RPSPNSort] [char] (30) NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblPartsModels] (
	[PartModelID] [int] IDENTITY (1, 1) NOT NULL ,
	[fkPartID] [int] NULL ,
	[Model] [smallint] NULL ,
	[Quantity] [float] NULL ,
	[upsize_ts] [timestamp] NULL ,
	[Optional] [bit] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblPartsSubParts] (
	[SubPartID] [int] IDENTITY (1, 1) NOT NULL ,
	[SubNum] [nvarchar] (50) NULL ,
	[SubDescription] [nvarchar] (100) NULL ,
	[SubCost] [money] NULL ,
	[SubExtCost] [money] NULL ,
	[SubSource] [nvarchar] (50) NULL ,
	[SubSourcePartNum] [nvarchar] (50) NULL ,
	[SubQty] [int] NULL ,
	[fkPartID] [int] NULL ,
	[upsize_ts] [timestamp] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblPO] (
	[POID] [int] NOT NULL ,
	[Vendor] [nvarchar] (50) NULL ,
	[Confirmed] [bit] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblPOPart] (
	[POPartID] [int] IDENTITY (1, 1) NOT NULL ,
	[fkPOID] [int] NULL ,
	[VendorPartNumber] [nvarchar] (50) NULL ,
	[RPSPartNum] [nvarchar] (50) NULL ,
	[PartDescription] [nvarchar] (100) NULL ,
	[combo] [nvarchar] (50) NULL ,
	[upsize_ts] [timestamp] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblPOPartDetail] (
	[Quantity] [int] NULL ,
	[RequestedShipDate] [datetime] NULL ,
	[ReceivedDate] [datetime] NULL ,
	[QuantityReceived] [int] NULL ,
	[CostEach] [money] NULL ,
	[Value] [money] NULL ,
	[Notes] [nvarchar] (255) NULL ,
	[POPartDetailID] [int] IDENTITY (1, 1) NOT NULL ,
	[fkPOPartID] [int] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblProdSchedItems] (
	[ScheduleID] [int] NOT NULL ,
	[Model] [smallint] NOT NULL ,
	[Quantity] [smallint] NOT NULL ,
	[SchedItemID] [int] IDENTITY (1, 1) NOT NULL ,
	[upsize_ts] [timestamp] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblProdSchedules] (
	[ScheduleID] [int] IDENTITY (1, 1) NOT NULL ,
	[Month] [smallint] NULL ,
	[Year] [smallint] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblProspects] (
	[ProspectID] [int] IDENTITY (1, 1) NOT NULL ,
	[CompanyName] [nvarchar] (100) NULL ,
	[FirstName] [nvarchar] (50) NULL ,
	[LastName] [nvarchar] (50) NULL ,
	[StreetAddress] [nvarchar] (100) NULL ,
	[StreetZipCode] [nvarchar] (10) NULL ,
	[POBox] [nvarchar] (20) NULL ,
	[POBoxZipCode] [nvarchar] (10) NULL ,
	[City] [nvarchar] (50) NULL ,
	[State] [char] (20) NULL ,
	[Phone] [nvarchar] (30) NULL ,
	[Fax] [nvarchar] (30) NULL ,
	[Territory] [nvarchar] (255) NULL ,
	[EquipmentCarried] [ntext] NULL ,
	[Notes] [ntext] NULL ,
	[RecordUpdateDate] [smalldatetime] NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblResponseMethod] (
	[ResposneID] [int] IDENTITY (1, 1) NOT NULL ,
	[ResponseMethod] [nvarchar] (50) NULL ,
	[ResponseMethodNotes] [ntext] NULL ,
	[upsize_ts] [timestamp] NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblSecurity] (
	[SecurityID] [int] IDENTITY (1, 1) NOT NULL ,
	[UserID] [nvarchar] (50) NOT NULL ,
	[SwID] [int] NOT NULL ,
	[AccessType] [int] NOT NULL ,
	[s] [char] (10) NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblSysMessage] (
	[SysMessageFrom] [nvarchar] (20) NULL ,
	[SysMessage] [nvarchar] (255) NULL ,
	[SysMessageTime] [datetime] NULL ,
	[SysKick] [bit] NOT NULL ,
	[SysMessageID] [int] IDENTITY (0, 1) NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblTip] (
	[TipID] [int] IDENTITY (1, 1) NOT NULL ,
	[TipArea] [char] (30) NULL ,
	[TipText] [varchar] (255) NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblTipDisable] (
	[UserName] [nvarchar] (50) NOT NULL ,
	[TipID] [int] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblToDo] (
	[ToDoID] [int] IDENTITY (1, 1) NOT NULL ,
	[ToDoArea] [varchar] (50) NULL ,
	[ToDoNote] [ntext] NULL ,
	[ToDoDone] [bit] NOT NULL ,
	[ToDoEnteredDate] [datetime] NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblUser] (
	[UserID] [int] IDENTITY (1, 1) NOT NULL ,
	[UserName] [nvarchar] (50) NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblVendors] (
	[VendorID] [int] IDENTITY (1, 1) NOT NULL ,
	[VendorName] [nvarchar] (50) NULL ,
	[CointactName] [nvarchar] (50) NULL ,
	[StreetAddress] [nvarchar] (50) NULL ,
	[City] [nvarchar] (30) NULL ,
	[State] [nvarchar] (2) NULL ,
	[Zip] [nvarchar] (10) NULL ,
	[Phone] [nvarchar] (20) NULL ,
	[Fax] [nvarchar] (20) NULL ,
	[BlanketProductionOrders] [bit] NOT NULL ,
	[RPSPartNum] [nvarchar] (50) NULL ,
	[VendorPartNum] [nvarchar] (50) NULL ,
	[PartDescription] [ntext] NULL ,
	[Notes] [ntext] NULL ,
	[Active] [bit] NOT NULL ,
	[Email] [nvarchar] (50) NULL ,
	[upsize_ts] [timestamp] NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblVersion] (
	[VersionID] [int] IDENTITY (1, 1) NOT NULL ,
	[VersionNumber] [nvarchar] (50) NULL ,
	[VersionDate] [smalldatetime] NULL ,
	[VersionRelNotes] [ntext] NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblWarranty] (
	[WarrantyID] [int] IDENTITY (1, 1) NOT NULL ,
	[MachineSerialNumber] [nvarchar] (50) NULL ,
	[DateOfFailure] [datetime] NULL ,
	[CreditMemoNum] [nvarchar] (50) NULL ,
	[CreditMemoAmt] [money] NULL ,
	[Dealer] [nvarchar] (50) NULL ,
	[Customer] [nvarchar] (50) NULL ,
	[RGANum] [float] NULL ,
	[PartCost] [float] NULL ,
	[LaborCost] [float] NULL ,
	[Freight] [float] NULL ,
	[Problem] [ntext] NULL ,
	[Model] [smallint] NULL ,
	[Resolution] [ntext] NULL ,
	[WarrantyOpen] [bit] NULL ,
	[Travel] [money] NULL ,
	[Policy] [money] NULL ,
	[DateEntered] [datetime] NULL ,
	[PartReceived] [bit] NULL ,
	[Hours] [real] NULL ,
	[fkDealerID] [smallint] NULL ,
	[Comment] [ntext] NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblWarrantyParts] (
	[WarrantyPartID] [int] IDENTITY (1, 1) NOT NULL ,
	[fkWarrantyID] [int] NULL ,
	[PartNumReplaced] [nvarchar] (50) NULL ,
	[PartDescription] [nvarchar] (200) NULL ,
	[PartCost] [money] NULL ,
	[PartFileIndex] [int] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tlkuContactTitles] (
	[ContactTitle] [nvarchar] (50) NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tlkuFinish] (
	[FinishID] [int] IDENTITY (1, 1) NOT NULL ,
	[FinishDesc] [nvarchar] (50) NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tmpLabelQtys] (
	[PartIndex] [int] NOT NULL ,
	[Quantity] [smallint] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tmpNames] (
	[PersonName] [nvarchar] (50) NULL ,
	[DealerIndex] [int] NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[pltblImportedFile] WITH NOCHECK ADD 
	CONSTRAINT [DF__Temporary__Inclu__0DAF0CB0] DEFAULT (0) FOR [Include]
GO

ALTER TABLE [dbo].[Switchboard Items] WITH NOCHECK ADD 
	CONSTRAINT [DF__Temporary__ItemN__1273C1CD] DEFAULT (0) FOR [ItemNumber],
	CONSTRAINT [aaaaaSwitchboard Items_PK] PRIMARY KEY  NONCLUSTERED 
	(
		[SwitchboardID],
		[ItemNumber]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Tbl_Numbers] WITH NOCHECK ADD 
	CONSTRAINT [DF__Temporary__Numbe__164452B1] DEFAULT (0) FOR [Number],
	CONSTRAINT [PK_Tbl_Numbers] PRIMARY KEY  NONCLUSTERED 
	(
		[Counter]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblAllLeads] WITH NOCHECK ADD 
	CONSTRAINT [DF__Temporary__Activ__3F466844] DEFAULT ('A') FOR [ActiveInactive],
	CONSTRAINT [DF__Temporary__Purch__403A8C7D] DEFAULT (0) FOR [Purchased],
	CONSTRAINT [tblLeads_PK] PRIMARY KEY  NONCLUSTERED 
	(
		[LeadID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblAllOrders] WITH NOCHECK ADD 
	CONSTRAINT [DF_tblOrders_MajorAccountID] DEFAULT (0) FOR [MajorAccountID],
	CONSTRAINT [DF_tblOrders_TomCatOrder] DEFAULT (0) FOR [OrderType],
	CONSTRAINT [PK_tblOrders] PRIMARY KEY  NONCLUSTERED 
	(
		[OrderID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblDealerAdvert] WITH NOCHECK ADD 
	CONSTRAINT [DF__Temporary__fkDea__30F848ED] DEFAULT (0) FOR [fkDealerID],
	CONSTRAINT [DF__Temporary__Adver__32E0915F] DEFAULT (0) FOR [AdvertAmt],
	CONSTRAINT [aaaaatblDealerAdvert_PK] PRIMARY KEY  NONCLUSTERED 
	(
		[AdvertID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblDealerDemos] WITH NOCHECK ADD 
	CONSTRAINT [DF__Temporary__fkDea__21B6055D] DEFAULT (0) FOR [fkDealerID],
	CONSTRAINT [DF__Temporary__Purch__22AA2996] DEFAULT (0) FOR [Purchased],
	CONSTRAINT [DF__Temporary__Model__239E4DCF] DEFAULT (0) FOR [Model40],
	CONSTRAINT [DF__Temporary__Model__24927208] DEFAULT (0) FOR [Model27],
	CONSTRAINT [DF__Temporary__Model__25869641] DEFAULT (0) FOR [Model10],
	CONSTRAINT [DF__Temporary__Model__267ABA7A] DEFAULT (0) FOR [Model39],
	CONSTRAINT [DF__Temporary__Model__276EDEB3] DEFAULT (0) FOR [Model34],
	CONSTRAINT [DF__Temporary__Model__286302EC] DEFAULT (0) FOR [Model48],
	CONSTRAINT [aaaaatblDealerDemos_PK] PRIMARY KEY  NONCLUSTERED 
	(
		[DealerDemoID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblDealerPartsList] WITH NOCHECK ADD 
	CONSTRAINT [aaaaatblDealerPartsList_PK] PRIMARY KEY  NONCLUSTERED 
	(
		[DealerPartID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblDealers] WITH NOCHECK ADD 
	CONSTRAINT [DF__Temporary__Curre__1B0907CE] DEFAULT (0) FOR [CurrentDealer],
	CONSTRAINT [DF__Temporary__Sweep__1BFD2C07] DEFAULT (0) FOR [SweeperDealer],
	CONSTRAINT [DF__Temporary__Labor__1CF15040] DEFAULT (0) FOR [LaborRate],
	CONSTRAINT [aaaaatblDealers_PK] PRIMARY KEY  NONCLUSTERED 
	(
		[DealerID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblDealersGoals] WITH NOCHECK ADD 
	CONSTRAINT [DF__Temporary__fkDea__37A5467C] DEFAULT (0) FOR [fkDealerID],
	CONSTRAINT [DF__Temporary__Model__398D8EEE] DEFAULT (0) FOR [Model],
	CONSTRAINT [DF__TemporaryU__Goal__3A81B327] DEFAULT (0) FOR [Goal],
	CONSTRAINT [aaaaatblDealersGoals_PK] PRIMARY KEY  NONCLUSTERED 
	(
		[Year],
		[Model],
		[fkDealerID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblLists] WITH NOCHECK ADD 
	CONSTRAINT [DF__Temporary__fkDea__45F365D3] DEFAULT (0) FOR [fkDealerID],
	CONSTRAINT [DF__Temporary__Flier__46E78A0C] DEFAULT (0) FOR [Flier],
	CONSTRAINT [aaaaatblLists_PK] PRIMARY KEY  NONCLUSTERED 
	(
		[ListID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblMajorAccounts] WITH NOCHECK ADD 
	CONSTRAINT [DF__Temporary__fkMAD__4BAC3F29] DEFAULT (0) FOR [fkMADealerID],
	CONSTRAINT [DF__Temporary__MANum__4CA06362] DEFAULT (0) FOR [MANumLocations],
	CONSTRAINT [DF__Temporary__MAApp__4D94879B] DEFAULT (0) FOR [MAApproved],
	CONSTRAINT [DF__Temporary__MADen__4E88ABD4] DEFAULT (0) FOR [MADenied],
	CONSTRAINT [DF__Temporary__MAAcc__4F7CD00D] DEFAULT (0) FOR [MAAccountNumKey],
	CONSTRAINT [aaaaatblMajorAccounts_PK] PRIMARY KEY  NONCLUSTERED 
	(
		[MajorAccountID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblMenuHistory] WITH NOCHECK ADD 
	CONSTRAINT [PK_tblMenuHistory] PRIMARY KEY  NONCLUSTERED 
	(
		[HistoryID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblModels] WITH NOCHECK ADD 
	CONSTRAINT [DF__Temporary__Model__5441852A] DEFAULT (0) FOR [Model],
	CONSTRAINT [DF__Temporary__Price__5535A963] DEFAULT (0) FOR [Price],
	CONSTRAINT [aaaaatblModels_PK] PRIMARY KEY  NONCLUSTERED 
	(
		[ModelID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblNewsletter] WITH NOCHECK ADD 
	CONSTRAINT [DF__Temporary__Deale__59FA5E80] DEFAULT (0) FOR [DealerFlag],
	CONSTRAINT [DF__Temporary__fkDea__5AEE82B9] DEFAULT (0) FOR [fkDealerID],
	CONSTRAINT [aaaaatblNewsletter_PK] PRIMARY KEY  NONCLUSTERED 
	(
		[NewsletterID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblParts] WITH NOCHECK ADD 
	CONSTRAINT [DF__Temporary__RPSPu__693CA210] DEFAULT (0) FOR [RPSPurchased],
	CONSTRAINT [DF__TemporaryUp__Qty__6A30C649] DEFAULT (0) FOR [Qty],
	CONSTRAINT [DF__Temporary__Total__6B24EA82] DEFAULT (0) FOR [TotalCostPerUnitForPart],
	CONSTRAINT [DF__Temporary__SubPa__6C190EBB] DEFAULT (0) FOR [SubPartTotal],
	CONSTRAINT [DF__Temporary__SubFl__6D0D32F4] DEFAULT (0) FOR [SubFlag],
	CONSTRAINT [DF__Temporary__Shelf__6E01572D] DEFAULT (0) FOR [ShelfNo],
	CONSTRAINT [DF__Temporary__Level__6EF57B66] DEFAULT (0) FOR [PartLevel],
	CONSTRAINT [DF__Temporary__HideO__6FE99F9F] DEFAULT (0) FOR [HideOnReports],
	CONSTRAINT [DF__Temporary__Optio__70DDC3D8] DEFAULT (0) FOR [PartOption],
	CONSTRAINT [DF__Temporary__RunQu__71D1E811] DEFAULT (0) FOR [RunQuantity],
	CONSTRAINT [aaaaatblParts_PK] PRIMARY KEY  NONCLUSTERED 
	(
		[PartID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblPartsModels] WITH NOCHECK ADD 
	CONSTRAINT [DF__Temporary__fkPar__76969D2E] DEFAULT (0) FOR [fkPartID],
	CONSTRAINT [DF__Temporary__Model__778AC167] DEFAULT (0) FOR [Model],
	CONSTRAINT [DF__Temporary__Quant__787EE5A0] DEFAULT (0) FOR [Quantity],
	CONSTRAINT [DF__tblPartsM__Optio__0EF836A4] DEFAULT (0) FOR [Optional],
	CONSTRAINT [tblPartsModels_PK] PRIMARY KEY  NONCLUSTERED 
	(
		[PartModelID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblPartsSubParts] WITH NOCHECK ADD 
	CONSTRAINT [DF__Temporary__SubCo__7D439ABD] DEFAULT (0) FOR [SubCost],
	CONSTRAINT [DF__Temporary__SubEx__7E37BEF6] DEFAULT (0) FOR [SubExtCost],
	CONSTRAINT [DF__Temporary__SubQt__7F2BE32F] DEFAULT (0) FOR [SubQty],
	CONSTRAINT [DF__Temporary__fkPar__00200768] DEFAULT (0) FOR [fkPartID],
	CONSTRAINT [aaaaatblPartsSubParts_PK] PRIMARY KEY  NONCLUSTERED 
	(
		[SubPartID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblPO] WITH NOCHECK ADD 
	CONSTRAINT [DF__Temporary__Confi__04E4BC85] DEFAULT (0) FOR [Confirmed],
	CONSTRAINT [aaaaatblVendorPOs_PK] PRIMARY KEY  NONCLUSTERED 
	(
		[POID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblPOPart] WITH NOCHECK ADD 
	CONSTRAINT [tblPODetail_PK] PRIMARY KEY  NONCLUSTERED 
	(
		[POPartID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblPOPartDetail] WITH NOCHECK ADD 
	CONSTRAINT [PK_tblPO] PRIMARY KEY  NONCLUSTERED 
	(
		[POPartDetailID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblProdSchedItems] WITH NOCHECK ADD 
	CONSTRAINT [DF__Temporary__Sched__0E6E26BF] DEFAULT (0) FOR [ScheduleID],
	CONSTRAINT [DF__Temporary__Model__0F624AF8] DEFAULT (0) FOR [Model],
	CONSTRAINT [DF__Temporary__Quant__10566F31] DEFAULT (0) FOR [Quantity],
	CONSTRAINT [aaaaatblProdSchedItems_PK] PRIMARY KEY  NONCLUSTERED 
	(
		[ScheduleID],
		[Model],
		[Quantity]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblProdSchedules] WITH NOCHECK ADD 
	CONSTRAINT [DF__Temporary__Month__151B244E] DEFAULT (0) FOR [Month],
	CONSTRAINT [DF__TemporaryU__Year__160F4887] DEFAULT (0) FOR [Year],
	CONSTRAINT [aaaaatblProdSchedules_PK] PRIMARY KEY  NONCLUSTERED 
	(
		[ScheduleID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblProspects] WITH NOCHECK ADD 
	CONSTRAINT [PK_tblProspects] PRIMARY KEY  NONCLUSTERED 
	(
		[ProspectID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblResponseMethod] WITH NOCHECK ADD 
	CONSTRAINT [aaaaatblResponseMethod_PK] PRIMARY KEY  NONCLUSTERED 
	(
		[ResposneID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblSysMessage] WITH NOCHECK ADD 
	CONSTRAINT [DF__Temporary__SysKi__1DB06A4F] DEFAULT (0) FOR [SysKick],
	CONSTRAINT [PK_tblSysMessage] PRIMARY KEY  NONCLUSTERED 
	(
		[SysMessageID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblTip] WITH NOCHECK ADD 
	CONSTRAINT [PK_tblTip] PRIMARY KEY  NONCLUSTERED 
	(
		[TipID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblTipDisable] WITH NOCHECK ADD 
	CONSTRAINT [PK_tblTipDisable] PRIMARY KEY  NONCLUSTERED 
	(
		[UserName],
		[TipID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblToDo] WITH NOCHECK ADD 
	CONSTRAINT [PK_tblNagList] PRIMARY KEY  NONCLUSTERED 
	(
		[ToDoID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblUser] WITH NOCHECK ADD 
	CONSTRAINT [PK_tblUser] PRIMARY KEY  NONCLUSTERED 
	(
		[UserID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblVendors] WITH NOCHECK ADD 
	CONSTRAINT [DF__Temporary__Blank__2739D489] DEFAULT (0) FOR [BlanketProductionOrders],
	CONSTRAINT [DF__Temporary__Activ__282DF8C2] DEFAULT ((-1)) FOR [Active],
	CONSTRAINT [aaaaatblVendors_PK] PRIMARY KEY  NONCLUSTERED 
	(
		[VendorID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblVersion] WITH NOCHECK ADD 
	CONSTRAINT [PK_tblVersion] PRIMARY KEY  NONCLUSTERED 
	(
		[VersionID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblWarranty] WITH NOCHECK ADD 
	CONSTRAINT [DF_tblWarranty_DateEntered] DEFAULT (getdate()) FOR [DateEntered],
	CONSTRAINT [PK_tblWarranty] PRIMARY KEY  NONCLUSTERED 
	(
		[WarrantyID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblWarrantyParts] WITH NOCHECK ADD 
	CONSTRAINT [DF__Temporary__fkWar__395884C4] DEFAULT (0) FOR [fkWarrantyID],
	CONSTRAINT [DF__Temporary__PartC__3A4CA8FD] DEFAULT (0) FOR [PartCost],
	CONSTRAINT [DF__Temporary__PartF__3B40CD36] DEFAULT (0) FOR [PartFileIndex],
	CONSTRAINT [aaaaatblWarrantyParts_PK] PRIMARY KEY  NONCLUSTERED 
	(
		[WarrantyPartID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tlkuContactTitles] WITH NOCHECK ADD 
	CONSTRAINT [aaaaatlkuContactTitles_PK] PRIMARY KEY  NONCLUSTERED 
	(
		[ContactTitle]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tlkuFinish] WITH NOCHECK ADD 
	CONSTRAINT [aaaaatlkuFinish_PK] PRIMARY KEY  NONCLUSTERED 
	(
		[FinishID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tmpLabelQtys] WITH NOCHECK ADD 
	CONSTRAINT [DF__Temporary__PartI__46B27FE2] DEFAULT (0) FOR [PartIndex],
	CONSTRAINT [DF__Temporary__Quant__47A6A41B] DEFAULT (0) FOR [Quantity],
	CONSTRAINT [PK_tmpLabelQtys] PRIMARY KEY  NONCLUSTERED 
	(
		[PartIndex]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tmpNames] WITH NOCHECK ADD 
	CONSTRAINT [DF__Temporary__Deale__4B7734FF] DEFAULT (0) FOR [DealerIndex]
GO

 CREATE  INDEX [Title] ON [dbo].[pltblImportedFile]([Title]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [Number] ON [dbo].[Tbl_Numbers]([Number]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [active] ON [dbo].[tblAllLeads]([ActiveInactive]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [Code] ON [dbo].[tblAllLeads]([Code]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [company name] ON [dbo].[tblAllLeads]([CompanyName]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [Date of Lead] ON [dbo].[tblAllLeads]([LeadDate]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [Dealer Name] ON [dbo].[tblAllLeads]([DealerName]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [phone] ON [dbo].[tblAllLeads]([Phone]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [IX_tblAllOrders_OrderNumber] ON [dbo].[tblAllOrders]([OrderNumber]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [IX_tblAllOrders_OrderDate] ON [dbo].[tblAllOrders]([OrderDate]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [tblallorders0] ON [dbo].[tblAllOrders]([OrderDate], [Model], [Dealer], [Quantity], [SalePrice], [CostPrice], [Margin], [OrderType]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [{DDDCE5F1-4923-11D3-BB05-004033532B7F}] ON [dbo].[tblDealerAdvert]([fkDealerID]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [fkDealerID] ON [dbo].[tblDealerAdvert]([fkDealerID]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [contact] ON [dbo].[tblDealerDemos]([ContactName]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [CustomerName] ON [dbo].[tblDealerDemos]([CustomerName]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [date of demo] ON [dbo].[tblDealerDemos]([DateOfDemo]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [dealernum] ON [dbo].[tblDealerDemos]([fkDealerID]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [RPSPartNum] ON [dbo].[tblDealerPartsList]([RPSPartNum]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [City] ON [dbo].[tblDealers]([City]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [Num] ON [dbo].[tblDealers]([Num]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [NumSalesman] ON [dbo].[tblDealers]([NumSalesman]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [phone] ON [dbo].[tblDealers]([Phone]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [Sort1] ON [dbo].[tblDealers]([DealerName]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [{DDDCE5F0-4923-11D3-BB05-004033532B7F}] ON [dbo].[tblDealersGoals]([fkDealerID]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [DealerIndex] ON [dbo].[tblDealersGoals]([fkDealerID]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [fkDealerID] ON [dbo].[tblLists]([fkDealerID]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [fkMADealerID] ON [dbo].[tblMajorAccounts]([fkMADealerID]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [MAAccountNum] ON [dbo].[tblMajorAccounts]([MAAccountNum]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [MAAccountNumKey] ON [dbo].[tblMajorAccounts]([MAAccountNumKey]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [VendorName] ON [dbo].[tblMajorAccounts]([MACompName]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [ModelID] ON [dbo].[tblModels]([ModelID]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [fkDealerID] ON [dbo].[tblNewsletter]([fkDealerID]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [dateorder] ON [dbo].[tblParts]([DateOrdered]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [DrawingNum] ON [dbo].[tblParts]([DrawingNum]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [ManfPartNum] ON [dbo].[tblParts]([ManfPartNum]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [model] ON [dbo].[tblParts]([Model]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [PartCode] ON [dbo].[tblParts]([PartCode]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [partname] ON [dbo].[tblParts]([PartName]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [RevisionNum] ON [dbo].[tblParts]([RevisionNum]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  UNIQUE  INDEX [rpspn] ON [dbo].[tblParts]([RPSPartNum]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [Vendorname] ON [dbo].[tblParts]([VendorName]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [IX_tblParts_RPSPNSort] ON [dbo].[tblParts]([RPSPNSort]) ON [PRIMARY]
GO

 CREATE  INDEX [fkPartID] ON [dbo].[tblPartsModels]([fkPartID]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [fkPartID] ON [dbo].[tblPartsSubParts]([fkPartID]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [SubNum] ON [dbo].[tblPartsSubParts]([SubNum]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [SubSourcePartNum] ON [dbo].[tblPartsSubParts]([SubSourcePartNum]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [Vendor] ON [dbo].[tblPO]([Vendor]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [PO] ON [dbo].[tblPOPart]([fkPOID]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [RPSPN] ON [dbo].[tblPOPart]([RPSPartNum]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [VenPN] ON [dbo].[tblPOPart]([VendorPartNumber]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [IX_tblPOPartDetail_fkPOPartID] ON [dbo].[tblPOPartDetail]([fkPOPartID]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [SchedItemID] ON [dbo].[tblProdSchedItems]([SchedItemID]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  UNIQUE  INDEX [apkResponse] ON [dbo].[tblResponseMethod]([ResposneID]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  UNIQUE  INDEX [ResponseMethod] ON [dbo].[tblResponseMethod]([ResponseMethod]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [RPSPartNum] ON [dbo].[tblVendors]([RPSPartNum]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [VendorName] ON [dbo].[tblVendors]([VendorName]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [VendorPartNum] ON [dbo].[tblVendors]([VendorPartNum]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [IX_tblWarranty_Dealer] ON [dbo].[tblWarranty]([Dealer]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [IX_tblWarranty_RGANum] ON [dbo].[tblWarranty]([RGANum]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  INDEX [fkWarrantyID] ON [dbo].[tblWarrantyParts]([fkWarrantyID]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

 CREATE  UNIQUE  INDEX [FinishDesc] ON [dbo].[tlkuFinish]([FinishDesc]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

ALTER TABLE [dbo].[tblDealerAdvert] ADD 
	CONSTRAINT [tblDealerAdvert_FK00] FOREIGN KEY 
	(
		[fkDealerID]
	) REFERENCES [dbo].[tblDealers] (
		[DealerID]
	)
GO

ALTER TABLE [dbo].[tblDealersGoals] ADD 
	CONSTRAINT [tblDealersGoals_FK00] FOREIGN KEY 
	(
		[fkDealerID]
	) REFERENCES [dbo].[tblDealers] (
		[DealerID]
	)
GO

ALTER TABLE [dbo].[tblPOPart] ADD 
	CONSTRAINT [tblPODetail_FK00] FOREIGN KEY 
	(
		[fkPOID]
	) REFERENCES [dbo].[tblPO] (
		[POID]
	)
GO

ALTER TABLE [dbo].[tblPOPartDetail] ADD 
	CONSTRAINT [FK_tblPOPartDetail_tblPOPart] FOREIGN KEY 
	(
		[fkPOPartID]
	) REFERENCES [dbo].[tblPOPart] (
		[POPartID]
	)
GO

ALTER TABLE [dbo].[tblWarrantyParts] ADD 
	CONSTRAINT [FK_tblWarrantyParts_tblWarranty] FOREIGN KEY 
	(
		[fkWarrantyID]
	) REFERENCES [dbo].[tblWarranty] (
		[WarrantyID]
	)
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE VIEW dlqrptAdvertAlloc
AS
SELECT tblDealers.DealerName, sum(tblDealerAdvert.AdvertAmt) AS SumOfAdvertAmt
FROM tblDealerAdvert RIGHT JOIN tblDealers ON (tblDealerAdvert.fkDealerID=tblDealers.DealerID)
WHERE (((tblDealers.CurrentDealer)=1))
GROUP BY tblDealers.DealerName


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE VIEW "dlqrptDealerListAddressView"
AS
SELECT tblDealers.DealerName, tblDealers.ContactName, tblDealers.StreetAddress, tblDealers.City, tblDealers.State, tblDealers.Zip, tblDealers.CurrentDealer
FROM tblDealers
WHERE (((tblDealers.CurrentDealer)=1))


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE VIEW "dlqrptDealerListAllDealersView"
AS
SELECT tblDealers.DealerName, tblDealers.ContactName, tblDealers.City, tblDealers.State, tblDealers.Phone, tblDealers.TollFreeNumber
FROM tblDealers


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE VIEW "dlqrptDealerListFaxView"
AS
SELECT tblDealers.DealerName, tblDealers.ContactName, tblDealers.City, tblDealers.State, tblDealers.Fax, tblDealers.TerritoryCovered, tblDealers.CurrentDealer
FROM tblDealers
WHERE (((tblDealers.CurrentDealer)=1))


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE VIEW "dlqrptDealerListPhoneView"
AS
SELECT tblDealers.DealerName, tblDealers.ContactName, tblDealers.City, tblDealers.State, tblDealers.Phone, tblDealers.TollFreeNumber, tblDealers.CurrentDealer
FROM tblDealers
WHERE (((tblDealers.CurrentDealer)=1))


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE VIEW "dlqrptMaillingLabelServicePartsManView"
AS
SELECT tblDealers.DealerName, tmpNames.PersonName, tblDealers.StreetAddress, tblDealers.City, tblDealers.State, tblDealers.Zip, tblDealers.CurrentDealer, tblDealers.Phone, tblDealers.ContactName
FROM tmpNames INNER JOIN tblDealers ON (tmpNames.DealerIndex=tblDealers.DealerID)
WHERE (((tblDealers.CurrentDealer)=1))


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE VIEW "dlqrptMaillingLabelsView"
AS
SELECT tblDealers.DealerName, tblDealers.ContactName AS Name, tblDealers.ContactName AS PersonName, tblDealers.StreetAddress, tblDealers.City, tblDealers.State, tblDealers.Zip, tblDealers.CurrentDealer, tblDealers.Phone, tblDealers.TollFreeNumber
FROM tblDealers
WHERE (((tblDealers.CurrentDealer)=1))


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE VIEW "dlqrptMaillingServiceView"
AS
SELECT tblDealers.DealerName, tblDealers.ServiceManagerName AS PersonName, tblDealers.StreetAddress, tblDealers.City, tblDealers.State, tblDealers.Zip, tblDealers.CurrentDealer, tblDealers.Phone, tblDealers.ContactName
FROM tblDealers
WHERE (((tblDealers.CurrentDealer)=1))


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE VIEW "dmqrptDealerDemoReportView"
AS
SELECT tblDealers.DealerName, tblDealerDemos.CustomerName, tblDealerDemos.DateOfDemo, tblDealerDemos.Purchased, tblDealerDemos.EquipmentDemoed
FROM tblDealerDemos INNER JOIN tblDealers ON (tblDealerDemos.fkDealerID=tblDealers.DealerID)


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE VIEW ldqfrmLeads
AS
SELECT tblLeads.*, tblLeads.Purchased AS Expr1001
FROM tblLeads
WHERE (((tblLeads.Purchased)=0))


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE VIEW "ldqrptMaillingLabelsView"
AS
SELECT tblLeads.Contact, tblLeads.CompanyName, tblLeads.Address, tblLeads.City, tblLeads.State, tblLeads.Zip, tblLeads.LeadDate
FROM tblLeads
WHERE (((tblLeads.Address) Is Not Null) AND ((tblLeads.State) Is Not Null))


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE VIEW "ldqrptResponseMethodView"
AS
SELECT tblLeads.DealerName, tblLeads.CompanyName, tblLeads.LeadDate, tblLeads.Contact, tblLeads.State, tblLeads.Phone, tblLeads.Result, tblLeads.City, tblLeads.ResponseMethod
FROM tblLeads


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE VIEW "lsqrptStatesDealerListView"
AS
SELECT tblDealers.DealerName, tblLists.LastDateMailed, tblLists.State, tblLists.Flier, tblLists.ListCompany
FROM tblDealers RIGHT JOIN tblLists ON (tblDealers.DealerID=tblLists.fkDealerID)
WHERE (((tblLists.LastDateMailed) Is Not Null))


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE VIEW "lsqrptStatesLastMailedView"
AS
SELECT tblLists.State, tblLists.LastDateMailed, tblLists.Flier, tblDealers.DealerName, tblLists.ListCompany
FROM tblDealers RIGHT JOIN tblLists ON (tblDealers.DealerID=tblLists.fkDealerID)


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE VIEW "lsqrptStatesNotMailedView"
AS
SELECT tblLists.Flier, tblLists.State, tblLists.LastDateMailed, tblDealers.DealerName, tblLists.ListCompany
FROM tblDealers RIGHT JOIN tblLists ON (tblDealers.DealerID=tblLists.fkDealerID)
WHERE (((tblLists.LastDateMailed) Is Null))


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO



CREATE VIEW dbo.maqrptMajorAccountInfoPage
AS
SELECT tblMajorAccounts.*, tblDealers.DealerName, 
    tblDealers.StreetAddress, tblDealers.City, tblDealers.State, 
    tblDealers.Zip
FROM tblDealers INNER JOIN
    tblMajorAccounts ON 
    tblDealers.DealerID = tblMajorAccounts.fkMADealerID



GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO


CREATE VIEW dbo.mlvCompany
AS
SELECT CompID, MailState, AreaCode, MailCounty, ProductDesc, 
    MailZip, PlantSqFt, Employment, MailAddress, Fax, 
    CompName, SICDesc, SICCode, Division, Phone800, HarrisID, 
    Phone, MailCity
FROM fcmail.dbo.tblMailCompany


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO


CREATE VIEW dbo.mlvContact
AS
SELECT ContactID, fkCompID, ContactName, fkContactTitleID, 
    DoNotMail
FROM fcmail.dbo.tblMailContact


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE VIEW dbo.mlvMaillist
AS
SELECT mlvCompany.CompID, mlvContact.ContactName, 
    mlvContact.fkContactTitleID, mlvCompany.CompName, 
    mlvCompany.MailAddress, mlvCompany.MailCity, 
    mlvCompany.MailState, mlvCompany.MailZip, 
    mlvCompany.MailCounty, mlvCompany.SICCode, 
    mlvCompany.SICDesc, mlvCompany.ProductDesc, 
    mlvCompany.PlantSqFt, mlvCompany.Employment, 
    mlvCompany.AreaCode, mlvCompany.Phone, 
    mlvCompany.Fax, mlvCompany.Phone800
FROM fcmail.dbo.tblMailCompany mlvCompany INNER JOIN
    fcmail.dbo.tblMailContact mlvContact ON 
    mlvCompany.CompID = mlvContact.fkCompID


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE VIEW "nwqrptAddressListingView"
AS
SELECT tblNewsletter.DealerFlag, tblNewsletter.DealerName, tblNewsletter.Name, tblNewsletter.Salutation, tblNewsletter.StreetAddress, tblNewsletter.City, tblNewsletter.State, tblNewsletter.Zip, tblNewsletter.Phone, tblNewsletter.Fax
FROM tblNewsletter


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE VIEW "nwqrptMailingLabelsView"
AS
SELECT tblNewsletter.DealerFlag, tblNewsletter.DealerName, tblNewsletter.Name AS PersonName, tblNewsletter.Salutation, tblNewsletter.StreetAddress, tblNewsletter.City, tblNewsletter.State, tblNewsletter.Zip, tblNewsletter.Phone, tblNewsletter.Fax
FROM tblNewsletter


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE VIEW paqrptListPricesView
AS
SELECT tblParts.RPSPartNum, tblParts.PartName, tblParts.SuggestedList, tblParts.Note, tblParts.HideOnReports, tblParts.RPSPNSort
FROM tblParts
WHERE (((tblParts.RPSPartNum) Not Like 'V%' And (tblParts.RPSPartNum) Not Like 'E%' And (tblParts.RPSPartNum) Not Like 'H%') And ((tblParts.SuggestedList)>0.01) And ((tblParts.HideOnReports)=0))


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE VIEW paqrptPartLabels
AS
SELECT tblParts.RPSPartNum, tblParts.PartName, tblParts.VendorName, tblParts.VendorPartName, tblParts.ManfPartNum, Tbl_Numbers.Counter, tblParts.Model
FROM Tbl_Numbers, tblParts

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO



CREATE VIEW paqrptPartLabelsMulti
AS
SELECT dbo.tblParts.RPSPartNum, dbo.tblParts.PartName, 
    dbo.tblParts.VendorName, dbo.tblParts.VendorPartName, 
    dbo.tblParts.ManfPartNum, dbo.Tbl_Numbers.Counter, dbo.tblParts.RPSPNSort
FROM dbo.Tbl_Numbers INNER JOIN
    dbo.tblParts INNER JOIN
    dbo.tmpLabelQtys ON 
    dbo.tblParts.PartID = dbo.tmpLabelQtys.PartIndex ON 
    dbo.Tbl_Numbers.Counter <= dbo.tmpLabelQtys.Quantity


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO



CREATE VIEW paqrptPartListFinishView
AS
SELECT tblParts.RPSPartNum, tblParts.PartName, tblParts.FinishDesc, tblParts.RPSPNSort
FROM tblParts
WHERE (((tblParts.FinishDesc) Is Not Null))


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE VIEW paqrptPartsPerModel2
AS
SELECT tblParts.PartID, tblParts.RPSPartNum, tblParts.PartName, tblModels.Model
FROM tblParts, tblModels

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO




CREATE VIEW "parptRPSPartsPricingView"
AS
SELECT tblParts.RPSPartNum, tblParts.VendorName, tblParts.ManfPartNum, tblParts.SuggestedList, tblParts.DealerNet, tblParts.PartName, tblParts.HideOnReports
FROM tblParts
WHERE (((tblParts.RPSPartNum) Not Like 'V%' And (tblParts.RPSPartNum) Not Like 'E%' And (tblParts.RPSPartNum) Not Like 'H%') And ((tblParts.HideOnReports)=0))



GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE VIEW parqryDealerPartsListView
AS
SELECT tblParts.RPSPartNum, tblParts.PartName, tblParts.DealerNet, tblParts.SuggestedList, tblParts.Note, tblParts.RPSPNSort
FROM tblParts
WHERE (((tblParts.RPSPartNum) Not Like 'V%' And (tblParts.RPSPartNum) Not Like 'E%' And (tblParts.RPSPartNum) Not Like 'H%') And ((tblParts.DealerNet)<>0 And (tblParts.DealerNet) Is Not Null) And ((tblParts.HideOnReports)=0))




GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO


CREATE VIEW dbo.ppqfrmProdParts
AS
SELECT tblPO.POID, tblPO.Vendor, tblPO.Confirmed, 
    tblPOPartDetail.ReceivedDate
FROM tblPO INNER JOIN
    tblPOPart ON tblPO.POID = tblPOPart.fkPOID INNER JOIN
    tblPOPartDetail ON 
    tblPOPart.POPartID = tblPOPartDetail.fkPOPartID
WHERE (tblPOPartDetail.ReceivedDate IS NULL)


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO


CREATE VIEW dbo.ppqrptOpenPOsDetailed
AS
SELECT POA.POID, POPart.POPartID, POA.Vendor, 
    POPart.RPSPartNum, POPart.VendorPartNumber, 
    POPart.PartDescription, PODetail.CostEach, PODetail.Quantity, 
    PODetail.RequestedShipDate, PODetail.ReceivedDate, 
    PODetail.QuantityReceived, PODetail.Value, 
    PODetail.Notes
FROM tblPO POA INNER JOIN
    tblPOPart POPart ON POA.POID = POPart.fkPOID AND 
    POA.POID = POPart.fkPOID INNER JOIN
    tblPOPartDetail PODetail ON 
    POPart.POPartID = PODetail.fkPOPartID
WHERE (PODetail.RequestedShipDate IS NOT NULL) AND 
    (PODetail.ReceivedDate IS NULL) AND (POA.POID IN
        (SELECT A.POID
      FROM dbo.tblPO A INNER JOIN
           dbo.tblPOPart B ON A.POID = B.fkPOID AND 
           A.POID = B.fkPOID INNER JOIN
           dbo.tblPOPartDetail C ON 
           B.POPartID = C.fkPOPartID
      WHERE (C.RequestedShipDate IS NOT NULL) AND 
           (C.ReceivedDate IS NULL)))


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE VIEW dbo.qselContactTitle
AS
SELECT ContactTitle
FROM tlkuContactTitles



GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE VIEW dbo.tblLeads
AS
SELECT tblAllLeads.*
FROM tblAllLeads
WHERE (Purchased = 0)
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE VIEW dbo.tblOrders
AS
SELECT tblAllOrders.*
FROM tblAllOrders
WHERE OrderType = 0
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE VIEW dbo.tblPurLeads
AS
SELECT tblAllLeads.*
FROM tblAllLeads
WHERE (Purchased = 1)
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE VIEW dbo.tblTCLeads
AS
SELECT tblAllLeads.*
FROM tblAllLeads
WHERE (Purchased = 2)
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE VIEW dbo.tblTCOrders
AS
SELECT tblAllOrders.*
FROM tblAllOrders
WHERE OrderType = 1
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE VIEW dbo.tempView
AS
SELECT tblParts.RPSPartNum, tblParts.PartName, 
    tblParts.DealerNet AS [Dealer Net Price], 
    tblParts.SuggestedList AS [Suggested List Price], 
    tblParts.QtyReq AS [Quantity Required], tblParts.Notes, 
    tblParts.PartID
FROM tblParts
WHERE (((tblParts.DealerNet) > 0))

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE VIEW "veqrptVendorLabelsView"
AS
SELECT tblVendors.CointactName, tblVendors.VendorName, tblVendors.StreetAddress, (CITY+', '+STATE+'  '+ZIP) AS "City Address"
FROM tblVendors
WHERE (((tblVendors.Active)=1))


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE VIEW dbo.vSwitchboard
AS
SELECT [Switchboard Items].*
FROM [Switchboard Items]
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO


CREATE VIEW dbo.waqfrmWarranty
AS
SELECT WarrantyID, MachineSerialNumber, CreditMemoNum, 
    CreditMemoAmt, DateOfFailure, Dealer, Customer, RGANum, 
    PartCost, LaborCost, Freight, Model, Problem, Resolution, 
    WarrantyOpen, Travel, Policy, DateEntered, PartReceived, 
    Hours, fkDealerID, Comment
FROM tblWarranty


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO

CREATE VIEW dbo.waqrptRgaClaim
AS
SELECT tblDealers.DealerName, tblDealers.StreetAddress, 
    tblDealers.City, tblDealers.State, tblDealers.Zip, 
    tblWarranty.Customer, tblWarranty.DateOfFailure, 
    tblWarranty.RGANum, tblWarranty.WarrantyID, 
    tblWarranty.Comment, tblWarranty.Problem, 
    tblWarranty.Resolution, tblWarranty.MachineSerialNumber, 
    tblWarranty.DateEntered
FROM tblDealers INNER JOIN
    tblWarranty ON tblDealers.DealerID = tblWarranty.fkDealerID

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE VIEW dbo.waqrptWarrantyPending
AS
SELECT dbo.tblWarranty.Dealer, 
    dbo.tblWarranty.MachineSerialNumber, 
    dbo.tblWarranty.RGANum, 
    dbo.tblWarrantyParts.PartNumReplaced, 
    dbo.tblWarranty.WarrantyOpen, dbo.tblWarranty.Hours
FROM dbo.tblWarranty INNER JOIN
    dbo.tblWarrantyParts ON 
    dbo.tblWarranty.WarrantyID = dbo.tblWarrantyParts.fkWarrantyID
WHERE (dbo.tblWarranty.WarrantyOpen = 1)


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
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

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE Procedure dlqrptDealerContractAlpha
/*
	(
		@parameter1 datatype = default value,
		@parameter2 datatype OUTPUT
	)
*/
As
SELECT DealerName, ContactName, StreetAddress, City, State, Zip, CurrentDealer, ContractExpires
FROM tblDealers
WHERE (CurrentDealer = 1)
ORDER BY DealerName, ContactName	

	return 






GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[dlqrptDealerContractAlpha]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE Procedure dlqrptDealerContractDate
/*
	(
		@parameter1 datatype = default value,
		@parameter2 datatype OUTPUT
	)
*/
As
SELECT DealerName, ContactName, StreetAddress, City, State, Zip, CurrentDealer, ContractExpires
FROM tblDealers
WHERE (CurrentDealer = 1)
ORDER BY ContractExpires, DealerName, ContactName	

	return 






GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[dlqrptDealerContractDate]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE dlqrptDealerListAddress
AS
SELECT * FROM "dlqrptDealerListAddressView"
ORDER BY "dlqrptDealerListAddressView".DealerName, "dlqrptDealerListAddressView".ContactName


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[dlqrptDealerListAddress]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE dlqrptDealerListAllDealers
AS
SELECT * FROM "dlqrptDealerListAllDealersView"
ORDER BY "dlqrptDealerListAllDealersView".DealerName, "dlqrptDealerListAllDealersView".ContactName


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[dlqrptDealerListAllDealers]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE dlqrptDealerListFax
AS
SELECT * FROM "dlqrptDealerListFaxView"
ORDER BY "dlqrptDealerListFaxView".DealerName, "dlqrptDealerListFaxView".ContactName


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[dlqrptDealerListFax]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE dlqrptDealerListPhone
AS
SELECT * FROM "dlqrptDealerListPhoneView"
ORDER BY "dlqrptDealerListPhoneView".DealerName, "dlqrptDealerListPhoneView".ContactName


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[dlqrptDealerListPhone]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE dlqrptMaillingLabels
AS
SELECT * FROM "dlqrptMaillingLabelsView"
ORDER BY "dlqrptMaillingLabelsView".DealerName


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[dlqrptMaillingLabels]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE dlqrptMaillingLabelServicePartsMan
AS
SELECT * FROM "dlqrptMaillingLabelServicePartsManView"
ORDER BY "dlqrptMaillingLabelServicePartsManView".DealerName


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[dlqrptMaillingLabelServicePartsMan]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE dlqrptMaillingService
AS
SELECT * FROM "dlqrptMaillingServiceView"
ORDER BY "dlqrptMaillingServiceView".DealerName


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

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
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
SELECT ao.Dealer, ao.Model, ao.OrderDate, ao.ShippedDate, ao.Quantity, ao.SalePrice, ao.OrderNumber, ao.SerialNumber
FROM tblAllOrders ao INNER JOIN tblDealers ON ao.Dealer = tblDealers.DealerName
WHERE (tblDealers.CurrentDealer = 1) AND SalePrice <> 0 AND ao.OrderType = @iOrderType
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

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
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
FROM dbo.tblMajorAccounts ma INNER JOIN dbo.tblOrders ao ON 
    ma.MajorAccountID = ao.MajorAccountID
WHERE ao.OrderType = @iOrderType
	AND ao.OrderDate Between @sFromDate And @sToDate
	AND ma.MACompName like @sMAName
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptMajorAccountSales]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
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
FROM dbo.tblOrders 
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

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE orsprptServicingDealers
	@iOrderType varchar(3) = 0
AS
-- Check input parameters
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
--- Run query
SELECT ao.Model, ao.SerialNumber, ao.[Name], ao.Phone, ao.City, ao.State, ao.SaleDate, ao.Dealer, dl.DealerName 
FROM dbo.tblAllOrders ao INNER JOIN dbo.tblDealers dl ON ao.fkServDealerID = dl.DealerID
WHERE OrderType = @iOrderType
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptServicingDealers]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE orsprptUnregSweepers
	@iOrderType varchar(3) = 0
AS
-- Check input parameters
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
--- Run query
SELECT DISTINCT ao.Dealer, ao.SerialNumber, ao.PurchaseOrder, ao.OrderDate, ao.ShippedDate, ao.[Name], ao.Model 
FROM dbo.tblDealers dl RIGHT OUTER JOIN dbo.tblAllOrders ao ON dl.DealerName = ao.Dealer 
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




CREATE PROCEDURE paqryExportDealerParts
AS
SELECT tblParts.RPSPartNum, tblParts.PartName, tblParts.DealerNet AS "Dealer Net Price", tblParts.SuggestedList AS "Suggested List Price", tblParts.QtyReq AS "Quantity Required", tblParts.Notes, tblParts.PartID
FROM tblParts
WHERE (((tblParts.DealerNet)>0))
ORDER BY tblParts.PartName




GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[paqryExportDealerParts]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE pasprptBillOfMaterials
	@sVendorName varchar(50),
	@sModel varchar(10)
 AS
DECLARE @sSQL varchar(1500)
DECLARE @sWhere varchar(500)
SELECT @sWhere = ''
SELECT @sSQL = 
'SELECT p.RPSPartNum, p.PartName, p.VendorName, p.CostEach, p.Quantity, 
	CONVERT (smallmoney, CONVERT (smallmoney, p.CostEach) * CONVERT (int, m.Quantity) ) AS ExtCost, p.RPSPNSort
FROM dbo.tblParts p, dbo.tblPartsModels m
WHERE p.PartID = m.fkPartID AND m.Quantity > 0 '
-- Check model
IF @sModel IS NOT NULL
	SELECT @sWhere = ' m.Model = ' + @sModel + ' '
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
IF @sModel IS NULL
	SELECT @sModel = '%'
IF @iQty IS NULL
	SELECT @iQty = 1
SELECT pa.RPSPartNum, pa.PartName, pa.VendorName, pa.VendorPartName, pa.ManfPartNum, nu.Counter, pa.Model, pa.RPSPNSort
FROM dbo.tblParts pa, dbo.tbl_Numbers nu
WHERE nu.Counter <= @iQty		-- allows multiple labels
	and pa.RPSPartNum Between @sStartPart And @sEndPart
	and pa.Model Like @sModel


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
AS

select tP.RPSPartNum, tP.VendorName, tP.ManfPartNum, tP.SuggestedList, tP.DealerNet, tP.PartName, tP.HideOnReports, tP.RPSPNSort
from dbo.tblParts tP
where tP.RPSPartNum not like 'V%' And tP.RPSPartNum not like 'E%' and tP.RPSPartNum not like 'H%' and tP.HideOnReports = 0
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

DECLARE @sSQL varchar(1200)
DECLARE @sWhere varchar(300)
SELECT @sSQL = 'SELECT dbo.tblPOPartDetail.RequestedShipDate, dbo.tblPOPart.VendorPartNumber, dbo.tblPO.Vendor, 
		dbo.tblPOPart.RPSPartNum, dbo.tblPOPartDetail.CostEach, dbo.tblPOPartDetail.Quantity, dbo.tblPOPartDetail.Value, 
		dbo.tblPOPartDetail.ReceivedDate, dbo.tblPO.POID 
		FROM dbo.tblPO INNER JOIN dbo.tblPOPart ON dbo.tblPO.POID = dbo.tblPOPart.fkPOID INNER JOIN dbo.tblPOPartDetail 
			ON dbo.tblPOPart.POPartID = dbo.tblPOPartDetail.fkPOPartID '
SELECT @sWhere = ''

-- check dates
IF @sFromDate IS NOT NULL OR @sToDate IS NOT NULL OR @sVendorName IS NOT NULL OR @sOpenClosed is not null
  BEGIN
	IF @sFromDate IS NOT NULL AND @sToDate  IS NOT NULL 
	  BEGIN
		SELECT @sWhere = 'dbo.tblPOPartDetail.RequestedShipDate Between ''' + @sFromDAte + ''' And ''' + @sToDAte + ''' '
	  END
  	ELSE IF @sFromDate IS NOT NULL
	  BEGIN
		SELECT @sWhere = 'dbo.tblPOPartDetail.RequestedShipDate > ' + '''' + @sFromDate + ''''	
	  END
	ELSE IF @sToDate IS NOT NULL
	  BEGIN
		SELECT @sWhere = ' dbo.tblPOPartDetail.RequestedShipDate > ' + '''' + @sToDate + ''''	
	  END
	IF @sVendorName IS NOT NULL
	  BEGIN
		IF @sWhere <> ''
			SELECT @sWhere= @sWhere + ' AND '
		SELECT @sWhere = @sWhere + ' AND Vendor = ''' + @sVendorName + ''''
	  END
	IF @sOpenClosed IS NOt null
	  BEGIN
		IF @sWhere <> ''
			SELECT @sWhere= @sWhere + ' AND '
		IF @sOpenClosed = 'open'
			SELECT @sWhere = @sWhere + ' ReceivedDate is null'
		ELSE IF @sOpenClosed = 'closed'
			SELECT @sWhere = @sWhere + 'ReceivedDate is not null'
	  END
	SELECT @sSQL = @sSQL + 'WHERE ' + @sWhere 
  END


EXEC (@sSQL)










GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[ppsprptCashFlow]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE ppsprptProdReleases
	@sToDate varchar(20)
AS
DECLARE @sSQL varchar(1000)
SELECT @sSQL = '
SELECT dbo.tblPO.POID, dbo.tblPO.Vendor, dbo.tblPOPart.VendorPartNumber, dbo.tblPOPart.RPSPartNum, dbo.tblPOPart.PartDescription, 
	dbo.tblPOPartDetail.RequestedShipDate, dbo.tblPOPartDetail.Quantity, dbo.tblPOPartDetail.Value, dbo.tblPOPartDetail.ReceivedDate, 
	dbo.tblPOPartDetail.CostEach, dbo.tblPOPartDetail.CostEach * dbo.tblPOPartDetail.Quantity AS ExtCost,
	DATEPART(yyyy, dbo.tblPOPartDetail.RequestedShipDate) AS ShipYear, DATEPART(wk, dbo.tblPOPartDetail.RequestedShipDate) AS ShipWeek
FROM dbo.tblPO INNER JOIN dbo.tblPOPart ON dbo.tblPO.POID = dbo.tblPOPart.fkPOID 
	INNER JOIN dbo.tblPOPartDetail ON dbo.tblPOPart.POPartID = dbo.tblPOPartDetail.fkPOPartID 
WHERE (dbo.tblPOPartDetail.ReceivedDate IS NULL)'
IF @sToDate IS NOT NULL 
  BEGIN
	SELECT @sSQL = @sSQL + ' AND (dbo.tblPOPartDetail.RequestedShipDate < ''' + @sToDate + ''')'
  END
EXEC (@sSQL)
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
	@sPOID varchar(20),
	@sRPSPartNum varchar(40),
	@sVendorPartNum varchar(40),
	@iOpenFlag varchar(2)
AS

DECLARE @sSQL varchar(1300)
DECLARE @sWhere varchar(300)

SELECT @sSQL = '
SELECT dbo.tblPO.POID, dbo.tblPO.Vendor, dbo.tblPOPart.VendorPartNumber, dbo.tblPOPart.RPSPartNum, 
	dbo.tblPOPart.PartDescription, dbo.tblPOPartDetail.RequestedShipDate, dbo.tblPOPartDetail.Quantity, 
	dbo.tblPOPartDetail.Value, dbo.tblPOPartDetail.ReceivedDate 
FROM dbo.tblPO INNER JOIN dbo.tblPOPart ON dbo.tblPO.POID = dbo.tblPOPart.fkPOID AND 
	dbo.tblPO.POID = dbo.tblPOPart.fkPOID INNER JOIN dbo.tblPOPartDetail ON dbo.tblPOPart.POPartID = dbo.tblPOPartDetail.fkPOPartID 
WHERE (dbo.tblPOPartDetail.RequestedShipDate IS NOT NULL)'

IF @sPOID IS NOT NULL
  BEGIN
	SELECT @sSQL = @sSQL + ' AND dbo.tblPO.POID = ' + @sPOID
  END

IF @sRPSPartNum IS NOT NULL
  BEGIN
	SELECT @sSQL = @sSQL + ' AND dbo.tblPOPart.RPSPartNum = ''' + @sRPSPartNum + ''''
  END

IF @sVendorPartNum IS NOT NULL
  BEGIN
	SELECT @sSQL = @sSQL + ' AND dbo.tblPOPart.VendorPartNumber = ''' + @sVendorPartNum + ''''
  END

IF @iOpenFlag = 1
  BEGIN
	SELECT @sSQL = @sSQL + ' AND dbo.tblPOPartDetail.ReceivedDate IS NULL'
  END

EXEC (@sSQL)


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
/*
	(
		@parameter1 datatype = default value,
		@parameter2 datatype OUTPUT
	)
*/
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
/*
	(
		@parameter1 datatype = default value,
		@parameter2 datatype OUTPUT
	)
*/
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
/*
	(
		@parameter1 datatype = default value,
		@parameter2 datatype OUTPUT
	)
*/
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
/*
	(
		@parameter1 datatype = default value,
		@parameter2 datatype OUTPUT
	)
*/
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
	EXEC sp_displayerrorinfo @object, @hr
	RETURN
  END

-- GET A PROPERTY BY CALLING METHOD
EXEC @hr = sp_OAMethod @object, 'GetUserStr', @property OUT
IF @hr <> 0
  BEGIN
	EXEC sp_displayerrorinfo @object, @hr
	RETURN
  END

SELECT @property AS uList


-- DESTROY OBJECT
EXEC @hr = sp_OADestroy @object
IF @hr <> 0
  BEGIN
	EXEC sp_displayerrorinfo @object, @hr
	RETURN
  END


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[spGetNTUserList]  TO [fcuser]
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

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE ut_qry33
AS
SELECT *
FROM tblProdSchedItems
ORDER BY Model



GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[ut_qry33]  TO [fcuser]
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

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE waqrptWarrantyRGANums
AS
SELECT * FROM "waqrptWarrantyRGANumsView"
ORDER BY "waqrptWarrantyRGANumsView".RGANum


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[waqrptWarrantyRGANums]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE wasprptDealerReimburse 
	@sDealerName varchar(40),
	@sFromDate varchar(20),
	@sToDate varchar(20),
	@sOpen varchar(2)
AS
DECLARE @sSQL varchar(1200)
DECLARE @sWhere varchar(200)
SELECT @sWhere = ''
SELECT @sSQL = '
SELECT dbo.tblWarranty.Dealer, dbo.tblWarranty.Model, dbo.tblWarranty.MachineSerialNumber, dbo.tblWarranty.CreditMemoNum, 
	dbo.tblWarranty.CreditMemoAmt, dbo.tblWarranty.RGANum, dbo.tblWarranty.DateOfFailure, dbo.tblWarranty.WarrantyOpen 
FROM dbo.tblWarrantyParts INNER JOIN dbo.tblWarranty ON dbo.tblWarrantyParts.fkWarrantyID = dbo.tblWarranty.WarrantyID'
-- check dates
IF @sFromDate IS NOT NULL OR @sToDate IS NOT NULL OR @sDealerName IS NOT NULL OR @sOpen IS NOT NULL
  BEGIN
	-- check dates
	IF @sFromDate IS NOT NULL AND @sToDate  IS NOT NULL 
	  BEGIN
		SELECT @sWhere =' DateOfFailure Between ''' + @sFromDAte + ''' And ''' + @sToDAte + ''' '
	  END
  	ELSE IF @sFromDate IS NOT NULL
	  BEGIN
		SELECT @sWhere = 'DateOfFailure > ' + '''' + @sFromDate + ''''	
	  END
	ELSE IF @sToDate IS NOT NULL
	  BEGIN
		SELECT @sWhere = 'DateOfFailure > ' + '''' + @sToDate + ''''	
	  END
	-- check dealer
	IF @sDealerName IS NOT NULL
	  BEGIN
		IF @sWhere <> ''
		  BEGIN
			SELECT @sWhere = @sWhere + ' AND '
		  END
		SELECT @sWhere = @sWhere + ' Dealer = ''' + @sDealerName + ''''
	  END
	-- check open flag
	IF @sOpen IS NOT NULL
	  BEGIN
		IF @sWhere <> ''
		  BEGIN
			SELECT @sWhere = @sWhere + ' AND '
		  END
		SELECT @sWhere = @sWhere + ' WarrantyOpen = ' + @sOpen
	  END
	SELECT @sSQL = @sSQL + ' WHERE ' + @sWhere 
  END
EXEC (@sSQL)
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[wasprptDealerReimburse]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE wasprptRgaClaimDates
	@sFromDate varchar(20),
	@sToDate varchar(20)
AS

DECLARE @sSQL varchar(1000)

SELECT @sSQL = 
'SELECT dbo.tblDealers.DealerName, dbo.tblDealers.StreetAddress, 
    dbo.tblDealers.City, 
	dbo.tblDealers.City + ' + quotename(', ','''') + ' + dbo.tblDealers.State + ' + quotename('  ', '''') + ' + dbo.tblDealers.Zip AS CityStateZip,
    dbo.tblWarranty.Customer, 
    dbo.tblWarranty.DateOfFailure, dbo.tblWarranty.RGANum, 
    dbo.tblWarranty.DateEntered, dbo.tblWarranty.Comment, 
    dbo.tblWarranty.WarrantyID, 
    dbo.tblWarranty.MachineSerialNumber, 
    dbo.tblWarranty.Problem, dbo.tblWarranty.Resolution, 
    dbo.tblDealers.State, dbo.tblDealers.Zip
FROM dbo.tblDealers INNER JOIN
    dbo.tblWarranty ON 
    dbo.tblDealers.DealerID = dbo.tblWarranty.fkDealerID'

IF @sFromDate IS NOT NULL AND @sToDate  IS NOT NULL 
  BEGIN
	SELECT @sSQL = @sSQL + ' WHERE DateEntered Between ''' + @sFromDAte + ''' And ''' + @sToDAte + ''' '
  END
ELSE IF @sFromDate IS NOT NULL
  BEGIN
	SELECT @sSQL = @sSQL + ' WHERE DateEntered > ' + '''' + @sFromDate + ''''	
  END
ELSE IF @sToDate IS NOT NULL
  BEGIN
	SELECT @sSQL = @sSQL + ' WHERE DateEntered > ' + '''' + @sToDate + ''''	
  END

EXEC (@sSQL)




GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[wasprptRgaClaimDates]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE wasprptWarrantyCosts 
	@sPartName varchar(40),
	@sFromDate varchar(20),
	@sToDate varchar(20),
	@sOpen varchar(2)
AS
DECLARE @sSQL varchar(1200)
DECLARE @sWhere varchar(200)
SELECT @sWhere = ''
SELECT @sSQL = 'SELECT tblWarranty.MachineSerialNumber, tblWarranty.DateOfFailure, tblWarranty.CreditMemoNum,
		tblWarranty.Dealer, tblWarranty.Customer, tblWarranty.RGANum, tblWarranty.LaborCost, tblWarranty.Freight, 
		tblWarranty.Problem, tblWarrantyParts.PartNumReplaced, tblWarrantyParts.PartDescription, tblWarranty.WarrantyID, 
		tblParts.PartName, tblWarrantyParts.PartCost, tblWarranty.Model, tblWarranty.Hours, tblWarranty.WarrantyOpen
	FROM tblParts RIGHT JOIN (tblWarrantyParts INNER JOIN tblWarranty ON 
		tblWarrantyParts.fkWarrantyID = tblWarranty.WarrantyID) ON tblParts.PartID = tblWarrantyParts.PartFileIndex '
-- check dates
IF @sFromDate IS NOT NULL OR @sToDate IS NOT NULL OR @sPartName IS NOT NULL OR @sOpen IS NOT NULL
  BEGIN
	-- check dates
	IF @sFromDate IS NOT NULL AND @sToDate  IS NOT NULL 
	  BEGIN
		SELECT @sWhere =' DateOfFailure Between ''' + @sFromDAte + ''' And ''' + @sToDAte + ''' '
	  END
  	ELSE IF @sFromDate IS NOT NULL
	  BEGIN
		SELECT @sWhere = 'DateOfFailure > ' + '''' + @sFromDate + ''''	
	  END
	ELSE IF @sToDate IS NOT NULL
	  BEGIN
		SELECT @sWhere = 'DateOfFailure > ' + '''' + @sToDate + ''''	
	  END
	-- check part
	IF @sPartName IS NOT NULL
	  BEGIN
		IF @sWhere <> ''
		  BEGIN
			SELECT @sWhere = @sWhere + ' AND '
		  END
		SELECT @sWhere = @sWhere + ' PartNumReplaced = ''' + @sPartName + ''''
	  END
	-- check open flag
	IF @sOpen IS NOT NULL
	  BEGIN
		IF @sWhere <> ''
		  BEGIN
			SELECT @sWhere = @sWhere + ' AND '
		  END
		SELECT @sWhere = @sWhere + ' WarrantyOpen = ' + @sOpen
	  END
	SELECT @sSQL = @sSQL + ' WHERE ' + @sWhere 
  END
EXEC (@sSQL)
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[wasprptWarrantyCosts]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE wasprptWarrantyTotalCost
	@sDealerName varchar(40),
	@sFromDate varchar(20),
	@sToDate varchar(20),
	@sOpen varchar(2)
AS
DECLARE @sSQL varchar(1400)
DECLARE @sWhere varchar(300)
SELECT @sWhere = ''
SELECT @sSQL = '
SELECT dbo.tblWarranty.Dealer, dbo.tblWarranty.Model, dbo.tblWarranty.MachineSerialNumber, dbo.tblWarranty.DateOfFailure, dbo.tblWarranty.LaborCost, 
	SUM(dbo.tblWarrantyParts.PartCost) AS ExtPartCost, dbo.tblWarranty.Travel, dbo.tblWarranty.Hours
FROM dbo.tblWarranty INNER JOIN dbo.tblWarrantyParts ON dbo.tblWarranty.WarrantyID = dbo.tblWarrantyParts.fkWarrantyID '
-- check dates
IF @sFromDate IS NOT NULL OR @sToDate IS NOT NULL OR @sDealerName IS NOT NULL OR @sOpen IS NOT NULL
  BEGIN
	-- check dates
	IF @sFromDate IS NOT NULL AND @sToDate  IS NOT NULL 
	  BEGIN
		SELECT @sWhere =' DateOfFailure Between ''' + @sFromDAte + ''' And ''' + @sToDAte + ''' '
	  END
  	ELSE IF @sFromDate IS NOT NULL
	  BEGIN
		SELECT @sWhere = 'DateOfFailure > ' + '''' + @sFromDate + ''''	
	  END
	ELSE IF @sToDate IS NOT NULL
	  BEGIN
		SELECT @sWhere = 'DateOfFailure > ' + '''' + @sToDate + ''''	
	  END
	-- check dealer
	IF @sDealerName IS NOT NULL
	  BEGIN
		IF @sWhere <> ''
		  BEGIN
			SELECT @sWhere = @sWhere + ' AND '
		  END
		SELECT @sWhere = @sWhere + ' Dealer = ''' + @sDealerName + ''''
	  END
	-- check open flag
	IF @sOpen IS NOT NULL
	  BEGIN
		IF @sWhere <> ''
		  BEGIN
			SELECT @sWhere = @sWhere + ' AND '
		  END
		SELECT @sWhere = @sWhere + ' WarrantyOpen = ' + @sOpen
	  END
	SELECT @sSQL = @sSQL + ' WHERE ' + @sWhere 
  END
SELECT @sSQL = @sSQL + '
GROUP BY dbo.tblWarranty.Dealer, dbo.tblWarranty.Model, dbo.tblWarranty.MachineSerialNumber, dbo.tblWarranty.DateOfFailure, 
	dbo.tblWarranty.LaborCost, dbo.tblWarranty.Travel, dbo.tblWarranty.Hours'
EXEC (@sSQL)



GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[wasprptWarrantyTotalCost]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE wasprsubRGAParts
	@sID varchar(10)

AS

DECLARE @sSQL varchar(800)

SELECT @sSQL = '
SELECT tblWarrantyParts.PartNumReplaced, tblParts.PartName, tblWarrantyParts.fkWarrantyID
FROM tblParts INNER JOIN tblWarrantyParts ON (tblParts.RPSPartNum=tblWarrantyParts.PartNumReplaced)'
SELECT @sSQL = @sSQL + ' WHERE tblWarrantyParts.fkWarrantyID = ' + @sID

EXEC (@sSQL)




GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[wasprsubRGAParts]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO

CREATE TRIGGER "tblLeads_UTrig" ON dbo.tblAllLeads FOR UPDATE AS
SET NOCOUNT ON
IF (SELECT Count(*) FROM inserted WHERE NOT (ActiveInactive='A' Or ActiveInactive='I')) > 0
    BEGIN
        RAISERROR 44444 'You must enter either an A for Active or an I for Inactive in this field.'
        ROLLBACK TRANSACTION
    END

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO

CREATE TRIGGER "tblLeads_ITrig" ON dbo.tblAllLeads FOR INSERT AS
SET NOCOUNT ON
IF (SELECT Count(*) FROM inserted WHERE NOT (ActiveInactive='A' Or ActiveInactive='I')) > 0
    BEGIN
        RAISERROR 44444 'You must enter either an A for Active or an I for Inactive in this field.'
        ROLLBACK TRANSACTION
    END

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE TRIGGER tblParts_ITrig  ON [tblParts] 
FOR INSERT 
AS
SET NOCOUNT ON
IF (SELECT COUNT(*) FROM inserted WHERE RPSPartNum IS NOT NULL and charindex('-',RPSPartNum) > 0) > 0
  begin
	update a
	set a.RPSPNSort = substring(a.RPSPartNum, 1, charindex('-', a.RPSPartNum)-1) + replicate('0', 10 - charindex('-', a.RPSPartNum)-1)
		+ '-' + substring(a.RPSPartNum, charindex('-', a.RPSPartNum) + 1, 21) + replicate('0', 21 - len(substring(a.RPSPartNum, charindex('-', a.RPSPartNum) + 1, 21)))
	from dbo.tblParts a, inserted
	where inserted.RPSPartNum is not null and charindex('-',inserted.RPSPartNum) > 0 and inserted.RPSPartNum = a.RPSPartNum
  end
IF (SELECT COUNT(*) FROM inserted WHERE RPSPartNum IS NOT NULL and charindex('-',RPSPartNum)= 0) > 0
  begin
	update a
	set a.RPSPNSort = a.RPSPartNum + replicate('0',30 - len(a.RPSPartNum))
	from dbo.tblParts a, inserted
	where a.RPSPartNum is not null and charindex('-',a.RPSPartNum) = 0 and inserted.RPSPartNum = a.RPSPartNum
  end



GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE TRIGGER tblParts_UTring ON tblParts
FOR UPDATE
AS
SET NOCOUNT ON
IF (SELECT COUNT(*) FROM inserted WHERE RPSPartNum IS NOT NULL and charindex('-',RPSPartNum) > 0) > 0
  begin
	update a
	set a.RPSPNSort = substring(a.RPSPartNum, 1, charindex('-', a.RPSPartNum)-1) + replicate('0', 10 - charindex('-', a.RPSPartNum)-1)
		+ '-' + substring(a.RPSPartNum, charindex('-', a.RPSPartNum) + 1, 21) + replicate('0', 21 - len(substring(a.RPSPartNum, charindex('-', a.RPSPartNum) + 1, 21)))
	from dbo.tblParts a, inserted
	where inserted.RPSPartNum is not null and charindex('-',inserted.RPSPartNum) > 0 and inserted.RPSPartNum = a.RPSPartNum
  end
IF (SELECT COUNT(*) FROM inserted WHERE RPSPartNum IS NOT NULL and charindex('-',RPSPartNum)= 0) > 0
  begin
	update a
	set a.RPSPNSort = a.RPSPartNum + replicate('0',30 - len(a.RPSPartNum))
	from dbo.tblParts a, inserted
	where a.RPSPartNum is not null and charindex('-',a.RPSPartNum) = 0 and inserted.RPSPartNum = a.RPSPartNum
  end

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

