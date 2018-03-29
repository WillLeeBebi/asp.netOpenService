using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace liemei.Common.cache
{
    /// <summary>
    /// 
    /// </summary>
    public class RedisBase
    {
#if DEBUG
        private static string[] ReadWriteHosts = { "127.0.0.1:6379" };
        private static string[] ReadOnlyHosts = { "127.0.0.1:6379" };
#endif
#if !DEBUG
         private static string[] ReadWriteHosts = System.Configuration.ConfigurationSettings.AppSettings["RedisWriteHosts"].Split(new char[] { ';' });
         private static string[] ReadOnlyHosts = System.Configuration.ConfigurationSettings.AppSettings["RedisReadOnlyHosts"].Split(new char[] { ';' });
#endif


        #region -- 连接信息 --
        public static PooledRedisClientManager prcm = CreateManager(ReadWriteHosts, ReadOnlyHosts);

        private static PooledRedisClientManager CreateManager(string[] readWriteHosts, string[] readOnlyHosts)
        {
            // 支持读写分离，均衡负载  
            return new PooledRedisClientManager(readWriteHosts, readOnlyHosts, new RedisClientManagerConfig
            {
                MaxWritePoolSize = 5, // “写”链接池链接数  
                MaxReadPoolSize = 5, // “读”链接池链接数  
                AutoStart = true,
            });
        }
#endregion
#region KEY
   
#endregion

#region -- Item String --
        /// <summary> 
        /// 设置单体 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <param name="t"></param> 
        /// <param name="timeSpan"></param> 
        /// <returns></returns> 
        public static bool Item_Set<T>(string key, T t)
        {
            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    return redis.Set<T>(key, t, new TimeSpan(1, 0, 0));
                }
            }
            catch (Exception ex)
            {
                // LogInfo 
            }
            return false;
        }
        public static int String_Append(string key, string value)
        {
            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    return redis.AppendToValue(key, value);
                }
            }
            catch (Exception ex) { }
            return 0;
        }
        /// <summary> 
        /// 获取单体 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <returns></returns> 
        public static T Item_Get<T>(string key) where T : class
        {
            using (IRedisClient redis = prcm.GetReadOnlyClient())
            {
                return redis.Get<T>(key);
            }
        }

        /// <summary> 
        /// 移除单体 
        /// </summary> 
        /// <param name="key"></param> 
        public static bool Item_Remove(string key)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                return redis.Remove(key);
            }
        }
        /// <summary>
        /// 根据指定的Key，将值加1(仅整型有效)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long IncrementValue(string key)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                return redis.IncrementValue(key);
            }
        }
        /// <summary>
        /// 根据指定的Key，将值加上指定值(仅整型有效)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static long IncrementValueBy(string key,int count)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                return redis.IncrementValueBy(key,count);
            }
        }
        /// <summary>
        /// 设置过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static bool ExpireEntryIn(string key, TimeSpan time)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                return redis.ExpireEntryIn(key, time);
            }
        }
        /// <summary>
        /// 设置过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expireAt"></param>
        /// <returns></returns>
        public static bool ExpireEntryAt(string key, DateTime expireAt)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                return redis.ExpireEntryAt(key, expireAt);
            }
        }
        /// <summary>
        /// 判断key是否已存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool ContainsKey(string key)
        {
            using (IRedisClient redis = prcm.GetReadOnlyClient())
            {
                return redis.ContainsKey(key);
            }
        }
#endregion

        #region -- List --

        public static void List_Add<T>(string key, T t)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                var redisTypedClient = redis.As<T>();
                redisTypedClient.AddItemToList(redisTypedClient.Lists[key], t);
            }
        }



        public static bool List_Remove<T>(string key, T t)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                var redisTypedClient = redis.As<T>();
                return redisTypedClient.RemoveItemFromList(redisTypedClient.Lists[key], t) > 0;
            }
        }
        public static void List_RemoveAll<T>(string key)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                var redisTypedClient = redis.As<T>();
                redisTypedClient.Lists[key].RemoveAll();
            }
        }

        public static long List_Count(string key)
        {
            using (IRedisClient redis = prcm.GetReadOnlyClient())
            {
                return redis.GetListCount(key);
            }
        }

        public static List<T> List_GetRange<T>(string key, int start, int count)
        {
            using (IRedisClient redis = prcm.GetReadOnlyClient())
            {
                var c = redis.As<T>();
                return c.Lists[key].GetRange(start, start + count - 1);
            }
        }


        public static List<T> List_GetList<T>(string key)
        {
            using (IRedisClient redis = prcm.GetReadOnlyClient())
            {
                var c = redis.As<T>();
                return c.Lists[key].GetRange(0, c.Lists[key].Count);
            }
        }

        public static List<T> List_GetList<T>(string key, int pageIndex, int pageSize)
        {
            int start = pageSize * (pageIndex - 1);
            return List_GetRange<T>(key, start, pageSize);
        }

        /// <summary> 
        /// 设置缓存过期 
        /// </summary> 
        /// <param name="key"></param> 
        /// <param name="datetime"></param> 
        public static void List_SetExpire(string key, DateTime datetime)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                redis.ExpireEntryAt(key, datetime);
            }
        }
