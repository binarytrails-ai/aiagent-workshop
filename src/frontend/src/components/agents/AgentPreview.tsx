import { ReactNode, useState, useMemo, useEffect, useCallback } from "react";
import { Button, Caption1, Spinner, Title2 } from "@fluentui/react-components";
import { ChatRegular } from "@fluentui/react-icons";

import { AgentIcon } from "./AgentIcon";
import { AgentPreviewChatBot } from "./AgentPreviewChatBot";
import { MenuButton } from "../core/MenuButton/MenuButton";
import { IChatItem } from "./chatbot/types";
import { API_BASE_URL } from "../../api";

import styles from "./AgentPreview.module.css";

interface IAgent {
  id: string;
  object: string;
  created_at: number;
  name: string;
  displayName?: string;
  description?: string | null;
  model: string;
  instructions?: string;
  tools?: Array<{ type: string }>;
  top_p?: number;
  temperature?: number;
  tool_resources?: {
    file_search?: {
      vector_store_ids?: string[];
    };
    [key: string]: any;
  };
  metadata?: Record<string, any>;
  response_format?: "auto" | string;
}

interface IAgentPreviewProps {
  resourceId: string;
  agentDetails: IAgent;
}

interface IAnnotation {
  file_name?: string;
  text: string;
  start_index: number;
  end_index: number;
}

const preprocessContent = (
  content: string,
  annotations?: IAnnotation[]
): string => {
  if (annotations) {
    // Process annotations in reverse order so that the indexes remain valid
    annotations
      .slice()
      .reverse()
      .forEach((annotation) => {
        // If there's a file_name, show it (wrapped in brackets), otherwise fall back to annotation.text.
        const linkText = annotation.file_name
          ? `[${annotation.file_name}]`
          : annotation.text;

        content =
          content.slice(0, annotation.start_index) +
          linkText +
          content.slice(annotation.end_index);
      });
  }
  return content;
};

// Vite provides the correct type for import.meta.env, but for strict TS, add a global type declaration if needed.
// Add at the top of the file (or in a global .d.ts file):
// declare var import.meta: { env: { VITE_API_BASE_URL: string } };

// Add a type declaration for import.meta.env for Vite compatibility
// (This can also go in a global .d.ts file, but is safe here for now)
interface ImportMeta {
  env: {
    VITE_API_BASE_URL: string;
    [key: string]: any;
  };
}

const apiBase: string = import.meta.env.VITE_API_BASE_URL;

const CHAT_SESSION_KEY = "chat_session_info";

const saveChatSession = (agentId: string, threadId: string) => {
  localStorage.setItem(CHAT_SESSION_KEY, JSON.stringify({ agentId, threadId }));
};

const getChatSession = () => {
  const data = localStorage.getItem(CHAT_SESSION_KEY);
  if (!data) return null;
  try {
    return JSON.parse(data);
  } catch {
    return null;
  }
};

