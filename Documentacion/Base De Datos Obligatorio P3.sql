CREATE DATABASE PrestamosExpress;
USE PrestamosExpress;
CREATE TABLE Usuario(UsuarioId INT IDENTITY (1,1),
					 Cedula CHARACTER (8) NOT NULL,
					 Nombre VARCHAR(30) NOT NULL,
					 Apellido VARCHAR(30) NOT NULL,
					 Pass VARCHAR(30) NOT NULL,
					 Fecha_Nacimiento DATETIME NOT NULL,
					 Celular CHARACTER(9) NOT NULL,
					 Email VARCHAR(100) NOT NULL,
					 Rol VARCHAR(13) NOT NULL,
					 CONSTRAINT PK_UsuarioId PRIMARY KEY (UsuarioId),
					 CONSTRAINT UK_Cedula UNIQUE (Cedula),
					 CONSTRAINT UK_Celular UNIQUE (Celular),
					 CONSTRAINT UK_Email UNIQUE (Email),
					 CONSTRAINT CK_Rol CHECK (Rol IN ('Administrador','Solicitante'))
					 );
CREATE INDEX IDX_usuario_Cedula ON Usuario(Cedula);
CREATE INDEX IDX_usuario_Celular ON Usuario(Celular);
CREATE INDEX IDX_usuario_Email ON Usuario(Email);

CREATE TABLE Proyecto(ProyectoId INT IDENTITY(1,1),
					  Titulo VARCHAR(50) NOT NULL,
	                  Descripcion VARCHAR(150) NOT NULL,
					  Imagen VARCHAR (50) NOT NULL, 
					  Monto_Solicitado NUMERIC (20,2) NOT NULL,
					  Cuotas INT NOT NULL,
					  Tasa_Interes NUMERIC(6,2) NOT NULL,
					  Estado VARCHAR(150) NOT NULL,
					  Experiencia VARCHAR(150) NOT NULL,
					  Integrantes INT NOT NULL,
					  Solicitante INT NOT NULL,
					  Tipo VARCHAR (11) NOT NULL,
					  Fecha_Creacion DATE,
					  CONSTRAINT PK_ProyectoId PRIMARY KEY (ProyectoId),
					  CONSTRAINT FK_Proyecto_Solicitante FOREIGN KEY (Solicitante) REFERENCES Usuario(UsuarioId),
					  CONSTRAINT UK_Titulo UNIQUE (Titulo),
					  CONSTRAINT CK_Estado CHECK (Estado IN ('Pendiente de revisión','Aprobado', 'Rechazado')),
					  CONSTRAINT CK_Tipo CHECK (Tipo IN ('Personal','Cooperativo'))
					  );
CREATE INDEX IDX_Proyecto_Solicitante ON Proyecto(Solicitante);
CREATE INDEX IDX_Proyecto_Titulo ON Proyecto(Titulo);

CREATE TABLE Aprobacion(AprobacionId INT IDENTITY(1,1),
						ProyectoId INT NOT NULL,
						Puntaje INT,
						Administrador CHARACTER (8),
						Fecha_Aprobacion DATE,
						CONSTRAINT PK_IdAprobacion PRIMARY KEY (AprobacionId),
						CONSTRAINT FK_Aprobacion_ProyectoId FOREIGN KEY (ProyectoId) REFERENCES Proyecto(ProyectoId),
						CONSTRAINT CK_Puntaje CHECK (Puntaje >= 1 AND Puntaje <= 10)
					    );

CREATE TABLE Tasa_Interes(Desde NUMERIC(2) NOT NULL,
						  Hasta NUMERIC(2) NOT NULL,
						  Interes NUMERIC(2) NOT NULL
						  );

CREATE TABLE Financiamiento(Monto_Max NUMERIC(5) NOT NULL,
							Monto_Min NUMERIC(5) NOT NULL
							);

INSERT INTO Financiamiento (Monto_Max, Monto_Min) VALUES (90000, 5000);
INSERT INTO Tasa_Interes (Desde, Hasta, Interes) VALUES (10, 20, 5);
INSERT INTO Tasa_Interes (Desde, Hasta, Interes) VALUES (21, 30, 8);
INSERT INTO Tasa_Interes (Desde, Hasta, Interes) VALUES (31, 45, 12);