#endregion

#region -- Set --
        public static void Set_Add<T>(string key, T t)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                var redisTypedClient = redis.As<T>();
                redisTypedClient.Sets[key].Add(t);
            }
        }
        public static bool Set_Contains<T>(string key, T t)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                var redisTypedClient = redis.As<T>();
                return redisTypedClient.Sets[key].Contains(t);
            }
        }
        public static bool Set_Remove<T>(string key, T t)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                var redisTypedClient = redis.As<T>();
                return redisTypedClient.Sets[key].Remove(t);
            }
        }
#endregion

#region -- Hash --
        /// <summary> 
        /// 判断某个数据是否已经被缓存 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <param name="dataKey"></param> 
        /// <returns></returns> 
        public static bool Hash_Exist<T>(string key, string dataKey)
        {
            using (IRedisClient redis = prcm.GetReadOnlyClient())
            {
                return redis.HashContainsEntry(key, dataKey);
            }
        }

        /// <summary> 
        /// 存储数据到hash表 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <param name="dataKey"></param> 
        /// <returns></returns> 
        public static bool Hash_Set<T>(string key, string dataKey, T t)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                string value = ServiceStack.Text.JsonSerializer.SerializeToString<T>(t);
                return redis.SetEntryInHash(key, dataKey, value);
            }
        }
        /// <summary> 
        /// 移除hash中的某值 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <param name="dataKey"></param> 
        /// <returns></returns> 
        public static bool Hash_Remove(string key, string dataKey)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                return redis.RemoveEntryFromHash(key, dataKey);
            }
        }
        /// <summary> 
        /// 移除整个hash 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <param name="dataKey"></param> 
        /// <returns></returns> 
        public static bool Hash_Remove(string key)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                return redis.Remove(key);
            }
        }
        /// <summary> 
        /// 从hash表获取数据 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <param name="dataKey"></param> 
        /// <returns></returns> 
        public static T Hash_Get<T>(string key, string dataKey)
        {
            using (IRedisClient redis = prcm.GetReadOnlyClient())
            {
                string value = redis.GetValueFromHash(key, dataKey);
                return ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(value);
            }
        }
        /// <summary> 
        /// 获取整个hash的数据 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <returns></returns> 
        public static List<T> Hash_GetAll<T>(string key)
        {
            using (IRedisClient redis = prcm.GetReadOnlyClient())
            {
                var list = redis.GetHashValues(key);
                if (list != null && list.Count > 0)
                {
                    List<T> result = new List<T>();
                    foreach (var item in list)
                    {
                        var value = ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(item);
                        result.Add(value);
                    }
                    return result;
                }
                return null;
            }
        }
        /// <summary> 
        /// 设置缓存过期 
        /// </summary> 
        /// <param name="key"></param> 
        /// <param name="datetime"></param> 
        public static void Hash_SetExpire(string key, DateTime datetime)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                redis.ExpireEntryAt(key, datetime);
            }
        }
#endregion

