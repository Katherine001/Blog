using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowSpecificOrigin")]
    public class ArticleController
    {
        private readonly MySQLDbContext _mySqlDbContext;

        public ArticleController(MySQLDbContext mySqlDbContext)
        {
            _mySqlDbContext = mySqlDbContext;
        }
        /// <summary>
        /// 获取所有文章标签
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Btag>>> getTags()
        {
            return await _mySqlDbContext.Btag.ToListAsync();
        }
    }
}