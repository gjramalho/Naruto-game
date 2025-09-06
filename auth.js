// Auth util: armazenamento local de usuários com hash de senha
(function(){
  function getUsers(){
    try{ return JSON.parse(localStorage.getItem('narutoUsers')||'[]'); }catch{ return []; }
  }
  function saveUsers(users){
    localStorage.setItem('narutoUsers', JSON.stringify(users));
  }
  async function hashPassword(password){
    try{
      if (window.crypto && window.crypto.subtle) {
        const enc = new TextEncoder();
        const buf = await crypto.subtle.digest('SHA-256', enc.encode(password));
        const arr = Array.from(new Uint8Array(buf));
        return arr.map(b=>b.toString(16).padStart(2,'0')).join('');
      }
    }catch{}
    // Fallback simples (não seguro) se SubtleCrypto não estiver disponível
    try { return btoa(unescape(encodeURIComponent(password))); } catch { return password; }
  }
  function findUserByIdentifier(identifier){
    const id = (identifier||'').trim().toLowerCase();
    return getUsers().find(u => u.nickname.toLowerCase()===id || u.email.toLowerCase()===id);
  }
  async function createUser({email, nickname, password}){
    const users = getUsers();
    const emailKey = (email||'').trim().toLowerCase();
    const nickKey = (nickname||'').trim().toLowerCase();
    if (users.some(u => u.email.toLowerCase()===emailKey)){
      return { ok:false, error:'E-mail já cadastrado.' };
    }
    if (users.some(u => u.nickname.toLowerCase()===nickKey)){
      return { ok:false, error:'Nome ninja já existe.' };
    }
    const passwordHash = await hashPassword(password);
    const newUser = { email: emailKey, nickname: nickKey, passwordHash, createdAt: Date.now() };
    users.push(newUser);
    saveUsers(users);
    return { ok:true, user:newUser };
  }
  async function verifyLogin(identifier, password){
    const user = findUserByIdentifier(identifier);
    if (!user) return { ok:false, error:'Conta não encontrada. Crie sua conta.' };
    const hash = await hashPassword(password);
    if (user.passwordHash !== hash) return { ok:false, error:'Senha incorreta.' };
    return { ok:true, user };
  }
  window.narutoAuth = { getUsers, createUser, verifyLogin };
})();
