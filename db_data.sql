USE [PRN231DB]
GO
SET IDENTITY_INSERT [dbo].[Teacher] ON 

INSERT [dbo].[Teacher] ([Id], [Name]) VALUES (1, N'someOne')
INSERT [dbo].[Teacher] ([Id], [Name]) VALUES (2, N'someThing')
INSERT [dbo].[Teacher] ([Id], [Name]) VALUES (3, N'someoneNew')
SET IDENTITY_INSERT [dbo].[Teacher] OFF
GO
SET IDENTITY_INSERT [dbo].[Class] ON 

INSERT [dbo].[Class] ([Id], [Name], [TeacherId]) VALUES (4, N'SE1506', 1)
INSERT [dbo].[Class] ([Id], [Name], [TeacherId]) VALUES (5, N'GD1506', 2)
INSERT [dbo].[Class] ([Id], [Name], [TeacherId]) VALUES (8, N'SE1523', 2)
SET IDENTITY_INSERT [dbo].[Class] OFF
GO
INSERT [dbo].[Student] ([Id], [Name]) VALUES (N'HE111111', N'DDD')
INSERT [dbo].[Student] ([Id], [Name]) VALUES (N'HE150544', N'Ta Kien Quoc')
INSERT [dbo].[Student] ([Id], [Name]) VALUES (N'HE153009', N'Chu Quoc Huy')
INSERT [dbo].[Student] ([Id], [Name]) VALUES (N'HE222222', N'NNN')
INSERT [dbo].[Student] ([Id], [Name]) VALUES (N'HE333333', N'SSS')
GO
INSERT [dbo].[Class_Student] ([ClassId], [StudentId]) VALUES (4, N'HE111111')
INSERT [dbo].[Class_Student] ([ClassId], [StudentId]) VALUES (4, N'HE150544')
INSERT [dbo].[Class_Student] ([ClassId], [StudentId]) VALUES (4, N'HE153009')
INSERT [dbo].[Class_Student] ([ClassId], [StudentId]) VALUES (4, N'HE222222')
INSERT [dbo].[Class_Student] ([ClassId], [StudentId]) VALUES (4, N'HE333333')
INSERT [dbo].[Class_Student] ([ClassId], [StudentId]) VALUES (5, N'HE111111')
INSERT [dbo].[Class_Student] ([ClassId], [StudentId]) VALUES (5, N'HE150544')
INSERT [dbo].[Class_Student] ([ClassId], [StudentId]) VALUES (5, N'HE153009')
INSERT [dbo].[Class_Student] ([ClassId], [StudentId]) VALUES (5, N'HE222222')
INSERT [dbo].[Class_Student] ([ClassId], [StudentId]) VALUES (5, N'HE333333')
INSERT [dbo].[Class_Student] ([ClassId], [StudentId]) VALUES (8, N'HE111111')
INSERT [dbo].[Class_Student] ([ClassId], [StudentId]) VALUES (8, N'HE150544')
INSERT [dbo].[Class_Student] ([ClassId], [StudentId]) VALUES (8, N'HE222222')
GO
SET IDENTITY_INSERT [dbo].[Account] ON 

INSERT [dbo].[Account] ([Id], [Email], [Password], [StudentId], [TeacherId], [Role]) VALUES (11, N'huy.com.hg@gmail.com', N'huyhg123', N'HE153009', NULL, 2)
INSERT [dbo].[Account] ([Id], [Email], [Password], [StudentId], [TeacherId], [Role]) VALUES (12, N'huycomvn01@gmail.com', N'huyhg123', NULL, 2, 1)
SET IDENTITY_INSERT [dbo].[Account] OFF
GO
