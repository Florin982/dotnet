<script setup>
import { ref, reactive, onMounted } from 'vue'
import { api } from './api.js'

const piloti = ref([])
const filtroScuderia = ref('')
const filtroAttivo = ref('')
const messaggio = ref(null) // { tipo: 'ok' | 'errore', testo }
const caricamento = ref(false)

const vuoto = () => ({ id: null, nome: '', scuderia: '', numero: null })
const form = reactive(vuoto())
const inModifica = ref(false)

function mostra(tipo, testo) {
  messaggio.value = { tipo, testo }
}

async function esegui(azione) {
  caricamento.value = true
  try {
    return await azione()
  } catch {
    mostra('errore', `Backend non raggiungibile (${api.baseUrl}). Su Render il primo avvio può richiedere fino a un minuto.`)
    return null
  } finally {
    caricamento.value = false
  }
}

async function carica() {
  const r = await esegui(() =>
    filtroAttivo.value ? api.getPilotiDiScuderia(filtroAttivo.value) : api.getPiloti()
  )
  if (r) piloti.value = r.dati ?? []
}

function filtra() {
  filtroAttivo.value = filtroScuderia.value.trim()
  carica()
}

function togliFiltro() {
  filtroScuderia.value = ''
  filtroAttivo.value = ''
  carica()
}

function modifica(p) {
  Object.assign(form, p)
  inModifica.value = true
  messaggio.value = null
}

function annulla() {
  Object.assign(form, vuoto())
  inModifica.value = false
}

async function salva() {
  const pilota = { ...form, id: Number(form.id), numero: Number(form.numero) }
  const r = await esegui(() => (inModifica.value ? api.update(pilota) : api.create(pilota)))
  if (!r) return
  if (r.ok) {
    mostra('ok', `${r.status} — pilota ${inModifica.value ? 'modificato' : 'aggiunto'}.`)
    annulla()
    carica()
  } else if (inModifica.value) {
    mostra('errore', `${r.status} — pilota non trovato o dati non validi.`)
  } else {
    mostra('errore', `${r.status} — dati non validi, id già presente o numero già usato.`)
  }
}

async function elimina(p) {
  if (!confirm(`Cancellare ${p.nome}?`)) return
  const r = await esegui(() => api.delete(p.id))
  if (!r) return
  mostra(r.ok ? 'ok' : 'errore', r.ok ? `${r.status} — ${p.nome} cancellato.` : `${r.status} — pilota non trovato.`)
  carica()
}

onMounted(carica)
</script>

<template>
  <main>
    <header>
      <h1>Piloti F1</h1>
      <p class="sub">
        Esercitazione 9.1 — CSR ·
        <a :href="api.baseUrl + '/swagger'" target="_blank">Swagger</a>
      </p>
    </header>

    <p v-if="messaggio" :class="['msg', messaggio.tipo]">{{ messaggio.testo }}</p>

    <section class="card">
      <h2>{{ inModifica ? 'Modifica pilota' : 'Aggiungi pilota' }}</h2>
      <form @submit.prevent="salva" class="form">
        <label>Id <input v-model="form.id" type="number" required :disabled="inModifica" /></label>
        <label>Nome <input v-model="form.nome" placeholder="Lewis Hamilton" /></label>
        <label>Scuderia <input v-model="form.scuderia" placeholder="Ferrari" /></label>
        <label>Numero <input v-model="form.numero" type="number" placeholder="1-99" required /></label>
        <div class="azioni">
          <button type="submit" :disabled="caricamento">{{ inModifica ? 'Salva' : 'Aggiungi' }}</button>
          <button v-if="inModifica" type="button" class="secondario" @click="annulla">Annulla</button>
        </div>
      </form>
    </section>

    <section class="card">
      <div class="barra">
        <h2>
          Elenco
          <span v-if="filtroAttivo" class="tag">scuderia: {{ filtroAttivo }}</span>
        </h2>
        <form @submit.prevent="filtra" class="filtro">
          <input v-model="filtroScuderia" placeholder="Filtra per scuderia" />
          <button type="submit">Filtra</button>
          <button v-if="filtroAttivo" type="button" class="secondario" @click="togliFiltro">Tutti</button>
        </form>
      </div>

      <table v-if="piloti.length">
        <thead>
          <tr><th>Id</th><th>Nome</th><th>Scuderia</th><th>Numero</th><th></th></tr>
        </thead>
        <tbody>
          <tr v-for="p in piloti" :key="p.id">
            <td>{{ p.id }}</td>
            <td>{{ p.nome }}</td>
            <td>{{ p.scuderia }}</td>
            <td class="numero">{{ p.numero }}</td>
            <td class="azioni-riga">
              <button class="secondario" @click="modifica(p)">Modifica</button>
              <button class="pericolo" @click="elimina(p)">Elimina</button>
            </td>
          </tr>
        </tbody>
      </table>
      <p v-else class="vuoto">{{ caricamento ? 'Caricamento…' : 'Nessun pilota.' }}</p>
    </section>
  </main>
</template>
