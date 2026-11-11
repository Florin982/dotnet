// Tutte le chiamate al backend stanno qui.
// VITE_API_URL arriva da .env.development (in locale) o da Render (in produzione).
const BASE_URL = (import.meta.env.VITE_API_URL || 'http://localhost:5281').replace(/\/$/, '')

async function richiesta(metodo, percorso, corpo) {
  const opzioni = { method: metodo, headers: {} }
  if (corpo !== undefined) {
    opzioni.headers['Content-Type'] = 'application/json'
    opzioni.body = JSON.stringify(corpo)
  }
  const risposta = await fetch(BASE_URL + percorso, opzioni)
  const testo = await risposta.text()
  return {
    ok: risposta.ok,
    status: risposta.status,
    dati: testo ? JSON.parse(testo) : null,
  }
}

export const api = {
  baseUrl: BASE_URL,
  getPiloti: () => richiesta('GET', '/piloti'),
  getPilota: (id) => richiesta('GET', `/piloti/${id}`),
  getPilotiDiScuderia: (nome) => richiesta('GET', `/piloti/scuderia/${encodeURIComponent(nome)}`),
  create: (pilota) => richiesta('POST', '/piloti', pilota),
  update: (pilota) => richiesta('PUT', '/piloti', pilota),
  delete: (id) => richiesta('DELETE', `/piloti/${id}`),
}
