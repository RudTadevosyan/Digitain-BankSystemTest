create schema core;
go

create table core.Account
(
	AccountId INT IDENTITY(1,1) Primary Key,
	FullName Nvarchar(200) Not null,
	Email Nvarchar(100) Not null,
	Balance Decimal(10,2) Not null,
	
	Constraint UQ_Account_Email unique (Email)
);

create table core.Operation
(
	OperationId INT IDENTITY(1,1) Primary Key,
	AccountId INT Not null

	Constraint FK_Operation_Account Foreign Key (AccountId)
	references core.Account(AccountId),

	Amount Decimal(10,2) Not null
	Constraint CK_Operation_Amount Check (Amount > 0),

	OperationType Nvarchar(50) not null,
	CreatedAt DATETIME2(3) Not null
	Constraint DF_Operation_Created Default SYSUTCDATETIME(),
);

create nonclustered index IX_Operation_AccountId on core.Operation(AccountId);