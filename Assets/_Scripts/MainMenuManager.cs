using UnityEngine;
using UnityEngine.SceneManagement; // Required for loading scenes
using TMPro; // <-- Obrigatório para mexer nos textos TextMeshPro!

public class MainMenuManager : MonoBehaviour
{

    [Header("Painéis da UI")]
    // Referência para o painel da loja
    public GameObject shopPanel;

    [Header("Modelos 3D")]
    // Nova referência para o nosso manequim 3D
    public GameObject jamesManequim;

    [Header("Textos")]
    // Nova referência para o texto das moedas
    public TMP_Text coinsText;

    // This function will be called by the Start button
    public void StartGame()
    {
        // Loads your game scene. 
        // Make sure the name matches your game scene exactly!
        SceneManager.LoadScene("Running"); 
    }

    // Optional: A function to quit the game
    public void QuitGame()
    {
        Debug.Log("Game Exited!");
        Application.Quit();
    }

    //FUNÇÕES DA LOJA

public void OpenShop()
    {
        shopPanel.SetActive(true); // Mostra a loja
        jamesManequim.SetActive(true); // LIGA o boneco 3D!

        // Vai ao banco ver quanto dinheiro tens e atualiza o texto!
        int myCoins = EconomyManager.Instance.GetCoins();
        coinsText.text = "Coins: " + myCoins.ToString();
    }
    public void CloseShop()
    {
        shopPanel.SetActive(false); // Esconde a loja
        jamesManequim.SetActive(false); // DESLIGA o boneco 3D!
    }


}