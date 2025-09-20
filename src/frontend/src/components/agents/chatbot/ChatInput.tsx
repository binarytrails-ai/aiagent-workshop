// ...existing code from frontend/src/components/agents/chatbot/ChatInput.tsx...
import React, { useState, useEffect, useRef } from "react";
import { ChatInput as ChatInputFluent, ImperativeControlPlugin, ImperativeControlPluginRef } from "@fluentui-copilot/react-copilot";
import { ChatInputProps } from "./types";
export const ChatInput: React.FC<ChatInputProps> = ({ onSubmit, isGenerating, currentUserMessage }) => {
  const [inputText, setInputText] = useState<string>("");
  const controlRef = useRef<ImperativeControlPluginRef>(null);
  useEffect(() => {
    if (currentUserMessage !== undefined) {
      controlRef.current?.setInputText(currentUserMessage ?? "");
    }
  }, [currentUserMessage]);
  const onMessageSend = (text: string): void => {
    if (text && text.trim() !== "") {
      onSubmit(text.trim());
      setInputText("");
      controlRef.current?.setInputText("");
    }
  };
  return (
    <div style={{
      marginTop: '16px',
      borderTop: '1px solid var(--colorNeutralStroke1)',
      paddingTop: '16px',
      width: '100%'
    }}>
      <ChatInputFluent
        aria-label="Chat Input"
        charactersRemainingMessage={(_value: number) => ``}
        data-testid="chat-input"
        disableSend={isGenerating}
        history={true}
        isSending={isGenerating}
        onChange={(
          _: React.ChangeEvent<HTMLInputElement>,
          d: { value: string }
        ) => {
          setInputText(d.value);
        }}
        onSubmit={() => {
          onMessageSend(inputText ?? "");
        }}
        placeholderValue="Ask me about Contoso bikes, accessories, or assistance..."
        style={{
          borderRadius: 'var(--borderRadiusLarge)',
          boxShadow: '0 2px 8px rgba(0, 0, 0, 0.08)',
          background: 'var(--colorNeutralBackground1)',
        }}
      >
        <ImperativeControlPlugin ref={controlRef} />
      </ChatInputFluent>
      {isGenerating && (
        <div style={{ 
          textAlign: 'center', 
          padding: '8px', 
          color: 'var(--colorBrandForeground1)', 
          fontSize: '0.9rem',
          fontStyle: 'italic',
          marginTop: '8px'
        }}>
          Contoso Bike Expert is thinking...
        </div>
      )}
    </div>
  );
};
