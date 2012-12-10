-- fcdata update 4.03a

-- Rocore Industries, Advance Mech, Jensen Metal Products, Inc.

update tblParts
set 	DealerNet = 2.5 * CostEach,
	USADealerNet = 2.5 * CostEach
where	(VendorName like 'Rocore%') or
	(VendorName like 'Advance%') or
	(VendorName like 'Jensen%')
go

update tblParts
set	SuggestedList = DealerNet * 1.6666,
	USASuggestedList = USADealerNet * 1.6666
where	(VendorName like 'Rocore%') or
	(VendorName like 'Advance%') or
	(VendorName like 'Jensen%')
go
	