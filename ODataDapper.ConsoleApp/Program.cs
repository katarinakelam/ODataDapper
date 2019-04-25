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

        static void Main(string[] args)
        {
            ODataClientSettings settings = new ODataClientSettings(url);
            settings.IgnoreResourceNotFoundException = true;
            settings.OnTrace = (x, y) => Console.WriteLine(string.Format(x, y));

            var client = new ODataClient(settings);

            Task.Run(async () =>
            {
                //Test client on Racuni collection
                await FetchRacuni(client);

                //Test client on Stavke collection
                await FetchStavke(client);
            });
        }

        private static async Task FetchRacuni(ODataClient client)
        {
            racunCount = await client.For<Racun>().Count().FindScalarAsync<int>();
            Console.WriteLine("Broj svih računa: " + racunCount);

            //Print out racun with given identifier
            await PrintRacun(client, 3);

            //Create a new racun
            var newRacun = new Racun
            {
                Id = racunCount + 1,
                DatumIzdavanja = DateTime.Now,
                JIR = "Zj46854dfs4s6",
                UkupanIznos = 153,
                Zaposlenik_Id = 3
            };

            //Insert racun using client
            var racun = await client.For<Racun>()
                .Set(newRacun).InsertEntryAsync();

            //Check if new racun was properly saved to the database
            Console.WriteLine("Račun dodan pod id-em " + racun.Id);
            await PrintRacun(client, racun.Id);

            //Update created racun
            racun.JIR += "_";

            racun = await client.For<Racun>()
                .Key(racun.Id)
                .Set(new { JIR = "Prazni naziv" })
                .UpdateEntryAsync();

            Console.WriteLine("Ažuriran račun");

            //Print out updated racun
            await PrintRacun(client, racun.Id);

            //Delete created racun
            await client.For<Racun>()
                .Key(racun.Id)
                .DeleteEntryAsync();

            Console.WriteLine("Obrisan račun");
        }

        private static async Task PrintRacun(ODataClient client, int key)
        {
            var racun = await client.For<Racun>()
                .Key(key)
                .FindEntryAsync();
            Console.WriteLine($"IdRacuna: {racun.Id}, datum izdavanja: {racun.DatumIzdavanja}, JIR: {racun.JIR}, iznos: {racun.UkupanIznos}");
        }

        private static async Task FetchStavke(ODataClient client)
        {
            //Get count of collection
            stavkeCount = await client.For<Stavka>().Count().FindScalarAsync<int>();
            Console.WriteLine("Broj svih stavki: " + stavkeCount);

            //Get all stavke with price larger than 6
            var stavke = await client.For<Stavka>()
                .Filter(s => s.Cijena > 6)
                .OrderByDescending(s => s.Id)
                .FindEntriesAsync();
            foreach (var stavka in stavke)
            {
                Console.WriteLine($"Id: {stavka.Id}, cijena: {stavka.Cijena}, naziv: {stavka.Naziv}");
            }
        }
    }
}
