
using System.Text;

namespace SilentInterview.Infrastructure.AI.Prompts;

/// <summary>
/// Shared language-name resolution.
/// </summary>
public static class LanguageMap
{
    public static string Name(string? code) =>
        (code ?? "en").ToLowerInvariant() switch
        {
            "az" => "Azerbaijani",
            "ru" => "Russian",
            _ => "English"
        };

    public static string Normalize(string? code)
    {
        var c = (code ?? "en").ToLowerInvariant();
        return c is "az" or "ru" or "en" ? c : "en";
    }
}

public static class QuestionGenerationPromptBuilder
{
    public static string BuildSystem(string languageName) => $"""
        You are a senior HR recruiter, technical interviewer and behavioral interviewer
        with 15+ years of experience building structured interview loops.

        You design precise, role-relevant, non-generic interview questions.
        Always respond with strict JSON only.
        Write all question text in {languageName}.

        ABSOLUTE LANGUAGE LOCK:
        - Every question, introduction, expected answer and evaluation criterion
          MUST be in {languageName}.
        - Never switch languages.
        - Output language is ALWAYS {languageName}.
        """;

    public static string BuildUser(
        string jobTitle,
        string jobDescription,
        IReadOnlyList<string> requiredSkills,
        string? experienceLevel,
        string? companyCulture,
        string? seniorityLevel,
        int durationMinutes,
        string languageName,
        string uniquenessSeed)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"Job Title: {jobTitle}");
        sb.AppendLine($"Job Description: {jobDescription}");
        sb.AppendLine(
            $"Required Skills: {(requiredSkills.Count > 0 ? string.Join(", ", requiredSkills) : "not specified")}");
        sb.AppendLine($"Experience Level: {experienceLevel ?? "not specified"}");
        sb.AppendLine($"Seniority Level: {seniorityLevel ?? "not specified"}");
        sb.AppendLine($"Company Culture: {companyCulture ?? "not specified"}");
        sb.AppendLine($"Target Interview Duration: {durationMinutes} minutes");
        sb.AppendLine($"Uniqueness Seed: {uniquenessSeed}");
        sb.AppendLine();

        sb.AppendLine(
            $"Generate a complete AI interview structure in {languageName}.");

        sb.AppendLine("""
            Produce 6 sections in this exact order:
            technical, behavioral, situational, soft_skill, culture_fit, problem_solving.

            Each section must contain 2-4 unique questions tailored to the job.

            Within each section, order questions by increasing difficulty:
            easy, medium, hard.

            Every question must include:
            - expectedAnswer
            - evaluationCriteria

            Also provide a short introduction.

            Respond with ONLY this JSON shape:
            {
              "interviewTitle": "string",
              "introduction": "string",
              "difficulty": "Junior | Mid | Senior | Lead",
              "estimatedDurationMinutes": 30,
              "sections": []
            }
            """);

        return sb.ToString();
    }
}

public static class InterviewFlowPromptBuilder
{
    public static string BuildEvaluationSystem(string languageName) => $"""
        You are an expert technical and behavioral interviewer.
        Evaluate answers fairly and based on evidence only.
        Do not invent details.
        Write all natural-language output in {languageName}.
        Respond with strict JSON only.
        """;

    public static string BuildEvaluationUser(
        string jobTitle,
        string sectionType,
        string question,
        string answer,
        IReadOnlyList<string> previousQuestions,
        string languageName)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"Job Title: {jobTitle}");
        sb.AppendLine($"Current Section: {sectionType}");

        if (previousQuestions.Count > 0)
        {
            sb.AppendLine("Previously asked questions:");

            foreach (var q in previousQuestions)
            {
                sb.AppendLine($"- {q}");
            }
        }

        sb.AppendLine();
        sb.AppendLine($"Question: {question}");
        sb.AppendLine($"Candidate's Answer: {answer}");
        sb.AppendLine();

        sb.AppendLine("""
            Evaluate the answer.

            Score each dimension from 0 to 100:
            - technicalAccuracy
            - communicationScore
            - confidenceScore
            - clarityScore
            - depthScore

            Decide nextAction:
            - continue_deeper
            - clarify
            - move_next

            Respond with ONLY this JSON shape:
            {
              "technicalAccuracy": 0,
              "communicationScore": 0,
              "confidenceScore": 0,
              "clarityScore": 0,
              "depthScore": 0,
              "keywordsDetected": [],
              "strengths": [],
              "weaknesses": [],
              "followUpNeeded": false,
              "aiComment": "string",
              "nextAction": "move_next",
              "followUpQuestion": null
            }
            """);

        sb.AppendLine(
            $"All natural-language values must be in {languageName}.");

        return sb.ToString();
    }
}

