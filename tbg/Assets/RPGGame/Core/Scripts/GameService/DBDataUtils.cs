﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using SQLite3TableDataTmp;
using UnityEngine;
using UnityEngine.Events;

public class DBDataUtils
{

    public string dbPath = "./data.sqlite3";


    public void Init()
    {
        if (Application.isMobilePlatform)
        {
            if (dbPath.StartsWith("./"))
                dbPath = dbPath.Substring(1);
            if (!dbPath.StartsWith("/"))
                dbPath = "/" + dbPath;
            dbPath = Application.persistentDataPath + dbPath;
        }
        if (!File.Exists(dbPath))
            //SqliteConnection.CreateFile(dbPath);

        // open connection
        //connection = new SqliteConnection("URI=file:" + dbPath);

        ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS player (
            id TEXT NOT NULL PRIMARY KEY,
            profileName TEXT NOT NULL,
            loginToken TEXT NOT NULL,
            exp INTEGER NOT NULL,
            selectedFormation TEXT NOT NULL,
            prefs TEXT NOT NULL
            )");

        //ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS playerItem (
        //    id TEXT NOT NULL PRIMARY KEY,
        //    playerId TEXT NOT NULL,
        //    Guid TEXT NOT NULL,
        //    amount INTEGER NOT NULL,
        //    exp INTEGER NOT NULL,
        //    equipItemId TEXT NOT NULL,
        //    equipPosition TEXT NOT NULL)");

        ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS playerAuth (
            id TEXT NOT NULL PRIMARY KEY,
            playerId TEXT NOT NULL,
            type TEXT NOT NULL,
            username TEXT NOT NULL,
            password TEXT NOT NULL)");

        ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS playerCurrency (
            id TEXT NOT NULL PRIMARY KEY,
            playerId TEXT NOT NULL,
            Guid TEXT NOT NULL,
            amount INTEGER NOT NULL,
            purchasedAmount INTEGER NOT NULL)");

        ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS playerStamina (
            id TEXT NOT NULL PRIMARY KEY,
            playerId TEXT NOT NULL,
            Guid TEXT NOT NULL,
            amount INTEGER NOT NULL,
            recoveredTime INTEGER NOT NULL)");

        ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS playerFormation (
            id INTEGER PRIMARY KEY,
            playerId TEXT NOT NULL,
            Guid TEXT NOT NULL,
            position INTEGER NOT NULL,
            itemId TEXT NOT NULL,
            itemguid TEXT NOT NULL)");

        ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS playerUnlockItem (
            id TEXT NOT NULL PRIMARY KEY,
            playerId TEXT NOT NULL,
            Guid TEXT NOT NULL,
            amount INTEGER NOT NULL)");

        ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS playerClearStage (
            id TEXT NOT NULL PRIMARY KEY,
            playerId TEXT NOT NULL,
            Guid TEXT NOT NULL,
            bestRating INTEGER NOT NULL)");

        ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS playerBattle (
            id TEXT NOT NULL PRIMARY KEY,
            playerId TEXT NOT NULL,
            Guid TEXT NOT NULL,
            session TEXT NOT NULL,
            battleResult INTEGER NOT NULL,
            rating INTEGER NOT NULL)");


        ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS playerOtherItem (
            id TEXT NOT NULL PRIMARY KEY,
            Guid TEXT NOT NULL,
            playerId TEXT NOT NULL,
            amount INTEGER NOT NULL)");


        ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS playerHasCharacters (
            itemid TEXT NOT NULL,
            playerId TEXT NOT NULL,
            Guid TEXT NOT NULL,
            amount INTEGER NOT NULL,
            exp INTEGER NOT NULL,
            id INTEGER PRIMARY KEY)");

        ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS playerHasEquips (
            itemid TEXT NOT NULL,
            playerId TEXT NOT NULL,
            Guid TEXT NOT NULL,
            amount INTEGER NOT NULL,
            exp INTEGER NOT NULL,
            equipItemGuid TEXT NOT NULL,
            equipPosition TEXT NOT NULL,
            id INTEGER PRIMARY KEY)");

    }

    #region 数据库操作

    public void ExecuteNonQuery(string sql, params object[] args)
    {
        //connection.Open();
        //using (var cmd = new SqliteCommand(sql, connection))
        //{
        //    foreach (var arg in args)
        //    {
        //        cmd.Parameters.Add(arg);
        //    }
        //    cmd.ExecuteNonQuery();
        //}
        //connection.Close();
    }

    public object ExecuteScalar(string sql, params object[] args)
    {
        object result = null;
//        connection.Open();
//        using (var cmd = new SqliteCommand(sql, connection))
//        {
//            foreach (var arg in args)
//            {
//                cmd.Parameters.Add(arg);
//            }
//            result = cmd.ExecuteScalar();
//        }
//        connection.Close();
        return result;
    }

    public DbRowsReader ExecuteReader(string sql, params object[] args)
    {
        DbRowsReader result = new DbRowsReader();
        //connection.Open();
        //using (var cmd = new SqliteCommand(sql, connection))
        //{
        //    foreach (var arg in args)
        //    {
        //        cmd.Parameters.Add(arg);
        //    }
        //    result.Init(cmd.ExecuteReader());
        //}
        //connection.Close();
        return result;
    }

