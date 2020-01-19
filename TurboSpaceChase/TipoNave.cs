using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TurboSpaceChase
{
    public class TipoNave
    {
        public Tipo tipo;
        public TipoCaracteristicas tipoCarac;

        public TipoNave() { }

        public TipoNave(Tipo _tipo, TipoCaracteristicas _tipoCarac)
        {
            this.tipo = _tipo;
            this.tipoCarac = _tipoCarac;
        }

        public enum Tipo
        {
            defenseship,
            attackship,
        }

        public enum TipoCaracteristicas
        {
            Ataque,
            Defesa
        }

        public List<Tipo> getTipos()
        {
            List<Tipo> lista = new List<Tipo>();
            lista.Add(Tipo.defenseship);
            lista.Add(Tipo.attackship);
            return lista;
        }

        public List<TipoNave> getTiposNaves()
        {
            List<TipoNave> lista = new List<TipoNave>();
            lista.Add(new TipoNave(Tipo.defenseship,TipoCaracteristicas.Defesa));
            lista.Add(new TipoNave(Tipo.attackship,TipoCaracteristicas.Ataque));
            return lista;
        }
    }
}
