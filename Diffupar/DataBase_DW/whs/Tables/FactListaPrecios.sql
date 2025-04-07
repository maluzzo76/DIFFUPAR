CREATE TABLE [whs].[FactListaPrecios]
(
	ItemCode	Varchar(100),
	PriceList	Int,
	Price		Decimal(18,6),
	Currency	Varchar(50),
	Ovrwritten	Varchar(50),
	Factor		Decimal(18,6),
	LogInstanc	Int,
	ObjType		Int,
	AddPrice1	Decimal(18,6),
	Currency1	Varchar(50),
	AddPrice2	Decimal(18,6),
	Currency2	Varchar(50),
	Ovrwritten1	Varchar(50),
	BasePLNum	Int,
	UomEntry	Int,
	PriceType	Varchar(50),
	Producto_Id Int,
	FOREIGN KEY (Producto_Id) References whs.DimProductos(Id)
)
