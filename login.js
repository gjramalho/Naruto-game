// Login e seleção de personagem: aplica tema salvo e persiste escolha
document.addEventListener('DOMContentLoaded', () => {
  // Aplicar tema salvo (naruto/dark/akatsuki como em index)
  // Lê e aplica tema salvo (naruto|akatsuki)
  const savedTheme = localStorage.getItem('narutoGameTheme');
  const body = document.body;
  body.classList.remove('naruto-theme', 'dark-theme', 'akatsuki-theme');
  const initialIcon = document.getElementById('loginThemeIcon');
  if (savedTheme === 'dark') {
    body.classList.add('akatsuki-theme');
    if (initialIcon) initialIcon.textContent = '☀️';
  } else {
    body.classList.add('naruto-theme');
    if (initialIcon) initialIcon.textContent = '🌙';
  }
  const form = document.getElementById('loginForm');
  const error = document.getElementById('loginError');
  const info = document.getElementById('loginInfo');
  const characterSelect = document.getElementById('characterSelect');

  // Se o usuário já estiver logado, ir para index diretamente
  // Se já logado, vá direto ao index
  if (localStorage.getItem('narutoGameLogged') === 'true') {
    window.location.href = 'index.html';
    return;
  }

  // Mensagem pós-cadastro (via query param ?registered=1)
  try {
    const qp = new URLSearchParams(window.location.search);
    if (qp.get('registered') === '1' && info) {
      info.textContent = 'Conta criada! Faça login para continuar.';
      info.classList.remove('hidden');
    }
  } catch {}

  form.addEventListener('submit', async (e) => {
    e.preventDefault();
    const username = document.getElementById('username').value.trim();
    const password = document.getElementById('password').value.trim();
    const remember = document.getElementById('rememberMe')?.checked;
    const userInput = document.getElementById('username');
    const passInput = document.getElementById('password');

    // Rate limit simples (3 tentativas -> cooldown 30s)
    const ATT_KEY = 'narutoLoginAttempts';
    const BLK_KEY = 'narutoLoginBlockedUntil';
    const now = Date.now();
    const blockedUntil = parseInt(localStorage.getItem(BLK_KEY) || '0', 10);
    if (blockedUntil && now < blockedUntil) {
      const secs = Math.ceil((blockedUntil - now) / 1000);
      if (error) {
        error.textContent = `Muitas tentativas. Tente novamente em ${secs}s.`;
        error.classList.remove('hidden');
      }
      return;
    }
    if (!username || !password) {
      error.textContent = 'Preencha usuário e senha.';
      error.classList.remove('hidden');
      userInput?.classList.toggle('input-error', !username);
      passInput?.classList.toggle('input-error', !password);
      return;
    }

    if (!window.narutoAuth) {
      error.textContent = 'Erro de autenticação. Recarregue a página.';
      error.classList.remove('hidden');
      return;
    }

    const result = await window.narutoAuth.verifyLogin(username, password);
    if (!result.ok) {
      error.textContent = result.error || 'Usuário ou senha inválidos. Crie sua conta.';
      error.classList.remove('hidden');
      userInput?.classList.add('input-error');
      passInput?.classList.add('input-error');
      try {
        const attempts = parseInt(localStorage.getItem(ATT_KEY) || '0', 10) + 1;
        localStorage.setItem(ATT_KEY, String(attempts));
        if (attempts >= 3) {
          localStorage.setItem(BLK_KEY, String(now + 30000)); // 30s
          localStorage.removeItem(ATT_KEY);
        }
      } catch {}
      return;
    }

    // Autenticado: marcar sessão/lembrar
    try {
      if (remember) {
        localStorage.setItem('narutoGameLogged', 'true');
      } else {
        sessionStorage.setItem('narutoSession', 'true');
      }
      // Guardar nickname da conta logada
      if (result.user?.nickname) {
        localStorage.setItem('narutoLoggedNickname', result.user.nickname);
      }
      // Reset de rate-limit ao sucesso
      localStorage.removeItem(ATT_KEY);
      localStorage.removeItem(BLK_KEY);
      userInput?.classList.remove('input-error');
      passInput?.classList.remove('input-error');
    } catch {}

    // Prosseguir para seleção de personagem
    const loginCard = document.querySelector('.glass-card-dark');
    if (loginCard) loginCard.classList.add('hidden');
    if (characterSelect) characterSelect.classList.remove('hidden');
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
        // Garante estado de login (mantém remember/sessão)
        if (!localStorage.getItem('narutoGameLogged') && !sessionStorage.getItem('narutoSession')) {
          sessionStorage.setItem('narutoSession', 'true');
        }
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
