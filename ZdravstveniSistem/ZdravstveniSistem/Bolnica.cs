using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravstveniSistem
{
    public class Bolnica
    {

        private string naziv;
        private List<Prostorija> listaProstorija = new List<Prostorija>();
        private List<Oprema> magacin = new List<Oprema>();

        public Bolnica(string naziv)
        {
            this.naziv = naziv;
        }
        public Bolnica(string naziv, List<Prostorija> listaProstorija)
        {
            this.naziv = naziv;
            this.listaProstorija = listaProstorija;
        }
        public Bolnica(string naziv, List<Prostorija> listaProstorija, List<Oprema> magacin)
        {
            this.naziv = naziv;
            this.listaProstorija = listaProstorija;
            this.magacin = magacin;

        }


    }
}
