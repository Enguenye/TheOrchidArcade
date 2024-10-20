
# The Orchid Arcade
For a more detailed description and tutorial of the project you can check the docs folder.

## Setup of the Application

### Requirements

To launch the application, the following are required:
- [Visual Studio](https://visualstudio.microsoft.com/)
- [.NET 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [MSSQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [SQL Server Management Studio](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16#download-ssms)

### Steps to Launch the Server

1. **Create a New Database**  
   Open the SQL Server Management Studio, connect to your MSSQL Server and create a new Database.


2. **Copy the Connection String**  
   Copy the connection string from the SQL Server. An example connection string may look like this:
   ```
   Server=localhost\MSSQLSERVER01;Database=master;Trusted_Connection=True;
   ```  
   Make sure to replace "master" with the name of your database

3. **Add Connection String to `appsettings.json`**  
   Update the `appsettings.json` file in your application by adding the connection URL like this:
   ```json
   "DefaultConnection": "Server=localhost\\MSSQLSERVER01;Database=TheOrchidArcade;Trusted_Connection=True;TrustServerCertificate=True;"
   ```

4. **Update the Database**  
   Open the dot net project using Visual Studio and .NET 8.0. Then, navigate to **Tools** > **NuGet Package Manager** > **Package Manager Console**.  
   Run the following command in the NuGet Console:
   ```
   Update-Database
   ```
   This command will create all the necessary database tables. If you need to reset the database you can run 
    ```
   Drop-Database
   ```
   Followed by
    ```
   Update-Database
   ```

5. **Run the Server**  
   You should now be able to run the HTTP server from Visual Studio without issues.

### Running Unit Tests

All unit tests can be run from inside Visual Studio by selecting **Test** > **Run All Tests**.