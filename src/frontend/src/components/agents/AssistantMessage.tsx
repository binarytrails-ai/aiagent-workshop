// ...existing code from frontend/src/components/agents/AssistantMessage.tsx...
import { Button, Spinner } from "@fluentui/react-components";
import { bundleIcon, DeleteFilled, DeleteRegular } from "@fluentui/react-icons";
import { CopilotMessageV2 as CopilotMessage } from "@fluentui-copilot/react-copilot-chat";
import { Suspense } from "react";
import { Markdown } from "../core/Markdown";
import { UsageInfo } from "./UsageInfo";
import { IAssistantMessageProps } from "./chatbot/types";
import styles from "./AgentPreviewChatBot.module.css";
import { AgentIcon } from "./AgentIcon";
const DeleteIcon = bundleIcon(DeleteFilled, DeleteRegular);
export function AssistantMessage({ message, agentLogo, loadingState, agentName, showUsageInfo, onDelete }: IAssistantMessageProps): React.JSX.Element {
  return (
    <CopilotMessage
      id={"msg-" + message.id}
      key={message.id}
      actions={
        <span>
          {onDelete && message.usageInfo && (
            <Button
              appearance="subtle"
              icon={<DeleteIcon />}
              onClick={() => { void onDelete(message.id); }}
            />
          )}
        </span>
      }
      className={styles.assistantMessageContainer}
      avatar={<AgentIcon iconName={agentLogo} alt={agentName || "Agent"} />}
      name={agentName || ""}
      loadingState={loadingState}
    >
      <Suspense fallback={<Spinner size="small" />}>
        <Markdown content={message.content} />
      </Suspense>
      {showUsageInfo && message.usageInfo && <UsageInfo info={message.usageInfo} duration={message.duration} />}
    </CopilotMessage>
  );
}
