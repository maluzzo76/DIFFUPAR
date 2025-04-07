CREATE VIEW rtl.MediosDePago
as
select 
pie.gpe_libelle as tipo,
pie.GPE_DATEPIECE,
pie.GPE_CAISSE as caja_almacen,
PIE.GPE_NUMECHE as num_vencimiento,
pie.GPE_MODEPAIE as cod_tipo,
PIE.GPE_LIBELLE AS DESCRIPCION_PAGO,
PIE.GPE_CBLIBELLE as apellido_tarjeta,
PIE.GPE_CBNUMTRANSAC as nro_tarjeta,
CONCAT( pie.GPE_ETABLISSEMENT,pie.GPE_NUMERO ) as fk_piece,
pie.gpe_montanteche as monto_compra,
pie.GPE_CBNBVERSEMENT as cuotas,
pie.gpe_cbtypversement as tipo_pagos_escalonados,
pie.GPE_NUMERO

from [200.32.54.90].Produccion.dbo.PIEDECHE as pie WITH (NoLocK)
where pie.GPE_NATUREPIECEG='FFO'
and pie.GPE_MODEPAIE<>'Y'