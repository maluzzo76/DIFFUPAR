------------------------------------------------
-- Vista Cegid Vendededores
------------------------------------------------
create view rtl.Vendedores
as

SELECT 
GCL_COMMERCIAL,
GCL_LIBELLE,
GCL_PRENOM,
GCL_SURNOM

FROM [200.32.54.90].Produccion.[dbo].COMMERCIAL