namespace McWuT.Common.CrimeGenerator.ExternalApis;

// LLM API Request/Response Models
public class LlmRequest
{
    public string Model { get; set; } = "gpt-4";
    public List<LlmMessage> Messages { get; set; } = new();
    public int MaxTokens { get; set; } = 1000;
    public double Temperature { get; set; } = 0.7;
    public double TopP { get; set; } = 1.0;
    public int N { get; set; } = 1;
    public List<string> Stop { get; set; } = new();
}

public class LlmMessage
{
    public string Role { get; set; } = string.Empty; // "system", "user", "assistant"
    public string Content { get; set; } = string.Empty;
}

public class LlmResponse
{
    public string Id { get; set; } = string.Empty;
    public string Object { get; set; } = string.Empty;
    public long Created { get; set; }
    public string Model { get; set; } = string.Empty;
    public List<LlmChoice> Choices { get; set; } = new();
    public LlmUsage Usage { get; set; } = new();
}

public class LlmChoice
{
    public int Index { get; set; }
    public LlmMessage Message { get; set; } = new();
    public string FinishReason { get; set; } = string.Empty;
}

public class LlmUsage
{
    public int PromptTokens { get; set; }
    public int CompletionTokens { get; set; }
    public int TotalTokens { get; set; }
}