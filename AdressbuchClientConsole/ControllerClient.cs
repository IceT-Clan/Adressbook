﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using __ClientSocket__;

namespace Adressbuch
{
    enum ServerCommand
    {
        NONE,
        FINDPERSONS,
        GETALLPERSONS,
        ADDPERSON,
        DELETEPERSON
    }

    enum ClientInfo
    {
        NOMOREDATA,
        MOREDATA
    }

    class ControllerClient
    {
        private ClientSocket client;
        private View view;
        private string host;
        private int port;

        public ControllerClient(string _host, int _port)
        {
            host = _host;
            port = _port;
            // Zugriff auf die View
            view = new Adressbuch.View();
        }

        // Hiermit wird der Client gestartet
        public int start()
        {
            // Hier erfolgt die Interaktion mit dem Benutzer
            // Die Ausgaben können in einem View-Objekt erfolgen

            int eingabe = 0;

            // Menü ausgeben und Auswahl treffen
            eingabe = menue();

            switch (eingabe)
            {
                // Suche Personen
                case 1:
                    suchePersonen();
                    break;

                // Hole Adressbuch
                case 2:
                    // holeAdressbuch(),
                    break;

                case 9:
                    break;

                default:
                    break;
            } // Ende switch

            return eingabe;
        }

        private int menue()
        {
            int auswahl = 0;

            view.zeigeMenue();

            // Auswahl lesen
            do
            {
                auswahl = Convert.ToInt32(Console.ReadLine());
            } while (auswahl < 1 || auswahl > 9);

            Console.WriteLine();

            // auswahl zurück liefern
            return auswahl;
        }

        private void suchePersonen()
        {
            // Suchbegriff abfragen
            Console.Write("Suchbegriff> ");
            string suchbegriff = Console.ReadLine();


            // Hier müsste eine Ausnahmebehandlung erfolgen
            // falls keine Verbindung möglich ist
            client = new ClientSocket(host, port);
            try
            {
                // Verbindung mit Server herstellen
                client.Connect();
                
                // Kommando senden
                client.Write((int)ServerCommand.FINDPERSONS);

                // Suchstring senden
                client.Write(suchbegriff);

                // Anzahl gefundener Personen lesen
                int anzahl = client.Read();

                Console.WriteLine("Anzahl gefundener Personen: {0}", anzahl);

                if (anzahl > 0)
                {
                    List<Person> ergebnis = new List<Person>();

                    for (int i = 0; i < anzahl; i++)
                    {
                        string person = client.ReadLine();

                        // Testausgabe
                        // Console.WriteLine(person);

                        // Person-Objekt aus empfangenem String
                        Person p = convertString2Person(person);

                        // Person-Objekt in die Liste für die Anzeige
                        ergebnis.Add(p);
                    } // Ende for

                    // Daten anzeigen
                    view.aktualisiereSicht(ergebnis);

                } // End if

                client.Close();

            }
            catch (Exception)
            {
                throw;
            }

        }

        private void holeAdressbuch()
        {
            client = new ClientSocket(host, port);
            try
            {
                client.Connect();
                client.Write((int)ServerCommand.GETALLPERSONS);
                int count = client.Read();

                if (count >= 1)
                {
                    List<Person> erg = new List<Person>();
                    for (int i = 0; i < count; i++)
                    {
                        string person = client.ReadLine();
                        Person p = convertString2Person(person);
                        erg.Add(p);
                    }
                    view.aktualisiereSicht(erg);
                }
                client.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private Person convertString2Person(string _p)
        {
            char[] separator = { ';' };
            string[] daten = _p.Split(separator);

            // Geburtsdatum umformen, um ein DateTime-Objekt
            // zu erstellen
            char[] trenner = { ',' };
            string[] geburtsdatum = daten[3].Split(trenner);

            int tag = Convert.ToInt32(geburtsdatum[0]);
            int monat = Convert.ToInt32(geburtsdatum[1]);
            int jahr = Convert.ToInt32(geburtsdatum[2]);

            DateTime datum = new DateTime(jahr, monat, tag);

            // Person-Objekt erstellen und der Liste hinzufügen
            Person p = new Person(daten[0], daten[1], datum, daten[2], daten[3], daten[4], daten[5], Int32.Parse(daten[6]), Int32.Parse(daten[7]), daten[8], daten[9], daten[10]);

            return p;
        }

        private string convertPerson2String(Person _p)
        {
            string person = "";

            // Hier wird ein Person-Objekt in den String umgeformt

            return person;
        }

    }
}
