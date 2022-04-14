using System;
namespace ZdravstveniSistem
{
    public class Oprema
    {
        private TipOpreme tipOpreme;
        private string naziv;
        private int kolicina = 0;

        public Oprema(TipOpreme tipOpreme, string naziv, int kolicina)
        {
            this.naziv = naziv;
            this.kolicina = kolicina;
            this.tipOpreme = tipOpreme;
        }

        public Oprema(TipOpreme tipOpreme, string naziv)
        {
            this.tipOpreme = tipOpreme;
            this.naziv = naziv;
        }

    }
}
