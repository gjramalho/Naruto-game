/** @type {import('tailwindcss').Config} */
export default {
  content: [
    './index.html',
    './login.html',
    './register.html',
    './*.js',
    './src/**/*.{html,js,ts,jsx,tsx}',
    '!./node_modules/**',
    '!./dist/**'
  ],
  theme: {
    extend: {}
  },
  plugins: []
};
