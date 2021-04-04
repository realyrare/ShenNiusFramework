using System;
using System.Threading.Tasks;

/*************************************
* 类名：ICacheHelper
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/3/25 17:59:55
*┌───────────────────────────────────┐　    
*│　   版权所有：一起牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Infrastructure.Cache
{
    public interface ICacheHelper
    {
        bool Exists(string key);

        void Set<T>(string key, T value);
        void Set<T>(string key, T value, TimeSpan timeSpan);

        T Get<T>(string key);

        void Remove(string key);

        T GetOrSet<T>(string key, Func<T> getDataCallback, TimeSpan? exp=null);
        Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> getDataCallback, TimeSpan? exp = null);
    }
}