    #endregion























    //playerHasCharacters
    //private bool AddItems1(string playerId,
    //    string Guid,
    //    int amount,
    //    out List<PlayerItem> createItems,
    //    out List<PlayerItem> updateItems)
    //{
    //    createItems = new List<PlayerItem>();
    //    updateItems = new List<PlayerItem>();
    //    CharacterItem itemData = null;
    //    if (!GameInstance.GameDatabase.characters.TryGetValue(Guid, out itemData))
    //        return false;
    //    var maxStack = itemData.MaxStack;
    //    var oldEntries = ExecuteReader(@"SELECT * FROM playerItem WHERE playerId=@playerId AND Guid=@Guid AND amount<@amount",
    //        new SqliteParameter("@playerId", playerId),
    //        new SqliteParameter("@Guid", Guid),
    //        new SqliteParameter("@amount", maxStack));
    //    while (oldEntries.Read())
    //    {
    //        var entry = new PlayerItem();
    //        entry.characterGuid = oldEntries.GetString(0);
    //        entry.PlayerId = oldEntries.GetString(1);
    //        entry.GUID = oldEntries.GetString(2);
    //        entry.Amount = oldEntries.GetInt32(3);
    //        entry.Exp = oldEntries.GetInt32(4);
    //        entry.equipItemGuid = oldEntries.GetString(5);
    //        entry.EquipPosition = oldEntries.GetString(6);
    //        var sumAmount = entry.Amount + amount;
    //        if (sumAmount > maxStack)
    //        {
    //            entry.Amount = maxStack;
    //            amount = sumAmount - maxStack;
    //        }
    //        else
    //        {
    //            entry.Amount += amount;
    //            amount = 0;
    //        }
    //        updateItems.Add(entry);

    //        if (amount == 0)
    //            break;
    //    }
    //    while (amount > 0)
    //    {
    //        var newEntry = new PlayerItem();
    //        newEntry.PlayerId = playerId;
    //        newEntry.GUID = Guid;
    //        if (amount > maxStack)
    //        {
    //            newEntry.Amount = maxStack;
    //            amount -= maxStack;
    //        }
    //        else
    //        {
    //            newEntry.Amount = amount;
    //            amount = 0;
    //        }
    //        createItems.Add(newEntry);
    //    }
    //    return true;
    //}





    //private void HelperUnlockItem(string playerId, string formationId)
    //{
    //    PlayerUnlockItem unlockItem = null;
    //    var oldUnlockItems = ExecuteReader(@"SELECT * FROM playerUnlockItem WHERE playerId=@playerId AND Guid=@Guid LIMIT 1",
    //        new SqliteParameter("@playerId", playerId),
    //        new SqliteParameter("@Guid", formationId));
    //    if (!oldUnlockItems.Read())
    //    {
    //        unlockItem = new PlayerUnlockItem();
    //        unlockItem.characterGuid = PlayerUnlockItem.GetId(playerId, formationId);
    //        unlockItem.PlayerId = playerId;
    //        unlockItem.formationId = formationId;
    //        unlockItem.Amount = 0;
    //        ExecuteNonQuery(@"INSERT INTO playerUnlockItem (id, playerId, Guid, amount)
    //            VALUES (@id, @playerId, @Guid, @amount)",
    //            new SqliteParameter("@id", unlockItem.characterGuid),
    //            new SqliteParameter("@playerId", unlockItem.PlayerId),
    //            new SqliteParameter("@Guid", unlockItem.formationId),
    //            new SqliteParameter("@amount", unlockItem.Amount));
    //    }
    //}







    //private PlayerItem GetPlayerItemById(string id)
    //{
    //    PlayerItem playerItem = null;
    //    var playerItems = ExecuteReader(@"SELECT * FROM playerItem WHERE id=@id",
    //        new SqliteParameter("@id", id));
    //    if (playerItems.Read())
    //    {
    //        playerItem = new PlayerItem();
    //        playerItem.characterGuid = playerItems.GetString(0);
    //        playerItem.PlayerId = playerItems.GetString(1);
    //        playerItem.GUID = playerItems.GetString(2);
    //        playerItem.Amount = playerItems.GetInt32(3);
    //        playerItem.Exp = playerItems.GetInt32(4);
    //        playerItem.equipItemGuid = playerItems.GetString(5);
    //        playerItem.EquipPosition = playerItems.GetString(6);
    //    }
    //    return playerItem;
    //}





    #region 商城




    public void DoGetAvailableLootBoxList(UnityAction<AvailableLootBoxListResult> onFinish)
    {
        var result = new AvailableLootBoxListResult();
        var gameDb = GameInstance.GameDatabase;
        result.list.AddRange(gameDb.LootBoxes.Keys);
        onFinish(result);
    }

