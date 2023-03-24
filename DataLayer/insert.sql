SET IDENTITY_INSERT [dbo].[KategorieInstituce] ON
INSERT INTO [dbo].[KategorieInstituce] ([Id], [Nazev]) VALUES (1, N'Vzdelání')
INSERT INTO [dbo].[KategorieInstituce] ([Id], [Nazev]) VALUES (2, N'Zábava')
INSERT INTO [dbo].[KategorieInstituce] ([Id], [Nazev]) VALUES (3, N'Kino')
INSERT INTO [dbo].[KategorieInstituce] ([Id], [Nazev]) VALUES (4, N'Volný cas')
SET IDENTITY_INSERT [dbo].[KategorieInstituce] OFF

SET IDENTITY_INSERT [dbo].[Objekt] ON
INSERT INTO [dbo].[Objekt] ([Id], [HierarchieId], [Nazev], [SmazanoOd]) VALUES (1, N'/1/', N'Kino', NULL)
INSERT INTO [dbo].[Objekt] ([Id], [HierarchieId], [Nazev], [SmazanoOd]) VALUES (2, N'/2/', N'Tělocvična', NULL)
INSERT INTO [dbo].[Objekt] ([Id], [HierarchieId], [Nazev], [SmazanoOd]) VALUES (3, N'/3/', N'Škola', NULL)
INSERT INTO [dbo].[Objekt] ([Id], [HierarchieId], [Nazev], [SmazanoOd]) VALUES (4, N'/3/1/', N'FEI', NULL)
INSERT INTO [dbo].[Objekt] ([Id], [HierarchieId], [Nazev], [SmazanoOd]) VALUES (5, N'/3/2/', N'EkF', NULL)
INSERT INTO [dbo].[Objekt] ([Id], [HierarchieId], [Nazev], [SmazanoOd]) VALUES (6, N'/3/1/1/', N'EC1', NULL)
INSERT INTO [dbo].[Objekt] ([Id], [HierarchieId], [Nazev], [SmazanoOd]) VALUES (7, N'/4/', N'Divadlo', NULL)
INSERT INTO [dbo].[Objekt] ([Id], [HierarchieId], [Nazev], [SmazanoOd]) VALUES (8, N'/5/', N'Dopravní podnik', NULL)
INSERT INTO [dbo].[Objekt] ([Id], [HierarchieId], [Nazev], [SmazanoOd]) VALUES (9, N'/6/', N'Škola Olomouc', NULL)
INSERT INTO [dbo].[Objekt] ([Id], [HierarchieId], [Nazev], [SmazanoOd]) VALUES (10, N'/7/', N'Bazen', NULL)
INSERT INTO [dbo].[Objekt] ([Id], [HierarchieId], [Nazev], [SmazanoOd]) VALUES (11, N'/3/1/2/', N'EC2', NULL)
INSERT INTO [dbo].[Objekt] ([Id], [HierarchieId], [Nazev], [SmazanoOd]) VALUES (12, N'/3/1/3/', N'EC3', NULL)
SET IDENTITY_INSERT [dbo].[Objekt] OFF

INSERT INTO [dbo].[Instituce] ([Id], [Email], [Telefon], [Ulice], [Cislo_popisne], [Mesto], [PSC]) VALUES (1, N'email1@seznam.cz', NULL, N'Místecká', N'123', N'Paskov', 73921)
INSERT INTO [dbo].[Instituce] ([Id], [Email], [Telefon], [Ulice], [Cislo_popisne], [Mesto], [PSC]) VALUES (2, N'email2@seznam.cz', NULL, N'Pod altánem', N'1230', N'Ostrava', 70030)
INSERT INTO [dbo].[Instituce] ([Id], [Email], [Telefon], [Ulice], [Cislo_popisne], [Mesto], [PSC]) VALUES (3, N'email3@seznam.cz', 608123456, N'Újezdská', N'123/45', N'Praha', 10000)
INSERT INTO [dbo].[Instituce] ([Id], [Email], [Telefon], [Ulice], [Cislo_popisne], [Mesto], [PSC]) VALUES (7, N'email4@seznam.cz', NULL, N'Družstevní', N'421/1', N'Adamov', 67908)
INSERT INTO [dbo].[Instituce] ([Id], [Email], [Telefon], [Ulice], [Cislo_popisne], [Mesto], [PSC]) VALUES (8, N'email5@seznam.cz', NULL, NULL, N'576', N'Baška', 73901)
INSERT INTO [dbo].[Instituce] ([Id], [Email], [Telefon], [Ulice], [Cislo_popisne], [Mesto], [PSC]) VALUES (9, N'email6@seznam.cz', NULL, N'Křížkovského', N'844/3', N'Olomouc', 77100)
INSERT INTO [dbo].[Instituce] ([Id], [Email], [Telefon], [Ulice], [Cislo_popisne], [Mesto], [PSC]) VALUES (10, N'email7@seznam.cz', NULL, NULL, N'21', N'Bohuslavice', 79856)

