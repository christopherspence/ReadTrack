declare global {
  interface ImportMeta {
    readonly env: { [key: string]: any };
  }
}

export {};