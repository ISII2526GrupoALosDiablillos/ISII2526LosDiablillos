SET IDENTITY_INSERT [dbo].[Fabricantes] ON
INSERT INTO [dbo].[Fabricantes] ([Id], [Nombre]) VALUES (1, N'Paco')
INSERT INTO [dbo].[Fabricantes] ([Id], [Nombre]) VALUES (2, N'María')
SET IDENTITY_INSERT [dbo].[Fabricantes] OFF
SET IDENTITY_INSERT [dbo].[Herramientas] ON
INSERT INTO [dbo].[Herramientas] ([id], [itemsReparacion], [material], [nombre], [OfertaItems], [precio], [tiempoReparacion], [fabricanteId]) VALUES (4, 12, N'Madera', N'Pepe', 3, 69, 180, 1)
INSERT INTO [dbo].[Herramientas] ([id], [itemsReparacion], [material], [nombre], [OfertaItems], [precio], [tiempoReparacion], [fabricanteId]) VALUES (5, 8, N'Hierro', N'Lucía', 7, 88, 200, 2)
SET IDENTITY_INSERT [dbo].[Herramientas] OFF
