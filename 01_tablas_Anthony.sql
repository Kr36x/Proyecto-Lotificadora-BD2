use Grupo8;
go

-- =========================
-- proyecto y ubicación
-- =========================

-- proyectos habitacionales principales
create table Proyecto (
    idProyecto int identity(1,1) primary key,
    nombreProyecto nvarchar(150) not null,
    descripcion nvarchar(255) null,
    fechaInicio date not null,
    fechaFinEstimada date null,
    areaTotalV2 decimal(18,2) not null check (areaTotalV2 > 0),
    maxAniosFinanciamiento int not null check (maxAniosFinanciamiento > 0),
    estado nvarchar(30) not null default 'activo'
        check (estado in ('activo','inactivo','finalizado'))
);
go

-- ubicación legal y tributaria del proyecto
create table UbicacionProyecto (
    idUbicacion int identity(1,1) primary key,
    idProyecto int not null,
    departamento nvarchar(100) not null,
    municipio nvarchar(100) not null,
    aldeaColonia nvarchar(150) null,
    direccionDetalle nvarchar(255) null,
    claveCatastral nvarchar(100) null,
    observacionLegal nvarchar(255) null,
    constraint fk_UbicacionProyecto_Proyecto
        foreign key (idProyecto) references Proyecto(idProyecto)
);
go

-- =========================
-- lotificación
-- =========================

-- etapas del proyecto (precio, área y tasa)
create table Etapa (
    idEtapa int identity(1,1) primary key,
    idProyecto int not null,
    nombreEtapa nvarchar(100) not null,
    fechaInicio date not null,
    fechaFinEstimada date null,
    areaTotalV2 decimal(18,2) not null check (areaTotalV2 > 0),
    porcentajeAreaVerde decimal(5,2) not null check (porcentajeAreaVerde >= 0 and porcentajeAreaVerde <= 100),
    porcentajeAreaComun decimal(5,2) not null check (porcentajeAreaComun >= 0 and porcentajeAreaComun <= 100),
    porcentajeAreaLotes decimal(5,2) not null check (porcentajeAreaLotes >= 0 and porcentajeAreaLotes <= 100),
    precioVaraCuadrada decimal(18,2) not null check (precioVaraCuadrada > 0),
    tasaInteresAnual decimal(5,2) not null check (tasaInteresAnual >= 0), -- interés para créditos de la etapa
    estado nvarchar(30) not null default 'activa'
        check (estado in ('activa','inactiva','finalizada')),
    constraint fk_Etapa_Proyecto
        foreign key (idProyecto) references Proyecto(idProyecto)
);
go

-- bloques dentro de cada etapa
create table Bloque (
    idBloque int identity(1,1) primary key,
    idEtapa int not null,
    nombreBloque nvarchar(50) not null,
    descripcion nvarchar(255) null,
    constraint fk_Bloque_Etapa
        foreign key (idEtapa) references Etapa(idEtapa)
);
go

-- lotes disponibles para venta
create table Lote (
    idLote int identity(1,1) primary key,
    idBloque int not null,
    numeroLote nvarchar(20) not null,
    areaV2 decimal(18,2) not null check (areaV2 > 0),
    esEsquina bit not null default 0,
    cercaParque bit not null default 0,
    calleCerrada bit not null default 0,
    precioBase decimal(18,2) not null default 0 check (precioBase >= 0),
    recargoTotal decimal(18,2) not null default 0 check (recargoTotal >= 0),
    precioFinal decimal(18,2) not null default 0 check (precioFinal >= 0),
    estadoLote nvarchar(30) not null default 'disponible'
        check (estadoLote in ('disponible','reservado','vendido','inactivo')),
    constraint uq_Lote_Bloque_Numero unique (idBloque, numeroLote),
    constraint fk_Lote_Bloque
        foreign key (idBloque) references Bloque(idBloque)
);
go

