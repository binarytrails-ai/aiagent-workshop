// ...existing code from frontend/src/components/agents/AgentIcon.tsx...
import type { ReactNode } from "react";
import styles from "./AgentIcon.module.css";
export interface IAgentIconProps {
  iconName?: string;
  alt: string;
  iconClassName?: string;
}
export function AgentIcon({
  iconName = "Avatar_Default.svg",
  iconClassName,
  alt = "",
}: IAgentIconProps): ReactNode {
  return (
    <div className={styles.iconContainer}>
      <img
        alt={alt}
        className={iconClassName ?? styles.icon}
        src={`/assets/template-images/Avatar_Default.svg`}
      />
    </div>
  );
}