INSERT INTO Usuario(Cedula, Nombre, Apellido, Pass, Fecha_Nacimiento, Celular, Email, Rol) VALUES(11111111, 'Fede', 'Porta', 'Admin123', GETDATE(), '090000000', '-','Administrador');
INSERT INTO Usuario(Cedula, Nombre, Apellido, Pass, Fecha_Nacimiento, Celular, Email, Rol) VALUES(22222222, 'Mati', 'Poletti', 'Admin456', GETDATE(), '090000001', '--','Administrador');

INSERT INTO Usuario(Cedula, Nombre, Apellido, Pass, Fecha_Nacimiento, Celular, Email, Rol) VALUES(33333333, 'Fernando', 'Ugarte', 'Usu123', '1987-01-26', '095000000', 'fernando@mail.com','Solicitante');
INSERT INTO Usuario(Cedula, Nombre, Apellido, Pass, Fecha_Nacimiento, Celular, Email, Rol) VALUES(44444444, 'Marta', 'Fernandez', 'Usu456', '1998-10-06', '096123123', 'Marta@mail.com','Solicitante');
INSERT INTO Usuario(Cedula, Nombre, Apellido, Pass, Fecha_Nacimiento, Celular, Email, Rol) VALUES(55555555, 'Juan', 'Nojo', 'Usu789', '1990-02-20', '098789450', 'Juan@mail.com','Solicitante');
INSERT INTO Usuario(Cedula, Nombre, Apellido, Pass, Fecha_Nacimiento, Celular, Email, Rol) VALUES(66666666, 'Esteban', 'Quito', 'Usu159', '1994-04-15', '095120050', 'EstebanQuito@mail.com','Solicitante');
INSERT INTO Usuario(Cedula, Nombre, Apellido, Pass, Fecha_Nacimiento, Celular, Email, Rol) VALUES(77777777, 'Pablo', 'Perez', 'Usu753', '1965-09-21', '095128070', 'Pablo@mail.com','Solicitante');
INSERT INTO Usuario(Cedula, Nombre, Apellido, Pass, Fecha_Nacimiento, Celular, Email, Rol) VALUES(88888888, 'Maria', 'Urioste', 'Usu741', '1978-12-14', '098146050', 'Maria12565@mail.com','Solicitante');
INSERT INTO Usuario(Cedula, Nombre, Apellido, Pass, Fecha_Nacimiento, Celular, Email, Rol) VALUES(99999999, 'Juanita', 'Gonzalez', 'Usu852', '1988-11-09', '096120050', 'JuanaG@mail.com','Solicitante');
INSERT INTO Usuario(Cedula, Nombre, Apellido, Pass, Fecha_Nacimiento, Celular, Email, Rol) VALUES(12312345, 'Ana', 'Rodriguez', 'Usu963', '1999-08-06', '098120560', 'Ana99@mail.com','Solicitante');
INSERT INTO Usuario(Cedula, Nombre, Apellido, Pass, Fecha_Nacimiento, Celular, Email, Rol) VALUES(45645645, 'Ricardo', 'Darin', 'Usu312', '1960-09-12', '095123250', 'Riqui@mail.com','Solicitante');
INSERT INTO Usuario(Cedula, Nombre, Apellido, Pass, Fecha_Nacimiento, Celular, Email, Rol) VALUES(78978978, 'Washington', 'Oscar', 'Usu654', '1913-05-24', '095123650', 'elBasington@mail.com','Solicitante');

