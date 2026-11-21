# PostgreSQL / Render deployment

The production stack is provisioned by the root `render.yaml` Blueprint. It creates a new paid Render Postgres database in Frankfurt and injects its private host, port, database, user, and password into the API service. The database is internal-only and is not a Free database with a 30-day expiry.

A new empty database is initialized with EF Core `EnsureCreatedAsync()` and the idempotent application seeder before the API starts accepting traffic. The project does not currently contain a data migration from the old expired PostgreSQL database. Old records must be exported from a recoverable source and imported separately.

For deployment, follow `DEPLOY_RENDER_AZ.md`. Never commit `.env` or database credentials.
