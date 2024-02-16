CREATE VIEW [whs].[Departamentos]
	AS 
SELECT
	Id,
	Codigo OcrCode2,
	trim(Departamento) Departamento,
	trim(Area) Area
FROM whs.DimDepartamentos WITH(NOLOCK)
