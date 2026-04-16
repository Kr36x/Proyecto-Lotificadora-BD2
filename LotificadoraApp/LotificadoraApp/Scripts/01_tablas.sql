use Grupo8;
go

-- ======================================
-- estado para las tablas que lo requieran
-- ======================================

create table Estado (
    id int identity(1,1) primary key,
    nombre varchar(100) not null,
    constraint uq_Estado_Nombre unique (nombre)
);
go



-- =========================
-- proyecto y ubicación
-- =========================

-- proyectos habitacionales principales
create table Proyecto (
    idProyecto int identity(1,1) primary key,
    nombreProyecto varchar(150) not null,
    descripcion varchar(255) null,
    fechaInicio date not null,
    fechaFinEstimada date null,
    areaTotalV2 decimal(18,2) not null check (areaTotalV2 > 0),
    maxAniosFinanciamiento int not null check (maxAniosFinanciamiento > 0),
    estadoId int not null,
    constraint fk_Proyecto_Estado
        foreign key (estadoId) references Estado(id)
);
go

-- ubicación legal y tributaria del proyecto
create table Departamento (
    id int primary key,
    codigo char(2) not null,
    nombre varchar(100) not null,
    constraint uq_Departamento_Codigo unique (codigo)
);
go

create table Municipio (
    id int primary key,
    codigo char(2) not null,
    departamentoId int not null,
    nombre varchar(100) not null,
    constraint fk_Municipio_Departamento
        foreign key (departamentoId) references Departamento(id)
);
go

create table UbicacionProyecto (
    idUbicacion int identity(1,1) primary key,
    idProyecto int not null,
    departamentoId int not null,
    municipioId int not null,
    aldeaColonia varchar(150) null,
    direccionDetalle varchar(255) null,
    claveCatastral varchar(100) null,
    observacionLegal varchar(255) null,
    constraint fk_UbicacionProyecto_Proyecto
        foreign key (idProyecto) references Proyecto(idProyecto),
    constraint fk_UbicacionProyecto_Departamento
        foreign key (departamentoId) references Departamento(id),
    constraint fk_UbicacionProyecto_Municipio
        foreign key (municipioId) references Municipio(id)
);
go

-- =========================
-- lotificación
-- =========================

-- etapas del proyecto (precio, área y tasa)
create table Etapa (
    idEtapa int identity(1,1) primary key,
    idProyecto int not null,
    nombreEtapa varchar(100) not null,
    fechaInicio date not null,
    fechaFinEstimada date null,
    areaTotalV2 decimal(18,2) not null check (areaTotalV2 > 0),
    porcentajeAreaVerde decimal(5,2) not null check (porcentajeAreaVerde >= 0 and porcentajeAreaVerde <= 100),
    porcentajeAreaComun decimal(5,2) not null check (porcentajeAreaComun >= 0 and porcentajeAreaComun <= 100),
    porcentajeAreaLotes decimal(5,2) not null check (porcentajeAreaLotes >= 0 and porcentajeAreaLotes <= 100),
    precioVaraCuadrada decimal(18,2) not null check (precioVaraCuadrada > 0),
    tasaInteresAnual decimal(5,2) not null check (tasaInteresAnual >= 0), -- interés para créditos de la etapa
    estadoId int not null,
    constraint fk_Etapa_Proyecto
        foreign key (idProyecto) references Proyecto(idProyecto),
    constraint fk_Etapa_Estado
        foreign key (estadoId) references Estado(id)
);
go

-- bloques dentro de cada etapa
create table Bloque (
    idBloque int identity(1,1) primary key,
    idEtapa int not null,
    nombreBloque varchar(50) not null,
    estadoId int not null,
    descripcion varchar(255) null,
    constraint fk_Bloque_Etapa
        foreign key (idEtapa) references Etapa(idEtapa),
    constraint fk_Bloque_Estado
        foreign key (estadoId) references Estado(id)
);
go

