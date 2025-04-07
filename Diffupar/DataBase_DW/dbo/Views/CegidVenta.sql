CREATE VIEW [dbo].[CegidVenta]
	AS 
select 
 
 p.GP_DATEPIECE,
 p.GP_ETABLISSEMENT,
 p.GP_NUMERO,
 p.GP_TOTALTTC Venta_Total,
 p.GP_TOTALHT,
 p.GP_PERIODE,
 p.GP_REFINTERNE,
 p.GP_DATEREFEXTERNE, ---CUIT
 ---p.GP_TYPEFISCAL,
 ---p.GP_TIERS,
 p.GP_NUMPIECE, ------NRO DE FACTURA CON "-" EN LUGAR DE LETRA
 p.GP_REPRESENTANT,
 p.GP_TOTALQTERESTE, ----ITEMS TOTALES EN TICKET
 P.GP_HEURECREATION, ----FECHA Y HORA DEL TICKET
CONCAT(p.GP_ETABLISSEMENT,p.GP_NUMERO) as U_RBI_EXTERCODE,
left(convert(varchar,p.gp_datepiece, 112),6) as ID_date_target,
left(convert(varchar,p.gp_datepiece, 112),8) as ID_date_ymd_target,
convert(int,p.GP_ETABLISSEMENT) AS id_sucursal_converted,
e.ET_ETABLISSEMENT,
e.ET_LIBELLE,
case
when substring(ET_ETABLISSEMENT,1,1)='1' then 'Rouge'
when substring(ET_ETABLISSEMENT,1,1)='2' then 'Beauty 24'
end as 'Tipo Unidad Retail',
e.ET_CODEPOSTAL,
CL.PK_CUSTOMER



from [200.32.54.90].Produccion.[dbo].[PIECE] as p
inner join [200.32.54.90].Produccion.[dbo].[ETABLISS] as e on e.ET_ETABLISSEMENT=p.GP_ETABLISSEMENT
left join(
select 
 T_TIERS AS PK_CUSTOMER
 FROM [200.32.54.90].Produccion.[dbo].TIERS 
where T_NATUREAUXI='CLI'
--AND T_PARTICULIER='X'
--AND T_TIERS<>'9999'

 ) as CL on CL.PK_CUSTOMER=P.GP_TIERS


