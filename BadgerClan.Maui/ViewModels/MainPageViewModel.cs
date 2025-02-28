﻿
using BadgerClan.Maui.Messages;
using BadgerClan.Maui.Models;
using BadgerClan.Shared;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BadgerClan.Maui.ViewModels
{
    public partial class MainPageViewModel : ObservableObject, IRecipient<StrategyChangedMessage>, IQueryAttributable
    {
        HttpClient _client;

        [ObservableProperty]
        public ClientModel _clientModel;
        public ObservableCollection<StrategyModel> Strategies { get; set; }
        public MainPageViewModel()
        {
            Strategies = new ObservableCollection<StrategyModel>()
            {
                new StrategyModel(StrategyType.MyStrategy),
                new StrategyModel(StrategyType.OtherStrategy),
                new StrategyModel(StrategyType.DoNothing),
                new StrategyModel(StrategyType.CircleStrategy)
            };
            WeakReferenceMessenger.Default.Register(this);
        }

        private async Task UpdateStrategy(StrategyType stratType)
        {
            await _client.PostAsJsonAsync("changestrategy", new StrategyDTO() { StratType = stratType });
        }


        public async void Receive(StrategyChangedMessage message)
        {
            await UpdateStrategy(message.StratType);
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            ClientModel = (ClientModel)query["client"];
            _client = ClientModel._client;
        }
    }
}
