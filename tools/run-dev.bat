@echo off
set PATH=C:\Program Files\nodejs;%PATH%
cd "%~dp0\.."
echo Iniciando servidor de desenvolvimento...
echo.
echo Frontend: http://localhost:5173
echo Backend:  http://localhost:5000
echo.
npm run dev