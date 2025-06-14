import type {
  AnchorHTMLAttributes,
  ClassAttributes
} from "react";
import { memo, useMemo } from "react";
import ReactMarkdown from "react-markdown";
import RemarkBreaks from "remark-breaks";
import RemarkGfm from "remark-gfm";
import RemarkMath from "remark-math";
import remarkParse from "remark-parse";
import supersub from "remark-supersub";
import RehypeKatex from "rehype-katex";
import RehypeRaw from "rehype-raw";
import rehypeSanitize, { defaultSchema } from "rehype-sanitize";
import rehypeStringify from "rehype-stringify";
import styles from "./Markdown.module.css";

interface ICitation extends React.JSX.Element {
  props: {
    "data-replace"?: string;
    [key: string]: unknown;
  };
}

interface IMarkdownProps {
  citations?: ICitation[] | undefined;
  content: string;
  className?: string;
  customDisallowedElements?: string[];
}

function Hyperlink({
  node,
  children,
  ...linkProps
}: ClassAttributes<HTMLAnchorElement> &
  AnchorHTMLAttributes<HTMLAnchorElement> &
  any) {
  return (
    <a
      href={node?.properties.href?.toString() ?? ""}
      target="_blank"
      rel="noopener noreferrer"
      className={styles.link}
      {...linkProps}
    >
      {children}
    </a>
  );
}

// Preprocesses LaTeX notation to standard math notation for the markdown parser
const preprocessLaTeX = (content: string): string => {
  if (typeof content !== "string") {
    return content;
  }
  // Convert \[ ... \] to $$ ... $$
  let result = content.replaceAll(
    /\\\[(.*?)\\\]/g,
    (_: string, equation: string) => `$$${equation}$$`
  );
  // Convert \( ... \) to $ ... $
  result = result.replaceAll(
    /\\\((.*?)\\\)/g,
    (_: string, equation: string) => `$${equation}$$`
  );
  return result;
};

export const Markdown = memo(function Markdown({
  citations,
  content,
  className,
  customDisallowedElements,
}: IMarkdownProps) {
  const processedContent = useMemo(
    () => preprocessLaTeX(content),
    [content]
  );

  return (
    <div className={className ?? styles.markdown}>
      <ReactMarkdown
        components={{
          a: Hyperlink,
        }}
        remarkPlugins={[
          remarkParse,
          RemarkBreaks,
          RemarkGfm,
          RemarkMath,
          supersub,
        ]}
        rehypePlugins={[
          RehypeRaw,
          RehypeKatex,
          [rehypeSanitize, { schema: defaultSchema }],
          rehypeStringify,
        ]}
      >
        {processedContent}
      </ReactMarkdown>
      {citations && (
        <div className={styles.citations}>
          {citations.map((citation, index) => (
            <div key={index} className={styles.citation}>
              {citation.props["data-replace"]}
            </div>
          ))}
        </div>
      )}
    </div>
  );
});
