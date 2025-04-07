delete stg.SucursalRetail
insert into stg.SucursalRetail
select distinct lc.OcrCode3,sc.ET_LIBELLE SCegid
from Cegid_Alusoft.dbo.Overview sc 
left join whs.LugarCliente lc on TRIM( 
										replace(
											replace(
														replace(												
																REPLACE(Lugar_Cliente,'Loc -','')												
														,'Gondola','Gond')
											,'SHOPPING','SHOPP')
										,'CABILDO MAISON','MAISON CABILDO')
									)
								collate Modern_Spanish_CI_AS 
								= sc.ET_LIBELLE collate Modern_Spanish_CI_AS
where OcrCode3 is not null

-------------------------------------------------------------------------------------------------
-- Agregar los grupos
-------------------------------------------------------------------------------------------------
update stg.SucursalRetail set grupo = r1.Grupo
from(
	select 
		ocrCode3,
		case
		when	   [NombreSucursalRetail]='ALTO AVELLANEDA'
				OR [NombreSucursalRetail]='ALTO PALERMO GOND' 
				OR [NombreSucursalRetail]='IMPRENTA' 
				OR [NombreSucursalRetail]='VILLA DEL PARQUE' 
				OR [NombreSucursalRetail]='PASEO ALCORTA 2P'
				OR [NombreSucursalRetail]='MENDOZA PLAZA'
				OR [NombreSucursalRetail]='PATIO BULLRICH PB'
				OR [NombreSucursalRetail]='VILLA DEL PARQUE'
				OR [NombreSucursalRetail]='PATIO BULLRICH PB'
				OR [NombreSucursalRetail]='QUILMES FACTORY'
				OR [NombreSucursalRetail]='MARKETING RETAIL'
				OR [NombreSucursalRetail]='DISTRITO ARCOS GOND'
				OR [NombreSucursalRetail]='CREED ALVEAR'
				OR [NombreSucursalRetail]='CABILDO Y PAMPA'
				OR [NombreSucursalRetail]='ALTO ROSARIO'
				OR [NombreSucursalRetail]='ABASTO MAC'
				OR [NombreSucursalRetail]='ALTO PALERMO MAC'
				OR [NombreSucursalRetail]='ALTO ROSARIO MAC'
				OR [NombreSucursalRetail]='PALERMO SOHO MAC'
				OR [NombreSucursalRetail]='MENDOZA PLAZA GOND'
				OR [NombreSucursalRetail]='GALERIAS PACIFICO MAC'
				OR [NombreSucursalRetail]='PALMARES'
				OR [NombreSucursalRetail]='ALTO AVELLANEDA MALA' then 'ST NO COMPARABLE'
 
		when (LEFT([NombreSucursalRetail],3)='B24' and [NombreSucursalRetail]<>'B24 ECOMMERCE' )then 'BEAUTY 24'
		when [NombreSucursalRetail]='CONNECTE' THEN 'ECOMMERCE'
		when [NombreSucursalRetail]='B24 ECOMMERCE' THEN 'B24 ECOMMERCE'
		when [NombreSucursalRetail]='OUTLET' THEN 'OUTLET'
		ELSE 'ST COMPARABLE'
		END Grupo
	from stg.sucursalretail
) as r1
where stg.SucursalRetail.ocrCode3 = r1.ocrCode3
