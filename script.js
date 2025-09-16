// Jogo Naruto - Script Principal
class NarutoGame {
    constructor() {
        this.currentSection = 'vila';
        this.currentTheme = 'naruto'; // 'naruto' ou 'akatsuki'
        this.player = {
            name: '',
            level: 1,
            xp: 0,
            maxXp: 100,
            village: 'Indefinido',
            element: 'Indefinido',
            chakra: 100,
            skills: {
                ninjutsu: 5,
                taijutsu: 3,
                genjutsu: 2
            }
        };
        
        this.npcs = {
            naruto: {
                name: 'Naruto Uzumaki',
                avatar: '🍜',
                color: 'bg-orange-500',
                dialogues: [
                    'Olá! Bem-vindo à Vila da Folha, dattebayo!',
                    'Vou me tornar o melhor Hokage de todos os tempos!',
                    'Nunca desista dos seus sonhos, esse é o meu jeito ninja!',
                    'Quer treinar comigo? Posso te ensinar o Rasengan!'
                ]
            },
            sasuke: {
                name: 'Sasuke Uchiha',
                avatar: '👁️',
                color: 'bg-red-600',
                dialogues: [
                    'Hmph... Outro ninja fraco.',
                    'O poder é tudo. Sem ele, você não pode proteger nada.',
                    'Meu Sharingan pode ver através de qualquer técnica.',
                    'Treine mais se quiser chegar ao meu nível.'
                ]
            },
            sakura: {
                name: 'Sakura Haruno',
                avatar: '🌸',
                color: 'bg-pink-500',
                dialogues: [
                    'Olá! Precisa de cura? Sou uma ninja médica.',
                    'A força não vem apenas dos músculos, mas também da mente.',
                    'Posso te ensinar algumas técnicas médicas básicas.',
                    'Cuidado para não se machucar nos treinos!'
                ]
            },
            kakashi: {
                name: 'Kakashi Hatake',
                avatar: '⚡',
                color: 'bg-gray-600',
                dialogues: [
                    'Yo! Desculpe o atraso, me perdi no caminho da vida.',
                    'Um ninja deve ver através da decepção.',
                    'Trabalho em equipe é essencial para um ninja.',
                    'Que tal lermos um pouco do Icha Icha Paradise?'
                ]
            }
        };

        this.init();
    }

    init() {
        this.loadPlayerData();
        this.setupEventListeners();
        this.updatePlayerDisplay();
        this.showSection('vila');

        // Mostrar nickname do usuário logado na sidebar (se existir)
        try {
            const users = JSON.parse(localStorage.getItem('narutoUsers') || '[]');
            const nickname = localStorage.getItem('narutoLoggedNickname');
            const user = users.find(u => u.nickname === nickname);
            const el = document.getElementById('sidebarPlayerName');
            if (user && el) el.textContent = `@${user.nickname}`;
            // Sincronizar nickname do player com o login salvo
            if (nickname) this.player.name = nickname;
        } catch {}
        
        // Adicionar efeitos de partículas
        this.addParticleEffects();
        
        console.log('🍃 Vila da Folha carregada com sucesso!');
    }

