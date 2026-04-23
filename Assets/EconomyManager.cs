using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    // Isto é o Singleton: cria uma referência global para este script
    public static EconomyManager Instance;

    // A variável que guarda as moedas enquanto o jogo está aberto
    private int currentCoins;

    void Awake()
    {
        // Garante que só existe UM EconomyManager no jogo todo
        if (Instance == null)
        {
            Instance = this;
            // Impede que este objeto seja destruído quando mudas do Menu para o Jogo (Running)
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Quando o jogo abre, vai à memória do telemóvel buscar as moedas. 
        // Se for a primeira vez a jogar, começa com 0.
        currentCoins = PlayerPrefs.GetInt("TotalCoins", 0);
    }

    // Função para ver quanto dinheiro tens
    public int GetCoins()
    {
        return currentCoins;
    }

    // Função para adicionar moedas (vais usar isto na corrida)
    public void AddCoins(int amount)
    {
        currentCoins += amount;
        PlayerPrefs.SetInt("MyCoins", currentCoins); // Grava o novo valor
        PlayerPrefs.Save();
    }

    // Função para gastar moedas (vais usar isto na loja)
    public bool SpendCoins(int amount)
    {
        // Só gasta se tiveres dinheiro suficiente
        if (currentCoins >= amount)
        {
            currentCoins -= amount;
            SaveCoins();
            return true; // Retorna verdadeiro (compra com sucesso!)
        }

        return false; // Retorna falso (não tem dinheiro)
    }


    // Função interna para gravar os dados na memória do telemóvel/PC
    private void SaveCoins()
    {
        PlayerPrefs.SetInt("TotalCoins", currentCoins);
        PlayerPrefs.Save();
    }
}