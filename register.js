// Cadastro Ninja - lógica de validação e fluxo
// - Valida email/senha/nickname
// - Persiste flag de cadastro e redireciona para login
// - Aplica tema salvo e permite alternar com persistência

// Simulação de nicknames já cadastrados
const usedNicknames = ['naruto', 'sasuke', 'sakura', 'kakashi'];

function validateEmail(email) {
  return /^[\w-.]+@[\w-]+\.[a-z]{2,}$/i.test(email);
}

function validatePassword(password) {
  return /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$/.test(password);
}

document.addEventListener('DOMContentLoaded', () => {
  // Aplicar tema salvo
  const savedTheme = localStorage.getItem('narutoGameTheme');
  const body = document.body;
  body.classList.remove('naruto-theme', 'dark-theme', 'akatsuki-theme');
  const themeIcon = document.getElementById('registerThemeIcon');
  if (savedTheme === 'dark') {
    body.classList.add('akatsuki-theme');
    if (themeIcon) themeIcon.textContent = '☀️';
  } else {
    body.classList.add('naruto-theme');
    if (themeIcon) themeIcon.textContent = '🌙';
  }

  // Toggle tema no register
  const toggleBtn = document.getElementById('registerThemeToggle');
  if (toggleBtn) {
    toggleBtn.addEventListener('click', () => {
      const isDark = document.body.classList.contains('akatsuki-theme');
      if (isDark) {
        document.body.classList.remove('akatsuki-theme');
        document.body.classList.add('naruto-theme');
        if (themeIcon) themeIcon.textContent = '🌙';
        localStorage.setItem('narutoGameTheme', 'light');
      } else {
        document.body.classList.remove('naruto-theme', 'dark-theme');
        document.body.classList.add('akatsuki-theme');
        if (themeIcon) themeIcon.textContent = '☀️';
        localStorage.setItem('narutoGameTheme', 'dark');
      }
    });
  }

  const form = document.getElementById('registerForm');
  if (!form) return;

  form.addEventListener('submit', function (e) {
    e.preventDefault();
    const email = document.getElementById('email').value.trim();
    const nickname = document.getElementById('nickname').value.trim().toLowerCase();
    const password = document.getElementById('password').value;
    const confirmPassword = document.getElementById('confirmPassword').value;
    const error = document.getElementById('registerError');
    const success = document.getElementById('registerSuccess');
    if (!error || !success) return;

    error.classList.add('hidden');
    success.classList.add('hidden');

    let msg = '';
    if (!validateEmail(email)) {
      msg = 'Insira um e-mail válido.';
    } else if (nickname.length < 3) {
      msg = 'Nome ninja precisa ter pelo menos 3 caracteres.';
    } else if (usedNicknames.includes(nickname)) {
      msg = 'Este nome ninja já existe.';
    } else if (!validatePassword(password)) {
      msg = 'Senha fraca. Use 8+ caracteres, 1 maiúscula e 1 número.';
    } else if (password !== confirmPassword) {
      msg = 'As senhas não coincidem.';
    }

    if (msg) {
      error.querySelector('span').textContent = msg;
      error.classList.remove('hidden');
    } else {
      success.querySelector('span').textContent = 'Cadastro concluído. Redirecionando para login...';
      success.classList.remove('hidden');
      localStorage.setItem('narutoGameRegistered', 'true');
      setTimeout(() => {
        window.location.href = 'login.html';
      }, 1500);
    }
  });
});