    // Registra listeners para navegação (por data-section), menu mobile e toggle de tema
    setupEventListeners() {
        // Navegação
    // Navegação: data-section ao invés de anchors
    document.querySelectorAll('.nav-link').forEach(link => {
            link.addEventListener('click', (e) => {
                e.preventDefault();
                const btn = e.currentTarget;
                const section = btn.getAttribute('data-section');
                if (section) this.showSection(section);
                // Fechar menu após navegar (mobile)
                this.closeMobileSidebar();
            });
        });

        // Menu mobile (removido no layout atual) – manter guardas para compatibilidade
        const mobileMenuBtn = document.getElementById('mobileMenuBtn');
        const overlay = document.getElementById('sidebarOverlay');
        if (mobileMenuBtn) {
            mobileMenuBtn.addEventListener('click', () => {
                document.body.classList.toggle('sidebar-open');
            });
        }
        if (overlay) {
            overlay.addEventListener('click', () => {
                this.closeMobileSidebar();
            });
        }

    // Toggle tema: persiste em localStorage (narutoGameTheme = 'dark'|'light')
        const themeToggle = document.getElementById('themeToggle');
        if (themeToggle) {
            themeToggle.addEventListener('click', () => {
                this.toggleTheme();
            });
        }

        // NPCs interativos (bloquear se personagem já escolhido)
        document.querySelectorAll('.npc-card').forEach(card => {
            card.addEventListener('click', () => {
                const locked = localStorage.getItem('narutoGameCharacter');
                if (locked) {
                    this.showNotification('Você já tem um personagem. Use "Trocar Personagem" no menu.');
                    return;
                }
                const npcId = card.getAttribute('data-npc');
                this.interactWithNPC(npcId);
            });
        });

    // (Busca de NPCs removida)

        // Modal de diálogo
        const closeDialog = document.getElementById('closeDialog');
        const dialogModal = document.getElementById('dialogModal');
        
        closeDialog.addEventListener('click', () => {
            this.closeDialog();
        });

        dialogModal.addEventListener('click', (e) => {
            if (e.target === dialogModal) {
                this.closeDialog();
            }
        });

        // Salvar perfil (mantém habilidades)
        const saveProfile = document.getElementById('saveProfile');
        if (saveProfile) {
            saveProfile.addEventListener('click', () => {
                this.savePlayerProfile();
            });
        }

        // Trocar personagem: limpa escolha e vai para login (seleção acontece lá)
        const changeChar = document.getElementById('changeCharacterBtn');
        if (changeChar) {
            changeChar.addEventListener('click', () => {
                localStorage.removeItem('narutoGameCharacter');
                window.location.href = 'login.html';
            });
        }

        // OBS: inputs de nome/vila/elemento foram movidos para o painel 'Status do Ninja' na Vila.

        // Atalhos de teclado
        document.addEventListener('keydown', (e) => {
            this.handleKeyPress(e);
        });

        // Logoff
        const logoutBtn = document.getElementById('logoutBtn');
        if (logoutBtn) {
            logoutBtn.addEventListener('click', () => {
                this.logout();
                this.closeMobileSidebar();
            });
        }

        // Fechar menu no Esc
        document.addEventListener('keydown', (e) => {
            if (e.key === 'Escape') this.closeMobileSidebar();
        });

        // Vila/Elemento são definidos durante o cadastro; edição inline removida.
    } 

    // Mostra uma seção por vez e marca item de menu ativo
    showSection(sectionId) {
        // Esconder todas as seções
        document.querySelectorAll('.section').forEach(section => {
            section.classList.remove('active');
            section.classList.add('hidden');
        });

        // Mostrar seção selecionada
        const targetSection = document.getElementById(sectionId);
        if (targetSection) {
            targetSection.classList.remove('hidden');
            targetSection.classList.add('active');
        }

        // Atualizar navegação
        document.querySelectorAll('.nav-link').forEach(link => {
            link.classList.remove('active');
        });

        document.querySelectorAll('.nav-link').forEach(link => {
            if (link.getAttribute('data-section') === sectionId) {
                link.classList.add('active');
            }
        });

        this.currentSection = sectionId;

        // Fechar menu mobile
    this.closeMobileSidebar();

        // Efeitos especiais por seção
        this.addSectionEffects(sectionId);
    }

    closeMobileSidebar() {
        document.body.classList.remove('sidebar-open');
    }

    addSectionEffects(sectionId) {
        switch(sectionId) {
            case 'vila':
                this.addVillaEffects();
                break;
            case 'perfil':
                this.updateProfileDisplay();
                break;
        }
    }

    addVillaEffects() {
        // Adicionar efeito de flutuação aos NPCs
        document.querySelectorAll('.npc-card').forEach((card, index) => {
            setTimeout(() => {
                card.classList.add('animate-float');
            }, index * 200);
        });
    }

