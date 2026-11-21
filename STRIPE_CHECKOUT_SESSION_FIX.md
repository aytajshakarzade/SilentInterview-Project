# Stripe Checkout return/session fix

This release fixes the race that could send a customer to the login page or show
“Your session has expired” immediately after a successful Stripe Checkout.

## What changed

- Protected and guest routes wait until the HttpOnly refresh-cookie session has
  finished hydrating. Billing pages therefore do not start their authenticated
  sync request before an access token is ready.
- Initial session hydration and the global Axios 401 handler now share one
  refresh request. This matters because the API rotates refresh tokens; two
  concurrent refresh requests could invalidate one another.
- The login redirect now preserves the full path, query string, and hash. If a
  session really has expired, signing in again returns to
  `/billing/success?session_id=...` instead of dropping the Stripe Session ID.
- Stripe webhook handling initializes its API key on every API process before
  retrieving subscription details.
- The uploaded local `back-end/.env` was excluded from this release archive.
  Only placeholder values are kept in `back-end/.env.example`.

## Stripe environment variables (Render API service)

Keep these on the API service, not the static frontend:

- `Stripe__SecretKey`: Stripe **test** secret key while testing.
- `Stripe__WebhookSecret`: signing secret for the webhook endpoint created in
  Stripe Workbench/Developers → Webhooks.
- `Stripe__GoPriceId`: `price_1U9NWE0WSpP34h0NTC8Ljtxp`
- `Stripe__ProPriceId`: `price_1U9NWZ0WSpP34h0NWToyhQLF`
- `Frontend__BaseUrl`: exact public frontend URL shown by Render, without a
  trailing slash.
- `FRONTEND_ORIGIN`: the same frontend origin, without a trailing slash.

On the Render static frontend, `VITE_API_BASE_URL` must be the exact public API
URL followed by `/api/v1`.

## Webhook endpoint

Create a webhook endpoint using the exact public base URL shown on the Render
API service, followed by:

`/api/v1/subscription/webhook`

Subscribe it to at least these events:

- `checkout.session.completed`
- `customer.subscription.updated`
- `customer.subscription.deleted`

Copy the endpoint's `whsec_...` signing secret into the API service's
`Stripe__WebhookSecret` environment variable, then redeploy the API.

Do not use a live-mode signing secret with a test-mode secret key, or vice versa.