-- lotes disponibles para venta
create table Lote (
    idLote int identity(1,1) primary key,
    idBloque int not null,
    numeroLote varchar(20) not null,
    areaV2 decimal(18,2) not null check (areaV2 > 0),
    esEsquina bit not null default 0,
    cercaParque bit not null default 0,
    calleCerrada bit not null default 0,
    precioBase decimal(18,2) not null default 0 check (precioBase >= 0),
    recargoTotal decimal(18,2) not null default 0 check (recargoTotal >= 0),
    precioFinal decimal(18,2) not null default 0 check (precioFinal >= 0),
    estadoId int not null,
    constraint uq_Lote_Bloque_Numero unique (idBloque, numeroLote),
    constraint fk_Lote_Bloque
        foreign key (idBloque) references Bloque(idBloque),
    constraint fk_Lote_Estado
        foreign key (estadoId) references Estado(id)
);
go

-- catálogo de características que elevan el valor del lote
create table CaracteristicaLote (
    idCaracteristica int identity(1,1) primary key,
    nombreCaracteristica varchar(100) not null,
    tipoRecargo varchar(30) not null
        check (tipoRecargo in ('porcentaje','monto')),
    valorRecargo decimal(18,2) not null check (valorRecargo >= 0),
    estado varchar(20) not null default 'activo'
        check (estado in ('activo','inactivo'))
);
go

-- relación entre lotes y características
create table LoteCaracteristica (
    idLote int not null,
    idCaracteristica int not null,
    constraint pk_LoteCaracteristica primary key (idLote, idCaracteristica),
    constraint fk_LoteCaracteristica_Lote
        foreign key (idLote) references Lote(idLote),
    constraint fk_LoteCaracteristica_Caracteristica
        foreign key (idCaracteristica) references CaracteristicaLote(idCaracteristica)
);
go

-- =========================
-- clientes y soporte para ventas
-- =========================


create table EstadoCivil (
    id int primary key,
    descripcion varchar(100) not null
);
go


-- cliente comprador
create table Cliente (
    idCliente int identity(1,1) primary key,
    identidad varchar(20) not null,
    nombres varchar(100) not null,
    apellidos varchar(100) not null,
    fechaNacimiento date null,
    telefono varchar(20) null,
    correo varchar(120) null,
    direccion varchar(255) null,
    estadoCivilId int null,
    rtn varchar(20) null,
    estadoId int not null,
    constraint uq_Cliente_Identidad unique (identidad),
    constraint uq_Cliente_Rtn unique (rtn),
    constraint fk_Cliente_EstadoCivil
        foreign key (estadoCivilId) references EstadoCivil(id),
    constraint fk_Cliente_Estado
        foreign key (estadoId) references Estado(id)
);
go

-- datos laborales para evaluar capacidad de pago
create table DatosLaboralesCliente (
    idDatosLaborales int identity(1,1) primary key,
    idCliente int not null,
    empresa varchar(150) not null,
    cargo varchar(100) not null,
    ingresoMensual decimal(18,2) not null check (ingresoMensual >= 0),
    antiguedadLaboral int null check (antiguedadLaboral >= 0),
    telefonoTrabajo varchar(20) null,
    direccionTrabajo varchar(255) null,
    constraint uq_DatosLaborales_Cliente unique (idCliente),
    constraint fk_DatosLaborales_Cliente
        foreign key (idCliente) references Cliente(idCliente)
);
go

--parentesco para aval y beneficiario
create table Parentesco (
    id int primary key,
    descripcion varchar(100) not null
);
go

-- aval para compras a crédito
create table Aval (
    idAval int identity(1,1) primary key,
    identidad varchar(20) not null,
    nombres varchar(100) not null,
    apellidos varchar(100) not null,
    telefono varchar(20) null,
    direccion varchar(255) null,
    lugarTrabajo varchar(150) null,
    ingresoMensual decimal(18,2) null check (ingresoMensual >= 0),
    parentescoId int null,
    constraint uq_Aval_Identidad unique (identidad),
    constraint fk_Aval_Parentesco
        foreign key (parentescoId) references Parentesco(id)
);
go

