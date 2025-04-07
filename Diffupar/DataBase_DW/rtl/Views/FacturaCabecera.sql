create VIEW rtl.FacturaCabecera
as
select  
 p.GP_DATEPIECE Fecha,
 p.GP_ETABLISSEMENT,
 p.GP_NUMERO Numero,
 p.GP_TOTALTTC TotalCIva,
 p.GP_TOTALHT Venta_Neta,
 p.GP_PERIODE Periodo,
 p.GP_REFINTERNE,
 p.GP_DATEREFEXTERNE CUIT, ---CUIT
 ---p.GP_TYPEFISCAL,
 ---p.GP_TIERS,
 p.GP_NUMPIECE NumeroCompelto, ------NRO DE FACTURA CON "-" EN LUGAR DE LETRA
 p.GP_REPRESENTANT Representante,
 p.GP_TOTALQTERESTE, ----ITEMS TOTALES EN TICKET
 P.GP_HEURECREATION, ----FECHA Y HORA DEL TICKET
CONCAT( p.GP_ETABLISSEMENT,p.GP_NUMERO) as pk_piece,
left(convert(varchar,p.gp_datepiece, 112),6) as ID_date_target,
left(convert(varchar,p.gp_datepiece, 112),8) as ID_date_ymd_target,
e.ET_ETABLISSEMENT,
e.ET_LIBELLE Sucursal,
e.ET_CODEPOSTAL,
CL.PK_CUSTOMER Cliente,
case
		when	   e.ET_LIBELLE='ALTO AVELLANEDA'
				OR e.ET_LIBELLE='ALTO PALERMO GOND' 
				OR e.ET_LIBELLE ='IMPRENTA' 
				OR e.ET_LIBELLE ='VILLA DEL PARQUE' 
				OR e.ET_LIBELLE ='PASEO ALCORTA 2P'
				OR e.ET_LIBELLE ='MENDOZA PLAZA'
				OR e.ET_LIBELLE ='PATIO BULLRICH PB'
				OR e.ET_LIBELLE ='VILLA DEL PARQUE'
				OR e.ET_LIBELLE ='PATIO BULLRICH PB'
				OR e.ET_LIBELLE ='QUILMES FACTORY'
				OR e.ET_LIBELLE ='MARKETING RETAIL'
				OR e.ET_LIBELLE ='DISTRITO ARCOS GOND'
				OR e.ET_LIBELLE ='CREED ALVEAR'
				OR e.ET_LIBELLE ='CABILDO Y PAMPA'
				OR e.ET_LIBELLE ='ALTO ROSARIO'
				OR e.ET_LIBELLE ='ABASTO MAC'
				OR e.ET_LIBELLE ='ALTO PALERMO MAC'
				OR e.ET_LIBELLE ='ALTO ROSARIO MAC'
				OR e.ET_LIBELLE ='PALERMO SOHO MAC'
				OR e.ET_LIBELLE ='MENDOZA PLAZA GOND'
				OR e.ET_LIBELLE ='GALERIAS PACIFICO MAC'
				OR e.ET_LIBELLE ='PALMARES'
				OR e.ET_LIBELLE ='ALTO AVELLANEDA MALA' then 'ST NO COMPARABLE'
  
		when (LEFT(e.ET_LIBELLE,3)='B24' and e.ET_LIBELLE<>'B24 ECOMMERCE' )then 'BEAUTY 24'
		when e.ET_LIBELLE ='CONNECTE' THEN 'ECOMMERCE'
		when e.ET_LIBELLE ='B24 ECOMMERCE' THEN 'B24 ECOMMERCE'
		when e.ET_LIBELLE ='OUTLET' THEN 'OUTLET'
		ELSE 'ST COMPARABLE'
		END Grupo,
		cl.ClienteNombre,
		cl.ClienteApellido,
		cl.Zona as ClienteZona,
		cl.Email as ClienteEmail,
		cl.Genero as ClienteGenero



from [200.32.54.90].Produccion.[dbo].[PIECE] as p WITH(Nolock)
inner join [200.32.54.90].Produccion.[dbo].[ETABLISS] as e  WITH(Nolock) on e.ET_ETABLISSEMENT=p.GP_ETABLISSEMENT

left join(
			select
			T_TIERS AS PK_CUSTOMER,
			UPPER(t_prenom) as ClienteNombre,
			upper(t_libelle) as ClienteApellido,
			t_ville as Zona,
			LOWER(t_email) as Email,
			case
				when t_sexe = 'F' then 'FEMENINO'
				when t_sexe = 'M' then 'MASCULINO'
			else 
				UPPER(t_sexe)
			end as Genero 
			FROM [200.32.54.90].Produccion.dbo.TIERS p WITH(Nolock)
			where T_NATUREAUXI='CLI'
 		) as CL on CL.PK_CUSTOMER=P.GP_TIERS


where p.GP_NATUREPIECEG='FFO'
and convert (DATE,p.GP_DATEPIECE)>='2019-01-01'
----and e.ET_LIBELLE<>'OUTLET'
and gp_typecompta<>'TRE'