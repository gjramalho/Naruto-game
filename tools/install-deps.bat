@echo off
set PATH=C:\Program Files\nodejs;%PATH%
cd "%~dp0\.."
npm install
echo.
echo Instalacao concluida! Pressione qualquer tecla para sair.
pause > nul