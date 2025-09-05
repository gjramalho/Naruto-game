import { defineConfig } from 'vite';

export default defineConfig({
  root: '.',
  server: { port: 5173, open: 'index.html' },
  build: {
    outDir: 'dist',
    emptyOutDir: true,
  },
});
