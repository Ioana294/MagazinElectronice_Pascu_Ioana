using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagazinElectronice_Pascu_Ioana.Models;

namespace MagazinElectronice_Pascu_Ioana.Data
{
    public class DbInitializer
    {
        public static void Initialize(MagazinElectronice context)
        {
            context.Database.EnsureCreated();

            if (context.Devices.Any())
            {
                return;
            }
            var devices = new Device[]
            {
                new Device{Denumire="Telefon SAMSUNG Galaxy A12 4G, 64GB, 4GB RAM, Dual SIM",Descriere="Galaxy A12 combină estetica unui design simplificat cu nuanțe clasice. Curbele rafinate îl fac confortabil de ținut în mână și asigură o navigare ușoară pe ecran. Alege dintre culorile negru, alb și albastru.", Pret=Decimal.Parse("800")},
                new Device{Denumire="Tableta LENOVO Tab M10 TB-X606X, 10.3, 32GB, 2GB RAM, Wi-Fi + 4G", Descriere="Cu design-ul sau modern din metal, LENOVO Tab M10 TB-X606X va va incanta ochii. Display-ul FHD+ de 10.3 si cele doua difuzoare Dolby Atmos va vor oferi un divertisment imersiv.",Pret=Decimal.Parse("680")},
                new Device{Denumire="Smartwatch HUAWEI Watch 3 Pro Classic Edition, eSIM, Android/iOS",Descriere="Baterie care dureaza. Design elegant. Interactiune smart. Ramai conectat nonstop.",Pret=Decimal.Parse("1630")}
            };
            foreach (Device d in devices)
            {
                context.Devices.Add(d);
            }
            context.SaveChanges();

            var clienti = new Client[]
            {
                new Client{Nume="Popescu",Prenume="Mihaela",Adresa="Strada Principala numarul 25",Total=Decimal.Parse("1630")},
                new Client{Nume="Aioanei",Prenume="Vlad",Adresa="Strada Universitatii numarul 20",Total=Decimal.Parse("800")},
                new Client{Nume="Hariga",Prenume="Catalina",Adresa="Strada Florilor numarul 450",Total=Decimal.Parse("680")}
            };
            foreach (Client c in clienti)
            {
                context.Clienti.Add(c);
            }
            context.SaveChanges();

            var oferte = new Oferta[]
            {
                new Oferta{Denumire="Smartphone SAMSUNG + smartwatch SAMSUNG", PretVechi=Decimal.Parse("6000"),PretRedus=Decimal.Parse("4500"), Valabilitate=DateTime.Parse("2022-01-30")},
                new Oferta{Denumire="Smartphone HUAWEI + smartwatch HUAWEI", PretVechi=Decimal.Parse("5000"),PretRedus=Decimal.Parse("3200"), Valabilitate=DateTime.Parse("2022-02-25")},
                new Oferta{Denumire="Tableta SAMSUNG + casti wireless SAMSUNG", PretVechi=Decimal.Parse("2800"),PretRedus=Decimal.Parse("1600"), Valabilitate=DateTime.Parse("2022-02-28")}
            };
            foreach (Oferta o in oferte)
            {
                context.Oferte.Add(o);
            }
            context.SaveChanges();

            var membri = new Membru[]
            {
                new Membru{Nume="Gica",Prenume="Gabriel",Adresa="Strada Portocalelor numarul 50", Puncte=Decimal.Parse("300")},
                new Membru{Nume="Bosinceanu",Prenume="Daiana",Adresa="Strada Lalelelor numarul 100", Puncte=Decimal.Parse("450")},
                new Membru{Nume="Popa",Prenume="Geanina",Adresa="Strada George Enescu numarul 12", Puncte=Decimal.Parse("250")}
            };
            foreach (Membru m in membri)
            {
                context.Add(m);
            }
            context.SaveChanges();

            var comenzi = new Comanda[]
            {
                new Comanda{DeviceID=3, ClientID=2},
                new Comanda{DeviceID=2, ClientID=1},
                new Comanda{DeviceID=1, ClientID=3},
                new Comanda{OfertaID=3, MembruID=1},
                new Comanda{OfertaID=2, MembruID=3},
                new Comanda{OfertaID=1, MembruID=2}
            };
            foreach (Comanda co in comenzi)
            {
                context.Comenzi.Add(co);
            }
            context.SaveChanges();

            var marci = new Marca[]
            {
                new Marca{DenumireMarca="Samsung", Fondator="Lee Byung-chul"},
                new Marca{DenumireMarca="Apple", Fondator="Steve Jobs"},
                new Marca{DenumireMarca="Huawei", Fondator="Ren Zhengfei"}
            };

            foreach (Marca m in marci)
            {
                context.Marci.Add(m);
            }
            context.SaveChanges();

            var marcadevices = new MarcaDevice[]
            {
                new MarcaDevice{DeviceID = devices.Single(d => d.Denumire == "Telefon SAMSUNG Galaxy A12 4G, 64GB, 4GB RAM, Dual SIM").ID, MarcaID = marci.Single(m => m.DenumireMarca == "Samsung").ID},
                new MarcaDevice{DeviceID = devices.Single(d => d.Denumire == "Tableta LENOVO Tab M10 TB-X606X, 10.3, 32GB, 2GB RAM, Wi-Fi + 4G").ID, MarcaID = marci.Single(m => m.DenumireMarca == "Lenovo").ID},
                new MarcaDevice{DeviceID = devices.Single(d => d.Denumire == "Smartwatch HUAWEI Watch 3 Pro Classic Edition, eSIM, Android/iOS").ID, MarcaID = marci.Single(m => m.DenumireMarca == "Huawei").ID},
            };

            foreach(MarcaDevice md in marcadevices)
            {
                context.MarcaDevices.Add(md);
            }
            context.SaveChanges();
        }
    }
}
