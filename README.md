# Piloti F1 — Esercitazione 9.1 (CSR) con frontend Vue

```
backend\    API ASP.NET Core .NET 8, divisa in Controller -> Service -> Repository
frontend\   Vue 3 + Vite
render.yaml Blueprint per Render.com
```

## Requisiti (Windows)

- **.NET 8 SDK**: controlla con `dotnet --list-sdks` (deve esserci una riga che inizia con `8.`)
- **Node.js LTS**: controlla con `node -v`. Se manca, installalo da PowerShell:
  ```powershell
  winget install OpenJS.NodeJS.LTS
  ```
  poi chiudi e riapri PowerShell.

## Avvio in locale

Servono **due finestre di PowerShell**.

**1. Backend**

```powershell
cd C:\Dev\its_dotnet\backend
dotnet run
```

- API: http://localhost:5281/piloti
- Swagger: http://localhost:5281/swagger

**2. Frontend**

```powershell
cd C:\Dev\its_dotnet\frontend
npm install
npm run dev
```

Apri http://localhost:5173. Il frontend chiama il backend su `http://localhost:5281`
(vedi `frontend\.env.development`), quindi il backend deve essere già avviato.

`npm install` serve solo la prima volta (o quando cambia `package.json`).

## Provare solo l'API

Apri `backend\backend.http` in Visual Studio o in VS Code (estensione REST Client) e
clicca **Send Request** sopra ogni richiesta: sono le prove della traccia, in ordine.

## Su Render

1. Metti la cartella su un repository GitHub (con `render.yaml` nella cartella principale).
2. Su Render: **New → Blueprint**, scegli il repository.
3. Quando Render chiede `VITE_API_URL`, scrivi l'URL del backend:
   `https://piloti-api.onrender.com`.
   Se il nome è già preso, Render aggiunge un suffisso: in quel caso, dopo il primo
   deploy, copia l'URL vero dal servizio `piloti-api`, aggiornalo in
   **piloti-frontend → Environment** e rifai il deploy del frontend
   (`VITE_API_URL` viene letta durante la build).

Note:
- Il `Dockerfile` serve solo a Render (che usa server Linux): in locale non ti serve Docker.
- Swagger è attivo anche su Render: `https://<backend>/swagger`.
- Il piano free sospende il backend dopo 15 minuti di inattività: la prima richiesta
  può impiegare circa un minuto.
- I dati stanno in un `Dictionary` in memoria: a ogni riavvio tornano Leclerc e Norris.
