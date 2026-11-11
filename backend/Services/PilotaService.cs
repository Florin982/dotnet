// LIVELLO 2 — SERVICE: contiene le regole del programma.
// Non sa nulla di HTTP né di come sono salvati i dati.
public class PilotaService
{
    private const int NumeroMinimo = 1;
    private const int NumeroMassimo = 99;

    private PilotaRepository pilotaRepository = new PilotaRepository();

    public IEnumerable<Pilota> GetPiloti()
    {
        return pilotaRepository.GetPiloti();
    }

    public Pilota? GetPilota(int id)
    {
        return pilotaRepository.GetPilota(id);
    }

    // Regola: solo i piloti della scuderia indicata, senza badare a maiuscole/minuscole
    public IEnumerable<Pilota> GetPilotiDiScuderia(string nome)
    {
        return pilotaRepository.GetPiloti()
            .Where(p => string.Equals(p.Scuderia, nome, StringComparison.OrdinalIgnoreCase))
            .ToArray();
    }

    public bool Create(Pilota pilota)
    {
        // Dati non validi: il Repository non viene nemmeno chiamato
        if (!IsValid(pilota))
            return false;

        // Regola solo per Create: il numero di gara non deve essere già usato
        if (pilotaRepository.GetPiloti().Any(p => p.Numero == pilota.Numero))
            return false;

        return pilotaRepository.Create(pilota);
    }

    public bool Update(Pilota pilota)
    {
        if (!IsValid(pilota))
            return false;

        return pilotaRepository.Update(pilota);
    }

    public bool Delete(int id)
    {
        return pilotaRepository.Delete(id);
    }

    // Regole comuni a Create e Update, scritte una volta sola
    private bool IsValid(Pilota pilota)
    {
        if (string.IsNullOrWhiteSpace(pilota.Nome))
            return false; // il nome è obbligatorio
        if (string.IsNullOrWhiteSpace(pilota.Scuderia))
            return false; // la scuderia è obbligatoria
        if (pilota.Numero < NumeroMinimo || pilota.Numero > NumeroMassimo)
            return false; // il numero di gara va da 1 a 99
        return true;
    }
}
