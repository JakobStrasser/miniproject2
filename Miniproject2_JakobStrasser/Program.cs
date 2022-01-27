using System;
using Microsoft.EntityFrameworkCore.SqlServer;
using System.Globalization;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Miniproject2_JakobStrasser
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console and culture settings
            Console.ForegroundColor = ConsoleColor.White;
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            //Uncomment to set up initial data
            InitialSetup();

            //Welcome message
            Console.WriteLine("Asset management console application!");

            //Print list
            PrintList();

            string menuMessage = "Välj (S)kapa tillgång, (T)a bort tillgång, (U)ppdatera tillgång, (V)isa tillgångar, (Q)uit:";

            string input = "";
            bool exit = false;

            while (!exit)
            {
                Console.Write(menuMessage);
                input = Console.ReadLine();
                switch (input.ToUpperInvariant())
                {
                    case "S":
                        CreateAsset();
                        break;
                    case "T":
                        DeleteAsset();
                        break;
                    case "U":
                        UpdateAsset();
                        break;
                    case "V":
                        PrintList();
                        break;
                    case "Q":
                        exit = true;
                        break;
                    default:
                        Console.Clear();
                        continue;
                }
            }
        }
        /// <summary>
        /// Asks for Id then updates that asset
        /// </summary>
        private static void UpdateAsset()
        {
            AssetDbContext dbContext = new AssetDbContext();
            Console.Write("Välj Id för tillgången som ska uppdateras: ");
            int idToUpdate = 0;
            try
            {
                idToUpdate = int.Parse(Console.ReadLine());
                var assetToUpdate = dbContext.Assets.Find(idToUpdate);
                dbContext.Assets.Update(assetToUpdate);

                switch (assetToUpdate.GetType().ToString())
                {
                    case "Miniproject2_JakobStrasser.Computer":
                        Console.Write("Modell ({0}): ", assetToUpdate.Model);
                        string model = Console.ReadLine();
                        Console.Write("Inköpsdatum (YYYY-MM-DD) ({0}): ", assetToUpdate.PurchaseDate.ToShortDateString());
                        DateTime purchaseDate = DateTime.Parse(Console.ReadLine());
                        Console.Write("Pris i USD ($) ({0}): ", assetToUpdate.USDPrice);
                        double purchasePriceUSD = Double.Parse(Console.ReadLine());
                        ListOFfices();
                        Console.WriteLine("Välj kontor ({0}): ", assetToUpdate.OfficeId);
                        int officeID = int.Parse(Console.ReadLine());
                        Console.Write("Minne i GB ({0}): ", ((Computer)assetToUpdate).MemorySize);
                        int memorySize = int.Parse(Console.ReadLine());
                        Console.Write("Disk i GB ({0}): ", ((Computer)assetToUpdate).DiskSize);
                        int diskSize = int.Parse(Console.ReadLine());
                        assetToUpdate.Model = model;
                        assetToUpdate.PurchaseDate = purchaseDate;
                        assetToUpdate.USDPrice = purchasePriceUSD;
                        assetToUpdate.OfficeId = officeID;
                        ((Computer)assetToUpdate).MemorySize = memorySize;
                        ((Computer)assetToUpdate).DiskSize = diskSize;
                        
                        break;
                    case "Miniproject2_JakobStrasser.Phone":
                        Console.Write("Modell ({0}): ", assetToUpdate.Model);
                        model = Console.ReadLine();
                        Console.Write("Inköpsdatum (YYYY-MM-DD) ({0}): ", assetToUpdate.PurchaseDate.ToShortDateString());
                        purchaseDate = DateTime.Parse(Console.ReadLine());
                        Console.Write("Pris i USD ($) ({0}): ", assetToUpdate.USDPrice);
                        purchasePriceUSD = Double.Parse(Console.ReadLine());
                        ListOFfices();
                        Console.WriteLine("Välj kontor ({0}): ", assetToUpdate.OfficeId);
                        officeID = int.Parse(Console.ReadLine());
                        Console.Write("Telefonnummer ({0}): ", ((Phone)assetToUpdate).PhoneNumber);
                        string phoneNumber = Console.ReadLine();
                        Console.Write("PUK ({0}): ", ((Phone)assetToUpdate).PUK);
                        string pUK = Console.ReadLine();

                        assetToUpdate.Model = model;
                        assetToUpdate.PurchaseDate = purchaseDate;
                        assetToUpdate.USDPrice = purchasePriceUSD;
                        assetToUpdate.OfficeId = officeID;
                        ((Phone)assetToUpdate).PhoneNumber = phoneNumber;
                        ((Phone)assetToUpdate).PUK = pUK;
                       
                        break;
                    default:
                        Console.Write("Fel inmatning, åter till huvudmenyn.");
                        return;
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine($"Hittade inte tillgången med Id: {idToUpdate}");
            }
            catch (FormatException)
            {
                Console.Write("Fel inmatning, åter till huvudmenyn.");
                return;
            }
            finally
            {
                dbContext.SaveChanges();
                Console.WriteLine("Uppdaterade tillgången med Id: " + idToUpdate);
            }

        }
        /// <summary>
        /// Asks for Id then removes asset with that Id
        /// </summary>
        private static void DeleteAsset()
        {
            Console.Write("Välj Id för tillgången som ska tas bort: ");
            int idToRemove;
            while(!int.TryParse(Console.ReadLine(),out idToRemove))
            {
                Console.WriteLine("Id måste vara en siffra, försök igen: ");
            }
            AssetDbContext dbContext = new AssetDbContext();
            try
            {
                dbContext.Assets.Remove(dbContext.Assets.Find(idToRemove));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine($"Hittade inte tillgången med Id: {idToRemove}");
            }
            finally
            {
                dbContext.SaveChanges();
                Console.WriteLine("Tog bort tillgång med Id: " + idToRemove);
            }

        }

        /// <summary>
        /// Creates an asset
        /// </summary>
        private static void CreateAsset()
        {
            Console.Write("Skapa (1) Dator eller (2) Telefon: ");
            string input = Console.ReadLine();
            AssetDbContext dbContext = new AssetDbContext();
            try
            {
                switch (input.ToUpperInvariant())
                {
                    case "1":
                        Console.Write("Modell: ");
                        string model = Console.ReadLine();
                        Console.Write("Inköpsdatum (YYYY-MM-DD): ");
                        DateTime purchaseDate = DateTime.Parse(Console.ReadLine());
                        Console.Write("Pris i USD ($): ");
                        double purchasePriceUSD = Double.Parse(Console.ReadLine());
                        ListOFfices();
                        Console.WriteLine("Välj kontor: ");
                        int officeID = int.Parse(Console.ReadLine());
                        Console.Write("Minne i GB: ");
                        int memorySize = int.Parse(Console.ReadLine());
                        Console.Write("Disk i GB: ");
                        int diskSize = int.Parse(Console.ReadLine());
                        dbContext.Assets.Add(new Computer(model, purchaseDate, purchasePriceUSD, officeID, memorySize, diskSize));
                        dbContext.SaveChanges();
                        break;
                    case "2":
                        Console.Write("Modell: ");
                        model = Console.ReadLine();
                        Console.Write("Inköpsdatum (YYYY-MM-DD): ");
                        purchaseDate = DateTime.Parse(Console.ReadLine());
                        Console.Write("Pris i USD ($): ");
                        purchasePriceUSD = Double.Parse(Console.ReadLine());
                        ListOFfices();
                        Console.WriteLine("Välj kontor: ");
                        officeID = int.Parse(Console.ReadLine());
                        Console.Write("Telefonnummer: ");
                        string phoneNumber = Console.ReadLine();
                        Console.Write("PUK: ");
                        string pUK = Console.ReadLine();
                        dbContext.Assets.Add(new Phone(model, purchaseDate, purchasePriceUSD, officeID, phoneNumber, pUK));
                        dbContext.SaveChanges();
                        break;
                    default:
                        Console.Write("Fel inmatning, åter till huvudmenyn.");
                        return;
                }
            }
            catch (FormatException)
            {
                Console.Write("Fel inmatning, åter till huvudmenyn.");
                return;
            }
        }
        /// <summary>
        /// Lists offices
        /// </summary>
        private static void ListOFfices()
        {
            Console.WriteLine();
            AssetDbContext dbContext = new AssetDbContext();
            Console.WriteLine("Id".PadRight(10) + "Country".PadRight(20));
            foreach (Office o in dbContext.Offices.ToList())
            {
                Console.WriteLine(o.Id.ToString().PadRight(10) + o.Country.PadRight(20));
            }
        }

        /// <summary>
        /// Prints the list of assets with header and average price in USD and average age in days
        /// </summary>
        private static void PrintList()
        {
            using AssetDbContext dbContext = new AssetDbContext();
            

            //Print header
            string header = "";
            header += "Id".PadRight(4);
            header += "Model".PadRight(20);

            header += "Purchase Date".PadRight(20);
            header += "Price".PadRight(20);
            header += "Location".PadRight(20);
            header += "Memory (GB)".PadRight(12);
            header += "Disk (GB)".PadRight(12);
            header += "Phone number".PadRight(20);
            header += "PUK".PadRight(20);

            Console.WriteLine(header);
            foreach (Asset a in dbContext.Assets.ToList())
            {
                string row = "";
                row += a.Id.ToString().PadRight(4);
                row += (a.Model.PadRight(20));

                row += (a.PurchaseDate.ToShortDateString().PadRight(20));

                //Set culture to get currency symbol
                CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(dbContext.Offices.Find(a.OfficeId).Culture);
                row += (a.USDPrice * dbContext.Currencies.First(c => c.Name == dbContext.Offices.Find(a.OfficeId).Currency).Rate).ToString("C2").PadRight(20);
                CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

                row += dbContext.Offices.Find(a.OfficeId).Country.PadRight(20);

                //By type additional columns
                switch (a.GetType().ToString())
                {
                    case "Miniproject2_JakobStrasser.Computer":
                        row += ((Computer)a).MemorySize.ToString().PadRight(12);
                        row += ((Computer)a).DiskSize.ToString().PadRight(12);
                        row += "".PadRight(20);
                        row += "".PadRight(20);
                        break;
                    case "Miniproject2_JakobStrasser.Phone":
                        row += "".PadRight(12);
                        row += "".PadRight(12);
                        row += ((Phone)a).PhoneNumber.PadRight(20);
                        row += ((Phone)a).PUK.PadRight(20);
                        break;
                    default:
                        row += "".PadRight(12);
                        row += "".PadRight(12);
                        row += "".PadRight(20);
                        row += "".PadRight(20);
                        break;
                }

                //Set text color
                if (a.DaysOld >= 915)
                    Console.ForegroundColor = ConsoleColor.Yellow;
                if (a.DaysOld >= 1005)
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(row);
                //Reset text color
                Console.ForegroundColor = ConsoleColor.White;
            }

            var averageAge = dbContext.Assets.ToList().Average(a => a.DaysOld);
            var averagePriceUSD = dbContext.Assets.Average(a => a.USDPrice);
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            Console.WriteLine("Average price of assets: {0:C2}", averagePriceUSD);
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            Console.WriteLine($"Average age of assets: {averageAge} days");
        }
        
        /// <summary>
        /// Sets up inital data
        /// </summary>
        private static void InitialSetup()
        {
            AssetDbContext dbContext = new AssetDbContext();
            // add offices
            dbContext.Add(new Office("Sweden", "SEK", "sv-SE"));
            dbContext.Add(new Office("United Kingdom", "GBP", "en-GB"));
            dbContext.Add(new Office("Denmark", "EUR", "da-DK"));
            dbContext.SaveChanges();

            //add currencies
            dbContext.Add(new Currency("SEK", 9.26));
            dbContext.Add(new Currency("GBP", 0.76));
            dbContext.Add(new Currency("EUR", 0.89));
            dbContext.SaveChanges();

            //add test data
            dbContext.Add(new Computer("ASUS Zenbook", DateTime.Now.AddDays(-1015), new Random().NextDouble() * 3000, dbContext.Offices.FirstOrDefault(office => office.Country == "Sweden").Id, 16, 1024));
            dbContext.Add(new Computer("Apple MacBook Pro  ", DateTime.Now.AddDays(-939), new Random().NextDouble() * 3000, dbContext.Offices.FirstOrDefault(office => office.Country == "Denmark").Id, 16, 512));
            dbContext.Add(new Phone("Apple iPhone 9", DateTime.Now.AddDays(-237), new Random().NextDouble() * 1500, dbContext.Offices.FirstOrDefault(office => office.Country == "United Kingdom").Id, "120937102973", "190273092173021973"));
            dbContext.SaveChanges();

        }
    }

}
