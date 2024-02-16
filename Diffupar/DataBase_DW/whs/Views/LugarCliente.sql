CREATE VIEW [whs].[LugarCliente]
	AS
Select
Id,
Codigo OcrCode3,
trim(Lugar_Cliente) Lugar_Cliente
From whs.DimLugarCliente with(nolock)
