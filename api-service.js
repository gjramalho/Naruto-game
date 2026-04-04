// Serviço de API para comunicação com o backend Naruto Game
class ApiService {
    constructor() {
        this.baseUrl = API_CONFIG.baseUrl;
        this.endpoints = API_CONFIG.endpoints;
    }

    // ============================================================
    // MÉTODOS DE AUTENTICAÇÃO
    // ============================================================

    /**
     * Registra um novo usuário
     * @param {Object} userData - Dados do usuário (username, email, password, nickname, village, element)
     * @returns {Promise<Object>} Resultado da operação
     */
    async register(userData) {
        const result = await apiRequest(this.endpoints.auth.register, {
            method: 'POST',
            body: JSON.stringify(userData)
        });
        
        if (result.success) {
            console.log('✅ Usuário registrado com sucesso');
        }
        
        return result;
    }

    /**
     * Autentica o usuário e obtém token JWT
     * @param {Object} credentials - Credenciais (username, password)
     * @returns {Promise<Object>} Resultado da operação com token
     */
    async login(credentials) {
        const result = await apiRequest(this.endpoints.auth.login, {
            method: 'POST',
            body: JSON.stringify(credentials)
        });
        
        if (result.success && result.data.token) {
            API_CONFIG.setToken(result.data.token);
            console.log('✅ Login realizado com sucesso');
        }
        
        return result;
    }

    /**
     * Realiza logout removendo o token
     */
    logout() {
        API_CONFIG.removeToken();
        console.log('✅ Logout realizado');
    }

    // ============================================================
    // MÉTODOS DO JOGADOR
    // ============================================================

    /**
     * Obtém o perfil do jogador autenticado
     * @returns {Promise<Object>} Perfil do jogador
     */
    async getPlayerProfile() {
        return await apiRequest(this.endpoints.player.profile);
    }

    /**
     * Atualiza o perfil do jogador
     * @param {Object} profileData - Dados do perfil para atualizar
     * @returns {Promise<Object>} Resultado da operação
     */
    async updatePlayerProfile(profileData) {
        return await apiRequest(this.endpoints.player.profile, {
            method: 'PUT',
            body: JSON.stringify(profileData)
        });
    }

    /**
     * Adiciona XP ao jogador
     * @param {number} amount - Quantidade de XP a adicionar
     * @returns {Promise<Object>} Resultado da operação
     */
    async addXp(amount) {
        return await apiRequest(this.endpoints.player.xp, {
            method: 'POST',
            body: JSON.stringify({ amount })
        });
    }

    /**
     * Atualiza as habilidades do jogador
     * @param {Object} skills - Habilidades para atualizar
     * @returns {Promise<Object>} Resultado da operação
     */
    async updateSkills(skills) {
        return await apiRequest(this.endpoints.player.skills, {
            method: 'PUT',
            body: JSON.stringify(skills)
        });
    }

    // ============================================================
    // MÉTODOS DO JOGO
    // ============================================================

    /**
     * Lista todos os NPCs
     * @returns {Promise<Object>} Lista de NPCs
     */
    async getNpcs() {
        return await apiRequest(this.endpoints.game.npcs);
    }

    /**
     * Obtém um NPC específico pela chave
     * @param {string} key - Chave do NPC
     * @returns {Promise<Object>} Dados do NPC
     */
    async getNpcByKey(key) {
        return await apiRequest(this.endpoints.game.npcByKey(key));
    }

    /**
     * Lista todas as missões
     * @returns {Promise<Object>} Lista de missões
     */
    async getMissions() {
        return await apiRequest(this.endpoints.game.missions);
    }

    /**
     * Lista missões diárias
     * @returns {Promise<Object>} Lista de missões diárias
     */
    async getDailyMissions() {
        return await apiRequest(this.endpoints.game.dailyMissions);
    }

    /**
     * Completa uma missão
     * @param {string} missionId - ID da missão
     * @returns {Promise<Object>} Resultado da operação
     */
    async completeMission(missionId) {
        return await apiRequest(this.endpoints.game.completeMission(missionId), {
            method: 'POST'
        });
    }

    // ============================================================
    // MÉTODOS AUXILIARES
    // ============================================================

    /**
     * Verifica se o usuário está autenticado
     * @returns {boolean} True se autenticado
     */
    isAuthenticated() {
        return API_CONFIG.isAuthenticated();
    }

    /**
     * Obtém o token atual
     * @returns {string|null} Token JWT
     */
    getToken() {
        return API_CONFIG.getToken();
    }
}

// Instância global do serviço de API
window.apiService = new ApiService();
