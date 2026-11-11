// La "forma" dei dati che l'API riceve e restituisce.
// JSON: { "id": 3, "nome": "Lewis Hamilton", "scuderia": "Ferrari", "numero": 44 }
public class Pilota
{
    public int Id { get; set; }

    public string? Nome { get; set; }

    public string? Scuderia { get; set; }

    // Numero di gara, quello dipinto sulla macchina
    public int Numero { get; set; }
}
