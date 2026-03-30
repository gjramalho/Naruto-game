// Auth util: integração com backend API
(function(){
  // Verifica se a API está disponível
  function isApiAvailable() {
    return typeof window.apiService !== 'undefined';
  }

  // Registra um novo usuário via API
  async function createUser({email, nickname, password, village, element}) {
    if (!isApiAvailable()) {
      return { ok: false, error: 'Serviço de API não disponível. Verifique se o backend está rodando.' };
    }

    // Gerar username a partir do nickname
    const username = nickname.toLowerCase().replace(/\s+/g, '_');

    const result = await window.apiService.register({
      username,
      email,
      password,
      nickname,
      village: village || 'Indefinido',
      element: element || 'Indefinido'
    });

    if (!result.success) {
      return { ok: false, error: result.error };
    }

    return { ok: true, user: result.data };
  }

  // Autentica usuário via API
  async function verifyLogin(identifier, password) {
    if (!isApiAvailable()) {
      return { ok: false, error: 'Serviço de API não disponível. Verifique se o backend está rodando.' };
    }

    const result = await window.apiService.login({
      username: identifier,
      password
    });

    if (!result.success) {
      return { ok: false, error: result.error };
    }

    // Salvar dados do usuário no localStorage para compatibilidade
    if (result.data.user) {
      localStorage.setItem('narutoLoggedNickname', result.data.user.nickname || identifier);
    }

    return { ok: true, user: result.data.user, token: result.data.token };
  }

  // Verifica se o usuário está autenticado
  function isAuthenticated() {
    if (!isApiAvailable()) {
      return localStorage.getItem('narutoGameLogged') === 'true' || sessionStorage.getItem('narutoSession') === 'true';
    }
    return window.apiService.isAuthenticated();
  }

  // Realiza logout
  function logout() {
    if (isApiAvailable()) {
      window.apiService.logout();
    }
    localStorage.removeItem('narutoGameLogged');
    sessionStorage.removeItem('narutoSession');
    localStorage.removeItem('narutoLoggedNickname');
  }

  // Obtém perfil do jogador via API
  async function getPlayerProfile() {
    if (!isApiAvailable()) {
      return { ok: false, error: 'Serviço de API não disponível.' };
    }

    return await window.apiService.getPlayerProfile();
  }

  // Atualiza perfil do jogador via API
  async function updatePlayerProfile(profileData) {
    if (!isApiAvailable()) {
      return { ok: false, error: 'Serviço de API não disponível.' };
    }

    return await window.apiService.updatePlayerProfile(profileData);
  }

  // Adiciona XP via API
  async function addXp(amount) {
    if (!isApiAvailable()) {
      return { ok: false, error: 'Serviço de API não disponível.' };
    }

    return await window.apiService.addXp(amount);
  }

  // Atualiza habilidades via API
  async function updateSkills(skills) {
    if (!isApiAvailable()) {
      return { ok: false, error: 'Serviço de API não disponível.' };
    }

    return await window.apiService.updateSkills(skills);
  }

  // Exportar funções globalmente
  window.narutoAuth = {
    createUser,
    verifyLogin,
    isAuthenticated,
    logout,
    getPlayerProfile,
    updatePlayerProfile,
    addXp,
    updateSkills
  };
})();
