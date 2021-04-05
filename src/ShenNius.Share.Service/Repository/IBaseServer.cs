using ShenNius.Share.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ShenNius.Share.Domain.Repository
{
    public interface IBaseServer<T> where T:class, new()
    {
        #region 同步版本
        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <param name="param">实体</param>
        /// <returns></returns>
        int Add(T param);

        /// <summary>
        /// 批量添加数据
        /// </summary>
        /// <param name="param">List<T></param>
        /// <returns></returns>
        int AddList(List<T> param);

        /// <summary>
        /// 获得列表
        /// </summary>
        /// <param name="whereExpression">查询条件</param>
        /// <param name="orderExpression">排序条件</param>
        /// <param name="isAsc">排序</param>
        /// <returns></returns>
        List<T> GetList(Expression<Func<T, bool>> whereExpression,
            Expression<Func<T, object>> orderExpression, bool isAsc);

        /// <summary>
        /// 获得列表
        /// </summary>
        /// <param name="whereExpression">查询条件</param>
        /// <param name="orderExpression">排序条件</param>
        /// <param name="isAsc">排序</param>
        /// <param name="take">多少条</param>
        /// <returns></returns>
        List<T> GetList(Expression<Func<T, bool>> whereExpression,
           Expression<Func<T, object>> orderExpression, bool isAsc, int take);

        /// <summary>
        /// 获得列表
        /// </summary>
        /// <param name="whereExpression">查询条件</param>
        /// <returns></returns>
        List<T> GetList(Expression<Func<T, bool>> whereExpression);

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
        /// <param name="page">当前页</param>
        /// <param name="limit">一页多少条</param>
        /// <param name="whereExpression">查询条件</param>
        /// <param name="orderExpression">排序条件</param>
        /// <param name="isAsc">是否升序</param>
        /// <returns></returns>
        Page<T> GetPages(int page,int limit, Expression<Func<T, bool>> whereExpression,
            Expression<Func<T, object>> orderExpression, bool isAsc);

        /// <summary>
        /// 获得一条数据
        /// </summary>
        /// <param name="param">string</param>
        /// <returns></returns>
        T GetModel(string param);

        /// <summary>
        /// 获得一条数据
        /// </summary>
        /// <param name="whereExpression">查询条件</param>
        /// <returns></returns>
        T GetModel(Expression<Func<T, bool>> whereExpression);

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
        /// 修改一条数据
        /// </summary>
        /// <param name="columnsExpression">要修改的字段</param>
        /// <param name="whereExpression">条件判断</param>
        /// <returns></returns>
        int Update(Expression<Func<T, T>> columnsExpression,
            Expression<Func<T, bool>> whereExpression);

        /// <summary>
        /// 修改一条数据
        /// </summary>
        /// <param name="columnsExpression">要修改的字段</param>
        /// <param name="whereExpression">条件判断</param>
        /// <returns></returns>
        int Update(Expression<Func<T, bool>> columnsExpression,
            Expression<Func<T, bool>> whereExpression);
        /// <summary>
        /// 更新整体，指定忽略个别字段
        /// </summary>
        /// <param name="param">实体</param>
        /// <param name="ignoreExpression">指定忽略个别字段</param>
        /// <returns></returns>
        int Update(T param, Expression<Func<T, object>> ignoreExpression);

        /// <summary>
        /// 删除一条或多条数据
        /// </summary>
        /// <param name="param">string</param>
        /// <returns></returns>
        int Delete(List<string> param);

        /// <summary>
        /// 删除一条或多条数据
        /// </summary>
        /// <param name="whereExpression">删除条件</param>
        /// <returns></returns>
        int Delete(Expression<Func<T, bool>> whereExpression);

        /// <summary>
        /// 查询条数
        /// </summary>
        /// <param name="whereExpression">查询条件</param>
        /// <returns></returns>
        int Count(Expression<Func<T, bool>> whereExpression);
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="whereExpression">查询条件</param>
        /// <returns></returns>
        bool IsExist(Expression<Func<T, bool>> whereExpression);

        #endregion

        #region 异步版本

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

        /// <summary>
        /// 获得列表
        /// </summary>
        /// <param name="whereExpression">查询条件</param>
        /// <param name="orderExpression">排序字段</param>
        /// <param name="isAsc">是否升序</param>
        /// <returns></returns>
        Task<List<T>> GetListAsync(Expression<Func<T, bool>> whereExpression,
            Expression<Func<T, object>> orderExpression,  bool isAsc);

        /// <summary>
        /// 获得列表
        /// </summary>
        /// <param name="whereExpression">查询条件</param>
        /// <param name="orderExpression">排序字段</param>
        /// <param name="isAsc">是否升序</param>
        /// <param name="take">多少条</param>
        /// <returns></returns>
        Task<List<T>> GetListAsync(Expression<Func<T, bool>> whereExpression,
           Expression<Func<T, object>> orderExpression, bool isAsc, int take);

        /// <summary>
        /// 获得列表
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        Task<List<T>> GetListAsync(Expression<Func<T, bool>> whereExpression);

     

        /// <summary>
        /// 获得列表
        /// </summary>
        /// <returns></returns>
        Task<List<T>> GetListAsync();

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="page">当前页</param>
        /// <param name="limit">一页多少条</param>
        /// <returns></returns>
		Task<Page<T>> GetPagesAsync(int page,int limit);

        Task<Page<T>> GetPagesAsync(int page, int limit, Expression<Func<T, object>> orderExpression, bool isAsc);
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="page">当前页</param>
        /// <param name="limit">一页多少条</param>
        /// <param name="whereExpression">查询条件</param>
        /// <param name="orderExpression">排序条件</param>
        /// <param name="isAsc">是否升序</param>
        /// <returns></returns>
        Task<Page<T>> GetPagesAsync(int page,int limit, Expression<Func<T, bool>> whereExpression,
            Expression<Func<T, object>> orderExpression, bool isAsc);

        /// <summary>
        /// 获得一条数据
        /// </summary>
        /// <param name="param">string</param>
        /// <returns></returns>
        Task<T> GetModelAsync(string param);

        /// <summary>
        /// 获得一条数据
        /// </summary>
        /// <param name="whereExpression">查询条件</param>
        /// <returns></returns>
        Task<T> GetModelAsync(Expression<Func<T, bool>> whereExpression);

        /// <summary>
        /// 修改一条数据
        /// </summary>
        /// <param name="param">实体</param>
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
        /// <param name="columnsExpression">修改的列</param>
        /// <param name="whereExpression">判断条件</param>
        /// <returns></returns>
        Task<int> UpdateAsync(Expression<Func<T, T>> columnsExpression,
            Expression<Func<T, bool>> whereExpression);

        /// <summary>
        /// 修改一条数据，可用作假删除
        /// </summary>
        /// <param name="columnsExpression">修改的列</param>
        /// <param name="whereExpression">判断条件</param>
        /// <returns></returns>
        Task<int> UpdateAsync(Expression<Func<T, bool>> columnsExpression,
            Expression<Func<T, bool>> whereExpression);
        /// <summary>
        /// 更新整体，指定忽略个别字段
        /// </summary>
        /// <param name="param">实体</param>
        /// <param name="ignoreExpression">指定忽略个别字段</param>
        /// <returns></returns>
        Task<int> UpdateAsync(T param, Expression<Func<T, object>> ignoreExpression);
        /// <summary>
        /// 删除一条或多条数据
        /// </summary>
        /// <param name="param">string</param>
        /// <returns></returns>
        Task<int> DeleteAsync(List<int> param);

        /// <summary>
        /// 删除一条或多条数据
        /// </summary>
        /// <param name="whereExpression">Expression<Func<T, bool>></param>
        /// <returns></returns>
        Task<int> DeleteAsync(Expression<Func<T, bool>> whereExpression);

        /// <summary>
        /// 查询Count
        /// </summary>
        /// <param name="whereExpression">查询条件</param>
        /// <returns></returns>
        Task<int> CountAsync(Expression<Func<T, bool>> whereExpression);


        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="whereExpression">查询条件</param>
        /// <returns></returns>
        Task<bool> IsExistAsync(Expression<Func<T, bool>> whereExpression);

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
        public Task<List<TResult>> QueryMuch<T1, T2, T3, TResult>(
          Expression<Func<T1, T2, T3, object[]>> joinExpression,
          Expression<Func<T1, T2, T3, TResult>> selectExpression,
          Expression<Func<T1, T2, T3, bool>> whereExpression = null) where T1 : class, new();

        #endregion
    }
}
