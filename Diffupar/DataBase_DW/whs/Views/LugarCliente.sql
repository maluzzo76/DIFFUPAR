CREATE VIEW [whs].[LugarCliente]
	AS
Select
Id,
Codigo OcrCode3,
trim(Lugar_Cliente) Lugar_Cliente,
sc.NombreSucursalRetail SucursalRetail,
sc.Grupo
From whs.DimLugarCliente lc with(nolock)
left join stg.SucursalRetail sc on sc.ocrCode3 = lc.Codigo