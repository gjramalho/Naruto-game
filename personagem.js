// Script específico para a página Personagem - carrega e salva dados do jogador
// Não importamos './script.js' aqui porque ele já é carregado pela página via <script>

// Se a instância global já existir, usamos ela; caso contrário, usamos undefined e apenas atualizamos o storage
const app = window.narutoGame;

function updateFromStorage() {
  try {
    const raw = localStorage.getItem('narutoGameData');
    if (!raw) return;
    const data = JSON.parse(raw);
    const player = data.player || {};

  // Apenas atualiza habilidades — nome/vila/elemento foram removidos desta página

    ['ninjutsu','taijutsu','genjutsu'].forEach(skill => {
      const levelEl = document.getElementById(`${skill}Level`);
      const barEl = document.getElementById(`${skill}Bar`);
      if (levelEl && player.skills && player.skills[skill] != null) levelEl.textContent = player.skills[skill];
      if (barEl && player.skills && player.skills[skill] != null) barEl.style.width = `${(player.skills[skill]/10)*100}%`;
    });
  } catch (e) {
    console.error('Erro ao carregar personagem:', e);
  }
}

function saveToStorage() {
  try {
    const raw = localStorage.getItem('narutoGameData');
    const data = raw ? JSON.parse(raw) : { player: {} };
    data.player = data.player || {};

    // Atualizar somente as skills vindas do UI
    data.player = data.player || {};
    data.player.skills = data.player.skills || {};
    ['ninjutsu','taijutsu','genjutsu'].forEach(skill => {
      const levelEl = document.getElementById(`${skill}Level`);
      if (levelEl) data.player.skills[skill] = parseInt(levelEl.textContent, 10) || data.player.skills[skill] || 0;
    });

    localStorage.setItem('narutoGameData', JSON.stringify(data));
    // Atualizar a instância principal, se existir
    if (app && typeof app.updatePlayerDisplay === 'function') app.updatePlayerDisplay();
    if (app && typeof app.updateProfileDisplay === 'function') app.updateProfileDisplay();
    alert('Personagem salvo!');
  } catch (e) {
    console.error('Erro ao salvar personagem:', e);
  }
}

window.addEventListener('DOMContentLoaded', () => {
  updateFromStorage();
  const btn = document.getElementById('saveProfile');
  if (btn) btn.addEventListener('click', saveToStorage);
});