-- beneficiario en caso de fallecimiento
create table Beneficiario (
    idBeneficiario int identity(1,1) primary key,
    identidad varchar(20) not null,
    nombres varchar(100) not null,
    apellidos varchar(100) not null,
    telefono varchar(20) null,
    parentescoId int null,
    direccion varchar(255) null,
    constraint uq_Beneficiario_Identidad unique (identidad),
    constraint fk_Beneficiario_Parentesco
        foreign key (parentescoId) references Parentesco(id)
);
go

-- =========================
-- ventas
-- =========================

-- cabecera general de la venta
create table Venta (
    idVenta int identity(1,1) primary key,
    idLote int not null,
    idCliente int not null,
    fechaVenta date not null,
    tipoVenta varchar(20) not null
        check (tipoVenta in ('contado','credito')),
    precioLote decimal(18,2) not null check (precioLote >= 0),
    descuento decimal(18,2) not null default 0 check (descuento >= 0),
    recargo decimal(18,2) not null default 0 check (recargo >= 0),
    totalVenta decimal(18,2) not null check (totalVenta >= 0),
    estadoId int not null,
    constraint fk_Venta_Lote
        foreign key (idLote) references Lote(idLote),
    constraint fk_Venta_Cliente
        foreign key (idCliente) references Cliente(idCliente),
    constraint fk_Venta_Estado
        foreign key (estadoId) references Estado(id)
);
go

-- detalle si la venta fue al contado
create table VentaContado (
    idVentaContado int identity(1,1) primary key,
    idVenta int not null unique,
    fechaPago date not null,
    montoPagado decimal(18,2) not null check (montoPagado >= 0),
    observacion varchar(255) null,
    constraint fk_VentaContado_Venta
        foreign key (idVenta) references Venta(idVenta)
);
go

-- detalle si la venta fue financiada
create table VentaCredito (
    idVentaCredito int identity(1,1) primary key,
    idVenta int not null unique,
    idAval int not null,
    idBeneficiario int not null,
    prima decimal(18,2) not null default 0 check (prima >= 0),
    financiaTotal bit not null default 0, -- 1 = financia todo, 0 = hay prima
    montoFinanciado decimal(18,2) not null check (montoFinanciado >= 0),
    plazoAnios int not null check (plazoAnios > 0),
    tasaInteresAnual decimal(5,2) not null check (tasaInteresAnual >= 0),
    fechaInicioPago date not null,
    estadoId int not null,
    constraint fk_VentaCredito_Venta
        foreign key (idVenta) references Venta(idVenta),
    constraint fk_VentaCredito_Aval
        foreign key (idAval) references Aval(idAval),
    constraint fk_VentaCredito_Beneficiario
        foreign key (idBeneficiario) references Beneficiario(idBeneficiario),
    constraint fk_VentaCredito_Estado
        foreign key (estadoId) references Estado(id)
);
go

-- =========================
-- plan de pagos
-- =========================

-- cabecera del financiamiento
create table PlanPago (
    idPlanPago int identity(1,1) primary key,
    idVentaCredito int not null unique,
    fechaGeneracion date not null,
    totalCapital decimal(18,2) not null check (totalCapital >= 0),
    totalInteres decimal(18,2) not null check (totalInteres >= 0),
    totalPlan decimal(18,2) not null check (totalPlan >= 0),
    cuotaMensualEstimada decimal(18,2) not null check (cuotaMensualEstimada >= 0),
    estadoId int not null,
    constraint fk_PlanPago_VentaCredito
        foreign key (idVentaCredito) references VentaCredito(idVentaCredito),
    constraint fk_PlanPago_Estado
        foreign key (estadoId) references Estado(id)
);
go

