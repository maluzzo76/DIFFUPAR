CREATE VIEW [whs].[Departamentos]
	AS 
SELECT
	Codigo,
	trim(Departamento) Departamento,
	trim(Area) Area
FROM whs.DimDepartamentos WITH(NOLOCK)
