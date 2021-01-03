using ShenNius.Share.Service.DbBusinessModel;
using ShenNius.Share.Service.Enum;
using ShenNius.Share.Service.Extensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ShenNius.Share.Service.Repository
{
    public class BaseServer<T> : DbContext, IBaseServer<T> where T : class, new()
    {
        #region 同步版本

        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <param name="param">T</param>
        /// <returns></returns>
        public int Add(T param)
        {
            return Db.Insertable<T>(param).ExecuteReturnIdentity();
        }

        /// <summary>
        /// 批量添加数据
        /// </summary>
        /// <param name="param">List<T></param>
        /// <returns></returns>
        public int AddList(List<T> param)
        {
            return Db.Insertable<T>(param).ExecuteCommand();
        }



        /// <summary>
        /// 获得一条数据
        /// </summary>
        /// <param name="where">Expression<Func<T, bool>></param>
        /// <returns></returns>
        public T GetModel(Expression<Func<T, bool>> where)
        {
            return Db.Queryable<T>().Where(where).First() ?? new T() { };
        }

        /// <summary>
        /// 获得一条数据
        /// </summary>
        /// <param name="param">string</param>
        /// <returns></returns>
        public T GetModel(string param)
        {
            return Db.Queryable<T>().Where(param).First() ?? new T() { };
        }

        /// <summary>
		/// 获得列表——分页
		/// </summary>
		/// <param name="param">Pageparam</param>
		/// <returns></returns>
        public Page<T> GetPages(int page,int limit)
        {
            return Db.Queryable<T>().ToPage(page, limit);
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="param">分页参数</param>
        /// <param name="where">条件</param>
        /// <param name="order">排序值</param>
        /// <param name="orderEnum">排序方式OrderByType</param>
        /// <returns></returns>
        public Page<T> GetPages(int page,int limit, Expression<Func<T, bool>> where,
            Expression<Func<T, object>> order, DbOrder orderEnum)
        {
            var query = Db.Queryable<T>()
                    .Where(where)
                    .OrderByIF((int)orderEnum == 1, order, SqlSugar.OrderByType.Asc)
                    .OrderByIF((int)orderEnum == 2, order, SqlSugar.OrderByType.Desc);
            return query.ToPage(page, limit);
        }

        /// <summary>
		/// 获得列表
		/// </summary>
		/// <param name="param">Pageparam</param>
		/// <returns></returns>
        public List<T> GetList(Expression<Func<T, bool>> where,
            Expression<Func<T, object>> order, DbOrder orderEnum)
        {
            var query = Db.Queryable<T>()
                    .Where(where)
                    .OrderByIF((int)orderEnum == 1, order, SqlSugar.OrderByType.Asc)
                    .OrderByIF((int)orderEnum == 2, order, SqlSugar.OrderByType.Desc);
            return query.ToList();
        }
        public List<T> GetList(Expression<Func<T, bool>> where,
           Expression<Func<T, object>> order, DbOrder orderEnum, int take)
        {

            var query = Db.Queryable<T>()
                    .Where(where).Take(take)
                    .OrderByIF((int)orderEnum == 1, order, SqlSugar.OrderByType.Asc)
                    .OrderByIF((int)orderEnum == 2, order, SqlSugar.OrderByType.Desc);
            return query.ToList();
        }
        /// <summary>
		/// 获得列表
		/// </summary>
		/// <param name="param">Pageparam</param>
		/// <returns></returns>
        public List<T> GetList(Expression<Func<T, bool>> where)
        {
            return Db.Queryable<T>().Where(where).ToList();
        }

        /// <summary>
        /// 获得列表，不需要任何条件
        /// </summary>
        /// <returns></returns>
        public List<T> GetList()
        {
            return Db.Queryable<T>().ToList();
        }

        /// <summary>
        /// 修改一条数据
        /// </summary>
        /// <param name="param">T</param>
        /// <returns></returns>
        public int Update(T param)
        {
            return Db.Updateable(param).ExecuteCommand();
        }

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="param">T</param>
        /// <returns></returns>
        public int Update(List<T> param)
        {
            return Db.Updateable(param).ExecuteCommand();
        }

        /// <summary>
        /// 修改一条数据，可用作假删除
        /// </summary>
        /// <param name="columns">修改的列=Expression<Func<T,T>></param>
        /// <param name="where">Expression<Func<T,bool>></param>
        /// <returns></returns>
        public int Update(Expression<Func<T, T>> columns,
            Expression<Func<T, bool>> where)
        {
            return Db.Updateable<T>().SetColumns(columns).Where(where).ExecuteCommand();
        }

        /// <summary>
        /// 修改一条数据，可用作假删除
        /// </summary>
        /// <param name="columns">修改的列=Expression<Func<T,T>></param>
        /// <param name="where">Expression<Func<T,bool>></param>
        /// <returns></returns>
        public int Update(Expression<Func<T, bool>> columns,
            Expression<Func<T, bool>> where)
        {
            return Db.Updateable<T>().SetColumns(columns).Where(where).ExecuteCommand();
        }

        /// <summary>
        /// 删除一条或多条数据
        /// </summary>
        /// <param name="param">string</param>
        /// <returns></returns>
        public int Delete(List<string> param)
        {
            return Db.Deleteable<T>().In(param.ToArray()).ExecuteCommand();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="where">Expression<Func<T, bool>></param>
        /// <returns></returns>
        public int Delete(Expression<Func<T, bool>> where)
        {
            return Db.Deleteable<T>().Where(where).ExecuteCommand();
        }


        /// <summary>
        /// 查询条数
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Count(Expression<Func<T, bool>> where)
        {
            return Db.Queryable<T>().Count(where);
        }


        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public bool IsExist(Expression<Func<T, bool>> where)
        {

            return Db.Queryable<T>().Any(where);

        }
        #endregion

        #region 异步版本
        #region 添加操作
        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <param name="param">T</param>
        /// <returns></returns>
        public async Task<int> AddAsync(T param)
        {
            return await Db.Insertable<T>(param).ExecuteReturnIdentityAsync();
        }

        /// <summary>
        /// 批量添加数据
        /// </summary>
        /// <param name="param">List<T></param>
        /// <returns></returns>
        public async Task<int> AddListAsync(List<T> param)
        {
            return await Db.Insertable<T>(param).ExecuteCommandAsync();
        }
        #endregion

        #region 查询操作
        /// <summary>
        /// 获得一条数据
        /// </summary>
        /// <param name="where">Expression<Func<T, bool>></param>
        /// <returns></returns>
        public async Task<T> GetModelAsync(Expression<Func<T, bool>> where)
        {
            return await Db.Queryable<T>().Where(where).FirstAsync() ?? new T() { };
        }

        /// <summary>
        /// 获得一条数据
        /// </summary>
        /// <param name="param">string</param>
        /// <returns></returns>
        public async Task<T> GetModelAsync(string param)
        {
            return await Db.Queryable<T>().Where(param).FirstAsync() ?? new T() { };
        }

        /// <summary>
		/// 获得列表——分页
		/// </summary>
		/// <param name="param">Pageparam</param>
		/// <returns></returns>
        public async Task<Page<T>> GetPagesAsync(int page,int limit)
        {
            return await Db.Queryable<T>().ToPageAsync(page, limit);
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="param">分页参数</param>
        /// <param name="where">条件</param>
        /// <param name="order">排序值</param>
        /// <param name="orderEnum">排序方式OrderByType</param>
        /// <returns></returns>
        public async Task<Page<T>> GetPagesAsync(int page,int limit, Expression<Func<T, bool>> where,
            Expression<Func<T, object>> order, DbOrder orderEnum)
        {
            var query = Db.Queryable<T>()
                    .Where(where)
                    .OrderByIF((int)orderEnum == 1, order, SqlSugar.OrderByType.Asc)
                    .OrderByIF((int)orderEnum == 2, order, SqlSugar.OrderByType.Desc);
            return await query.ToPageAsync(page, limit);
        }

        /// <summary>
		/// 获得列表
		/// </summary>
		/// <param name="param">Pageparam</param>
		/// <returns></returns>
        public async Task<List<T>> GetListAsync(Expression<Func<T, bool>> where,
            Expression<Func<T, object>> order, DbOrder orderEnum)
        {
            var query = Db.Queryable<T>()
                    .Where(where)
                    .OrderByIF((int)orderEnum == 1, order, SqlSugar.OrderByType.Asc)
                    .OrderByIF((int)orderEnum == 2, order, SqlSugar.OrderByType.Desc);
            return await query.ToListAsync();
        }
        public async Task<List<T>> GetListAsync(Expression<Func<T, bool>> where,
           Expression<Func<T, object>> order, DbOrder orderEnum, int take)
        {

            var query = Db.Queryable<T>()
                    .Where(where).Take(take)
                    .OrderByIF((int)orderEnum == 1, order, SqlSugar.OrderByType.Asc)
                    .OrderByIF((int)orderEnum == 2, order, SqlSugar.OrderByType.Desc);
            return await query.ToListAsync();
        }
        /// <summary>
		/// 获得列表
		/// </summary>
		/// <param name="param">Pageparam</param>
		/// <returns></returns>
        public async Task<List<T>> GetListAsync(Expression<Func<T, bool>> where)
        {
            return await Db.Queryable<T>().Where(where).ToListAsync();
        }

        /// <summary>
        /// 获得列表，不需要任何条件
        /// </summary>
        /// <returns></returns>
        public async Task<List<T>> GetListAsync()
        {
            return await Db.Queryable<T>().ToListAsync();
        }
        #endregion

        #region 修改操作
        /// <summary>
        /// 修改一条数据
        /// </summary>
        /// <param name="param">T</param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(T param)
        {
            return await Db.Updateable(param).ExecuteCommandAsync();
        }

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="param">T</param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(List<T> param)
        {
            return await Db.Updateable(param).ExecuteCommandAsync();
        }

        /// <summary>
        /// 修改一条数据，可用作假删除
        /// </summary>
        /// <param name="columns">修改的列=Expression<Func<T,T>></param>
        /// <param name="where">Expression<Func<T,bool>></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(Expression<Func<T, T>> columns,
            Expression<Func<T, bool>> where)
        {
            return await Db.Updateable<T>().SetColumns(columns).Where(where).ExecuteCommandAsync();
        }

        /// <summary>
        /// 修改一条数据，可用作假删除
        /// </summary>
        /// <param name="columns">修改的列=Expression<Func<T,T>></param>
        /// <param name="where">Expression<Func<T,bool>></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(Expression<Func<T, bool>> columns,
            Expression<Func<T, bool>> where)
        {
            return await Db.Updateable<T>().SetColumns(columns).Where(where).ExecuteCommandAsync();
        }
        #endregion

        #region 删除操作
        /// <summary>
        /// 删除一条或多条数据
        /// </summary>
        /// <param name="param">string</param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(List<string> param)
        {
            return await Db.Deleteable<T>().In(param.ToArray()).ExecuteCommandAsync();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="where">Expression<Func<T, bool>></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(Expression<Func<T, bool>> where)
        {
            return await Db.Deleteable<T>().Where(where).ExecuteCommandAsync();
        }
        #endregion

        #region 查询Count
        public async Task<int> CountAsync(Expression<Func<T, bool>> where)
        {
            return await Db.Queryable<T>().CountAsync(where);
        }
        #endregion


        public async Task<bool> IsExistAsync(Expression<Func<T, bool>> where)
        {

            return await Db.Queryable<T>().AnyAsync(where);

        }

        #endregion
    }
}
