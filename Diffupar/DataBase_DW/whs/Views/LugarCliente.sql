CREATE VIEW [whs].[LugarCliente]
	AS
Select
Codigo,
trim(Lugar_Cliente) Lugar_Cliente
From whs.DimLugarCliente with(nolock)
