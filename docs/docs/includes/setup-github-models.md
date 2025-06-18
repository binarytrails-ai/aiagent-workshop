# 🚀 Get Started with GitHub Models

This guide will walk you through the setting up ChatGPT (OpenAI GPT-4o) on GitHub.

GitHub Models are **free to use**, meaning you can explore and build without worrying about costs. However, **rate limits** that apply, so you can only make a certain number of requests per minute. If you hit a limit, just wait a bit before trying again.

Check out the full details [here](https://docs.github.com/en/github-models/use-github-models/prototyping-with-ai-models#rate-limits).

---

## 🔧 Setting Up in a Few Easy Steps

1. Login using your GitHub account. Don't have an account yet? No worries! You can sign up in just a few clicks [on the GitHub sign up page](https://github.com/join).

2. Go to [GitHub Models](https://github.com/features/models).
3. Browse available models  from [GitHub Models Catalog](https://github.com/marketplace/models).

   ![GitHub Models Catalog](https://docs.github.com/assets/images/help/models/models-catalog.png)

5. Want to chat with a model? Click **Playground** in the left menu after selecting a model.
6. Choose **OpenAI GPT-4.1** to start interacting.
7. Try asking something like:

   ```plaintext
    Can you explain the basics of quantum computing?
   ```

## 4️⃣ Grab Your API Key

We will be interacting with the models from our own applications, so we need an API key to authenticate our requests. Follow these steps to get your API key:

1. On the top right corner of the **Playground**, click on the **Use this model** button.
1. Select **Get Developer Key**. This will take you to the page to create Personal Access Token (PAT).
1. Click on **Generate new token**.
1. Give your token a name, like `GitHub Models Access`.
1. Set the expiration to **No expiration** (or choose a suitable duration).
1. Under Permissions, navigate to Models and select  **Read-Only** access.
1. Copy the generated token and save it securely. You will need this token to authenticate your requests.

---

## 📌 References

- [GitHub Models Overview](https://github.com/features/models)
- [GitHub Marketplace Models](https://github.com/marketplace/models)

---

🎉 That’s it! Time to experiment with AI Agents
