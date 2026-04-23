using UnityEngine;

public class EquipManager : MonoBehaviour
{
    // A lista de chapéus do James da corrida
    public GameObject[] hats; 

    void Start()
    {
        // 1. Vai ao "Cartão de Memória" (PlayerPrefs) ver qual foi o chapéu equipado na loja. 
        // Se não houver nada guardado, ele assume que é o 0 (o cubo grátis).
        int chapeuParaVestir = PlayerPrefs.GetInt("ChapeuEquipado", 0);

        // 2. Por segurança, desliga todos os chapéus da lista
        for (int i = 0; i < hats.Length; i++)
        {
            hats[i].SetActive(false);
        }

        // 3. Liga APENAS o chapéu que estava guardado na memória
        if (chapeuParaVestir < hats.Length)
        {
            hats[chapeuParaVestir].SetActive(true);
        }
    }
}