-- catálogo de características que elevan el valor del lote
create table CaracteristicaLote (
    idCaracteristica int identity(1,1) primary key,
    nombreCaracteristica nvarchar(100) not null,
    tipoRecargo nvarchar(30) not null
        check (tipoRecargo in ('porcentaje','monto')),
    valorRecargo decimal(18,2) not null check (valorRecargo >= 0),
    estado nvarchar(20) not null default 'activo'
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

-- cliente comprador
create table Cliente (
    idCliente int identity(1,1) primary key,
    identidad nvarchar(20) not null,
    nombres nvarchar(100) not null,
    apellidos nvarchar(100) not null,
    fechaNacimiento date null,
    telefono nvarchar(20) null,
    correo nvarchar(120) null,
    direccion nvarchar(255) null,
    estadoCivil nvarchar(30) null,
    rtn nvarchar(20) null,
    estado nvarchar(20) not null default 'activo'
        check (estado in ('activo','inactivo')),
    constraint uq_Cliente_Identidad unique (identidad),
    constraint uq_Cliente_Rtn unique (rtn)
);
go

-- datos laborales para evaluar capacidad de pago
create table DatosLaboralesCliente (
    idDatosLaborales int identity(1,1) primary key,
    idCliente int not null,
    empresa nvarchar(150) not null,
    cargo nvarchar(100) not null,
    ingresoMensual decimal(18,2) not null check (ingresoMensual >= 0),
    antiguedadLaboral int null check (antiguedadLaboral >= 0),
    telefonoTrabajo nvarchar(20) null,
    direccionTrabajo nvarchar(255) null,
    constraint uq_DatosLaborales_Cliente unique (idCliente),
    constraint fk_DatosLaborales_Cliente
        foreign key (idCliente) references Cliente(idCliente)
);
go

-- aval para compras a crédito
create table Aval (
    idAval int identity(1,1) primary key,
    identidad nvarchar(20) not null,
    nombres nvarchar(100) not null,
    apellidos nvarchar(100) not null,
    telefono nvarchar(20) null,
    direccion nvarchar(255) null,
    lugarTrabajo nvarchar(150) null,
    ingresoMensual decimal(18,2) null check (ingresoMensual >= 0),
    parentescoCliente nvarchar(50) null,
    constraint uq_Aval_Identidad unique (identidad)
);
go

-- beneficiario en caso de fallecimiento
create table Beneficiario (
    idBeneficiario int identity(1,1) primary key,
    identidad nvarchar(20) not null,
    nombres nvarchar(100) not null,
    apellidos nvarchar(100) not null,
    telefono nvarchar(20) null,
    parentesco nvarchar(50) null,
    direccion nvarchar(255) null,
    constraint uq_Beneficiario_Identidad unique (identidad)
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
    tipoVenta nvarchar(20) not null
        check (tipoVenta in ('contado','credito')),
    precioLote decimal(18,2) not null check (precioLote >= 0),
    descuento decimal(18,2) not null default 0 check (descuento >= 0),
    recargo decimal(18,2) not null default 0 check (recargo >= 0),
    totalVenta decimal(18,2) not null check (totalVenta >= 0),
    estadoVenta nvarchar(30) not null default 'activa'
        check (estadoVenta in ('activa','anulada','finalizada')),
    constraint fk_Venta_Lote
        foreign key (idLote) references Lote(idLote),
    constraint fk_Venta_Cliente
        foreign key (idCliente) references Cliente(idCliente)
);
go

-- detalle si la venta fue al contado
create table VentaContado (
    idVentaContado int identity(1,1) primary key,
    idVenta int not null unique,
    fechaPago date not null,
    montoPagado decimal(18,2) not null check (montoPagado >= 0),
    observacion nvarchar(255) null,
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
    estadoCredito nvarchar(30) not null default 'activo'
        check (estadoCredito in ('activo','cancelado','moroso','finalizado')),
    constraint fk_VentaCredito_Venta
        foreign key (idVenta) references Venta(idVenta),
    constraint fk_VentaCredito_Aval
        foreign key (idAval) references Aval(idAval),
    constraint fk_VentaCredito_Beneficiario
        foreign key (idBeneficiario) references Beneficiario(idBeneficiario)
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
    estado nvarchar(30) not null default 'activo'
        check (estado in ('activo','cancelado','finalizado')),
    constraint fk_PlanPago_VentaCredito
        foreign key (idVentaCredito) references VentaCredito(idVentaCredito)
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
    estadoCuota nvarchar(30) not null default 'pendiente'
        check (estadoCuota in ('pendiente','parcial','pagada','vencida')),
    constraint uq_Cuota_Plan_Numero unique (idPlanPago, numeroCuota),
    constraint fk_Cuota_PlanPago
        foreign key (idPlanPago) references PlanPago(idPlanPago)
);
go

-- =========================
-- bancos y cuentas
-- =========================

-- catálogo de bancos
create table Banco (
    idBanco int identity(1,1) primary key,
    nombreBanco nvarchar(100) not null,
    estado nvarchar(20) not null default 'activo'
        check (estado in ('activo','inactivo')),
    constraint uq_Banco_Nombre unique (nombreBanco)
);
go

-- cuenta asignada a una etapa
create table CuentaBancaria (
    idCuentaBancaria int identity(1,1) primary key,
    idBanco int not null,
    idEtapa int not null,
    numeroCuenta nvarchar(50) not null,
    tipoCuenta nvarchar(30) not null,
    saldoActual decimal(18,2) not null default 0 check (saldoActual >= 0),
    estado nvarchar(20) not null default 'activa'
        check (estado in ('activa','inactiva')),
    constraint uq_CuentaBancaria_Numero unique (numeroCuenta),
    constraint fk_CuentaBancaria_Banco
        foreign key (idBanco) references Banco(idBanco),
    constraint fk_CuentaBancaria_Etapa
        foreign key (idEtapa) references Etapa(idEtapa)
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
    formaPago nvarchar(20) not null
        check (formaPago in ('efectivo','deposito')),
    montoTotal decimal(18,2) not null check (montoTotal > 0),
    idCuentaBancaria int null, -- se usa cuando fue depósito
    numeroReferencia nvarchar(100) null,
    depositadoCaja bit not null default 0, -- solo aplica a pagos en efectivo
    observacion nvarchar(255) null,
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
    numeroFactura nvarchar(50) not null,
    fechaFactura datetime not null default getdate(),
    nombreCliente nvarchar(200) not null,
    rtnCliente nvarchar(20) null,
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
    descripcion nvarchar(255) not null,
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
    observacion nvarchar(255) null,
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
    nombreTipoGasto nvarchar(100) not null,
    estado nvarchar(20) not null default 'activo'
        check (estado in ('activo','inactivo')),
    constraint uq_TipoGasto_Nombre unique (nombreTipoGasto)
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
    descripcion nvarchar(255) not null,
    monto decimal(18,2) not null check (monto > 0),
    estado nvarchar(20) not null default 'activo'
        check (estado in ('activo','anulado')),
    constraint fk_GastoProyecto_Proyecto
        foreign key (idProyecto) references Proyecto(idProyecto),
    constraint fk_GastoProyecto_Etapa
        foreign key (idEtapa) references Etapa(idEtapa),
    constraint fk_GastoProyecto_TipoGasto
        foreign key (idTipoGasto) references TipoGasto(idTipoGasto),
    constraint fk_GastoProyecto_CuentaBancaria
        foreign key (idCuentaBancaria) references CuentaBancaria(idCuentaBancaria)
);
go