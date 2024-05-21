using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyReward : MonoBehaviour
{
    public GameObject[] dailyRewardDayImage;
    public DailyRewardData[] rewardDatas;
    public GameObject warningDaily;
    public GameObject claimButtons;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("DailyReward") != DateTime.Now.Day)
        {
            warningDaily.SetActive(true);
            dailyRewardDayImage[PlayerPrefs.GetInt("CountLoginDay") % 7].transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
            dailyRewardDayImage[Mathf.Clamp(PlayerPrefs.GetInt("CountLoginDay") % 7 - 1, 0, 100)].transform.GetChild(0).gameObject.SetActive(true);
            claimButtons.SetActive(false);
        }

        for (int i = 0; i < PlayerPrefs.GetInt("CountLoginDay") % 7; i++)
        {
            dailyRewardDayImage[i].transform.GetChild(3).gameObject.SetActive(true);
        }
    }

    public void Claim()
    {
        PlayerPrefs.SetInt("DailyReward", DateTime.Now.Day);
        dailyRewardDayImage[PlayerPrefs.GetInt("CountLoginDay") % 7].transform.GetChild(3).gameObject.SetActive(true);

        //Get reward
        if (rewardDatas[PlayerPrefs.GetInt("CountLoginDay") % 7].type == DailyRewardData.RewardType.Coin)
        {
            DataManager.EarnCoin(rewardDatas[PlayerPrefs.GetInt("CountLoginDay") % 7].amount);
            CurrencyManager.Instance.UpdateCurrency();
        }
        else if (rewardDatas[PlayerPrefs.GetInt("CountLoginDay") % 7].type == DailyRewardData.RewardType.Ticket)
        {
            DataManager.EarnTicket(rewardDatas[PlayerPrefs.GetInt("CountLoginDay") % 7].amount);
            CurrencyManager.Instance.UpdateCurrency();
        }
        claimButtons.SetActive(false);

        //Next day
        int count = PlayerPrefs.GetInt("CountLoginDay") + 1;
        PlayerPrefs.SetInt("CountLoginDay", count);

        //Turn off warning
        warningDaily.SetActive(false);
    }

    public void ClaimX2()
    {
        if (DataManager.SpendTicket(1))
        {
            PlayerPrefs.SetInt("DailyReward", DateTime.Now.Day);
            dailyRewardDayImage[PlayerPrefs.GetInt("CountLoginDay") % 7].transform.GetChild(3).gameObject.SetActive(true);

            //Get reward
            if (rewardDatas[PlayerPrefs.GetInt("CountLoginDay") % 7].type == DailyRewardData.RewardType.Coin)
            {
                DataManager.EarnCoin(rewardDatas[PlayerPrefs.GetInt("CountLoginDay") % 7].amount * 2);
            }
            else if (rewardDatas[PlayerPrefs.GetInt("CountLoginDay") % 7].type == DailyRewardData.RewardType.Ticket)
            {
                DataManager.EarnTicket(rewardDatas[PlayerPrefs.GetInt("CountLoginDay") % 7].amount * 2);
                CurrencyManager.Instance.UpdateCurrency();
            }
            claimButtons.SetActive(false);

            CurrencyManager.Instance.UpdateCurrency();

            //Next day
            int count = PlayerPrefs.GetInt("CountLoginDay") + 1;
            PlayerPrefs.SetInt("CountLoginDay", count);

            //Turn off warning
            warningDaily.SetActive(false);
        }
        else
        {
            //AdsManager.Instance.ShowRewardedVideo("Daily", (result) =>
            //{
            //    switch (result)
            //    {
            //        case ShowResult.Failed:

            //            break;
            //        case ShowResult.Skipped:

            //            break;
            //        case ShowResult.Finished:
            //            PlayerPrefs.SetInt("DailyReward", DateTime.Now.Day);
            //            dailyRewardDayImage[PlayerPrefs.GetInt("CountLoginDay") % 7].transform.GetChild(3).gameObject.SetActive(true);

            //            //Get reward
            //            if (rewardDatas[PlayerPrefs.GetInt("CountLoginDay") % 7].type == DailyRewardData.RewardType.Coin)
            //            {
            //                DataManager.EarnCoin(rewardDatas[PlayerPrefs.GetInt("CountLoginDay") % 7].amount * 2);
            //                CurrencyManager.Instance.UpdateCurrency();
            //            }
            //            else if (rewardDatas[PlayerPrefs.GetInt("CountLoginDay") % 7].type == DailyRewardData.RewardType.Screw)
            //            {
            //                int screw = PlayerPrefs.GetInt("Screw") + rewardDatas[PlayerPrefs.GetInt("CountLoginDay") % 7].amount * 2;
            //                PlayerPrefs.SetInt("Screw", screw);
            //            }
            //            else if (rewardDatas[PlayerPrefs.GetInt("CountLoginDay") % 7].type == DailyRewardData.RewardType.Saw)
            //            {
            //                int saw = PlayerPrefs.GetInt("Saw") + rewardDatas[PlayerPrefs.GetInt("CountLoginDay") % 7].amount * 2;
            //                PlayerPrefs.SetInt("Saw", saw);
            //            }
            //            else if (rewardDatas[PlayerPrefs.GetInt("CountLoginDay") % 7].type == DailyRewardData.RewardType.Ticket)
            //            {
            //                DataManager.EarnTicket(rewardDatas[PlayerPrefs.GetInt("CountLoginDay") % 7].amount * 2);
            //                CurrencyManager.Instance.UpdateCurrency();
            //            }
            //            claimButtons.SetActive(false);

            //            //Next day
            //            int count = PlayerPrefs.GetInt("CountLoginDay") + 1;
            //            if (count > 6) count = 0;
            //            PlayerPrefs.SetInt("CountLoginDay", count);

            //            //Turn off warning
            //            warningDaily.SetActive(false);
            //            break;
            //        default:
            //            break;
            //    }
            //});
        }
    }
}



[System.Serializable]
public struct DailyRewardData
{
    public enum RewardType
    {
        Coin, Ticket
    }
    public RewardType type;
    public int amount;
}