    interactWithNPC(npcId) {
        const npc = this.npcs[npcId];
        if (!npc) return;

        // Selecionar diálogo aleatório
        const randomDialogue = npc.dialogues[Math.floor(Math.random() * npc.dialogues.length)];
        
        // Mostrar modal de diálogo
        this.showDialog(npc.name, npc.avatar, randomDialogue, npc.color);
        
        // Ganhar XP por interação
        this.gainXP(10);
        
        // Efeito sonoro (simulado)
        this.playSound('interaction');
    }

    showDialog(name, avatar, text, colorClass) {
        const modal = document.getElementById('dialogModal');
        const dialogName = document.getElementById('dialogName');
        const dialogAvatar = document.getElementById('dialogAvatar');
        const dialogText = document.getElementById('dialogText');

        dialogName.textContent = name;
        dialogAvatar.textContent = avatar;
        dialogAvatar.className = `w-16 h-16 rounded-full mx-auto mb-4 flex items-center justify-center text-2xl ${colorClass}`;
        dialogText.textContent = text;

        modal.classList.remove('hidden');
        modal.classList.add('flex');

        // Animação de entrada
        setTimeout(() => {
            modal.querySelector('.bg-white\\/10').classList.add('animate-fade-in');
        }, 10);
    }

    closeDialog() {
        const modal = document.getElementById('dialogModal');
        modal.classList.add('hidden');
        modal.classList.remove('flex');
    }

    gainXP(amount) {
        this.player.xp += amount;
        
        // Verificar level up
        while (this.player.xp >= this.player.maxXp) {
            this.levelUp();
        }
        
        this.updatePlayerDisplay();
        this.savePlayerData();
    }

    levelUp() {
        this.player.xp -= this.player.maxXp;
        this.player.level++;
        this.player.maxXp = Math.floor(this.player.maxXp * 1.2);
        
        // Aumentar habilidades aleatoriamente
        const skills = Object.keys(this.player.skills);
        const randomSkill = skills[Math.floor(Math.random() * skills.length)];
        this.player.skills[randomSkill]++;
        
        // Mostrar notificação de level up
        this.showNotification(`🎉 Level Up! Agora você é nível ${this.player.level}!`);
        
        this.playSound('levelup');
    }

    updatePlayerDisplay() {
        const levelElement = document.getElementById('playerLevel');
        const xpElement = document.getElementById('playerXP');
        const xpBar = document.getElementById('xpBar');
        const nickEl = document.getElementById('statusNickname');
        const villageEl = document.getElementById('statusVillage');
        const elementEl = document.getElementById('statusElement');
        const chakraEl = document.getElementById('statusChakra');
        const chakraBar = document.getElementById('chakraBar');

        if (levelElement) levelElement.textContent = this.player.level;
        if (xpElement) xpElement.textContent = `${this.player.xp}/${this.player.maxXp}`;
        if (xpBar) {
            const percentage = (this.player.xp / this.player.maxXp) * 100;
            xpBar.style.width = `${percentage}%`;
        }

        // Status panel
        if (nickEl) nickEl.textContent = this.player.name || (localStorage.getItem('narutoLoggedNickname') || 'Jogador');
        if (villageEl) villageEl.textContent = this.player.village || 'Indefinido';
        if (elementEl) elementEl.textContent = this.player.element || 'Indefinido';
        if (chakraEl) chakraEl.textContent = this.player.chakra != null ? this.player.chakra : 100;
        if (chakraBar) {
            const pct = Math.max(0, Math.min(100, (this.player.chakra / 100) * 100));
            chakraBar.style.width = `${pct}%`;
        }
    }

