// ...existing code from frontend/src/components/core/theme/useThemeProvider.ts...
import { useCallback, useEffect, useState } from "react";
import { IThemeContextValue, Theme } from "./ThemeContext";
import { darkTheme, lightTheme } from "./themes";
const LOCAL_STORAGE_THEME_KEY = "get-started-with-agents-app-theme-preference";
export function useMediaQuery(query: string): boolean {
  const [matches, setMatches] = useState(() => {
    if (typeof window !== "undefined") {
      return window.matchMedia(query).matches;
    }
    return false;
  });
  useEffect(() => {
    if (typeof window !== "undefined") {
      const mediaQuery = window.matchMedia(query);
      const updateMatches = () => {
        setMatches(mediaQuery.matches);
      };
      mediaQuery.addEventListener("change", updateMatches);
      updateMatches();
      return () => {
        mediaQuery.removeEventListener("change", updateMatches);
      };
    }
    return () => {
      // Cleanup if needed
    };
  }, [query]);
  return matches;
}
export const useThemeProvider = (): IThemeContextValue => {
  // We're forcing light mode for this application
  const prefersDark = false;

  const [savedTheme, setSavedTheme] = useState<Theme>("Light");

  // Force light mode
  const isDarkMode = false;
  const currentTheme = "Light";
  const themeStyles = lightTheme;

  const setTheme = useCallback((newTheme: Theme) => {
    setSavedTheme(newTheme);
    if (typeof localStorage !== "undefined") {
      localStorage.setItem(LOCAL_STORAGE_THEME_KEY, newTheme);
    }
  }, []);

  useEffect(() => {
    // Update document color scheme for browser UI
    if (typeof document !== "undefined") {
      document.documentElement.style.colorScheme = isDarkMode
        ? "dark"
        : "light";
    }
  }, [isDarkMode]);

  return {
    theme: savedTheme,
    savedTheme,
    currentTheme,
    themeStyles,
    setTheme,
    isDarkMode,
  };
};
// ...existing code...
