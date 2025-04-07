CREATE PROCEDURE [whs].[Sp_ActualizarFactVentas]
AS

declare @FechaComplementoManual datetime

set @FechaComplementoManual = (select max(Fecha_Contabilizacion) from whs.VentasMensual where Origen = 'MANUAL' and AcctCode in('4.1.010.10.001','4.1.010.20.001'))
 
delete whs.factVentas

--------------------------------------------------------------
-- Actualizo los vendedores desde cgid
--------------------------------------------------------------
update Whs.FactComprobantes set  VendorNum = r1.vendedor_codigo
from
(
	select * from stg.cegidVentaVendedor 
)as r1
where Whs.FactComprobantes.U_RBI_EXTCOD = r1.U_RBI_EXTERCODE

--------------------------------------------------------------
-- Inserta la historia y los complementos Manuales
--------------------------------------------------------------
insert into  whs.factVentas
select * from whs.VentasMensual where Origen Not like 'SAP%' and AcctCode in('4.1.010.10.001','4.1.010.20.001')

--------------------------------------------------------------
-- Inserta las transacciones de SAP y Complementos SAPB
--------------------------------------------------------------
insert into  whs.factVentas
select * from whs.Facturacion where Origen like 'SAP%' and AcctCode in('4.1.010.10.001','4.1.010.20.001') and Fecha_Contabilizacion > @FechaComplementoManual