    updateProfileDisplay() {
        // Atualizar campos do perfil (checar existência — inputs básicos podem ter sido movidos)
        const playerNameEl = document.getElementById('playerName');
        const playerVillageEl = document.getElementById('playerVillage');
        const playerElementEl = document.getElementById('playerElement');
        if (playerNameEl) playerNameEl.value = this.player.name || '';
        if (playerVillageEl) playerVillageEl.value = this.player.village || 'Indefinido';
        if (playerElementEl) playerElementEl.value = this.player.element || 'Indefinido';

        // Atualizar habilidades
        Object.keys(this.player.skills).forEach(skill => {
            const levelElement = document.getElementById(`${skill}Level`);
            const barElement = document.getElementById(`${skill}Bar`);
            
            if (levelElement) levelElement.textContent = this.player.skills[skill];
            if (barElement) {
                const percentage = (this.player.skills[skill] / 10) * 100;
                barElement.style.width = `${percentage}%`;
            }
        });
    }

    savePlayerProfile() {
        this.savePlayerData();
        this.showNotification('✅ Perfil salvo com sucesso!');
        this.playSound('save');
    }

    // Alterna entre naruto-theme (claro) e akatsuki-theme (escuro)
    toggleTheme() {
        this.isDarkMode = !this.isDarkMode;
        const body = document.getElementById('body');
    const themeIcon = document.getElementById('themeIcon');

        if (this.isDarkMode) {
            body.classList.remove('naruto-theme');
            body.classList.remove('dark-theme');
            body.classList.add('akatsuki-theme');
            if (themeIcon) themeIcon.textContent = '☀️';
        } else {
            body.classList.remove('akatsuki-theme');
            body.classList.remove('dark-theme');
            body.classList.add('naruto-theme');
            if (themeIcon) themeIcon.textContent = '🌙';
        }

        localStorage.setItem('narutoGameTheme', this.isDarkMode ? 'dark' : 'light');
    }

    handleKeyPress(e) {
        switch(e.key) {
            case '1':
                this.showSection('vila');
                break;
            case '2':
                this.showSection('perfil');
                break;
            case '3':
                this.showSection('creditos');
                break;
            case 'Escape':
                this.closeDialog();
                break;
            case 't':
            case 'T':
                if (e.ctrlKey) {
                    e.preventDefault();
                    this.toggleTheme();
                }
                break;
        }
    }

    addParticleEffects() {
        // Adicionar efeitos de partículas aos NPCs
        document.querySelectorAll('.npc-card').forEach(card => {
            card.classList.add('particle-effect');
        });
    }

    showNotification(message) {
        // Criar elemento de notificação
        const notification = document.createElement('div');
        notification.className = 'fixed top-24 right-4 bg-green-500 text-white px-6 py-3 rounded-lg shadow-lg z-50 animate-fade-in';
        notification.textContent = message;

        document.body.appendChild(notification);

        // Remover após 3 segundos
        setTimeout(() => {
            notification.classList.add('opacity-0');
            setTimeout(() => {
                document.body.removeChild(notification);
            }, 300);
        }, 3000);
    }

    playSound(type) {
        // Simulação de efeitos sonoros (pode ser expandido com Web Audio API)
        console.log(`🔊 Som reproduzido: ${type}`);
        
        // Vibração no mobile (se suportado)
        if (navigator.vibrate) {
            switch(type) {
                case 'interaction':
                    navigator.vibrate(100);
                    break;
                case 'levelup':
                    navigator.vibrate([100, 50, 100, 50, 200]);
                    break;
                case 'save':
                    navigator.vibrate(50);
                    break;
            }
        }
    }

    savePlayerData() {
        const gameData = {
            player: this.player,
            isDarkMode: this.isDarkMode,
            currentSection: this.currentSection
        };
        
        localStorage.setItem('narutoGameData', JSON.stringify(gameData));
    }

