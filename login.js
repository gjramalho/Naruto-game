// Login e seleção de personagem: aplica tema salvo e persiste escolha
document.addEventListener('DOMContentLoaded', () => {
  // Aplicar tema salvo (naruto/dark/akatsuki como em index)
  // Lê e aplica tema salvo (naruto|akatsuki)
  const savedTheme = localStorage.getItem('narutoGameTheme');
  const body = document.body;
  body.classList.remove('naruto-theme', 'dark-theme', 'akatsuki-theme');
  if (savedTheme === 'dark') {
    body.classList.add('akatsuki-theme');
  } else {
    body.classList.add('naruto-theme');
  }
  const form = document.getElementById('loginForm');
  const error = document.getElementById('loginError');
  const characterSelect = document.getElementById('characterSelect');

  // Se o usuário já estiver logado, ir para index diretamente
  // Se já logado, vá direto ao index
  if (localStorage.getItem('narutoGameLogged') === 'true') {
    window.location.href = 'index.html';
    return;
  }

  form.addEventListener('submit', (e) => {
    e.preventDefault();
    const username = document.getElementById('username').value.trim();
    const password = document.getElementById('password').value.trim();
    if (username && password) {
      // Esconde o card de login e mostra seleção de personagem
      const loginCard = document.querySelector('.glass-card-dark');
      if (loginCard) loginCard.classList.add('hidden');
      if (characterSelect) characterSelect.classList.remove('hidden');
    } else {
      error.classList.remove('hidden');
    }
  });

  // Seleção de personagem
  let selected = null;
  document.querySelectorAll('.character-card').forEach((card) => {
    card.addEventListener('click', () => {
      document.querySelectorAll('.character-card').forEach((c) => c.classList.remove('selected'));
      card.classList.add('selected');
      selected = card.getAttribute('data-character');
      const btn = document.getElementById('confirmCharacter');
      if (btn) btn.disabled = false;
    });
  });

  const confirmBtn = document.getElementById('confirmCharacter');
  if (confirmBtn) {
    confirmBtn.addEventListener('click', () => {
      if (selected) {
        localStorage.setItem('narutoGameLogged', 'true');
        localStorage.setItem('narutoGameCharacter', selected);
        window.location.href = 'index.html';
      }
    });
  }

  // Toggle do tema no login
  const loginToggle = document.getElementById('loginThemeToggle');
  const loginIcon = document.getElementById('loginThemeIcon');
  if (loginToggle) {
    loginToggle.addEventListener('click', () => {
      const isDark = document.body.classList.contains('akatsuki-theme');
      if (isDark) {
        document.body.classList.remove('akatsuki-theme');
        document.body.classList.add('naruto-theme');
        if (loginIcon) loginIcon.textContent = '🌙';
        localStorage.setItem('narutoGameTheme', 'light');
      } else {
        document.body.classList.remove('naruto-theme', 'dark-theme');
        document.body.classList.add('akatsuki-theme');
        if (loginIcon) loginIcon.textContent = '☀️';
        localStorage.setItem('narutoGameTheme', 'dark');
      }
    });
  }
});
