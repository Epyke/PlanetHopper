using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    // O "Singleton" para esta cena, para o James conseguir falar com ele facilmente
    public static HUDManager Instance; 

    public TMP_Text coinsText;
    
    // O dinheiro que ganhamos NESTA corrida (começa sempre a zero!)
    public int moedasDestaCorrida = 0; 

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Ao começar a corrida, atualiza o texto para mostrar 0
        AtualizarTexto();
    }

    // O James vai chamar esta função quando apanhar uma moeda
    public void GanharMoeda(int quantidade)
    {
        moedasDestaCorrida += quantidade;
        AtualizarTexto();
    }

    private void AtualizarTexto()
    {
        coinsText.text = "Coins: " + moedasDestaCorrida.ToString();
    }

    // Vamos chamar esta função MAIS TARDE, quando o James bater num obstáculo!
    public void DepositarNoBanco()
    {
        if (EconomyManager.Instance != null)
        {
            EconomyManager.Instance.AddCoins(moedasDestaCorrida);
            Debug.Log("Dinheiro depositado no banco: " + moedasDestaCorrida);
        }
    }
}