public static class ReportAIPromptBuilder
{
    public static string BuildSystem(string languageName) => $"""
        You are an evidence-first interview coach.
        Ground every conclusion in recorded candidate answers.
        Never invent facts, skills, achievements, metrics or quotes.

        Fairness safeguards:
        - This report is decision support, not an automated hiring decision.
        - Never infer competence or personality from appearance, facial expressions,
          emotion, eye contact, accent or camera signals.
        - Use Needs Review when evidence is insufficient.
        - Do not estimate salary without reliable salary data.
        - Clearly distinguish self-reported skills from verified evidence.

        Write the entire report in {languageName}.
        Respond with strict JSON only.
        """;

    public static string BuildUser(
        string jobTitle,
        string candidateSkills,
        string candidateExperience,
        string transcriptAndEvaluations,
        string deterministicSignals,
        string languageName)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"Job title: {jobTitle}");
        sb.AppendLine($"Candidate-stated skills: {candidateSkills}");
        sb.AppendLine($"Candidate-stated experience: {candidateExperience}");
        sb.AppendLine();

        sb.AppendLine("=== Recorded Interview Answers ===");
        sb.AppendLine(transcriptAndEvaluations);
        sb.AppendLine();

        sb.AppendLine("=== Additional Session Metrics ===");
        sb.AppendLine(deterministicSignals);
        sb.AppendLine();

        sb.AppendLine(
            $"Prepare an evidence-based interview report in {languageName}.");

        sb.AppendLine("""
            Respond with ONLY this JSON shape:
            {
              "executiveSummary": "string",
              "hiringRecommendation": "Needs Review",
              "hiringRecommendationReason": "string",
              "candidateProfile": "string",
              "technicalAnalysis": "string",
              "communicationAnalysis": "string",
              "behaviorAnalysis": "string",
              "leadershipAnalysis": "string",
              "cultureFit": "string",
              "problemSolving": "string",
              "strengths": [],
              "weaknesses": [],
              "riskFactors": [],
              "learningPotential": "string",
              "salaryEstimation": "Not assessed",
              "roleMatch": "string",
              "nextInterviewRecommendation": "string",
              "customFollowUpQuestions": [],
              "confidenceScore": 0
            }
            """);

        return sb.ToString();
    }
}

public static class AIAssistantPromptBuilder
{
    public static string BuildSystem(string languageName) => $"""
        You are SilentInterview's intelligent AI HR Assistant.

        YOUR ROLE:
        - Help recruiters with hiring, recruitment, interviewing, candidate evaluation,
          job descriptions, interview questions and HR best practices.
        - Answer general knowledge and conversational questions naturally.
        - Respond naturally to greetings such as hello, hi, salam and how are you.
        - Give practical, clear, professional and useful answers.

        CANDIDATE DATA AND ACCURACY:
        - Use ONLY the company-specific candidate and interview data provided below.
        - Never invent candidate names, answers, scores, experience or hiring outcomes.
        - If asked about a specific candidate and no relevant data is available,
          clearly explain that the company has no interview data available yet.
        - If the question is general HR or recruitment advice, answer it even when
          candidate data is empty.
        - Clearly distinguish general professional advice from actual company facts.
        - Never pretend that a candidate or interview exists when data is missing.

        CONVERSATION:
        - Remember previous messages when relevant.
        - Answer the recruiter's current question directly.
        - Do not repeat the same fallback response to unrelated questions.

        LANGUAGE AND STYLE:
        - Respond entirely in {languageName}.
        - Use plain text, not JSON.
        - Be concise but informative.
        - Use a friendly, professional recruiter-assistant tone.
        """;

    public static string BuildUser(
        string question,
        string groundingData,
        List<(string Role, string Content)>? conversationHistory = null)
    {
        var sb = new StringBuilder();

        sb.AppendLine("=== Company Candidate and Interview Data ===");

        if (string.IsNullOrWhiteSpace(groundingData))
        {
            sb.AppendLine(
                "No candidate or interview data is currently available.");
        }
        else
        {
            sb.AppendLine(groundingData);
        }

        sb.AppendLine();

        if (conversationHistory is { Count: > 0 })
        {
            sb.AppendLine("=== Previous Conversation ===");

            foreach (var (role, msg) in conversationHistory)
            {
                var label = role.Equals(
                    "user",
                    StringComparison.OrdinalIgnoreCase)
                    ? "Recruiter"
                    : "Assistant";

                var truncated = msg.Length > 400
                    ? msg[..400] + "…"
                    : msg;

                sb.AppendLine($"{label}: {truncated}");
            }

            sb.AppendLine();
        }

        sb.AppendLine("=== Recruiter's Current Question ===");
        sb.AppendLine(question);

        return sb.ToString();
    }
}
