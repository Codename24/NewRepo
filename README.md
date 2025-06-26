# TaxCalculator.API
- In order to run the application in proper way connection string to the MS SQL database should be set as env variable
- On Each application run, EF Migration is getting performed on the Database to check whether the latest DB schema is applied
- For CQRS implementation MediatR library was used
- For Unit Tests Moq library and xUnit Framework were used
- Ín order to have structured logging Serilog package was used. For now, Serilog is configured in application settings, so the system is capable of changing logging configuration dynamically
