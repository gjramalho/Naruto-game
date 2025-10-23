#!/usr/bin/env node
// Diagnóstico simples para verificar se os arquivos de PostCSS podem ser lidos
// Executar: node tools/checkPostCSS.js
import fs from 'fs/promises';
import path from 'path';

const root = process.cwd();
const files = [
  path.join(root, 'postcss.config.js'),
  path.join(root, 'postcss.config.cjs'),
  path.join(root, 'src', 'index.css'),
  path.join(root, 'nome-do-projeto', 'src', 'index.css'),
];

async function check(file) {
  try {
    const stat = await fs.stat(file);
    console.log(`OK: ${file} -> size=${stat.size} bytes, mode=${(stat.mode & 0o777).toString(8)}`);
    const content = await fs.readFile(file, { encoding: 'utf8' });
    console.log(`  Preview: ${content.slice(0, 200).replace(/\n/g, ' ')}\n`);
  } catch (err) {
    console.error(`ERR: ${file} -> ${err.code || err.message}`);
  }
}

(async () => {
  console.log('Running PostCSS read checks from', root);
  for (const f of files) {
    await check(f);
  }
})();
