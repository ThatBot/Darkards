using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RivalAI
{
    public int AggressionTokens;
    public int DefensiveTokens;

    private int consecutiveDraws = 0;
    private const int MAX_CONSECUTIVE_DRAWS = 2;
    private int consecutiveActions = 0;
    private int lastActionIndex = -1;
    private const int MAX_CONSECUTIVE_ACTIONS = 3;

    public void ProcessAction()
    {
        Debug.Log("Processing AI action");
        Debug.Log("Current stance: [Aggression: " + AggressionTokens + ", Defensive: " + DefensiveTokens + "]");

        // Check if our hand is low and we haven't drawn recently
        if(CardManager.Instance.RivalHand.Count < 2 && consecutiveDraws < MAX_CONSECUTIVE_DRAWS && consecutiveActions < MAX_CONSECUTIVE_ACTIONS)
        {
            Debug.Log("Hand low, drawing");
            DrawAction();
            return;
        }

        bool _fieldFull = true;
        bool _playerFieldFull = true;
        bool _hasCreatures = false;
        bool _hasSpells = false;
        int _playerEmptyLane = -1;

        // Check if our creature field is full
        for (int i = 0; i < CardManager.Instance.RivalCreatureLanes.Length; i++)
        {
            if(CardManager.Instance.RivalCreatureLanes[i] == null) _fieldFull = false;
        }
        
        // Check if the player's creature field is full
        for (int i = 0; i < CardManager.Instance.PlayerCreatureLanes.Length; i++)
        {
            if(CardManager.Instance.PlayerCreatureLanes[i] == null) 
            {
                _playerFieldFull = false;
                _playerEmptyLane = i;
            }
        }

        // Check if our hand has creatures & spells
        for (int i = 0; i < CardManager.Instance.RivalHand.Count; i++)
        {
            if(CardManager.Instance.RivalHand[i].Type == CardType.Creature) _hasCreatures = true;
            if(CardManager.Instance.RivalHand[i].Type == CardType.Spell) _hasSpells = true;
        }

        Debug.Log("Current board: [AI Full: " + _fieldFull + ", Player Full: " + _playerFieldFull + "]");
        Debug.Log("Current hand: [Creatures?: " + _hasCreatures + ", Spells?: " + _hasSpells + "]");

        // If there is a hole in the player's creature field, place a creature there to exploit it
        if((!_playerFieldFull && _hasCreatures) && consecutiveActions < MAX_CONSECUTIVE_ACTIONS)
        {
            Debug.Log("Casting a creature to counteract player");
            PlayAction(true, _playerEmptyLane);
            return;
        }

        if(!_fieldFull)
        {
            // If we are too aggressive attack, otherwise try to place a creature
            if(AggressionTokens > 4 && consecutiveActions < MAX_CONSECUTIVE_ACTIONS)
            {
                Debug.Log("Aggression outburst, attacking");
                AttackAction();
                return;
            }
            else if(_hasCreatures && consecutiveActions < MAX_CONSECUTIVE_ACTIONS)
            {
                Debug.Log("Casting a creature");
                PlayAction(true);
                return;
            }
        }

        // If we are more aggressive than defensive, attack
        if((AggressionTokens > DefensiveTokens || !_hasSpells))
        {
            Debug.Log("Aggressive stance, attacking");
            AttackAction();
        }
        // If we are aggressive & defensive, assume a neutral position and draw
        else if(AggressionTokens == DefensiveTokens && CardManager.Instance.RivalDeck.Count <= 0 && consecutiveDraws < MAX_CONSECUTIVE_DRAWS)
        {
            Debug.Log("Neutral stance, drawing");
            DrawAction();
        }
        // If we are more defensive than aggressive, cast spells
        else if(_hasSpells)
        {
            Debug.Log("Defensive stance, casting");
            PlayAction(false);
        }
        // If there is no other option, draw
        else if(CardManager.Instance.RivalDeck.Count <= 0)
        {
            Debug.Log("No other option, drawing");
            DrawAction();
        }
        else // If there is nothing we can do, pass the turn
        {
            Debug.Log("No other option, passing turn");
            CardManager.Instance.SwitchTurn();
        }
    }

    public void AttackAction()
    {
        // Check we haven't attacked yet, otherwise increment the counter
        if(lastActionIndex == 1) consecutiveActions++;
        else consecutiveActions = 0;
        lastActionIndex = 1;

        List<int> _availableAttackLanes = new List<int>();

        // Check for creatures in our lanes
        for (int i = 0; i < CardManager.Instance.RivalCreatureLanes.Length; i++)
        {
            if(CardManager.Instance.RivalCreatureLanes[i] != null)
                _availableAttackLanes.Add(i);
        }

        int _attacker = _availableAttackLanes[Random.Range(0, _availableAttackLanes.Count)];
        int _defendantRow = -1;
        int _defendantLane = -1;

        // Choose which player lane to attack, taking blockers into consideration
        //while(_defendant != -1)
        //{

        //    if(CardManager.Instance.PlayerCreatureLanes[_rand] == null)
        //    {
        //        if(CardManager.Instance.PlayerStructureLanes[_rand] == null)
        //        {
        //            _defendant = -1;
        //        }
        //        else
        //        {
        //            _defendant = 0;
        //        }
        //    }
        //    else
        //    {
        //        _defendant = 1;
        //    }
        //}

        for (int i = 0; i < 3; i++)
        {
            if(CardManager.Instance.PlayerCreatureLanes[i] != null) 
            {
                _defendantRow = 1;
                _defendantLane = i;
            }
            else if(CardManager.Instance.PlayerStructureLanes[i] != null) 
            {
                _defendantRow = 0; 
                _defendantLane = i;
            }
        }

        CardManager.Instance.AttackCard(_attacker, 2, _defendantLane, _defendantRow);
        consecutiveDraws = 0;
    }

    public void DrawAction()
    {
        // Check we haven't drawn yet, otherwise increment the counter
        if(lastActionIndex == 2) consecutiveActions++;
        else consecutiveActions = 0;
        lastActionIndex = 2;

        consecutiveDraws++;
        CardManager.Instance.DrawCard(false);
    }
 
    public void PlayAction(bool _creature)
    {
        // Check we haven't played a card yet, otherwise increment the counter
        if(lastActionIndex == 3) consecutiveActions++;
        else consecutiveActions = 0;
        lastActionIndex = 3;

        if(_creature)
        {
            int _cardIndex = -1;

            for (int i = 0; i < CardManager.Instance.RivalHand.Count; i++)
            {
                if(CardManager.Instance.RivalHand[i].Type == CardType.Creature) 
                {
                    _cardIndex = i;
                    break;
                }
            }

            for (int i = 0; i < 3; i++)
            {
                if(CardManager.Instance.RivalCreatureLanes[i] == null)
                {
                    CardManager.Instance.PlayRivalCard(i, 2, _cardIndex);
                    consecutiveDraws = 0;
                    return;
                }
            }
        }
        else // Card is a spell
        {
            int _cardIndex = -1;

            for (int i = 0; i < CardManager.Instance.RivalHand.Count; i++)
            {
                if(CardManager.Instance.RivalHand[i].Type == CardType.Spell) 
                {
                    _cardIndex = i;
                    break;
                }
            }

            if(CardManager.Instance.RivalHand[_cardIndex].Beneficial)
            {
                for (int i = 0; i < 3; i++)
                {
                    if(CardManager.Instance.RivalCreatureLanes[i] != null)
                    {
                        CardManager.Instance.PlayRivalCard(i, 2, _cardIndex);
                        consecutiveDraws = 0;
                        return;
                    }
                }
            }
            else
            {
                int _rand = Random.Range(0, 1);

                for (int i = 0; i < 3; i++)
                {
                    if(CardManager.Instance.PlayerCreatureLanes[i] != null)
                    {
                        CardManager.Instance.PlayRivalCard(i, _rand, _cardIndex);
                        consecutiveDraws = 0;
                        return;
                    }
                }

                for (int i = 0; i < 3; i++)
                {
                    if(CardManager.Instance.PlayerStructureLanes[i] != null)
                    {
                        CardManager.Instance.PlayRivalCard(i, _rand, _cardIndex);
                        consecutiveDraws = 0;
                        return;
                    }
                }
            }
        }
    }

    public void PlayAction(bool _creature, int _lane)
    {
        // Check we haven't played a card yet, otherwise increment the counter
        if(lastActionIndex == 3) consecutiveActions++;
        else consecutiveActions = 0;
        lastActionIndex = 3;

        if(_creature)
        {
            int _cardIndex = -1;

            for (int i = 0; i < CardManager.Instance.RivalHand.Count; i++)
            {
                if(CardManager.Instance.RivalHand[i].Type == CardType.Creature) 
                {
                    _cardIndex = i;
                    break;
                }
            }

            CardManager.Instance.PlayRivalCard(_lane, 2, _cardIndex);
            consecutiveDraws = 0;
        }
        else
        {
            int _cardIndex = -1;

            for (int i = 0; i < CardManager.Instance.RivalHand.Count; i++)
            {
                if(CardManager.Instance.RivalHand[i].Type == CardType.Spell) 
                {
                    _cardIndex = i;
                    break;
                }
            }

            if(CardManager.Instance.RivalHand[_cardIndex].Beneficial)
            {
                for (int i = 0; i < 3; i++)
                {
                    if(CardManager.Instance.RivalCreatureLanes[i] != null)
                    {
                        CardManager.Instance.PlayRivalCard(_lane, 2, _cardIndex);
                        consecutiveDraws = 0;
                        return;
                    }
                }
            }
            else
            {
                int _rand = Random.Range(0, 1);

                for (int i = 0; i < 3; i++)
                {
                    if(CardManager.Instance.PlayerCreatureLanes[i] != null)
                    {
                        CardManager.Instance.PlayRivalCard(_lane, _rand, _cardIndex);
                        consecutiveDraws = 0;
                        return;
                    }
                }

                for (int i = 0; i < 3; i++)
                {
                    if(CardManager.Instance.PlayerStructureLanes[i] != null)
                    {
                        CardManager.Instance.PlayRivalCard(_lane, _rand, _cardIndex);
                        consecutiveDraws = 0;
                        return;
                    }
                }
            }
        }
    }
}
