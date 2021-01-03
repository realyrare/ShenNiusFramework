using ShenNius.Share.Service.DbBusinessModel;
using ShenNius.Share.Service.Enum;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ShenNius.Share.Service.Repository
{
    public interface IBaseServer<T> where T:class
    {
        #region 同步版本
        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <param name="param">cms_advlist</param>
        /// <returns></returns>
        int Add(T param);

        /// <summary>
        /// 批量添加数据
        /// </summary>
        /// <param name="param">List<T></param>
        /// <returns></returns>
        int AddList(List<T> param);



        #region 查询操作
        /// <summary>
        /// 获得列表
        /// </summary>
        /// <param name="where">Expression<Func<T, bool>></param>
        /// <param name="order">Expression<Func<T, object>></param>
        /// <param name="orderEnum">DbOrderEnum</param>
        /// <returns></returns>
        List<T> GetList(Expression<Func<T, bool>> where,
            Expression<Func<T, object>> order, DbOrder orderEnum);


        List<T> GetList(Expression<Func<T, bool>> where,
           Expression<Func<T, object>> order, DbOrder orderEnum, int take);

        /// <summary>
        /// 获得列表
        /// </summary>
        /// <param name="where">Expression<Func<T, bool>></param>
        /// <returns></returns>
        List<T> GetList(Expression<Func<T, bool>> where);

        /// <summary>
        /// 获得列表
        /// </summary>
        /// <returns></returns>
        List<T> GetList();

        /// <summary>
		/// 获得列表——分页
		/// </summary>
		/// <param name="param">Pageparam</param>
		/// <returns></returns>
		Page<T> GetPages(int page,int limit);

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="param">分页参数</param>
        /// <param name="where">条件</param>
        /// <param name="order">排序值</param>
        /// <param name="orderEnum">排序方式OrderByType</param>
        /// <returns></returns>
        Page<T> GetPages(int page,int limit, Expression<Func<T, bool>> where,
            Expression<Func<T, object>> order, DbOrder orderEnum);

        /// <summary>
        /// 获得一条数据
        /// </summary>
        /// <param name="param">string</param>
        /// <returns></returns>
        T GetModel(string param);

        /// <summary>
        /// 获得一条数据
        /// </summary>
        /// <param name="where">Expression<Func<T, bool>></param>
        /// <returns></returns>
        T GetModel(Expression<Func<T, bool>> where);
        #endregion

        #region 修改操作
        /// <summary>
        /// 修改一条数据
        /// </summary>
        /// <param name="param">T</param>
        /// <returns></returns>
        int Update(T param);

        /// <summary>
        /// 修改一条数据
        /// </summary>
        /// <param name="param">T</param>
        /// <returns></returns>
        int Update(List<T> param);

        /// <summary>
        /// 修改一条数据，可用作假删除
        /// </summary>
        /// <param name="columns">修改的列=Expression<Func<T,T>></param>
        /// <param name="where">Expression<Func<T,bool>></param>
        /// <returns></returns>
        int Update(Expression<Func<T, T>> columns,
            Expression<Func<T, bool>> where);

        /// <summary>
        /// 修改一条数据，可用作假删除
        /// </summary>
        /// <param name="columns">修改的列=Expression<Func<T,T>></param>
        /// <param name="where">Expression<Func<T,bool>></param>
        /// <returns></returns>
        int Update(Expression<Func<T, bool>> columns,
            Expression<Func<T, bool>> where);
        #endregion

        #region 删除操作
        /// <summary>
        /// 删除一条或多条数据
        /// </summary>
        /// <param name="param">string</param>
        /// <returns></returns>
        int Delete(List<string> param);

        /// <summary>
        /// 删除一条或多条数据
        /// </summary>
        /// <param name="where">Expression<Func<T, bool>></param>
        /// <returns></returns>
        int Delete(Expression<Func<T, bool>> where);

        #endregion

        /// <summary>
        /// 查询条数
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        int Count(Expression<Func<T, bool>> where);


        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        bool IsExist(Expression<Func<T, bool>> where);

        #endregion

        #region 异步版本
        #region 添加操作
        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <param name="param">cms_advlist</param>
        /// <returns></returns>
        Task<int> AddAsync(T param);

        /// <summary>
        /// 批量添加数据
        /// </summary>
        /// <param name="param">List<T></param>
        /// <returns></returns>
        Task<int> AddListAsync(List<T> param);

        #endregion

        #region 查询操作
        /// <summary>
        /// 获得列表
        /// </summary>
        /// <param name="where">Expression<Func<T, bool>></param>
        /// <param name="order">Expression<Func<T, object>></param>
        /// <param name="orderEnum">DbOrderEnum</param>
        /// <returns></returns>
        Task<List<T>> GetListAsync(Expression<Func<T, bool>> where,
            Expression<Func<T, object>> order, DbOrder orderEnum);


        Task<List<T>> GetListAsync(Expression<Func<T, bool>> where,
           Expression<Func<T, object>> order, DbOrder orderEnum, int take);

        /// <summary>
        /// 获得列表
        /// </summary>
        /// <param name="where">Expression<Func<T, bool>></param>
        /// <returns></returns>
        Task<List<T>> GetListAsync(Expression<Func<T, bool>> where);

        /// <summary>
        /// 获得列表
        /// </summary>
        /// <returns></returns>
        Task<List<T>> GetListAsync();

        /// <summary>
		/// 获得列表——分页
		/// </summary>
		/// <param name="param">Pageparam</param>
		/// <returns></returns>
		Task<Page<T>> GetPagesAsync(int page,int limit);

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="param">分页参数</param>
        /// <param name="where">条件</param>
        /// <param name="order">排序值</param>
        /// <param name="orderEnum">排序方式OrderByType</param>
        /// <returns></returns>
        Task<Page<T>> GetPagesAsync(int page,int limit, Expression<Func<T, bool>> where,
            Expression<Func<T, object>> order, DbOrder orderEnum);

        /// <summary>
        /// 获得一条数据
        /// </summary>
        /// <param name="param">string</param>
        /// <returns></returns>
        Task<T> GetModelAsync(string param);

        /// <summary>
        /// 获得一条数据
        /// </summary>
        /// <param name="where">Expression<Func<T, bool>></param>
        /// <returns></returns>
        Task<T> GetModelAsync(Expression<Func<T, bool>> where);
        #endregion

        #region 修改操作
        /// <summary>
        /// 修改一条数据
        /// </summary>
        /// <param name="param">T</param>
        /// <returns></returns>
        Task<int> UpdateAsync(T param);

        /// <summary>
        /// 修改一条数据
        /// </summary>
        /// <param name="param">T</param>
        /// <returns></returns>
        Task<int> UpdateAsync(List<T> param);

        /// <summary>
        /// 修改一条数据，可用作假删除
        /// </summary>
        /// <param name="columns">修改的列=Expression<Func<T,T>></param>
        /// <param name="where">Expression<Func<T,bool>></param>
        /// <returns></returns>
        Task<int> UpdateAsync(Expression<Func<T, T>> columns,
            Expression<Func<T, bool>> where);

        /// <summary>
        /// 修改一条数据，可用作假删除
        /// </summary>
        /// <param name="columns">修改的列=Expression<Func<T,T>></param>
        /// <param name="where">Expression<Func<T,bool>></param>
        /// <returns></returns>
        Task<int> UpdateAsync(Expression<Func<T, bool>> columns,
            Expression<Func<T, bool>> where);
        #endregion

        #region 删除操作
        /// <summary>
        /// 删除一条或多条数据
        /// </summary>
        /// <param name="param">string</param>
        /// <returns></returns>
        Task<int> DeleteAsync(List<string> param);

        /// <summary>
        /// 删除一条或多条数据
        /// </summary>
        /// <param name="where">Expression<Func<T, bool>></param>
        /// <returns></returns>
        Task<int> DeleteAsync(Expression<Func<T, bool>> where);

        #endregion

        #region 查询Count
        Task<int> CountAsync(Expression<Func<T, bool>> where);
        #endregion

        #region 是否存在
        Task<bool> IsExistAsync(Expression<Func<T, bool>> where);
        #endregion 
        #endregion
    }
}
