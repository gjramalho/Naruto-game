PR: Remoção de segredos e limpeza do repositório

Resumo
-----
Este PR remove segredos versionados, adiciona um template de `.env`, atualiza healthchecks do Docker e fornece instruções para limpar o histórico e rotacionar segredos.

Passos sugeridos para aplicar localmente (antes de abrir PR)
---------------------------------------------------------
1. Verifique arquivos que contenham segredos:

```bash
grep -nRE "password|passwd|secret|jwt|SA_PASSWORD|JWT_SECRET|YourSuperSecret|YourStrong" . || true
```

2. Remova os arquivos sensíveis do índice do git (não os apague localmente):

```bash
git rm --cached backend/.env
git commit -m "chore: stop tracking backend/.env and remove secrets"
```

3. Atualize `.gitignore` (já feito neste PR) e crie `backend/.env` localmente a partir do template:

```bash
cp backend/.env.template backend/.env
# editar backend/.env localmente para inserir segredos
```

4. Rotacione segredos críticos que possam ter sido expostos (SA_PASSWORD, JWT_SECRET):
   - Gere nova senha para `sa` e nova chave JWT.
   - Atualize seus serviços/CI/secret manager com os novos valores.

5. (Opcional, caso os segredos tenham sido publicamente expostos) Limpar histórico git:

Recomenda-se usar `git filter-repo` (mais moderno) ou `bfg` para remover arquivos do histórico. Exemplo com `git filter-repo`:

```bash
# instalar git-filter-repo
# executar a partir do diretório do repositório
git filter-repo --path backend/.env --invert-paths
```

ATENÇÃO: Reescrever o histórico exige coordenação com todos os colaboradores (force-push e instruções para atualizar forks/branches).

6. Adicionar detecção de segredos no CI / pre-commit hooks:
   - `detect-secrets`, `trufflehog` ou `git-secrets`
   - Configurar uma Action no CI para bloquear pushes com segredos.

Checklist após merge
--------------------
- [ ] Confirmação de que `backend/.env` foi rotacionado e removido do repositório
- [ ] CI atualiza e passa com as novas configurações
- [ ] Documentação para desenvolvedores explicando como obter segredos (secret manager ou local .env)
