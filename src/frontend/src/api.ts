declare global {
  interface ImportMeta {
    env: {
      VITE_API_BASE_URL: string;
      [key: string]: any;
    };
  }
}
export const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;