    public void DoOpenLootBox(string lootBoxDataId, int packIndex, UnityAction<ItemResult> onFinish)
    {
        //var cplayer = IPlayer.CurrentPlayer;
        //var playerId = cplayer.guid;
        //var loginToken = cplayer.loginToken;
        //var result = new ItemResult();
        //var gameDb = GameInstance.GameDatabase;
        //var player = GameInstance.dbLogin.GetPlayerByLoginToken(playerId, loginToken);
        //LootBox lootBox;
        //if (player == null)
        //    result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        //else if (!gameDb.LootBoxes.TryGetValue(lootBoxDataId, out lootBox))
        //    result.error = GameServiceErrorCode.INVALID_LOOT_BOX_DATA;
        //else
        //{
        //    var softCurrency = GameInstance.dbPlayerData.GetCurrency(playerId, gameDb.softCurrency.id);
        //    var hardCurrency = GameInstance.dbPlayerData.GetCurrency(playerId, gameDb.hardCurrency.id);
        //    var requirementType = lootBox.requirementType;
        //    if (packIndex > lootBox.lootboxPacks.Length - 1)
        //        packIndex = 0;
        //    var pack = lootBox.lootboxPacks[packIndex];
        //    var price = pack.price;
        //    var openAmount = pack.openAmount;
        //    if (requirementType == LootBoxRequirementType.RequireSoftCurrency && price > softCurrency.amount)
        //        result.error = GameServiceErrorCode.NOT_ENOUGH_SOFT_CURRENCY;
        //    else if (requirementType == LootBoxRequirementType.RequireHardCurrency && price > hardCurrency.amount)
        //        result.error = GameServiceErrorCode.NOT_ENOUGH_HARD_CURRENCY;
        //    else
        //    {
        //        switch (requirementType)
        //        {
        //            case LootBoxRequirementType.RequireSoftCurrency:
        //                softCurrency.amount -= price;
        //                GameInstance.dbDataUtils.ExecuteNonQuery(@"UPDATE playerCurrency SET amount=@amount WHERE id=@id",
        //                    new SqliteParameter("@amount", softCurrency.amount),
        //                    new SqliteParameter("@id", softCurrency.id));
        //                result.updateCurrencies.Add(softCurrency);
        //                break;
        //            case LootBoxRequirementType.RequireHardCurrency:
        //                hardCurrency.amount -= price;
        //                GameInstance.dbDataUtils.ExecuteNonQuery(@"UPDATE playerCurrency SET amount=@amount WHERE id=@id",
        //                    new SqliteParameter("@amount", hardCurrency.amount),
        //                    new SqliteParameter("@id", hardCurrency.id));
        //                result.updateCurrencies.Add(hardCurrency);
        //                break;
        //        }

        //        for (var i = 0; i < openAmount; ++i)
        //        {
        //            var rewardItem = lootBox.RandomReward().rewardItem;
        //            //var createItems = new List<PlayerItem>();
        //            //var updateItems = new List<PlayerItem>();
        //            //todo 开商城的数据
        //            //if (AddItems(playerId, rewardItem.characterGuid, rewardItem.amount, out createItems, out updateItems))
        //            //{

        //            //    foreach (var createEntry in createItems)
        //            //    {
        //            //        createEntry.characterGuid = System.Guid.NewGuid().ToString();
        //            //        ExecuteNonQuery(@"INSERT INTO playerItem (id, playerId, Guid, amount, exp, equipItemGuid, equipPosition) VALUES (@id, @playerId, @Guid, @amount, @exp, @equipItemId, @equipPosition)",
        //            //            new SqliteParameter("@id", createEntry.characterGuid),
        //            //            new SqliteParameter("@playerId", createEntry.PlayerId),
        //            //            new SqliteParameter("@Guid", createEntry.GUID),
        //            //            new SqliteParameter("@amount", createEntry.Amount),
        //            //            new SqliteParameter("@exp", createEntry.Exp),
        //            //            new SqliteParameter("@equipItemGuid", createEntry.equipItemGuid),
        //            //            new SqliteParameter("@equipPosition", createEntry.EquipPosition));
        //            //        result.createItems.Add(createEntry);
        //            //        HelperUnlockItem(player.characterGuid, rewardItem.characterGuid);
        //            //    }
        //            //    foreach (var updateEntry in updateItems)
        //            //    {
        //            //        ExecuteNonQuery(@"UPDATE playerItem SET playerId=@playerId, Guid=@Guid, amount=@amount, exp=@exp, equipItemId=@equipItemId, equipPosition=@equipPosition WHERE id=@id",
        //            //            new SqliteParameter("@playerId", updateEntry.PlayerId),
        //            //            new SqliteParameter("@Guid", updateEntry.GUID),
        //            //            new SqliteParameter("@amount", updateEntry.Amount),
        //            //            new SqliteParameter("@exp", updateEntry.Exp),
        //            //            new SqliteParameter("@equipItemGuid", updateEntry.equipItemGuid),
        //            //            new SqliteParameter("@equipPosition", updateEntry.EquipPosition),
        //            //            new SqliteParameter("@id", updateEntry.characterGuid));
        //            //        result.updateItems.Add(updateEntry);
        //            //    }
        //            //}
        //        }
        //    }
        //}
        //onFinish(result);
    }

    #endregion

}
