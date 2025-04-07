create view rtl.DetalleDeVantaPorMedioDePago
as
select 
Fecha,
fd.fk_piece,
GPE_NUMERO,
fc.Sucursal,
pk_piece,
GP_REFINTERNE,
fc.Sucursal +' '+
case when LEFT( (RIGHT([GP_REFINTERNE],4)),1)='0' then ''
ELSE LEFT( (RIGHT([GP_REFINTERNE],4)),1)
end  + RIGHT([GP_REFINTERNE],3) PK_Control_OB,
cli.nombre_Cliente + ' ' + cli.Apellido_Cliente NombreApellidoCliente,
mp.caja_almacen ,
mp.cod_tipo CodigoTipo,
case 
when left([tipo],4)= 'VISA' and [tipo] like '%DEBIT%' then 'DEBITO'
when left([tipo],4) ='VISA'   then 'VISA'
when left([tipo],5)='MASTER' and tipo like '%DEBIT%' THEN 'DEBITO'
when left([tipo],5) = 'MASTER' then 'MASTERCARD'
when left([tipo],4)='AMEX' then 'AMEX'
when left([tipo],5)='CABAL' and tipo like '%DEBIT%' THEN 'DEBITO'
when left([tipo],5)= 'CABAL' then 'CABAL'
when left([tipo],12) = 'MERCADO PAGO' and tipo like '%DEBIT%' THEN 'DEBITO'
when left([tipo],12)='MERCADO PAGO' then 'MERCADO PAGO'
when left([tipo],9)='TODO PAGO' and tipo like '%DEBIT%' THEN 'DEBITO'
when left([tipo],10)='TODO PAGO' THEN 'TODO PAGO'
when [tipo] like '%EFECTIVO%' then 'EFECTIVO'
when [tipo] like '%ARGENCARD%' then 'ARGENCARD'
when left([tipo],9)='TODO PAGO' then 'TODO PAGO'
when left([tipo],5)='BAHIA' then 'BAHIA SHOPPING'
when left([tipo],16)= 'TARJETA CENCOSUD' then 'TARJETA CENCOSUD'
when left([tipo],16)= 'TARJETA SHOPPING' then 'TARJETA SHOPPING'
when left([tipo],9) ='COOPEPLUS' then 'COOPEPLUS'
when [tipo] like '%ELECTRON%' then 'DEBITO'
when [tipo] like '%TARJETA NARANJA%' then 'TARJETA NARANJA'
when Left([tipo],6)='DINERS' then 'DINERS'
when [tipo] like '%NATIVA%' then 'NATIVA'
when [tipo] like '%MAESTRO%' then 'DEBITO'
ELSE [tipo]
END MedioDePago,
mp.DESCRIPCION_PAGO,
mp.tipo TipoMedioPago,
case 
	when [tipo] like '%MERCADO PAGO%' THEN tipo
	when [tipo] like '%TODO PAGO%' THEN tipo
ELSE tipo
END Banco_Terjeta,
mp.cuotas,
case
	when[cuotas]=13 then 'Ahora 3'
	when [cuotas]=16 then 'Ahora 6'
ELSE str([cuotas])
END Cuotasv3,
mp.nro_tarjeta,
mp.apellido_tarjeta,
mp.monto_compra

from rtl.FacturaCabecera fc
inner join rtl.FacturaDetalle fd on fd.fk_piece collate Modern_Spanish_CI_AS = fc.pk_piece collate Modern_Spanish_CI_AS
inner join rtl.MediosDePago mp on mp.fk_piece collate Modern_Spanish_CI_AS = pk_piece collate Modern_Spanish_CI_AS
inner join rtl.Clientes cli on cli.T_TIERS = fc.Cliente