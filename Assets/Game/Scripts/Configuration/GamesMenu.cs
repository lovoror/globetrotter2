﻿using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Game.Scripts.DataStorage;
using Game.Scripts.Session;
using Game.Scripts;
using Game.Scripts.Matchmaking;
using Nakama;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace Game.Scripts.Configuration
{
    public class GamesMenu : MonoBehaviour
    {

        #region Fields

        [SerializeField] private GameObject _gamePrefab = null;

        [SerializeField] private GameObject _snap = null;

        [SerializeField] private GameObject _content = null;

        [SerializeField] private GamesStorage _gamesStorage = null;

        [SerializeField] private HorizontalScrollSnap _horizontalScrollSnap = null;

        [SerializeField] private MatchmakingMenu _matchmakingMenu = null;

        #endregion

        #region Properties

        private Client Client { get { return NakamaSessionManager.Instance.Client; } }

        private ISession Session { get { return NakamaSessionManager.Instance.Session; } }

        #endregion

        private void Awake()
        {
            if (NakamaSessionManager.Instance.IsConnected == false)
            {
                NakamaSessionManager.Instance.OnConnectionSuccess += Init;
            }
            else
            {
                Init();
            }
        }

        async private void Init()
        {
            NakamaSessionManager.Instance.OnConnectionSuccess -= Init;
            //_showClan.onClick.AddListener(() => ShowClanLeaderboards(null));
            //_showGlobal.onClick.AddListener(() => ShowGlobalLeaderboards(null));
            //_showFriends.onClick.AddListener(() => ShowFriendsLeaderboards(null));
            await ShowAsync();
        }

        public async Task<bool> ShowAsync()
        {

            Game _game = await _gamesStorage.LoadDataAsync(null, "games");

            //GameObject xxx = Instantiate(_gamePrefab, _content.transform) as GameObject;
            //xxx.transform.parent = _content.transform;
            //xxx.transform.SetParent(_content.transform);

            _game.matchmaking.ForEach(delegate (MatchmakingGame _matchmakingGame)
            {
                GameObject game = Instantiate(_gamePrefab, _content.transform) as GameObject;

                game.GetComponentInChildren<Text>().text = _matchmakingGame.name;
                game.GetComponentInChildren<Button>().onClick.AddListener(() =>
                {
                    _matchmakingMenu.matchmakingGame = _matchmakingGame;
                    Menus.MenuManager.Instance.ShowMenu(_matchmakingMenu, true);
                });
            });

            _horizontalScrollSnap.UpdateLayout();
            //_snap.SetActive(true);

            //Debug.Log(result);

            return true;
        }
    }
}
