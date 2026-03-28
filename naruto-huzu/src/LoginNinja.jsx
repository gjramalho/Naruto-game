import React, { useState } from 'react';
import './LoginNinja.css';

// Funções utilitárias de autenticação (adaptadas do auth.js)
function getUsers() {
  try { return JSON.parse(localStorage.getItem('narutoUsers') || '[]'); } catch { return []; }
}
function saveUsers(users) {
  localStorage.setItem('narutoUsers', JSON.stringify(users));
}
async function hashPassword(password) {
  if (window.crypto && window.crypto.subtle) {
    const enc = new TextEncoder();
    const buf = await window.crypto.subtle.digest('SHA-256', enc.encode(password));
    const arr = Array.from(new Uint8Array(buf));
    return arr.map(b => b.toString(16).padStart(2, '0')).join('');
  }
  return password;
}
async function verifyLogin(identifier, password) {
  const user = getUsers().find(u => u.nickname === identifier);
  if (!user) return { ok: false, error: 'Conta não encontrada. Crie sua conta.' };
  const hash = await hashPassword(password);
  if (user.passwordHash !== hash) return { ok: false, error: 'Senha incorreta.' };
  return { ok: true, user };
}

const personagens = [
  { key: 'naruto', nome: 'Naruto Uzumaki', desc: 'O ninja que nunca desiste!', emoji: '🍜' },
  { key: 'sasuke', nome: 'Sasuke Uchiha', desc: 'O último dos Uchiha', emoji: '👁️' },
  { key: 'sakura', nome: 'Sakura Haruno', desc: 'Ninja médica poderosa', emoji: '🌸' },
  { key: 'kakashi', nome: 'Kakashi Hatake', desc: 'O ninja copiador', emoji: '⚡' },
];

export default function LoginNinja() {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [remember, setRemember] = useState(false);
  const [error, setError] = useState('');
  const [info, setInfo] = useState('');
  const [step, setStep] = useState('login');
  const [selected, setSelected] = useState(null);

  const handleLogin = async (e) => {
    e.preventDefault();
    setError('');
    if (!username || !password) {
      setError('Preencha usuário e senha.');
      return;
    }
    const result = await verifyLogin(username, password);
    if (!result.ok) {
      setError(result.error);
      return;
    }
    if (remember) {
      localStorage.setItem('narutoGameLogged', 'true');
    } else {
      sessionStorage.setItem('narutoSession', 'true');
    }
    localStorage.setItem('narutoLoggedNickname', result.user.nickname);
    setStep('personagem');
  };

  const handleConfirm = () => {
    if (selected) {
      localStorage.setItem('narutoGameCharacter', selected);
      window.location.href = '/'; // Redireciona para home (ajuste se necessário)
    }
  };

  return (
    <div className="min-h-screen w-full flex flex-col md:flex-row items-center justify-center naruto-theme">
      {step === 'login' && (
        <div className="w-full md:w-1/2 flex items-center justify-center">
          <div className="max-w-md w-full glass-card-dark p-8 m-8 animate-fade-in shadow-2xl">
            <h1 className="login-title-dark text-center mb-6 flex items-center justify-center gap-2">
              <span>🍃</span> Login Ninja
            </h1>
            <form className="space-y-4" onSubmit={handleLogin}>
              <div>
                <label className="block text-red-200 mb-2">Usuário Ninja</label>
                <input type="text" value={username} onChange={e => setUsername(e.target.value)}
                  className="w-full px-4 py-2 rounded-lg bg-[#1a0000] text-red-100 font-bold border border-red-700 focus:border-red-500 focus:outline-none"
                  placeholder="Digite seu nome ninja" required />
              </div>
              <div>
                <label className="block text-red-200 mb-2">Senha</label>
                <input type="password" value={password} onChange={e => setPassword(e.target.value)}
                  className="w-full px-4 py-2 rounded-lg bg-[#1a0000] text-red-100 font-bold border border-red-700 focus:border-red-500 focus:outline-none"
                  placeholder="Digite sua senha" required />
              </div>
              <label className="flex items-center gap-2 text-red-200 select-none">
                <input type="checkbox" checked={remember} onChange={e => setRemember(e.target.checked)} className="accent-red-600" />
                Lembrar de mim
              </label>
              <button type="submit" className="w-full mt-4 bg-gradient-to-r from-red-700 via-red-900 to-black text-white font-bold py-3 rounded-lg transition-colors flex items-center justify-center gap-2 shadow-lg hover:scale-105">
                <span aria-hidden="true">🔓</span> Entrar
              </button>
              {error && <div className="text-red-400 text-center mt-2">{error}</div>}
              {info && <div className="text-green-400 text-center mt-2">{info}</div>}
              <p className="text-center text-red-200 mt-2">Não tem conta? <a href="/register" className="text-orange-400 hover:underline font-semibold">Crie agora</a></p>
            </form>
          </div>
        </div>
      )}
      {step === 'personagem' && (
        <div className="w-full md:w-1/2 items-center justify-center animate-fade-in">
          <div className="max-w-2xl w-full glass-card-dark p-8 m-8 shadow-2xl">
            <h2 className="text-2xl font-bold text-red-200 mb-6 text-center">Escolha seu personagem inicial</h2>
            <div className="grid grid-cols-2 md:grid-cols-2 gap-6">
              {personagens.map(p => (
                <div key={p.key} className={`character-card bg-[#1a0000] rounded-xl p-6 text-center cursor-pointer border border-red-900 ${selected === p.key ? 'selected' : ''}`}
                  onClick={() => setSelected(p.key)}>
                  <div className="w-16 h-16 bg-orange-500 rounded-full mx-auto mb-2 flex items-center justify-center text-3xl">{p.emoji}</div>
                  <div className="font-bold text-orange-400 mb-1">{p.nome}</div>
                  <div className="text-red-200 text-sm">{p.desc}</div>
                </div>
              ))}
            </div>
            <button id="confirmCharacter" className="mt-8 w-full bg-gradient-to-r from-orange-500 via-red-700 to-black text-white font-bold py-3 rounded-lg transition-colors shadow-lg hover:scale-105 disabled:opacity-50" disabled={!selected} onClick={handleConfirm}>
              Confirmar Personagem
            </button>
          </div>
        </div>
      )}
    </div>
  );
}
