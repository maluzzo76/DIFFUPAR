﻿CREATE TABLE [stg].[FacturaDetalle]
(
	DocEntry int,
	LineNum int,
	ItemCode varchar(500),
	Price decimal(18,2),
	Currency varchar(20),
	LineTotal decimal(18,2),
	OpenSum decimal(18,2),
	SlpCode int,
	AcctCode varchar(500),
	PriceBefDi decimal(18,2),
	DocDate datetime,
	BaseCard varchar(500),
	TotalSumSy decimal(18,2),
	OpenSumSys decimal(18,2),
	VatPrcnt decimal(18,2),
	PriceAfVAT decimal(18,2),
	VatSum decimal(18,2),
	VatSumSy decimal(18,2),
	DedVatSum decimal(18,2),
	DedVatSumS decimal(18,2),
	GrssProfit decimal(18,2),
	GrssProfSC decimal(18,2),
	VisOrder int,
	INMPrice decimal(18,2),
	TaxCode varchar(100),
	LineVat decimal(18,2),
	LineVatS decimal(18,2),
	OwnerCode varchar(500),
	StockSum decimal(18,2),
	StockSumSc decimal(18,2),
	ShipToCode varchar(500),
	ShipToDesc varchar(500),
	GTotal decimal(18,2),
	GTotalSC decimal(18,2),
	TaxOnly varchar(500),
	CogsOcrCod int,
	CogsAcct varchar(500),
	CogsOcrCo2 int,
	CogsOcrCo3 int,
	CogsOcrCo4 int,
	CogsOcrCo5 int,
	GPBefDisc decimal(18,2)

)
go

CREATE NONCLUSTERED INDEX [ind_factura_Detalle]
ON [stg].[FacturaDetalle] ([DocEntry])
INCLUDE ([LineNum],[ItemCode],[Price],[Currency],[LineTotal],[OpenSum],[SlpCode],[AcctCode],[PriceBefDi],[DocDate],[BaseCard],[TotalSumSy],[OpenSumSys],[VatPrcnt],[PriceAfVAT],[VatSum],[VatSumSy],[DedVatSum],[DedVatSumS],[GrssProfit],[GrssProfSC],[VisOrder],[INMPrice],[TaxCode],[LineVat],[LineVatS],[OwnerCode],[StockSum],[StockSumSc],[ShipToCode],[ShipToDesc],[GTotal],[GTotalSC],[TaxOnly],[CogsOcrCod],[CogsAcct],[CogsOcrCo2],[CogsOcrCo3],[CogsOcrCo4],[CogsOcrCo5],[GPBefDisc])
GO