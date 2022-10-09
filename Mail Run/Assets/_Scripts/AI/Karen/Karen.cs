using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

namespace GGD
{
    public class Karen : NPCBehaviourState
    {
        private static string[] words = new string[]
        {
            "blah",
            "la",
            "dee",
            "da",
            "do",
            "bee",
            "boo",
            "boop",
        };

        public GameObject dialogueBox;
        public TextMeshProUGUI dialogueText;
        public GameObject indicator;
        [SerializeField] private int clicks = 10;
        [SerializeField] private BehaviourState _patrolState;

        private int press;

        PlayerController player;

        protected override void OnEnter()
        {
            player = GameManager.Instance.Player.GetComponent<PlayerController>();
            _NPC.NavMeshAgent.SetDestination(player.transform.position);

            player.NumInteractions++;
            _NPC.NumInteractions++;
            player.SetIsControllable(false);
            dialogueBox.SetActive(true);
            RandomizeDialogueText();

            indicator.SetActive(true);

            press = clicks;
        }

        public override void ExecuteUpdate(float deltaTime)
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= 1f)
            {
                _NPC.NavMeshAgent.SetDestination(transform.position);
            }

            if (Input.GetButtonDown("Fire1"))
            {
                press--;
                RandomizeDialogueText();
                // Debug.Log(press);
            }

            if (press <= 0)
            {
                dialogueBox.SetActive(false);
                indicator.SetActive(false);
                player.NumInteractions--;
                _NPC.NumInteractions--; ;
                player.SetIsControllable(true);
                _NPC.StateController.SetState(_patrolState);
            }
        }

        private void RandomizeDialogueText()
        {
            StringBuilder sb = new();
            int numWords = Random.Range(10, 40);
            string firstWord = words[Random.Range(0, words.Length)];
            firstWord = firstWord[0].ToString().ToUpper() + firstWord.Substring(1);
            sb.Append(firstWord);
            for (int i = 1; i < numWords; i++)
            {
                sb.Append(" " + words[Random.Range(0, words.Length)]);
            }

            int rand = Random.Range(0, 4);
            if (rand == 0)
                sb.Append(".");
            else if (rand == 1)
                sb.Append("...");
            else if (rand == 2)
                sb.Append("!");
            else if (rand == 3)
                sb.Append("?");

            dialogueText.text = sb.ToString();
        }
    }
}
