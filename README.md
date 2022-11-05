# JWTAPI
Simple WebApi with JWT token and simple database

Know How:

1. Zaciągnij do siebie (git clone https://github.com/DSanak/JWTAPI.git)
2. Odpal w VS (kliknij w JWTAPI.sln)
3. Wybierz SQL Server Object explorer
   - Powinieneś zobaczyć coś takiego ![image](https://user-images.githubusercontent.com/82310098/200123825-9b24111f-f07b-42c7-8bb5-acf028e9ce33.png)
4. Dodaj nową baze danych ( nazwij ją Users)
5. Na bazie:
   - Prawy przycisk myszki -> New Query
   Wykonaj te zapytania:
   
   5.1 
      CREATE TABLE [dbo].[UserInfo] (
    [UserId]      INT          IDENTITY (1, 1) NOT NULL,
    [DisplayName] VARCHAR (60) NULL,
    [UserName]    VARCHAR (30) NULL,
    [Email]       VARCHAR (50) NOT NULL,
    [Password]    VARCHAR (20) NOT NULL,
    [CreatedDate] DATETIME     DEFAULT (getdate()) NOT NULL,
    [LastLogin]   DATETIME     NULL,
    CONSTRAINT [PK_UserInfo] PRIMARY KEY CLUSTERED ([UserId] ASC)
);

5.2
      CREATE TABLE [dbo].[Logs] (
    [userID]      INT           NULL,
    [Timestamp]   DATETIME      NULL,
    [Descryption] NVARCHAR (50) NULL
);

5.3 
<code>
    CREATE TABLE [dbo].[Employee] (
    [EmployeeID]       INT              NOT NULL,
    [NationalIDNumber] NVARCHAR (15)    NOT NULL,
    [EmployeeName]     NVARCHAR (100)   NULL,
    [LoginID]          NVARCHAR (256)   NOT NULL,
    [JobTitle]         NVARCHAR (50)    NOT NULL,
    [BirthDate]        DATE             NOT NULL,
    [MaritalStatus]    NCHAR (1)        NOT NULL,
    [Gender]           NCHAR (1)        NOT NULL,
    [HireDate]         DATE             NOT NULL,
    [VacationHours]    SMALLINT         NOT NULL,
    [SickLeaveHours]   SMALLINT         NOT NULL,
    [rowguid]          UNIQUEIDENTIFIER ROWGUIDCOL NOT NULL,
    [ModifiedDate]     DATETIME         NOT NULL,
    CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED ([EmployeeID] ASC)
);</code>

6. Podmień connection stringa na swojego (znajdziesz go po kliknięciu PPM na baze Users (Properties, pole Connection string)
    Podmień je w appsetings.json
    
 7. Możliwe że będziesz musiał doinstalować paczki z Nugeta (screen)
 ![image](https://user-images.githubusercontent.com/82310098/200124059-27c45f9e-ccc8-4cf1-bf28-58ebd099ad95.png)

8. Odpal apke i POSTMANA :)

W plikach które pobierzesz jest też plik JWTAPI.postmancollection.json.
Zaimportuj go tutaj:
![image](https://user-images.githubusercontent.com/82310098/200124308-a643cdc4-2f00-4df5-8523-fc697dd110c7.png)

Autoryzacja odbywa się URL <https://localhost:7259/api/token>
Dostaniesz z tego Token. Wklej go w innych requestach do zakładki "Authoryzation" wybierając "Bearer token"
Następnie strzelaj do woli :)

P.S Każdy strzał po API to tak naprawdę URL opisany w każdym z controllerów:
![image](https://user-images.githubusercontent.com/82310098/200124427-be948e49-a11b-426a-8ad2-54999a15d31d.png)

np. chcąc uzyskać info nt. jednego usera - podaj jego id:
![image](https://user-images.githubusercontent.com/82310098/200124451-602abd9a-e6b7-4175-9069-ebb84564c0d2.png)

Bez ID dostaniesz liste wszystkich.

Jak coś to pisz.


