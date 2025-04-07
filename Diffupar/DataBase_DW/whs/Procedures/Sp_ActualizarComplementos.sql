CREATE PROCEDURE [whs].[Sp_ActualizarComplementos]
AS
	insert into whs.DimVendedores
select * from 
(
select * from stg.Vendedores
union all
select * from stg.VendedoresComplemento
) r1
where not exists(select * from whs.DimVendedores v where v.SlpCode = r1.SlpCode)

update whs.DimVendedores set SlpName = v1.SlpName
from(
	select * from stg.VendedoresComplemento
) as v1
where v1.SlpCode = whs.DimVendedores.SlpCode

