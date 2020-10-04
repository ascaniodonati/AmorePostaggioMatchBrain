using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

namespace AmorePostaggioMatchBrain
{
    public static class CSVUtility
    {
        public static List<CSVElement> csvList = new List<CSVElement>();   

        public static void Aggiorna(string path)
        {
            csvList.Clear();

            StreamReader sr = new StreamReader(path);
            while(!sr.EndOfStream)
            {
                string currentLine = sr.ReadLine();
                string[] splittedLine = currentLine.Split("|");

                CSVElement csvElement = new CSVElement
                {
                    Nome = splittedLine[1],
                    InMusica = splittedLine[2],
                    InSerieTV = splittedLine[3],
                    InFilm = splittedLine[4],
                    InGiappo = splittedLine[5],
                    InLibri = splittedLine[6],
                    InGames = splittedLine[7],
                    InSport = splittedLine[8],
                    SabTranq = double.Parse(splittedLine[9]),
                    SabLuoghi = splittedLine[10],
                    SabCompagnia = splittedLine[11],
                    SabHobby = splittedLine[12],
                    SabFine = double.Parse(splittedLine[13]),
                    InOther = splittedLine[14],
                    Blacklist = splittedLine[15]
                };

                csvList.Add(csvElement);
            }

            csvList.OrderBy(s => s.Nome).ToList();
            sr.Close();
        }

        public static string? Doppioni()
        {
            Dictionary<string, int> gruppi = new Dictionary<string, int>();
            string finale = "";

            foreach (CSVElement e in csvList)
            {
                if (gruppi.ContainsKey(e.Nome.ToLower()))
                {
                    gruppi[e.Nome.ToLower()]++;
                }
                else
                {
                    gruppi.Add(e.Nome.ToLower(), 1);
                }
            }

            foreach (KeyValuePair<string, int> kvp in gruppi)
            {
                if (kvp.Value > 1) { finale += $"{kvp.Key} viene ripetuto {kvp.Value} volte\n"; }
            }

            if (finale != "") { return finale; } else { return null; }
        }

        public static double ConfrontaElementi(CSVElement first, CSVElement second)
        {
            double matchScore = 0;

            matchScore += ConfrontoInteressi(first.InMusica, second.InMusica);
            matchScore += ConfrontoInteressi(first.InFilm, second.InFilm);
            matchScore += ConfrontoInteressi(first.InGiappo, second.InGiappo);
            matchScore += ConfrontoInteressi(first.InLibri, second.InLibri);
            matchScore += ConfrontoInteressi(first.InSerieTV, second.InSerieTV);
            matchScore += ConfrontoInteressi(first.InGames, second.InGames);
            matchScore += ConfrontoInteressi(first.InSport, second.InSport);
            matchScore += ConfrontoInteressi(first.InOther, second.InOther);

            //Sabato sera
            if (!(first.SabTranq == 0 || second.SabTranq == 0))
            {
                matchScore += CoppieUtility.PointDifference(first.SabTranq, second.SabTranq);
            }

            if (!(first.SabLuoghi.ToLower() == "" || second.SabLuoghi.ToLower() == ""))
            {
                matchScore += NumeroCoseInComune(first.SabLuoghi.ToLower().Split(','), second.SabLuoghi.ToLower().Split(','), false);
            }

            if (!(first.SabHobby.ToLower() == "" || second.SabHobby.ToLower() == ""))
            {
                matchScore += NumeroCoseInComune(first.SabHobby.ToLower().Split(','), second.SabHobby.ToLower().Split(','), false);
            }

            if (!(first.SabCompagnia.ToLower() == "" || second.SabCompagnia.ToLower() == "") && (first.SabCompagnia == second.SabCompagnia))
            {
                matchScore += 10;
            }

            if (!(first.SabFine == 0 || second.SabFine == 0))
            {
                matchScore += CoppieUtility.PointDifference(first.SabFine, second.SabFine);
            }
            
            return matchScore;
        }

        public static double ConfrontoInteressi(string first, string second)
        {
            int matchScore = 0;

            if ((first.ToLower() == "No" || first.ToLower() == "") && (second.ToLower() == "No" || second.ToLower() == ""))
            {
                matchScore += 10;
            }
            if (first.ToLower() != "No" && second.ToLower() != "No")
            {
                matchScore += 20;
            }

            if ((first.ToLower() == "No" && second.ToLower() != "No") || (second.ToLower() == "No" && first.ToLower() != "No"))
            {
                matchScore -= 20;
            }

            matchScore += NumeroCoseInComune(first.ToLower().Split(','), second.ToLower().Split(','), true);

            return matchScore;
        }

        public static List<string> ListaNomi()
        {
            return csvList.Select(s => s.Nome).OrderBy(s => s).ToList();
        }

        public static CSVElement FindByName(string name)
        {
            return csvList.Where(s => s.Nome == name).FirstOrDefault();
        }

        public static string PiselloQuadrato(string name)
        {
            string boh = "";
            CSVElement element = CSVUtility.FindByName(name);

            boh += name + "\n";
            boh += "Musica: " + element.InMusica + "\n";
            boh += "Film: " + element.InFilm + "\n";
            boh += "SerieTV: " + element.InSerieTV + "\n";
            boh += "Giappo: " + element.InGiappo + "\n";
            boh += "Libri: " + element.InLibri + "\n";
            boh += "Giochi: " + element.InGames + "\n";
            boh += "Sport: " + element.InSport + "\n";
            boh += "Tranquillo il sabato?: " + element.SabTranq + "\n";
            boh += "Luoghi del sabato: " + element.SabLuoghi + "\n";
            boh += "Compagnia sabato: " + element.SabCompagnia + "\n";
            boh += "Hobby sabato: " + element.SabHobby + "\n";
            boh += "Fine del sabato: " + element.SabFine + "\n";
            boh += "Altri interessi: " + element.InOther + "\n";
            boh += "Blacklist: " + element.Blacklist + "\n\n\n";

            return boh;
        }

        public static int NumeroCoseInComune(String[] first, String[] second, bool Nerd)
        {
            int count = new int();

            if (Nerd) { count = 30; } else { count = 10; }

            foreach (string str in first) { if (second.Contains(str.Trim())) { if (Nerd) { count += 15; } else { count += 5; } } }

            if (Nerd && count > 30)
                { return count; }
            
            if (!Nerd && count > 10)
                { return count; }
            
            return 0;
        }

    }
}