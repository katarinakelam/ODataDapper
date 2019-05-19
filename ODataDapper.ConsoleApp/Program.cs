using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Serilog;
using System.Threading.Tasks;
using ODataDapper.Models;
using Simple.OData.Client;

namespace ODataDapper.ConsoleApp
{
    class Program
    {
        private static string url = "http://localhost:63061";
        private static int racunCount;
        private static int stavkeCount;
        private static ILogger log;

        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            ODataClientSettings settings = new ODataClientSettings(url);
            settings.IgnoreResourceNotFoundException = true;
            settings.PreferredUpdateMethod = ODataUpdateMethod.Put;
            // Get application base directory
            string basedir = AppDomain.CurrentDomain.BaseDirectory;
            log = new LoggerConfiguration()
                .WriteTo.RollingFile(basedir + "/Logs/consoleAppLog-{Date}.txt")
                .CreateLogger();

            settings.OnTrace = (x, y) =>
            {
                log.Information(string.Format(x, y));
                Console.WriteLine(string.Format(x, y));
            };

            var client = new ODataClient(settings);

            //Test client on Racuni collection
            await FetchRacuni(client);

            //Test client on Stavke collection
            await FetchStavke(client);
        }

        private static async Task FetchRacuni(ODataClient client)
        {
            racunCount = await client.For<Racun>("Racuni").Count().FindScalarAsync<int>();
            Console.WriteLine("Broj svih računa: " + racunCount);
            log.Information("Broj svih računa: " + racunCount);

            Console.WriteLine("Zadnje uneseni racun: ");
            log.Information("Zadnje uneseni racun: ");
            //Print out racun with given identifier
            await PrintRacun(client, racunCount);

            //Create a new racun
            var newRacun = new Racun
            {
                Id = racunCount + 1,
                DatumIzdavanja = DateTime.Now,
                JIR = "Zj46854dfs4s6",
                UkupanIznos = 153,
                Zaposlenik_Id = 2
            };

            //Insert racun using client
            var racun = await client.For<Racun>("Racuni")
                .Set(newRacun).InsertEntryAsync();

            //Check if new racun was properly saved to the database
            Console.WriteLine("Račun dodan pod id-em " + racun.Id);
            log.Information("Račun dodan pod id-em " + racun.Id);

            await PrintRacun(client, racun.Id);

            //Update created racun
            racun.JIR = "dsfdgdsf";
            await client.For<Racun>("Racuni")
                .Key(racun.Id)
                .Set(racun)
                .UpdateEntryAsync(false);

            Console.WriteLine("Ažuriran račun");
            log.Information("Ažuriran račun");

            //Print out updated racun
            await PrintRacun(client, racun.Id);

            //Delete created racun
            await client.For<Racun>("Racuni")
                .Key(racun.Id)
                .DeleteEntryAsync();

            Console.WriteLine("Obrisan račun");
            log.Information("Obrisan račun");
        }

        private static async Task PrintRacun(ODataClient client, int key)
        {
            var racun = await client.For<Racun>("Racuni")
                .Key(key)
                .FindEntryAsync();
            Console.WriteLine($"IdRacuna: {racun.Id}, datum izdavanja: {racun.DatumIzdavanja}, JIR: {racun.JIR}, iznos: {racun.UkupanIznos}");
            log.Information($"IdRacuna: {racun.Id}, datum izdavanja: {racun.DatumIzdavanja}, JIR: {racun.JIR}, iznos: {racun.UkupanIznos}");
        }

        private static async Task FetchStavke(ODataClient client)
        {
            //Get count of collection
            stavkeCount = await client.For<Stavka>("Stavke").Count().FindScalarAsync<int>();
            Console.WriteLine("Broj svih stavki: " + stavkeCount);
            log.Information("Broj svih stavki: " + stavkeCount);

            //Get all stavke with price larger than 6
            var stavke = await client.For<Stavka>("Stavke")
                .Filter(s => s.Cijena > 6)
                .OrderByDescending(s => s.Cijena)
                .FindEntriesAsync();

            Console.WriteLine("Stavke po filteru unutar kolekcije ");
            log.Information("Stavke po filteru unutar kolekcije ");
            foreach (var stavka in stavke)
            {
                Console.WriteLine($"Id: {stavka.Id}, cijena: {stavka.Cijena}, naziv: {stavka.Naziv}");
                log.Information($"Id: {stavka.Id}, cijena: {stavka.Cijena}, naziv: {stavka.Naziv}");
            }
            Console.WriteLine(" ");
            log.Information(" ");

            //Get stavke in the middle of the collection
            var middleStavke = await client.For<Stavka>("Stavke")
                .Top(2)
                .Skip(1)
                .FindEntriesAsync();

            Console.WriteLine("Stavke u sredini kolekcije ");
            log.Information("Stavke u sredini kolekcije ");
            foreach (var stavka in middleStavke)
            {
                Console.WriteLine($"Id: {stavka.Id}, cijena: {stavka.Cijena}, naziv: {stavka.Naziv}");
                log.Information($"Id: {stavka.Id}, cijena: {stavka.Cijena}, naziv: {stavka.Naziv}");
            }
            Console.ReadKey();
        }
    }
}
