// Menu lateral fixo: sem overlay/hambúrguer/recolher.
// Helper para reagir a clique nos itens, compatível com a navegação SPA.
document.querySelectorAll('#sidebar .nav-link').forEach(btn => {
  btn.addEventListener('click', () => {
    // Navegação é tratada por script.js via data-section.
  });
});
