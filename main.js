// Menu lateral fixo: sem overlay/hambúrguer/recolher.
// Mantemos apenas um pequeno helper se quisermos, no futuro, reagir a clique dos itens.
document.querySelectorAll('#sidebar .nav-link').forEach(btn => {
  btn.addEventListener('click', () => {
    // Navegação é tratada por script.js via data-section.
  });
});
