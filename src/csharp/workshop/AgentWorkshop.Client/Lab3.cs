using Azure.AI.Projects;

namespace AgentWorkshop.Client;

public class Lab3(AIProjectClient client, string modelName) : Lab(client, modelName)
{
    protected override string InstructionsFileName => "code_interpreter.txt";

    public override IEnumerable<ToolDefinition> IntialiseLabTools() =>
        [new CodeInterpreterToolDefinition()];
}
