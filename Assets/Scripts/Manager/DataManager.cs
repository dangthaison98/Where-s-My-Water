using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataManager
{
    public static string LEVEL = "Level";
    public static string COIN = "Coin";
    public static string TICKET = "Ticket";

    //Level
    public static int GetLevel()
    {
        return PlayerPrefs.GetInt(LEVEL);
    }
    public static void SetLevel(int level)
    {
        PlayerPrefs.SetInt(LEVEL, level);
    }
    public static void CompleteLevel()
    {
        SetLevel(GetLevel() + 1);
    }


    //Coin
    public static int GetCoin()
    {
        return PlayerPrefs.GetInt(COIN);
    }
    public static void SetCoin(int coin)
    {
        PlayerPrefs.SetInt(COIN, coin);
    }
    public static void EarnCoin(int coin)
    {
        SetCoin(GetCoin() + coin);
    }
    public static bool SpendCoin(int coin)
    {
        if(GetCoin() >= coin)
        {
            SetCoin(GetCoin() - coin);
            return true;
        }
        else
        {
            return false;
        }
    }

    //Ticket
    public static int GetTicket()
    {
        return PlayerPrefs.GetInt(TICKET);
    }
    public static void SetTicket(int ticket)
    {
        PlayerPrefs.SetInt(TICKET, ticket);
    }
    public static void EarnTicket(int ticket)
    {
        SetTicket(GetTicket() + ticket);
    }
    public static bool SpendTicket(int ticket)
    {
        if (GetTicket() >= ticket)
        {
            SetTicket(GetTicket() - ticket);
            return true;
        }
        else
        {
            return false;
        }
    }
}
