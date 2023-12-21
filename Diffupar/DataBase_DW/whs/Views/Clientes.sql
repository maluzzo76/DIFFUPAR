CREATE VIEW [whs].[Clientes]
AS
select 
bp.id,
bp.CardCode Cliente_Codigo,
bp.CardName Cliente_Nombre,
bp.CardType Cliente_Tipo,
bp.CmpPrivate Cliente_Privado,
bpg.GroupCode Grupo_Codigo,
bpg.GroupName Grupo_Nombre,
bpg.GroupType Grupo_Tipo,
cp.GroupNum CondicionPago_Numero,
cp.PymntGroup CondicionPago_GrupoPago,
v.SlpCode Vendedor_Codigo,
v.SlpName Vendedor_Nombre,
p.Codigo Provincia_Codigo,
p.Nombre Provincia_Nombre,
p.Pais Provincia_Pais

from whs.DimBusinessPartner bp
left join whs.DimBusinessPartnerGroup bpg on bpg.id = bp.BusinessPartnerGroup_Id
left join whs.DimCondicionesPago cp on cp.id = bp.CondicionPago_Id
left join whs.DimVendedores v on v.id = bp.Vendedor_Id
left join whs.DimProvincias p on p.id = bp.Provincia_Id
GO




