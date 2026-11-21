# SilentInterview — production-ready deployment baseline

A full-stack recruitment and interview platform.

## Stack
- React 19 + Vite frontend (Render Static Site)
- ASP.NET Core 10 LTS API (Docker)
- PostgreSQL 17 (managed Render Postgres)
- Render Blueprint (`render.yaml`) provisions and connects the services

## Production deploy
1. Upload the contents of this project to a **new, clean GitHub repository**. The ZIP is intentionally supplied without `.git` history or local `.env` files.
2. In Render, choose **New → Blueprint**, connect that GitHub repository, and use `render.yaml` at the repository root.
3. During the initial Blueprint setup, enter the email address for the first SuperAdmin account when prompted. Render generates the JWT signing key, database credentials, and initial SuperAdmin password.
4. Deploy all resources. The API waits for the PostgreSQL schema and initial seed to finish before it accepts traffic.
5. Open the frontend URL shown by Render. The default URL configured in this Blueprint is `https://silentinterview-prod-web-v2.onrender.com`.

## Render resources and cost
- API: paid `1c-2g` web service, Frankfurt, plus a 5 GB persistent upload disk.
- PostgreSQL: paid `0.5c-1g` plan with 15 GB storage, Frankfurt, internal-only networking. It is not the expiring 30-day Free database.
- Frontend: static site served by Render's global CDN.

Check current Render pricing before creating the Blueprint. Paid service and database charges apply.

## Production secrets
Never commit `back-end/.env`, `frontend/.env`, API keys, passwords, or connection strings. Render generates DB credentials and `Jwt__Key` from the Blueprint. The API is configured to process Render's HTTPS proxy headers. Keep any additional provider credentials in the API service's **Environment** page.

Optional integrations require valid credentials from their respective providers:
- AI features: `GROQ_API_KEY` (Groq).
- Voice transcription: `OpenAI__ApiKey` (OpenAI).
- Google sign-in: `Google__ClientId`; also add the frontend URL as an authorized JavaScript origin in Google Cloud Console.
- Billing: `Stripe__SecretKey`, `Stripe__WebhookSecret`, `Stripe__GoPriceId`, `Stripe__ProPriceId`. Set the Stripe webhook endpoint to `https://<YOUR-RENDER-API-HOST>/api/v1/subscription/webhook` (replace the host with the exact public URL shown on your Render API service).
- Password-reset email: `Smtp__Host`, `Smtp__Port`, `Smtp__From`, `Smtp__Username`, and `Smtp__Password`. Use a paid API plan for SMTP providers that require port 587.

These integrations cannot be activated with invented values. Core email/password registration and login, database-backed application functions, and the frontend/API connection do not require those optional provider keys.

## Local Docker development
```powershell
Copy-Item back-end/.env.example back-end/.env
cd back-end
docker compose up --build -d
```
Frontend: `http://localhost:3000`  
API: `http://localhost:5000`

The local `.env.example` values are development-only and must never be reused as production secrets.
