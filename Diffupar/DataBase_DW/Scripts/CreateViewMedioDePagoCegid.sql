------------------------------------------------
-- Vista Cegid Medio de pago
------------------------------------------------
create view rtl.MedioDePago
as
select 
pie.gpe_libelle as tipo,
pie.GPE_MODEPAIE as cod_tipo,
CONCAT( cast (pie.GPE_ETABLISSEMENT as int),cast(pie.GPE_NUMERO as int)) as fk_piece,
pie.gpe_montanteche as monto_compra,
pie.GPE_CBNBVERSEMENT as cuotas,
pie.GPE_ETABLISSEMENT,
pie.GPE_NUMERO

from [200.32.54.90].Produccion.dbo.PIEDECHE as pie
where pie.GPE_NATUREPIECEG='FFO'
and pie.GPE_MODEPAIE<>'Y'