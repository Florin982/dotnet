// LIVELLO 1 — CONTROLLER: riceve la richiesta HTTP, chiama il Service
// e trasforma il risultato in una risposta (200, 400, 404).
// Non contiene regole e non tocca mai i dati.
public static class PilotaEndpoints
{
    // static: un solo Service (e quindi un solo Repository) per tutta la vita dell'API
    private static PilotaService pilotaService = new PilotaService();

    public static void MapPilotiEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/piloti", () => pilotaService.GetPiloti());

        // {id:int}: la rotta accetta solo numeri interi
        app.MapGet("/piloti/{id:int}", (int id) =>
        {
            Pilota? pilota = pilotaService.GetPilota(id);
            if (pilota != null)
                return Results.Ok(pilota);
            else
                return Results.NotFound();
        });

        app.MapGet("/piloti/scuderia/{nome}", (string nome) => pilotaService.GetPilotiDiScuderia(nome));

        app.MapPost("/piloti", (Pilota pilota) =>
        {
            bool result = pilotaService.Create(pilota);
            if (result)
                return Results.Ok();
            else
                return Results.BadRequest();
        });

        app.MapPut("/piloti", (Pilota pilota) =>
        {
            bool result = pilotaService.Update(pilota);
            if (result)
                return Results.Ok();
            else
                return Results.NotFound();
        });

        app.MapDelete("/piloti/{id:int}", (int id) =>
        {
            bool result = pilotaService.Delete(id);
            if (result)
                return Results.Ok();
            else
                return Results.NotFound();
        });
    }
}
