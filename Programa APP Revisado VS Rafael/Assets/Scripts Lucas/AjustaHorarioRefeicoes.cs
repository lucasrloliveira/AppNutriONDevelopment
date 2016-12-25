using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AjustaHorarioRefeicoes : MonoBehaviour
{
    [SerializeField]
    string m_tituloRefeicao = "";
    [SerializeField]
    Text m_exibeHorarioRefeicao;
    [SerializeField]
    Slider m_regulaHorario;
    [SerializeField]
    PopupUI m_popupHorarioRefeicao;
    [SerializeField]
    Text m_exibeTituloRefeicaoFormatted;

    [SerializeField]
    Text m_exibeHoraCafeManhaGerenciadorDietas;
    [SerializeField]
    Text m_exibeHoraColacaoGerenciadorDietas;
    [SerializeField]
    Text m_exibeHoraAlmocoGerenciadorDietas;
    [SerializeField]
    Text m_exibeHoraLancheTardeGerenciadorDietas;
    [SerializeField]
    Text m_exibeHoraJantarGerenciadorDietas;
    [SerializeField]
    Text m_exibeHoraCeiaGerenciadorDietas;


    public Text ExibeHoraCafeManhaGerenciadorDietas
    {
        get
        {
            return m_exibeHoraCafeManhaGerenciadorDietas;
        }
        set
        {
            if (m_exibeHoraCafeManhaGerenciadorDietas == value)
                return;
            m_exibeHoraCafeManhaGerenciadorDietas = value;
            SetDirty();
        }
    }

    public Text ExibeHoraColacaoGerenciadorDietas
    {
        get
        {
            return m_exibeHoraColacaoGerenciadorDietas;
        }
        set
        {
            if (m_exibeHoraColacaoGerenciadorDietas == value)
                return;
            m_exibeHoraColacaoGerenciadorDietas = value;
            SetDirty();
        }
    }
    public Text ExibeHoraAlmocoGerenciadorDietas
    {
        get
        {
            return m_exibeHoraAlmocoGerenciadorDietas;
        }
        set
        {
            if (m_exibeHoraAlmocoGerenciadorDietas == value)
                return;
            m_exibeHoraAlmocoGerenciadorDietas = value;
            SetDirty();
        }
    }
    public Text ExibeHoraLancheTardeGerenciadorDietas
    {
        get
        {
            return m_exibeHoraLancheTardeGerenciadorDietas;
        }
        set
        {
            if (m_exibeHoraLancheTardeGerenciadorDietas == value)
                return;
            m_exibeHoraLancheTardeGerenciadorDietas = value;
            SetDirty();
        }
    }
    public Text ExibeHoraJantarGerenciadorDietas
    {
        get
        {
            return m_exibeHoraJantarGerenciadorDietas;
        }
        set
        {
            if (m_exibeHoraJantarGerenciadorDietas == value)
                return;
            m_exibeHoraJantarGerenciadorDietas = value;
            SetDirty();
        }
    }
    public Text ExibeHoraCeiaGerenciadorDietas
    {
        get
        {
            return m_exibeHoraCeiaGerenciadorDietas;
        }
        set
        {
            if (m_exibeHoraCeiaGerenciadorDietas == value)
                return;
            m_exibeHoraCeiaGerenciadorDietas = value;
            SetDirty();
        }
    }

    public string TituloRefeicao
    {
        get
        {
            return m_tituloRefeicao;
        }
        set
        {
            if (m_tituloRefeicao == value)
                return;
            m_tituloRefeicao = value;
            SetDirty();
        }
    }

    public Text ExibeHorarioRefeicao
    {
        get
        {
            return m_exibeHorarioRefeicao;
        }
        set
        {
            if (m_exibeHorarioRefeicao == value)
                return;
            m_exibeHorarioRefeicao = value;
            SetDirty();
        }
    }

    public Text ExibeTituloRefeicaoFormatted
    {
        get
        {
            return m_exibeTituloRefeicaoFormatted;
        }
        set
        {
            if (m_exibeTituloRefeicaoFormatted == value)
                return;
            m_exibeTituloRefeicaoFormatted = value;
            SetDirty();
        }
    }

    public PopupUI PopupHorarioRefeicao
    {
        get
        {
            return m_popupHorarioRefeicao;
        }
        set
        {
            if (m_popupHorarioRefeicao == value)
                return;
            m_popupHorarioRefeicao = value;
            SetDirty();
        }
    }

    public Slider RegulaHorario
    {
        get
        {
            return m_regulaHorario;
        }
        set
        {
            if (m_regulaHorario == value)
                return;
            m_regulaHorario = value;
            SetDirty();
        }
    }

    bool _isDirty = false;
    public void SetDirty()
    {
        _isDirty = true;
    }

    protected virtual void OnEnable()
    {
        TryApplyInformations();
    }

    protected virtual void OnStart()
    {
        TryApplyInformations();
    }

    protected virtual void Update()
    {
        TryApplyInformations();
    }

    public void TryApplyInformations(bool force = false)
    {
        if (_isDirty || force)
        {
            _isDirty = false;
            TryApplyInformations();
        }
    }

    protected virtual void ApplyInformations()
    {
        if (m_tituloRefeicao != null || m_exibeHorarioRefeicao != null || m_popupHorarioRefeicao != null)
            SetaTextoHorarioRefeicoesComSlider();
    }

    public void SetaTextoHorarioRefeicoesComSlider()
    {
        int v_horaMinima = 0;
        int v_horas = 0;
        int v_minutos = 0;
        switch (m_tituloRefeicao)
        {
            case "CafeManha":
                {
                    m_exibeTituloRefeicaoFormatted.text = "Café Da Manhã";
                    if (!PlayerPrefs.HasKey("CafeManhaHorarioTexto"))
                    {
                        m_regulaHorario.value = 10;
                        m_exibeHorarioRefeicao.text = "7 : 30";
                        
                    }
                    v_horaMinima = 5;
                    v_minutos = 0;
                    m_regulaHorario.minValue = 0;
                    m_regulaHorario.maxValue = 20;
                    v_horas = v_horaMinima + Mathf.RoundToInt(m_regulaHorario.value) / 4;
                    v_minutos = (Mathf.RoundToInt(m_regulaHorario.value) * 15) % 60;
                    if (v_minutos == 0)
                        m_exibeHorarioRefeicao.text = v_horas.ToString() + " : 00";
                    else
                        m_exibeHorarioRefeicao.text = (v_horas.ToString() + " : " + v_minutos.ToString());
                    m_exibeHoraCafeManhaGerenciadorDietas.text = m_exibeHorarioRefeicao.text;
                    break;
                }
            case "Colacao":
                {
                    m_exibeTituloRefeicaoFormatted.text = "Colação";
                    if (!PlayerPrefs.HasKey("ColacaoHorarioTexto"))
                    {
                        m_regulaHorario.value = 10;
                        m_exibeHorarioRefeicao.text = "10 : 30";
                    }
                    v_horaMinima = 8;
                    v_minutos = 0;
                    m_regulaHorario.minValue = 0;
                    m_regulaHorario.maxValue = 14;
                    v_horas = v_horaMinima + Mathf.RoundToInt(m_regulaHorario.value) / 4;
                    v_minutos = (Mathf.RoundToInt(m_regulaHorario.value) * 15) % 60;
                    if (v_minutos == 0)
                        m_exibeHorarioRefeicao.text = v_horas.ToString() + " : 00";
                    else
                        m_exibeHorarioRefeicao.text = (v_horas.ToString() + " : " + v_minutos.ToString());
                    m_exibeHoraColacaoGerenciadorDietas.text = m_exibeHorarioRefeicao.text;
                    break;
                }
            case "Almoco":
                {
                    m_exibeTituloRefeicaoFormatted.text = "Almoço";
                    if (!PlayerPrefs.HasKey("ColacaoHorarioTexto"))
                    {
                        m_regulaHorario.value = 10;
                        m_exibeHorarioRefeicao.text = "12 : 30";
                    }
                    v_horaMinima = 11;
                    v_minutos = 0;
                    m_regulaHorario.minValue = 0;
                    m_regulaHorario.maxValue = 14;
                    v_horas = v_horaMinima + Mathf.RoundToInt(m_regulaHorario.value) / 4;
                    v_minutos = (Mathf.RoundToInt(m_regulaHorario.value) * 15) % 60;
                    if (v_minutos == 0)
                        m_exibeHorarioRefeicao.text = v_horas.ToString() + " : 00";
                    else
                        m_exibeHorarioRefeicao.text = (v_horas.ToString() + " : " + v_minutos.ToString());
                    m_exibeHoraAlmocoGerenciadorDietas.text = m_exibeHorarioRefeicao.text;
                    break;
                }
            case "LancheTarde":
                {
                    m_exibeTituloRefeicaoFormatted.text = "Lanche Da Tarde";
                    if (!PlayerPrefs.HasKey("LancheTardeHorarioTexto"))
                    {
                        m_regulaHorario.value = 6;
                        m_exibeHorarioRefeicao.text = "15 : 30";
                    }
                    v_horaMinima = 14;
                    v_minutos = 0;
                    m_regulaHorario.minValue = 0;
                    m_regulaHorario.maxValue = 12;
                    v_horas = v_horaMinima + Mathf.RoundToInt(m_regulaHorario.value) / 4;
                    v_minutos = (Mathf.RoundToInt(m_regulaHorario.value) * 15) % 60;
                    if (v_minutos == 0)
                        m_exibeHorarioRefeicao.text = v_horas.ToString() + " : 00";
                    else
                        m_exibeHorarioRefeicao.text = (v_horas.ToString() + " : " + v_minutos.ToString());
                    m_exibeHoraLancheTardeGerenciadorDietas.text = m_exibeHorarioRefeicao.text;
                    break;
                }
            case "Jantar":
                {
                    m_exibeTituloRefeicaoFormatted.text = "Jantar";
                    if (!PlayerPrefs.HasKey("JantarHorarioTexto"))
                    {
                        m_regulaHorario.value = 6;
                        m_exibeHorarioRefeicao.text = "18 : 30";
                    }
                    v_horaMinima = 17;
                    v_minutos = 0;
                    m_regulaHorario.minValue = 0;
                    m_regulaHorario.maxValue = 16;
                    v_horas = v_horaMinima + Mathf.RoundToInt(m_regulaHorario.value) / 4;
                    v_minutos = (Mathf.RoundToInt(m_regulaHorario.value) * 15) % 60;
                    if (v_minutos == 0)
                        m_exibeHorarioRefeicao.text = v_horas.ToString() + " : 00";
                    else
                        m_exibeHorarioRefeicao.text = (v_horas.ToString() + " : " + v_minutos.ToString());
                    m_exibeHoraJantarGerenciadorDietas.text = m_exibeHorarioRefeicao.text;
                    break;
                }
            case "Ceia":
                {
                    m_exibeTituloRefeicaoFormatted.text = "Ceia";
                    if (!PlayerPrefs.HasKey("CeiaHorarioTexto"))
                    {
                        m_regulaHorario.value = 6;
                        m_exibeHorarioRefeicao.text = "21 : 30";
                    }
                    v_horaMinima = 20;
                    v_minutos = 0;
                    m_regulaHorario.minValue = 0;
                    m_regulaHorario.maxValue = 14;
                    v_horas = v_horaMinima + Mathf.RoundToInt(m_regulaHorario.value) / 4;
                    v_minutos = (Mathf.RoundToInt(m_regulaHorario.value) * 15) % 60;
                    if (v_minutos == 0)
                        m_exibeHorarioRefeicao.text = v_horas.ToString() + " : 00";
                    else
                        m_exibeHorarioRefeicao.text = (v_horas.ToString() + " : " + v_minutos.ToString());
                    m_exibeHoraCeiaGerenciadorDietas.text = m_exibeHorarioRefeicao.text;
                    break;
                }
        }
    }

    public void GetSavedInfoGerenciadorDietasSliders()
    {
        switch (m_tituloRefeicao)
        {
            case "CafeManha":
                m_exibeHoraCafeManhaGerenciadorDietas.text = PlayerPrefs.GetString("CafeManhaHorarioTexto");
                m_regulaHorario.value = PlayerPrefs.GetInt("CafeManhaHorarioSlider");
                m_exibeHorarioRefeicao.text = PlayerPrefs.GetString("CafeManhaHorarioTexto");
                break;
            case "Colacao":
                m_exibeHoraColacaoGerenciadorDietas.text = PlayerPrefs.GetString("ColacaoHorarioTexto");
                m_regulaHorario.value = PlayerPrefs.GetInt("ColacaoHorarioSlider");
                m_exibeHorarioRefeicao.text = PlayerPrefs.GetString("ColacaoHorarioTexto");

                break;
            case "Almoco":
                m_exibeHoraAlmocoGerenciadorDietas.text = PlayerPrefs.GetString("AlmocoHorarioTexto");
                m_regulaHorario.value = PlayerPrefs.GetInt("AlmocoHorarioSlider");
                m_exibeHorarioRefeicao.text = PlayerPrefs.GetString("AlmocoHorarioTexto");
                break;
            case "LancheTarde":
                m_exibeHoraLancheTardeGerenciadorDietas.text = PlayerPrefs.GetString("LancheTardeHorarioTexto");
                m_regulaHorario.value = PlayerPrefs.GetInt("LancheTardeHorarioSlider");
                m_exibeHorarioRefeicao.text = PlayerPrefs.GetString("LancheTardeHorarioTexto");
                break;
            case "Jantar":
                m_exibeHoraJantarGerenciadorDietas.text = PlayerPrefs.GetString("JantarHorarioTexto");
                m_regulaHorario.value = PlayerPrefs.GetInt("JantarHorarioSlider");
                m_exibeHorarioRefeicao.text = PlayerPrefs.GetString("JantarHorarioTexto");

                break;
            case "Ceia":
                m_exibeHoraCeiaGerenciadorDietas.text = PlayerPrefs.GetString("CeiaHorarioTexto");
                m_regulaHorario.value = PlayerPrefs.GetInt("CeiaHorarioSlider");
                m_exibeHorarioRefeicao.text = PlayerPrefs.GetString("CeiaHorarioTexto");
                break;
        }
    }



    public void SalvaInfoGerenciadorDietas()
    {
        switch (m_tituloRefeicao)
        {
            case "CafeManha":
                PlayerPrefs.SetInt("CafeManhaHorarioSlider", Mathf.RoundToInt(m_regulaHorario.value));
                PlayerPrefs.SetString("CafeManhaHorarioTexto", m_exibeHoraCafeManhaGerenciadorDietas.text);
                PlayerPrefs.SetString("CafeManhaHorarioTexto", m_exibeHorarioRefeicao.text);
                break;
            case "Colacao":
                PlayerPrefs.SetInt("ColacaoHorarioSlider", Mathf.RoundToInt(m_regulaHorario.value));
                PlayerPrefs.SetString("ColacaoHorarioTexto", m_exibeHoraColacaoGerenciadorDietas.text);
                PlayerPrefs.SetString("ColacaoHorarioTexto", m_exibeHorarioRefeicao.text);
                break;
            case "Almoco":
                PlayerPrefs.SetInt("AlmocoHorarioSlider", Mathf.RoundToInt(m_regulaHorario.value));
                PlayerPrefs.SetString("AlmocoHorarioTexto", m_exibeHoraAlmocoGerenciadorDietas.text);
                PlayerPrefs.SetString("AlmocoHorarioTexto", m_exibeHorarioRefeicao.text);
                break;
            case "LancheTarde":
                PlayerPrefs.SetInt("LancheTardeHorarioSlider", Mathf.RoundToInt(m_regulaHorario.value));
                PlayerPrefs.SetString("LancheTardeHorarioTexto", m_exibeHoraLancheTardeGerenciadorDietas.text);
                PlayerPrefs.SetString("LancheTardeHorarioTexto", m_exibeHorarioRefeicao.text);
                break;
            case "Jantar":
                PlayerPrefs.SetInt("JantarHorarioSlider", Mathf.RoundToInt(m_regulaHorario.value));
                PlayerPrefs.SetString("JantarHorarioTexto", m_exibeHoraJantarGerenciadorDietas.text);
                PlayerPrefs.SetString("JantarHorarioTexto", m_exibeHorarioRefeicao.text);
                break;
            case "Ceia":
                PlayerPrefs.SetInt("CeiaHorarioSlider", Mathf.RoundToInt(m_regulaHorario.value));
                PlayerPrefs.SetString("CeiaHorarioTexto", m_exibeHoraCeiaGerenciadorDietas.text);
                PlayerPrefs.SetString("CeiaHorarioTexto", m_exibeHorarioRefeicao.text);
                break;
        }              
    }

    public void GetInitialInfoHorarioGerenciador()
    {
        m_exibeHoraCafeManhaGerenciadorDietas.text = PlayerPrefs.GetString("CafeManhaHorarioTexto");
        m_exibeHoraColacaoGerenciadorDietas.text = PlayerPrefs.GetString("ColacaoHorarioTexto");
        m_exibeHoraAlmocoGerenciadorDietas.text = PlayerPrefs.GetString("AlmocoHorarioTexto");
        m_exibeHoraLancheTardeGerenciadorDietas.text = PlayerPrefs.GetString("LancheTardeHorarioTexto");
        m_exibeHoraJantarGerenciadorDietas.text = PlayerPrefs.GetString("JantarHorarioTexto");
        m_exibeHoraCeiaGerenciadorDietas.text = PlayerPrefs.GetString("CeiaHorarioTexto");
    }

    void Start()
    {
        GetInitialInfoHorarioGerenciador();
    }
}