INSERT INTO [dbo].[Instituce_KategorieInstituce] ([InstituceId], [KategorieInstituceId]) VALUES (1, 2)
INSERT INTO [dbo].[Instituce_KategorieInstituce] ([InstituceId], [KategorieInstituceId]) VALUES (1, 3)
INSERT INTO [dbo].[Instituce_KategorieInstituce] ([InstituceId], [KategorieInstituceId]) VALUES (2, 4)
INSERT INTO [dbo].[Instituce_KategorieInstituce] ([InstituceId], [KategorieInstituceId]) VALUES (3, 1)
INSERT INTO [dbo].[Instituce_KategorieInstituce] ([InstituceId], [KategorieInstituceId]) VALUES (7, 2)
INSERT INTO [dbo].[Instituce_KategorieInstituce] ([InstituceId], [KategorieInstituceId]) VALUES (9, 1)
INSERT INTO [dbo].[Instituce_KategorieInstituce] ([InstituceId], [KategorieInstituceId]) VALUES (10, 2)


SET IDENTITY_INSERT [dbo].[Udalost] ON
INSERT INTO [dbo].[Udalost] ([Id], [Nazev], [ObjektId], [Start], [Konec], [RezervaceOd], [RezervaceDo], [SmazanoOd]) VALUES (1, N'Prednáška 1', 6, N'2018-12-16 10:00:00', N'2018-12-16 12:00:00', N'2018-12-15 10:00:00', N'2018-12-16 10:00:00', NULL)
INSERT INTO [dbo].[Udalost] ([Id], [Nazev], [ObjektId], [Start], [Konec], [RezervaceOd], [RezervaceDo], [SmazanoOd]) VALUES (2, N'Prednáška 2', 6, N'2018-12-16 10:00:00', N'2018-12-16 12:00:00', N'2018-12-15 10:00:00', N'2018-12-16 10:00:00', NULL)
SET IDENTITY_INSERT [dbo].[Udalost] OFF

INSERT INTO [dbo].[RezervacniObjekt] ([Id], [Cena], [Pocet], [TypRezervace]) VALUES (6, 150, 6, 0)
INSERT INTO [dbo].[RezervacniObjekt] ([Id], [Cena], [Pocet], [TypRezervace]) VALUES (1, 200, 50, 0)
INSERT INTO [dbo].[RezervacniObjekt] ([Id], [Cena], [Pocet], [TypRezervace]) VALUES (2, 4000, 1, 0)
INSERT INTO [dbo].[RezervacniObjekt] ([Id], [Cena], [Pocet], [TypRezervace]) VALUES (7, 350, 30, 0)
INSERT INTO [dbo].[RezervacniObjekt] ([Id], [Cena], [Pocet], [TypRezervace]) VALUES (8, 0, 1, 0)
INSERT INTO [dbo].[RezervacniObjekt] ([Id], [Cena], [Pocet], [TypRezervace]) VALUES (10, 300, 6, 0)

SET IDENTITY_INSERT [dbo].[Zakaznik] ON
INSERT INTO [dbo].[Zakaznik] ([Id], [Jmeno], [Prijmeni], [Email], [Telefon], [Ulice], [Cislo_popisne], [Mesto], [PSC]) VALUES (1, N'Karel', N'Nový', N'customer@seznam.cz', NULL, NULL, N'1234', N'Frýdek-Místek', 73802)
SET IDENTITY_INSERT [dbo].[Zakaznik] OFF

