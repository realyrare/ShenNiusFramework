using ShenNius.Share.Models.Dtos.Input.Cms;
using ShenNius.Share.Models.Entity.Cms;
using ShenNius.Share.Domain.Repository;
using ShenNius.Share.Domain.Repository.Extensions;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

/*************************************
* 类名：ColumnService
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/3/11 17:15:30
*┌───────────────────────────────────┐　    
*│　   版权所有：一起牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Domain.Services.Cms
{
    public interface IArticleService : IBaseServer<Article>
    {
       Task<Page<ArticleOutput>> GetArtcileByConditionAsync(Expression<Func<Article, Column, bool>> where, int pageIndex, int pageSize);
        Task<List<string>> GetAllTagsAsync(List<int> columnIds);
        Task<ArticleOutput> GetArtcileDetailAsync(Expression<Func<Article, Column, bool>> where);
        Task<ArticleOutput> GetNextOrUpArticleAsync(Expression<Func<Article, Column, bool>> expression);
    }
    public class ArticleService : BaseServer<Article>, IArticleService
    {
        public async Task<Page<ArticleOutput>> GetArtcileByConditionAsync(Expression<Func<Article, Column, bool>> where, int pageIndex, int pageSize)
        {
            return await Db.Queryable<Article, Column>((ca, cc) => new object[] { JoinType.Inner, ca.ColumnId == cc.Id })
                  .Where(where)
                  .OrderBy((ca, cc) => ca.Id, OrderByType.Desc)
                  .Select((ca, cc) => new ArticleOutput
                  {
                      Title = ca.Title,
                      Id = ca.Id,
                      ColumnId = ca.ColumnId,
                      Summary = ca.Summary,
                      EnTitle = cc.EnTitle,
                      Author = ca.Author,
                      Source = ca.Source,
                      Tag = ca.Tag,
                      CreateTime = ca.CreateTime,
                      ColumnName = cc.Title,
                      Content = ca.Content,
                      ThumImg = ca.ThumImg,
                      IsTop = ca.IsTop,
                      IsHot = ca.IsHot,
                      ParentColumnUrl = SqlFunc.Subqueryable<Column>().Where(s => s.Id == cc.ParentId).Select(s => s.EnTitle)
                  })
                  .ToPageAsync(pageIndex, pageSize);
        }

        public async Task<List<string>> GetAllTagsAsync(List<int> columnIds)
        {
            return await Db.Queryable<Article>().Where(d => d.Audit == true && columnIds.Contains(d.ColumnId)).GroupBy(d => d.Tag).Select(d => d.Tag).ToListAsync();
        }
        public async Task<ArticleOutput> GetArtcileDetailAsync(Expression<Func<Article, Column, bool>> where)
        {
            return await Db.Queryable<Article, Column>((ca, cc) => new object[] { JoinType.Inner, ca.ColumnId == cc.Id })
                  .Where(where)
                  .Select((ca, cc) => new ArticleOutput
                  {
                      Title = ca.Title,
                      Id = ca.Id,
                      ColumnId = ca.ColumnId,
                      Summary = ca.Summary,
                      EnTitle = cc.EnTitle,
                      Author = ca.Author,
                      Source = ca.Source,
                      Tag = ca.Tag,
                      CreateTime = ca.CreateTime,
                      Content = ca.Content,
                      ThumImg = ca.ThumImg,
                      ColumnName = cc.Title,
                      KeyWord = ca.KeyWord,
                      ParentColumnUrl = SqlFunc.Subqueryable<Column>().Where(s => s.Id == cc.ParentId).Select(s => s.EnTitle)
                  })
                  .FirstAsync();
        }
        public async Task<ArticleOutput> GetUpArticleAsync(int id, int columnId)
        {
            return await Db.Queryable<Article, Column>((ca, cc) => new object[] { JoinType.Inner, ca.ColumnId == cc.Id })
                .Where((ca, cc) => ca.Id > id && ca.ColumnId == columnId)
                .OrderBy((ca, cc) => ca.Id, OrderByType.Asc)
                .Select((ca, cc) => new ArticleOutput
                {
                    Title = ca.Title,
                    Id = ca.Id,
                    ColumnId = ca.ColumnId,
                    EnTitle = cc.EnTitle,
                    ParentColumnUrl = SqlFunc.Subqueryable<Column>().Where(s => s.Id == cc.ParentId).Select(s => s.EnTitle)
                }).FirstAsync();
        }
        public async Task<ArticleOutput> GetNextOrUpArticleAsync(Expression<Func<Article,Column,bool>> expression)
        {
            //(ca, cc) => ca.Id < id && ca.ColumnId == columnId
            return await Db.Queryable<Article, Column>((ca, cc) => new object[] { JoinType.Inner, ca.ColumnId == cc.Id })
                .Where(expression)
                .OrderBy((ca, cc) => ca.Id, OrderByType.Desc)
                .Select((ca, cc) => new ArticleOutput
                {
                    Title = ca.Title,
                    Id = ca.Id,
                    ColumnId = ca.ColumnId,
                    EnTitle = cc.EnTitle,
                    ParentColumnUrl = SqlFunc.Subqueryable<Column>().Where(s => s.Id == cc.ParentId).Select(s => s.EnTitle)
                })
                .FirstAsync();
        }
    }   
}