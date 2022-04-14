using System;
namespace ZdravstveniSistem
{
    public class Upravnik : Korisnik
    {
        private Bolnica bolnica;

        public Upravnik(string korisnickoIme, string lozinka, Bolnica bolnica)
        {
            this.korisnickoIme = korisnickoIme;
            this.lozinka = lozinka;
            this.bolnica = bolnica;
        }
    }
}
