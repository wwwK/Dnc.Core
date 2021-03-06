﻿using CSRedis;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Dnc.Redis
{
    /// <summary>
    /// Uses CSRedis to connect redis.
    /// </summary>
    public class CsRedis
        : IRedis
    {
        #region Private memeber.
        private readonly CSRedisClient _client;
        private readonly int _seconds = 30;
        #endregion

        #region Ctor.
        public CsRedis(CSRedisClient client, int seconds)
        {
            _client = client;
            _seconds = seconds;
        }
        #endregion

        #region Sync.
        public void Set<T>(string key, T t, int expireMS) => _client.Set(key, t, RandomExpireMS(expireMS));

        public void Clear(string key)
        {
            if (!_client.Exists(key))
                return;
            _client.Del(key);
        }

        public T TryGetOrCreate<T>(string key, Func<T> func, int expireMS)
        {
            var val = _client.Get<T>(key);
            if (val != null)
                return val;

            var rt = func();
            _client.Set(key, rt, RandomExpireMS(expireMS));
            return rt;
        }

        public T TryGetOrCreateDistributely<T>(string key, Func<T> func, int expireMS)
        {
            var val = _client.Get<T>(key);
            if (val != null)
                return val;

            if (_client.Set("mutex", 1, 60 * 2, RedisExistence.Nx))
            {
                var rt = func();
                _client.Set(key, rt, RandomExpireMS(expireMS));
                _client.Del("mutex");
                return rt;
            }
            else
            {
                Thread.Sleep(50);
                return TryGetOrCreateDistributely(key, func, expireMS);
            }
        }

        public long Like<T>(object id, string desc, params T[] likedMembers)
        {
            if (likedMembers == null)
                throw new ArgumentNullException(nameof(likedMembers));

            return _client.SAdd(GenerateStoreKey<T>(nameof(Like), id, desc), likedMembers);
        }

        public long Count<T>(object id, string desc, long increment)
        {
            return _client.IncrBy(GenerateStoreKey<T>(nameof(Count), id, desc), increment);
        }

        public long Rank<T>(string desc, params Ranking<T>[] rankings)
        {
            if (rankings == null)
                throw new ArgumentNullException(nameof(rankings));

            var items = new List<(double, object)>();
            foreach (var ranking in rankings)
            {
                items.Add((ranking.Score, ranking.Ranked));
            }
            return _client.ZAdd(GenerateStoreKey<T>(nameof(Rank), desc), items.ToArray());
        }
        #endregion

        #region Async.
        public async Task SetAsync<T>(string key, T t, int expireMS) => await _client.SetAsync(key, t, RandomExpireMS(expireMS));

        public async Task ClearAsync(string key)
        {
            if (!await _client.ExistsAsync(key))
                return;
            await _client.DelAsync(key);
        }

        /// <summary>
        /// Try get value from cache or create into cache async. 
        /// </summary>
        /// <param name="key">key name.</param>
        /// <param name="func">Delegate used to return value.</param>
        /// <returns></returns>
        public async Task<T> TryGetOrCreateAsync<T>(string key,
            Func<Task<T>> func, int expireMS)
        {
            var val = await _client.GetAsync<T>(key);

            if (val != null)
                return val;

            var rt = await func();
            await _client.SetAsync(key, rt, RandomExpireMS(expireMS));
            return rt;
        }

        public async Task<T> TryGetOrCreateDistributelyAsync<T>(string key, Func<T> func, int expireMS)
        {
            var val = await _client.GetAsync<T>(key);
            if (val != null)
                return val;

            if (await _client.SetAsync($"mutex{key}", 1, 60 * 2, RedisExistence.Nx))
            {
                var rt = func();
                await _client.SetAsync(key, rt, RandomExpireMS(expireMS));
                await _client.DelAsync($"mutex{key}");
                return rt;
            }
            else
            {
                Thread.Sleep(50);
                return await TryGetOrCreateDistributelyAsync(key, func, expireMS);
            }
        }

        public async Task<long> LikeAsync<T>(object id, string desc, params T[] likedMembers)
        {
            if (likedMembers == null)
                throw new ArgumentNullException(nameof(likedMembers));

            return await _client.SAddAsync(GenerateStoreKey<T>(nameof(Like), id, desc), likedMembers);
        }

        public async Task<long> CountAsync<T>(object id, string desc, long increment = 1) => await _client.IncrByAsync(GenerateStoreKey<T>(nameof(Count), id, desc), increment);

        public async Task<long> RankAsync<T>(string desc, params Ranking<T>[] rankings)
        {
            if (rankings == null)
                throw new ArgumentNullException(nameof(rankings));

            var items = new List<(double, object)>();
            foreach (var ranking in rankings)
            {
                items.Add((ranking.Score, ranking.Ranked));
            }
            return await _client.ZAddAsync(GenerateStoreKey<T>(nameof(Rank), desc), items.ToArray());
        }
        #endregion

        #region Helper.
        private int RandomExpireMS(int expireMS) => expireMS + new Random().Next(0, _seconds + 1);

        private string GenerateStoreKey<T>(string operation, params object[] paras) => $"{typeof(T).Name}:{string.Join(":", paras)}_{operation.ToLower()}";
        #endregion
    }
}
