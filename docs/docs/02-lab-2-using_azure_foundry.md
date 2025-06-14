# Lab 2: Using Azure AI Foundry Agent Service

## Welcome to Lab 2! 🎉

In this lab, you’ll learn how to build and extend the capabilities of your AI agents using [Azure AI Foundry Agent Service](https://learn.microsoft.com/en-us/azure/ai-services/agents/overview).

!!! Info
    **Azure AI Hub** provides a unified interface for managing datasets, models, prompts, and workflows. It streamlines collaboration across teams, ensures compliance and governance, and offers monitoring tools to track the lifecycle of your AI assets from development to production.

    **Azure Foundry Project** is a structured workspace that brings together all the resources, code, and configurations needed to build, train, and deploy AI solutions. It leverages best practices for organizing your AI workloads and integrates seamlessly with Azure AI Hub.

---

## Setting up your environment 🔧

1. Complete the steps in [Getting Started](00-getting_started.md) to set up your environment.
2. In this lab, you’ll use **Azure AI Foundry Agents** to build AI agents. To get started, you’ll need to deploy the required resources to your Azure subscription.

    - Open a terminal and navigate to the root of your project.

    - Log in to your Azure account - Run the following command to authenticate with Azure. This command will prompt you to open a browser and enter a device code displayed in your terminal. After successful authentication, return to the terminal to continue.

      ```powershell
        azd auth login --use-device-code
      ```

    - Verify you’re logged in:

      ```powershell
      azd auth show
      ```

      You should see your account information.

    - Set the Azure region for your resources (you can change `australiaeast` to your preferred region):

      ```powershell
      azd env set AZURE_LOCATION australiaeast
      ```

    - Deploy the Azure resources using the Infrastructure as Code (IaC) files in the `infra` directory:

      ```powershell
      azd up
      ```

      This command will provision all the necessary Azure resources required for this lab. You can browse the resources in the Azure portal once the deployment is complete.

3. You will be connecting to these resources when running the code in this lab.

    After deployment, copy all environment variables from `.azure/dev/.env` into the `.env` file in the root of your code repository.

    ```powershell
    cp .azure/dev/.env .env
    ```

<!-- TODO: Add details about the azure resources deployed -->
---

## Let's get started 👩‍💻🤖

You’re now ready to start building AI agents using Azure AI Foundry Agent Service!
Follow the instructions in this lab to create your own AI agents.

---

## Further Reading 📚

- [Azure AI Hub Documentation](https://learn.microsoft.com/azure/ai-hub/)
- [Azure Foundry Project Documentation](https://learn.microsoft.com/azure/ai-foundry/)
- [Azure AI Foundry Agent Service Documentation](https://learn.microsoft.com/en-us/azure/ai-services/agents/overview)
- [Azure Developer CLI Documentation](https://learn.microsoft.com/azure/developer/azure-developer-cli/)
