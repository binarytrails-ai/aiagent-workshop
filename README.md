# AI Agent Workshop

The documentation for this workshop can be found here - [AI Agent Workshop](https://binarytrails-ai.github.io/aiagent-workshop/)

---

## Step-by-Step Plan to Scaffold and Build a React Application

1. **Initialize the Project**
   - Use Vite to scaffold a new React + TypeScript project for fast development.
   - Example command:

     ```bash
     npm create vite@latest my-react-app -- --template react-ts
     ```

   - Change directory:

     ```bash
     cd my-react-app
     ```

2. **Install Dependencies**
   - Install project dependencies:

     ```bash
     npm install
     ```

   - Add ESLint and Prettier for code quality:

     ```bash
     npm install -D eslint prettier eslint-plugin-react eslint-config-prettier
     ```

3. **Initialize Version Control**
   - Initialize a git repository:

     ```bash
     git init
     git add .
     git commit -m "Initial scaffold with Vite + React + TypeScript"
     ```

4. **Run the Development Server**
   - Start the app locally:

     ```bash
     npm run dev
     ```

   - Open the provided local URL in your browser.

5. **Iterate and Add Features**
   - For each new feature:
     1. Create a new branch:

        ```bash
        git checkout -b feature/your-feature-name
        ```

     2. Add or update components in the `src` directory.
     3. Test changes locally.
     4. Commit your changes:

        ```bash
        git add .
        git commit -m "Add feature: your-feature-name"
        ```

     5. Merge the branch back to main after review.

6. **Prompt Instructions for Iteration**
   - To add a new feature, describe it clearly. Example:

     ```text
     Add a login form with email and password fields, and basic validation.
     ```

   - To refactor or improve code, specify the area and goal. Example:

     ```text
     Refactor the App component to use React Context for theme management.
     ```

   - To add tests, specify what to test. Example:

     ```text
     Add unit tests for the Button component using React Testing Library.
     ```

---

You can copy and use these instructions to scaffold and grow your React application efficiently.
