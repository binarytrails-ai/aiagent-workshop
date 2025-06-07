# Lab 2: Using Azure AI Foundry Agents

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
   <br>Navigate to your code repository and run the following commands from the root directory:

    - Log in to your Azure account by entering the following command in your terminal. This will open a browser window for you to log in to your Azure account. Enter the device code displayed in the terminal and authenticate your account.
      ```bash
      azd auth login --use-device-code
      ```
    - Check if you are logged in successfully by running the following command. It should display your account information.
      ```bash
      azd auth show
      ```
    - Now you can deploy the Azure resources to your Azure subscription. The Infrastructure as Code (IaC) files are located in the `infra` directory of your code repository. Run the following command to deploy the resources:
      ```bash
      azd up
      ```
---

## Let's get started 👩‍💻🤖

<!-- Lab instructions will go here. -->

**Objectives:**

- Understand Azure AI Hub and Azure Foundry Project
- Work with Azure AI Foundry Agents

## Further Reading 📚

- [Azure AI Hub Documentation](https://learn.microsoft.com/azure/ai-hub/)
- [Azure Foundry Project Documentation](https://learn.microsoft.com/azure/ai-foundry/)
- [Azure Developer CLI Documentation](https://learn.microsoft.com/azure/developer/azure-developer-cli/)
