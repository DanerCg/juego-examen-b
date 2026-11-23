@echo off
title Subir proyecto Unity a GitHub
echo ==============================================
echo   SUBIR PROYECTO DE BART A GITHUB
echo ==============================================
echo.

where git >nul 2>nul
if errorlevel 1 (
  echo ERROR: Git no esta instalado.
  echo Descargalo en https://git-scm.com/download/win e instalalo.
  pause
  exit /b
)

if not exist "Assets" (
  echo ERROR: Este archivo debe estar en la carpeta raiz del proyecto.
  echo Es la carpeta donde estan Assets, Packages y ProjectSettings.
  pause
  exit /b
)

if not exist ".gitignore" (
  echo ERROR: Falta el archivo .gitignore en esta misma carpeta.
  echo Copialo aqui y vuelve a ejecutar este archivo.
  pause
  exit /b
)

set NEEDCFG=0
git config user.name >nul 2>nul || set NEEDCFG=1
if "%NEEDCFG%"=="1" echo Git necesita saber quien eres, solo esta vez.
if "%NEEDCFG%"=="1" set /p UN=Escribe tu nombre de usuario: 
if "%NEEDCFG%"=="1" set /p UE=Escribe tu correo de GitHub: 
if "%NEEDCFG%"=="1" git config --global user.name "%UN%"
if "%NEEDCFG%"=="1" git config --global user.email "%UE%"

echo.
echo Primero crea un repositorio VACIO en github.com y copia su direccion.
echo Ejemplo: https://github.com/TuUsuario/juego-bart.git
echo.
set /p REPO=Pega aqui la direccion del repositorio: 
set /p MSG=Mensaje del commit (Enter para dejar el normal): 
if "%MSG%"=="" set MSG=Juego de Bart con marca de agua DanerCg

echo.
echo Preparando archivos...
if not exist ".git" git init
git add .
git commit -m "%MSG%"
git branch -M main
git remote remove origin >nul 2>nul
git remote add origin %REPO%

echo.
echo Subiendo a GitHub. Si se abre una ventana, inicia sesion.
git push -u origin main

echo.
echo ==============================================
echo   Si arriba no hay errores en rojo, ya quedo subido.
echo   Actualiza la pagina de tu repositorio para verlo.
echo ==============================================
pause
