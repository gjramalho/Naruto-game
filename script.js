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
            village: 'folha',
            element: 'fogo',
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
                avatar: '⚡',
                color: 'bg-purple-600',
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
                avatar: '👁️',
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
        
        // Adicionar efeitos de partículas
        this.addParticleEffects();
        
        console.log('🍃 Vila da Folha carregada com sucesso!');
    }

    setupEventListeners() {
        // Navegação
        document.querySelectorAll('.nav-link').forEach(link => {
            link.addEventListener('click', (e) => {
                e.preventDefault();
                const section = e.target.getAttribute('href').substring(1);
                this.showSection(section);
            });
        });

        // Menu mobile
        const mobileMenuBtn = document.getElementById('mobileMenuBtn');
        const mobileMenu = document.getElementById('mobileMenu');
        
        mobileMenuBtn.addEventListener('click', () => {
            mobileMenu.classList.toggle('hidden');
        });

        // Toggle tema
        const themeToggle = document.getElementById('themeToggle');
        themeToggle.addEventListener('click', () => {
            this.toggleTheme();
        });

        // NPCs interativos
        document.querySelectorAll('.npc-card').forEach(card => {
            card.addEventListener('click', () => {
                const npcId = card.getAttribute('data-npc');
                this.interactWithNPC(npcId);
            });
        });

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

        // Salvar perfil
        const saveProfile = document.getElementById('saveProfile');
        saveProfile.addEventListener('click', () => {
            this.savePlayerProfile();
        });

        // Inputs do perfil
        document.getElementById('playerName').addEventListener('input', (e) => {
            this.player.name = e.target.value;
        });

        document.getElementById('playerVillage').addEventListener('change', (e) => {
            this.player.village = e.target.value;
        });

        document.getElementById('playerElement').addEventListener('change', (e) => {
            this.player.element = e.target.value;
        });

        // Atalhos de teclado
        document.addEventListener('keydown', (e) => {
            this.handleKeyPress(e);
        });
    }

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

        document.querySelectorAll(`[href="#${sectionId}"]`).forEach(link => {
            link.classList.add('active');
        });

        this.currentSection = sectionId;

        // Fechar menu mobile
        document.getElementById('mobileMenu').classList.add('hidden');

        // Efeitos especiais por seção
        this.addSectionEffects(sectionId);
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

        if (levelElement) levelElement.textContent = this.player.level;
        if (xpElement) xpElement.textContent = `${this.player.xp}/${this.player.maxXp}`;
        if (xpBar) {
            const percentage = (this.player.xp / this.player.maxXp) * 100;
            xpBar.style.width = `${percentage}%`;
        }
    }

    updateProfileDisplay() {
        // Atualizar campos do perfil
        document.getElementById('playerName').value = this.player.name;
        document.getElementById('playerVillage').value = this.player.village;
        document.getElementById('playerElement').value = this.player.element;

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

    toggleTheme() {
        this.isDarkMode = !this.isDarkMode;
        const body = document.getElementById('body');
        const themeIcon = document.getElementById('themeIcon');

        if (this.isDarkMode) {
            body.classList.remove('naruto-theme');
            body.classList.add('dark-theme');
            themeIcon.className = 'fas fa-sun';
        } else {
            body.classList.remove('dark-theme');
            body.classList.add('naruto-theme');
            themeIcon.className = 'fas fa-moon';
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

        // Carregar tema salvo
        const savedTheme = localStorage.getItem('narutoGameTheme');
        const body = document.getElementById('body');
        const themeIcon = document.getElementById('themeIcon');
        if (savedTheme === 'dark') {
            this.isDarkMode = true;
            body.classList.remove('naruto-theme');
            body.classList.add('dark-theme');
            if (themeIcon) themeIcon.className = 'fas fa-sun';
        } else {
            this.isDarkMode = false;
            body.classList.remove('dark-theme');
            body.classList.add('naruto-theme');
            if (themeIcon) themeIcon.className = 'fas fa-moon';
        }
    }

    // Método para resetar o jogo (útil para desenvolvimento)
    resetGame() {
        localStorage.removeItem('narutoGameData');
        localStorage.removeItem('narutoGameTheme');
        location.reload();
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
