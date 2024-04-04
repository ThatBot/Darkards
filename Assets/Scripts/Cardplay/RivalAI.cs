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

        for (int i = 0; i < CardManager.Instance.RivalCreatureLanes.Length; i++)
        {
            if(CardManager.Instance.RivalCreatureLanes[i] == null) _fieldFull = false;
        }
        
        for (int i = 0; i < CardManager.Instance.PlayerCreatureLanes.Length; i++)
        {
            if(CardManager.Instance.PlayerCreatureLanes[i] == null) 
            {
                _playerFieldFull = false;
                _playerEmptyLane = i;
            }
        }

        for (int i = 0; i < CardManager.Instance.RivalHand.Count; i++)
        {
            if(CardManager.Instance.RivalHand[i].Type == CardType.Creature) _hasCreatures = true;
            if(CardManager.Instance.RivalHand[i].Type == CardType.Spell) _hasSpells = true;
        }

        Debug.Log("Current board: [AI Full: " + _fieldFull + ", Player Full: " + _playerFieldFull + "]");
        Debug.Log("Current hand: [Creatures?: " + _hasCreatures + ", Spells?: " + _hasSpells + "]");

        if((!_playerFieldFull && _hasCreatures) && consecutiveActions < MAX_CONSECUTIVE_ACTIONS)
        {
            Debug.Log("Casting a creature to counteract player");
            PlayAction(true, _playerEmptyLane);
            return;
        }

        if(!_fieldFull)
        {
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

        if((AggressionTokens > DefensiveTokens || !_hasSpells) && consecutiveActions < MAX_CONSECUTIVE_ACTIONS)
        {
            Debug.Log("Aggressive stance, attacking");
            AttackAction();
        }
        else if((AggressionTokens == DefensiveTokens && CardManager.Instance.RivalDeck.Count <= 0) && consecutiveDraws < MAX_CONSECUTIVE_DRAWS && consecutiveActions < MAX_CONSECUTIVE_ACTIONS)
        {
            Debug.Log("Neutral stance, drawing");
            DrawAction();
        }
        else if((_hasSpells) && consecutiveActions < MAX_CONSECUTIVE_ACTIONS)
        {
            Debug.Log("Defensive stance, casting");
            PlayAction(false);
        }
        else if((CardManager.Instance.RivalDeck.Count <= 0) && consecutiveDraws < MAX_CONSECUTIVE_DRAWS && consecutiveActions < MAX_CONSECUTIVE_ACTIONS)
        {
            Debug.Log("No other option, drawing");
            DrawAction();
        }
        else // There is nothing you can do, pass the turn
        {
            Debug.Log("No other option, passing turn");
            CardManager.Instance.SwitchTurn();
        }
    }

    public void AttackAction()
    {
        if(lastActionIndex == 1) consecutiveActions++;
        else consecutiveActions = 0;
        lastActionIndex = 1;

        List<int> _availableAttackLanes = new List<int>();

        for (int i = 0; i < CardManager.Instance.RivalCreatureLanes.Length; i++)
        {
            _availableAttackLanes.Add(i);
        }

        int _attacker = _availableAttackLanes[Random.Range(0, _availableAttackLanes.Count)];
        int _defendant = -1;
        int _rand = -1;

        while(_defendant != -1)
        {
            // Used to select lane to attack
            _rand = Random.Range(0, 2);

            if(CardManager.Instance.PlayerCreatureLanes[_rand] == null)
            {
                if(CardManager.Instance.PlayerStructureLanes[_rand] == null)
                {
                    _defendant = -1;
                }
                else
                {
                    _defendant = 0;
                }
            }
            else
            {
                _defendant = 1;
            }
        }

        CardManager.Instance.AttackCard(_attacker, 2, _rand, _defendant);
        consecutiveDraws = 0;
    }

    public void DrawAction()
    {
        if(lastActionIndex == 2) consecutiveActions++;
        else consecutiveActions = 0;
        lastActionIndex = 2;

        consecutiveDraws++;
        CardManager.Instance.DrawCard(false);
    }
 
    public void PlayAction(bool _creature)
    {
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
