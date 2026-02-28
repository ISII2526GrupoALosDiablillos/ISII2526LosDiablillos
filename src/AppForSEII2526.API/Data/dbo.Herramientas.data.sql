SET IDENTITY_INSERT [dbo].[Herramientas] ON
INSERT INTO [dbo].[Herramientas] ([id], [itemsReparacion], [material], [nombre], [precio], [tiempoReparacion], [fabricanteId]) VALUES (1, 1, N'Madera', N'Martillo', 20, 10, 1)
INSERT INTO [dbo].[Herramientas] ([id], [itemsReparacion], [material], [nombre], [precio], [tiempoReparacion], [fabricanteId]) VALUES (3, 2, N'Metal', N'Destornillador', 15, 5, 2)
INSERT INTO [dbo].[Herramientas] ([id], [itemsReparacion], [material], [nombre], [precio], [tiempoReparacion], [fabricanteId]) VALUES (4, 6, N'Madera', N'Astillas', 5, 2, 3)
INSERT INTO [dbo].[Herramientas] ([id], [itemsReparacion], [material], [nombre], [precio], [tiempoReparacion], [fabricanteId]) VALUES (6, 3, N'Acero', N'Sierra', 20, 12, 4)
SET IDENTITY_INSERT [dbo].[Herramientas] OFF
