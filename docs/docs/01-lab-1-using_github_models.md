# Explore Semantic Kernel using GitHub Models

## Welcome to the hands-on labs! 🎉

In this lab, you will learn how to interact with Large Language Models (LLMs) using Semantic Kernel.

You will use [GitHub Models](https://github.com/marketplace?type=models), which are free and provide a great way to prototype and experiment with different Large Language Models (LLMs).

---

## Setting up your environment 🔧

1. Complete the [Getting Started](00-getting_started.md) lab to set up your environment.
2. You will be using GitHub Models in this lab. From your browser, navigate to [GitHub Models Catalog](https://github.com/marketplace?type=models). You can chat with the models from the **Playground**. </br>
   ![GitHub Models Catalog](https://docs.github.com/assets/images/help/models/models-catalog.png)
3. We will be interacting with the models from our own applications, so we need an API key to authenticate our requests. Follow these steps to get your API key:

    - From the **Playground**, select `gpt-4o` or any other chat completion model you want to use.
    - On the top right corner of the **Playground**, click on the **Use this model** button. This will open a pop-up window with API key options.
    - Select **Get Developer Key**. This will take you to the page to create Personal Access Token (PAT).
    - Click on **Generate new token**.
    - Give your token a name, like `aiagent-workshop`.
    - Set the expiration to **No expiration** (or choose a suitable duration).
    - Under Permissions, navigate to Models and select  **Read-Only** access.
    - Click on **Generate token**. You will see a confirmation message with your new token.
    - Copy the generated token and save it securely. You will need this token to authenticate your requests.

4. Navigate to the code repository and run the following command to install the required packages:

    ```bash
    cp -r .env.example .env
    ```

5. Update the `.env` file with your GitHub Models API key:

    ```plaintext
    GITHUB_TOKEN=<your_github_models_api_key>
    ```

---

## Let's get started 👩‍💻🤖

Start your first exercise for this lab here:

👉 **[Lab 1.1: Interacting with Large Language Models using Semantic Kernel](./01-lab-1.1-interacting_with_llm.md)**

Happy coding!

---

## Further Reading 📚

- [Semantic Kernel Documentation](https://learn.microsoft.com/semantic-kernel/)
- [GitHub Models Documentation](https://docs.github.com/en/github-models/use-github-models)
