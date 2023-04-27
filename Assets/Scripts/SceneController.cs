using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public const int gridRows = 2;
    public const int gridColumns = 4;
    public const float offsetX = 4f;
    public const float offsetY = 5f;

    [SerializeField] private MainCard originalCard;
    [SerializeField] private Sprite[] images;
    [SerializeField] private TextMesh scoreLabel;
    [SerializeField] private TextMesh triesLabel;
    [SerializeField] private TextMesh timeLabel;
    [SerializeField] private TextMesh totalSLabel;

    private int _noc = 0;
    private int _tries = 0;
    private float _time = 0.0f;
    private float _score = 0.0f;
    private MainCard _firstRevealedCard;
    private MainCard _secondRevealedCard;
    private bool end = false;


    public bool canReveal
    {
        get
        {
            return _secondRevealedCard == null;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        SetCardStartLayout();
    }
    private void Update()
    {
        if (!end) { 
        _time += Time.deltaTime;
        timeLabel.text = "Time: " + _time;
        }
    }

    private void ShowScore()
    {
        totalSLabel.gameObject.SetActive(true);
        float fNoc = _noc;
        float fTries = _tries;
        _score = 4000 / (fTries * _time);
        totalSLabel.text = "Total Score: " + Convert.ToInt32(_score);
    }

    private void SetCardStartLayout()
    {
        Vector3 startPosition = originalCard.transform.position;

        int[] cardNumbers = { 0, 0, 1, 1, 2, 2, 3, 3 };
        cardNumbers = ShuffleCards(cardNumbers);

        MainCard card;

        for (int i = 0; i < gridColumns; i++)
        {
            for (int j = 0; j < gridRows; j++)
            {
                if (i == 0 && j == 0)
                    card = originalCard;
                else
                    card = Instantiate(originalCard);

                int index = j * gridColumns + i;
                int id = cardNumbers[index];
                card.ChangeSprite(id, images[id]);

                float xPos = startPosition.x + (i * offsetX);
                float yPos = startPosition.y + (j * offsetY);
                card.transform.position = new Vector3(xPos, yPos, startPosition.z);
            }
        }
    }

    private int[] ShuffleCards(int[] cardNumbers)
    {
        int[] shuffledCarNumbers = cardNumbers.Clone() as int[];

        for (int i = 0; i < shuffledCarNumbers.Length; i++)
        {
            int temp = shuffledCarNumbers[i];
            int r = UnityEngine.Random.Range(0, shuffledCarNumbers.Length);
            shuffledCarNumbers[i] = shuffledCarNumbers[r];
            shuffledCarNumbers[r] = temp;
        }

        return shuffledCarNumbers;
    }

    public void RevealCard(MainCard card)
    {
        
        if(_firstRevealedCard == null)
        {
            _firstRevealedCard = card;
        }
        else
        {
            _secondRevealedCard = card;
            StartCoroutine(CheckCardMatchCoroutine());
        }
    }

    private IEnumerator CheckCardMatchCoroutine()
    {
        if(_firstRevealedCard.Id == _secondRevealedCard.Id)
        {
            FindObjectOfType<AudioManager>().Play("Correct");
            _noc++;
            _tries++;
            scoreLabel.text = "Score: " + _noc + " / 4";
            triesLabel.text = "Tries: " + _tries;
            if (_noc == 4)
            {
                ShowScore();
                end = true;
            }
                
    }
        else
        {
            yield return new WaitForSeconds(1f);
            _tries++;
            triesLabel.text = "Tries: " + _tries;
            _firstRevealedCard.Unreveal();
            _secondRevealedCard.Unreveal();
        }

        _firstRevealedCard = null;
        _secondRevealedCard = null;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
