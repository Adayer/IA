using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicaDifusaEjemplo : MonoBehaviour
{
    [Header("Cantidad comensales")]
    [SerializeField] private AnimationCurve m_individual;
    [SerializeField] private AnimationCurve m_pareja;
    [SerializeField] private AnimationCurve m_grupo;
    [SerializeField] private AnimationCurve m_familia;
    [SerializeField] private AnimationCurve m_familiaNumerosa;

    [SerializeField] private float m_valorMaximoComensales;
    [SerializeField] private float m_valorComensales;

    private float m_individualValorVerdad = 0f;
    private float m_parejaValorVerdad = 0f;
    private float m_grupoValorVerdad = 0f;
    private float m_familiaValorVerdad = 0f;
    private float m_familiaNumerosaValorVerdad = 0f;
            
    [Header("Nivel de hambre")]
    [SerializeField] private AnimationCurve m_poca;
    [SerializeField] private AnimationCurve m_decente;
    [SerializeField] private AnimationCurve m_mucha;
    [SerializeField] private AnimationCurve m_comerianUnaVaca;

    [SerializeField] private float m_valorMaximoHambre;
    [SerializeField] private float m_valorHambre;

    private float m_pocaHamValorVerdad = 0f;
    private float m_decenteHamValorVerdad = 0f;
    private float m_muchaHamValorVerdad = 0f;
    private float m_unaVacaHamValorVerdad = 0f;



    private float m_pocaComidaValor = 0f;
    private float m_normalComidaValor = 0f;
    private float m_muchaComidaValor = 0f;
    private float m_festinComidaValor = 0f;



    [SerializeField] private float m_ifPocaCuantoArroz = 0f;
    [SerializeField] private float m_ifNormaCuantoArroz = 0f;
    [SerializeField] private float m_ifMuchaCuantoArroz = 0f;
    [SerializeField] private float m_ifFestinCuantoArroz = 0f;

    private float m_cantidadDeArrozQueHacer = 0f;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            m_valorComensales = Random.Range(1, (m_valorMaximoComensales));
            m_valorHambre = Random.Range(1, (m_valorMaximoHambre));

            print(m_valorComensales + "  -----  "  + m_valorHambre);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            print("Poca comida verdad = " + m_pocaComidaValor);
            print("Normal comida verdad = " + m_normalComidaValor);
            print("Mucha comida verdad = " + m_muchaComidaValor);
            print("Festín comida verdad = " + m_festinComidaValor);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            float inputValueComensales = m_valorComensales / m_valorMaximoComensales;

            m_individualValorVerdad = Mathf.Clamp(m_individual.Evaluate(inputValueComensales), 0 , 1);
            m_parejaValorVerdad = Mathf.Clamp(m_pareja.Evaluate(inputValueComensales), 0, 1);
            m_grupoValorVerdad = Mathf.Clamp(m_grupo.Evaluate(inputValueComensales), 0, 1);
            m_familiaValorVerdad = Mathf.Clamp(m_familia.Evaluate(inputValueComensales), 0, 1);
            m_familiaNumerosaValorVerdad = Mathf.Clamp(m_familiaNumerosa.Evaluate(inputValueComensales), 0, 1);

            float inputHambre = m_valorHambre / m_valorMaximoHambre;

            m_pocaHamValorVerdad = Mathf.Clamp(m_poca.Evaluate(inputHambre), 0 , 1);
            m_decenteHamValorVerdad = Mathf.Clamp(m_decente.Evaluate(inputHambre), 0, 1);
            m_muchaHamValorVerdad = Mathf.Clamp(m_mucha.Evaluate(inputHambre), 0, 1);
            m_unaVacaHamValorVerdad = Mathf.Clamp(m_comerianUnaVaca.Evaluate(inputHambre), 0, 1);

            m_pocaComidaValor = EvaluatePocaTable();
            m_normalComidaValor = EvaluateNormalTable();
            m_muchaComidaValor = EvaluateMuchaTable();
            m_festinComidaValor = EvaluateFestinTable();



            m_cantidadDeArrozQueHacer = (m_pocaComidaValor * m_ifPocaCuantoArroz + m_normalComidaValor * m_ifNormaCuantoArroz +
                m_muchaComidaValor * m_ifMuchaCuantoArroz + m_festinComidaValor * m_festinComidaValor)
                / (m_pocaComidaValor + m_normalComidaValor + m_muchaComidaValor + m_festinComidaValor);


            print("Se van a hacer " + m_cantidadDeArrozQueHacer + " tazas de arroz para " + m_valorComensales + " comensales. Los cuales tienen " + m_valorHambre + " hambre");

        }
        
    }


    private float EvaluatePocaTable()
    {
        float pocaComidaR = Mathf.Max(
            Mathf.Min(m_individualValorVerdad, m_pocaHamValorVerdad), 
            Mathf.Min(m_individualValorVerdad, m_decenteHamValorVerdad), 
            Mathf.Min(m_individualValorVerdad, m_muchaHamValorVerdad),

            Mathf.Min(m_parejaValorVerdad, m_pocaHamValorVerdad)
            );
        return pocaComidaR;
    }
    private float EvaluateNormalTable()
    {
        float normalComidaR = Mathf.Max(
            Mathf.Min(m_individualValorVerdad, m_unaVacaHamValorVerdad), 

            Mathf.Min(m_parejaValorVerdad, m_decenteHamValorVerdad), 
            Mathf.Min(m_parejaValorVerdad, m_muchaHamValorVerdad),

            Mathf.Min(m_grupoValorVerdad, m_decenteHamValorVerdad),
            Mathf.Min(m_grupoValorVerdad, m_muchaHamValorVerdad),
            Mathf.Min(m_grupoValorVerdad, m_pocaHamValorVerdad),

            Mathf.Min(m_familiaValorVerdad, m_pocaHamValorVerdad),
            Mathf.Min(m_familiaValorVerdad, m_decenteHamValorVerdad),

            Mathf.Min(m_familiaNumerosaValorVerdad, m_pocaHamValorVerdad)
            );
        return normalComidaR;
    }
    private float EvaluateMuchaTable()
    {
        float muchaComidaR = Mathf.Max(
            Mathf.Min(m_parejaValorVerdad, m_unaVacaHamValorVerdad), 

            Mathf.Min(m_grupoValorVerdad, m_unaVacaHamValorVerdad),

            Mathf.Min(m_familiaValorVerdad, m_muchaHamValorVerdad),
            Mathf.Min(m_familiaValorVerdad, m_unaVacaHamValorVerdad),

            Mathf.Min(m_familiaNumerosaValorVerdad, m_decenteHamValorVerdad),
            Mathf.Min(m_familiaNumerosaValorVerdad, m_muchaHamValorVerdad)
            );
        return muchaComidaR;
    }

    private float EvaluateFestinTable()
    {
        float festinComidaR = Mathf.Max(
            Mathf.Min(m_familiaNumerosaValorVerdad, m_unaVacaHamValorVerdad)
            );
        return festinComidaR;
    }


}
