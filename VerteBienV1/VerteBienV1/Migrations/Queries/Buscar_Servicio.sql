create procedure [dbo].[Buscar_Servicio] (@dato varchar(100), @sector varchar(100))
as
begin

Declare @id_servicio int
Declare @id_usuario nvarchar(128)
Declare @id_categoria int
Declare @nombre_servicio varchar(100)
Declare @descripcion varchar(max)
Declare @precio_servicio decimal(12,2)
Declare @tiempo decimal(12,2)
Declare @imagenes varchar(max)
Declare @estado varchar(10)

	-- Tabla para almacenar los id de los servicio que se mostraran
	 CREATE TABLE #id_servicios (
	 id int identity primary key,
	 id_servicio int
	 )
	 -- Tabla para almacenar los id de los servicio que se mostraran


	-- Servicios de las peluqierias premium
	DECLARE cursor_servicio_premium CURSOR
	FOR select s.id_servicio 
	from servicios s 
	inner join AspNetUsers u on s.id_usuario = u.Id
	inner join AspNetUserRoles r on s.id_usuario = r.UserId
	where (s.nombre_servicio LIKE '%'+@dato+'%' or s.descripcion LIKE '%'+@dato+'%') 
	and (s.estado = 'activo' and u.sector = @sector and r.RoleId = 5)

	OPEN cursor_servicio_premium;

	FETCH NEXT FROM cursor_servicio_premium INTO @id_servicio
	WHILE @@FETCH_STATUS = 0
	BEGIN
		insert into #id_servicios(id_servicio) values(@id_servicio)
		FETCH NEXT FROM cursor_servicio_premium  INTO @id_servicio
	END;

	CLOSE cursor_servicio_premium ;
	DEALLOCATE cursor_servicio_premium;
	-- Servicios de las peluqierias premium


	-- Demas servicios con match
	DECLARE cursor_servicio CURSOR
	FOR select s.id_servicio 
	from servicios s inner join AspNetUsers u on s.id_usuario = u.Id
	where (s.nombre_servicio LIKE '%'+@dato+'%' or s.descripcion LIKE '%'+@dato+'%') and (s.estado = 'activo' and u.sector = @sector)

	OPEN cursor_servicio;

	FETCH NEXT FROM cursor_servicio INTO @id_servicio
	WHILE @@FETCH_STATUS = 0
	BEGIN
		insert into #id_servicios(id_servicio) values(@id_servicio)
		FETCH NEXT FROM cursor_servicio INTO @id_servicio
	END;

	CLOSE cursor_servicio;
	DEALLOCATE cursor_servicio;
	-- Demas servicios con match


	-- Tabla final de los servicios a mostrar
	CREATE TABLE #servicios (
	 id_servicio int,
	 id_usuario nvarchar(128),
	 id_categoria int,
	 nombre_servicio varchar(100),
	 descripcion varchar(max),
	 precio_servicio decimal(12,2),
	 tiempo decimal(12,2),
	 imagenes varchar(max),
	 estado varchar(10)
	 )
	-- Tabla final de los servicios a mostrar


	-- Cursor para llenar tabla de servicios con los servicio de los id que se encuentran en #id_servicios
	DECLARE cursor_servicio_final CURSOR
	FOR select id_servicio, id_usuario, id_categoria, nombre_servicio, descripcion, precio_servicio, tiempo, imagenes, estado
	  from #id_servicios 

	OPEN cursor_servicio_final;

	FETCH NEXT FROM cursor_servicio_final INTO @id_servicio, @id_usuario, @id_categoria, @nombre_servicio, @descripcion, @precio_servicio, @tiempo, @imagenes, @estado
	WHILE @@FETCH_STATUS = 0
	BEGIN
		insert into #servicios(id_servicio, id_usuario, id_categoria, nombre_servicio, descripcion, precio_servicio, tiempo, imagenes, estado)
		values(@id_servicio, @id_usuario, @id_categoria, @nombre_servicio, @descripcion, @precio_servicio, @tiempo, @imagenes, @estado)

		FETCH NEXT FROM cursor_servicio_final INTO @id_servicio, @id_usuario, @id_categoria, @nombre_servicio, @descripcion, @precio_servicio, @tiempo, @imagenes, @estado
	END;

	CLOSE cursor_servicio_final;
	DEALLOCATE cursor_servicio_final;
	-- Cursor para llenar tabla de servicios con los servicio de los id que se encuentran en #id_servicios


	Select * from #servicios;

end