How to run locally:
- .NET 7 required
- Set ConnectionStrings__FunBooksAndVideosConnectionString environment variable to a valid SQL Server connection string
- The SQL Server user needs to have sufficient permissions to create a DB and alter schema.
- Run the project src\FunBooksAndVideos.Web
- API documentation available at /swagger/index.html
- Admin API endpoints are available without authorization for demo purposes.
- API endpoints requiring a logged-in user use the ID specified in src\FunBooksAndVideos.Web\appsettings.Development.json under the key FakeCustomerId.

Running the integration tests:
- The integration tests use a real DB. It is recommended to set ConnectionStrings__FunBooksAndVideosConnectionString to a different DB before execution.
