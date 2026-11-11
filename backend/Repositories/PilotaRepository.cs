// LIVELLO 3 — REPOSITORY: l'unico livello che sa dove sono salvati i dati.
// Contiene solo i vincoli dei dati (es. "la chiave id è unica"), nessuna regola.
public class PilotaRepository
{
    // private: nessun altro livello può toccare il dizionario
    private Dictionary<int, Pilota> piloti = new Dictionary<int, Pilota>
    {
        { 1, new Pilota { Id = 1, Nome = "Charles Leclerc", Scuderia = "Ferrari", Numero = 16 } },
        { 2, new Pilota { Id = 2, Nome = "Lando Norris", Scuderia = "McLaren", Numero = 4 } }
    };

    public IEnumerable<Pilota> GetPiloti()
    {
        return piloti.Values.ToArray();
    }

    public Pilota? GetPilota(int id)
    {
        if (piloti.ContainsKey(id))
            return piloti[id];
        else
            return null;
    }

    public bool Create(Pilota pilota)
    {
        // Vincolo dei dati: due elementi non possono avere la stessa chiave
        if (piloti.ContainsKey(pilota.Id))
            return false;

        piloti.Add(pilota.Id, pilota);
        return true;
    }

    public bool Update(Pilota pilota)
    {
        if (!piloti.ContainsKey(pilota.Id))
            return false;

        piloti[pilota.Id] = pilota;
        return true;
    }

    public bool Delete(int id)
    {
        return piloti.Remove(id); // Remove restituisce già false se l'id non c'è
    }
}
