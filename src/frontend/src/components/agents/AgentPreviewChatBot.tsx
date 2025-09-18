// ...existing code from frontend/src/components/agents/AgentPreviewChatBot.tsx...
import React, { useState, useMemo } from "react";
import { AssistantMessage } from "./AssistantMessage";
import { UserMessage } from "./UserMessage";
import { ChatInput } from "./chatbot/ChatInput";
import { AgentPreviewChatBotProps } from "./chatbot/types";
import styles from "./AgentPreviewChatBot.module.css";
import clsx from "clsx";
export function AgentPreviewChatBot({ agentName, agentLogo, chatContext }: AgentPreviewChatBotProps): React.JSX.Element {
  const [currentUserMessage, setCurrentUserMessage] = useState<string | undefined>();
  const messageListFromChatContext = useMemo(() => chatContext.messageList ?? [], [chatContext.messageList]);
  const onEditMessage = (messageId: string) => {
    const selectedMessage = messageListFromChatContext.find((message) => !message.isAnswer && message.id === messageId)?.content;
    setCurrentUserMessage(selectedMessage);
  };
  const isEmpty = messageListFromChatContext.length === 0;
  return (
    <div className={styles.chatContainer}>
      {!isEmpty && (
        <div className={styles.copilotChatContainer}>
          {messageListFromChatContext.map((message, index, messageList) =>
            message.isAnswer ? (
              <AssistantMessage
                key={message.id}
                agentLogo={agentLogo}
                agentName={agentName}
                loadingState={index === messageList.length - 1 && chatContext.isResponding ? "loading" : undefined}
                message={message}
                showUsageInfo={true}
                onDelete={chatContext.onDelete}
              />
            ) : (
              <UserMessage
                key={message.id}
                message={message}
                onEditMessage={onEditMessage}
              />
            )
          )}
        </div>
      )}
      <ChatInput onSubmit={chatContext.onSubmit} isGenerating={chatContext.isResponding} currentUserMessage={currentUserMessage} />
    </div>
  );
}
