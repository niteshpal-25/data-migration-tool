**First Table User Details**

CREATE TABLE [dbo].[UserDetails](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[EmpID] [nchar](10) NULL,
	[UserName] [nvarchar](150) NOT NULL,
	[Pswrd] [nvarchar](50) NOT NULL,
	[UserType] [nvarchar](50) NOT NULL,
	[Dept] [nvarchar](50) NOT NULL,
	[email_ID] [nvarchar](200) NOT NULL,
	[Gender] [nvarchar](50) NULL,
	[DOB] [datetime] NULL,
	[IsActive] [bit] NOT NULL,
	[IsAdded] [bit] NULL,
 CONSTRAINT [PK_UserDetails] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

**Department Details**

CREATE TABLE [dbo].[Departments](
	[DeptID] [int] IDENTITY(1,1) NOT NULL,
	[DeptName] [nvarchar](100) NOT NULL,
	[Location] [nvarchar](100) NULL,
	[CreatedDate] [datetime] NULL,
	[IsAdded] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[DeptID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

**Project Details**

CREATE TABLE project (
    project_id INT PRIMARY KEY identity(1,1),
    project_name VARCHAR(50),
    dept_id INT,
	isactive bit,
	IsAdded	bit
);
