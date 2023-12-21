CREATE TABLE [whs].[DimBusinessPartner]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1),
	CardCode	varchar(100),
	CardName	varchar(300),
	CardType	varchar(300),
	CmpPrivate	varchar(300),
	BusinessPartnerGroup_Id	int,--(OCRG)
	CondicionPago_Id int,--(OCTG
	Vendedor_Id int,--(OSLP)
	Provincia_Id int, --(OCST)
	FOREIGN KEY (BusinessPartnerGroup_Id) REFERENCES whs.DimBusinessPartnerGroup(Id),
	FOREIGN KEY (CondicionPago_Id) REFERENCES whs.DimCondicionesPago(Id),
	FOREIGN KEY (Vendedor_Id) REFERENCES whs.DimVendedores(Id),
	FOREIGN KEY (Provincia_Id) REFERENCES whs.DimProvincias(Id)
)
