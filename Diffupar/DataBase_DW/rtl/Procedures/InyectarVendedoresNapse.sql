insert into rtl.VendedoresNapse
select 
CodigoVendedor,
max(NombreVendedor)NombreVendedor
from DiffuparAnalytics.stg.NapseTransactions
group by CodigoVendedor