SET IDENTITY_INSERT [dbo].[Rezervace] ON
INSERT INTO [dbo].[Rezervace] ([Id], [ZakaznikId], [RezervacniObjektId], [UdalostId], [Od], [Do], [SmazanoOd]) VALUES (2004, 1, 6, 1, N'2018-12-16 10:00:00', N'2018-12-16 12:00:00', NULL)
INSERT INTO [dbo].[Rezervace] ([Id], [ZakaznikId], [RezervacniObjektId], [UdalostId], [Od], [Do], [SmazanoOd]) VALUES (2005, 1, 6, 1, N'2018-12-16 10:00:00', N'2018-12-16 12:00:00', NULL)
INSERT INTO [dbo].[Rezervace] ([Id], [ZakaznikId], [RezervacniObjektId], [UdalostId], [Od], [Do], [SmazanoOd]) VALUES (2006, 1, 6, 1, N'2018-12-16 10:00:00', N'2018-12-16 12:00:00', NULL)
INSERT INTO [dbo].[Rezervace] ([Id], [ZakaznikId], [RezervacniObjektId], [UdalostId], [Od], [Do], [SmazanoOd]) VALUES (2007, 1, 1, NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[Rezervace] ([Id], [ZakaznikId], [RezervacniObjektId], [UdalostId], [Od], [Do], [SmazanoOd]) VALUES (2008, 1, 1, NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[Rezervace] ([Id], [ZakaznikId], [RezervacniObjektId], [UdalostId], [Od], [Do], [SmazanoOd]) VALUES (2009, 1, 1, NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[Rezervace] ([Id], [ZakaznikId], [RezervacniObjektId], [UdalostId], [Od], [Do], [SmazanoOd]) VALUES (2010, 1, 1, NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[Rezervace] ([Id], [ZakaznikId], [RezervacniObjektId], [UdalostId], [Od], [Do], [SmazanoOd]) VALUES (2011, 1, 1, NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[Rezervace] ([Id], [ZakaznikId], [RezervacniObjektId], [UdalostId], [Od], [Do], [SmazanoOd]) VALUES (2012, 1, 1, NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[Rezervace] ([Id], [ZakaznikId], [RezervacniObjektId], [UdalostId], [Od], [Do], [SmazanoOd]) VALUES (2013, 1, 1, NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[Rezervace] ([Id], [ZakaznikId], [RezervacniObjektId], [UdalostId], [Od], [Do], [SmazanoOd]) VALUES (2014, 1, 1, NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[Rezervace] ([Id], [ZakaznikId], [RezervacniObjektId], [UdalostId], [Od], [Do], [SmazanoOd]) VALUES (2015, 1, 1, NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[Rezervace] ([Id], [ZakaznikId], [RezervacniObjektId], [UdalostId], [Od], [Do], [SmazanoOd]) VALUES (2016, 1, 2, NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[Rezervace] ([Id], [ZakaznikId], [RezervacniObjektId], [UdalostId], [Od], [Do], [SmazanoOd]) VALUES (2017, 1, 7, NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[Rezervace] ([Id], [ZakaznikId], [RezervacniObjektId], [UdalostId], [Od], [Do], [SmazanoOd]) VALUES (2018, 1, 7, NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[Rezervace] ([Id], [ZakaznikId], [RezervacniObjektId], [UdalostId], [Od], [Do], [SmazanoOd]) VALUES (2019, 1, 7, NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[Rezervace] ([Id], [ZakaznikId], [RezervacniObjektId], [UdalostId], [Od], [Do], [SmazanoOd]) VALUES (2020, 1, 7, NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[Rezervace] ([Id], [ZakaznikId], [RezervacniObjektId], [UdalostId], [Od], [Do], [SmazanoOd]) VALUES (2021, 1, 8, NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[Rezervace] ([Id], [ZakaznikId], [RezervacniObjektId], [UdalostId], [Od], [Do], [SmazanoOd]) VALUES (2022, 1, 10, NULL, NULL, NULL, NULL)
INSERT INTO [dbo].[Rezervace] ([Id], [ZakaznikId], [RezervacniObjektId], [UdalostId], [Od], [Do], [SmazanoOd]) VALUES (2023, 1, 10, NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Rezervace] OFF

