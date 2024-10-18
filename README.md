
# The Orchid Arcade

## Setup of the Application

### Requirements

To launch the application, the following are required:
- [Visual Studio](https://visualstudio.microsoft.com/)
- [MSSQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

### Steps to Launch the Server

1. **Create a New Database**  
   Open Microsoft SQL Server and create a new database using Windows Authentication.

2. **Copy the Connection String**  
   Copy the connection string from the SQL Server. An example connection string may look like this:
   ```
   Server=localhost\MSSQLSERVER01;Database=master;Trusted_Connection=True;
   ```

3. **Add Connection String to `appsettings.json`**  
   Update the `appsettings.json` file in your application by adding the connection URL like this:
   ```json
   "DefaultConnection": "Server=localhost\\MSSQLSERVER01;Database=TheOrchidArcade;Trusted_Connection=True;TrustServerCertificate=True;"
   ```

4. **Update the Database**  
   In Visual Studio, navigate to **Tools** > **NuGet Package Manager** > **Package Manager Console**.  
   Run the following command in the NuGet Console:
   ```
   Update-Database
   ```
   This command will create all the necessary database tables.

5. **Run the Server**  
   You should now be able to run the HTTP server from Visual Studio without issues.

### Running Unit Tests

All unit tests can be run from inside Visual Studio by selecting **Test** > **Run All Tests**.