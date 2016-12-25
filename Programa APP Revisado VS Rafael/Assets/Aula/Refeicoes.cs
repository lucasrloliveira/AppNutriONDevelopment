using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Aula
{
    [System.Serializable]
    public class RefeicaoPredefinida
    {
        #region Private Variables

        [SerializeField]
        string m_key;
        [SerializeField]
        List<string> m_alimentosPossiveis = new List<string>();
        [SerializeField]
        List<TipoPorQuantidadeCal> m_tipoAlimentoPorQuantidade = new List<TipoPorQuantidadeCal>();

        #endregion

        #region Public Properties

        public string Key
        {
            get
            {
                return m_key;
            }
            set
            {
                if (m_key == value)
                    return;
                m_key = value;
            }
        }

        public List<string> AlimentosPossiveis
        {
            get
            {
                if (m_alimentosPossiveis == null)
                    m_alimentosPossiveis = new List<string>();
                return m_alimentosPossiveis;
            }
            set
            {
                if (m_alimentosPossiveis == value)
                    return;
                m_alimentosPossiveis = value;
            }
        }

        public List<TipoPorQuantidadeCal> TipoAlimentoPorQuantidade
        {
            get
            {
                if (m_tipoAlimentoPorQuantidade == null)
                    m_tipoAlimentoPorQuantidade = new List<TipoPorQuantidadeCal>();
                return m_tipoAlimentoPorQuantidade;
            }
            set
            {
                if (m_tipoAlimentoPorQuantidade == value)
                    return;
                m_tipoAlimentoPorQuantidade = value;
            }
        }

        #endregion

        //TODO retornar os alimentos, ao pesquisar na refeicao manager, 
        //baseado na quantidade de calorias passadas por parametro
        public List<Alimento> GetAlimentos(float p_qualidadeCalorias)
        {
            int v_checkCounter = 0;
            while (v_checkCounter < 500)
            {
                List<Alimento> v_retorno = new List<Alimento>();
                List<Alimento> v_possiveisAlimentos = GetAlimentosPossiveis();

                foreach (var v_tipoPorQuantidade in m_tipoAlimentoPorQuantidade)
                {
                    for (int i = 0; i < v_tipoPorQuantidade.QuantidadeDeAlimentosDesteTipo; i++)
                    {
                        float v_quantidadeCalorias = p_qualidadeCalorias * (v_tipoPorQuantidade.QuantidadeCalPercent / v_tipoPorQuantidade.QuantidadeDeAlimentosDesteTipo);
                        List<Alimento> v_possiveisPorTipo = GetAlimentosWithNotaByTipo(v_possiveisAlimentos, v_tipoPorQuantidade.Tipo, 3); // pega os alimentos com nota maior que 3 por tipo
                        Alimento v_alimentoRandom = null;
                        int v_count = 0;
                        //Garante que nao tera o mesmo alimento
                        while ((v_retorno.Contains(v_alimentoRandom) || v_alimentoRandom == null) && v_count < 10)
                        {
                            v_alimentoRandom = GetRandomAlimento(v_possiveisPorTipo);
                            v_count++;
                        }
                        if (v_alimentoRandom != null)
                        {
                            float v_fatorMultiplicador = v_quantidadeCalorias / v_alimentoRandom.QuantidadeCal;
                            //Altera alimento duplicado para apresentar os valores corretos dado a quantidade de Calorias
                            v_alimentoRandom.ModificaAlimentoPorFatorMultiplicator(v_fatorMultiplicador);
                            v_retorno.Add(v_alimentoRandom);
                        }
                    }
                }
                if (PratoIsValid(p_qualidadeCalorias, v_retorno))
                    return v_retorno;
                v_checkCounter++;
            }
            return new List<Alimento>();
        }

        public bool PratoIsValid(float p_qualidadeCaloriasDesejada, List<Aula.Alimento> p_alimentos)
        {
            bool v_sucess = false;
            if (p_alimentos != null)
            {
                float v_quantidadeCalorias = 0;
                foreach (var v_alimento in p_alimentos)
                {
                    if (v_alimento != null)
                        v_quantidadeCalorias += v_alimento.QuantidadeCal;
                }
                float v_delta = Mathf.Abs(v_quantidadeCalorias - p_qualidadeCaloriasDesejada);
                if (v_delta <= p_qualidadeCaloriasDesejada * 0.2f)
                {
                    v_sucess = true;
                }
            }
            return v_sucess;
        }

        public List<Alimento> GetAlimentosPossiveis()
        {
            List<Alimento> v_possiveisAlimentos = new List<Alimento>();
            foreach (var v_key in AlimentosPossiveis)
            {
                Alimento v_alimento = RefeicaoManager.Instance.GetAlimentoByKey(v_key);
                if (v_alimento != null)
                    v_possiveisAlimentos.Add(v_alimento);
            }
            return v_possiveisAlimentos;
        }

        public List<Alimento> GetAlimentosWithNotaByTipo(AlimentoTipoEnum p_alimentoTipo, int p_nota = 3)
        {
            return GetAlimentosWithNotaByTipo(GetAlimentosPossiveis(), p_alimentoTipo, p_nota);
        }

        public List<Alimento> GetAlimentosWithNotaByTipo(List<Alimento> p_alimentosPossiveis, AlimentoTipoEnum p_alimentoTipo, int p_nota = 3)
        {
            List<Alimento> v_listRetorno = new List<Alimento>();
            foreach (var v_alimento in p_alimentosPossiveis)
            {
                if (v_alimento.Tipo == p_alimentoTipo && v_alimento.Nota >= p_nota)
                    v_listRetorno.Add(v_alimento);
            }
            return v_listRetorno;
        }

        /*public Alimento GetRandomAlimento(List<Alimento> p_alimentosPossiveis)
        {
            List<Alimento> v_alimentosPossiveisPonderado = new List<Alimento>();
            if (p_alimentosPossiveis != null)
            {
                foreach (var v_alimentos in p_alimentosPossiveis)
                {
                    string v_key = v_alimentos.Key;
                    if (RefeicaoManager.AlimentosMaisConsumidos.ContainsKey(v_key))
                    {
                        int v_value = RefeicaoManager.AlimentosMaisConsumidos[v_key];
                        for (int i = 0; i < v_value; i++)
                        {
                            v_alimentosPossiveisPonderado.Add(v_alimentos);
                        }
                    }
                    v_alimentosPossiveisPonderado.Add(v_alimentos);
                }
            }
            int v_index = Random.Range(0, v_alimentosPossiveisPonderado.Count);
            if (v_index < v_alimentosPossiveisPonderado.Count)
            {
                return v_alimentosPossiveisPonderado[v_index];
            }
            return null;
        }
    }*/

        public Alimento GetRandomAlimento(List<Alimento> p_alimentosPossiveis)
        {
            int v_index = Random.Range(0, p_alimentosPossiveis.Count);
            if (v_index < p_alimentosPossiveis.Count)
            {
                return p_alimentosPossiveis[v_index];
            }
            return null;
        }
    }

    [System.Serializable]
    public class TipoPorQuantidadeCal
    {
        #region Private Variables

        [SerializeField, Range(0, 1)]
        float m_quantidadeCalPercent = 1.0f;
        [SerializeField]
        AlimentoTipoEnum m_tipo = AlimentoTipoEnum.Cereais;
        [SerializeField]
        int m_quantidadeDeAlimentosDesteTipo = 1;

        #endregion

        #region Public Properties

        public int QuantidadeDeAlimentosDesteTipo
        {
            get
            {
                return m_quantidadeDeAlimentosDesteTipo;
            }
            set
            {
                if (m_quantidadeDeAlimentosDesteTipo == value)
                    return;
                m_quantidadeDeAlimentosDesteTipo = value;
            }
        }

        public float QuantidadeCalPercent
        {
            get
            {
                return m_quantidadeCalPercent;
            }
            set
            {
                if (m_quantidadeCalPercent == value)
                    return;
                m_quantidadeCalPercent = value;
            }
        }

        public AlimentoTipoEnum Tipo
        {
            get
            {
                return m_tipo;
            }
            set
            {
                if (m_tipo == value)
                    return;
                m_tipo = value;
            }
        }

        #endregion
    }

    [System.Serializable]
    public class Refeicao
    {
        [SerializeField]
        string m_nomeDaRefeicao;
        //Unity ira mostrar todas as variaveis com SerializeField nela
        [SerializeField]
        List<Alimento> m_alimentos = new List<Alimento>();

        public string NomeDaRefeicao
        {
            get
            {
                return m_nomeDaRefeicao;
            }
            set
            {
                if (m_nomeDaRefeicao == value)
                    return;
                m_nomeDaRefeicao = value;
            }
        }

        //Json Irá serializar todos as properties publicas
        public List<Alimento> Alimento
        {
            get
            {
                if (m_alimentos == null)
                    m_alimentos = new List<Aula.Alimento>();
                return m_alimentos;
            }
            set
            {
                if (m_alimentos == value)
                    return;
                m_alimentos = value;
            }
        }

    }

    public enum AlimentoTipoEnum { Cereais, Lacteos, Queijos, Proteinas, Oleaginosas, LegumesA, LegumesB, Leguminosas, Frutas, Sucos, Batatas }

    [System.Serializable]
    public class Alimento
    {
        #region Private Variables

        [SerializeField]
        string m_key; //Usado para achar o alimento quando procurado pela refeicao pre definida
        [SerializeField]
        string m_nome; //Nome para o usuario
        [SerializeField]
        AlimentoTipoEnum m_tipo = AlimentoTipoEnum.Cereais;
        [SerializeField]
        Porcao m_porcao = new Porcao();
        [SerializeField]
        float m_quantidadeCal;
        [SerializeField]
        float m_quantidadeCarb;
        [SerializeField]
        float m_quantidadeProt;
        [SerializeField]
        float m_quantidadeGord;
        [SerializeField]
        int m_nota;
        [SerializeField]
        float m_quantidadeCalUmaPorcao;
        [SerializeField]
        float m_quantidadeCarbUmaPorcao;
        [SerializeField]
        float m_quantidadeProtUmaPorcao;
        [SerializeField]
        float m_quantidadeGordUmaPorcao;

        #endregion

        #region Public Properties

        public AlimentoTipoEnum Tipo
        {
            get
            {
                return m_tipo;
            }
            set
            {
                if (m_tipo == value)
                    return;
                m_tipo = value;
            }
        }

        public string Key
        {
            get
            {
                return m_key;
            }
            set
            {
                if (m_key == value)
                    return;
                m_key = value;
            }
        }

        public string Nome
        {
            get
            {
                return m_nome;
            }
            set
            {
                if (m_nome == value)
                    return;
                m_nome = value;
            }
        }

        public Porcao Porcao
        {
            get
            {
                return m_porcao;
            }
            set
            {
                if (m_porcao == value)
                    return;
                m_porcao = value;
            }
        }

        public float QuantidadeCal
        {
            get
            {
                return m_quantidadeCal;
            }
            set
            {
                if (m_quantidadeCal == value)
                    return;
                m_quantidadeCal = value;
            }
        }

        public float QuantidadeCalUmaPorcao
        {
            get
            {
                return m_quantidadeCalUmaPorcao;
            }
            set
            {
                if (m_quantidadeCalUmaPorcao == value)
                    return;
                m_quantidadeCalUmaPorcao = value;
            }
        }

        public float QuantidadeCarb
        {
            get
            {
                return m_quantidadeCarb;
            }
            set
            {
                if (m_quantidadeCarb == value)
                    return;
                m_quantidadeCarb = value;
            }
        }

        public float QuantidadeCarbUmaPorcao
        {
            get
            {
                return m_quantidadeCarbUmaPorcao;
            }
            set
            {
                if (m_quantidadeCarbUmaPorcao == value)
                    return;
                m_quantidadeCarbUmaPorcao = value;
            }
        }

        public float QuantidadeProt
        {
            get
            {
                return m_quantidadeProt;
            }
            set
            {
                if (m_quantidadeProt == value)
                    return;
                m_quantidadeProt = value;
            }
        }

        public float QuantidadeProtUmaPorcao
        {
            get
            {
                return m_quantidadeProtUmaPorcao;
            }
            set
            {
                if (m_quantidadeProtUmaPorcao == value)
                    return;
                m_quantidadeProtUmaPorcao = value;
            }
        }


        public float QuantidadeGord
        {
            get
            {
                return m_quantidadeGord;
            }
            set
            {
                if (m_quantidadeGord == value)
                    return;
                m_quantidadeGord = value;
            }
        }

        public float QuantidadeGordUmaPorcao
        {
            get
            {
                return m_quantidadeGordUmaPorcao;
            }
            set
            {
                if (m_quantidadeGordUmaPorcao == value)
                    return;
                m_quantidadeGordUmaPorcao = value;
            }
        }


        public int Nota
        {
            get
            {
                return m_nota;
            }
            set
            {
                if (m_nota == value)
                    return;
                m_nota = value;
            }
        }

        #endregion

        public Alimento(Alimento p_parametro)
        {
            if (p_parametro != null)
            {
                m_key = p_parametro.Key;
                m_nome = p_parametro.Nome;
                m_tipo = p_parametro.Tipo;
                m_porcao = new Porcao();
                m_porcao.PesoEmGramas = p_parametro.Porcao.PesoEmGramas;
                m_porcao.QuantidadeDaPorcao = p_parametro.Porcao.QuantidadeDaPorcao;
                m_porcao.Tipo = p_parametro.Porcao.Tipo;
                m_porcao.PorcaoMaximaPossivel = p_parametro.Porcao.PorcaoMaximaPossivel;
                m_porcao.PorcaoMinimaPossivel = p_parametro.Porcao.PorcaoMinimaPossivel;
                m_quantidadeCal = p_parametro.QuantidadeCal;
                m_quantidadeCarb = p_parametro.QuantidadeCarb;
                m_quantidadeProt = p_parametro.QuantidadeProt;
                m_quantidadeGord = p_parametro.QuantidadeGord;
                m_nota = p_parametro.Nota;
            }
        }

        public void ModificaAlimentoPorFatorMultiplicator(float p_fator)
        {

            QuantidadeCalUmaPorcao = Mathf.RoundToInt(QuantidadeCal / (float)Porcao.QuantidadeDaPorcao);
            QuantidadeCarbUmaPorcao = Mathf.RoundToInt(QuantidadeCarb / (float)Porcao.QuantidadeDaPorcao);
            QuantidadeGordUmaPorcao = Mathf.RoundToInt(QuantidadeGord / (float)Porcao.QuantidadeDaPorcao);
            QuantidadeProtUmaPorcao = Mathf.RoundToInt(QuantidadeProt / (float)Porcao.QuantidadeDaPorcao);
            Porcao.PesoEmGramasUmaPorcao = Mathf.RoundToInt(Porcao.PesoEmGramas / (float)Porcao.QuantidadeDaPorcao);
            //Tem tudo pra 1 alimento
            //Altera alimento duplicado para apresentar os valores corretos dado a quantidade de Calorias
    
            Porcao.QuantidadeDaPorcao = Mathf.Clamp((Porcao.QuantidadeDaPorcao * p_fator), Porcao.PorcaoMinimaPossivel, Porcao.PorcaoMaximaPossivel);

            /*QuantidadeCal = (QuantidadeCalUmaPorcao * Porcao.QuantidadeDaPorcao);
            QuantidadeCarb = (QuantidadeCarbUmaPorcao * Porcao.QuantidadeDaPorcao);
            QuantidadeGord = (QuantidadeGordUmaPorcao * Porcao.QuantidadeDaPorcao);
            QuantidadeProt = (QuantidadeProtUmaPorcao * Porcao.QuantidadeDaPorcao);
            Porcao.PesoEmGramas = (Porcao.PesoEmGramasUmaPorcao * Porcao.QuantidadeDaPorcao);*/

            if (QuantidadeCal < 0.8 * QuantidadeCalUmaPorcao)
            {
                Porcao.QuantidadeDaPorcao = Mathf.Max(1, Mathf.RoundToInt(Porcao.PorcaoMinimaPossivel));
                QuantidadeCal = Mathf.RoundToInt(QuantidadeCalUmaPorcao * Porcao.QuantidadeDaPorcao);
                QuantidadeCarb = Mathf.RoundToInt(QuantidadeCarbUmaPorcao * Porcao.QuantidadeDaPorcao);
                QuantidadeGord = Mathf.RoundToInt(QuantidadeGordUmaPorcao * Porcao.QuantidadeDaPorcao);
                QuantidadeProt = Mathf.RoundToInt(QuantidadeProtUmaPorcao * Porcao.QuantidadeDaPorcao);
                Porcao.PesoEmGramas = Mathf.RoundToInt(Porcao.PesoEmGramasUmaPorcao * Porcao.QuantidadeDaPorcao);
            }
            else
            {
                Porcao.QuantidadeDaPorcao = Mathf.Max(1, Mathf.RoundToInt(Porcao.QuantidadeDaPorcao));
                QuantidadeCal = Mathf.Max(1, Mathf.RoundToInt(QuantidadeCalUmaPorcao * Porcao.QuantidadeDaPorcao));
                QuantidadeCarb = Mathf.Max(1, Mathf.RoundToInt(QuantidadeCarbUmaPorcao * Porcao.QuantidadeDaPorcao));
                QuantidadeGord = Mathf.Max(1, Mathf.RoundToInt(QuantidadeGordUmaPorcao * Porcao.QuantidadeDaPorcao));
                QuantidadeProt = Mathf.Max(1, Mathf.RoundToInt(QuantidadeProtUmaPorcao * Porcao.QuantidadeDaPorcao));
                Porcao.PesoEmGramas = Mathf.Max(1, Mathf.RoundToInt(Porcao.PesoEmGramasUmaPorcao * Porcao.QuantidadeDaPorcao));
            }

        }
    }

    [System.Serializable]
    public enum PorcaoTipo { ColherDeSopa, UnidadeDe, CopoAmericano, ConchaMediaCheia, MeiaXicaraDeCha, ColherDeServir, Fatias, PedacoPequeno, UnidadePequena, Pote, ConchaGrande, File, PostaMedia, UnidadeGrande, PedacosMedios, CopoDeRequeijao, Folhas }
    [System.Serializable]
    public class Porcao
    {
        #region Private Variables

        [SerializeField]
        PorcaoTipo m_tipo = PorcaoTipo.ColherDeSopa;
        [SerializeField]
        float m_quantidadeDaPorcao;
        [SerializeField]
        float m_pesoEmGramas;
        [SerializeField]
        int m_PorcaoMaximaPossivel = 5;
        [SerializeField]
        float m_PorcaoMinimaPossivel = 1.0f;
        float m_PesoEmGramasUmaPorcao;

        #endregion

        #region Public Properties

        public PorcaoTipo Tipo
        {
            get
            {
                return m_tipo;
            }
            set
            {
                if (m_tipo == value)
                    return;
                m_tipo = value;
            }
        }

        public float QuantidadeDaPorcao
        {
            get
            {
                return m_quantidadeDaPorcao;
            }
            set
            {
                if (m_quantidadeDaPorcao == value)
                    return;
                m_quantidadeDaPorcao = value;
            }
        }


        public float PesoEmGramas
        {
            get
            {
                return m_pesoEmGramas;
            }
            set
            {
                if (m_pesoEmGramas == value)
                    return;
                m_pesoEmGramas = value;
            }
        }

        public float PesoEmGramasUmaPorcao
        {
            get
            {
                return m_PesoEmGramasUmaPorcao;
            }
            set
            {
                if (m_PesoEmGramasUmaPorcao == value)
                    return;
                m_PesoEmGramasUmaPorcao = value;
            }
        }

        public int PorcaoMaximaPossivel
        {
            get
            {
                return m_PorcaoMaximaPossivel;
            }
            set
            {
                if (m_PorcaoMaximaPossivel == value)
                    return;
                m_PorcaoMaximaPossivel = value;
            }
        }

        public float PorcaoMinimaPossivel
        {
            get
            {
                return m_PorcaoMinimaPossivel;
            }
            set
            {
                if (m_PorcaoMinimaPossivel == value)
                    return;
                m_PorcaoMinimaPossivel = value;
            }
        }

        public string GetTipoPorcaoFormatted()
        {
            string v_return = Tipo.ToString();
            if (Tipo == PorcaoTipo.ColherDeServir)
                v_return = "Colher(es) de Servir";
            if (Tipo == PorcaoTipo.ColherDeSopa)
                v_return = "Colher(es) de Sopa";
            if (Tipo == PorcaoTipo.UnidadeDe)
                v_return = "Unidade(s)";
            if (Tipo == PorcaoTipo.CopoAmericano)
                v_return = "Copo(s)";
            if (Tipo == PorcaoTipo.Fatias)
                v_return = "Fatia(s)";
            if (Tipo == PorcaoTipo.File)
                v_return = "File(s)";
            if (Tipo == PorcaoTipo.Folhas)
                v_return = "Folha(s)";
            if (Tipo == PorcaoTipo.ConchaGrande)
                v_return = "Concha(s) Grande(s)";
            if (Tipo == PorcaoTipo.ConchaMediaCheia)
                v_return = "Concha(s) Média(s) Cheia(s)";
            if (Tipo == PorcaoTipo.CopoDeRequeijao)
                v_return = "Copo(s)";
            if (Tipo == PorcaoTipo.UnidadeGrande)
                v_return = "Unidade(s) Grande(s)";
            if (Tipo == PorcaoTipo.UnidadePequena)
                v_return = "Unidade(s) Pequena(s)";
            if (Tipo == PorcaoTipo.PostaMedia)
                v_return = "Posta(s) Média(s)";
            if (Tipo == PorcaoTipo.MeiaXicaraDeCha)
                v_return = "Meia Xícara de Chá";
            if (Tipo == PorcaoTipo.Pote)
                v_return = "Pote(s)";
            if (Tipo == PorcaoTipo.PedacoPequeno)
                v_return = "Pedaço(s) Pequeno(s)";
            return v_return;
        }

        #endregion
    }

}