-- cuotas mensuales del plan
create table Cuota (
    idCuota int identity(1,1) primary key,
    idPlanPago int not null,
    numeroCuota int not null check (numeroCuota > 0),
    fechaVencimiento date not null,
    saldoInicial decimal(18,2) not null check (saldoInicial >= 0),
    capitalProgramado decimal(18,2) not null check (capitalProgramado >= 0),
    interesProgramado decimal(18,2) not null check (interesProgramado >= 0),
    montoCuota decimal(18,2) not null check (montoCuota >= 0),
    saldoFinal decimal(18,2) not null check (saldoFinal >= 0),
    estadoId int not null,
    constraint uq_Cuota_Plan_Numero unique (idPlanPago, numeroCuota),
    constraint fk_Cuota_PlanPago
        foreign key (idPlanPago) references PlanPago(idPlanPago),
    constraint fk_Cuota_Estado
        foreign key (estadoId) references Estado(id)
);
go

-- =========================
-- bancos y cuentas
-- =========================

-- catálogo de bancos
create table Banco (
    idBanco int identity(1,1) primary key,
    nombreBanco varchar(100) not null,
    estadoId int not null,
    constraint uq_Banco_Nombre unique (nombreBanco),
    constraint fk_Banco_Estado
        foreign key (estadoId) references Estado(id)
);
go

-- cuenta asignada a una etapa
create table CuentaBancaria (
    idCuentaBancaria int identity(1,1) primary key,
    idBanco int not null,
    idEtapa int not null,
    numeroCuenta varchar(50) not null,
    tipoCuenta varchar(30) not null,
    saldoActual decimal(18,2) not null default 0 check (saldoActual >= 0),
    estadoId int not null,
    constraint uq_CuentaBancaria_Numero unique (numeroCuenta),
    constraint fk_CuentaBancaria_Banco
        foreign key (idBanco) references Banco(idBanco),
    constraint fk_CuentaBancaria_Etapa
        foreign key (idEtapa) references Etapa(idEtapa),
    constraint fk_CuentaBancaria_Estado
        foreign key (estadoId) references Estado(id)
);
go

-- =========================
-- pagos y facturación
-- =========================

-- pago realizado por el cliente
create table Pago (
    idPago int identity(1,1) primary key,
    idVenta int not null,
    fechaPago datetime not null default getdate(),
    formaPago varchar(20) not null
        check (formaPago in ('efectivo','deposito')),
    montoTotal decimal(18,2) not null check (montoTotal > 0),
    idCuentaBancaria int null, -- se usa cuando fue depósito
    numeroReferencia varchar(100) null,
    depositadoCaja bit not null default 0, -- solo aplica a pagos en efectivo
    observacion varchar(255) null,
    constraint fk_Pago_Venta
        foreign key (idVenta) references Venta(idVenta),
    constraint fk_Pago_CuentaBancaria
        foreign key (idCuentaBancaria) references CuentaBancaria(idCuentaBancaria)
);
go

-- desglose del pago aplicado a cuotas
create table DetallePagoCuota (
    idDetallePagoCuota int identity(1,1) primary key,
    idPago int not null,
    idCuota int not null,
    montoCapital decimal(18,2) not null default 0 check (montoCapital >= 0),
    montoInteres decimal(18,2) not null default 0 check (montoInteres >= 0),
    montoAplicado decimal(18,2) not null check (montoAplicado > 0),
    constraint fk_DetallePagoCuota_Pago
        foreign key (idPago) references Pago(idPago),
    constraint fk_DetallePagoCuota_Cuota
        foreign key (idCuota) references Cuota(idCuota)
);
go

