# Lab 2: Using Azure AI Foundry Agent Service

## Welcome to Lab 2! 🚀

In this lab, you will learn how to use Azure AI Foundry Agents to build and extend AI-powered solutions.

!!! Info
    **Azure AI Hub** provides a unified interface for managing datasets, models, prompts, and workflows. It streamlines collaboration across teams, ensures compliance and governance, and offers monitoring tools to track the lifecycle of your AI assets from development to production.

    **Azure Foundry Project** is a structured workspace that brings together all the resources, code, and configurations needed to build, train, and deploy AI solutions. It leverages best practices for organizing your AI workloads and integrates seamlessly with Azure AI Hub.

---

## Setting up your environment 🔧

1. Complete the [Getting Started](00-getting_started.md) lab to set up your environment.
2. In this lab, you will use **Azure AI Foundry Agents**.
   <br>To deploy the required Azure resources, use the [Azure Developer CLI (`azd`)](https://learn.microsoft.com/azure/developer/azure-developer-cli/).
   <br>Open a terminal and navigate to the root of the project.
    - Log in to your Azure account by entering the following command in your terminal. This will open a browser window for you to log in to your Azure account. Enter the device code displayed in the terminal and authenticate your account.

      ```bash
      azd auth login --use-device-code
      ```

    - Check if you are logged in successfully by running the following command. It should display your account information.

      ```bash
      azd auth show
      ```

    - Set the location for your Azure resources. This is where your resources will be deployed.

      ```bash
      azd env set AZURE_LOCATION australiaeast
      ```

    - Now you can deploy the Azure resources to your Azure subscription. The Infrastructure as Code (IaC) files are located in the `infra` directory of your code repository. Run the following command to deploy the resources:

      ```bash
      azd up
      ```

        <!-- When you run this command, you will be prompted to provide values for the `environment name` and `location`. Enter `dev` as the environment name and `australiaeast` as the location. You can accept the default values for any additional prompts.</br>
        If you get an error as `TODO:Add the error message here`, add the `AZURE_LOCATION` environment variable to your `.env` file located in `.azure/dev/` directory -

        ```plaintext
        AZURE_LOCATION="australiaeast"
        ``` -->

3. After the deployment is complete, copy all the environment variables from the `.env` file located iatn `.azure/dev/.env` into `.env` file in the root directory of your code repository.

## Let's get started 👩‍💻🤖

<!-- Lab instructions will go here. -->

## Challenges in Building AI Agents ⚡

TODO: update this section with the challenges in building AI agents.
<!-- 
Agents developed using Foundry Agent Service have the following elements:

<https://learn.microsoft.com/en-us/training/modules/ai-agent-fundamentals/4-azure-ai-agent-service>

Model: A deployed generative AI model that enables the agent to reason and generate natural language responses to prompts. You can use common OpenAI models and a selection of models from the Azure AI Foundry model catalog.
Knowledge: data sources that enable the agent to ground prompts with contextual data. Potential knowledge sources include Internet search results from Microsoft Bing, an Azure AI Search index, or your own data and documents.
Tools: Programmatic functions that enable the agent to automate actions. Built-in tools to access knowledge in Azure AI Search and Bing are provided as well as a code interpreter tool that you can use to generate and run Python code. You can also create custom tools using your own code or Azure Functions.
Conversations between users and agents take place on a thread, which retains a history of the messages exchanged in the conversation as well as any data assets, such as files, that are generated.

<https://learn.microsoft.com/en-us/azure/ai-foundry/model-inference/how-to/quickstart-create-resources?pivots=ai-foundry-portal#understand-the-resources>

Building robust AI agents involves addressing several key challenges:

### 🧠 Memory Management

- Ensuring agents can store, retrieve, and update relevant information efficiently.
- Handling context over long conversations or tasks.

### 🔗 Communication Across Agents

- Facilitating seamless information sharing and coordination between multiple agents.
- Managing dependencies and message passing in multi-agent systems.

### ⏳ Long-Running Processes

- Supporting tasks that require extended execution time or background processing.
- Handling interruptions, retries, and state persistence.

### 🛡️ Security and Compliance

- Protecting sensitive data and ensuring agents operate within governance policies.
- Managing access controls and audit trails.

---

**Azure AI Foundry Agents** are designed to address these challenges by providing built-in solutions for memory management, agent communication, long-running process orchestration, and enterprise-grade security. This enables you to focus on building intelligent solutions without reinventing the wheel.

**Objectives:**

- Understand Azure AI Hub and Azure Foundry Project
- Work with Azure AI Foundry Agents

## Further Reading 📚

- [Azure AI Hub Documentation](https://learn.microsoft.com/azure/ai-hub/)
- [Azure Foundry Project Documentation](https://learn.microsoft.com/azure/ai-foundry/)
- [Azure Developer CLI Documentation](https://learn.microsoft.com/azure/developer/azure-developer-cli/) -->
