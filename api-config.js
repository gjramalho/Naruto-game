// Configuração da API do Naruto Game
const API_CONFIG = {
    // URL base da API backend
    baseUrl: 'http://localhost:5000',
    
    // Endpoints da API
    endpoints: {
        auth: {
            register: '/api/auth/register',
            login: '/api/auth/login'
        },
        player: {
            profile: '/api/player/profile',
            xp: '/api/player/xp',
            skills: '/api/player/skills'
        },
        game: {
            npcs: '/api/game/npcs',
            npcByKey: (key) => `/api/game/npcs/${key}`,
            missions: '/api/game/missions',
            dailyMissions: '/api/game/missions/daily',
            completeMission: (missionId) => `/api/game/missions/${missionId}/complete`
        }
    },
    
    // Headers padrão
    headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json'
    },
    
    // Obtém o token JWT do localStorage
    getToken() {
        return localStorage.getItem('narutoGameToken');
    },
    
    // Salva o token JWT no localStorage
    setToken(token) {
        localStorage.setItem('narutoGameToken', token);
    },
    
    // Remove o token JWT do localStorage
    removeToken() {
        localStorage.removeItem('narutoGameToken');
    },
    
    // Verifica se o usuário está autenticado
    isAuthenticated() {
        return !!this.getToken();
    },
    
    // Obtém headers com autenticação (se disponível)
    getAuthHeaders() {
        const headers = { ...this.headers };
        const token = this.getToken();
        if (token) {
            headers['Authorization'] = `Bearer ${token}`;
        }
        return headers;
    }
};

// Função auxiliar para fazer requisições à API
async function apiRequest(endpoint, options = {}) {
    const url = `${API_CONFIG.baseUrl}${endpoint}`;
    
    const defaultOptions = {
        headers: API_CONFIG.getAuthHeaders(),
        ...options
    };
    
    try {
        const response = await fetch(url, defaultOptions);
        const data = await response.json();
        
        if (!response.ok) {
            throw new Error(data.error || `Erro na requisição: ${response.status}`);
        }
        
        return { success: true, data };
    } catch (error) {
        console.error('Erro na requisição API:', error);
        return { success: false, error: error.message };
    }
}

// Exportar para uso global
window.API_CONFIG = API_CONFIG;
window.apiRequest = apiRequest;