-- factura generada por cada pago
create table Factura (
    idFactura int identity(1,1) primary key,
    idPago int not null unique,
    numeroFactura varchar(50) not null,
    fechaFactura datetime not null default getdate(),
    nombreCliente varchar(200) not null,
    rtnCliente varchar(20) null,
    totalFactura decimal(18,2) not null check (totalFactura >= 0),
    constraint uq_Factura_Numero unique (numeroFactura),
    constraint fk_Factura_Pago
        foreign key (idPago) references Pago(idPago)
);
go

-- detalle de capital e interés facturado
create table DetalleFactura (
    idDetalleFactura int identity(1,1) primary key,
    idFactura int not null,
    descripcion varchar(255) not null,
    capital decimal(18,2) not null default 0 check (capital >= 0),
    interes decimal(18,2) not null default 0 check (interes >= 0),
    subtotal decimal(18,2) not null check (subtotal >= 0),
    constraint fk_DetalleFactura_Factura
        foreign key (idFactura) references Factura(idFactura)
);
go

-- =========================
-- depósitos de caja
-- =========================

-- depósito agrupado de pagos recibidos en caja
create table DepositoCajaBanco (
    idDepositoCaja int identity(1,1) primary key,
    fechaDeposito datetime not null default getdate(),
    idCuentaBancaria int not null,
    totalDepositado decimal(18,2) not null check (totalDepositado > 0),
    observacion varchar(255) null,
    constraint fk_DepositoCajaBanco_CuentaBancaria
        foreign key (idCuentaBancaria) references CuentaBancaria(idCuentaBancaria)
);
go

-- detalle de los pagos incluidos en el depósito
create table DetalleDepositoCaja (
    idDetalleDepositoCaja int identity(1,1) primary key,
    idDepositoCaja int not null,
    idPago int not null,
    monto decimal(18,2) not null check (monto > 0),
    constraint fk_DetalleDepositoCaja_Deposito
        foreign key (idDepositoCaja) references DepositoCajaBanco(idDepositoCaja),
    constraint fk_DetalleDepositoCaja_Pago
        foreign key (idPago) references Pago(idPago)
);
go

-- =========================
-- gastos
-- =========================

-- tipos de gasto del proyecto
create table TipoGasto (
    idTipoGasto int identity(1,1) primary key,
    nombreTipoGasto varchar(100) not null,
    estadoId int not null,
    constraint uq_TipoGasto_Nombre unique (nombreTipoGasto),
    constraint fk_TipoGasto_Estado
        foreign key (estadoId) references Estado(id)
);
go

-- gastos cubiertos desde cuentas del proyecto
create table GastoProyecto (
    idGasto int identity(1,1) primary key,
    idProyecto int not null,
    idEtapa int not null,
    idTipoGasto int not null,
    idCuentaBancaria int not null,
    fechaGasto datetime not null default getdate(),
    descripcion varchar(255) not null,
    monto decimal(18,2) not null check (monto > 0),
    estadoId int not null,
    constraint fk_GastoProyecto_Proyecto
        foreign key (idProyecto) references Proyecto(idProyecto),
    constraint fk_GastoProyecto_Etapa
        foreign key (idEtapa) references Etapa(idEtapa),
    constraint fk_GastoProyecto_TipoGasto
        foreign key (idTipoGasto) references TipoGasto(idTipoGasto),
    constraint fk_GastoProyecto_CuentaBancaria
        foreign key (idCuentaBancaria) references CuentaBancaria(idCuentaBancaria),
    constraint fk_GastoProyecto_Estado
        foreign key (estadoId) references Estado(id)
);
go

-- =========================
-- empleados
-- =========================

create table Empleado (
    id int identity(1,1) primary key,
    nombres varchar(100) not null,
    apellidos varchar(100) not null,
    identidad varchar(14) not null,
    fechaNacimiento date not null,
    telefono varchar(20) null,
    sexoId char(1) not null,
    fechaIngreso datetime not null,
    fechaEgreso datetime null,
    salario decimal(18,2) not null,
    estadoId int not null,
    constraint fk_Empleado_Estado
        foreign key (estadoId) references Estado(id)
);
go

