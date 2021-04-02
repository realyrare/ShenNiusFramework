using ShenNius.Share.Models.Dtos.Output.Cms;
using ShenNius.Share.Models.Entity.Cms;
using ShenNius.Share.Service.Repository;
using ShenNius.Share.Service.Repository.Extensions;
using SqlSugar;
using System;
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

namespace ShenNius.Share.Service.Cms
{
    public interface IArticleService : IBaseServer<Article>
    {
       Task<Page<ArticleOutput>> GetArtcileByCondition(Expression<Func<Article, Column, bool>> where, int pageIndex, int pageSize);
    }
    public class ArticleService : BaseServer<Article>, IArticleService
    {
        public async Task<Page<ArticleOutput>> GetArtcileByCondition(Expression<Func<Article, Column, bool>> where, int pageIndex, int pageSize)
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
    }

   
}