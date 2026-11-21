# AI integrations

The interview assistant, AI question generation, evaluations, and report generation use Groq through the OpenAI-compatible endpoint. Add a valid `GROQ_API_KEY` to the Render API service's Environment page, save, and redeploy.

Audio transcription uses the OpenAI configuration and requires `OpenAI__ApiKey`. Without a valid provider key, those AI-specific features return a configuration/provider error; the rest of the application can still run.

Never put production provider keys in Git, `.env.example`, source code, or a Docker build context.
