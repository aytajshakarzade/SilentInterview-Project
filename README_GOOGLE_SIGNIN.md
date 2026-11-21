# Google Sign-In setup

SilentInterview uses Google Identity Services for the browser button and verifies the returned ID token on the API.

Google Cloud must allow the exact JavaScript origin used by the browser. For the Render Blueprint default, add `https://silentinterview-prod-web-v2.onrender.com` to Authorized JavaScript origins. For local Docker development, add `http://localhost:3000`. A host or port mismatch causes `origin_mismatch`.

Set `Google__ClientId` on the Render API service to the web OAuth client ID. The client ID is private project configuration (not an API secret), but still should be supplied through deployment configuration rather than hard-coded into this source tree.

The API accepts the web client id from `Google__ClientId` / `VITE_GOOGLE_CLIENT_ID` and never trusts a client-provided email. Existing accounts sign in immediately; first-time users receive a short-lived signed setup ticket so the setup step does not verify the same Google ID token twice.
