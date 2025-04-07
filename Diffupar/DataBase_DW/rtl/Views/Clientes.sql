CREATE VIEW rtl.Clientes
as
select 
T_PRENOM as nombre_cliente,
T_LIBELLE as apellido_cliente,
t_email,
t_ville as partido_cliente,
T_ADRESSE1 as domicilio_cliente,
T_CODEPOSTAL as codigo_postal_cliente,
T_TELEPHONE as telefono_cliente,
t_sexe as sexo_cliente,
t_journaissance as dia_nacimiento,
t_moisnaissance as mes_nacimiento,
t_anneenaissance as year_nacimiento,
t_particulier as particular,
T_TIERS

from [200.32.54.90].produccion.dbo.TIERS WITH (NoLock)
where T_NATUREAUXI='CLI'
----and convert(date,t_datedernpiece)>='2021-01-01'