INSERT INTO Proyecto(Titulo, Descripcion, Imagen, Monto_Solicitado, Cuotas, Tasa_Interes, Estado, Experiencia, Integrantes, Solicitante, Tipo, Fecha_Creacion) VALUES('Reparacion a Domicilio', 'Reparacion electrodomesticos', 'Reparacion a Domicilio', 50000, 10, 5, 'Pendiente de revisión', 'Albañil 15 años de experiencia', 1, 4, 'Personal', '2019-03-25');
INSERT INTO Proyecto(Titulo, Descripcion, Imagen, Monto_Solicitado, Cuotas, Tasa_Interes, Estado, Experiencia, Integrantes, Solicitante, Tipo, Fecha_Creacion) VALUES('Reparacion de PC', 'Reparacion de PC a domicilio', 'Reparacion de PC', 25000, 15, 5, 'Pendiente de revisión', 'Taller de Reparacion PC', 1, 11, 'Personal', '2018-07-12');
INSERT INTO Proyecto(Titulo, Descripcion, Imagen, Monto_Solicitado, Cuotas, Tasa_Interes, Estado, Experiencia, Integrantes, Solicitante, Tipo, Fecha_Creacion) VALUES('Repartidora', 'Empresa de fletes', 'Repartidora', 45000, 25, 8, 'Pendiente de revisión', 'Empresa transportista Fletex S.A.', 1, 8, 'Personal', '2020-02-03');
INSERT INTO Proyecto(Titulo, Descripcion, Imagen, Monto_Solicitado, Cuotas, Tasa_Interes, Estado, Experiencia, Integrantes, Solicitante, Tipo, Fecha_Creacion) VALUES('Inversion inmobiliaria', 'Propiedad horizontal para alquileres', 'Inversion inmobiliaria', 90000, 40, 12, 'Pendiente de revisión', '', 6, 9, 'Cooperativo', '2019-06-15');
INSERT INTO Proyecto(Titulo, Descripcion, Imagen, Monto_Solicitado, Cuotas, Tasa_Interes, Estado, Experiencia, Integrantes, Solicitante, Tipo, Fecha_Creacion) VALUES('Salon de Fiestas', 'Renovación de Salon el Tucán', 'Salon de Fiestas', 85000, 36, 12, 'Pendiente de revisión', 'Empresario emprendedor con experiencia en eventos', 1, 3, 'Personal', '2020-04-12');
INSERT INTO Proyecto(Titulo, Descripcion, Imagen, Monto_Solicitado, Cuotas, Tasa_Interes, Estado, Experiencia, Integrantes, Solicitante, Tipo, Fecha_Creacion) VALUES('Rotiseria', 'Rotisería barrio el Cordón', 'Rotiseria', 15000, 10, 5, 'Pendiente de revisión', 'Experiencia en panadería de barrio 15 años', 1, 7, 'Personal', '2020-06-12');
INSERT INTO Proyecto(Titulo, Descripcion, Imagen, Monto_Solicitado, Cuotas, Tasa_Interes, Estado, Experiencia, Integrantes, Solicitante, Tipo, Fecha_Creacion) VALUES('Banda Tropical', 'Conjunto musical los Turlurones', 'Banda Tropical', 75000, 20, 5, 'Pendiente de revisión', 'Empresario emprendedor con experiencia en eventos', 1, 3, 'Personal', '2020-05-14');
INSERT INTO Proyecto(Titulo, Descripcion, Imagen, Monto_Solicitado, Cuotas, Tasa_Interes, Estado, Experiencia, Integrantes, Solicitante, Tipo, Fecha_Creacion) VALUES('Funeraria', 'Arreglos cosméticos a sala principal', 'Funeraria', 20000, 15, 5, 'Pendiente de revisión', 'Empresa familiar tercera generación', 1, 10, 'Personal', '2018-06-22');
INSERT INTO Proyecto(Titulo, Descripcion, Imagen, Monto_Solicitado, Cuotas, Tasa_Interes, Estado, Experiencia, Integrantes, Solicitante, Tipo, Fecha_Creacion) VALUES('Barraca', 'Expansión de planta', 'Barraca', 90000, 45, 12, 'Pendiente de revisión', '', 5, 5, 'Cooperativo', '2019-09-18');
INSERT INTO Proyecto(Titulo, Descripcion, Imagen, Monto_Solicitado, Cuotas, Tasa_Interes, Estado, Experiencia, Integrantes, Solicitante, Tipo, Fecha_Creacion) VALUES('Emprendimiento IT', 'Startup IT', 'Emprendimiento IT', 35000, 45, 12, 'Pendiente de revisión', '', 3, 6, 'Cooperativo', '2019-10-22');

