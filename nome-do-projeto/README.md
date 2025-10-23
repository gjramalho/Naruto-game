# React + Vite

Este template fornece uma configuração mínima para fazer o React funcionar com Vite, incluindo HMR (Hot Module Replacement) e algumas regras do ESLint.

Atualmente, dois plugins oficiais estão disponíveis:

- [@vitejs/plugin-react](https://github.com/vitejs/vite-plugin-react/blob/main/packages/plugin-react) — utiliza [Babel](https://babeljs.io/) (ou [oxc](https://oxc.rs) quando usado em [rolldown-vite](https://vite.dev/guide/rolldown)) para Fast Refresh.
- [@vitejs/plugin-react-swc](https://github.com/vitejs/vite-plugin-react/blob/main/packages/plugin-react/blob/main/packages/plugin-react-swc) — utiliza [SWC](https://swc.rs/) para Fast Refresh.

## React Compiler

O React Compiler não está habilitado neste template devido ao impacto no desempenho em desenvolvimento e na etapa de build. Para adicioná‑lo, consulte a documentação: https://react.dev/learn/react-compiler/installation

## Expandindo a configuração do ESLint

Se você estiver desenvolvendo uma aplicação para produção, recomendam‑se o uso do TypeScript com regras de lint conscientes de tipos habilitadas. Veja o [template com TS](https://github.com/vitejs/vite/tree/main/packages/create-vite/template-react-ts) para informações sobre como integrar TypeScript e o [`typescript-eslint`](https://typescript-eslint.io) ao projeto.