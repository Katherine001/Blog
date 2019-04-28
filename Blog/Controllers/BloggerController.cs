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
    public class BloggerController:ControllerBase
    {
        private readonly MySQLDbContext _mySqlDbContext;

        public BloggerController(MySQLDbContext mySqlDbContext)
        {
            _mySqlDbContext = mySqlDbContext;
        }
        
        /// <summary>
        /// 获取所有博客
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Blog>>> getBlogs()
        {
            return await _mySqlDbContext.Blog.ToListAsync();
        }
        /// <summary>
        /// 根据作者id获取博客
        /// </summary>
        /// <param name="a_uid"></param>
        /// <returns></returns>
        [HttpGet("uid/{a_uid}")]
        public async Task<ActionResult<IEnumerable<Models.Blog>>> getBlog(int a_uid)
        {
            return await _mySqlDbContext.Blog.FromSql($"select * from blog where a_uid={a_uid}").ToListAsync();
        }
        
        /// <summary>
        /// 根据作者id添加博客
        /// </summary>
        /// <param name="a_uid"></param>
        /// <param name="blog"></param>
        /// <returns></returns>
        [HttpPost("uid/{a_uid}")]
        public async Task<ActionResult<Models.Blog>> postByUid(int a_uid,Models.Blog blog)
        {
            blog.a_uid = a_uid;
            _mySqlDbContext.Blog.Add(blog);
            await _mySqlDbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(getBlogs), new {a_id = blog.a_id});
        }

        /// <summary>
        /// 根据博客id获取博客
        /// </summary>
        /// <param name="a_id"></param>
        /// <returns></returns>
        [HttpGet("{a_id}")]
        public async Task<ActionResult<Models.Blog>> getByAid(int a_id)
        {
            var blog = await _mySqlDbContext.Blog.FindAsync(a_id);
            if (blog == null)
            {
                return NotFound();
            }

            return blog;
        }

        /// <summary>
        /// 根据博客id删除博客
        /// </summary>
        /// <param name="a_id"></param>
        /// <returns></returns>
        [HttpDelete("{a_id}")]
        public async Task<IActionResult> deleteBlog(int a_id)
        {
            var blog = await _mySqlDbContext.Blog.FindAsync(a_id);
            var remark = await _mySqlDbContext.Breview.FromSql($"select * from breview where p_aid={a_id}")
                .ToListAsync();
            if (blog == null)
            {
                return NotFound();
            }
            foreach (var VARIABLE in remark)
            {
                _mySqlDbContext.Breview.Remove(VARIABLE);
                await _mySqlDbContext.SaveChangesAsync();
            }
            _mySqlDbContext.Blog.Remove(blog);
            await _mySqlDbContext.SaveChangesAsync();
            return NoContent();
        }
        
        /// <summary>
        /// 根据博客id修改博客内容
        /// </summary>
        /// <param name="a_id"></param>
        /// <param name="blog"></param>
        /// <returns></returns>
        [HttpPut("{a_id}")]
        public async Task<IActionResult> editBlog(int a_id, Models.Blog blog)
        {
            if (a_id != blog.a_id)
            {
                return BadRequest();
            }

            _mySqlDbContext.Entry(blog).State = EntityState.Modified;
            await _mySqlDbContext.SaveChangesAsync();
            return NoContent();
        }
        /// <summary>
        /// 根据博客id获取博客所有评论
        /// </summary>
        /// <param name="p_aid"></param>
        /// <returns></returns>
        [HttpGet("pl/{p_aid}")]
        public async Task<ActionResult<IEnumerable<Models.Breview>>> getRemark(int p_aid)
        {
            return await _mySqlDbContext.Breview.FromSql($"select * from breview where p_aid={p_aid}").ToListAsync();
        }
    }
}