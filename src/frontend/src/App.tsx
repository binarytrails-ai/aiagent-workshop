import React, { useEffect, useState } from "react";
import { AgentPreview } from "./components/agents/AgentPreview";
import { ThemeProvider } from "./components/core/theme/ThemeProvider";
import { API_BASE_URL } from "./api";

const App: React.FC = () => {
  // State to store the agent details
  const [agentDetails, setAgentDetails] = useState({
    id: "loading",
    object: "agent",
    created_at: Date.now(),
    name: "Loading...",
    displayName: "",
    description: "Loading agent details...",
    model: "default",
    metadata: {
      logo: "robot",
    },
  });

  // Fetch agent details from remote API when component mounts
  useEffect(() => {
    const fetchAgentDetails = async () => {
      try {
        const response = await fetch(`${API_BASE_URL}/api/agent`, {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
          },
        });

        if (response.ok) {
          const data = await response.json();
          setAgentDetails(data);
        } else {
          setAgentDetails({
            id: "fallback",
            object: "agent",
            created_at: Date.now(),
            name: "AI Agent",
            displayName: "AI Agent",
            description: "Could not load agent details",
            model: "default",
            metadata: {
              logo: "robot",
            },
          });
        }
      } catch (error) {
        setAgentDetails({
          id: "error",
          object: "agent",
          created_at: Date.now(),
          name: "AI Agent",
          displayName: "AI Agent",
          description: "Error loading agent details",
          model: "default",
          metadata: {
            logo: "robot",
          },
        });
      }
    };

    fetchAgentDetails();
  }, []);

  return (
    <ThemeProvider>
      <div className="app-container">
        <AgentPreview
          resourceId="sample-resource-id"
          agentDetails={agentDetails}
        />
      </div>
    </ThemeProvider>
  );
};

export default App;