    // Carrega dados e aplica tema salvo
    loadPlayerData() {
        const savedData = localStorage.getItem('narutoGameData');

        if (savedData) {
            try {
                const gameData = JSON.parse(savedData);
                this.player = { ...this.player, ...gameData.player };
                this.isDarkMode = gameData.isDarkMode || false;
                this.currentSection = gameData.currentSection || 'vila';
            } catch (error) {
                console.error('❌ Erro ao carregar dados salvos:', error);
            }
        }

        // Garantir defaults do gameData (caso campos novos estejam ausentes)
        this.ensureGameDataDefaults();

        // Carregar tema salvo
        const savedTheme = localStorage.getItem('narutoGameTheme');
        const body = document.getElementById('body');
        const themeIcon = document.getElementById('themeIcon');
        if (savedTheme === 'dark') {
            this.isDarkMode = true;
            body.classList.remove('naruto-theme', 'dark-theme');
            body.classList.add('akatsuki-theme');
            if (themeIcon) themeIcon.textContent = '☀️';
        } else {
            this.isDarkMode = false;
            body.classList.remove('akatsuki-theme', 'dark-theme');
            body.classList.add('naruto-theme');
            if (themeIcon) themeIcon.textContent = '🌙';
        }
    }

    ensureGameDataDefaults() {
        // nickname do login se presente
        const loginNick = localStorage.getItem('narutoLoggedNickname');
        if (loginNick) this.player.name = this.player.name || loginNick;

        // Defaults
        if (this.player.village == null) this.player.village = 'Indefinido';
        if (this.player.element == null) this.player.element = 'Indefinido';
        if (this.player.chakra == null) this.player.chakra = 100;
        if (this.player.level == null) this.player.level = 1;
        if (this.player.xp == null) this.player.xp = 0;
        if (this.player.maxXp == null) this.player.maxXp = 100;
        if (this.player.skills == null) this.player.skills = { ninjutsu: 1, taijutsu: 1, genjutsu: 1 };

        // Salvar caso algo tenha sido preenchido
        this.savePlayerData();
    }

    // Método para resetar o jogo (útil para desenvolvimento)
    resetGame() {
        localStorage.removeItem('narutoGameData');
        localStorage.removeItem('narutoGameTheme');
        location.reload();
    }

    // Logoff: limpa estado de login/personagem e volta ao login
    logout() {
        try {
            localStorage.removeItem('narutoGameLogged');
            localStorage.removeItem('narutoGameCharacter');
            sessionStorage.removeItem('narutoSession');
            // Mantém o tema e progresso, a não ser que queira limpar tudo:
            // localStorage.removeItem('narutoGameData');
        } catch (e) {
            console.warn('Falha ao limpar localStorage no logoff:', e);
        }
        window.location.href = 'login.html';
    }

    // Método para adicionar conquistas (expansível)
    unlockAchievement(achievementId) {
        console.log(`🏆 Conquista desbloqueada: ${achievementId}`);
        this.showNotification(`🏆 Nova conquista: ${achievementId}!`);
    }

    // Sistema de missões simples (expansível)
    startMission(missionId) {
        console.log(`📋 Missão iniciada: ${missionId}`);
        this.showNotification(`📋 Nova missão: ${missionId}`);
    }
}

// Inicializar o jogo quando a página carregar
document.addEventListener('DOMContentLoaded', () => {
    window.narutoGame = new NarutoGame();
});

// Adicionar comandos de console para desenvolvimento
window.gameCommands = {
    addXP: (amount) => window.narutoGame.gainXP(amount),
    levelUp: () => window.narutoGame.levelUp(),
    resetGame: () => window.narutoGame.resetGame(),
    toggleTheme: () => window.narutoGame.toggleTheme(),
    showSection: (section) => window.narutoGame.showSection(section)
};

console.log('🎮 Comandos disponíveis no console:');
console.log('gameCommands.addXP(100) - Adicionar XP');
console.log('gameCommands.levelUp() - Subir de nível');
console.log('gameCommands.resetGame() - Resetar jogo');
console.log('gameCommands.toggleTheme() - Alternar tema');
console.log('gameCommands.showSection("perfil") - Ir para seção');
