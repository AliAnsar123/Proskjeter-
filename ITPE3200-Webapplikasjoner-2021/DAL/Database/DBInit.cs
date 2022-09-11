using System;
using System.Collections.Generic;
using System.Linq;
using Gruppeoppgave_1.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Gruppeoppgave_1.DAL
{
    public class DBInit
    {
        public static void Initialize(IApplicationBuilder app)
        {
            var serviceScope = app.ApplicationServices.CreateScope();

            var db = serviceScope.ServiceProvider.GetService<DatabaseContext>();

            Random random = new Random();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            
            Company colorLine = new Company { Name = "Color Line" };
            Company fjordLine = new Company { Name = "Fjord Line" };
            Company fjord1    = new Company { Name = "Fjord 1" };

            db.Companies.AddRange(colorLine, fjordLine, fjord1);

            // downloaded all norwegian zip codes from https://www.bring.no/tjenester/adressetjenester/postnummer
            string[] csv = System.IO.File.ReadAllLines(@"./post.csv");
            for (int i = 1; i < csv.Length; i++)
            {
                string[] dataRow = csv[i].Split(';');
                for (int j = 0; j < dataRow.Length; j += 5)
                {
                    ZipCode zip = new ZipCode { Id = dataRow[j], City = dataRow[j + 1] };
                    db.ZipCodes.Add(zip);
                }
            }

            ZipCode zip1 = db.ZipCodes.Find("0001");
            ZipCode zip2 = db.ZipCodes.Find("3012");
            ZipCode zip3 = db.ZipCodes.Find("3050");

            Customer customer1 = new Customer { Email = "test1@email.com", FirstName = "Fornavn", LastName = "Etternavn", Phone = 95413314, Street = "Osloveien 23", ZipCode = zip1 };
            Customer customer2 = new Customer { Email = "test2@email.com", FirstName = "Fornavn", LastName = "Etternavn", Phone = 12313314, Street = "Drammensveien 135", ZipCode = zip2 };
            Customer customer3 = new Customer { Email = "test3@email.com", FirstName = "Fornavn", LastName = "Etternavn", Phone = 12313514, Street = "Mjøndalensveien 35", ZipCode = zip3 };

            db.Customers.AddRange(customer1, customer2, customer3);

            Port kiel =         new Port { Name = "Kiel" };
            Port oslo =         new Port { Name = "Oslo" };
            Port sandefjord =   new Port { Name = "Sandefjord" };
            Port stromstad =    new Port { Name = "Strømstad" };
            Port hirtshals =    new Port { Name = "Hirtshals" };
            Port kristiansand = new Port { Name = "Kristiansand" };

            db.Ports.AddRange(kiel, oslo, sandefjord, stromstad, hirtshals, kristiansand);
            
            Route osloKiel =                     new Route { Company = fjord1,    Origin = oslo,         Destination = kiel };
            Route osloHirtshals =                new Route { Company = colorLine, Origin = oslo,         Destination = hirtshals };
            Route sandefjordStromstad =          new Route { Company = fjord1,    Origin = sandefjord,   Destination = stromstad };
            Route kristiansandKiel =             new Route { Company = fjordLine, Origin = kristiansand, Destination = kiel };
            Route kielOslo =                     new Route { Company = fjordLine, Origin = kiel,         Destination = oslo };
            Route hirtshalsOsloFjordLine =       new Route { Company = fjordLine, Origin = hirtshals,    Destination = oslo };
            Route kielKristiansandFjord1 =       new Route { Company = fjord1,    Origin = kiel,         Destination = kristiansand };
            Route stromstadSandefjordColorLine = new Route { Company = colorLine, Origin = stromstad,    Destination = sandefjord };


            db.Routes.AddRange(osloKiel, osloHirtshals, sandefjordStromstad, kristiansandKiel);

            db.SaveChanges();

            // creates random routetimes
            for (int i = 1; i <= 4; i++)
            {
                Route randomRoute = db.Routes.Find(i);

                for (int j = 0; j < 100; j++)
                {
                    int randomMonth = random.Next(10, 12);
                    int randomDay = random.Next(1, 31);

                    RouteTime newRouteTime = new RouteTime {
                        Route     = randomRoute,
                        Price     = random.Next(100, 5000),
                        Date      = new DateTime(2021, randomMonth, randomDay),
                        Direction = random.Next(0, 2)
                    };

                    db.RouteTimes.Add(newRouteTime);
                }
            }

            db.SaveChanges();

            // creates random orders
            for (int i = 0; i < 2; i++)
            {
                int randomRouteId = random.Next(1, 5);
                RouteTime randomDepartureRouteTime = db.RouteTimes.FirstOrDefault(rt => rt.Direction == 0 && rt.Route.Id == randomRouteId);
                RouteTime randomReturnRouteTime    = db.RouteTimes.FirstOrDefault(rt => rt.Direction == 1 && rt.Route.Id == randomRouteId);

                if (randomDepartureRouteTime == null)
                {
                    // jumps to next loop iteration if we were unable to find a routetime for the selected route
                    continue;
                }

                Order newOrder = new Order {
                    DepartureRouteTime = randomDepartureRouteTime,
                    ReturnRouteTime    = randomReturnRouteTime,
                    NumberOfVehicles   = random.Next(0, 10),
                    IsRoundTrip        = randomReturnRouteTime != null,
                    MainCustomer       = customer1,
                    Customers          = new List<Customer> {
                        customer2
                    },
                };

                db.Orders.Add(newOrder);
            }


            // creates admin user
            byte[] salt = UserRepository.GenerateSalt();
            byte[] hash = UserRepository.GenerateHash(password: "admin123", salt);

            User adminUser = new User {
                Username = "admin",
                Password = hash,
                Salt     = salt
            };

            db.Users.Add(adminUser);

            db.SaveChanges();
        }
    }
}
    