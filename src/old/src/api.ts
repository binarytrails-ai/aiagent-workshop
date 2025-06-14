/// <reference types="vite/client" />

// Declare global types for ImportMeta
declare global {
  interface ImportMeta {
    env: {
      VITE_API_BASE_URL: string;
      [key: string]: any;
    };
  }
}

// Centralized API base URL utility
export const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;