export function AgentPreview({ agentDetails }: IAgentPreviewProps): ReactNode {
  const [messageList, setMessageList] = useState<IChatItem[]>([]);
  const [isResponding, setIsResponding] = useState(false);
  const [isLoadingChatHistory, setIsLoadingChatHistory] = useState(true);

  // Add newThread handler to clear chat
  const newThread = useCallback(async () => {
    // Optionally, call an API endpoint to start a new thread if your backend supports it
    setMessageList([]);
    setIsLoadingChatHistory(false);
    localStorage.removeItem(CHAT_SESSION_KEY);
  }, []);

  const loadChatHistory = useCallback(async () => {
    try {
      const session = getChatSession();
      let url = `${API_BASE_URL}/api/chat/history`;
      if (session && session.agentId && session.threadId) {
        url = `${API_BASE_URL}/api/chat/history?agentId=${encodeURIComponent(
          session.agentId
        )}&threadId=${encodeURIComponent(session.threadId)}`;
      }
      const response = await fetch(url, {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
        },
        credentials: "include",
      });

      if (response.ok) {
        const json_response: Array<{
          role: string;
          content: string;
          created_at: string;
          annotations?: IAnnotation[];
        }> = await response.json();

        // It's generally better to build the new list and set state once
        const historyMessages: IChatItem[] = [];
        const reversedResponse = [...json_response].reverse();

        for (const entry of reversedResponse) {
          if (entry.role === "user") {
            historyMessages.push({
              id: `${entry.created_at}-user`,
              role: "user",
              content: entry.content,
              isAnswer: false,
              more: { time: entry.created_at },
            });
          } else if (entry.role === "assistant") {
            historyMessages.push({
              id: `${entry.created_at}-assistant`,
              role: "assistant",
              content: preprocessContent(entry.content, entry.annotations),
              isAnswer: true,
              annotations: entry.annotations,
              more: { time: entry.created_at },
            });
          }
        }
        setMessageList(historyMessages);
      } else {
        setMessageList([]);
      }
    } catch (error) {
      setMessageList([]);
    } finally {
      setIsLoadingChatHistory(false);
    }
  }, []); // apiBase is a constant, not a React state/prop

  useEffect(() => {
    loadChatHistory();
  }, [loadChatHistory]);

  const chatContext = useMemo(
    () => ({
      messageList,
      isResponding,
      onSend: async (message: string) => {
        setIsResponding(true);

        const session = getChatSession();

        const response = await fetch(`${API_BASE_URL}/api/chat/send`, {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify({
            message: message,
            agentName: agentDetails.name,
            agentId: session?.agentId,
            threadId: session?.threadId,
          }),
          credentials: "include",
        });
        if (response.ok) {
          const data = await response.json();
          if (data && data.agentId && data.threadId) {
            saveChatSession(data.agentId, data.threadId);
          }
        }
        setIsResponding(false);
        loadChatHistory();
      },
      onSubmit: async (message: string) => {
        setIsResponding(true);
        const session = getChatSession();
        const response = await fetch(`${API_BASE_URL}/api/chat/send`, {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify({
            message: message,
            agentName: agentDetails.name,
            agentId: session?.agentId,
            threadId: session?.threadId,
          }),
          credentials: "include",
        });
        if (response.ok) {
          const data = await response.json();
          if (data && data.agentId && data.threadId) {
            saveChatSession(data.agentId, data.threadId);
          }
        }
        setIsResponding(false);
        loadChatHistory();
      },
      onDelete: async (id: string) => {
        await fetch(`${API_BASE_URL}/api/chat/delete/${id}`, {
          method: "DELETE",
          credentials: "include",
        });
        loadChatHistory();
      },
    }),
    [messageList, isResponding, loadChatHistory]
  );

  return (
    <div className={styles.container}>
      <div className={styles.topBar}>
        <div className={styles.leftSection}>
          <div className={styles.logoContainer}>
            <span className={styles.bikeIcon}>🚲</span>
          </div>
          <div className={styles.titleContainer}>
            {/* <span className={styles.companyName}>Contoso Bike Store</span> */}
            <span className={styles.companyName}>{agentDetails.displayName || "AI Assistant"}</span>
          </div>
        </div>
        <div className={styles.rightSection}>
        </div>
      </div>
      <div className={styles.content}>
        {isLoadingChatHistory ? (
          <Spinner />
        ) : (
          <>
            {messageList.length === 0 && (
              <div className={styles.emptyChatContainer}>
                <AgentIcon
                  alt=""
                  iconClassName={styles.emptyStateAgentIcon}
                  iconName={agentDetails.metadata?.logo}
                />

                {agentDetails.displayName && (
                  <Title2 as="h2" className={styles.emptyStateTitle}>
                    Welcome to {agentDetails.displayName}
                  </Title2>
                )}

                <div className={styles.emptyStateDescription}>
                  This agent provides customer support for Contoso Bike Store. It assists users with product inquiries, order status, and store information. The agent utilizes the <strong>MCP</strong> tools to effectively address and resolve customer questions.
                </div>
                
                <div className={styles.suggestedPrompts}>
                  <p className={styles.suggestedPromptsLabel}>Try asking about:</p>
                  <div className={styles.promptButtons}>
                    <Button appearance="outline" className={styles.promptButton}
                      onClick={() => chatContext.onSubmit("What bikes do you have available?")}>
                      Available bikes
                    </Button>
                    <Button appearance="outline" className={styles.promptButton}
                      onClick={() => chatContext.onSubmit("What accessories do you recommend?")}>
                      Bike accessories
                    </Button>
                    <Button appearance="outline" className={styles.promptButton}
                      onClick={() => chatContext.onSubmit("Help me find the right bike for me")}>
                      Find the right bike
                    </Button>
                  </div>
                </div>
                
                <div className={styles.emptyStateFooter}>
                  <Title2 as="h3">
                    How can I assist with your biking needs today?
                  </Title2>
                </div>
              </div>
            )}
            <div className={styles.chatControlWrapper}>
              <AgentPreviewChatBot
                agentName={agentDetails.name}
                agentLogo={agentDetails.name}
                chatContext={chatContext}
              />
              <Button
                appearance="primary"
                icon={<ChatRegular aria-hidden={true} />}
                className={styles.newChatButton}
                onClick={newThread}
              >
                New Chat
              </Button>
            </div>
          </>
        )}
      </div>
    </div>
  );
}
