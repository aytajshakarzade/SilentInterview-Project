# PostgreSQL deployment notes

The backend uses Npgsql with PostgreSQL. The current Render production configuration is defined in the repository-root `render.yaml`; follow `DEPLOY_RENDER_AZ.md` for deployment. Render provisions a new paid PostgreSQL instance and injects `PGHOST`, `PGPORT`, `PGDATABASE`, `PGUSER`, and `PGPASSWORD` into the API. Do not manually paste a connection string or the old expired database hostname.

On a new empty PostgreSQL database, the API runs `EnsureCreatedAsync()` and the idempotent seeder before the API starts accepting traffic. This repository does not include a data export/import from the expired database, so previous users, interviews, or company records are not automatically restored.

The project does not currently contain EF Core migrations. `EnsureCreatedAsync()` creates the initial schema for a fresh database; future structural schema changes should be accompanied by proper EF Core migrations before production deployment.
