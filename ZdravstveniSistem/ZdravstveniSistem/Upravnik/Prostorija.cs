using System;
namespace ZdravstveniSistem
{
    public class Prostorija
    {
        private string naziv;
        private TipProstorije tipProstorije;
        private List<Oprema> listaOpreme = new List<Oprema>();

        public Prostorija(TipProstorije tipProstorije, string naziv)
        {
            this.tipProstorije = tipProstorije;
            this.naziv = naziv;
        }

        public Prostorija(TipProstorije tipProstorije, string naziv, List<Oprema> listaOpreme)
        {
            this.tipProstorije = tipProstorije;
            this.naziv = naziv;
            this.listaOpreme = listaOpreme;
        }
    }
}
