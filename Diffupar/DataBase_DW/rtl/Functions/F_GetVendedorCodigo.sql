alter FUNCTION rtl.F_GetVendedorCodigo(@VendedorNombre varchar(255))
Returns VARCHAR(200)
AS
Begin
Declare @Result VARCHAR(200)

set @Result = (
				select top 1
				CodigoVendedor 			
				from rtl.VendedoresNapse
				where NombreVendedor = @VendedorNombre
				)

RETURN @Result
End