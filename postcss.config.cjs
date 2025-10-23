// Fallback CommonJS PostCSS config.
// Alguns loaders (ou ambientes) tentam `require()` o arquivo de config.
// Manter este arquivo em CJS evita falhas ao carregar plugins (Tailwind/autoprefixer).
module.exports = {
  plugins: {
    tailwindcss: {},
    autoprefixer: {},
  },
};
 