#region -- SortedSet --
        /// <summary> 
        ///  添加数据到 SortedSet 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <param name="t"></param> 
        /// <param name="score"></param> 
        public static bool SortedSet_Add<T>(string key, T t, double score)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                string value = ServiceStack.Text.JsonSerializer.SerializeToString<T>(t);
                return redis.AddItemToSortedSet(key, value, score);
            }
        }
        /// <summary>
        /// 为有序集 key 的成员 member 的 score 值加上增量 increment 
        /// 可以通过传递一个负数值 increment ，让 score 减去相应的值
        /// 当 key 不存在，或 member 不是 key 的成员时， ZINCRBY key increment member 等同于 ZADD key increment member
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <param name="incrementBy"></param>
        /// <returns></returns>
        public static double SortedSet_Zincrby<T>(string key,T t,double incrementBy)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                string value = ServiceStack.Text.JsonSerializer.SerializeToString<T>(t);
                return redis.IncrementItemInSortedSet(key, value, incrementBy);
            }
        }
        /// <summary>
        /// 返回有序集 key 中，成员 member 的 score 值。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t">member</param>
        /// <returns></returns>
        public static double SortedSet_ZSCORE<T>(string key,T t)
        {
            using (IRedisClient redis = prcm.GetReadOnlyClient())
            {
                string value = ServiceStack.Text.JsonSerializer.SerializeToString<T>(t);
                return redis.GetItemScoreInSortedSet(key, value);
            }
        }
        /// <summary> 
        /// 移除数据从SortedSet 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <param name="t"></param> 
        /// <returns></returns> 
        public static bool SortedSet_Remove<T>(string key, T t)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                string value = ServiceStack.Text.JsonSerializer.SerializeToString<T>(t);
                return redis.RemoveItemFromSortedSet(key, value);
            }
        }
        /// <summary> 
        /// 修剪SortedSet 
        /// </summary> 
        /// <param name="key"></param> 
        /// <param name="size">保留的条数</param> 
        /// <returns></returns> 
        public static long SortedSet_Trim(string key, int size)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                return redis.RemoveRangeFromSortedSet(key, size, 9999999);
            }
        }
        /// <summary> 
        /// 获取SortedSet的长度 
        /// </summary> 
        /// <param name="key"></param> 
        /// <returns></returns> 
        public static long SortedSet_Count(string key)
        {
            using (IRedisClient redis = prcm.GetReadOnlyClient())
            {
                return redis.GetSortedSetCount(key);
            }
        }

        /// <summary> 
        /// 按照由小到大排序获取SortedSet的分页数据 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <param name="pageIndex"></param> 
        /// <param name="pageSize"></param> 
        /// <returns></returns> 
        public static List<T> SortedSet_GetList<T>(string key, int pageIndex, int pageSize)
        {
            using (IRedisClient redis = prcm.GetReadOnlyClient())
            {
                var list = redis.GetRangeFromSortedSet(key, (pageIndex - 1) * pageSize, pageIndex * pageSize - 1);
                if (list != null && list.Count > 0)
                {
                    List<T> result = new List<T>();
                    foreach (var item in list)
                    {
                        var data = ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(item);
                        result.Add(data);
                    }
                    return result;
                }
            }
            return null;
        }
        /// <summary>
        /// 按照由大到小顺序获取SortedSet的分页数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static List<T> SortedSet_GetListDesc<T>(string key, int pageIndex, int pageSize)
        {
            using (IRedisClient redis = prcm.GetReadOnlyClient())
            {
                var list = redis.GetRangeFromSortedSetDesc(key, (pageIndex - 1) * pageSize, pageIndex * pageSize - 1);
                if (list != null && list.Count > 0)
                {
                    List<T> result = new List<T>();
                    foreach (var item in list)
                    {
                        var data = ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(item);
                        result.Add(data);
                    }
                    return result;
                }
            }
            return null;
        }
        /// <summary>
        /// 按照由大到小的顺序获取SortedSet包含分数的分页数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static IDictionary<T,double> SortedSet_GetListWidthScoreDesc<T>(string key, int pageIndex, int pageSize)
        {
            using (IRedisClient redis = prcm.GetReadOnlyClient())
            {
                var list = redis.GetRangeWithScoresFromSortedSetDesc(key, (pageIndex - 1) * pageSize, pageIndex * pageSize - 1);
                if (list != null && list.Count > 0)
                {
                    IDictionary<T,double> result = new Dictionary<T,double>();
                    foreach (var item in list)
                    {
                        var data = ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(item.Key);
                        result[data]=item.Value;
                    }
                    return result;
                }
            }
            return null;
        }

        /// <summary> 
        /// 获取SortedSet的全部数据 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <param name="pageIndex"></param> 
        /// <param name="pageSize"></param> 
        /// <returns></returns> 
        public static List<T> SortedSet_GetListALL<T>(string key)
        {
            using (IRedisClient redis = prcm.GetReadOnlyClient())
            {
                var list = redis.GetRangeFromSortedSet(key, 0, 9999999);
                if (list != null && list.Count > 0)
                {
                    List<T> result = new List<T>();
                    foreach (var item in list)
                    {
                        var data = ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(item);
                        result.Add(data);
                    }
                    return result;
                }
            }
            return null;
        }

        /// <summary> 
        /// 设置缓存过期 
        /// </summary> 
        /// <param name="key"></param> 
        /// <param name="datetime"></param> 
        public static void SortedSet_SetExpire(string key, DateTime datetime)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                redis.ExpireEntryAt(key, datetime);
            }
        }
#endregion
    }
}
