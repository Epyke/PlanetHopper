using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [Header("Acessórios da Cabeça")]
    public GameObject[] hats; 
    public int[] hatPrices; 
    
    // NOVA LISTA: O jogo vai guardar aqui um "Verdadeiro" se já o compraste, ou "Falso" se não.
    public bool[] hatUnlocked; 

    [Header("Interface da Loja")]
    public TMP_Text buyButtonText; 
    
    private int currentIndex = 0; 

    void Start()
    {
        // Garante que o primeiro chapéu (o cubo) é sempre grátis/desbloqueado
        if(hatUnlocked.Length > 0)
        {
            hatUnlocked[0] = true;
        }

        if(hats.Length > 0)
        {
            ShowHat(currentIndex);
        }
    }

    public void NextHat()
    {
        currentIndex++; 
        if (currentIndex >= hats.Length)
        {
            currentIndex = 0; 
        }
        ShowHat(currentIndex);
    }

    public void PreviousHat()
    {
        currentIndex--; 
        if (currentIndex < 0)
        {
            currentIndex = hats.Length - 1; 
        }
        ShowHat(currentIndex);
    }

    private void ShowHat(int index)
    {
        for (int i = 0; i < hats.Length; i++)
        {
            hats[i].SetActive(false);
        }
        hats[index].SetActive(true);

        // NOVA MAGIA: O botão muda se já tivermos comprado o chapéu!
        if (hatUnlocked[index] == true)
        {
            buyButtonText.text = "Equipado"; 
        }
        else
        {
            buyButtonText.text = "Buy: " + hatPrices[index].ToString();
        }
    }

    // NOVA FUNÇÃO: O que acontece quando o jogador clica no botão Amarelo!
    public void BuyHat()
    {
        // Se já tens este chapéu, o botão não faz nada (mais tarde faremos equipar no jogo)
        if (hatUnlocked[currentIndex] == true)
        {
            Debug.Log("Já tens este chapéu!");
            return;
        }

        // Vai ao Banco ver quanto dinheiro temos e qual o preço do chapéu
        int myCoins = EconomyManager.Instance.GetCoins();
        int price = hatPrices[currentIndex];

        // Se o dinheiro for maior ou igual ao preço... COMPRA APROVADA!
        if (myCoins >= price)
        {
            EconomyManager.Instance.AddCoins(-price); // Tira o dinheiro do Banco (usamos o sinal de menos)
            hatUnlocked[currentIndex] = true; // Desbloqueia o chapéu!
             PlayerPrefs.SetInt("ChapeuEquipado", currentIndex);  // equipa o cahpeu
            ShowHat(currentIndex); // Atualiza o visual do botão para "Equipado"
            
            // Um pequeno truque para atualizar o texto das moedas lá no topo da loja
            FindObjectOfType<MainMenuManager>().OpenShop(); 
            
            Debug.Log("Chapéu Comprado!");
        }
        else
        {
            Debug.Log("Não tens dinheiro suficiente!");
        }
    }
}