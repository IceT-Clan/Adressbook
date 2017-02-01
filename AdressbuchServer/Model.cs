﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adressbuch {
    enum SearchType {
        BASIC_REGEX,
        REGEX,
        PERL_REGEX,
        FIXED_STRING
    }

    // Diese Klasse stellt das Datenmodell für das Adressbuch
    // und notwendige Methoden für die Datenverarbeitung
    // zur Verfügung
    class Model {
        private string _path;
        private List<Person> _persons;

        public Model(string path) {
            _persons = new List<Person>();

            _path = path;
            readAddressbook();
        }

        public List<Person> search(string pattern, SearchType searchType=SearchType.FIXED_STRING) {
            List<Person> results = new List<Person>();
            switch (searchType) {
                case SearchType.BASIC_REGEX:
                    throw new NotImplementedException();
                    break;
                case SearchType.REGEX:
                    throw new NotImplementedException();
                    break;
                case SearchType.PERL_REGEX:
                    throw new NotImplementedException();
                    break;
                case SearchType.FIXED_STRING:

                    break;
                default:
                    throw new NotImplementedException();
                    break;
            }

            return results;
        }

        private List<Person> search_fixed_string(string pattern) {
            List<Person> result = new List<Person>();
            foreach (Person person in _persons) {
                if (person.Name.Contains(pattern) ||
                    false) {
                    result.Add(person);
                } 
            }
            return result;
        }

        public List<Person> getall() {
            return _persons;
        }

        private void readAddressbook() {
            if (System.IO.File.Exists(_path) == false) {
                Console.WriteLine("No" + _path + "found!\n");
                File.Create(_path);
            }

            using (StreamReader reader = new StreamReader(_path)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    _persons.Add(new Person(line));
                }
            }

        }

        private Person convertString2Person(string _p) {
            char[] separator = { ';' };
            string[] daten = _p.Split(separator);

            // Geburtsdatum umformen, um ein DateTime-Objekt
            // zu erstellen
            char[] trenner = { '.' };
            string[] geburtsdatum = daten[3].Split(trenner);

            int tag = Convert.ToInt32(geburtsdatum[0]);
            int monat = Convert.ToInt32(geburtsdatum[1]);
            int jahr = Convert.ToInt32(geburtsdatum[2]);

            DateTime datum = new DateTime(jahr, monat, tag);

            // Person-Objekt erstellen und der Liste hinzufügen
            Person p = new Person(daten[0], daten[1], datum, daten[2], daten[3], daten[4], daten[5], Int32.Parse(daten[6]), Int32.Parse(daten[7]), daten[8], daten[9], daten[10]);

            return p;
        }
    }
}
