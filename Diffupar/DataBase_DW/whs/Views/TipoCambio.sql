CREATE VIEW [whs].[TipoCambio]
	AS 
Select Fecha,Moneda,TipoCambio from whs.DimTipoCambio
where Fecha > ( Select max(Fecha) fecha from whs.DimTipoCambioHistorico )
union all 
Select Fecha,Moneda,TipoCambio from whs.DimTipoCambioHistorico
