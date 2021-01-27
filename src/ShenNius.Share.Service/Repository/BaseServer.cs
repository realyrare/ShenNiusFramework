using ShenNius.Share.Service.Repository.Extensions;
using SqlSugar;
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
        /// <param name="param">实体</param>
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
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public T GetModel(Expression<Func<T, bool>> whereExpression)
        {
            return Db.Queryable<T>().WhereIF(whereExpression != null, whereExpression).First() ?? new T() { };
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
        /// 分页
        /// </summary>
        /// <param name="page">多少页</param>
        /// <param name="limit">一页多少条</param>
        /// <returns></returns>
        public Page<T> GetPages(int page, int limit)
        {
            return Db.Queryable<T>().ToPage(page, limit);
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="page">当前页</param>
        /// <param name="limit">一页多少条</param>
        /// <param name="whereExpression">查询条件</param>
        /// <param name="orderExpression">排序值</param>
        /// <param name="isAsc">是否升序</param>
        /// <returns></returns>
        public Page<T> GetPages(int page, int limit, Expression<Func<T, bool>> whereExpression,
            Expression<Func<T, object>> orderExpression, bool isAsc)
        {
            var query = Db.Queryable<T>()
                    .WhereIF(whereExpression != null, whereExpression)
                    .OrderBy(orderExpression, isAsc == true ? OrderByType.Asc : OrderByType.Desc);
            return query.ToPage(page, limit);
        }

        /// <summary>
        /// 获得列表
        /// </summary>
        /// <param name="whereExpression">查询条件</param>
        /// <param name="orderExpression">排序值</param>
        /// <param name="isAsc">是否升序</param>
        /// <returns></returns>
        public List<T> GetList(Expression<Func<T, bool>> whereExpression,
            Expression<Func<T, object>> orderExpression, bool isAsc)
        {
            var query = Db.Queryable<T>()
                     .WhereIF(whereExpression != null, whereExpression)
                    .OrderBy(orderExpression, isAsc == true ? OrderByType.Asc : OrderByType.Desc);
            return query.ToList();
        }
        /// <summary>
        /// 获得列表
        /// </summary>
        /// <param name="whereExpression">查询条件</param>
        /// <param name="orderExpression">排序值</param>
        /// <param name="isAsc">是否升序</param>
        /// <param name="take">查询多少条</param>
        /// <returns></returns>
        public List<T> GetList(Expression<Func<T, bool>> whereExpression,
           Expression<Func<T, object>> orderExpression, bool isAsc, int take)
        {

            var query = Db.Queryable<T>()
                     .WhereIF(whereExpression != null, whereExpression).Take(take)
                    .OrderBy(orderExpression, isAsc == true ? OrderByType.Asc : OrderByType.Desc);
            return query.ToList();
        }
        /// <summary>
        /// 获得列表
        /// </summary>
        /// <param name="whereExpression">查询条件</param>
        /// <returns></returns>
        public List<T> GetList(Expression<Func<T, bool>> whereExpression)
        {
            return Db.Queryable<T>().WhereIF(whereExpression != null, whereExpression).ToList();
        }

        /// <summary>
        /// 获得列表
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
        /// 修改一条数据
        /// </summary>
        /// <param name="columnsExpression">修改的列</param>
        /// <param name="whereExpression">判断条件</param>
        /// <returns></returns>
        public int Update(Expression<Func<T, T>> columnsExpression,
            Expression<Func<T, bool>> whereExpression)
        {
            return Db.Updateable<T>().SetColumns(columnsExpression).Where(whereExpression).ExecuteCommand();
        }

        /// <summary>
        /// 修改一条数据
        /// </summary>
        /// <param name="columnsExpression">修改的列</param>
        /// <param name="whereExpression">判断条件</param>
        /// <returns></returns>
        public int Update(Expression<Func<T, bool>> columnsExpression,
            Expression<Func<T, bool>> whereExpression)
        {
            return Db.Updateable<T>().SetColumns(columnsExpression).Where(whereExpression).ExecuteCommand();
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
        /// <param name="whereExpression">删除条件</param>
        /// <returns></returns>
        public int Delete(Expression<Func<T, bool>> whereExpression)
        {
            return Db.Deleteable<T>().Where(whereExpression).ExecuteCommand();
        }

        /// <summary>
        /// 查询条数
        /// </summary>
        /// <param name="whereExpression">查询条件</param>
        /// <returns></returns>
        public int Count(Expression<Func<T, bool>> whereExpression)
        {
            return Db.Queryable<T>().Count(whereExpression);
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="whereExpression">查询条件</param>
        /// <returns></returns>
        public bool IsExist(Expression<Func<T, bool>> whereExpression)
        {
            return Db.Queryable<T>().Any(whereExpression);
        }
        #endregion

        #region 异步版本

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

        /// <summary>
        /// 获得一条数据
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public async Task<T> GetModelAsync(Expression<Func<T, bool>> whereExpression)
        {
            return await Db.Queryable<T>().WhereIF(whereExpression != null, whereExpression).FirstAsync() ?? new T() { };
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
        /// 分页
        /// </summary>
        /// <param name="page">当前页</param>
        /// <param name="limit">一页多少条</param>
        /// <returns></returns>
        public async Task<Page<T>> GetPagesAsync(int page, int limit)
        {
            return await Db.Queryable<T>().ToPageAsync(page, limit);
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="page">当前页</param>
        /// <param name="limit">一页多少条</param>
        /// <param name="whereExpression">查询条件</param>
        /// <param name="orderExpression">排序值</param>
        /// <param name="isAsc">是否升序</param>
        /// <returns></returns>
        public async Task<Page<T>> GetPagesAsync(int page, int limit, Expression<Func<T, bool>> whereExpression,
            Expression<Func<T, object>> orderExpression, bool isAsc)
        {
            var query = Db.Queryable<T>()
                    .WhereIF(whereExpression != null, whereExpression)
                    .OrderBy(orderExpression, isAsc == true ? OrderByType.Asc : OrderByType.Desc);
            return await query.ToPageAsync(page, limit);
        }

        /// <summary>
        /// 获得列表
        /// </summary>
        /// <param name="whereExpression">查询条件</param>
        /// <param name="orderExpression">排序值</param>
        /// <param name="isAsc">是否升序</param>
        /// <returns></returns>
        public async Task<List<T>> GetListAsync(Expression<Func<T, bool>> whereExpression,
            Expression<Func<T, object>> orderExpression, bool isAsc)
        {
            var query = Db.Queryable<T>()
                    .WhereIF(whereExpression != null, whereExpression)
                    .OrderBy(orderExpression, isAsc == true ? OrderByType.Asc : OrderByType.Desc);
            return await query.ToListAsync();
        }
        /// <summary>
        /// 获得列表
        /// </summary>
        /// <param name="whereExpression">查询条件</param>
        /// <param name="orderExpression">排序值</param>
        /// <param name="isAsc">是否升序</param>
        /// <param name="take">查询多少条</param>
        /// <returns></returns>
        public async Task<List<T>> GetListAsync(Expression<Func<T, bool>> whereExpression,
           Expression<Func<T, object>> orderExpression, bool isAsc, int take)
        {

            var query = Db.Queryable<T>()
                    .WhereIF(whereExpression != null, whereExpression).Take(take)
                    .OrderBy(orderExpression, isAsc == true ? OrderByType.Asc : OrderByType.Desc);
            return await query.ToListAsync();
        }
        /// <summary>
		/// 获得列表
		/// </summary>
		/// <param name="whereExpression">查询条件</param>
		/// <returns></returns>
        public async Task<List<T>> GetListAsync(Expression<Func<T, bool>> whereExpression)
        {
            return await Db.Queryable<T>().WhereIF(whereExpression != null, whereExpression).ToListAsync();
        }

        /// <summary>
        /// 获得列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<T>> GetListAsync()
        {
            return await Db.Queryable<T>().ToListAsync();
        }


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
        /// <param name="columnsExpression">修改的列</param>
        /// <param name="whereExpression">判断条件</param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(Expression<Func<T, T>> columnsExpression,
            Expression<Func<T, bool>> whereExpression)
        {
            return await Db.Updateable<T>().SetColumns(columnsExpression).Where(whereExpression).ExecuteCommandAsync();
        }

        /// <summary>
        /// 修改一条数据，可用作假删除
        /// </summary>
        /// <param name="columns">修改的列=Expression<Func<T,T>></param>
        /// <param name="where">Expression<Func<T,bool>></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(Expression<Func<T, bool>> columnsExpression,
            Expression<Func<T, bool>> whereExpression)
        {
            return await Db.Updateable<T>().SetColumns(columnsExpression).Where(whereExpression).ExecuteCommandAsync();
        }

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
        /// <param name="whereExpression">Expression<Func<T, bool>></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(Expression<Func<T, bool>> whereExpression)
        {
            return await Db.Deleteable<T>().Where(whereExpression).ExecuteCommandAsync();
        }
        /// <summary>
        /// 查询多少条
        /// </summary>
        /// <param name="whereExpression">查询条件</param>
        /// <returns></returns>
        public async Task<int> CountAsync(Expression<Func<T, bool>> whereExpression)
        {
            return await Db.Queryable<T>().CountAsync(whereExpression);
        }
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="whereExpression">查询条件</param>
        /// <returns></returns>
        public async Task<bool> IsExistAsync(Expression<Func<T, bool>> whereExpression)
        {
            return await Db.Queryable<T>().AnyAsync(whereExpression);
        }
        /// <summary>
        /// 多表查询
        /// </summary>
        /// <typeparam name="T1">表1</typeparam>
        /// <typeparam name="T2">表2</typeparam>
        /// <typeparam name="T3">表3</typeparam>
        /// <typeparam name="TResult">结果</typeparam>
        /// <param name="joinExpression">连接条件</param>
        /// <param name="selectExpression">投影</param>
        /// <param name="whereExpression">查询条件</param>
        /// <returns></returns>
        public async Task<List<TResult>> QueryMuch<T1, T2, T3, TResult>(
            Expression<Func<T1, T2, T3, object[]>> joinExpression,
            Expression<Func<T1, T2, T3, TResult>> selectExpression,
            Expression<Func<T1, T2, T3, bool>> whereExpression = null) where T1 : class, new()
        {
            if (whereExpression == null)
            {
                return await Db.Queryable(joinExpression).Select(selectExpression).ToListAsync();
            }
            return await Db.Queryable(joinExpression).Where(whereExpression).Select(selectExpression).ToListAsync();
        }
        #endregion
    }
}
